using Gungeon;
using Alexandria.ItemAPI;

namespace Items
{
    class Phazor : GunBehaviour
    {



        public static void Add()
        {
            int LimitGunId = 98;
            Gun limitGun = PickupObjectDatabase.GetById(LimitGunId) as Gun;



            Gun gun = ETGMod.Databases.Items.NewGun("Phazor", "phazor");
            Game.Items.Rename("outdated_gun_mods:phazor", "cel:phazor");
            gun.gameObject.AddComponent<Phazor>();
            gun.SetShortDescription("Shiny");
            gun.SetLongDescription("Fires faster the longer the fire button is held down.\n\nA gun made from pure rainbows, imagination, hours of slavery, and fantasy.");
            gun.SetupSprite(null, "phazor_idle_001", 13);
           
            gun.SetAnimationFPS(gun.shootAnimation, 24);
            gun.AddProjectileModuleFrom("future_assault_rifle");
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0f;
            gun.DefaultModule.angleVariance = 4f;
            gun.GainsRateOfFireAsContinueAttack = true;
            gun.RateOfFireMultiplierAdditionPerSecond = .5f;
            gun.DefaultModule.cooldownTime = 0.2f;
            gun.SetBaseMaxAmmo(300);
            Gun gun2 = PickupObjectDatabase.GetById(32) as Gun;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.gunClass = GunClass.PISTOL;
            gun.DefaultModule.numberOfShotsInClip = gun.GetBaseMaxAmmo();
            gun.quality = PickupObject.ItemQuality.A;
            gun.encounterTrackable.EncounterGuid = "zip zoop zappity do";

            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 2.28571428571f;
            projectile.baseData.speed *= 1f;
            projectile.SetProjectileSpriteRight("phazor_projectile_002", 21, 3);
            gun.sprite.IsPerpendicular = true;
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
            projectile.transform.parent = gun.barrelOffset;
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

            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_zapper_shot_01", gameObject);
        }

        

        public Phazor()
        {

        }
    }
}
    

