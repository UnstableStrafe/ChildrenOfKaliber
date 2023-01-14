using Alexandria.ItemAPI;
using Gungeon;
namespace Items
{
    class Spellcaster : AdvancedGunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Spellcaster", "spellcaster"); 
            Game.Items.Rename("outdated_gun_mods:spellcaster", "cel:spellcaster");
            gun.gameObject.AddComponent<Spellcaster>();
            gun.SetShortDescription("Fractal");
            gun.SetLongDescription("Upon entering a new floor, the gun changes between different ammo types.");
            gun.SetupSprite(null, "spellcaster_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 9);
            gun.AddProjectileModuleFrom("38_special");
            gun.SetBaseMaxAmmo(350);
            gun.DefaultModule.ammoCost = 1;
            gun.InfiniteAmmo = true;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = .8f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.gunClass = GunClass.PISTOL;
            gun.DefaultModule.numberOfShotsInClip = 6;
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            Gun gun2 = PickupObjectDatabase.GetById(12) as Gun;
            Gun gun3 = PickupObjectDatabase.GetById(128) as Gun;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.gunSwitchGroup = gun3.gunSwitchGroup;
            gun.gunHandedness = GunHandedness.OneHanded;
            gun.DefaultModule.angleVariance = 4f;
            gun.encounterTrackable.EncounterGuid = "totally not a promo for my WIP game Fractal. Nope. Not at all.";
            gun.sprite.IsPerpendicular = true;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 1.5f;
            projectile.baseData.speed *= 1f;
            projectile.baseData.force *= 1f;
            projectile.transform.parent = gun.barrelOffset;
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

        }


        public Spellcaster()
        {
        }
    }
}
