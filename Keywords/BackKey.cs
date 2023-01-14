using UnityEngine;
using Alexandria.ItemAPI;

namespace Items.Keywords
{
    class BackKey : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Backshot Keyword";

            string resourceName = "Items/Keywords/back_key.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<BackKey>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Monogun Keyword";
            string longDesc = "Gives The Monogun backshots.";

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

