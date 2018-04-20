using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueClientAPI.API
{
    internal static class Login
    {
        [Serializable]
        public class Session
        {
            public UInt64 accountId;
            public Boolean connected;
            public String username;
            public UInt64 summonerId;
        }

        static readonly APIRequest _apiRequest = APIRequest.Instance;


        public static bool GetSession(out Session session)
        {
            String response;
            session = _apiRequest.Get("/lol-login/v1/session", out response)
                ? JsonHandler.LoadJson<Session>(response)
                : null;
            return session != null;
        }
    }
}
