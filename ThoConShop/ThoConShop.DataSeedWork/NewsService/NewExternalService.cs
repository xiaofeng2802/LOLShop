using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThoConShop.DataSeedWork.NewsService
{
    public class NewExternalService
    {

        public static void SaveNew(string path, string data)
        {
            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                TextWriter writer = new StreamWriter(stream);

                writer.Write(data.Remove(data.Length - 3));
                writer.Close();
            }
        }

        public static string ReadNew(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read))
            {
                TextReader reader = new StreamReader(stream);

                return reader.ReadToEnd();
            }
        }
    }
}
