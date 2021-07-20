using System;
using System.Collections.Generic;
using System.Collections;
using Gungeon;
using ItemAPI;
namespace Items
{
    class NenFist : AdvancedGunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Nen Fist", "nen_fist");
            Game.Items.Rename("outdated_gun_mods:nen_fist", "cel:nen_fist");
            gun.gameObject.AddComponent<NenFist>();
            gun.SetShortDescription("Hunted");
            gun.SetLongDescription("Cycles between a long range fireball, a medium range energy spear, and a powerful dash with each shot.");
            gun.SetupSprite(null, "nen_fist_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 24);
            gun.AddProjectileModuleFrom("38_special", true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = .8f;
            gun.DefaultModule.cooldownTime = .2f;
            gun.DefaultModule.numberOfShotsInClip = 5;
            gun.gunClass = GunClass.FIRE;
            gun.gunHandedness = GunHandedness.HiddenOneHanded;
            Gun gun2 = PickupObjectDatabase.GetById(748) as Gun;
            gun.gunSwitchGroup = gun2.gunSwitchGroup;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.SetBaseMaxAmmo(200);
            gun.DefaultModule.angleVariance = 0f;
            gun.sprite.IsPerpendicular = true;
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            gun.encounterTrackable.EncounterGuid = "NN doesnt watch Hunter x Hunter.";
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            Projectile nenProj1 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(336) as Gun).DefaultModule.projectiles[0]);
            PierceProjModifier pierce1 = nenProj1.gameObject.GetComponent<PierceProjModifier>();
            pierce1.penetration -= 4;
            nenProj1.AdditionalScaleMultiplier *= 1.5f;
            nenProj1.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(nenProj1.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(nenProj1);
            nenProj1.baseData.damage *= 1.5f;
            nenProj1.baseData.speed *= 1f;
            nenProj1.transform.parent = gun.barrelOffset;
            Projectile nenProj2 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(748) as Gun).DefaultModule.chargeProjectiles[0].Projectile);
            nenProj2.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(nenProj2.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(nenProj2);
            nenProj2.baseData.damage *= 1f;
            nenProj2.baseData.speed *= 1f;
            nenProj2.baseData.range = 7.5f;
            nenProj2.transform.parent = gun.barrelOffset;
            Projectile origProj = (PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0];
            Projectile nenProj3 = UnityEngine.Object.Instantiate(origProj);
            nenProj3.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(nenProj3.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(nenProj3);
            nenProj3.baseData.damage *= 14f;
            nenProj3.baseData.speed *= 2f;
            nenProj3.baseData.range = 14;
            nenProj3.sprite.renderer.enabled = false;
            PierceProjModifier orAddComponent = nenProj3.gameObject.GetOrAddComponent<PierceProjModifier>();
            nenProj3.gameObject.AddComponent<UnchangeableRangeController>();
            orAddComponent.penetration -= 50;
            nenProj3.GetComponent<TrailController>();
            nenProj3.transform.parent = gun.barrelOffset;
            NenForms.Add(nenProj1);
            NenForms.Add(nenProj2);
            NenForms.Add(nenProj3);
            ProjectileModule.ChargeProjectile chargeProj1 = new ProjectileModule.ChargeProjectile()
            {
                Projectile = nenProj1,
                ChargeTime = 0f,
            };
            ProjectileModule.ChargeProjectile chargeProj2 = new ProjectileModule.ChargeProjectile()
            {
                Projectile = nenProj2,
                ChargeTime = .4f,
            };
            ProjectileModule.ChargeProjectile chargeProj3 = new ProjectileModule.ChargeProjectile()
            {
                Projectile = nenProj3,
                ChargeTime = 1.2f,
            };
            gun.DefaultModule.chargeProjectiles = new List<ProjectileModule.ChargeProjectile>
            {
                chargeProj1,
                chargeProj2,
                chargeProj3
            };
        }
        public override void OnAutoReload(PlayerController player, Gun gun)
        {
            base.OnAutoReload(player, gun);
            this.forme = 1;
        }
        private bool HasReloaded;
        public override void PostProcessProjectile(Projectile projectile)
        {
            base.PostProcessProjectile(projectile);
            PlayerController player = gun.CurrentOwner as PlayerController;
            if(projectile.gameObject.GetComponent<UnchangeableRangeController>() != null)
            {
                player.StartCoroutine(this.HandleDash(player, projectile));
            }
        }
        private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
        {
            Projectile component = otherRigidbody.GetComponent<Projectile>();
            if (component != null && !(component.Owner is PlayerController))
            {
                PassiveReflectItem.ReflectBullet(component, true, gun.CurrentOwner.specRigidbody.gameActor, 40f, 1f, 1f, 0f);
                PhysicsEngine.SkipCollision = true;
            }
        }
        public IEnumerator HandleDash(PlayerController target, Projectile projectile)
        {
            float duration = 1f;
            float elapsed = -BraveTime.DeltaTime;
            float angle = gun.CurrentOwner.CurrentGun.CurrentAngle;
            float adjSpeed = 100;
            this.gun.CanBeDropped = false;
            target.inventory.GunLocked.SetOverride("Nen Dash", true, null);
            target.ReceivesTouchDamage = false;
            target.SetIsFlying(true, "nen fist", true, false);
            duration = .15f;
            adjSpeed = 90;
            SpeculativeRigidbody specRigidbody = target.specRigidbody;
            specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
            target.healthHaver.IsVulnerable = false;
                       
            while (elapsed < duration)
            {
                elapsed += BraveTime.DeltaTime;
                gun.CurrentOwner.specRigidbody.Velocity = BraveMathCollege.DegreesToVector(angle).normalized * adjSpeed;
                yield return null;
            }
            target.ReceivesTouchDamage = true;
            this.gun.CanBeDropped = true;
            target.inventory.GunLocked.RemoveOverride("Nen Dash");
            if (adjSpeed == 90)
            {
                target.healthHaver.IsVulnerable = true;
                SpeculativeRigidbody specRigidbody2 = target.specRigidbody;
                specRigidbody2.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody2.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
            }
            target.SetIsFlying(false, "nen fist", true, false);
        }
        protected override void Update()
        {
            PlayerController player = gun.CurrentOwner as PlayerController;
            if (gun.CurrentOwner)
            {
                
                if (!gun.IsReloading && !HasReloaded)
                {
                    this.HasReloaded = true;
                }

                if (gun.PreventNormalFireAudio)
                {
                    this.gun.PreventNormalFireAudio = true;
                }
            }
        }
        public override Projectile OnPreFireProjectileModifier(Gun gun, Projectile projectile, ProjectileModule mod)
        {
            return NenForms[this.forme - 1];
        }

        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            base.OnReloadPressed(player, gun, bSOMETHING);
            bool isReloading = this.gun.IsReloading;
            if (isReloading)
            {
                this.forme = 1;
                bool flag = this.HasReloaded;
                if (flag)
                {
                    AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                    AkSoundEngine.PostEvent("Play_WPN_plasmacell_reload_01", base.gameObject);
                    this.HasReloaded = false;
                }
            }
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            base.OnPostFired(player, gun);
            AkSoundEngine.PostEvent("Play_WPN_plasmacell_shot_01", base.gameObject);
            this.forme++;
            bool flag = this.forme > 3;
            if (flag)
            {
                this.forme = 1;
            }
        }

        private static List<Projectile> NenForms = new List<Projectile>()
        {

        };
        private int forme = 1;
        public NenFist()
        {

        }
    }
}
