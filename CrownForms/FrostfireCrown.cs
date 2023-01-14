using System;
using System.Collections.Generic;
using Alexandria.ItemAPI;
using System.Collections;
using UnityEngine;
using Gungeon;
using Dungeonator;

namespace Items
{
    class FrostfireCrown : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Frostfire Crown";

            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<FrostfireCrown>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Forever Wandering";
            string longDesc = "Adds a flaming projectile and a freezing projectile to each shot";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

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
            debrisObject.GetComponent<FrostfireCrown>().m_pickedUpThisRun = true;

            return debrisObject;
        }
    }
}
