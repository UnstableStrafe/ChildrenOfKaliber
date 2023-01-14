using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;

namespace Items
{
    class Magnorbs : AdvancedGunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Magnorbs", "magnorbs");
            Game.Items.Rename("outdated_gun_mods:magnorbs", "cel:magnorbs");
            gun.gameObject.AddComponent<Magnorbs>();
            gun.SetShortDescription("Magnets Are Fun");
            gun.SetLongDescription("Shoots magnetic orbs that boomerang back to the gun. Whenever an orb returns to the player, they regain ammo. Reloading the gun with full ammo creates a temporary shield.");
            gun.SetupSprite(null, "magnorbs_idle_001", 13);
            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.AddProjectileModuleFrom("38_special");
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0f;
            gun.DefaultModule.angleVariance = 0f;
            gun.DefaultModule.cooldownTime = 0.25f;
            gun.SetBaseMaxAmmo(3);
            gun.gunClass = GunClass.SILLY;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.quality = PickupObject.ItemQuality.C;
            gun.encounterTrackable.EncounterGuid = "I stole this from starbound lol";
            gun.gunHandedness = GunHandedness.HiddenOneHanded;
            gun.barrelOffset.transform.localPosition = new Vector3(0.3125f, 0.1875f, 0f);
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 1f;
            projectile.baseData.speed *= 1.5f;
            projectile.baseData.range = float.MaxValue;
            projectile.transform.parent = gun.barrelOffset;
            projectile.pierceMinorBreakables = true;
            projectile.gameObject.AddComponent<MaintainDamageOnPierce>();
            PierceProjModifier pierce = projectile.gameObject.GetOrAddComponent<PierceProjModifier>();
            pierce.penetratesBreakables = true;
            projectile.ImmuneToBlanks = true;
            projectile.SetProjectileSpriteRight("magnorb_projectile", 9, 9);
            gun.sprite.IsPerpendicular = true;
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
        }

        private bool HasReloaded;
        public override void PostProcessProjectile(Projectile projectile)
        {
            base.PostProcessProjectile(projectile);
            BounceProjModifier bounce = projectile.gameObject.GetOrAddComponent<BounceProjModifier>();
            bounce.OnBounceContext += OnProjBounced;

            projectile.OnHitEnemy += OnHitEnemy;
            projectile.specRigidbody.OnRigidbodyCollision += OnRigidbodyCollision;
        }
        private void OnRigidbodyCollision(CollisionData collisionData)
        {
            //if (collisionData.OtherRigidbody.majorBreakable != null || collisionData.OtherRigidbody.minorBreakable != null)
            //{
                PierceProjModifier orAddComponent = collisionData.MyRigidbody.gameObject.GetOrAddComponent<PierceProjModifier>();
                if (orAddComponent.penetration == 0)
                {
                    orAddComponent.penetration++;

                }
           // }
        }
        private void OnHitEnemy(Projectile projectile, SpeculativeRigidbody enemy, bool killed)
        {
            PierceProjModifier orAddComponent = projectile.gameObject.GetOrAddComponent<PierceProjModifier>();
            if(orAddComponent.penetration == 0)
            {
                orAddComponent.penetration++;

            }
        }
        
        protected override void Update()
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
            if(gun.ammo != 0)
            {
                gun.ClipShotsRemaining = 1;
                gun.ClearReloadData();
            }
           
        }

        private void OnProjBounced(BounceProjModifier projModifier, SpeculativeRigidbody other)
        {
            Projectile proj = projModifier.GetComponent<Projectile>();
            if(projModifier.numberOfBounces == 0)
            {
                DoProjectileMods(proj);
            }
            //code a thing that checks if the bounce is the last one the projectile has, and if yes, send the projectile back to the player, ignoring tile collisions and also peircing enemies on the way back.
        }
       
        private void DoProjectileMods(Projectile proj)
        {
            proj.OverrideMotionModule = new MagnorbReturnBehav();
            proj.specRigidbody.CollideWithTileMap = false;
            proj.gameObject.AddComponent<PierceDeadActors>();
            PierceProjModifier pierce = proj.gameObject.GetOrAddComponent<PierceProjModifier>();
            pierce.penetratesBreakables = true;
            pierce.UsesMaxBossImpacts = false;
            pierce.penetration = int.MaxValue;
            proj.specRigidbody.AddCollisionLayerIgnoreOverride(5);
            proj.specRigidbody.AddCollisionLayerIgnoreOverride(6);
            proj.OnDestruction += OnProjDeath;
            proj.UpdateCollisionMask();
        }

        private void OnProjDeath(Projectile obj)
        {
            PlayerController player = (obj.Owner is PlayerController) ? obj.Owner as PlayerController : null;
            if (player != null)
            {
                if (player.CurrentGun.PickupObjectId == ETGMod.Databases.Items["Magnorbs"].PickupObjectId)
                {
                    if (player.CurrentGun.ammo != player.CurrentGun.AdjustedMaxAmmo)
                    {
                        player.CurrentGun.GainAmmo(1);
                        player.CurrentGun.ClearReloadData();
                    }
                }
            }

        }

       
        public Magnorbs()
        {

        }
    }
    public class MagnorbReturnBehav : ProjectileMotionModule
    {
        public override void UpdateDataOnBounce(float angleDiff)
        {
           // throw new NotImplementedException();
        }
        public override void Move(Projectile source, Transform projectileTransform, tk2dBaseSprite projectileSprite, SpeculativeRigidbody specRigidbody, ref float m_timeElapsed, ref Vector2 m_currentDirection, bool Inverted, bool shouldRotate)
        {
            source.specRigidbody.Velocity = Vector2.zero;
            PlayerController player = source.Owner as PlayerController;
            Vector2 centerPosition = source.Owner.CenterPosition;
            Vector2 vector = centerPosition - source.specRigidbody.UnitCenter;
            float magnitude = vector.magnitude;
            float d = Mathf.Lerp(20, 50, (magnitude - 50) / (25f - 50));
            source.specRigidbody.Velocity = vector.normalized * d;
            if (Vector2.Distance(source.Owner.CenterPosition, source.transform.position) < 1)
            {
                //give ammo to the player if they have magnorbs
                
                source.DieInAir(true, true, true, false);
            }
        }
    }
    public class ProjectileOrbit : MonoBehaviour
    {
        public ProjectileOrbit()
        {
            orbitalAmount = 1;
        }
        private void Start()
        {

        }
        private void Update()
        {
            if(orbitals.Count != orbitalAmount)
            {
                RebuildOrbitals();
            }
        }
        private void RebuildOrbitals()
        {
            if (orbitals.Any()) 
            { 
                foreach(GameObject obj in orbitals)
                {
                    Destroy(obj);
                }
            }
            for(int i = 0; i < orbitalAmount; i++)
            {
                GameObject gameObject = SpriteBuilder.SpriteFromResource("Items/Resources/VFX/magnorb_projectile");
                SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(9, 9));
                PlayerOrbital orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
                speculativeRigidbody.CollideWithTileMap = false;
                speculativeRigidbody.CollideWithOthers = false;
                orbitalPrefab.shouldRotate = true;
                orbitalPrefab.orbitRadius = 2f;

                orbitalPrefab.orbitDegreesPerSecond = 10;
                orbitalPrefab.SetOrbitalTier(0);
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                FakePrefab.MarkAsFakePrefab(gameObject);
                gameObject.SetActive(false);
                GameObject gameobject2 = PlayerOrbitalItem.CreateOrbital(attachedPlayer, gameObject, false);
                orbitals.Add(gameobject2);
            }
        }
        private void OnDestroy()
        {
            if (orbitals.Any())
            {
                foreach (GameObject obj in orbitals)
                {
                    Destroy(obj);
                }
            }
        }
        public int orbitalAmount;
        private List<GameObject> orbitals = new List<GameObject> { };
         
        private PlayerController attachedPlayer;

    }
}
