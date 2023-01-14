using Gungeon;

namespace Items
{
    class IVGun : GunBehaviour
    {
        private static Gun IV_gun;



        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("IV Gun", "IV_gun");
            IVGun.IV_gun = gun;
            Game.Items.Rename("outdated_gun_mods:IV_gun", "cel:IV_gun");
            gun.gameObject.AddComponent<IVGun>();

            int Keys = m_owner.carriedConsumables.KeyBullets;
            IVGun.IV_gun.SetBaseMaxAmmo(Keys);

            gun.SetShortDescription("Impressionable");
            gun.SetLongDescription("A gun left unfinished and abandoned by its creator. It still has great potential.");

            gun.SetupSprite(null, "IV_gun_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 24);
            gun.AddProjectileModuleFrom("ak-47");            
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.1f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.RequiresFundsToShoot = true;
            gun.CurrencyCostPerShot = 1;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.numberOfShotsInClip = 6;
            gun.SetBaseMaxAmmo(250);

            gun.quality = PickupObject.ItemQuality.D;
            gun.encounterTrackable.EncounterGuid = "change this for different guns, so the game doesn't think they're the same gun";
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());

        }


  
        public override void PostProcessProjectile(Projectile projectile)
        {
            PlayerController playerController = this.gun.CurrentOwner as PlayerController;
            if (playerController == null)
                this.gun.ammo = this.gun.GetBaseMaxAmmo();

            projectile.baseData.damage *= 1.10f;
            projectile.baseData.speed *= 1f;
           // this.gun.DefaultModule.ammoCost = 1;
            base.PostProcessProjectile(projectile);

            //projectile.gameObject.AddComponent<BasicGunProjectile>(); NOTE THIS FOR LATER!!!!!!!!!

        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", gameObject);
        }



        public IVGun()
        {

        }

        private static PlayerController m_owner;
    }
}
