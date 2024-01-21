using UnityEngine;
using Gungeon;
using Alexandria.ItemAPI;
namespace Items
{
    class DEATHDrone : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("d.e.a.t.h._drone", "d.e.a.t.h._drone");
            Game.Items.Rename("outdated_gun_mods:d.e.a.t.h._drone", "ck:d.e.a.t.h._drone");
            gun.gameObject.AddComponent<DEATHDrone>();
            gun.SetShortDescription("fswwawdw");
            gun.SetLongDescription("wsdfeesadwdasdwda.");
            gun.SetupSprite(null, "d.e.a.t.h._drone_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 24);
            gun.SetAnimationFPS(gun.reloadAnimation, 10);
            gun.AddProjectileModuleFrom("future_assault_rifle");
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = .6f;
            gun.DefaultModule.angleVariance = 4f;
            gun.DefaultModule.cooldownTime = .11f;
            gun.DefaultModule.numberOfShotsInClip = 25;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(229) as Gun).gunSwitchGroup;
            Gun gun2 = PickupObjectDatabase.GetById(151) as Gun;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.SetBaseMaxAmmo(500);
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            gun.encounterTrackable.EncounterGuid = "why the fuck am i even typing this shit out any more? Nobody will read this.";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.FULLAUTO;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage *= .75f;
            projectile.baseData.speed *= 1f;
            projectile.baseData.force *= 1f;
            projectile.HasDefaultTint = true;
            projectile.DefaultTintColor = new Color(10 / 150f, 152 / 150f, 216 / 150f);
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
        }


        private bool HasReloaded;

        public override void Update()
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



        }

        public DEATHDrone()
        {

        }
    }
}
