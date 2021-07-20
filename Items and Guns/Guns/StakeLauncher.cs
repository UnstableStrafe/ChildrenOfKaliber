using Gungeon;
using ItemAPI;


namespace Items
{
    class StakeLauncher : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Stake Launcher", "stake_launcher");
            Game.Items.Rename("outdated_gun_mods:stake_launcher", "cel:stake_launcher");
            gun.gameObject.AddComponent<StakeLauncher>();
            gun.SetShortDescription("Pumpking's Bane");
            gun.SetLongDescription("Deals less damage to non-jammed enemies.\n\nThe stake is a staple weapon in combating unholy creatures. These bolts have been modified to affect the Jammed Gundead.");
            gun.SetupSprite(null, "stake_launcher_idle_001", 13);

            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.AddProjectileModuleFrom("crossbow", true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = .7f;
            gun.DefaultModule.angleVariance = 4f;
            gun.DefaultModule.cooldownTime = 0.5f;
            gun.SetBaseMaxAmmo(150);
            gun.gunClass = GunClass.PISTOL;
            gun.DefaultModule.numberOfShotsInClip = 5;
            Gun gun2 = PickupObjectDatabase.GetById(12) as Gun;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.quality = PickupObject.ItemQuality.D;
            gun.encounterTrackable.EncounterGuid = "stake launcher";

            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 0.18182f;
            projectile.baseData.speed *= 1f;
            projectile.transform.parent = gun.barrelOffset;
            projectile.SetProjectileSpriteRight("stake_projectile_001", 10, 5, null, null);
            gun.sprite.IsPerpendicular = true;
            ETGMod.Databases.Items.Add(gun, null, "ANY");


        }

        private bool HasReloaded;
        public override void PostProcessProjectile(Projectile projectile)
        {
            base.PostProcessProjectile(projectile);
            projectile.BlackPhantomDamageMultiplier *= 7.5f;
        }
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
                AkSoundEngine.PostEvent("Play_WPN_crossbow_reload_01", base.gameObject);
            }
        }


        // private int MalRounds = Gungeon.Game.Items["rtr:malediction_rounds"].PickupObjectId;
        public override void OnPostFired(PlayerController player, Gun gun)
        {

            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_crossbow_shot_01", gameObject);
        }



        public StakeLauncher()
        {

        }
    }
}
