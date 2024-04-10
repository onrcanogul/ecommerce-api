using ECommerceAPI.Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ECommerceAPI.Infrastructure.Services
{
    public class FileService
    {

        private async Task<string> FileRenameAsync(string path, string fileName, int num = 0)
        {
            string newFileName = await Task.Run<string>(async () =>
            {

                string newFileName = String.Empty;
                string extension = Path.GetExtension(fileName);
                if (num == 0)
                {
                    string oldName = Path.GetFileNameWithoutExtension(fileName);
                    newFileName = NameOperation.CharacterRegulatory(oldName) + extension;
                }
                else
                {
                    newFileName = fileName;
                }

                if (File.Exists($"{path}\\{newFileName}"))
                {
                   return await FileRenameAsync(path, $"{Path.GetFileNameWithoutExtension(newFileName)}-{num}{extension}", ++num);
                }
                return newFileName;
            });
            return newFileName;
        }   

        


            
            
        }
   }

