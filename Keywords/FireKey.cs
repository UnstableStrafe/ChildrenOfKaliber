using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Gungeon;

namespace Items.Keywords
{
    class FireKey : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Fire Type";

            string resourceName = "Items/Keywords/fire_key.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<FireKey>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Monogun Type";
            string longDesc = "Gives The Monogun Fire type.";

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
            debrisObject.GetComponent<FireKey>().m_pickedUpThisRun = true;
            return debrisObject;
        }
    }
}
