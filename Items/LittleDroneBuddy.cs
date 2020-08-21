using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;
using Dungeonator;

namespace Items
{
    class LittleDroneBuddy : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Little Drone Buddy";

            string resourceName = "Items/Resources/little_drone.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<LittleDroneBuddy>();
            
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "D-1";
            string longDesc = "Shoots automatically at nearby enemies while reloading. Slightly increases reload speed.\n\nFirst of Isaac Fogcutter's foray into friendly flying fellows fashioned for fighting foes.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ReloadSpeed, 1.15f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.quality = ItemQuality.C;
            item.sprite.IsPerpendicular = true;

        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<LittleDroneBuddy>().m_pickedUpThisRun = true;

            return debrisObject;
        }
    }
}
