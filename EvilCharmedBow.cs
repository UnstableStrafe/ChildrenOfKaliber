using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gungeon;
using ItemAPI;
namespace Items
{
    class EvilCharmedBow : AdvancedGunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Evil Charmed Bow", "evil_charmed_bow");
            Game.Items.Rename("outdated_gun_mods:evil_charmed_bow", "cel:evil_charmed_bow");
            gun.gameObject.AddComponent<EvilCharmedBow>();
            gun.SetShortDescription("Click");
            gun.SetLongDescription("filler text.");
            gun.SetupSprite(null, "evil_charmed_bow_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 6);
            gun.AddProjectileModuleFrom("crossbow", true, false);
            gun.SetBaseMaxAmmo(180);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.2f;
            gun.DefaultModule.cooldownTime = 0.25f;
            gun.gunClass = GunClass.RIFLE;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            Gun gun2 = PickupObjectDatabase.GetById(12) as Gun;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.gunHandedness = GunHandedness.OneHanded;
            gun.DefaultModule.angleVariance = 0f;
            gun.encounterTrackable.EncounterGuid = "evil charmed bow";
            gun.sprite.IsPerpendicular = true;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 1.10f;
            projectile.baseData.speed *= 2f;
            projectile.baseData.force *= 1f;
            projectile.AppliesPoison = true;
            projectile.PoisonApplyChance = .33f;
            projectile.healthEffect = Library.Venom;
            projectile.transform.parent = gun.barrelOffset;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
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


        public EvilCharmedBow()
        {
        }
    }
}
