using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace Items
{
    class Slag : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Slag Shotgun", "monogun");
            Game.Items.Rename("outdated_gun_mods:slag_shotgun", "cel:slag_shotgun");
            gun.gameObject.AddComponent<Slag>();
            gun.SetShortDescription("The Old One-Two");
            gun.SetLongDescription("The first shot in each clip hooks enemies. The next fills them with lead. Gives contact damage immunity on pickup.\n\nNothing is more satisfying than a nice and clean combo attack wrapped up in one ragtag shotgun.");
            gun.SetupSprite(null, "monogun_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 4);
            gun.SetAnimationFPS(gun.reloadAnimation, 2);
            gun.AddProjectileModuleFrom("38_special", true, false);
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SHOTGUN;
         //   gun.DefaultModule.customAmmoType = "Shotgun";
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.reloadTime = 2.1f;
            gun.DefaultModule.angleVariance = 0f;
            gun.DefaultModule.usesOptionalFinalProjectile = true;
            gun.DefaultModule.numberOfFinalProjectiles = 1;
            gun.DefaultModule.cooldownTime = .3f;
            gun.DefaultModule.numberOfShotsInClip = 2;
            Gun gun2 = PickupObjectDatabase.GetById(51) as Gun;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.SetBaseMaxAmmo(150);
          //  gun.barrelOffset.transform.localPosition = new Vector3(0f, 0f, 0f);
            gun.quality = PickupObject.ItemQuality.C;
            gun.encounterTrackable.EncounterGuid = "Slag Shotgin.";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.SHOTGUN;

            Gun gun1 = PickupObjectDatabase.GetById(601) as Gun;
            Projectile projectile2 = gun1.projectile;
            gun.DefaultModule.finalProjectile = projectile2;
            projectile2.transform.parent = gun.barrelOffset;
            projectile2.baseData.damage *= 1.2f;
            projectile2.baseData.speed *= 1.2f;
            projectile2.baseData.force *= 4f;


            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage *= 0;
            projectile.baseData.speed *= 2f;
            projectile.baseData.force *= -2f;
            projectile.SetProjectileSpriteRight("hook_proj", 9, 7, null, null);
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            gun.AddToSubShop(ItemBuilder.ShopType.Cursula);
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
                AkSoundEngine.PostEvent("Play_WPN_dl45heavylaser_reload", base.gameObject);
            }
        }


        public override void OnPostFired(PlayerController player, Gun gun)
        {

            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_energycannon_shot_01", gameObject);
        }



        public Slag()
        {

        }
        protected override void OnPickup(PlayerController player)
        {
            base.OnPickup(player);
            player.ReceivesTouchDamage = false;
        }
        protected override void OnPostDrop(PlayerController player)
        {
            base.OnPostDrop(player);
            player.ReceivesTouchDamage = true;
        }
    }
}
