using Gungeon;
using Alexandria.ItemAPI;
namespace Items
{
    class TimeKeepersPistol : GunBehaviour
    {
        
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Time Keeper's Pistol", "time_keeper's_pistol");
            Game.Items.Rename("outdated_gun_mods:time_keeper's_pistol", "cel:time_keeper's_pistol");
            gun.gameObject.AddComponent<TimeKeepersPistol>();

            gun.SetShortDescription("His Last Gift");
            gun.SetLongDescription("A basic pistol. Damage scales with the current loop.\n\nA gun given to you by Grandfather Tick, it will at least keep you safe until you get something better.");
            
            gun.SetupSprite(null, "time_keeper's_pistol_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 16);
            gun.SetAnimationFPS(gun.reloadAnimation, 2);
            gun.AddProjectileModuleFrom("magnum");

            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.2f;
            gun.DefaultModule.angleVariance = 7f;
            gun.DefaultModule.cooldownTime = .2f;
            gun.DefaultModule.numberOfShotsInClip = 6;
            Gun gun2 = PickupObjectDatabase.GetById(38) as Gun;            
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.SetBaseMaxAmmo(1);
            gun.InfiniteAmmo = true;
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            gun.encounterTrackable.EncounterGuid = "Time Keeper's Pistol.";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.PISTOL;
            gun.CanBeDropped = false;
            gun.CanBeSold = false;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            float Cal1 = LoopManager.LoopAMT * .15f;
            float Cal2 = Cal1 + 1f;
            float Cal3 = 6 * Cal2;
            float Cal4 = Cal3 / 13;

            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage *= Cal4;
            projectile.baseData.speed *= 1f;
            projectile.baseData.force *= .33f;
            projectile.baseData.range *= 0.266f;


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
                AkSoundEngine.PostEvent("Play_WPN_SAA_reload_01", base.gameObject);
            }
        }


        public override void OnPostFired(PlayerController player, Gun gun)
        {

            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_magnum_shot_01", gameObject);
        }



        public TimeKeepersPistol()
        {

        }
    }
}
