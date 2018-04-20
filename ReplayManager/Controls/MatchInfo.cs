using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LeagueClientAPI;

namespace ReplayManager.Controls
{
    public partial class MatchInfo : UserControl
    {
        readonly String SharableWebUrl;
        private readonly Func<UInt64, String> DownloadReplayCall;
        private UInt64 matchId;
        public MatchInfo(LeagueClient _leagueClient, int champId, int spell1, int spell2, int item1,
            int item2, int item3, int item4, int item5, int item6, int item7, int kill, int deaths, int assists, string webUrl,
            UInt64 matchId, Func<UInt64, String> DownloadReplayCall, DateTime matchDate, Int32 gameLength)
        {
            InitializeComponent();
            this.DownloadReplayCall = DownloadReplayCall;
            this.matchId = matchId;
            picChampion.Image = _leagueClient.GetChampionIcon(champId);
            picSummonerSpell1.Image = _leagueClient.GetSummonerSpellIcon(spell1);
            picSummonerSpell2.Image = _leagueClient.GetSummonerSpellIcon(spell2);
            picItem1.Image = _leagueClient.GetItemIcon(item1);
            picItem2.Image = _leagueClient.GetItemIcon(item2);
            picItem3.Image = _leagueClient.GetItemIcon(item3);
            picItem4.Image = _leagueClient.GetItemIcon(item4);
            picItem5.Image = _leagueClient.GetItemIcon(item5);
            picItem6.Image = _leagueClient.GetItemIcon(item6);
            picItem7.Image = _leagueClient.GetItemIcon(item7);
            lblMatchDate.Text = matchDate.ToShortDateString();
            TimeSpan t = TimeSpan.FromSeconds(gameLength);
            lblGameLength.Text = string.Format("{0:D2}m:{1:D2}s", t.Minutes, t.Seconds);
            SharableWebUrl = webUrl;
            if (String.IsNullOrEmpty(SharableWebUrl))
                btnOpenMatchHistoryWebpage.Visible = false;
        }

        private void btnOpenMatchHistoryWebpage_Click(object sender, EventArgs e)
        {
            Process.Start(SharableWebUrl);
        }

        private void btnStoreDownloadToClipboard_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(DownloadReplayCall(matchId));
        }
    }
}
