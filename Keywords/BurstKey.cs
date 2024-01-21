using UnityEngine;
using Alexandria.ItemAPI;

namespace Items.Keywords
{
    class BurstKey : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Burst Keyword";

            string resourceName = "Items/Keywords/burst_key.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<BurstKey>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Monogun Keyword";
            string longDesc = "Gives The Monogun burst shoot style.";

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
