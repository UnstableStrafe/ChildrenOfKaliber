using Gungeon;
using Alexandria.ItemAPI;

namespace Items
{
    public class Dispenser : GunBehaviour
    {
        public static int DispenserID;
        //ITEM IDEA --- CROWN OF THE CHOSEN or smth like it. Gives certain effects for different people. Special effects for each modder, Turtle, Reto, Hutts, etc. Name changes depeding on who has it. Other ppl will prob have a random effect or smth idk good luck tomorrow me AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Dispenser", "dispenser");
            Game.Items.Rename("outdated_gun_mods:dispenser", "ck:dispenser");
            gun.gameObject.AddComponent<Dispenser>();
            gun.SetShortDescription("Click");
            gun.SetLongDescription("Originally designed for basic home security, thousands of engineers have discovered obscure uses for the humble dispenser.");
            gun.SetupSprite(null, "dispenser_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 24);
            gun.SetAnimationFPS(gun.reloadAnimation, 9);
            gun.AddProjectileModuleFrom("crossbow");
            gun.SetBaseMaxAmmo(180);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.2f;
            gun.DefaultModule.cooldownTime = 0.25f;
            gun.gunClass = GunClass.PISTOL;
            gun.DefaultModule.numberOfShotsInClip = 9;
            gun.quality = PickupObject.ItemQuality.B;
            Gun gun2 = PickupObjectDatabase.GetById(12) as Gun;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.DefaultModule.angleVariance = 0f;
            gun.encounterTrackable.EncounterGuid = "dispenser";
            gun.sprite.IsPerpendicular = true;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(12) as Gun).gunSwitchGroup;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= .75f;
            projectile.baseData.speed *= .769f;
            projectile.baseData.force *= .6f;
            projectile.transform.parent = gun.barrelOffset;
            projectile.SetProjectileSpriteRight("dispenser_projectile_001", 11, 3);
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
            DispenserID = gun.PickupObjectId;
        }

        private bool HasReloaded;

        public override void Update()
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
                AkSoundEngine.PostEvent("Play_WPN_crossbow_reload_01", base.gameObject);
            }
        }

       

        


        public Dispenser()
        {
        }
    }
}