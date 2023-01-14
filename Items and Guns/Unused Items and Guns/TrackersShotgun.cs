using System;
using Gungeon;


namespace Items
{
    class TrackersShotgun : GunBehaviour
    {
        public static void Add()
        {
            

            Gun gun = ETGMod.Databases.Items.NewGun("Tracker's Shotgun", "tracker's_shotgun");
            Game.Items.Rename("outdated_gun_mods:tracker's_shotgun", "cel:tracker's_shotgun");
            gun.gameObject.AddComponent<TrackersShotgun>();
            gun.SetShortDescription("Finds its Mark");
            gun.SetLongDescription("The Tracker's Shotgun was brought to the Gungeon by a vengeful bounty hunter.\n\nOld and worn, it is no stranger to combat. It has never failed its current companion.");
            gun.SetupSprite(null, "tracker's_shotgun_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 8);
            gun.SetAnimationFPS(gun.reloadAnimation, 1);
            gun.AddProjectileModuleFrom("shotgun");
            gun.Volley = (PickupObjectDatabase.GetById(202) as Gun).Volley;
            gun.singleModule = (PickupObjectDatabase.GetById(202) as Gun).singleModule;
            gun.RawSourceVolley = (PickupObjectDatabase.GetById(202) as Gun).RawSourceVolley;
            gun.alternateVolley = (PickupObjectDatabase.GetById(202) as Gun).alternateVolley;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.sprite.IsPerpendicular = true;
            //  gun.PreventOutlines = true;
            gun.reloadTime = 1.2f;
        //    gun.DefaultModule.numberOfShotsInClip = 6;
           // gun.DefaultModule.cooldownTime = .4f;
            gun.gunClass = GunClass.SHOTGUN;
            gun.InfiniteAmmo = true;
            gun.ammo = 150;
            gun.SetBaseMaxAmmo(150);
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            gun.CanBeDropped = false;
            gun.CanBeSold = false;
            gun.CanGainAmmo = false;
            gun.carryPixelOffset = new IntVector2(7, 0);
            
            Guid.NewGuid().ToString();
           
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
        }

        public override void PostProcessProjectile(Projectile projectile)
        {
            PlayerController playerController = this.gun.CurrentOwner as PlayerController;
            if (playerController == null)
                this.gun.ammo = this.gun.GetBaseMaxAmmo();

            projectile.baseData.damage *= 0.5f;
            projectile.baseData.speed *= 1f;
            projectile.baseData.range *= 1.20f;
            this.gun.DefaultModule.ammoCost = 1;
            base.PostProcessProjectile(projectile);


            //projectile.gameObject.AddComponent<VindicationProjectile>();

        }
     
        public override void OnPostFired(PlayerController player, Gun gun)
        {

            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_shotgun_shot_01", gameObject);
        }

        public TrackersShotgun()
        {

        }
    }
}
