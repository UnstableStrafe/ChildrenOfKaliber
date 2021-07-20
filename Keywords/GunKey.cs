using UnityEngine;
using ItemAPI;

namespace Items.Keywords
{
    class GunKey : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Gun Type";

            string resourceName = "Items/Keywords/gun_key.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<GunKey>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Monogun Type";
            string longDesc = "Gives The Monogun Gun type.";

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
            debrisObject.GetComponent<GunKey>().m_pickedUpThisRun = true;
            return debrisObject;
        }
    }
}
