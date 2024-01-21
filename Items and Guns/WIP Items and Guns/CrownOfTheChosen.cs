using System;
using Alexandria.ItemAPI;
using UnityEngine;
using Gungeon;
using Dungeonator;

namespace Items
{
    class CrownOfTheChosen : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Crown Of The Chosen";

            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<CrownOfTheChosen>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "For The Worthy";
            string longDesc = "Gives a powerful blessing to those who wear it.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ck");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 1.4f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.RateOfFire, 1.3f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.quality = ItemQuality.S;
            item.sprite.IsPerpendicular = true;
            
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);

        }
        
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<CrownOfTheChosen>().m_pickedUpThisRun = true;

            return debrisObject;
        }
    }
}
