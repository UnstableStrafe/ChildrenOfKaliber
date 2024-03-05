using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;
namespace Items
{
    class Phazor : GunBehaviour
    {



        public static void Add()
        {
            int LimitGunId = 98;
            Gun limitGun = PickupObjectDatabase.GetById(LimitGunId) as Gun;



            Gun gun = ETGMod.Databases.Items.NewGun("Phazor", "phazor");
            Game.Items.Rename("outdated_gun_mods:phazor", "ck:phazor");
            gun.gameObject.AddComponent<Phazor>();
            gun.SetShortDescription("RGB Compatible");
            gun.SetLongDescription("Fires faster the longer the fire button is held down.\n\nA gun made from pure rainbows, imagination, hours of slavery, and fantasy.");
            gun.SetupSprite(null, "phazor_idle_001", 12);           
            gun.SetAnimationFPS(gun.shootAnimation, 16);
            gun.SetAnimationFPS(gun.reloadAnimation, 8);
            tk2dSpriteAnimationClip animationclipReload = gun.sprite.spriteAnimator.GetClipByName(gun.reloadAnimation);
            float[] reloadOffsetsX = new float[] { 0.0000f, -0.3125f, -0.3750f, -0.3125f, -0.3750f, -0.3750f, -0.3750f, -0.3750f, 0.0000f, 0.0000f, 0.0000f, 0.0000f };
            float[] reloadOffsetsY = new float[] { -0.1875f, -0.4375f, -0.3750f, -0.4375f, 0.1875f, 0.1875f, 0.1875f, 0.1875f, 0.0000f, -0.3125f, -0.1250f, 0.0000f };

            tk2dSpriteAnimationClip animationclipShoot = gun.sprite.spriteAnimator.GetClipByName(gun.shootAnimation);
            float[] shootOffsetsX = new float[] { -0.0625f, -0.4375f, -0.2500f, -0.0625f, 0.0000f };
            float[] shootOffsetsY = new float[] { 0.0000f, 0.4375f, 0.3125f, 0.0625f, 0.0000f };

            for (int i = 0; i < shootOffsetsX.Length && i < shootOffsetsY.Length && i < animationclipShoot.frames.Length; i++)
            {
                int id = animationclipShoot.frames[i].spriteId;
                Vector3 vector3 = new Vector3(shootOffsetsX[i], shootOffsetsY[i]);
                animationclipShoot.frames[i].spriteCollection.spriteDefinitions[id].position0 += vector3;
                animationclipShoot.frames[i].spriteCollection.spriteDefinitions[id].position1 += vector3;
                animationclipShoot.frames[i].spriteCollection.spriteDefinitions[id].position2 += vector3;
                animationclipShoot.frames[i].spriteCollection.spriteDefinitions[id].position3 += vector3;
            }

            for (int i = 0; i < reloadOffsetsX.Length && i < reloadOffsetsY.Length && i < animationclipReload.frames.Length; i++)
            {
                int id = animationclipReload.frames[i].spriteId;
                Vector3 vector3 = new Vector3(reloadOffsetsX[i], reloadOffsetsY[i]);
                animationclipReload.frames[i].spriteCollection.spriteDefinitions[id].position0 += vector3;
                animationclipReload.frames[i].spriteCollection.spriteDefinitions[id].position1 += vector3;
                animationclipReload.frames[i].spriteCollection.spriteDefinitions[id].position2 += vector3;
                animationclipReload.frames[i].spriteCollection.spriteDefinitions[id].position3 += vector3;
            }
            gun.AddProjectileModuleFrom("future_assault_rifle");
            gun.usesContinuousFireAnimation = false;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.2f;
            gun.DefaultModule.angleVariance = 4f;
            gun.GainsRateOfFireAsContinueAttack = true;
            gun.RateOfFireMultiplierAdditionPerSecond = .17f;
            gun.DefaultModule.cooldownTime = .6f;
            gun.SetBaseMaxAmmo(320);
            Gun gun2 = PickupObjectDatabase.GetById(32) as Gun;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.gunClass = GunClass.PISTOL;
            gun.DefaultModule.numberOfShotsInClip = 16;
            gun.quality = PickupObject.ItemQuality.A;
            gun.encounterTrackable.EncounterGuid = "zip zoop zappity do";
            gun.barrelOffset.localPosition = new Vector3(1.0625f, 0.3125f, 0f);
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 2.28571428571f;
            projectile.baseData.speed *= 1f;
            projectile.SetProjectileSpriteRight("phazor_projectile_002", 21, 3);
            gun.sprite.IsPerpendicular = true;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(13) as Gun).gunSwitchGroup;
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
            projectile.transform.parent = gun.barrelOffset;
        }
        private bool HasReloaded;
        public override void Update()
        {
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


        public override void OnPostFired(PlayerController player, Gun gun)
        {
            base.OnPostFired(player, gun);
            string anim = gun.shootAnimation;
            gun.PlayIfExists(anim, true);
        }




        public Phazor()
        {

        }
    }
}
    

