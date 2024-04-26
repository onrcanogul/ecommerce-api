using ECommerceAPI.Application.Abstractions.Services.Configurations;
using ECommerceAPI.Application.CustomAttributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes ="Admin")]
    public class ApplicationServicesController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        
        public ApplicationServicesController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        [AuthorizeDefinition(Definition ="Get All Authorize Definitions" , Menu = "Authorize Definition" , Action= Application.Enums.ActionType.Reading)]
        public IActionResult GetAuthorizeDefinitionEndpoints()
        {
            var datas = _applicationService.GetAuthorizeDefinitionEndpoints(typeof(Program));
            return Ok(datas);
        }
    }
}
