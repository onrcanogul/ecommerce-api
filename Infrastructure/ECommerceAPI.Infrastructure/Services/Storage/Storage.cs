using ECommerceAPI.Infrastructure.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Services.Storage
{
    public class Storage
    {
        protected delegate bool HasFile(string pathOrContainerName, string fileName);
        protected async Task<string> FileRenameAsync(string pathOrContainerName, string fileName,HasFile hasFileMethod ,int num = 0)
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

                //if (File.Exists($"{path}\\{newFileName}"))
                if (hasFileMethod(pathOrContainerName, newFileName))
                {
                    return await FileRenameAsync(pathOrContainerName, $"{Path.GetFileNameWithoutExtension(newFileName)}-{num}{extension}",hasFileMethod,++num);
                }
                return newFileName;
            });
            return newFileName;
        }
    }
}
