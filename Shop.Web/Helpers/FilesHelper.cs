using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Helpers
{
    using System.IO;

    public class FilesHelper
    {
        public static bool UploadPhoto(MemoryStream stream, string folder, string name)
        {
            try
            {
                stream.Position = 0;
                var path = Path.Combine(Directory.GetCurrentDirectory(), folder, name);
                File.WriteAllBytes(path, stream.ToArray());
            }
            catch
            {
                return false;
            }

            return true;
        }
    }

}
