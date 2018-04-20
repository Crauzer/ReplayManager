using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueClientAPI.API
{
    internal static class MatchHistory
    {
        static readonly APIRequest _apiRequest = APIRequest.Instance;

        public static bool GetMatchHistoryUrl(out String response)
        {
            if (_apiRequest.Get("/lol-match-history/v1/web-url", out response))
            {
                response = response.Replace("\"", "");
                return true;
            }

            return false;
        }

        //Return https://matchhistory.leagueoflegends.com/{2}/#match-details/EUW1/{0}/{1}
        //0 = Match ID
        //1 = Account ID
        //2 = Language from the Region Locale in RiotClient
        public static bool GetMatchDetailsUrlTemplate(out String template)
        {
            return PlatformConfig.GetConfigSetting("ShareMatchHistory", "MatchDetailsUrlTemplate", out template);
        }
    }
}
