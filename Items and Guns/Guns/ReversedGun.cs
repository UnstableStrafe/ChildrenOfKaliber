using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using Gungeon;

namespace Items
{
    class ReversedGun : AdvancedGunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Reversed Gun", "reversed_gun");
            Game.Items.Rename("outdated_gun_mods:reversed_gun", "cel:reversed_gun");
            gun.gameObject.AddComponent<ReversedGun>();
            gun.SetShortDescription("Not How It's Supposed To Work");
            gun.SetLongDescription("Fires in reverse.\n\nYet another example of how time in The Gungeon is less like a linear path and more like a jumbled mess of chaos.");
            gun.SetupSprite(null, "reversed_gun_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 4);
            gun.SetAnimationFPS(gun.reloadAnimation, 2);
            gun.AddProjectileModuleFrom("38_special");

            tk2dSpriteAnimationClip shootClip = gun.sprite.spriteAnimator.GetClipByName(gun.shootAnimation);
            float[] offsetsX = new float[] { 0.0000f, 0.1250f, -0.1875f, -0.5000f, -0.5000f, -0.1875f };
            float[] offsetsY = new float[] { 0.0000f, 0.0000f, 0.0000f, 0.0625f, 0.0625f, 0.0000f };
            for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < shootClip.frames.Length; i++)
            {
                int id = shootClip.frames[i].spriteId; shootClip.frames[i].spriteCollection.spriteDefinitions[id].position0.x += offsetsX[i]; shootClip.frames[i].spriteCollection.spriteDefinitions[id].position0.y += offsetsY[i]; shootClip.frames[i].spriteCollection.spriteDefinitions[id].position1.x += offsetsX[i]; shootClip.frames[i].spriteCollection.spriteDefinitions[id].position1.y += offsetsY[i]; shootClip.frames[i].spriteCollection.spriteDefinitions[id].position2.x += offsetsX[i]; shootClip.frames[i].spriteCollection.spriteDefinitions[id].position2.y += offsetsY[i]; shootClip.frames[i].spriteCollection.spriteDefinitions[id].position3.x += offsetsX[i]; shootClip.frames[i].spriteCollection.spriteDefinitions[id].position3.y += offsetsY[i];
            }

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.MEDIUM_BULLET;
            gun.DefaultModule.ammoCost = -1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0f;
            gun.barrelOffset.transform.localPosition = new Vector3(1f, .5f, 0f);
            gun.DefaultModule.angleVariance = 0f;
            gun.DefaultModule.cooldownTime = .15f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.SetBaseMaxAmmo(400);
            gun.ammo = 1;
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            gun.encounterTrackable.EncounterGuid = "i have no idea why i am even fucking making this";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.SILLY;           
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 1;
            projectile.baseData.speed *= 1f;
            projectile.baseData.force *= 1f;
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
        }

        private bool HasReloaded;



        protected override void Update()
        {
            if (gun.CurrentOwner)
            {
                if (!gun.PreventNormalFireAudio)
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
            if (gun.ammo != 0)
            {
                gun.ClipShotsRemaining = 1;
                gun.ClearReloadData();
            }
        }



        public ReversedGun()
        {

        }
    }
}
