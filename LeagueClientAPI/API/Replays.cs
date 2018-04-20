using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueClientAPI.API
{
    internal static class Replays
    {
        [Serializable]
        public class Configuration
        {
            public String gameVersion;
            public Boolean isLoggedIn;
        }

        static readonly APIRequest _apiRequest = APIRequest.Instance;

        public static bool GetDefaultReplayPath(out String response)
        {
            if (_apiRequest.Get("/lol-replays/v1/rofls/path/default", out response))
            {
                response = response.Replace("\"", "");
                return true;
            }

            return false;
        }

        public static bool GetReplayPath(out String response)
        {
            if (_apiRequest.Get("/lol-replays/v1/rofls/path/", out response))
            {
                response = response.Replace("\"", "");
                return true;
            }

            return false;
        }

        public static bool GetReplayConfiguration(out Configuration config)
        {
            String response;
            config = _apiRequest.Get("/lol-replays/v1/configuration", out response)
                ? JsonHandler.LoadJson<Configuration>(response)
                : null;
            return config != null;
        }
    }
}
