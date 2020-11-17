using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using Gungeon;
using ItemAPI;
using MonoMod;
using MonoMod.RuntimeDetour;
using Random = UnityEngine.Random;
using UnityEngine;
using Dungeonator;

namespace Items
{
    class MidasRounds : PassiveItem
    {


        public static void Init()
        {
            string itemName = "Midas Rounds";

            string resourceName = "Items/Resources/midas_rounds.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<MidasRounds>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Givers of Gold";
            string longDesc = "Killed enemies may be transmuted into gold.\n\nA curse that had befallen a human king who wanted unlimitted wealth. Now the curse has made its way to the gungeon.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;
        }
        private void postProj(Projectile projectile, float eff)
        {

        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<MidasRounds>().m_pickedUpThisRun = true;
            return debrisObject;
        }
    }
}
