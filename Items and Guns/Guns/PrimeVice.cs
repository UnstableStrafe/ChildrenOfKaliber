using Gungeon;
using Alexandria.ItemAPI;

namespace Items
{
    class PrimeVice : GunBehaviour
    {
        
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Prime Vice", "prime_vice");
            Game.Items.Rename("outdated_gun_mods:prime_vice", "cel:prime_vice");
            gun.gameObject.AddComponent<PrimeVice>();
            gun.SetShortDescription("The Second Model");
            gun.SetLongDescription("Designed by a mechanic on a far away planet. These weapons were part of a mechanical skeleton meant to restore the damaged body of Cthulhu.");
            gun.SetupSprite(null, "prime_vice_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 11);
            gun.AddProjectileModuleFrom("38_special");
            gun.SetBaseMaxAmmo(310);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SMALL_BULLET;
            gun.reloadTime = 0f;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(539) as Gun).gunSwitchGroup;
            gun.DefaultModule.cooldownTime = .4f;
            gun.gunHandedness = GunHandedness.OneHanded;
            gun.DefaultModule.numberOfShotsInClip = 310;
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.DefaultModule.angleVariance = 2f;
            gun.encounterTrackable.EncounterGuid = "Prime Vice";
            gun.sprite.IsPerpendicular = true;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 3f;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.range *= .14f;
            projectile.sprite.renderer.enabled = false;
            projectile.baseData.speed *= 2f;
            projectile.hitEffects.suppressMidairDeathVfx = true;
            projectile.AdditionalScaleMultiplier = 1.2f;
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
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

            
            
        }


        public PrimeVice()
        {
        }
    }
}

