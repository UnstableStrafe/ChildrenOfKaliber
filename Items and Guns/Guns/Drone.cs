using Gungeon;
using Alexandria.ItemAPI;

namespace Items
{
    class Drone : GunBehaviour
    {
        public static int Id;

        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Drone", "drone");
            Game.Items.Rename("outdated_gun_mods:drone", "ck:drone");
            gun.gameObject.AddComponent<Drone>();
            gun.SetShortDescription("zeep zoop");
            gun.SetLongDescription("why are you reading this?.");
            gun.SetupSprite(null, "drone_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 15);
            gun.AddProjectileModuleFrom("plasma_rifle");
            gun.SetBaseMaxAmmo(69);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(229) as Gun).gunSwitchGroup;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SMALL_BULLET;
            gun.reloadTime = 0f;
            gun.DefaultModule.cooldownTime = .11f;
            gun.gunHandedness = GunHandedness.HiddenOneHanded;
            gun.DefaultModule.numberOfShotsInClip = 420;
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(229) as Gun).muzzleFlashEffects;
            gun.DefaultModule.angleVariance = 0f;
            gun.encounterTrackable.EncounterGuid = "quack quack nerd";
            gun.sprite.IsPerpendicular = true;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= .625f;
            projectile.transform.parent = gun.barrelOffset;
            projectile.sprite.renderer.enabled = true;

            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());

            Id = gun.PickupObjectId;
        }

        private bool HasReloaded;

        public override void Update()
        {
            PlayerController player = gun.CurrentOwner as PlayerController;
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


        public Drone()
        {
        }
    }
}
