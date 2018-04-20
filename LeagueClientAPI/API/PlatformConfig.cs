using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueClientAPI.API
{
    internal static class PlatformConfig
    {
        static readonly APIRequest _apiRequest = APIRequest.Instance;

        public static bool GetConfigSetting(String NameSpace, String Setting, out String response)
        {
            if (_apiRequest.Get(String.Format("/lol-platform-config/v1/namespaces/{0}/{1}", NameSpace, Setting),
                out response))
            {
                response = response.Replace("\"", "");
                return true;
            }

            return false;
        }
    }
}
