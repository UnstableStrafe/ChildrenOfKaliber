using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;
using Gungeon;

namespace Items
{
    class NineBarreledGun : AdvancedGunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Nine-Barreled Shotgun", "nine-barreled_shotgun");
            Game.Items.Rename("outdated_gun_mods:nine-barreled_shotgun", "cel:nine-barreled_shotgun");
            gun.gameObject.AddComponent<NineBarreledGun>();
            gun.SetShortDescription("Slightly Overkill");
            gun.SetLongDescription("Do I even need to say what it does?");
            gun.SetupSprite(null, "nine-barreled_shotgun_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 7);
            gun.SetAnimationFPS(gun.reloadAnimation, 5);
            for (int i = 0; i < 36; i++)
            {
                GunExt.AddProjectileModuleFrom(gun, (PickupObjectDatabase.GetById(82) as Gun), true, false);
            }
            foreach (ProjectileModule projectileModule in gun.Volley.projectiles)
            {
                projectileModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
                Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(projectileModule.projectiles[0]);
                projectile.gameObject.SetActive(false);
                FakePrefab.MarkAsFakePrefab(projectile.gameObject);
                UnityEngine.Object.DontDestroyOnLoad(projectile);
                projectileModule.projectiles[0] = projectile;
                projectileModule.angleVariance = 50;
                projectile.transform.parent = gun.barrelOffset;
                projectile.baseData.damage *= .5f;
                projectile.baseData.speed *= 1f;
                projectileModule.numberOfShotsInClip = 1;
                projectileModule.cooldownTime = .4f;
                projectile.baseData.range *= 1f;
                projectileModule.ammoType = GameUIAmmoType.AmmoType.SHOTGUN;
                
                bool flag = projectileModule == gun.DefaultModule;
                if (flag)
                {
                    projectileModule.ammoCost = 9;
                }
                else
                {
                    projectileModule.ammoCost = 0;
                }

            }                        
            gun.reloadTime = 2.8f;
          
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(82) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(84) as Gun).muzzleFlashEffects;
            gun.SetBaseMaxAmmo(540);
           
            gun.quality = PickupObject.ItemQuality.B;
            gun.encounterTrackable.EncounterGuid = "oh lord o fuck what have i fucking done shit o fuck";
            gun.sprite.IsPerpendicular = true;
            gun.barrelOffset.transform.localPosition = new Vector3(2.1875f, 1.4375f, 0f);
            gun.gunClass = GunClass.SHOTGUN;
            
            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }
        private bool HasReloaded;
        protected override void Update()
        {
            base.Update();
            if (gun.CurrentOwner)
            {               
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
        
        public NineBarreledGun()
        {

        }
    }
}
