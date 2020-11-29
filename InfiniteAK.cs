using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace Items
{
    class InfiniteAK : AdvancedGunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Infinite AK", "infinite_ak");
            Game.Items.Rename("outdated_gun_mods:infinite_ak", "cel:infinite_ak");
            gun.gameObject.AddComponent<InfiniteAK>();
            gun.SetShortDescription("Witness Perfection");
            gun.SetLongDescription("Perfect, brilliant. This gun might not be the strongest around, but it is the most refined. Every aspect of it is flawless. Each bullet it shoots is symmetrical and expertly crafted. Be grateful to witness such beauty.");
            gun.SetupSprite(null, "infinite_ak_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 9);
            gun.SetAnimationFPS(gun.reloadAnimation, 10);
            gun.AddProjectileModuleFrom("ak-47", true, false);
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SMALL_BULLET;
            gun.DefaultModule.ammoCost = 0;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = -1f;
            gun.CanBeDropped = false;
            gun.CanBeSold = false;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.DefaultModule.angleVariance = 0f;
            gun.InfiniteAmmo = true;
            gun.DefaultModule.cooldownTime = .1f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(15) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(15) as Gun).muzzleFlashEffects;
            gun.SetBaseMaxAmmo(2000);
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            gun.encounterTrackable.EncounterGuid = "the condition, state, or quality of being free or as free as possible from all flaws or defects.";
            gun.sprite.IsPerpendicular = true;
            gun.barrelOffset.transform.localPosition = new Vector3(2.25f, 0.3125f, 0f);
            gun.gunClass = GunClass.FULLAUTO;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage *= 0;
            projectile.baseData.speed *= 1;
            projectile.baseData.force *= 0;
            projectile.baseData.range = 0;
            projectile.hitEffects.suppressMidairDeathVfx = true;
            projectile.sprite.renderer.enabled = false;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }
        private bool HasReloaded;

        protected override void Update()
        {
            base.Update();
            if (gun.CurrentOwner)
            {

                if (gun.PreventNormalFireAudio)
                {
                    this.gun.PreventNormalFireAudio = true;
                }
                if (!gun.IsReloading && !HasReloaded)
                {
                    this.HasReloaded = true;
                }
            }
        }
        public override void PostProcessProjectile(Projectile projectile)
        {
            base.PostProcessProjectile(projectile);
            PlayerController x = this.gun.CurrentOwner as PlayerController;
            bool flag = x == null;
            bool flag2 = flag;
            if (flag2)
            {
                this.gun.ammo = this.gun.GetBaseMaxAmmo();
            }
            this.gun.ClipShotsRemaining = 2;
            this.gun.GainAmmo(2);
        }
        protected override void OnPickedUpByPlayer(PlayerController player)
        {
            base.OnPickedUpByPlayer(player);          
            Gun gun = ETGMod.Databases.Items["cel:ak_188"] as Gun;
            if (player.HasGun(gun.PickupObjectId))
            {
                player.inventory.DestroyGun(gun);
            }
        }        
        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && this.HasReloaded)
            {
                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
            }
        }
        float v1 = 0;
        float v2 = 90;
        float v3 = 180;
        float v4 = 270;
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            base.OnPostFired(player, gun);
            gun.ClearReloadData();
            v1 += UnityEngine.Random.Range(5, 16);
            v2 += UnityEngine.Random.Range(5, 16);
            v3 += UnityEngine.Random.Range(5, 16);
            v4 += UnityEngine.Random.Range(5, 16);
            Projectile projectile2 = ((Gun)ETGMod.Databases.Items[15]).DefaultModule.projectiles[0];
            GameObject gameObject2 = SpawnManager.SpawnProjectile(projectile2.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle + v1), true);
            Projectile component2 = gameObject2.GetComponent<Projectile>();
            if (component2 != null)
            {
                component2.Owner = base.Owner;
                component2.Shooter = base.Owner.specRigidbody;
                component2.baseData.speed *= player.stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed);
                component2.baseData.force *= player.stats.GetStatValue(PlayerStats.StatType.KnockbackMultiplier);
                component2.baseData.damage *= player.stats.GetStatValue(PlayerStats.StatType.Damage);
                component2.ignoreDamageCaps = true;
            }

            Projectile projectile3 = ((Gun)ETGMod.Databases.Items[15]).DefaultModule.projectiles[0];
            GameObject gameObject3 = SpawnManager.SpawnProjectile(projectile3.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle + v2), true);
            Projectile component3 = gameObject3.GetComponent<Projectile>();
            if (component3 != null)
            {
                component3.Owner = base.Owner;
                component3.Shooter = base.Owner.specRigidbody;
                component3.baseData.speed *= player.stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed);
                component3.baseData.force *= player.stats.GetStatValue(PlayerStats.StatType.KnockbackMultiplier);
                component3.baseData.damage *= player.stats.GetStatValue(PlayerStats.StatType.Damage);
                component3.ignoreDamageCaps = true;
            }
            Projectile projectile4 = ((Gun)ETGMod.Databases.Items[15]).DefaultModule.projectiles[0];
            GameObject gameObject4 = SpawnManager.SpawnProjectile(projectile4.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle + v3), true);
            Projectile component4 = gameObject4.GetComponent<Projectile>();
            if (component4 != null)
            {
                component4.Owner = base.Owner;
                component4.Shooter = base.Owner.specRigidbody;
                component4.baseData.speed *= player.stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed);
                component4.baseData.force *= player.stats.GetStatValue(PlayerStats.StatType.KnockbackMultiplier);
                component4.baseData.damage *= player.stats.GetStatValue(PlayerStats.StatType.Damage);
                component4.ignoreDamageCaps = true;
            }
            Projectile projectile5 = ((Gun)ETGMod.Databases.Items[15]).DefaultModule.projectiles[0];
            GameObject gameObject5 = SpawnManager.SpawnProjectile(projectile5.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle + v4), true);
            Projectile component5 = gameObject5.GetComponent<Projectile>();
            if (component5 != null)
            {
                component5.Owner = base.Owner;
                component5.Shooter = base.Owner.specRigidbody;
                component5.baseData.speed *= player.stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed);
                component5.baseData.force *= player.stats.GetStatValue(PlayerStats.StatType.KnockbackMultiplier);
                component5.baseData.damage *= player.stats.GetStatValue(PlayerStats.StatType.Damage);
                component5.ignoreDamageCaps = true;
            }
        }

        public InfiniteAK()
        {

        }
    }
}
