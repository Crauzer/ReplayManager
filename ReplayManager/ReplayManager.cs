using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LeagueClientAPI;
using LeagueClientAPI.ACS.JSON;
using MetroFramework.Forms;
using ReplayManager.Controls;
using ReplayManager.ROFL;
using PlayerStats = ReplayManager.ROFL.PlayerStats;

namespace ReplayManager
{
    public partial class ReplayManager : MetroForm
    {
        private readonly LeagueClient _leagueClient;

        private class ReplayLocation
        {
            public string replayLocation { private set; get; }

            public ReplayLocation(string location)
            {
                replayLocation = location;
            }
            public override string ToString()
            {
                return Path.GetFileNameWithoutExtension(replayLocation);
            }
        }

        public ReplayManager()
        {
            InitializeComponent();
            //Remove the tab after intializing it
            metroTabControl.TabPages.Remove(tabMatchHistory);
            _leagueClient = new LeagueClient(OnError, OnLeagueClientLoggedIn, OnLeagueClientClosed);
        }

        private string GetDownloadUrl(UInt64 matchId)
        {
            return _leagueClient.GetReplayDownloadUrl(matchId);
        }

        private void FirstTimeSetup()
        {
            String defaultPath;
            String path;
            if (_leagueClient.GetDefaultReplayFolder(out defaultPath) && _leagueClient.GetReplayFolder(out path))
            {
                if (defaultPath != path)
                    lstReplayFolders.Items.Add(defaultPath);

                lstReplayFolders.Items.Add(path);
            }
            UpdateReplayList();

            //Todo: Cut Down?
            //EG: E:/Games/Riot Games/League of Legends/RADS/solutions/lol_game_client_sln/releases/0.0.1.198/deploy/
            //Cut off version number + deploy?
            txtGameLocation.Text = _leagueClient.GetLeagueOfLegendsExecutableLocation();
        }

        private void OnError(String ErrorMessage)
        {
            Invoke(new MethodInvoker(delegate
                {
                    lblGameVersion.Text = ErrorMessage;
                }));
        }

        private void OnLeagueClientLoggedIn()
        {
            Invoke(new MethodInvoker(FirstTimeSetup));
            Invoke(new MethodInvoker(delegate
            {
                //TODO: Actually get this from the league of legends.exe, won't be dependant on the LCU and potential mismatching?
                String version;
                if( _leagueClient.GetGameVersion(out version))
                    lblGameVersion.Text = "Game Version: " + version;
                metroTabControl.TabPages.Insert(1, tabMatchHistory);
            }));
        }

        private void UpdateReplayList()
        {
            Invoke(new MethodInvoker(delegate
            {
                lstReplays.Items.Clear();
                foreach (string folder in lstReplayFolders.Items)
                {
                    string[] replayFiles = Directory.GetFiles(folder, "*.rofl");
                    foreach (string replay in replayFiles)
                    {
                        lstReplays.Items.Add(new ReplayLocation(replay));
                    }
                }
            }));
        }

        private void OnLeagueClientClosed()
        {
            //TODO: Panels?
            //Hide/Disable Functionality
            Invoke(new MethodInvoker(delegate
            {
                metroTabControl.TabPages.Remove(tabMatchHistory);
            }));
        }

        private void btnGetUrl_Click(object sender, EventArgs e)
        {
            txtDownloadUrl.Text = _leagueClient.GetReplayDownloadUrl(Convert.ToUInt64(txtMatchId.Text));
        }

        private void lstReplays_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstReplays.SelectedItem == null)
                return;

            PopulateReplayData(lstReplays.SelectedIndex);
        }

        private void PopulateReplayData(int index)
        {
            txtBlueStats.Text = "";
            txtRedStats.Text = "";

            string file = ((ReplayLocation) lstReplays.Items[index]).replayLocation;
            Replay roflReplay = new Replay(file);
            if (!roflReplay.Initialized || roflReplay.Header.Metadata.statsJson == null)
                return;
            List<string> BlueStats = new List<string>();
            List<string> RedStats = new List<string>();

            foreach (PlayerStats stats in roflReplay.Header.Metadata.playerStats)
            {
                if (stats.TEAM == 100)
                {
                    if (stats.SKINID != -1)
                        BlueStats.Add(string.Concat(new object[] { stats.NAME + "     Skin ID: " + stats.SKINID }));
                    else
                    {
                        BlueStats.Add(string.Concat(new object[] { stats.NAME }));
                    }
                    BlueStats.Add(
                        string.Concat(new object[] { stats.SKIN + " - " + stats.CHAMPIONS_KILLED + "/" + stats.NUM_DEATHS + "/" + stats.ASSISTS }));
                    BlueStats.Add(string.Concat(new object[] { "Minion Kills: " + stats.MINIONS_KILLED }));
                    BlueStats.Add(string.Concat(new object[] { }));
                }
                else
                {
                    if (stats.SKINID != -1)
                        RedStats.Add(string.Concat(new object[] { stats.NAME + "     Skin ID: " + stats.SKINID }));
                    else
                    {
                        RedStats.Add(string.Concat(new object[] { stats.NAME }));
                    }
                    RedStats.Add(
                        string.Concat(new object[] { stats.SKIN + " - " + stats.CHAMPIONS_KILLED + "/" + stats.NUM_DEATHS + "/" + stats.ASSISTS }));
                    RedStats.Add(string.Concat(new object[] { "Minion Kills: " + stats.MINIONS_KILLED }));
                    RedStats.Add(string.Concat(new object[] { }));
                }
            }

            TimeSpan timeSpan = TimeSpan.FromMilliseconds(roflReplay.Header.Metadata.gameLength);
            lblGameData.Text = string.Format("Game Length: {0:D2}:{1:D2}           Replay Version: {2}", (int)timeSpan.TotalMinutes, timeSpan.Seconds, roflReplay.Header.Metadata.gameVersion);

            txtBlueStats.Lines = BlueStats.ToArray();
            txtRedStats.Lines = RedStats.ToArray();
        }

        private void btnPlayReplay_Click(object sender, EventArgs e)
        {
            String gameExe = txtGameLocation.Text + "League of Legends.exe";
            string file = ((ReplayLocation)lstReplays.Items[lstReplays.SelectedIndex]).replayLocation;
            ProcessStartInfo processInfo = new ProcessStartInfo(gameExe, String.Format("\"{0}\"", file))
            {
                CreateNoWindow = true,
                UseShellExecute = true
            };
            processInfo.WorkingDirectory = Path.GetDirectoryName(gameExe);
            Process.Start(processInfo);
        }

        private void tileRefresh_Click(object sender, EventArgs e)
        {
            tileRefresh.Enabled = false;
            //Get Match History
            List<GameData> gameDataList = new List<GameData>();
            int matchIndex = 0;
            //TODO: Not hit the endpoint so hard and spawn it in a new Thread or background worker
            while (true)
            {
                bool breakout = false;
                ACSMatchList MatchHistory = _leagueClient.GetMatchHistory(matchIndex * 20, (matchIndex + 1) * 20);
                foreach (GameData gameData in MatchHistory.games.games)
                {
                    if ((DateTime.Now - FromUnixTime(Convert.ToInt64(gameData.gameCreation))).Days > 30)
                    {
                        breakout = true;
                        break;
                    }

                    gameDataList.Add(gameData);
                }

                if (breakout)
                    break;

                matchIndex++;
            }
            //Clear the Current Panel
            panelMatchHistory.Controls.Clear();

            //Generate The UI
            //MatchHistory List is in reverse, oldest to newest
            //So Panel is set to show in reverse
            foreach (GameData gameData in gameDataList.OrderBy(g => g.gameCreation))
            {
                Application.DoEvents();
                MatchInfo matchInfoControl = new MatchInfo(_leagueClient, gameData.participants[0].championId, gameData.participants[0].spell1Id, gameData.participants[0].spell2Id,
                    gameData.participants[0].stats.item0, gameData.participants[0].stats.item1, gameData.participants[0].stats.item2, gameData.participants[0].stats.item3, gameData.participants[0].stats.item4, gameData.participants[0].stats.item5, gameData.participants[0].stats.item6,
                    gameData.participants[0].stats.kills, gameData.participants[0].stats.deaths, gameData.participants[0].stats.assists, _leagueClient.GetShareableMatchHistoryURL(gameData.gameId), gameData.gameId, GetDownloadUrl, FromUnixTime(Convert.ToInt64(gameData.gameCreation)), gameData.gameDuration);

                //Add it to the panel
                panelMatchHistory.Controls.Add(matchInfoControl);
            }

            tileRefresh.Enabled = true;
        }

        //TODO: Move to Static Utility Class
        public static DateTime FromUnixTime(long unixTime)
        {
            return epoch.AddMilliseconds(unixTime);
        }
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }
}
