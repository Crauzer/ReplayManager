using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueClientAPI.API;
using System.Net;
using System.Net.Cache;
using LeagueClientAPI.ACS.JSON;

namespace LeagueClientAPI.ACS
{
    internal static class MatchList
    {

        public static ACSMatchList GetMatchList(int startIndex, int endIndex)
        {
            String history = "";
            String bearerToken = APIRequest.Instance.IDToken;
            Login.Session session;

            if(!Login.GetSession(out session))
                return null;

            MatchHistory.GetMatchHistoryUrl(out history);

            String acsUrl;
            PlatformConfig.GetConfigSetting("LCUACS", "Endpoint", out acsUrl);
            String region;
            PlatformConfig.GetConfigSetting("LoginDataPacket", "platformId", out region);

            String url = String.Format("{0}/v1/stats/player_history/{1}/{2}?begIndex={3}&endIndex={4}",
                acsUrl, region, session.accountId, startIndex, endIndex);

            using (WebClient webClient = new WebClient())
            {
                webClient.Proxy = null; //Because otherwise downloads are slow
                webClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)");
                webClient.Headers.Add("Bearer", bearerToken);
                webClient.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                using (MemoryStream stream = new MemoryStream(webClient.DownloadData(url)))
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        history = reader.ReadToEnd();
                    }
                }
            }
            ACSMatchList matchHistoryList = JsonHandler.LoadJson<ACSMatchList>(history);
            return matchHistoryList;
        }
    }
}
