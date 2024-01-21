
using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;

namespace Items
{
    class Fallout : AdvancedGunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Fallout", "fallout");
            Game.Items.Rename("outdated_gun_mods:fallout", "ck:fallout");
            gun.gameObject.AddComponent<Fallout>();
            gun.SetShortDescription("Keep Out Of Reach Of Children");
            gun.SetLongDescription("Shoots powerful blasts of nuclear energy.\n\n This gun is powered by pure poisonium (not to be confused with the harmless substance, toxicium) which is lethal to any creature in the known universe.");

            gun.SetupSprite(null,"fallout_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 6);
            gun.AddProjectileModuleFrom("38_special");
            tk2dSpriteAnimationClip animationclipReload = gun.sprite.spriteAnimator.GetClipByName(gun.reloadAnimation);
            float[] reloadOffsetsX = new float[] { 0.0000f, -0.0625f, -0.1250f, -0.1250f, -0.1250f, -0.1250f, -0.1250f, -0.1250f, -0.1250f, -0.1250f, -0.1250f, -0.1250f, -0.1250f, -0.1250f, 0.0000f };
            float[] reloadOffsetsY = new float[] { 0.0000f, -0.2500f, -0.3125f, -0.3125f, -0.3125f, -0.3125f, -0.3125f, -0.3125f, -0.3125f, -0.3125f, -0.3125f, -0.3125f, -0.3125f, -0.3125f, 0.0000f };

            tk2dSpriteAnimationClip animationclipShoot = gun.sprite.spriteAnimator.GetClipByName(gun.shootAnimation);
            float[] shootOffsetsX = new float[] { 0.0000f, -0.4375f, -3.0000f, -2.8750f, 0.3125f, 0.3750f, -0.0625f, -0.0625f, -0.0625f, -0.0625f, 0.0000f, -0.0625f, 0.0000f, -0.0625f };
            float[] shootOffsetsY = new float[] { 0.0000f, 0.0000f, 1.8125f, 1.7500f, 0.1875f, -1.3125f, -3.0000f, -3.0000f, -2.2500f, -2.2500f, -1.2500f, -0.9375f, 0.0625f, 0.2500f };

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
            
            gun.usesContinuousFireAnimation = false;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.4f;
            gun.DefaultModule.customAmmoType = "poison_blob";
            gun.DefaultModule.angleVariance = 0f;
            gun.DefaultModule.cooldownTime = .75f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.PreventNormalFireAudio = false;
            Gun gun2 = PickupObjectDatabase.GetById(151) as Gun;
            Gun gun3 = PickupObjectDatabase.GetById(508) as Gun;
            gun.gunSwitchGroup = gun3.gunSwitchGroup;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.SetBaseMaxAmmo(150);
            gun.barrelOffset.localPosition = new Vector3(7.25f, 0.625f, 0f);
            gun.quality = PickupObject.ItemQuality.A;
            gun.encounterTrackable.EncounterGuid = "For his nuetral special, gun wields a Joker.";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.RIFLE;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage *= 16;
            projectile.baseData.speed *= 5f;
            projectile.baseData.force *= 4f;
            projectile.baseData.range *= 1000;
            projectile.SetProjectileSpriteRight("fallout_smol_projectile_001", 31, 8);
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
            gun.AddToSubShop(ItemBuilder.ShopType.Goopton);
            gun.AddToSubShop(ItemBuilder.ShopType.Trorc);

            
        }

        private bool HasReloaded;

        protected override void Update()
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
                AkSoundEngine.PostEvent("Play_WPN_dl45heavylaser_reload", base.gameObject);
            }
        }

       
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            
          
        }



        public Fallout()
        {

        }
    }
}
