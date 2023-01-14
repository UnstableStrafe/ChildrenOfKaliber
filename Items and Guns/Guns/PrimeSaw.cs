using Gungeon;
using Alexandria.ItemAPI;

namespace Items
{
    class PrimeSaw : GunBehaviour
    {
        public static bool HasGottenVice;
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Prime Saw", "prime_saw");
            Game.Items.Rename("outdated_gun_mods:prime_saw", "cel:prime_saw");
            gun.gameObject.AddComponent<PrimeSaw>();
            gun.SetShortDescription("The Second Model");
            gun.SetLongDescription("Designed by a mechanic on a far away planet. These weapons were part of a mechanical skeleton meant to restore the damaged body of Cthulhu.");
            gun.SetupSprite(null, "prime_saw_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 15);
            gun.AddProjectileModuleFrom("38_special");
            gun.SetBaseMaxAmmo(1500);
            gun.DefaultModule.ammoCost = 1;
            gun.ammo = 1500;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SMALL_BULLET;
            gun.reloadTime = 0f;
            gun.DefaultModule.cooldownTime = .05f;
            gun.gunHandedness = GunHandedness.OneHanded;
            gun.DefaultModule.numberOfShotsInClip = 1500;
            gun.quality = PickupObject.ItemQuality.S;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.DefaultModule.angleVariance = 2f;
            gun.encounterTrackable.EncounterGuid = "Prime Saw";
            gun.sprite.IsPerpendicular = true;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 2f;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.range *= .05f;
            projectile.sprite.renderer.enabled = false;
            projectile.hitEffects.suppressMidairDeathVfx = true;
            projectile.AdditionalScaleMultiplier = .5f;
            HasGottenVice = false;
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
        }

        private bool HasReloaded;

        public override void Update()
        {
            PlayerController player = gun.CurrentOwner as PlayerController;
            if (gun.CurrentOwner)
            {
                if (!player.HasPickupID(ETGMod.Databases.Items["prime_vice"].PickupObjectId) && HasGottenVice == false)
                {
                    LootEngine.GivePrefabToPlayer(ETGMod.Databases.Items["prime_vice"].gameObject, player);
                    HasGottenVice = true;
                }
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
            
        }
        public override void OnDropped()
        {
            base.OnDropped();
            PlayerController player = gun.CurrentOwner as PlayerController;
            if (player.HasPickupID(ETGMod.Databases.Items["prime_vice"].PickupObjectId))
            {
                player.inventory.RemoveGunFromInventory((ETGMod.Databases.Items["prime_vice"] as Gun));
            }
        }
        

        public PrimeSaw()
        {
        }
    }
}
