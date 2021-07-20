using UnityEngine;
using ItemAPI;

namespace Items.Keywords
{
    class LargeKey : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Large Keyword";

            string resourceName = "Items/Keywords/large_key.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<LargeKey>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Monogun Keyword";
            string longDesc = "Gives The Monogun 25% more shot size.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

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
