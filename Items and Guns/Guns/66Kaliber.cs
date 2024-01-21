using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;
using Dungeonator;
using Planetside;

namespace Items
{
    class _66Kaliber : AdvancedGunBehavior
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun(".66 Kaliber", "66_kaliber");
            Game.Items.Rename("outdated_gun_mods:66_kaliber", "ck:66_kaliber");
            gun.gameObject.AddComponent<_66Kaliber>();
            gun.SetShortDescription("The Bigger Gun");
            gun.SetLongDescription("Projectiles create explosive fragments on hitting an enemy.\n\nA living gun woven from Kaliber's own flesh, it feeds on bloodshed and favors those with execeptional gunslinging skills. The bullets this gun shoots strike with enough force to turn those hit into shrapnel.\nSomeone's name is carved into the stock of the gun, though you can't make out what it says.");

            gun.SetupSprite(null, "66_kaliber_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 16);
            gun.SetAnimationFPS(gun.reloadAnimation, 12);
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(5) as Gun);
            tk2dSpriteAnimationClip animationclipReload = gun.sprite.spriteAnimator.GetClipByName(gun.reloadAnimation);
            float[] reloadOffsetsX = new float[] { 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f };
            float[] reloadOffsetsY = new float[] { 0.0000f, -0.0625f, -0.1875f, -0.3125f, -0.3750f, -0.4375f, -0.5000f, -0.5000f, -0.6250f, -0.6250f, 0.0000f, -0.5000f, -0.4375f, -0.3750f, -0.3750f, 0.0625f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f };

            tk2dSpriteAnimationClip animationclipShoot = gun.sprite.spriteAnimator.GetClipByName(gun.shootAnimation);
            float[] shootOffsetsX = new float[] { 0.0000f, -0.0625f, -0.1250f, -1.5625f, -1.5000f, 0.0000f, -0.0625f, -0.1875f, -0.1250f, 0.0000f, 0.0000f, 0.0000f };
            float[] shootOffsetsY = new float[] { 0.0000f, 0.0000f, 0.0000f, 1.3750f, 1.3750f, 0.6875f, 0.1875f, -0.5625f, -0.3125f, -0.1250f, 0.0000f, 0.0000f };

            for (int i = 0; i < shootOffsetsX.Length && i < shootOffsetsY.Length && i < animationclipShoot.frames.Length; i++)
            {
                int id = animationclipShoot.frames[i].spriteId;
                Vector3 vector3 = new Vector3(shootOffsetsX[i], shootOffsetsY[i]);
                animationclipShoot.frames[i].spriteCollection.spriteDefinitions[id].position0 += vector3;
                animationclipShoot.frames[i].spriteCollection.spriteDefinitions[id].position1 += vector3;
                animationclipShoot.frames[i].spriteCollection.spriteDefinitions[id].position2 += vector3;
                animationclipShoot.frames[i].spriteCollection.spriteDefinitions[id].position3 += vector3;
            }

            for (int i = 0; i < reloadOffsetsX.Length && i < reloadOffsetsY.Length && i < animationclipReload.frames.Length; i++)
            {
                int id = animationclipReload.frames[i].spriteId;
                Vector3 vector3 = new Vector3(reloadOffsetsX[i], reloadOffsetsY[i]);
                animationclipReload.frames[i].spriteCollection.spriteDefinitions[id].position0 += vector3;
                animationclipReload.frames[i].spriteCollection.spriteDefinitions[id].position1 += vector3;
                animationclipReload.frames[i].spriteCollection.spriteDefinitions[id].position2 += vector3;
                animationclipReload.frames[i].spriteCollection.spriteDefinitions[id].position3 += vector3;
            }

            gun.usesContinuousFireAnimation = false;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 3f;
            gun.DefaultModule.customAmmoType = "rail";
            //gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("66_Kaliber", "Items/Resources/CustomGunAmmoTypes/66_kaliber_ammo_full", "Items/Resources/CustomGunAmmoTypes/66_kaliber_ammo_empty");
            gun.DefaultModule.angleVariance = 0f;
            gun.DefaultModule.cooldownTime = 1.4f;
            gun.DefaultModule.numberOfShotsInClip = 4;
            gun.PreventNormalFireAudio = false;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(5) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(5) as Gun).muzzleFlashEffects;
            gun.SetBaseMaxAmmo(40);
            gun.barrelOffset.localPosition = new Vector3(5f, 0.625f, 0f);
            gun.quality = PickupObject.ItemQuality.S;
            gun.encounterTrackable.EncounterGuid = "The mod's final gun.";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.RIFLE;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage *= 1.25f;
            projectile.baseData.speed *= 3f;
            projectile.baseData.force *= 1f;
            if (projectile.GetComponent<PierceProjModifier>()) { Destroy(projectile.GetComponent<PierceProjModifier>()); }
            projectile.gameObject.AddComponent<FracturingPierce>();
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
            id = gun.PickupObjectId;


        }
        public static GameObject targetPrefab;
        public static List<int> spriteIds = new List<int>();
        public static List<GameObject> existingTargets = new List<GameObject>();
        public static int id;
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

        public override void OnReload(PlayerController player, Gun gun)
        {
            base.OnReload(player, gun);
            
        }

        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && this.HasReloaded)
            {
                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_WPN_m1rifle_reload_01", base.gameObject);
            }
        }


        public override void OnPickup(PlayerController player)
        {

            base.OnPickup(player);
            
        }

        public override void OnPostDrop(PlayerController player)
        {
            
            base.OnPostDrop(player);
            
        }

        
        public override void OnPostFired(PlayerController player, Gun gun)
        {


        }

        
        class FracturingPierce : MonoBehaviour
        {
            void Start()
            {
                proj = gameObject.GetComponent<Projectile>();
                proj.OnHitEnemy += OnHit;
                owner = proj.Owner;
                shooter = proj.Shooter;
            }

            void OnHit(Projectile p, SpeculativeRigidbody e,bool b)
            {
                int num = UnityEngine.Random.Range(3, 6);
               
                for(int i = 0; i < num; i++)
                {
                    float spread = UnityEngine.Random.Range(-30f, 30f);
                    GameObject gameObject = SpawnManager.SpawnProjectile((PickupObjectDatabase.GetById(26) as Gun).DefaultModule.projectiles[0].gameObject, e.sprite.WorldCenter, Quaternion.Euler(0f, 0f, p.Direction.ToAngle()+spread), true);
                    Projectile p2 = gameObject.GetComponent<Projectile>();
                    p2.Owner = owner;
                    p2.Shooter = shooter;
                    p2.baseData.speed *= 1.2f;
                    p2.specRigidbody.RegisterSpecificCollisionException(e);
                    p2.AdditionalScaleMultiplier = 1.5f;
                    ProjectileExplodeOnCollide projectileExplode = p2.gameObject.AddComponent<ProjectileExplodeOnCollide>();
                    ExplosionData explosionData = (PickupObjectDatabase.GetById(4) as Gun).DefaultModule.projectiles[0].gameObject.GetComponent<StickyGrenadeBuff>().explosionData;
                    if(explosionData.ignoreList == null)
                    {
                        explosionData.ignoreList = new List<SpeculativeRigidbody>();
                    }
                    explosionData.ignoreList.Add(e);
                    projectileExplode.explosionData = explosionData;


                   
                }
            }
            

            
            private SpeculativeRigidbody hitEnemy;
            private Projectile proj;
            private GameActor owner;
            private SpeculativeRigidbody shooter;
        }

        class ProjectileExplodeOnCollide : MonoBehaviour
        {
            void Start()
            {
                proj = gameObject.GetComponent<Projectile>();
                proj.OnDestruction += OnDestruction;
                
                owner = proj.Owner;
                shooter = proj.Shooter;
            }

            private void OnDestruction(Projectile obj)
            {
                Exploder.Explode(obj.specRigidbody.UnitCenter, explosionData, Vector2.zero);
            }
            private void Update()
            {
                this.elapsed += BraveTime.DeltaTime;
                bool expire = this.elapsed > .25f;
                if (expire)
                {
                    proj.ForceDestruction();
                }
            }
            private float elapsed;
            public ExplosionData explosionData;
            private Projectile proj;
            private GameActor owner;
            private SpeculativeRigidbody shooter;
        }
    }


}