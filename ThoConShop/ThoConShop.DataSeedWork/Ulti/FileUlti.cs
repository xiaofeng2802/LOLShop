using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ThoConShop.DataSeedWork.Ulti
{
    public class FileUlti
    {
        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static string SaveFile(HttpPostedFileBase file, string path, string nameFile = "")
        {
            if (file != null)
            {
                string pic = Guid.NewGuid() + Path.GetExtension(file.FileName);

                if (!string.IsNullOrEmpty(nameFile))
                {
                    pic = nameFile + Path.GetExtension(file.FileName);
                }
               
                string fullPath = System.IO.Path.Combine(path, pic);
                // file is uploaded
                file.SaveAs(fullPath);
                return fullPath;;
            }
            return null;
        }
    }
}
