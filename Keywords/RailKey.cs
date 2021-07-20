using UnityEngine;
using ItemAPI;

namespace Items.Keywords
{
    class RailKey : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Rail Type";

            string resourceName = "Items/Keywords/rail_key.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<RailKey>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Monogun Type";
            string longDesc = "Gives The Monogun Rail type.";

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
            debrisObject.GetComponent<RailKey>().m_pickedUpThisRun = true;
            return debrisObject;
        }
    }
}
