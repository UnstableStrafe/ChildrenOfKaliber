using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace Items
{
    class SpiritOfTheDragun : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Spirit Of The Dragun", "spirit_of_the_dragun");
            Game.Items.Rename("outdated_gun_mods:spirit_of_the_dragun", "cel:spirit_of_the_dragun");
            gun.gameObject.AddComponent<SpiritOfTheDragun>();
            gun.SetShortDescription("Ego Sum Aeternae");
            gun.SetLongDescription("Shoots powerful, molten, armor-piercing shots shots.\n\n\"Perhaps you will succeed where I have failed.\"");
            gun.SetupSprite(null, "spirit_of_the_dragun_idle_001", 13);
            gun.SetAnimationFPS(gun.shootAnimation, 10);
            gun.SetAnimationFPS(gun.reloadAnimation, 10);
            gun.AddProjectileModuleFrom("ak-47", true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 2f;
            gun.DefaultModule.angleVariance = 4f;
            gun.DefaultModule.cooldownTime = .3f;
            gun.DefaultModule.burstShotCount = 3;
            gun.DefaultModule.burstCooldownTime = .1f;
            gun.SetBaseMaxAmmo(213);
            gun.gunClass = GunClass.FULLAUTO;
            gun.DefaultModule.numberOfShotsInClip = 45;           
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            Gun gun2 = PickupObjectDatabase.GetById(15) as Gun;
            Gun gun3 = PickupObjectDatabase.GetById(705) as Gun;            
            gun.InfiniteAmmo = true;
            gun.barrelOffset.transform.localPosition = new Vector3(2.5f, .5f, 0f);
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.encounterTrackable.EncounterGuid = "Easily the strongest gun ive made.";
            Projectile originalProjectile = (PickupObjectDatabase.GetById(370) as Gun).DefaultModule.chargeProjectiles[1].Projectile;
            Projectile projectile = UnityEngine.Object.Instantiate(originalProjectile);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 0.15f;
            projectile.baseData.speed *= .25f;
            PierceProjModifier orAddComponent = projectile.gameObject.GetOrAddComponent<PierceProjModifier>();
            orAddComponent.penetration -= 50;
            projectile.GetComponent<TrailController>();
            projectile.transform.parent = gun.barrelOffset;
            projectile.ignoreDamageCaps = true;
            gun.sprite.IsPerpendicular = true;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }
        private bool HasReloaded;
        protected void Update()
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

        public SpiritOfTheDragun()
        {

        }
    }
}
