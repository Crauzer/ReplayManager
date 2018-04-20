using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueClientAPI.Images
{
    internal static class Utility
    {
        public static Bitmap LoadImage(Byte[] image)
        {
            using (var ms = new MemoryStream(image))
            {
                return new Bitmap(ms);
            }
        }
    }
}
