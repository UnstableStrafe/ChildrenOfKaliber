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
    class DragunSkull : PassiveItem
    {
        public static void Init()
        {
               
            string itemName = "Dragun Skull";

            string resourceName = "Items/Resources/dragun_skull.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<DragunSkull>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "More Bite Than Bark";
            string longDesc = "Replaces all shots with fireballs that deal 20% more damage than the original shot and bypass even the strongest of fire immunities.\n\nPart of the skull of the High Dragun, flames still curl from the barrel." +
                "\n\nBroken by grief, the Dragun Warrior now guards the Gungeon's depths as the last of his kind, The High Dragun. Perhaps he seeks to prevent what he leanred from happening to others.";
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalShotPiercing, 1f, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.A;
            item.sprite.IsPerpendicular = true;
            item.CanBeDropped = true;
            item.CanBeSold = true;
        }
        private Gun HyperLight = ETGMod.Databases.Items[504] as Gun;
        private Projectile HandlePreFireProjectileModification(Gun sourceGun, Projectile sourceProjectile)
        {
            if (sourceGun.InfiniteAmmo || sourceGun.HasShootStyle(ProjectileModule.ShootStyle.Beam) || sourceGun.PickupObjectId == 504 || sourceGun.PickupObjectId == 251 || sourceGun.PickupObjectId == 695)
            {
                return sourceProjectile;
            }
            if (Owner.HasPickupID(146))
            {
                ReplacementProjectile = ((Gun)ETGMod.Databases.Items[722]).DefaultModule.projectiles[0];
            }
            else
            {
                ReplacementProjectile = ((Gun) ETGMod.Databases.Items[336]).DefaultModule.projectiles[0];
            }

            ReplacementProjectile.Owner = Owner;
            ReplacementProjectile.baseData.damage = sourceProjectile.baseData.damage * 1.2f * Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
            ReplacementProjectile.baseData.speed = sourceProjectile.baseData.speed * Owner.stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed);
            ReplacementProjectile.baseData.force = sourceProjectile.baseData.force * Owner.stats.GetStatValue(PlayerStats.StatType.KnockbackMultiplier);
            ReplacementProjectile.damageTypes = CoreDamageTypes.None;
            ReplacementProjectile.ignoreDamageCaps = sourceProjectile.ignoreDamageCaps;
   
            

            return this.ReplacementProjectile;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            WeightedGameObject Object1 = new WeightedGameObject
            {
                pickupId = ETGMod.Databases.Items["Dragun Wing"].PickupObjectId,
                weight = 1.5f,
                rawGameObject = ETGMod.Databases.Items["Dragun Wing"].gameObject,
                forceDuplicatesPossible = false
            };
            WeightedGameObject Object2 = new WeightedGameObject
            {
                pickupId = ETGMod.Databases.Items["Dragun Claw"].PickupObjectId,
                weight = 1.5f,
                rawGameObject = ETGMod.Databases.Items["Dragun Claw"].gameObject,
                forceDuplicatesPossible = false
            };
            WeightedGameObject Object3 = new WeightedGameObject
            {
                pickupId = ETGMod.Databases.Items["Dragun Heart"].PickupObjectId,
                weight = 1.5f,
                rawGameObject = ETGMod.Databases.Items["Dragun Heart"].gameObject,
                forceDuplicatesPossible = false
            };
            if (!player.HasPickupID(ETGMod.Databases.Items["Dragun Wing"].PickupObjectId))
            {
                GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements.Add(Object1);
            }
            if (!player.HasPickupID(ETGMod.Databases.Items["Dragun Claw"].PickupObjectId))
            {
                GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements.Add(Object2);
            }
            if (!player.HasPickupID(ETGMod.Databases.Items["Dragun Heart"].PickupObjectId))
            {
                GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements.Add(Object3);
            }
            player.OnPreFireProjectileModifier = (Func<Gun, Projectile, Projectile>)Delegate.Combine(player.OnPreFireProjectileModifier, new Func<Gun, Projectile, Projectile>(this.HandlePreFireProjectileModification));
         //   if (player.HasPickupID(ETGMod.Databases.Items["Dragun Heart"].PickupObjectId) && player.HasPickupID(ETGMod.Databases.Items["Dragun Wing"].PickupObjectId) && player.HasPickupID(ETGMod.Databases.Items["Dragun Claw"].PickupObjectId))
          //  {
          //      AkSoundEngine.PostEvent("Play_VO_dragun_death_01", gameObject);
          //  }
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnPreFireProjectileModifier = (Func<Gun, Projectile, Projectile>)Delegate.Remove(player.OnPreFireProjectileModifier, new Func<Gun, Projectile, Projectile>(this.HandlePreFireProjectileModification));
            debrisObject.GetComponent<DragunSkull>().m_pickedUpThisRun = true;           
            return debrisObject;
        }

        public Projectile ReplacementProjectile;
       // private int DragunWing = ETGMod.Databases.Items["Dragun Wing"].PickupObjectId;
       // private int DragunClaw = ETGMod.Databases.Items["Dragun Claw"].PickupObjectId;
      //  private int DragunHeart = ETGMod.Databases.Items["Dragun Heart"].PickupObjectId;
        public static PlayerController player;
        
    }
}
