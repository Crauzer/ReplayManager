using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LeagueClientAPI.API
{
    internal static class Patcher
    {
        [DataContract] 
        public class Solutions
        {
            [DataMember(IsRequired = false)]
            public String league_client_sln;
            [DataMember(IsRequired = false)]
            public String lol_game_client_sln;
        }

        static readonly APIRequest _apiRequest = APIRequest.Instance;
        
        public static bool GetGamePaths(out Solutions solutions)
        {
            String response;
            solutions = _apiRequest.Get("/patcher/v1/products/league_of_legends/paths", out response)
                ? JsonHandler.LoadJson<Solutions>(response)
                : null;
            return solutions != null;
        }
    }
}
