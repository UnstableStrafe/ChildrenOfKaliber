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
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0f;
            gun.DefaultModule.burstShotCount = 5;
            gun.DefaultModule.burstCooldownTime = .1f;
            gun.DefaultModule.cooldownTime = .8f;
            gun.DefaultModule.numberOfShotsInClip = 500;
            gun.DefaultModule.angleVariance = 5;
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
        private PlayerController lastOwner = null;
        private DamageTypeModifier electricityImmunity = null;
        protected void Update()
        {
            PlayerController player = gun.CurrentOwner as PlayerController;
            if (gun.CurrentOwner)
            {
                bool flag5 = this.gun.CurrentOwner != null && this.gun.CurrentOwner is PlayerController;
                if (flag5)
                {
                    this.lastOwner = (this.gun.CurrentOwner as PlayerController);
                    bool flag6 = this.electricityImmunity == null;
                    if (flag6)
                    {
                        this.electricityImmunity = new DamageTypeModifier
                        {
                            damageMultiplier = 0f,
                            damageType = CoreDamageTypes.Electric
                        };
                        this.lastOwner.healthHaver.damageTypeModifiers.Add(this.electricityImmunity);
                    }
                }
                else
                {
                    bool flag7 = this.electricityImmunity != null;
                    if (flag7)
                    {
                        this.lastOwner.healthHaver.damageTypeModifiers.Remove(this.electricityImmunity);
                        this.electricityImmunity = null;
                    }
                }
                
                
                if (!gun.IsReloading && !HasReloaded)
                {
                    this.HasReloaded = true;
                }
                
                if (gun.PreventNormalFireAudio)
                {
                    this.gun.PreventNormalFireAudio = true;
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
