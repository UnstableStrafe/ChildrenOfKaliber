using UnityEngine;
using ItemAPI;

namespace Items.Keywords
{
    class PoisonKey : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Poison Keyword";

            string resourceName = "Items/Keywords/poison_key.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<PoisonKey>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Monogun Keyword";
            string longDesc = "Gives The Monogun a chance to poison.";

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
