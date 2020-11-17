using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;
using Dungeonator;

namespace Items
{
    class PassiveTemplate : PassiveItem
    {
        /* public static void Init()
         {
             string itemName = "";

             string resourceName = "Items/Resources/test_icon.png";

             GameObject obj = new GameObject(itemName);

             var item = obj.AddComponent<>();

             ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

             string shortDesc = "";
             string longDesc = "";

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
             debrisObject.GetComponent<>().m_pickedUpThisRun = true;

             return debrisObject;
         }
         */
    }
}
