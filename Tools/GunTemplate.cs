using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Items
{
    class GunTemplate
    {
        /*
         public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Name", "sprite_name");
            Game.Items.Rename("outdated_gun_mods:short_name", "prefix:short_name");
            gun.gameObject.AddComponent<>();
            gun.SetShortDescription("");
            gun.SetLongDescription("");
            gun.SetupSprite(null, "sprite_name_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 10);
            gun.SetAnimationFPS(gun.reloadAnimation, 2);
            gun.AddProjectileModuleFrom("38_special", true, false);
            
           // tk2dSpriteAnimationClip shootClip = gun.sprite.spriteAnimator.GetClipByName(gun.shootAnimation);
           // float[] offsetsX = new float[] { 0.5625f, 2.6250f, 2.6250f, 2.6250f, 2.6250f, 2.6250f, 2.6875f, 2.6875f };
           // float[] offsetsY = new float[] { 0.4375f, 0.4375f, -1.7500f, -1.7500f, -1.7500f, -1.7500f, -0.9375f, 0.0000f };
           // for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < shootClip.frames.Length; i++)
           // {
           //     int id = shootClip.frames[i].spriteId; shootClip.frames[i].spriteCollection.spriteDefinitions[id].position0.x += offsetsX[i]; shootClip.frames[i].spriteCollection.spriteDefinitions[id].position0.y += offsetsY[i]; shootClip.frames[i].spriteCollection.spriteDefinitions[id].position1.x += offsetsX[i]; shootClip.frames[i].spriteCollection.spriteDefinitions[id].position1.y += offsetsY[i]; shootClip.frames[i].spriteCollection.spriteDefinitions[id].position2.x += offsetsX[i]; shootClip.frames[i].spriteCollection.spriteDefinitions[id].position2.y += offsetsY[i]; shootClip.frames[i].spriteCollection.spriteDefinitions[id].position3.x += offsetsX[i]; shootClip.frames[i].spriteCollection.spriteDefinitions[id].position3.y += offsetsY[i];
           // }

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "white";
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0f;
            gun.barrelOffset.transform.localPosition = new Vector3(0f, 0f, 0f);
            gun.DefaultModule.angleVariance = 0f;
            gun.DefaultModule.cooldownTime = .15f;
            gun.DefaultModule.numberOfShotsInClip = 1;
             gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.SetBaseMaxAmmo(480);
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            gun.encounterTrackable.EncounterGuid = "";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.SILLY;
           // gun.AddPassiveStatModifier(PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE); // for melees/cursed guns
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
           
            projectile.baseData.damage *= 1;
            projectile.baseData.speed *= 1f;
            projectile.baseData.force *= 1f;
            projectile.SetProjectileSpriteRight("projectile_sprite", 9, 5, null, null); // if using a custom projectile sprite
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


       // public override void OnPostFired(PlayerController player, Gun gun) //for if you want the gun to "insta reload" each time its shot
       // {
       //     if(gun.ammo != 0)
       //     {
       //         gun.ClipShotsRemaining = 1;
       //         gun.ClearReloadData();
       //     }   
       // }



        public PanicPistol()
        {

        }
        */

    }
}
