using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Gungeon;
using ItemAPI;
using MonoMod;
using MonoMod.RuntimeDetour;
using Random = UnityEngine.Random;
using UnityEngine;

namespace Items
{
    class Tesla : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Tesla", "tesla");
            Game.Items.Rename("outdated_gun_mods:tesla", "cel:tesla");
            gun.gameObject.AddComponent<Tesla>();
            gun.SetShortDescription("Static Fields");
            gun.SetLongDescription("Shoots bolts of energy\n\n Kinda tastes metallic.\n\n\n (Disclaimer: Cel does NOT endorse licking a tesla coil or any form of electricty generators in real life.)");

            gun.SetupSprite(null, "tesla_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 24);

            gun.AddProjectileModuleFrom("electric_rifle", true, false);
            // Here we just take the default projectile module and change its settings how we want it to be.
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0f;
            gun.DefaultModule.burstShotCount = 5;
            gun.DefaultModule.burstCooldownTime = .1f;
            gun.DefaultModule.cooldownTime = .8f;
            gun.DefaultModule.numberOfShotsInClip = 500;
            gun.gunClass = GunClass.FULLAUTO;
            gun.gunHandedness = GunHandedness.HiddenOneHanded;
            Gun gun2 = PickupObjectDatabase.GetById(153) as Gun;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.SetBaseMaxAmmo(500);
            gun.sprite.IsPerpendicular = true;
            gun.quality = PickupObject.ItemQuality.S;
            gun.encounterTrackable.EncounterGuid = "Please do not lick electricty. k thanks.";
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 1.25f;
            projectile.baseData.speed *= 1f;
            projectile.transform.parent = gun.barrelOffset;

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
                
            }
        }


        public override void OnPostFired(PlayerController player, Gun gun)
        {

            gun.PreventNormalFireAudio = true;
           AkSoundEngine.PostEvent("Play_ENV_puddle_zap_01", gameObject);
        }



        public Tesla()
        {

        }
    }
}
