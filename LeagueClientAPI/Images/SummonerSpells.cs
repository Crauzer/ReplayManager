using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueClientAPI.Images
{
    internal static class SummonerSpells
    {
        [Serializable]
        public class SummonerSpell
        {
            public Int32 id;
            public String iconPath;
        }

        private static Dictionary<Int32, String> SummonerSpellLocations;

        static readonly APIRequest _apiRequest = APIRequest.Instance;

        public static Bitmap GetSummonerSpellIcon(int summonerSpellId)
        {
            if (SummonerSpellLocations == null)
            {
                SummonerSpellLocations = new Dictionary<Int32, String>();
                //SummonerSpellLocations
                String summonerSpellListJson;
                if (_apiRequest.Get("/lol-game-data/assets/v1/summoner-spells.json", out summonerSpellListJson))
                {
                    List<SummonerSpell> summonerSpellList = JsonHandler.LoadJson<List<SummonerSpell>>(summonerSpellListJson);
                    foreach (SummonerSpell summonerSpell in summonerSpellList)
                        if (!SummonerSpellLocations.ContainsKey(summonerSpell.id))
                            SummonerSpellLocations.Add(summonerSpell.id, summonerSpell.iconPath);
                }
                else
                    return PlaceholderIcon.Image;
            }

            byte[] image;
            if (SummonerSpellLocations.ContainsKey(summonerSpellId))
                if (_apiRequest.Get(SummonerSpellLocations[summonerSpellId], out image))
                    return Utility.LoadImage(image);

            return PlaceholderIcon.Image;
        }
    }
}
