using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ReplayManager.ROFL
{

    public class PlayerStats
    {
        public string NAME;
        public long ID;
        public string SKIN;
        public int SKINID;
        public int TEAM;
        public string WIN;
        public int EXP;
        public int LEVEL;
        public int GOLD_SPENT;
        public int GOLD_EARNED;
        public int MINIONS_KILLED;
        public int NEUTRAL_MINIONS_KILLED0;
        public int NEUTRAL_MINIONS_KILLED_YOUR_JUNGLE;
        public int NEUTRAL_MINIONS_KILLED_ENEMY_JUNGLE;
        public int CHAMPIONS_KILLED;
        public int NUM_DEATHS;
        public int ASSISTS;
        public int LARGEST_KILLING_SPREE;
        public int KILLING_SPREES;
        public int LARGEST_MULTI_KILL;
        public int BOUNTY_LEVEL;
        public int DOUBLE_KILLS;
        public int TRIPLE_KILLS;
        public int QUADRA_KILLS;
        public int PENTA_KILLS;
        public int UNREAL_KILLS;
        public int BARRACKS_KILLED;
        public int TURRETS_KILLED;
        public int HQ_KILLED;
        public int FRIENDLY_DAMPEN_LOST;
        public int FRIENDLY_TURRET_LOST;
        public int FRIENDLY_HQ_LOST;
        public int NODE_CAPTURE;
        public int NODE_CAPTURE_ASSIST;
        public int NODE_NEUTRALIZE;
        public int NODE_NEUTRALIZE_ASSIST;
        public int TEAM_OBJECTIVE;
        public int PLAYER_SCORE_0;
        public int PLAYER_SCORE_1;
        public int PLAYER_SCORE_2;
        public int PLAYER_SCORE_3;
        public int VICTORY_POINT_TOTAL;
        public int TOTAL_PLAYER_SCORE;
        public int COMBAT_PLAYER_SCORE;
        public int OBJECTIVE_PLAYER_SCORE;
        public int TOTAL_SCORE_RANK;
        public int ITEMS_PURCHASED;
        public int CONSUMABLES_PURCHASED;
        public int ITEM0;
        public int ITEM1;
        public int ITEM2;
        public int ITEM3;
        public int ITEM4;
        public int ITEM5;
        public int ITEM6;
        public int SIGHT_WARDS_BOUGHT_IN_GAME;
        public int VISION_WARDS_BOUGHT_IN_GAME;
        public int WARD_PLACED;
        public int WARD_KILLED;
        public int WARD_PLACED_DETECTOR;
        public int VISION_SCORE;
        public int SPELL1_CAST;
        public int SPELL2_CAST;
        public int SPELL3_CAST;
        public int SPELL4_CAST;
        public int SUMMON_SPELL1_CAST;
        public int SUMMON_SPELL2_CAST;
        public int KEYSTONE_ID;
        public int TOTAL_DAMAGE_DEALT;
        public int PHYSICAL_DAMAGE_DEALT_PLAYER;
        public int MAGIC_DAMAGE_DEALT_PLAYER;
        public int TRUE_DAMAGE_DEALT_PLAYER;
        public int TOTAL_DAMAGE_DEALT_TO_CHAMPIONS;
        public int PHYSICAL_DAMAGE_DEALT_TO_CHAMPIONS;
        public int MAGIC_DAMAGE_DEALT_TO_CHAMPIONS;
        public int TRUE_DAMAGE_DEALT_TO_CHAMPIONS;
        public int TOTAL_DAMAGE_TAKEN;
        public int PHYSICAL_DAMAGE_TAKEN;
        public int MAGIC_DAMAGE_TAKEN;
        public int TRUE_DAMAGE_TAKEN;
        public int TOTAL_DAMAGE_SELF_MITIGATED;
        public int TOTAL_DAMAGE_DEALT_TO_BUILDINGS;
        public int TOTAL_DAMAGE_DEALT_TO_TURRETS;
        public int TOTAL_DAMAGE_DEALT_TO_OBJECTIVES;
        public int LARGEST_CRITICAL_STRIKE;
        public int TOTAL_TIME_CROWD_CONTROL_DEALT;
        public int TOTAL_HEAL;
        public int TOTAL_UNITS_HEALED;
        public int TIME_PLAYED;
        public int LONGEST_TIME_SPENT_LIVING;
        public int TOTAL_TIME_SPENT_DEAD;
        public int TIME_OF_FROM_LAST_DISCONNECT;
        public int TIME_SPENT_DISCONNECTED;
        public int TIME_CCING_OTHERS;
        public bool WAS_AFK;
        public bool WAS_AFK_AFTER_FAILED_SURRENDER;
        public bool WAS_EARLY_SURRENDER_ACCOMPLICE;
        public bool TEAM_EARLY_SURRENDERED;
        public bool GAME_ENDED_IN_EARLY_SURRENDER;
        public bool GAME_ENDED_IN_SURRENDER;
        public int PLAYERS_I_MUTED;
        public int PLAYERS_THAT_MUTED_ME;
        public int MUTED_ALL;
        public int PING;
        public int PLAYER_ROLE;
        public int PLAYER_POSITION;
        public string CHAMPION_TRANSFORM;
    }

    [DataContract]
    public class ReplayMetadata
    {
        [DataMember]
        public string gameVersion;
        [DataMember]
        public int gameLength;
        [DataMember]
        public int lastGameChunkId;
        [DataMember]
        public int lastKeyFrameId;
        [DataMember]
        public string statsJson;
        public List<PlayerStats> playerStats;

        #region Methods

        public override string ToString()
        {
            return string.Format("<ReplayMetadata v={0}>", gameVersion);
        }

        #endregion

        #region Static Functions

        public static ReplayMetadata Deserialize(byte[] p_json)
        {
            ReplayMetadata rpmd;
            DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings()
            {
                UseSimpleDictionaryFormat = true,
                EmitTypeInformation = EmitTypeInformation.Never,
            };

            using (MemoryStream m = new MemoryStream(p_json))
            {
                DataContractJsonSerializer s = new DataContractJsonSerializer(typeof(ReplayMetadata), settings);
                rpmd = (ReplayMetadata)s.ReadObject(m);
            }

            if (rpmd.statsJson != null)
            {

                using (MemoryStream m = new MemoryStream(Encoding.UTF8.GetBytes(rpmd.statsJson)))
                {
                    DataContractJsonSerializer s = new DataContractJsonSerializer(typeof(List<PlayerStats>), settings);
                    rpmd.playerStats = (List<PlayerStats>)s.ReadObject(m);

                    if (rpmd.playerStats != null)
                    {
                        foreach (var stat in rpmd.playerStats)
                        {
                            stat.SKINID = -1;
                            if (stat.CHAMPION_TRANSFORM == null)
                                stat.CHAMPION_TRANSFORM = "NONE";
                        }
                    }
                }
            }

            return rpmd;
        }
        public static byte[] GetReplayMetadataBytes(ReplayMetadata rpmd)
        {
            DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings()
            {
                UseSimpleDictionaryFormat = true,
                EmitTypeInformation = EmitTypeInformation.Never,
            };

            MemoryStream ms1 = new MemoryStream();
            DataContractJsonSerializer ser1 = new DataContractJsonSerializer(typeof(List<PlayerStats>), settings);
            ser1.WriteObject(ms1, rpmd.playerStats);
            ms1.Position = 0;
            StreamReader sr1 = new StreamReader(ms1);
            String json1 = sr1.ReadToEnd();

            rpmd.statsJson = json1;

            MemoryStream ms = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ReplayMetadata), settings);
            ser.WriteObject(ms, rpmd);
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            String json = sr.ReadToEnd();
            return Encoding.UTF8.GetBytes(json);
        }

        #endregion

    }
}