using Gungeon;
using ItemAPI;

namespace Items
{
    class EarthBending : GunBehaviour
    {
        public static void Add()
        {

            Gun gun = ETGMod.Databases.Items.NewGun("Earth Bending", "earth_bending");
            Game.Items.Rename("outdated_gun_mods:earth_bending", "cel:earth_bending");
            gun.gameObject.AddComponent<EarthBending>();
            gun.SetShortDescription("Bang, Bang, Bang! Here We Go!");
            gun.SetLongDescription("");
            gun.SetupSprite(null, "earth_bending_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 24);
            gun.SetAnimationFPS(gun.reloadAnimation, 2);
            gun.AddProjectileModuleFrom("ak-47", true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = .8f;
            gun.gunHandedness = GunHandedness.HiddenOneHanded;
            gun.DefaultModule.angleVariance = 4f;
            gun.DefaultModule.cooldownTime = .5f;
            gun.DefaultModule.numberOfShotsInClip = 12;
            gun.DefaultModule.ammoType = (PickupObjectDatabase.GetById(16) as Gun).DefaultModule.ammoType;
            Gun gun2 = PickupObjectDatabase.GetById(32) as Gun;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.SetBaseMaxAmmo(150);
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            gun.encounterTrackable.EncounterGuid = "pewpewgoboomboomdeath.";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.NONE;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.shouldRotate = true;
            projectile.SetProjectileSpriteRight("firework_proj", 13, 7, null, null);
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
                AkSoundEngine.PostEvent("Play_WPN_rpg_reload_01", base.gameObject);
            }
        }


        public override void OnPostFired(PlayerController player, Gun gun)
        {
            
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_energycannon_shot_01", gameObject);
        }
        public override void OnFinishAttack(PlayerController player, Gun gun)
        {
            base.OnFinishAttack(player, gun);
            
        }


        public EarthBending()
        {

        }
    }
}
