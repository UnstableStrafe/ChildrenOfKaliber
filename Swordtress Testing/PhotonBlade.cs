using System.Collections;
using System.Collections.Generic;
using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;

namespace Items
{
    class PhotonBlade : AdvancedGunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Photon Blade", "Photon Blade");
            Game.Items.Rename("outdated_gun_mods:photon_blade", "sts:photon_blade");
            gun.gameObject.AddComponent<PhotonBlade>();
            gun.SetShortDescription("Not Just The Children...");
            gun.SetLongDescription("Slices with deadly power.");
            gun.SetupSprite(null, "photon_blade_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 10);
            gun.SetAnimationFPS(gun.reloadAnimation, 2);
            gun.AddProjectileModuleFrom("38_special");
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SMALL_BULLET;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0f;
            gun.DefaultModule.angleVariance = 0f;
            gun.DefaultModule.cooldownTime = .5f;
            gun.DefaultModule.numberOfShotsInClip = 350;
            Gun gun2 = PickupObjectDatabase.GetById(151) as Gun;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.SetBaseMaxAmmo(350);
            gun.barrelOffset.transform.localPosition = new Vector3(1.5f, 0f, 0f);
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            gun.encounterTrackable.EncounterGuid = "zwoooop.";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.NONE;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage *= 3f;
            projectile.baseData.speed = 0;
            ProjectileSlashingBehaviour slashingBehaviour = projectile.gameObject.AddComponent<ProjectileSlashingBehaviour>();
            slashingBehaviour.SlashDimensions = 90;
            slashingBehaviour.SlashRange = 2.5f;
            //VFXPool SlashVFX = VFXLibrary.CreateMuzzleflash("photon_blade_slice", new List<string> { "katanaslice_001", "katanaslice_002", "katanaslice_003", }, 10, new List<IntVector2> { new IntVector2(72, 67), new IntVector2(72, 67), new IntVector2(72, 67), }, new List<tk2dBaseSprite.Anchor> {
            //    tk2dBaseSprite.Anchor.MiddleLeft, tk2dBaseSprite.Anchor.MiddleLeft, tk2dBaseSprite.Anchor.MiddleLeft}, new List<Vector2> { Vector2.zero, Vector2.zero, Vector2.zero }, false, false, false, false, 0, VFXAlignment.Fixed, true, new List<float> { 0, 0, 0 }, new List<Color> { VFXLibrary.emptyColor, VFXLibrary.emptyColor, VFXLibrary.emptyColor });
            //slashingBehaviour.SlashVFX = SlashVFX;
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



        public PhotonBlade()
        {

        }
    }
}
