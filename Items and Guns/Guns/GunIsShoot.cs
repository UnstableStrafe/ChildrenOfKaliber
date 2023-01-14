using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;
using System.Collections;
using Dungeonator;
using System;

namespace Items
{
    class GunIsShoot : AdvancedGunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("GUN is SHOOT", "gun_is_shoot");
            Game.Items.Rename("outdated_gun_mods:gun_is_shoot", "cel:gun_is_shoot");
            gun.gameObject.AddComponent<GunIsShoot>();
            gun.SetShortDescription("BULLET is DEFEAT");
            gun.SetLongDescription("The gun has 3 charge stages: the first picks a noun, the second picks a conjuction, and the third picks either a noun or a property. Reloading the gun fires projectiles made from the combination of the queued words, e.g. \"BULLET is HOT\"" +
                "\n\nGUN is SHOOT\nPROJECTILE is HURT\nBULLET is DEFEAT\nYOU is WIN");
            gun.SetupSprite(null, "gun_is_shoot_idle_001", 4);
            gun.SetAnimationFPS(gun.shootAnimation, 24);
            gun.SetAnimationFPS(gun.reloadAnimation, 9);
            gun.AddProjectileModuleFrom("38_special");
            gun.SetBaseMaxAmmo(180);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.2f;
            gun.DefaultModule.cooldownTime = 0.25f;
            gun.gunClass = GunClass.SILLY;
            gun.PreventOutlines = true;
            gun.DefaultModule.numberOfShotsInClip = 9;
            gun.quality = PickupObject.ItemQuality.B;
            Gun gun2 = PickupObjectDatabase.GetById(12) as Gun;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.DefaultModule.angleVariance = 0f;
            gun.encounterTrackable.EncounterGuid = "LEVEL too HARD / BABA is DONE / BETTER look OUT / BABA has GUN";
            gun.sprite.IsPerpendicular = true;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 1.5f;
            projectile.baseData.speed *= 2f;
            projectile.baseData.force *= 1f;
            projectile.transform.parent = gun.barrelOffset;
            projectile.SetProjectileSpriteRight("rebar_projectile", 11, 3);

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
                AkSoundEngine.PostEvent("Play_WPN_crossbow_reload_01", base.gameObject);
            }
        }



        public override void OnPostFired(PlayerController player, Gun gun)
        {

            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_crossbow_shot_01", gameObject);
        }


        public GunIsShoot()
        {
        }
    }
}
