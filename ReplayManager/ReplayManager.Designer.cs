namespace ReplayManager
{
    partial class ReplayManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.metroTabControl = new MetroFramework.Controls.MetroTabControl();
            this.tabPageReplays = new MetroFramework.Controls.MetroTabPage();
            this.btnPlayReplay = new MetroFramework.Controls.MetroButton();
            this.lblGameData = new MetroFramework.Controls.MetroLabel();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.txtRedStats = new MetroFramework.Controls.MetroTextBox();
            this.txtBlueStats = new MetroFramework.Controls.MetroTextBox();
            this.lstReplays = new System.Windows.Forms.ListBox();
            this.label2 = new MetroFramework.Controls.MetroLabel();
            this.label1 = new MetroFramework.Controls.MetroLabel();
            this.btnGetUrl = new MetroFramework.Controls.MetroButton();
            this.txtDownloadUrl = new MetroFramework.Controls.MetroTextBox();
            this.txtMatchId = new MetroFramework.Controls.MetroTextBox();
            this.tabMatchHistory = new MetroFramework.Controls.MetroTabPage();
            this.panelMatchHistory = new MetroFramework.Controls.MetroFlowLayoutPanel();
            this.tileRefresh = new MetroFramework.Controls.MetroTile();
            this.tabPageSettings = new MetroFramework.Controls.MetroTabPage();
            this.txtGameLocation = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.lstReplayFolders = new System.Windows.Forms.ListBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.metroStyleExtender = new MetroFramework.Components.MetroStyleExtender(this.components);
            this.lblGameVersion = new MetroFramework.Controls.MetroLabel();
            this.metroTabControl.SuspendLayout();
            this.tabPageReplays.SuspendLayout();
            this.tabMatchHistory.SuspendLayout();
            this.tabPageSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
            this.SuspendLayout();
            // 
            // metroTabControl
            // 
            this.metroTabControl.Controls.Add(this.tabPageReplays);
            this.metroTabControl.Controls.Add(this.tabMatchHistory);
            this.metroTabControl.Controls.Add(this.tabPageSettings);
            this.metroTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabControl.FontWeight = MetroFramework.MetroTabControlWeight.Bold;
            this.metroTabControl.Location = new System.Drawing.Point(20, 60);
            this.metroTabControl.Name = "metroTabControl";
            this.metroTabControl.SelectedIndex = 1;
            this.metroTabControl.Size = new System.Drawing.Size(1023, 413);
            this.metroTabControl.TabIndex = 5;
            this.metroTabControl.UseSelectable = true;
            // 
            // tabPageReplays
            // 
            this.tabPageReplays.Controls.Add(this.btnPlayReplay);
            this.tabPageReplays.Controls.Add(this.lblGameData);
            this.tabPageReplays.Controls.Add(this.metroLabel3);
            this.tabPageReplays.Controls.Add(this.metroLabel4);
            this.tabPageReplays.Controls.Add(this.txtRedStats);
            this.tabPageReplays.Controls.Add(this.txtBlueStats);
            this.tabPageReplays.Controls.Add(this.lstReplays);
            this.tabPageReplays.Controls.Add(this.label2);
            this.tabPageReplays.Controls.Add(this.label1);
            this.tabPageReplays.Controls.Add(this.btnGetUrl);
            this.tabPageReplays.Controls.Add(this.txtDownloadUrl);
            this.tabPageReplays.Controls.Add(this.txtMatchId);
            this.tabPageReplays.HorizontalScrollbarBarColor = true;
            this.tabPageReplays.HorizontalScrollbarHighlightOnWheel = false;
            this.tabPageReplays.HorizontalScrollbarSize = 10;
            this.tabPageReplays.Location = new System.Drawing.Point(4, 38);
            this.tabPageReplays.Name = "tabPageReplays";
            this.tabPageReplays.Size = new System.Drawing.Size(1015, 371);
            this.tabPageReplays.TabIndex = 0;
            this.tabPageReplays.Text = "Replays     ";
            this.tabPageReplays.VerticalScrollbarBarColor = true;
            this.tabPageReplays.VerticalScrollbarHighlightOnWheel = false;
            this.tabPageReplays.VerticalScrollbarSize = 10;
            // 
            // btnPlayReplay
            // 
            this.btnPlayReplay.Location = new System.Drawing.Point(384, 321);
            this.btnPlayReplay.Name = "btnPlayReplay";
            this.btnPlayReplay.Size = new System.Drawing.Size(72, 36);
            this.btnPlayReplay.TabIndex = 21;
            this.btnPlayReplay.Text = "Play Replay";
            this.btnPlayReplay.UseSelectable = true;
            this.btnPlayReplay.Click += new System.EventHandler(this.btnPlayReplay_Click);
            // 
            // lblGameData
            // 
            this.lblGameData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGameData.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblGameData.Location = new System.Drawing.Point(192, 6);
            this.lblGameData.Name = "lblGameData";
            this.lblGameData.Size = new System.Drawing.Size(456, 21);
            this.lblGameData.TabIndex = 20;
            this.lblGameData.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.metroLabel3.Location = new System.Drawing.Point(462, 27);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(79, 19);
            this.metroLabel3.TabIndex = 19;
            this.metroLabel3.Text = "Red Team:";
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.metroLabel4.Location = new System.Drawing.Point(192, 27);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(82, 19);
            this.metroLabel4.TabIndex = 18;
            this.metroLabel4.Text = "Blue Team:";
            // 
            // txtRedStats
            // 
            this.txtRedStats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtRedStats.Lines = new string[0];
            this.txtRedStats.Location = new System.Drawing.Point(462, 49);
            this.txtRedStats.MaxLength = 32767;
            this.txtRedStats.Multiline = true;
            this.txtRedStats.Name = "txtRedStats";
            this.txtRedStats.PasswordChar = '\0';
            this.txtRedStats.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtRedStats.SelectedText = "";
            this.txtRedStats.SelectionStart = 0;
            this.txtRedStats.Size = new System.Drawing.Size(186, 311);
            this.txtRedStats.TabIndex = 17;
            this.txtRedStats.UseSelectable = true;
            // 
            // txtBlueStats
            // 
            this.txtBlueStats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtBlueStats.Lines = new string[0];
            this.txtBlueStats.Location = new System.Drawing.Point(192, 49);
            this.txtBlueStats.MaxLength = 32767;
            this.txtBlueStats.Multiline = true;
            this.txtBlueStats.Name = "txtBlueStats";
            this.txtBlueStats.PasswordChar = '\0';
            this.txtBlueStats.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtBlueStats.SelectedText = "";
            this.txtBlueStats.SelectionStart = 0;
            this.txtBlueStats.Size = new System.Drawing.Size(186, 311);
            this.txtBlueStats.TabIndex = 16;
            this.txtBlueStats.UseSelectable = true;
            // 
            // lstReplays
            // 
            this.lstReplays.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.metroStyleExtender.SetApplyMetroTheme(this.lstReplays, true);
            this.lstReplays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstReplays.FormattingEnabled = true;
            this.lstReplays.Location = new System.Drawing.Point(3, 3);
            this.lstReplays.Name = "lstReplays";
            this.lstReplays.Size = new System.Drawing.Size(183, 366);
            this.lstReplays.TabIndex = 10;
            this.lstReplays.SelectedIndexChanged += new System.EventHandler(this.lstReplays_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(732, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 19);
            this.label2.TabIndex = 9;
            this.label2.Text = "DownloadURL:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(780, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 19);
            this.label1.TabIndex = 8;
            this.label1.Text = "Match ID:";
            // 
            // btnGetUrl
            // 
            this.btnGetUrl.Location = new System.Drawing.Point(782, 133);
            this.btnGetUrl.Name = "btnGetUrl";
            this.btnGetUrl.Size = new System.Drawing.Size(144, 22);
            this.btnGetUrl.TabIndex = 7;
            this.btnGetUrl.Text = "Get Url";
            this.btnGetUrl.UseSelectable = true;
            this.btnGetUrl.Click += new System.EventHandler(this.btnGetUrl_Click);
            // 
            // txtDownloadUrl
            // 
            this.txtDownloadUrl.Lines = new string[0];
            this.txtDownloadUrl.Location = new System.Drawing.Point(732, 104);
            this.txtDownloadUrl.MaxLength = 32767;
            this.txtDownloadUrl.Name = "txtDownloadUrl";
            this.txtDownloadUrl.PasswordChar = '\0';
            this.txtDownloadUrl.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtDownloadUrl.SelectedText = "";
            this.txtDownloadUrl.SelectionStart = 0;
            this.txtDownloadUrl.Size = new System.Drawing.Size(280, 20);
            this.txtDownloadUrl.TabIndex = 6;
            this.txtDownloadUrl.UseSelectable = true;
            // 
            // txtMatchId
            // 
            this.txtMatchId.Lines = new string[0];
            this.txtMatchId.Location = new System.Drawing.Point(782, 59);
            this.txtMatchId.MaxLength = 32767;
            this.txtMatchId.Name = "txtMatchId";
            this.txtMatchId.PasswordChar = '\0';
            this.txtMatchId.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtMatchId.SelectedText = "";
            this.txtMatchId.SelectionStart = 0;
            this.txtMatchId.Size = new System.Drawing.Size(145, 20);
            this.txtMatchId.TabIndex = 5;
            this.txtMatchId.UseSelectable = true;
            // 
            // tabMatchHistory
            // 
            this.tabMatchHistory.Controls.Add(this.panelMatchHistory);
            this.tabMatchHistory.Controls.Add(this.tileRefresh);
            this.tabMatchHistory.HorizontalScrollbarBarColor = true;
            this.tabMatchHistory.HorizontalScrollbarHighlightOnWheel = false;
            this.tabMatchHistory.HorizontalScrollbarSize = 10;
            this.tabMatchHistory.Location = new System.Drawing.Point(4, 38);
            this.tabMatchHistory.Name = "tabMatchHistory";
            this.tabMatchHistory.Size = new System.Drawing.Size(1015, 371);
            this.tabMatchHistory.TabIndex = 2;
            this.tabMatchHistory.Text = "Match History     ";
            this.tabMatchHistory.VerticalScrollbarBarColor = true;
            this.tabMatchHistory.VerticalScrollbarHighlightOnWheel = false;
            this.tabMatchHistory.VerticalScrollbarSize = 10;
            // 
            // panelMatchHistory
            // 
            this.panelMatchHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMatchHistory.AutoScroll = true;
            this.panelMatchHistory.FlowDirection = System.Windows.Forms.FlowDirection.BottomUp;
            this.panelMatchHistory.Location = new System.Drawing.Point(3, 26);
            this.panelMatchHistory.Name = "panelMatchHistory";
            this.panelMatchHistory.Size = new System.Drawing.Size(1009, 342);
            this.panelMatchHistory.TabIndex = 4;
            this.panelMatchHistory.WrapContents = false;
            // 
            // tileRefresh
            // 
            this.tileRefresh.ActiveControl = null;
            this.tileRefresh.Location = new System.Drawing.Point(3, 3);
            this.tileRefresh.Name = "tileRefresh";
            this.tileRefresh.PaintTileCount = false;
            this.tileRefresh.Size = new System.Drawing.Size(68, 20);
            this.tileRefresh.TabIndex = 2;
            this.tileRefresh.Text = "Refresh";
            this.tileRefresh.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.tileRefresh.UseSelectable = true;
            this.tileRefresh.Click += new System.EventHandler(this.tileRefresh_Click);
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.Controls.Add(this.txtGameLocation);
            this.tabPageSettings.Controls.Add(this.metroLabel2);
            this.tabPageSettings.Controls.Add(this.lstReplayFolders);
            this.tabPageSettings.Controls.Add(this.metroLabel1);
            this.tabPageSettings.HorizontalScrollbarBarColor = true;
            this.tabPageSettings.HorizontalScrollbarHighlightOnWheel = false;
            this.tabPageSettings.HorizontalScrollbarSize = 10;
            this.tabPageSettings.Location = new System.Drawing.Point(4, 38);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Size = new System.Drawing.Size(1015, 371);
            this.tabPageSettings.TabIndex = 1;
            this.tabPageSettings.Text = "Settings     ";
            this.tabPageSettings.UseStyleColors = true;
            this.tabPageSettings.VerticalScrollbarBarColor = true;
            this.tabPageSettings.VerticalScrollbarHighlightOnWheel = false;
            this.tabPageSettings.VerticalScrollbarSize = 10;
            // 
            // txtGameLocation
            // 
            this.txtGameLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGameLocation.Lines = new string[0];
            this.txtGameLocation.Location = new System.Drawing.Point(19, 145);
            this.txtGameLocation.MaxLength = 32767;
            this.txtGameLocation.Name = "txtGameLocation";
            this.txtGameLocation.PasswordChar = '\0';
            this.txtGameLocation.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtGameLocation.SelectedText = "";
            this.txtGameLocation.SelectionStart = 0;
            this.txtGameLocation.Size = new System.Drawing.Size(632, 24);
            this.txtGameLocation.TabIndex = 5;
            this.txtGameLocation.UseSelectable = true;
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.metroLabel2.Location = new System.Drawing.Point(19, 123);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(165, 19);
            this.metroLabel2.TabIndex = 4;
            this.metroLabel2.Text = "Default Game Location:";
            // 
            // lstReplayFolders
            // 
            this.lstReplayFolders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroStyleExtender.SetApplyMetroTheme(this.lstReplayFolders, true);
            this.lstReplayFolders.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstReplayFolders.FormattingEnabled = true;
            this.lstReplayFolders.Location = new System.Drawing.Point(19, 41);
            this.lstReplayFolders.Name = "lstReplayFolders";
            this.lstReplayFolders.Size = new System.Drawing.Size(632, 67);
            this.lstReplayFolders.TabIndex = 3;
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.metroLabel1.Location = new System.Drawing.Point(17, 13);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(112, 19);
            this.metroLabel1.TabIndex = 2;
            this.metroLabel1.Text = "Replay Folders:";
            // 
            // metroStyleManager
            // 
            this.metroStyleManager.Owner = this;
            this.metroStyleManager.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // lblGameVersion
            // 
            this.lblGameVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGameVersion.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblGameVersion.Location = new System.Drawing.Point(320, 39);
            this.lblGameVersion.Name = "lblGameVersion";
            this.lblGameVersion.Size = new System.Drawing.Size(723, 21);
            this.lblGameVersion.TabIndex = 6;
            this.lblGameVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ReplayManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1063, 493);
            this.Controls.Add(this.lblGameVersion);
            this.Controls.Add(this.metroTabControl);
            this.Name = "ReplayManager";
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
            this.StyleManager = this.metroStyleManager;
            this.Text = "Replay Manager";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroTabControl.ResumeLayout(false);
            this.tabPageReplays.ResumeLayout(false);
            this.tabPageReplays.PerformLayout();
            this.tabMatchHistory.ResumeLayout(false);
            this.tabPageSettings.ResumeLayout(false);
            this.tabPageSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroTabControl metroTabControl;
        private MetroFramework.Controls.MetroTabPage tabPageReplays;
        private MetroFramework.Controls.MetroTabPage tabPageSettings;
        private MetroFramework.Components.MetroStyleManager metroStyleManager;
        private MetroFramework.Components.MetroStyleExtender metroStyleExtender;
        private MetroFramework.Controls.MetroLabel label2;
        private MetroFramework.Controls.MetroLabel label1;
        private MetroFramework.Controls.MetroButton btnGetUrl;
        private MetroFramework.Controls.MetroTextBox txtDownloadUrl;
        private MetroFramework.Controls.MetroTextBox txtMatchId;
        private System.Windows.Forms.ListBox lstReplayFolders;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel lblGameVersion;
        private System.Windows.Forms.ListBox lstReplays;
        private MetroFramework.Controls.MetroTextBox txtGameLocation;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroTextBox txtRedStats;
        private MetroFramework.Controls.MetroTextBox txtBlueStats;
        private MetroFramework.Controls.MetroLabel lblGameData;
        private MetroFramework.Controls.MetroButton btnPlayReplay;
        private MetroFramework.Controls.MetroTabPage tabMatchHistory;
        private MetroFramework.Controls.MetroTile tileRefresh;
        private MetroFramework.Controls.MetroFlowLayoutPanel panelMatchHistory;
    }
}

