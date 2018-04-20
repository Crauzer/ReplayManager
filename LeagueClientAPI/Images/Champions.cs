using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueClientAPI.Images
{
    internal static class Champions
    {
        static readonly APIRequest _apiRequest = APIRequest.Instance;

        public static Bitmap GetChampionIcon(int championId)
        {
            byte[] image;
            if (_apiRequest.Get(String.Format("/lol-game-data/assets/v1/champion-icons/{0}.png", championId), out image))
            {
                return Utility.LoadImage(image);
            }

            return PlaceholderIcon.Image;
        }

        public static Bitmap GetChampionSplash(int championId, int skinId)
        {
            byte[] image;
            if (_apiRequest.Get(String.Format("/lol-game-data/assets/v1/champion-splashes/{0}/{0}{1}.jpg", championId, skinId.ToString("D3")), out image))
            {
                return Utility.LoadImage(image);
            }

            return PlaceholderIcon.Image;
        }

        public static Bitmap GetChampionSplashUncentered(int championId, int skinId)
        {
            byte[] image;
            if (_apiRequest.Get(String.Format("/lol-game-data/assets/v1/champion-splashes/uncentered/{0}/{0}{1}.jpg", championId, skinId.ToString("D3")), out image))
            {
                return Utility.LoadImage(image);
            }

            return PlaceholderIcon.Image;
        }
    }
}
