using UnityEngine;
using Alexandria.ItemAPI;

namespace Items.Keywords
{
    class SidearmKey : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Sidearm Keyword";

            string resourceName = "Items/Keywords/sidearm_key.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<SidearmKey>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Monogun Keyword";
            string longDesc = "Gives The Monogun less damage, but more clip size.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ck");

            item.CanBeDropped = false;
            item.CanBeSold = false;
            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;


        }
        
        public override void Pickup(PlayerController player)
        {

            base.Pickup(player);

        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            return debrisObject;
        }
    }
}
