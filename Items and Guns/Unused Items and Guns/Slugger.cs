using Gungeon;

namespace Items
{
    class Slugger : GunBehaviour
    {
        public static void Add()
        {
          
            Gun gun = ETGMod.Databases.Items.NewGun("Slugger", "slugger");
            
            
            Game.Items.Rename("outdated_gun_mods:slugger", "cel:slugger");
            gun.gameObject.AddComponent<Slugger>();

            gun.SetShortDescription("You're Freakin' Dead Kiddo");
            gun.SetLongDescription("Fires a burst of shotgun shots.");

            gun.SetupSprite(null, "slugger_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 24);


            gun.AddProjectileModuleFrom("shotgun", true, false);

            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
            gun.gunClass = GunClass.SHOTGUN;
            
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.DefaultModule.angleVariance = 11f;
            gun.reloadTime = 2.1f;    
            gun.DefaultModule.burstShotCount = 4;
            gun.DefaultModule.burstCooldownTime = .1f;
            gun.DefaultModule.cooldownTime = 0.8f;
            gun.DefaultModule.numberOfShotsInClip = 4;
            gun.SetBaseMaxAmmo(120);
            gun.Volley.UsesShotgunStyleVelocityRandomizer = true;
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            gun.encounterTrackable.EncounterGuid = "What the fuck did you just fucking say about me, you little bitch? I'll have you know I graduated top of my class in the Navy Seals, and I've been involved in numerous secret raids on Al-Quaeda, and I have over 300 confirmed kills. I am trained in gorilla warfare and I'm the top sniper in the entire US armed forces. You are nothing to me but just another target. I will wipe you the fuck out with precision the likes of which has never been seen before on this Earth, mark my fucking words. You think you can get away with saying that shit to me over the Internet? Think again, fucker. As we speak I am contacting my secret network of spies across the USA and your IP is being traced right now so you better prepare for the storm, maggot. The storm that wipes out the pathetic little thing you call your life. You're fucking dead, kid. I can be anywhere, anytime, and I can kill you in over seven hundred ways, and that's just with my bare hands. Not only am I extensively trained in unarmed combat, but I have access to the entire arsenal of the United States Marine Corps and I will use it to its full extent to wipe your miserable ass off the face of the continent, you little shit. If only you could have known what unholy retribution your little 'clever' comment was about to bring down upon you, maybe you would have held your fucking tongue. But you couldn't, you didn't, and now you're paying the price, you goddamn idiot. I will shit fury all over you and you will drown in it. You're fucking dead, kiddo.";
            ETGMod.Databases.Items.Add(gun, null, "ANY");

        }

         Gun gilded = ETGMod.Databases.Items["gilded_hydra"] as Gun;




        // This determines what the projectile does when it fires.
        public override void PostProcessProjectile(Projectile projectile)
        {
            PlayerController playerController = this.gun.CurrentOwner as PlayerController;
            if (playerController == null)
                this.gun.ammo = this.gun.GetBaseMaxAmmo();
            
            projectile.baseData.damage *= 1.10f;
            projectile.baseData.speed *= 1f;
            this.gun.DefaultModule.ammoCost = 1;
            base.PostProcessProjectile(projectile);


            //projectile.gameObject.AddComponent<BasicGunProjectile>(); NOTE THIS FOR LATER!!!!!!!!!

        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = false;

        }


        
        public Slugger()
        {
          
        }
    }


}
