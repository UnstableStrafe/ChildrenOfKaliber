using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace Items
{
    class Skeleton : GunBehaviour
    {
        
        public static void Add()
        {
            
            Gun gun = ETGMod.Databases.Items.NewGun("Skeleton", "skeleton");
            Game.Items.Rename("outdated_gun_mods:skeleton", "cel:skeleton");
            gun.gameObject.AddComponent<Skeleton>();
            gun.SetShortDescription("Rattled");
            gun.SetLongDescription("It is not unheard of for a mimic to die with its last meal still mid-digestion. How the Gungeon could possible consider this as a gun is best left to the imagination.");
            gun.SetupSprite(null, "skeleton_idle_001", 13);

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.AddProjectileModuleFrom("38_special", true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0f;
            gun.DefaultModule.angleVariance = 5f;
            gun.DefaultModule.cooldownTime = 0.11f;
            gun.SetBaseMaxAmmo(512);
            gun.gunClass = GunClass.SILLY;            
            gun.DefaultModule.numberOfShotsInClip = gun.GetBaseMaxAmmo();
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.quality = PickupObject.ItemQuality.C;
            gun.encounterTrackable.EncounterGuid = "A whole-ass skeleton";
            gun.barrelOffset.transform.localPosition = new UnityEngine.Vector3(.5f, 1.4f, 0f);
            gun.carryPixelOffset = new IntVector2(0, 3);
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 1;
            projectile.baseData.speed *= 1f;
            projectile.transform.parent = gun.barrelOffset;
            projectile.SetProjectileSpriteRight("skeleton_projectile_001", 16, 7, null, null);
            gun.sprite.IsPerpendicular = true;
           
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

            }
        }


        // private int MalRounds = Gungeon.Game.Items["rtr:malediction_rounds"].PickupObjectId;
        public override void OnPostFired(PlayerController player, Gun gun)
        {

            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_skullgun_shot_01", gameObject);
        }



        public Skeleton()
        {

        }
    }
}
