using Gungeon;
using Alexandria.ItemAPI;
namespace Items
{
    class Günther : AdvancedGunBehaviour
    {
        public static void Add()
        {
            string ShortName = "günther";
            Gun gun = ETGMod.Databases.Items.NewGun("Günther", ShortName);
            Game.Items.Rename("outdated_gun_mods:" + ShortName, "cel:"+ ShortName);
            gun.gameObject.AddComponent<Günther>();
            gun.SetShortDescription("He Tries His Best");
            gun.SetLongDescription("Not as powerful as Gunther, this gun still holds his own. Treat with kindness.");
            gun.SetupSprite(null, ShortName+"_idle_001", 13);
            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 4);
            gun.AddProjectileModuleFrom("38_special");
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;            
            gun.DefaultModule.angleVariance = 4f;
            gun.DefaultModule.cooldownTime = 0.08f;
            gun.SetBaseMaxAmmo(200);
            gun.gunClass = GunClass.PISTOL;
            gun.DefaultModule.numberOfShotsInClip = 5;
            Gun gun2 = PickupObjectDatabase.GetById(56) as Gun;
            gun.gunSwitchGroup = gun2.gunSwitchGroup;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.quality = PickupObject.ItemQuality.C;
            gun.encounterTrackable.EncounterGuid = "Gunther Mimic's adopted son";
            
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 1.5f;
            projectile.baseData.speed *= 1f;
            projectile.transform.parent = gun.barrelOffset;
            gun.sprite.IsPerpendicular = true;
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
        }

        private bool HasReloaded;
        protected override void Update()
        {
            if (gun.CurrentOwner)
            {

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



        public Günther()
        {

        }
    }
}
