using ECommerceAPI.API.Configurations.ColumnWriters;
using ECommerceAPI.API.Extensions;
using ECommerceAPI.Application;
using ECommerceAPI.Application.Validators.Products;
using ECommerceAPI.Infrastructure;
using ECommerceAPI.Infrastructure.Filters;
using ECommerceAPI.Infrastructure.Services.Storage.Azure;
using ECommerceAPI.Infrastructure.Services.Storage.Local;
using ECommerceAPI.Persistance;
using ECommerceAPI.SignalR;
using ECommerceAPI.SignalR.Hubs;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using NpgsqlTypes;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials()
)) ;
builder.Services.AddHttpContextAccessor(); //clientten gelen req neticesinde oluþturulan httpcontext nesnesine katmanlardaki classlar üzerinden eriþmemizi saðlar
builder.Services.AddPersistanceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddSignalRServices();


//builder.Services.AddStorage<LocalStorage>();
builder.Services.AddStorage<AzureStorage>();
//builder.Services.AddStorage(ECommerceAPI.Infrastructure.Enums.StorageType.Azure);
//builder.Services.AddStorage(StorageType.Azure);

Logger log = new LoggerConfiguration()
    .WriteTo.File("logs/logs.txt")
    .WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PostgreSQL"), "logs",
    columnOptions: new Dictionary<string, ColumnWriterBase>
    {
        {"message",new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
        {"message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
        {"level",new LevelColumnWriter(true,NpgsqlDbType.Varchar) },
        {"time_stamp" , new TimestampColumnWriter(NpgsqlDbType.Timestamp)},
        {"exception",new ExceptionColumnWriter(NpgsqlDbType.Text) },
        {"log_event",new LogEventSerializedColumnWriter(NpgsqlDbType.Json) },
        {"user_name",new UsernameColumnWriter() }
    }, needAutoCreateTable: true)
    .Enrich.FromLogContext() //---> harici context istiyorsak yapmalýyýz
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog(log);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.ResponseHeaders.Add("MyResponseHeader");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
    logging.CombineLogs = true;
});


builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>()).AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>()).ConfigureApiBehaviorOptions(
    opt =>
    {
        opt.SuppressModelStateInvalidFilter = true;
    });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer("Admin", options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,




        ValidIssuer = builder.Configuration["Token:Issuer"],
        ValidAudience = builder.Configuration["Token:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,
        NameClaimType = ClaimTypes.Name, // jwtde name claim --> User.Identity.Name        
    };



});



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>());
app.UseStaticFiles();

app.UseSerilogRequestLogging();
app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    string username = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null; //user n? identity? isAuth? truemi--> content.User.Identity döndür , eðer false veya null ise null döndür
    LogContext.PushProperty("user_name", username); //UsernameColumnWriterdeki propertiese username'i pushladýk. 
    await next();
});

app.MapControllers();
app.MapHubs();
app.Run();
