using Gungeon;

namespace Items
{
    class ScrapGun1 : GunBehaviour
    {
        public static void Add()
        {
 
            Gun gun = ETGMod.Databases.Items.NewGun("Scrap Gun", "scrap_gun");
            Game.Items.Rename("outdated_gun_mods:Scrap_gun", "cel:Scrap_gun");
            gun.gameObject.AddComponent<ScrapGun1>();
            gun.SetShortDescription("Seen Better Days");
            gun.SetLongDescription("Upgrades with Junk collected. \n\n Some heartless fool bought this gun, then decided to abandon it a day later. " +
                "Left alone for eons, the Gungeon's magic keeping it barely functional, this gun has only known sorrow.");

            gun.SetupSprite(null, "scrap_gun_idle_001", 8);
            // ETGMod automatically checks which animations are available.
            // The numbers next to "shootAnimation" determine the animation fps. You can also tweak the animation fps of the reload animation and idle animation using this method.
            gun.SetAnimationFPS(gun.shootAnimation, 24);
            // Every modded gun has base projectile it works with that is borrowed from other guns in the game. 
            // The gun names are the names from the JSON dump! While most are the same, some guns named completely different things. If you need help finding gun names, ask a modder on the Gungeon discord.
            gun.AddProjectileModuleFrom("ak-47", true, false);
            // Here we just take the default projectile module and change its settings how we want it to be.
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.1f;
            gun.DefaultModule.cooldownTime = 0.3f;
            gun.DefaultModule.numberOfShotsInClip = 6;
            gun.SetBaseMaxAmmo(250);
            // Here we just set the quality of the gun and the "EncounterGuid", which is used by Gungeon to identify the gun.
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            gun.encounterTrackable.EncounterGuid = "According to all known laws of aviation, there is no way a bee should be able to fly.";
            ETGMod.Databases.Items.Add(gun, null, "ANY");

        }


        public override void PostProcessProjectile(Projectile projectile)
        {
            PlayerController playerController = this.gun.CurrentOwner as PlayerController;
            if (playerController == null)
                this.gun.ammo = this.gun.GetBaseMaxAmmo();
            //projectile.baseData allows you to modify the base properties of your projectile module.
            //In our case, our gun uses modified projectiles from the ak-47.
            //Setting static values for a custom gun's projectile stats prevents them from scaling with player stats and bullet modifiers (damage, shotspeed, knockback)
            //You have to multiply the value of the original projectile you're using instead so they scale accordingly. For example if the projectile you're using as a base has 10 damage and you want it to be 6 you use this
            //In our case, our projectile has a base damage of 5.5, so we multiply it by 1.1 so it does 10% more damage from the ak-47.
            projectile.baseData.damage *= 1.09090909091f;
            projectile.baseData.speed *= 0.65217391304f;
            projectile.baseData.force *= 1f;
            this.gun.DefaultModule.ammoCost = 1;
            base.PostProcessProjectile(projectile);

            //projectile.gameObject.AddComponent<BasicGunProjectile>(); NOTE THIS FOR LATER!!!!!!!!!
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {

            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_rustysidearm_shot_01", gameObject);
        }



        public ScrapGun1()
        {

        }
    }
}
