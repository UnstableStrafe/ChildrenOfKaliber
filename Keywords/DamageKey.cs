using UnityEngine;
using Alexandria.ItemAPI;

namespace Items.Keywords
{
    class DamageKey : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Damage Keyword";

            string resourceName = "Items/Keywords/damage_key.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<DamageKey>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Monogun Keyword";
            string longDesc = "Gives The Monogun more damage, but lowers rate of fire.";

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
