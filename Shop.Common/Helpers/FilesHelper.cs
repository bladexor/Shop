using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Common.Helpers
{
    using System.IO;

    public class FilesHelper
    {
        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }

}
