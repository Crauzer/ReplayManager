using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueClientAPI.Images
{
    internal static class Items
    {
        [Serializable]
        public class Item
        {
            public Int32 id;
            public String iconPath;
        }

        private static Dictionary<Int32, String> ItemLocations;

        static readonly APIRequest _apiRequest = APIRequest.Instance;

        public static Bitmap GetItemIcon(int itemId)
        {
            if (ItemLocations == null)
            {
                ItemLocations = new Dictionary<Int32, String>();
                //SummonerSpellLocations
                String itemListJson;
                if (_apiRequest.Get("/lol-game-data/assets/v1/items.json", out itemListJson))
                {
                    List<Item> itemList = JsonHandler.LoadJson<List<Item>>(itemListJson);
                    foreach (Item item in itemList)
                        if (!ItemLocations.ContainsKey(item.id))
                            ItemLocations.Add(item.id, item.iconPath);
                }
                else
                    return PlaceholderIcon.Image;
            }

            byte[] image;
            if (ItemLocations.ContainsKey(itemId))
                if (_apiRequest.Get(ItemLocations[itemId], out image))
                    return Utility.LoadImage(image);

            return PlaceholderIcon.Image;
        }
    }
}
