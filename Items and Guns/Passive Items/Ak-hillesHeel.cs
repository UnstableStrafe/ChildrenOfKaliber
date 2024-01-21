using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;

namespace Items
{
    class Ak_hillesHeel : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Ak-hilles Heel";

            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<Ak_hillesHeel>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "To Shoot A Bullet...";
            string longDesc = "";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ck");

            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;
            
            SpearProj = UnityEngine.Object.Instantiate<Projectile>(((Gun)ETGMod.Databases.Items[56]).DefaultModule.projectiles[0]); ;
            SpearProj.SetProjectileSpriteRight("ak-hilles_spear", 23, 7);
        }
        private Projectile ReplacementProj(Gun sourceGun, Projectile sourceProj)
        {
            if (sourceGun.InfiniteAmmo || sourceGun.HasShootStyle(ProjectileModule.ShootStyle.Beam) || sourceGun.PickupObjectId == 504 || sourceGun.PickupObjectId == 251 || sourceGun.PickupObjectId == 695)
            {
                return sourceProj;
            }
            else
            {
                float num = 1f / sourceGun.DefaultModule.cooldownTime;
                if (sourceGun.Volley != null)
                {
                    float num2 = 0f;
                    for (int i = 0; i < sourceGun.Volley.projectiles.Count; i++)
                    {
                        ProjectileModule projectileModule = sourceGun.Volley.projectiles[i];
                        num2 += projectileModule.GetEstimatedShotsPerSecond(sourceGun.reloadTime);
                    }
                    if (num2 > 0f)
                    {
                        num = num2;
                    }
                }
                float chanceToProc = .5f;
                float num3 = Mathf.Clamp01(chanceToProc / num);
                num3 = Mathf.Max(0.0001f, num3);
                ETGModConsole.Log(num3.ToString());
                if (UnityEngine.Random.value > num3)
                {
                    return sourceProj;
                }
                Projectile newProj = SpearProj;
                newProj.baseData.range *= 10;
                StickyProjectile sticky = newProj.gameObject.AddComponent<StickyProjectile>();
                newProj.Owner = Owner;
                newProj.Shooter = Owner.specRigidbody;
                newProj.baseData.damage = sourceProj.baseData.damage * 2;
                newProj.baseData.speed = sourceProj.baseData.speed * 1.5f;
                return newProj;
            }
            
        }
        public override void Pickup(PlayerController player)
        {
            player.OnPreFireProjectileModifier += ReplacementProj;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<Ak_hillesHeel>().m_pickedUpThisRun = true;
            player.OnPreFireProjectileModifier -= ReplacementProj;
            return debrisObject;
        }
        private static Projectile SpearProj;
    }
    
}
