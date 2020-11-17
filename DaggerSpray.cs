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
    class DaggerSpray : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Ritual Dagger";

            string resourceName = "Items/Resources/dagger.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<DaggerSpray>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Fatality!";
            string longDesc = "Killing an enemy releases a swarm of homing daggers. \n\nWhen given blood, this dagger vibrates rapidly.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
            item.quality = ItemQuality.A;
            item.sprite.IsPerpendicular = true;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
        }
        protected override void Update()
        {
            base.Update();
            this.CheckDamage();
        }
        private float DaggerBase = 3.5f, DaggerTrue;
        private float Damage, lastDamage = -1;
        private void CheckDamage()
        {
            Damage = Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
            if (Damage == lastDamage) return;
            DaggerTrue = DaggerBase * Damage;
            lastDamage = Damage;

        }
        private void DaggerSprayKill(PlayerController player)
        {
            Projectile projectile2 = ((Gun)ETGMod.Databases.Items[377]).DefaultModule.projectiles[0];
            GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : 0f), true);
            GameObject gameObject2 = SpawnManager.SpawnProjectile(projectile2.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : 90f), true);
            GameObject gameObject3 = SpawnManager.SpawnProjectile(projectile2.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : 180f), true);
            GameObject gameObject4 = SpawnManager.SpawnProjectile(projectile2.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : 270f), true);

            Projectile component = gameObject.GetComponent<Projectile>();
            Projectile component2 = gameObject2.GetComponent<Projectile>();
            Projectile component3 = gameObject3.GetComponent<Projectile>();
            Projectile component4 = gameObject4.GetComponent<Projectile>();

            if (component != null)
            {
                component.Owner = base.Owner;
                HomingModifier homingModifier = component.gameObject.AddComponent<HomingModifier>();
                homingModifier.HomingRadius = 100f;
                homingModifier.AngularVelocity = 500f;
                component.Shooter = base.Owner.specRigidbody;
                component.baseData.speed = 25f;
                component.baseData.damage = DaggerTrue;

            }
            if (component2 != null)
            {
                component2.Owner = base.Owner;
              //  component2.AdjustPlayerProjectileTint(new Color(117, 3, 1), 10, 0f);
                HomingModifier homingModifier = component2.gameObject.AddComponent<HomingModifier>();
                homingModifier.HomingRadius = 100f;
                homingModifier.AngularVelocity = 500f;
                component2.Shooter = base.Owner.specRigidbody;
                component2.baseData.speed = 25f;
                component2.baseData.damage = DaggerTrue;

            }
            if (component3 != null)
            {
                component3.Owner = base.Owner;
             //   component3.AdjustPlayerProjectileTint(new Color(117, 3, 1), 10, 0f);
                HomingModifier homingModifier = component3.gameObject.AddComponent<HomingModifier>();
                homingModifier.HomingRadius = 100f;
                homingModifier.AngularVelocity = 500f;
                component3.Shooter = base.Owner.specRigidbody;
                component3.baseData.speed = 25f;
                component3.baseData.damage = DaggerTrue;

            }
            if (component4 != null)
            {
                component4.Owner = base.Owner;
              //  component4.AdjustPlayerProjectileTint(new Color(117, 3, 1), 10, 0f);
                HomingModifier homingModifier = component4.gameObject.AddComponent<HomingModifier>();
                homingModifier.HomingRadius = 100f;
                homingModifier.AngularVelocity = 500f;
                component4.Shooter = base.Owner.specRigidbody;
                component4.baseData.speed = 25f;
                component4.baseData.damage = DaggerTrue;

            }
        }
        
        

        public override void Pickup(PlayerController player)
        {
            if (this.m_pickedUp)
            {
                return;
            }
            base.Pickup(player);
            player.OnKilledEnemy += this.DaggerSprayKill;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debris = base.Drop(player);
            debris.GetComponent<DaggerSpray>().m_pickedUpThisRun = true;
            player.OnKilledEnemy -= this.DaggerSprayKill;
            return debris;
        }
    }
}
