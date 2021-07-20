using Gungeon;

namespace Items
{
    class H_Gun : GunBehaviour
    {
        private static Gun GunCost; 
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("H-Gun", "h-gun");
            H_Gun.GunCost = gun;
            H_Gun.GunCost.RequiresFundsToShoot = true;
            H_Gun.GunCost.CurrencyCostPerShot = 1;

            Game.Items.Rename("outdated_gun_mods:h-gun", "cel:h-gun");
            gun.gameObject.AddComponent<H_Gun>();
            gun.SetShortDescription("Keep Out of Reach of Children");
            gun.SetLongDescription("WIP.");

            gun.SetupSprite(null, "fallout_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 4);
            gun.SetAnimationFPS(gun.reloadAnimation, 2);
            gun.AddProjectileModuleFrom("ak-47", true, false);

            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.angleVariance = 0f;
            gun.DefaultModule.cooldownTime = .2f;
            gun.RequiresFundsToShoot = H_Gun.GunCost.RequiresFundsToShoot;
            gun.CurrencyCostPerShot = H_Gun.GunCost.CurrencyCostPerShot;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.SetBaseMaxAmmo(150);

            gun.quality = PickupObject.ItemQuality.SPECIAL;
            gun.encounterTrackable.EncounterGuid = "eivdkswdgdgvsbdgdsgdsgcvascsakvkvdvs.";
            ETGMod.Databases.Items.Add(gun, null, "ANY");

        }

        private static void Cost(ProjectileModule module)
        {
            if(H_Gun.GunCost.RequiresFundsToShoot == true)
            {
                
            }
        }

        public override void PostProcessProjectile(Projectile projectile)
        {
            PlayerController playerController = this.gun.CurrentOwner as PlayerController;
            if (playerController == null)
                this.gun.ammo = this.gun.GetBaseMaxAmmo();

            projectile.baseData.damage *= 5.45454545455f;
            projectile.baseData.speed *= 2f;
            this.gun.DefaultModule.ammoCost = 1;
            base.PostProcessProjectile(projectile);


       

        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {

            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_energycannon_shot_01", gameObject);
        }



        private static GameActor m_owner;
        public H_Gun()
        {

        }
    }
}
