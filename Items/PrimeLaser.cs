using System;
using System.Collections;
using Gungeon;
using ItemAPI;
using MonoMod;
using UnityEngine;

namespace Items
{
    class PrimeLaser : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Prime Laser", "prime_laser");
            Game.Items.Rename("outdated_gun_mods:prime_laser", "cel:prime_laser");
            gun.gameObject.AddComponent<PrimeLaser>();
            gun.SetShortDescription("Why are you reading this?");
            gun.SetLongDescription("Loser.");
            gun.SetupSprite(null, "prime_laser_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 15);
            gun.AddProjectileModuleFrom("future_assault_rifle", true, false);
            gun.SetBaseMaxAmmo(1000);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SMALL_BULLET;
            gun.reloadTime = 0f;
            gun.DefaultModule.cooldownTime = .1f;
            gun.gunHandedness = GunHandedness.OneHanded;
            gun.DefaultModule.numberOfShotsInClip = 1000;
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            Gun gun2 = PickupObjectDatabase.GetById(32) as Gun;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.barrelOffset.transform.localPosition += new Vector3(0, -.1f, 0);
            gun.DefaultModule.angleVariance = 0f;
            gun.encounterTrackable.EncounterGuid = "Prime Laser";
            gun.sprite.IsPerpendicular = true;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            Projectile proj2 = (PickupObjectDatabase.GetById(724) as Gun).DefaultModule.projectiles[0];
            gun.DefaultModule.projectiles[0] = proj2;
            proj2.baseData.damage *= 1f;
            proj2.transform.parent = gun.barrelOffset;
            proj2.AppliesFire = false;
            BounceProjModifier bounce = proj2.gameObject.GetComponent<BounceProjModifier>();
            bounce.numberOfBounces -= 1;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }

        private bool HasReloaded;

        protected void Update()
        {
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

        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && this.HasReloaded)
            {
                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_WPN_crossbow_reload_01", base.gameObject);
            }
        }



        public override void OnPostFired(PlayerController player, Gun gun)
        {

            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_crossbow_shot_01", gameObject);
        }


        public PrimeLaser()
        {
        }
    }
}
