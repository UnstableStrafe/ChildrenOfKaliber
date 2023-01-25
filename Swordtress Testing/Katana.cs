using System.Collections;
using System.Collections.Generic;
using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;
using Alexandria.Misc;

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
            gun.AddProjectileModuleFrom("38_special");
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

            Projectile projectile = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();
            Projectile projectile2 = gun.DefaultModule.projectiles[0].InstantiateAndFakeprefab();

            projectile.baseData.damage *= 3f;
            projectile.baseData.speed = 0;

            ProjectileSlashingBehaviour slashingBehaviour = projectile.gameObject.AddComponent<ProjectileSlashingBehaviour>();
            SlashData slashInfo = ScriptableObject.CreateInstance<SlashData>();
            slashInfo.slashDegrees = 135;
            slashInfo.slashRange = 5f;
            slashingBehaviour.initialDelay = .1f;
            slashingBehaviour.slashParameters = slashInfo;

            projectile2.baseData.damage *= 3f;
            projectile2.baseData.speed = 0;

            ProjectileSlashingBehaviour slashingBehaviour2 = projectile2.gameObject.AddComponent<ProjectileSlashingBehaviour>();
            SlashData slashInfo2 = ScriptableObject.CreateInstance<SlashData>();
            slashInfo2.slashRange = 5f; 
            slashInfo2.slashDegrees = 135f;
            slashingBehaviour2.slashParameters = slashInfo2;
            slashingBehaviour2.angleVariance = 8f;
            slashingBehaviour2.timeBetweenSlashes = 0.2f;
            slashingBehaviour2.timeBetweenCustomSequenceSlashes = 0.2f;
            slashingBehaviour2.customSequence = new List<float>() { 0, 0, 0, 0, 0 };

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
            //VFXPool SlashVFX = VFXLibrary.CreateMuzzleflash("katanaslice", new List<string> { "katanaslice_001", "katanaslice_002", "katanaslice_003",}, 10, new List<IntVector2> { new IntVector2(72, 67), new IntVector2(72, 67), new IntVector2(72, 67), }, new List<tk2dBaseSprite.Anchor> {
            //    tk2dBaseSprite.Anchor.MiddleLeft, tk2dBaseSprite.Anchor.MiddleLeft, tk2dBaseSprite.Anchor.MiddleLeft}, new List<Vector2> { Vector2.zero, Vector2.zero, Vector2.zero}, false, false, false, false, 0, VFXAlignment.Fixed, true, new List<float> { 0, 0, 0}, new List<Color> { VFXLibrary.emptyColor, VFXLibrary.emptyColor, VFXLibrary.emptyColor});
            //slashingBehaviour.SlashVFX = SlashVFX;
            //slashingBehaviour2.SlashVFX = SlashVFX;
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
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
    }
}
