using Gungeon;
using Alexandria.ItemAPI;
namespace Items
{
    class DEATHGun : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("D.E.A.T.H.", "d.e.a.t.h.");
            Game.Items.Rename("outdated_gun_mods:d.e.a.t.h.", "ck:d.e.a.t.h.");
            gun.gameObject.AddComponent<DEATHGun>();
            gun.SetShortDescription("This Army");
            gun.SetLongDescription("While held, summons three drones. Projectiles of the gun ignore the boss damage cap, but no those of the drones.\n\nDestructive Energized Automated Targeted Hellfire.");
            gun.SetupSprite(null, "d.e.a.t.h._idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 16);
            gun.SetAnimationFPS(gun.reloadAnimation, 10);
            gun.AddProjectileModuleFrom("electric_rifle");
            gun.DefaultModule.ammoCost = 1;
            gun.gunHandedness = GunHandedness.HiddenOneHanded;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.2f;
            gun.DefaultModule.angleVariance = 4f;
            gun.DefaultModule.burstShotCount = 5;
            gun.DefaultModule.burstCooldownTime = .1f;
            gun.DefaultModule.cooldownTime = .8f;
            gun.DefaultModule.cooldownTime = .11f;
            gun.InfiniteAmmo = true;
            gun.DefaultModule.numberOfShotsInClip = 25;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(153) as Gun).gunSwitchGroup;
            Gun gun2 = PickupObjectDatabase.GetById(153) as Gun;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.SetBaseMaxAmmo(500);
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            gun.encounterTrackable.EncounterGuid = "If you are not UnstableStrafe, reading this has permanently cursed you forever. Unless you are looking for the code about the drones. That's in HoveringGunsAdder.";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.FULLAUTO;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage *= .75f;
            projectile.baseData.speed *= 1f;
            projectile.baseData.force *= .25f;
            projectile.sprite.renderer.enabled = false;
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
        }
        private bool HasReloaded;
        private PlayerController lastOwner = null;
        private DamageTypeModifier electricityImmunity = null;
        public override void Update()
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

        public DEATHGun()
        {

        }
    }
}


