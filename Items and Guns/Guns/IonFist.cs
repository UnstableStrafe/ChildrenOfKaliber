using System;
using System.Collections.Generic;
using System.Collections;
using Alexandria.ItemAPI;
using Gungeon;
using UnityEngine;
namespace Items
{
    class IonFist : AdvancedGunBehaviour
    {
        public static void Add()
        {
            string shorthandName = "ion_fist";
            Gun gun = ETGMod.Databases.Items.NewGun("Ion Fist", shorthandName);
            Game.Items.Rename("outdated_gun_mods:"+ shorthandName, "ck:"+shorthandName);
            gun.gameObject.AddComponent<IonFist>();
            gun.SetShortDescription("Casually Approach Child");
            gun.SetLongDescription("Charge up for powerful dashes. Grants melee immunity and flight while dashing. A fully-charged dash reflects projectiles.\n\nThis guantlet is charged to the brim with ions. Simply touching another object creates a powerful electric surge.");
            gun.SetupSprite(null, shorthandName+"_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 24);
            gun.AddProjectileModuleFrom("38_special");
            gun.SetBaseMaxAmmo(180);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.2f;
            gun.DefaultModule.cooldownTime = 0.5f;
            gun.gunClass = GunClass.CHARGE;
            gun.DefaultModule.numberOfShotsInClip = 5;
            gun.quality = PickupObject.ItemQuality.A;
            gun.muzzleFlashEffects.type = VFXPoolType.None;            
            gun.DefaultModule.angleVariance = 0f;
            gun.encounterTrackable.EncounterGuid = "ion dash fist gun owo";
            gun.sprite.IsPerpendicular = true;
            Projectile projectile1 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(153) as Gun).DefaultModule.projectiles[0]);
            projectile1.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile1.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile1);
            gun.DefaultModule.projectiles[0] = projectile1;
            projectile1.gameObject.AddComponent<UnchangeableRangeController>();
            projectile1.baseData.damage *= 4f;
            projectile1.baseData.speed *= 1f;
            projectile1.baseData.force *= 1f;
            projectile1.baseData.range = 4f;
            projectile1.AdditionalScaleMultiplier = 1.4f;
            projectile1.transform.parent = gun.barrelOffset;
            projectile1.sprite.renderer.enabled = false;
            Projectile projectile2 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(153) as Gun).DefaultModule.projectiles[0]);
            projectile2.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile2.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile2);
            gun.DefaultModule.projectiles[0] = projectile2;
            gun.gunHandedness = GunHandedness.HiddenOneHanded;
            projectile2.gameObject.AddComponent<UnchangeableRangeController>();
            projectile2.baseData.damage *= 8f;
            projectile2.baseData.speed *= .75f;
            projectile2.baseData.force *= 1f;
            projectile2.baseData.range = 7.5f;
            projectile2.AdditionalScaleMultiplier = 1.4f;
            projectile2.transform.parent = gun.barrelOffset;
            projectile2.sprite.renderer.enabled = false;
            Projectile projectile3 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(153) as Gun).DefaultModule.projectiles[0]);
            projectile3.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile3.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile3);
            gun.DefaultModule.projectiles[0] = projectile3;
            projectile3.gameObject.AddComponent<UnchangeableRangeController>();
            projectile3.baseData.damage *= 15f;
            projectile3.baseData.speed *= .75f;
            projectile3.baseData.force *= 1f;
            projectile3.baseData.range = 14f;
            projectile3.AdditionalScaleMultiplier = 1.4f;
            projectile3.transform.parent = gun.barrelOffset;
            projectile3.sprite.renderer.enabled = false;
            ProjectileModule.ChargeProjectile chargeProj1 = new ProjectileModule.ChargeProjectile()
            {
                Projectile = projectile1,
                ChargeTime = .0f,
            };
            ProjectileModule.ChargeProjectile chargeProj2 = new ProjectileModule.ChargeProjectile()
            {
                Projectile = projectile2,
                ChargeTime = .6f,
            };
            ProjectileModule.ChargeProjectile chargeProj3 = new ProjectileModule.ChargeProjectile()
            {
                Projectile = projectile3,
                ChargeTime = 1.2f,
            };
            gun.DefaultModule.chargeProjectiles = new List<ProjectileModule.ChargeProjectile>
            {  
                chargeProj1,
                chargeProj2,
                chargeProj3
            };
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(13) as Gun).gunSwitchGroup;
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
        }

        private bool HasReloaded;
        private PlayerController lastOwner = null;
        private DamageTypeModifier electricityImmunity = null;
        protected override void Update()
        {
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
                //this.forme = 1;
                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_WPN_plasmacell_reload_01", base.gameObject);
            }
        }
        protected override void OnPickedUpByPlayer(PlayerController player)
        {
            base.OnPickedUpByPlayer(player);
        }

        public IEnumerator HandleDash(PlayerController target, Projectile projectile)
        {
            float duration = 0f;
			float elapsed = -BraveTime.DeltaTime;
            float angle = gun.CurrentOwner.CurrentGun.CurrentAngle;
            float adjSpeed = 0;
            this.gun.CanBeDropped = false;
            target.inventory.GunLocked.SetOverride("Ion Dash", true, null);
            target.ReceivesTouchDamage = false;
            target.SetIsFlying(true, "ion fist", true, false);
            if (projectile.baseData.range == 4f && projectile.gameObject.GetComponent<UnchangeableRangeController>() != null)
            {
                duration = .1f;
                adjSpeed = 40;
            }
            else
            {
                if (projectile.baseData.range == 7.5f && projectile.gameObject.GetComponent<UnchangeableRangeController>() != null)
                {
                    duration = .1f;
                    adjSpeed = 75;
                }
                else
                {
                    if (projectile.baseData.range == 14f && projectile.gameObject.GetComponent<UnchangeableRangeController>() != null)
                    {
                        duration = .15f;
                        adjSpeed = 90;
                        SpeculativeRigidbody specRigidbody = target.specRigidbody;
                        specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
                        target.healthHaver.IsVulnerable = false;
                    }
                }
            }
            
            while (elapsed < duration)
			{
				elapsed += BraveTime.DeltaTime;
				gun.CurrentOwner.specRigidbody.Velocity = BraveMathCollege.DegreesToVector(angle).normalized * adjSpeed;
				yield return null;
			}
            target.ReceivesTouchDamage = true;
            this.gun.CanBeDropped = true;
            target.inventory.GunLocked.RemoveOverride("Ion Dash");
            if(adjSpeed == 90)
            {
                target.healthHaver.IsVulnerable = true;
                SpeculativeRigidbody specRigidbody2 = target.specRigidbody;
                specRigidbody2.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody2.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
            }
            target.SetIsFlying(false, "ion fist", true, false);
        }
        public override void PostProcessProjectile(Projectile projectile)
        {
            base.PostProcessProjectile(projectile);
            PlayerController player = this.Owner as PlayerController;
            if(projectile.gameObject.GetComponent<UnchangeableRangeController>() != null)
            {
                player.StartCoroutine(this.HandleDash(player, projectile));
            }
            
        }
        public override void OnAutoReload(PlayerController player, Gun gun)
        {
            base.OnAutoReload(player, gun);
            //forme = 1;
        }
        
        private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
        {
            Projectile component = otherRigidbody.GetComponent<Projectile>();
            if (component != null && !(component.Owner is PlayerController))
            {
                PassiveReflectItem.ReflectBullet(component, true, Owner.specRigidbody.gameActor, 40f, 1f, .5f, 0f);
                PhysicsEngine.SkipCollision = true;
            }
        }
        private static List<Projectile> NenForms = new List<Projectile>()
        {

        };
        

        

        public IonFist()
        {
        }
    }
}
