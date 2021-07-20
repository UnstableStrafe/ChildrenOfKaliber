using UnityEngine;
using ItemAPI;

namespace Items.Keywords
{
    class HomingKey : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Homing Keyword";

            string resourceName = "Items/Keywords/homing_key.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<HomingKey>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Monogun Keyword";
            string longDesc = "Gives The Monogun homing shots.";

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
