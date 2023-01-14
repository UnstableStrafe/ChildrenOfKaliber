using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;

namespace Items
{
    class Hitscanner : AdvancedGunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Hitscanner", "hitscanner");
            Game.Items.Rename("outdated_gun_mods:hitscanner", "cel:hitscanner");
            gun.gameObject.AddComponent<Hitscanner>();
            gun.SetShortDescription("Click");
            gun.SetLongDescription("test");
            gun.SetupSprite(null, "hitscanner_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 24);
            gun.SetAnimationFPS(gun.reloadAnimation, 9);
            gun.AddProjectileModuleFrom("38_special");
            gun.SetBaseMaxAmmo(250);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 2.2f;
            gun.DefaultModule.cooldownTime = 0.25f;
            gun.gunClass = GunClass.RIFLE;
            gun.DefaultModule.numberOfShotsInClip = 5;
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            Gun gun2 = PickupObjectDatabase.GetById(12) as Gun;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.DefaultModule.angleVariance = 0f;
            gun.encounterTrackable.EncounterGuid = "why did i decid to make a hitscan gun in gungeon? tell me when you find the answer to that, please.";
            gun.sprite.IsPerpendicular = true;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 8f; //ill use this later
            projectile.baseData.speed *= 0f; //this isnt needed since the projectile is destroyed when i shoot
            projectile.baseData.force *= 0f;
            projectile.transform.parent = gun.barrelOffset;
            projectile.sprite.renderer.enabled = false;
            projectile.hitEffects.suppressMidairDeathVfx = true;
           
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
        }

        public override void PostProcessProjectile(Projectile projectile)
        {
            base.PostProcessProjectile(projectile);
            Destroy(projectile.gameObject);
            
            if(gun.CurrentOwner.gameObject.GetComponent<PlayerController>())
            {
                PlayerController player = gun.CurrentOwner.gameObject.GetComponent<PlayerController>();

                RaycastHit2D ray = Physics2D.Raycast(player.CurrentGun.barrelOffset.transform.position, player.CurrentGun.CurrentAngle.DegreeToVector2(), Mathf.Infinity);

                if (ray.transform)
                {
                    Transform t = ray.transform;
                    if(t.gameObject.GetComponent<AIActor>() && t.gameObject.GetComponent<HealthHaver>())
                    {
                        ETGModConsole.Log("gottem");
                    }
                }
            }
            
        }

        private bool HasReloaded;

        protected override void Update()
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

        }


        public Hitscanner()
        {
        }
    }
}
