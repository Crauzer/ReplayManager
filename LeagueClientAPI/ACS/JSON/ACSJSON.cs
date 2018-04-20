using System;
using System.Collections.Generic;

namespace LeagueClientAPI.ACS.JSON
{
    public class ACSMatchList
    {
        public GameMetaData games;
    }

    public class GameMetaData
    {
        public int gameIndexBegin;
        public int gameIndexEnd;
        public int gameCount;
        public List<GameData> games;
        public List<int> shownQueues;
    }

    public class GameData
    {
        public UInt64 gameId;
        public string platformId;
        public string gameCreation;
        public int gameDuration;
        public int queueId;
        public int mapId;
        public int seasonId;
        public string gameVersion;
        public string gameMode;
        public string gameType;
        public List<PlayerData> participants;
    }

    public class PlayerData
    {
        public int participantId;
        public int teamId;
        public int championId;
        public int spell1Id;
        public int spell2Id;
        public PlayerStats stats;
        public TimelineData timeline;
    }

    public class PlayerStats
    {
        public bool win;
        public int item0;
        public int item1;
        public int item2;
        public int item3;
        public int item4;
        public int item5;
        public int item6;
        public int kills;
        public int deaths;
        public int assists;
        public int largestKillingSpree;
        public int largestMultiKill;
        public int killingSprees;
        public int longestTimeSpentLiving;
        public int doubleKills;
        public int tripleKills;
        public int quadraKills;
        public int pentaKills;
        public int unrealKills;
        public int totalDamageDealt;
        public int magicDamageDealt;
        public int physicalDamageDealt;
        public int trueDamageDealt;
        public int largestCriticalStrike;
        public int totalDamageDealtToChampions;
        public int magicDamageDealtToChampions;
        public int physicalDamageDealtToChampions;
        public int trueDamageDealtToChampions;
        public int totalHeal;
        public int totalUnitsHealed;
        public int damageSelfMitigated;
        public int damageDealtToObjectives;
        public int damageDealtToTurrets;
        public int visionScore;
        public int timeCCingOthers;
        public int totalDamageTaken;
        public int magicalDamageTaken;
        public int physicalDamageTaken;
        public int trueDamageTaken;
        public int goldEarned;
        public int goldSpent;
        public int turretKills;
        public int inhibitorKills;
        public int totalMinionsKilled;
        public int neutralMinionsKilled;
        public int neutralMinionsKilledTeamJungle;
        public int neutralMinionsKilledEnemyJungle;
        public int totalTimeCrowdControlDealt;
        public int champLevel;
        public int visionWardsBoughtInGame;
        public int sightWardsBoughtInGame;
        public int wardsPlaced;
        public int wardsKilled;
        public bool firstBloodKill;
        public bool firstBloodAssist;
        public bool firstTowerKill;
        public bool firstTowerAssist;
        public bool firstInhibitorKill;
        public bool firstInhibitorAssist;
        public int combatPlayerScore;
        public int objectivePlayerScore;
        public int totalPlayerScore;
        public int totalScoreRank;
        public int playerScore0;
        public int playerScore1;
        public int playerScore2;
        public int playerScore3;
        public int playerScore4;
        public int playerScore5;
        public int playerScore6;
        public int playerScore7;
        public int playerScore8;
        public int playerScore9;
        public int perk0;
        public int perk0Var1;
        public int perk0Var2;
        public int perk0Var3;
        public int perk1;
        public int perk1Var1;
        public int perk1Var2;
        public int perk1Var3;
        public int perk2;
        public int perk2Var1;
        public int perk2Var2;
        public int perk2Var3;
        public int perk3;
        public int perk3Var1;
        public int perk3Var2;
        public int perk3Var3;
        public int perk4;
        public int perk4Var1;
        public int perk4Var2;
        public int perk4Var3;
        public int perk5;
        public int perk5Var1;
        public int perk5Var2;
        public int perk5Var3;
        public int perkPrimaryStyle;
        public int perkSubStyle;
    }

    public class TimelineData
    {
        public string role;
        public string lane;
        /*creepsPerMinDeltas	
            10-20	5.8
            0-10	6.6
            20-30	6.4
        xpPerMinDeltas	
            10-20	541.9
            0-10	440.4
            20-30	701.8
        goldPerMinDeltas	
            10-20	346.5
            0-10	270.20000000000005
            20-30	490.5
        csDiffPerMinDeltas	
            10-20	-2.4000000000000004
            0-10	-1
            20-30	2.3
        xpDiffPerMinDeltas	
            10-20	98.89999999999998
            0-10	-46.99999999999997
            20-30	292.19999999999993
        damageTakenPerMinDeltas	
            10-20	1263
            0-10	310.79999999999995
            20-30	1625.9
        damageTakenDiffPerMinDeltas	
            10-20	431.0999999999999
            0-10	-78.50000000000001
            20-30	309.69999999999993
        */
    }



}
