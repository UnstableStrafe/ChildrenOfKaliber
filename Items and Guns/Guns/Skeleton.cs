using Gungeon;
using Alexandria.ItemAPI;

namespace Items
{
    class Skeleton : AdvancedGunBehaviour
    {
        public static int itemID;
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Skeleton", "skeleton");
            Game.Items.Rename("outdated_gun_mods:skeleton", "ck:skeleton");
            gun.gameObject.AddComponent<Skeleton>();
            gun.SetShortDescription("Rattled");
            gun.SetLongDescription("It is not unheard of for a mimic to die with its last meal still mid-digestion. How the Gungeon could possibly consider this as a gun is best left to the imagination.");
            gun.SetupSprite(null, "skeleton_idle_001", 13);
            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.AddProjectileModuleFrom("38_special");
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0f;
            gun.DefaultModule.angleVariance = 5f;
            gun.DefaultModule.cooldownTime = 0.11f;
            gun.SetBaseMaxAmmo(512);
            gun.gunClass = GunClass.SILLY;            
            gun.DefaultModule.numberOfShotsInClip = gun.GetBaseMaxAmmo();
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.quality = PickupObject.ItemQuality.C;
            gun.encounterTrackable.EncounterGuid = "A whole-ass skeleton";
            gun.barrelOffset.transform.localPosition = new UnityEngine.Vector3(.5f, 1.4f, 0f);
            gun.carryPixelOffset = new IntVector2(0, 3);
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 1;
            projectile.baseData.speed *= 1f;            
            projectile.transform.parent = gun.barrelOffset;
            projectile.SetProjectileSpriteRight("skeleton_projectile_001", 16, 7);
            gun.sprite.IsPerpendicular = true;
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(45) as Gun).gunSwitchGroup;
            itemID = gun.PickupObjectId;
        }

        private bool HasReloaded;

        public override Projectile OnPreFireProjectileModifier(Gun gun, Projectile projectile, ProjectileModule mod)
        {

            if(gun.CurrentOwner is PlayerController)
            {
                PlayerController player = gun.CurrentOwner as PlayerController;
                if (player.PlayerHasActiveSynergy("S H A K E N"))
                {
                    Projectile proj = (PickupObjectDatabase.GetById(406) as Gun).Volley.projectiles[1].projectiles[0];
                    proj.baseData.damage *= 2.5f;
                    return proj;
                }
            }

            return projectile;
        }
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

       



        public Skeleton()
        {

        }
    }
}
