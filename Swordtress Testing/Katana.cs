using System.Collections;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace Items
{
    class Katana : AdvancedGunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Ancient Katana", "ancient_katana");
            Game.Items.Rename("outdated_gun_mods:ancient_katana", "sts:ancient_katana");
            gun.gameObject.AddComponent<Katana>();
            gun.SetShortDescription("Blade Breathing, 1st Form");
            gun.SetLongDescription("Can be charged to release a flurry of attacks.");
            gun.SetupSprite(null, "ancient_katana_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 2);
            gun.AddProjectileModuleFrom("38_special", true, false);
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SMALL_BULLET;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0f;
            gun.DefaultModule.angleVariance = 0f;
            gun.DefaultModule.cooldownTime = .5f;
            gun.DefaultModule.numberOfShotsInClip = 350;
            Gun gun2 = PickupObjectDatabase.GetById(151) as Gun;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.SetBaseMaxAmmo(350);
            gun.barrelOffset.transform.localPosition = new Vector3(2.5f, 0f, 0f);
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            gun.encounterTrackable.EncounterGuid = "wht the fuck did this break MTG AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAaAAAA.";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.NONE;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            Projectile projectile2 = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage *= 3f;
            projectile.baseData.speed = 0;
            ProjectileSlashingBehaviour slashingBehaviour = projectile.gameObject.AddComponent<ProjectileSlashingBehaviour>();
            slashingBehaviour.SlashDimensions = 135;
            slashingBehaviour.SlashRange = 5f;
            slashingBehaviour.delayBeforeSlash = .1f;
            

            projectile2.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile2.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile2);
            projectile2.transform.parent = gun.barrelOffset;
            projectile2.baseData.damage *= 3f;
            projectile2.baseData.speed = 0;
            ProjectileSlashingBehaviour slashingBehaviour2 = projectile2.gameObject.AddComponent<ProjectileSlashingBehaviour>();
            slashingBehaviour2.SlashDimensions = 135;
            slashingBehaviour2.SlashRange = 5f;
            slashingBehaviour2.delayBeforeSlash = .2f;
            slashingBehaviour2.DoesMultipleSlashes = true;
            slashingBehaviour2.AmountOfMultiSlashes = 5;
            slashingBehaviour2.DelayBetweenMultiSlashes = .2f;
            slashingBehaviour2.UsesAngleVariance = true;
            slashingBehaviour2.MinSlashAngleOffset = -8;
            slashingBehaviour2.MaxSlashAngleOffset = 8;
            ProjectileModule.ChargeProjectile chargeProjectile1 = new ProjectileModule.ChargeProjectile()
            {
                Projectile = projectile,
                ChargeTime = 0
            };
            ProjectileModule.ChargeProjectile chargeProjectile2 = new ProjectileModule.ChargeProjectile()
            {
                Projectile = projectile2,
                ChargeTime = .7f
            };
            gun.DefaultModule.chargeProjectiles = new List<ProjectileModule.ChargeProjectile>
            {
                chargeProjectile1,
                chargeProjectile2
               
            };
            VFXPool SlashVFX = VFXLibrary.CreateMuzzleflash("katanaslice", new List<string> { "katanaslice_001", "katanaslice_002", "katanaslice_003",}, 10, new List<IntVector2> { new IntVector2(72, 67), new IntVector2(72, 67), new IntVector2(72, 67), }, new List<tk2dBaseSprite.Anchor> {
                tk2dBaseSprite.Anchor.MiddleLeft, tk2dBaseSprite.Anchor.MiddleLeft, tk2dBaseSprite.Anchor.MiddleLeft}, new List<Vector2> { Vector2.zero, Vector2.zero, Vector2.zero}, false, false, false, false, 0, VFXAlignment.Fixed, true, new List<float> { 0, 0, 0}, new List<Color> { VFXLibrary.emptyColor, VFXLibrary.emptyColor, VFXLibrary.emptyColor});
            slashingBehaviour.SlashVFX = SlashVFX;
            slashingBehaviour2.SlashVFX = SlashVFX;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            gun.AddToSubShop(ItemBuilder.ShopType.Goopton);
            gun.AddToSubShop(ItemBuilder.ShopType.Trorc);
            
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
            gun.PreventNormalFireAudio = true;
        }



        public Katana()
        {

        }
    }
}
