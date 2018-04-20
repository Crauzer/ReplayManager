using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueClientAPI.API
{
    internal static class RiotClient
    {
        [Serializable]
        public class RegionLocale
        {
            public String locale;
            public String region;
            public String webLanguage;
            public String webRegion;
        }

        static readonly APIRequest _apiRequest = APIRequest.Instance;

        public static bool GetRegionLocale(out RegionLocale locale)
        {
            String response;
            locale = _apiRequest.Get("/riotclient/get_region_locale", out response)
                ? JsonHandler.LoadJson<RegionLocale>(response)
                : null;
            return locale != null;
        }

        public static bool GetPlatformId(out String response)
        {
            if (_apiRequest.Get("/riotclient/v1/bugsplat/platform-id", out response))
            {
                response = response.Replace("\"", "");
                return true;
            }

            return false;
        }
    }
}
