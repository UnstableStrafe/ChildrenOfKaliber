using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace Items
{
    class Glyph : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Glyph of Wind 11";

            string resourceName = "Items/Resources/glyph.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<PassiveItem>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Surges Bound";
            string longDesc = "Increases speed and damage as health decreases.\n\n" +
                "Nightingales go through rigorous training for years to refine their bodies. Those who survive are marked with this glyph on their forehead, which boosts the bearer's adrenal glands.";


            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

                 

            int Speed;

            Speed = 4;

           // ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, Speed, StatModifier.ModifyMethod.MULTIPLICATIVE);
            

            item.quality = PickupObject.ItemQuality.EXCLUDED;

           
        }



        public override void Pickup(PlayerController player)
        {
            if (this.m_pickedUp)
            {
                return;
            }
            base.Pickup(player);
            float curHealth = player.healthHaver.GetCurrentHealthPercentage();
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<Glyph>().m_pickedUpThisRun = true;
            return debrisObject;
        }


    }

}
