using System;
using System.Configuration;
using System.Drawing;
using System.Threading;
using LeagueClientAPI.ACS;
using LeagueClientAPI.ACS.JSON;
using LeagueClientAPI.API;
using LeagueClientAPI.RemoteCalls;
using LeagueClientAPI.Images;

namespace LeagueClientAPI
{
    public class LeagueClient : IDisposable
    {
        public delegate void OnError(String ErrorMessage);
        public delegate void OnLogin();
        public delegate void OnLeagueClientClosed();

        private event OnError OnErrorCallback;
        private event OnLogin OnLoginCallback;
        private event OnLeagueClientClosed OnLeagueClientClosedCallback;   

        private readonly MemoryEditor _leagueClientMemoryEditor = MemoryEditor.Instance;
        private RemoteLoLReplays _remoteLoLReplays;

        private Thread _scanningThread;
        private bool running;

        public LeagueClient(OnError OnErrorCallback, OnLogin OnLoginCallback, OnLeagueClientClosed OnLeagueClientClosedCallback)
        {
            this.OnErrorCallback = OnErrorCallback;
            this.OnLoginCallback = OnLoginCallback;
            this.OnLeagueClientClosedCallback = OnLeagueClientClosedCallback;

            running = true;
            _scanningThread = new Thread(ScanForGame){ IsBackground = true };
            _scanningThread.Start();
        }

        private void ScanForGame()
        {
            while (running)
            {
                Thread.Sleep(1000);
                String error;
                //Find LeagueClient:
                FindGameResult findGameResult = _leagueClientMemoryEditor.FindGame("LeagueClientUx", out error, false);
                if (findGameResult != FindGameResult.GameFound)
                {
                    OnErrorCallback("League Client Not Open");
                    continue;
                }

                //Get Command Line for Password & Port
                String Password = "";
                Int32 Port = 0;
                String commandLine = _leagueClientMemoryEditor.GetCommandLine();
                String[] commands = commandLine.Split(' ');
                foreach (String command in commands)
                {
                    if (command.Contains("--remoting-auth-token="))
                        Password = command.Replace("\"", "").Replace("--remoting-auth-token=", "");
                    else if (command.Contains("--app-port="))
                        Port = Convert.ToInt32(command.Replace("\"", "").Replace("--app-port=", ""));
                }

                //Check to see if the user is logged in
                if (!APIRequest.Instance.Setup("riot", Password, Port))
                {
                    OnErrorCallback("Not Logged In");
                    continue;
                }

                //Find the LeagueClient for remote calling
                findGameResult = _leagueClientMemoryEditor.FindGame("LeagueClient", out error, false);
                _leagueClientMemoryEditor.ProcessExitedHandler += OnProcessClosed;
                if (findGameResult != FindGameResult.GameFound) continue;

                String region;
                if (PlatformConfig.GetConfigSetting("LoginDataPacket", "platformId", out region))
                {
                    _remoteLoLReplays = new RemoteLoLReplays(region);
                    OnLoginCallback();
                    running = false;
                }
                else
                {
                    //Actually check to see if they're logged in
                    /*Login.Session session;
                    if(!Login.GetSession(out session))
                        OnErrorCallback("Error Requesting PlatformID");*/
                    OnErrorCallback("Logging In");
                }
            }
        }

        public String GetReplayDownloadUrl(UInt64 matchId)
        {
            if (_remoteLoLReplays != null)
                if (_remoteLoLReplays.Enabled)
                    return _remoteLoLReplays.fetchRoflDownloadUrl(matchId);

            return "";
        }

        public ACSMatchList GetMatchHistory(int startIndex = 0, int endIndex = 20)
        {
            return MatchList.GetMatchList(startIndex, endIndex);
        }

        public Bitmap GetSummonerSpellIcon(int summonerSpellId)
        {
            return SummonerSpells.GetSummonerSpellIcon(summonerSpellId);
        }

        public Bitmap GetChampionIcon(int championId)
        {
            return Champions.GetChampionIcon(championId);
        }

        public Bitmap GetItemIcon(int itemId)
        {
            if (itemId == 0)
                return null;

            return Items.GetItemIcon(itemId);
        }

        public String GetShareableMatchHistoryURL(UInt64 matchId)
        {
            String url = null;
            if (!MatchHistory.GetMatchDetailsUrlTemplate(out url))
                return url;

            RiotClient.RegionLocale locale;
            if (!RiotClient.GetRegionLocale(out locale))
                return url;

            Login.Session session;
            if (!Login.GetSession(out session))
                return url;

            return String.Format(url, matchId, session.accountId, locale.webLanguage);
        }

        public bool GetDefaultReplayFolder(out string folder)
        {
            return Replays.GetDefaultReplayPath(out folder);
        }

        public bool GetReplayFolder(out string folder)
        {
            return Replays.GetReplayPath(out folder);
        }

        public bool GetGameVersion(out string version)
        {
            Replays.Configuration config;
            bool result = Replays.GetReplayConfiguration(out config);
            version = config.gameVersion;
            return result;
        }

        private void OnProcessClosed(object sender, EventArgs e)
        {
            OnLeagueClientClosedCallback();

            if (!_scanningThread.IsAlive)
            {
                running = true;
                _scanningThread = new Thread(ScanForGame) {IsBackground = true};
                _scanningThread.Start();
            }
        }

        public String GetLeagueOfLegendsExecutableLocation()
        {
            Patcher.Solutions sol;
            if(Patcher.GetGamePaths(out sol))
            {
                return sol.lol_game_client_sln;
            }
            return "";
        }

        internal String GetIDToken()
        {
            return APIRequest.Instance.IDToken;
        }

        public void Dispose()
        {
            running = false;
            try
            {
                if(_scanningThread.IsAlive)
                    _scanningThread.Abort();
            }
            catch { }
        }
    }
}
