using UnityEngine;
using Alexandria.ItemAPI;

namespace Items.Keywords
{
    class FlareKey : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Flare Keyword";

            string resourceName = "Items/Keywords/flare_key.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<FlareKey>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Monogun Keyword";
            string longDesc = "Gives The Monogun a burst of sparks when the shot explodes.";

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
