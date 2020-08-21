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
    class GravityWell : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Gravity Well Module";

            string resourceName = "Items/Resources/gravity_well.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<GravityWell>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Where Is My Super Suit?";
            string longDesc = "Reduces knockback from weapons and gives some damage.\n\nAdding this to a gun increases the gravitational field of its bullets until they have a pull that is strong enough to counteract their kenetic force.";
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.KnockbackMultiplier, .6f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 1.25f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.C;
            item.sprite.IsPerpendicular = true;
            
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
        }
        private void Syn(Projectile projectile, float effectChanceScalar)
        {
            // projectile.AdjustPlayerProjectileTint(new Color(32, 4, 46), 6, 0f);
            Color color1 = new Color(32, 4, 46);
          // projectile.AdjustPlayerProjectileTint(color1.WithAlpha(color1.a / 2f), 7, 0f);
            if (Owner.HasPickupID(111))
            {
                projectile.BossDamageMultiplier *= 1.2f;
            }
            if(Owner.HasPickupID(155) || Owner.HasPickupID(169))
            {
                projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.VoidOnHit));
            }
        }
        private void VoidOnHit(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {

            if (!Owner.CurrentGun.InfiniteAmmo)
            {
                float Void = 1f;
                if((Random.Range(0f, 100f) <= Void))
                {
                    Projectile projectile2 = ((Gun)ETGMod.Databases.Items[169]).DefaultModule.projectiles[0];
                    GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, arg2.sprite.WorldBottomCenter, Quaternion.Euler(0f, 0f, 0f));
                    Projectile component = gameObject.GetComponent<Projectile>();


                    if (component != null)
                    {
                        component.Owner = base.Owner;
                        component.Shooter = base.Owner.specRigidbody;
                        component.baseData.speed = 0f;
                        component.baseData.range = 1f;
                        
                    }
                }
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.Syn;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.PostProcessProjectile -= this.Syn;
            debris.GetComponent<GravityWell>().m_pickedUpThisRun = true;
            return debrisObject;
        }
    }
}
