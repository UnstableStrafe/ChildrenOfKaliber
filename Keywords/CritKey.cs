using UnityEngine;
using Alexandria.ItemAPI;

namespace Items.Keywords
{
    class CritKey : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Crit Keyword";

            string resourceName = "Items/Keywords/crit_key.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<CritKey>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Monogun Keyword";
            string longDesc = "Gives The Monogun a chance to crit enemies.";

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
