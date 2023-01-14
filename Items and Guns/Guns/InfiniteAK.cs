using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

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
            for (int i = 0; i < 1; i++)
            {
                GunExt.AddProjectileModuleFrom(gun, "ak-47");
            }
            foreach (ProjectileModule projectileModule in gun.Volley.projectiles)
            {
                projectileModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
                Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(projectileModule.projectiles[0]);
                projectile.gameObject.SetActive(false);
                FakePrefab.MarkAsFakePrefab(projectile.gameObject);
                UnityEngine.Object.DontDestroyOnLoad(projectile);
                projectileModule.projectiles[0] = projectile;
                projectileModule.angleVariance = 4;
                projectile.transform.parent = gun.barrelOffset;
                gun.DefaultModule.projectiles[0] = projectile;
                projectile.baseData.damage *= 1.5f;
                projectile.baseData.speed *= 2f;
                projectileModule.numberOfShotsInClip = 1;
                projectileModule.cooldownTime = .08f;
                projectile.ignoreDamageCaps = true;
                projectileModule.ammoType = GameUIAmmoType.AmmoType.SMALL_BULLET;
                List<string> frameNames = new List<string> 
                {
                    "infinity_projectile_001",
                    "infinity_projectile_002",
                    "infinity_projectile_003",
                    "infinity_projectile_004",
                };
                projectile.AnimateProjectile(frameNames, 10, true, Library.ConstructListOfSameValues(new IntVector2(13, 13), 4), Library.ConstructListOfSameValues(true, 4), Library.ConstructListOfSameValues(tk2dBaseSprite.Anchor.LowerLeft, 4), Library.ConstructListOfSameValues(false, 4), Library.ConstructListOfSameValues(false, 4), Library.ConstructListOfSameValues<Vector3?>(null, 4), Library.ConstructListOfSameValues<IntVector2?>(null, 4), Library.ConstructListOfSameValues<IntVector2?>(null, 4), Library.ConstructListOfSameValues<Projectile>(null, 4));
                projectile.shouldRotate = false;
                bool flag = projectileModule == gun.DefaultModule;
                if (flag)
                {
                    projectileModule.ammoCost = 1;
                }
                else
                {
                    projectileModule.ammoCost = 0;
                }

            }

            gun.reloadTime = -1f;
            gun.CanBeDropped = false;
            gun.CanBeSold = false;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.InfiniteAmmo = true;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(15) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(15) as Gun).muzzleFlashEffects;
            gun.SetBaseMaxAmmo(2000);
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            gun.encounterTrackable.EncounterGuid = "the condition, state, or quality of being free or as free as possible from all flaws or defects.";
            gun.sprite.IsPerpendicular = true;
            gun.barrelOffset.transform.localPosition = new Vector3(2.25f, 0.3125f, 0f);
            gun.gunClass = GunClass.FULLAUTO;
            
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
            InfiniteAK.AKINFID = gun.PickupObjectId;
        }
        public static int AKINFID;
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

        }
        protected override void OnPickup(GameActor owner)
        {
            base.OnPickup(owner);

          
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

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            base.OnPostFired(player, gun);
            PlayerController x = this.gun.CurrentOwner as PlayerController;
            bool flag = x == null;
            bool flag2 = flag;
            if (flag2)
            {
                this.gun.ammo = this.gun.GetBaseMaxAmmo();
            }
            this.gun.ClipShotsRemaining = 2;
            this.gun.GainAmmo(2);
            gun.ClearReloadData();

        }

        public InfiniteAK()
        {

        }
    }
}
