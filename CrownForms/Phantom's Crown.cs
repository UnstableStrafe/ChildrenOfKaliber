using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;

namespace Items.CrownForms
{
    class Phantom_s_Crown : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Phantom's Crown";

            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<Phantom_s_Crown>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Steal Your Heart!";
            string longDesc = "Unleas devastating finales on bosses under 33% health!";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ck");

            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;
        }
        private void PostProj(Projectile projectile, float eff)
        {
            projectile.OnHitEnemy += CheckIfLowHP;
        }
        private void CheckIfLowHP(Projectile projectile, SpeculativeRigidbody shadow, bool fatal)
        {
            if(shadow.healthHaver && shadow.aiActor && !fatal)
            {
                float healthPercent = shadow.healthHaver.GetCurrentHealthPercentage();
                if (shadow.healthHaver.IsBoss && healthPercent < (1 / 3))
                {

                }
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += PostProj;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<Phantom_s_Crown>().m_pickedUpThisRun = true;
            player.PostProcessProjectile -= PostProj;
            return debrisObject;
        }
    }
}
