using UnityEngine;
using Alexandria.ItemAPI;

namespace Items.Keywords
{
    class BounceKey : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Bounce Keyword";

            string resourceName = "Items/Keywords/bounce_key.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<BounceKey>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Monogun Keyword";
            string longDesc = "Gives The Monogun bouncing shots.";

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
