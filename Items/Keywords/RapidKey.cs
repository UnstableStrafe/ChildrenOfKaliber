using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Gungeon;

namespace Items.Keywords
{
    class RapidKey : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Rapid Type";

            string resourceName = "Items/Keywords/rapid_key.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<RapidKey>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Monogun Type";
            string longDesc = "Gives The Monogun Rapid type.";

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
            debrisObject.GetComponent<RapidKey>().m_pickedUpThisRun = true;
            return debrisObject;
        }
    }
}
