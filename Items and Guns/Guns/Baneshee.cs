using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Alexandria.ItemAPI;
using Gungeon;
using Dungeonator;
using UnityEngine;

namespace Items
{
    class Baneshee : AdvancedGunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Baneshee", "baneshee");
            Game.Items.Rename("outdated_gun_mods:baneshee", "ck:baneshee");
            gun.gameObject.AddComponent<Baneshee>();
            gun.SetShortDescription("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            gun.SetLongDescription("A powerful gun with an equally devastating curse.");
            gun.SetupSprite(null, "baneshee_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 18);
            gun.SetAnimationFPS(gun.reloadAnimation, 9);
            gun.AddProjectileModuleFrom("ak-47");
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).frames[0].eventAudio = "Play_baneshee_reload";
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).frames[0].triggerEvent = true;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.introAnimation).frames[0].eventAudio = "Play_baneshee_swap";
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.introAnimation).frames[0].triggerEvent = true;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[0].eventAudio = "Play_baneshee_fire_001";
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[0].triggerEvent = true;
            gun.SetBaseMaxAmmo(500);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 2.4f;
            gun.gunHandedness = GunHandedness.AutoDetect;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.gunClass = GunClass.FULLAUTO;
            gun.DefaultModule.numberOfShotsInClip = 25;
            gun.quality = PickupObject.ItemQuality.B;
            Gun gun2 = PickupObjectDatabase.GetById(15) as Gun;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.DefaultModule.angleVariance = 4f;
            gun.encounterTrackable.EncounterGuid = "I SCREAM! YOU SCREAM! WE ALL SCREAM FOR THE SCARLET KING!";
            gun.sprite.IsPerpendicular = true;
            gun.barrelOffset.transform.localPosition = new Vector3(1.1875f, 0.4375f, 0f);
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.MEDIUM_BULLET;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 2.2f;
            projectile.baseData.speed *= 1.5f;
            projectile.baseData.force *= 1f;
            projectile.shouldRotate = false;
            projectile.transform.parent = gun.barrelOffset;
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
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
                
            }
        }

      

        public override void OnPostFired(PlayerController player, Gun gun)
        {


        }


        public Baneshee()
        {
        }
    }
}
