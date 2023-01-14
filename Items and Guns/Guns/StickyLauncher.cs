using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using Gungeon;

namespace Items
{
    class StickyLauncher : AdvancedGunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Sticky Launcher", "sticky_launcher");
            Game.Items.Rename("outdated_gun_mods:sticky_launcher", "cel:sticky_launcher");
            gun.SetShortDescription("KA-BLOOIE!");
            gun.SetLongDescription("Shoots sticky bombs that detonate on reloading.\n\nA weathered sticky bomb launcher with the words \"Mann Co. Property\" written on the side.");
            gun.SetupSprite(null, "sticky_launcher_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 16);
            gun.SetAnimationFPS(gun.reloadAnimation, 2);
            gun.AddProjectileModuleFrom("38_special");
            gun.gameObject.AddComponent<StickyLauncher>();
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.MEDIUM_BULLET;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.2f;
            gun.DefaultModule.angleVariance = 4f;
            gun.DefaultModule.cooldownTime = .45f;
            gun.DefaultModule.numberOfShotsInClip = 8;
            Gun gun2 = PickupObjectDatabase.GetById(56) as Gun;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.SetBaseMaxAmmo(320);
            gun.quality = PickupObject.ItemQuality.C;
            gun.encounterTrackable.EncounterGuid = "What makes me a good demoman? If I were a bad demoman, I wouldn't be sittin' here discussin' it with you, now would I?! LET'S DO IT! Not one of you's gonna survive this! One crossed wire, one wayward pinch of potassium chlorate, one errant twitch, and KA-BLOOIE! nd I got a manky eye. I'm a black Scottish cyclops. They got more fecking sea monsters in the great Lochett Ness than they got the likes of me. So! T'all you fine dandies, so proud, so cocksure, prancin' about with your heads full of eyeballs... come and get me, I say! I'll be waitin' on you with a whiff of the old brimstone! I'm a Grimm bloody fable with an unhappy bloody end! Oh, they're going to have to glue you back together...IN HELL!";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.PISTOL;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage *= 0;
            projectile.SetProjectileSpriteRight("sticky_bomb", 11, 11);
            StickyProjectile sticky = projectile.gameObject.AddComponent<StickyProjectile>();
            sticky.explosionData = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultSmallExplosionData;
            sticky.explosionData.damageToPlayer = 0;
            sticky.explosionData.damage = 15f;
            sticky.destroyOnGunChanged = true;
            sticky.shouldExplodeOnReload = true;
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
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
            AkSoundEngine.PostEvent("Play_WPN_skullgun_shot_01", gameObject);
        }

        public StickyLauncher()
        {

        }
    }
}
