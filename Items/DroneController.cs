
using UnityEngine;
using Gungeon;
using ItemAPI;

namespace Items
{
    class DroneController : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Drone Controller", "drone_controller");
            Game.Items.Rename("outdated_gun_mods:drone_controller", "cel:drone_controller");
            gun.gameObject.AddComponent<DroneController>();
            gun.SetShortDescription("Autobots");
            gun.SetLongDescription("Creates 2 drones that fire at the nearest enemies.\n\nThese cute little drones excel at creating violence for those on their recieving end. Having just one of these is enough to earn 1000 years in a statis chamber, under Hegemony law. Thankfully, time has no meaning inside the Gungeon! Also, they can make coffee.");
            gun.SetupSprite(null, "drone_controller_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 15);
            gun.AddProjectileModuleFrom("38_special", true, false);
            gun.SetBaseMaxAmmo(500);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(229) as Gun).gunSwitchGroup;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SMALL_BULLET;
            gun.reloadTime = 0f;
            gun.DefaultModule.cooldownTime = .11f;
            gun.gunHandedness = GunHandedness.TwoHanded;
            gun.DefaultModule.numberOfShotsInClip = 500;
            gun.quality = PickupObject.ItemQuality.A;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.DefaultModule.angleVariance = 0f;
            gun.encounterTrackable.EncounterGuid = "quack";
            gun.sprite.IsPerpendicular = true;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 0f;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.range *= 0000000000000000000000001f;
            projectile.sprite.renderer.enabled = false;
            projectile.hitEffects.suppressMidairDeathVfx = true;
            projectile.AdditionalScaleMultiplier = .0005f;

            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }

        private bool HasReloaded;

        protected void Update()
        {
            PlayerController player = gun.CurrentOwner as PlayerController;
            if (gun.CurrentOwner)
            {
                bool flag5 = this.gun.CurrentOwner != null && this.gun.CurrentOwner is PlayerController;
                if (flag5)
                {
                    this.lastOwner = (this.gun.CurrentOwner as PlayerController);
                    bool flag6 = this.electricityImmunity == null;
                    if (flag6)
                    {
                        this.electricityImmunity = new DamageTypeModifier
                        {
                            damageMultiplier = 0f,
                            damageType = CoreDamageTypes.Electric
                        };
                        this.lastOwner.healthHaver.damageTypeModifiers.Add(this.electricityImmunity);
                    }
                }
                else
                {
                    bool flag7 = this.electricityImmunity != null;
                    if (flag7)
                    {
                        this.lastOwner.healthHaver.damageTypeModifiers.Remove(this.electricityImmunity);
                        this.electricityImmunity = null;
                    }
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


        private PlayerController lastOwner = null;
        private DamageTypeModifier electricityImmunity = null;
        public override void OnPostFired(PlayerController player, Gun gun)
        {

            gun.PreventNormalFireAudio = true;

        }


        public DroneController()
        {
        }
    }
}
