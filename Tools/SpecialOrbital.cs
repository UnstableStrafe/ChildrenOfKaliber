using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using ItemAPI;
using UnityEngine;
using Dungeonator;
using System.Collections;

namespace Items
{

    public class SpecialOrbital : MonoBehaviour, IPlayerOrbital
    {
        public SpecialOrbital()
        {
            orbitalSpeed = 90;
            orbitalRadius = 3f;
            canShoot = false;
            doesPostProcess = false;
            hasLifetime = false;
            lifetime = 100;
            currentOrbitTarget = null;
            currentCustomOrbitTarget = null;
            spread = 0;
            doesMultishot = false;
            doesBurstshot = false;
            outlineColor = Color.black;
            fireCooldown = .4f;
            cooldownAffectedByPlayerStats = false;
            damageAffectedByPlayerStats = true;
            preventOutline = false;
            affectedByLute = false;
            canShootEnemyOrbiter = true;
            targetToShoot = null;
            specBodyOffsets = new IntVector2(0, 0);
            specBodyDimensions = new IntVector2(3, 3);
            firingMode = FiringSequenceMode.SEQUENCE;
            orbitingMode = OrbitingMode.PLAYER;
            shouldRotate = false;
            rotatesTowardsTargetEnemy = true;
            hasSinWaveMovement = false;
            sinAmplitude = 1;
            sinWavelength = 3;
            shootRange = 6.5f;
            currentShootIndex = 0;
            affectedByBattleStandard = false;
            reloadTime = .7f;
            clipSize = 6;
            hasTarget = false;
            projectileRangeMultiplier = 1;
            projectile = null;
        }

        private void Start()
        {
            //you'll need code from PlayerOrbital and HoveringGunController (the base game one) to make this thing work
            //Add Lute support too
            if(owner == null)
            {
                owner = GameManager.Instance.PrimaryPlayer;
            }
            initialized = true;           
            if(base.gameObject.GetComponent<tk2dSprite>() != null && !preventOutline) // needs a sprite to work!
            {
                sprite = base.gameObject.GetComponent<tk2dSprite>();
                SpriteOutlineManager.AddOutlineToSprite(sprite, outlineColor);
            }
            this.SetOrbitalTier(PlayerOrbital.CalculateTargetTier(owner, this));
            this.SetOrbitalTierIndex(PlayerOrbital.GetNumberOfOrbitalsInTier(owner, orbitalTier));
            owner.orbitals.Add(this);
            ownerCenterPos = owner.CenterPosition;
            if(base.gameObject.GetComponent<SpeculativeRigidbody>() == null)
            {
                body = sprite.SetUpSpeculativeRigidbody(specBodyOffsets, specBodyDimensions);
                if (pixelColliders.Any())
                {
                    body.PixelColliders.Clear();
                    body.PixelColliders.AddRange(pixelColliders);
                    body.CollideWithOthers = true;
                }
                else
                {
                    body.PixelColliders.Clear();
                    body.PixelColliders.Add(new PixelCollider
                    {
                        ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                        CollisionLayer = CollisionLayer.EnemyBlocker,
                        IsTrigger = false,
                        BagleUseFirstFrameOnly = false,
                        SpecifyBagelFrame = string.Empty,
                        BagelColliderNumber = 0,
                        ManualOffsetX = specBodyOffsets.x,
                        ManualOffsetY = specBodyOffsets.y,
                        ManualWidth = specBodyDimensions.x,
                        ManualHeight = specBodyDimensions.y,
                        ManualDiameter = 0,
                        ManualLeftX = 0,
                        ManualLeftY = 0,
                        ManualRightX = 0,
                        ManualRightY = 0,
                    });
                    body.CollideWithOthers = false;
                }
                body.CollideWithTileMap = false;
            }
            if (projectile == null)
            {
                projectile = ((Gun)ETGMod.Databases.Items[56]).DefaultModule.projectiles[0];
                projectile.Owner = owner;
                projectile.Shooter = owner.specRigidbody;

            }
            
        }

        private void Update()
        {
            if (!initialized)
            {
                return;
            }
            HandleMotion();
            if (this.canShoot && isOnCooldown && !isReloading)
            {
                HandleShootCooldown();
            }
            if(canShoot && isReloading && doesReload)
            {
                HandleReload();
            }
            if(this.canShoot && !isOnCooldown && !isReloading)
            {
                HandleShooting(); //finish after doing motion
            }
            if(orbitingMode == OrbitingMode.ENEMY)
            {
                retargetOrbiter -= BraveTime.DeltaTime;
            }
            if (canShoot)
            {
                retargetShoot -= BraveTime.DeltaTime;
            }
        }
        private void HandleReload()
        {
            float f = 0;
            if (cooldownAffectedByPlayerStats)
            {
                cachedReloadTime = reloadTime;
                reloadTime *= owner.stats.GetStatModifier(PlayerStats.StatType.ReloadSpeed);
            }
            if(f < reloadTime)
            {
                f += BraveTime.DeltaTime;
            }
            if(f >= reloadTime)
            {
                isReloading = false;
                isOnCooldown = false;
                cooldownTimer = 0;
                reloadTime = cachedReloadTime;
            }
        }
        private void HandleShootCooldown()
        {
            if (cooldownAffectedByPlayerStats)
            {
                cachedCooldownTime = fireCooldown;
                fireCooldown *= owner.stats.GetStatModifier(PlayerStats.StatType.RateOfFire);
            }
            if(cooldownTimer < fireCooldown)
            {
                cooldownTimer += BraveTime.DeltaTime;
            }
            if(cooldownTimer >= fireCooldown)
            {
                cooldownTimer = 0;
                isOnCooldown = false;
                fireCooldown = cachedCooldownTime;
            }
        }
        private void HandleShooting()
        {
            if (GameManager.Instance.IsPaused || !this.owner || this.owner.CurrentInputState != PlayerInputState.AllInput || this.owner.IsInputOverridden)
            {
                return;
            }
            if (owner.IsStealthed)
            {
                return;
            }
            if (targetToShoot == null || !targetToShoot || targetToShoot.healthHaver.IsDead)
            {
                hasTarget = false;
                FindShootTarget();
            }
            if (targetToShoot == null || !targetToShoot)
            {
                hasTarget = false;
                return;
            }
            if (doesReload)
            {
                currentShootIndex++;
                if (currentShootIndex >= clipSize)
                {
                    currentShootIndex = 0;
                    isReloading = true;
                }
            }

            Vector2 a = FindPredictedTargetPosition(projectile);
            if (!doesBurstshot && !doesMultishot)
            {
                Shoot(a, Vector2.zero, projectile);
                isOnCooldown = true;
            }
            else if(!doesBurstshot && doesMultishot)
            {
                for(int i = 0; i < multishotAmount; i++)
                {
                    Shoot(a, Vector2.zero, projectile);
                }
                isOnCooldown = true;
            }
            else if (doesBurstshot)
            {
                GameManager.Instance.StartCoroutine(HandleBurstShot(a, Vector2.zero, projectile));
            }
            
            //Do burst shot and burst shot + multishot code! shouldnt be too bad. also fucking do collision stuff you nerd
        }
        private void FindShootTarget()
        {
            retargetShoot = .25f;
            targetToShoot = null;
            if (owner == null || owner.CurrentRoom == null)
            {
                return;
            }
            if (canShootEnemyOrbiter && currentOrbitTarget != null)
            {
                targetToShoot = currentOrbitTarget;
                hasTarget = true;
            }
            else
            {
                List<AIActor> activeEnemies = owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                if (activeEnemies != null && activeEnemies.Count > 0)
                {
                    AIActor aiactor = null;
                    float num = -1f;
                    for (int i = 0; i < activeEnemies.Count; i++)
                    {
                        AIActor aiactor2 = activeEnemies[i];
                        if (aiactor2 && aiactor2.HasBeenEngaged && aiactor2.IsWorthShootingAt && aiactor2 != currentOrbitTarget)
                        {
                            float num2 = Vector2.Distance(base.transform.position.XY(), aiactor2.specRigidbody.UnitCenter);
                            if (aiactor == null || num2 < num)
                            {
                                aiactor = aiactor2;
                                num = num2;
                            }
                        }
                    }
                    if (aiactor)
                    {
                        hasTarget = true;
                        targetToShoot = aiactor;
                    }
                    else if(aiactor == null)
                    {
                        hasTarget = true;
                        targetToShoot = currentOrbitTarget;
                    }
                }
            }

        }
        private void FindOrbitTarget()
        {
            retargetOrbiter = .25f;
            currentOrbitTarget = null;
            if (owner == null || owner.CurrentRoom == null)
            {
                return;
            }
            List<AIActor> activeEnemies = owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            if (activeEnemies != null && activeEnemies.Count > 0)
            {
                AIActor aiactor = null;
                float num = -1f;
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    AIActor aiactor2 = activeEnemies[i];
                    if (aiactor2 && aiactor2.HasBeenEngaged && aiactor2.IsWorthShootingAt)
                    {
                        float num2 = Vector2.Distance(base.transform.position.XY(), aiactor2.specRigidbody.UnitCenter);
                        if (aiactor == null || num2 < num)
                        {
                            aiactor = aiactor2;
                            num = num2;
                        }
                    }
                }
                if (aiactor)
                {
                    currentOrbitTarget = aiactor;
                }
            }
        }
        private IEnumerator HandleBurstShot(Vector2 targetPos, Vector2 offset, Projectile projectileGameObject)
        {
           
            int num = 0;
            do
            {
                if (doesMultishot)
                {
                    for (int i = 0; i < multishotAmount; i++)
                    {
                        Shoot(targetPos, offset, projectileGameObject);
                        yield return null;
                    }
                }
                else
                {
                    Shoot(targetPos, offset, projectileGameObject);
                    yield return null;
                }
                yield return new WaitForSeconds(timeBetweenBurstShots);
                num++;
            } while (num < burstAmount);
            isOnCooldown = true;
            yield break;
        }
        private void Shoot(Vector2 targetPos, Vector2 offset, Projectile projectileGameObject)
        {
            Vector2 vector = base.transform.position.XY() + offset;
            Vector2 vector2 = targetPos - vector;
            float z = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f; // Base game stuff dont know why it exists but it might be useful if my method is unreliable or doesnt work at all.
            float angle = Vector2.Angle(vector, targetPos);
            GameObject gameObject2 = SpawnManager.SpawnProjectile(projectileGameObject.gameObject, base.transform.position.XY(), Quaternion.Euler(0f, 0f, z + (UnityEngine.Random.Range(-spread, spread))), true);
            //Uses projectile.SendInDirection() to make the projectile aim properly. It worked with psi scale so why wouldnt it work for special orbital too?
            Projectile component = gameObject2.GetComponent<Projectile>();
            if (component != null)
            {
                component.collidesWithEnemies = true;
                component.collidesWithPlayer = false;
                component.Owner = owner;
                component.Shooter = owner.specRigidbody;
                component.TreatedAsNonProjectileForChallenge = true;
                component.specRigidbody.CollideWithTileMap = false;
              //  component.SendInDirection(targetPos - vector, false, true);
                if (damageAffectedByPlayerStats)
                {
                    component.baseData.damage *= owner.stats.GetStatModifier(PlayerStats.StatType.Damage);
                }
                component.UpdateCollisionMask();
                GameManager.Instance.StartCoroutine(DelayedAddTileCollision(component));
                if (affectedByBattleStandard)
                {
                    if (PassiveItem.IsFlagSetForCharacter(owner, typeof(BattleStandardItem)))
                    {
                        component.baseData.damage *= BattleStandardItem.BattleStandardCompanionDamageMultiplier;
                    }
                }
                if (affectedByLute)
                {
                    if (owner.CurrentGun && owner.CurrentGun.LuteCompanionBuffActive)
                    {
                        component.baseData.damage *= 2f;
                        component.RuntimeUpdateScale(1.75f);
                    }
                }
                if (doesPostProcess)
                {
                    owner.DoPostProcessProjectile(component);
                }
            }
            
        }
        private IEnumerator DelayedAddTileCollision(Projectile proj)
        {
            yield return new WaitForSeconds(.3f);
            proj.specRigidbody.CollideWithTileMap = true;
            proj.UpdateCollisionMask();
            yield break;
        }
        private Vector2 FindPredictedTargetPosition(Projectile proj)
        {
            float num = proj.baseData.speed;
            if (num < 0f)
            {
                num = float.MaxValue;
            }
            Vector2 a = base.transform.position.XY();
            Vector2 vector = (targetToShoot.specRigidbody.HitboxPixelCollider == null) ? this.targetToShoot.specRigidbody.UnitCenter : this.targetToShoot.specRigidbody.HitboxPixelCollider.UnitCenter;
            float d = Vector2.Distance(a, vector) / num;
            return vector + this.targetToShoot.specRigidbody.Velocity * d;
        }
        private void HandleMotion() // Add rotations to facing the target enemy
        {
            if(orbitingMode == OrbitingMode.ENEMY)
            {
               // if(retargetOrbiter <= 0)
               // {
               //     currentOrbitTarget = null;
               // }
                if(currentOrbitTarget == null || !currentOrbitTarget || currentOrbitTarget.healthHaver.IsDead)
                {
                    FindOrbitTarget();
                }
            }
            Vector2 centerPosition = owner.CenterPosition;
            if (Vector2.Distance(centerPosition, base.transform.position.XY()) > 20f)
            {
                base.transform.position = centerPosition.ToVector3ZUp(0f);
                body.Reinitialize();
            }
            if (orbitingMode == OrbitingMode.ENEMY && currentOrbitTarget != null)
            {
                centerPosition = currentOrbitTarget.CenterPosition;
            }
            else if(orbitingMode == OrbitingMode.CUSTOM && currentCustomOrbitTarget != null)
            {
                centerPosition = currentCustomOrbitTarget.transform.position;
            }
            Vector2 vector = centerPosition - ownerCenterPos;
            float num = Mathf.Lerp(0.1f, 15f, vector.magnitude / 4f);
            float d = Mathf.Min(num * BraveTime.DeltaTime, vector.magnitude);
            float num2 = 360f / (float)PlayerOrbital.GetNumberOfOrbitalsInTier(owner, this.GetOrbitalTier()) * (float)this.GetOrbitalTierIndex() + BraveTime.ScaledTimeSinceStartup * orbitalSpeed;
            Vector2 vector2 = ownerCenterPos + (centerPosition - ownerCenterPos).normalized * d;
            vector2 = Vector2.Lerp(vector2, centerPosition, perfectOrbitFactor);
            Vector2 vector3 = vector2 + (Quaternion.Euler(0f, 0f, num2) * Vector3.right * orbitalRadius).XY();
            if (hasSinWaveMovement)
            {
                float d2 = Mathf.Sin(Time.time * sinWavelength) * sinAmplitude;
                vector3 += (Quaternion.Euler(0f, 0f, num2) * Vector3.right).XY().normalized * d2;
            }
            ownerCenterPos = vector2;
            vector3 = vector3.Quantize(0.0625f);
            Vector2 velocity = (vector3 - base.transform.position.XY()) / BraveTime.DeltaTime;
            body.Velocity = velocity;
            this.currentAngle = num2 % 360f;
            if (shouldRotate && hasTarget && targetToShoot == null || !targetToShoot || targetToShoot.healthHaver.IsDead)
            {
                hasTarget = false;
            }
              //fix rotating to target
              
            base.transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
                
            

        }

        
        public void Reinitialize()
        {
            body.Reinitialize();
            ownerCenterPos = owner.CenterPosition;
        }

        public void ToggleRenderer(bool visible)
        {

        }

        public Transform GetTransform()
        {
            return base.transform;
        }

        public float GetOrbitalRadius()
        {
            return orbitalRadius;
        }

        public float GetOrbitalRotationalSpeed()
        {
            return orbitalSpeed;
        }

        public int GetOrbitalTier()
        {
            return orbitalTier;
        }

        public void SetOrbitalTier(int tier)
        {
            orbitalTier = tier;
        }

        public int GetOrbitalTierIndex()
        {
            return orbitalTierIndex;
        }

        public void SetOrbitalTierIndex(int tierIndex)
        {
            orbitalTierIndex = tierIndex;
        }
        
        public enum OrbitingMode //What the orbital orbits around
        {
            PLAYER, //Makes orbital orbit around the player
            ENEMY, //Makes orbital orbit around enemies
            CUSTOM, //Makes orbital orbit around a chosen game object
            DEBUG_NONE //Debug
        }
       
        public enum FiringSequenceMode //If it can shoot, determines the firing sequence of the orbital.
        {
            SEQUENCE, //Each time it shoots, it will cycle through each projectile in the list of projectiles it can shoot
            RANDOM, //Each time it shoots, it will pick a random projectile from the list of projectiles it can shoot
        }
        /// <summary>
        /// The radius at which it orbits from its target
        /// </summary>
        public float orbitalRadius;
        /// <summary>
        /// The speed at which it orbits
        /// </summary>
        public float orbitalSpeed;
        /// <summary>
        /// If you want the orbital to have a lifetime, this is the amount of time, in seconds. Only works if hasLifetime is true. !!Not Implemented Yet!!
        /// </summary>
        public float lifetime;
        /// <summary>
        /// The spread of the projectiles the orbital shoots. 
        /// </summary>
        public float spread;
        /// <summary>
        /// The time between shots.
        /// </summary>
        public float fireCooldown;
        /// <summary>
        /// How tightly it orbits the target. Set super high to make it stick to the target.
        /// </summary>
        public float perfectOrbitFactor;
        /// <summary>
        /// The length of the sin wave motion
        /// </summary>
        public float sinWavelength;
        /// <summary>
        /// Amplitude of the sin waves
        /// </summary>
        public float sinAmplitude;
        /// <summary>
        /// Range of the orbital. !!Not Implemented Yet!!
        /// </summary>
        public float shootRange;
        /// <summary>
        /// Time between shots in a burst. Only works if doesBurstshot is true
        /// </summary>
        public float timeBetweenBurstShots;
        /// <summary>
        /// How long it takes to "reload" after going through each shot in its clip. Note: a burst shot uses only one shot in the "clip." (Maybe tweak that later idk?)
        /// </summary>
        public float reloadTime;
        /// <summary>
        /// base range is 35
        /// </summary>
        public float projectileRangeMultiplier;
        public int orbitalTierIndex;
        /// <summary>
        /// Orbital tier
        /// </summary>
        public int orbitalTier;
        /// <summary>
        /// Amount of shots in a multishot shotgun-like attack. Only works if doesMultishot is true
        /// </summary>
        public int multishotAmount;
        /// <summary>
        /// Amount of shots in a burst. Only works if doesBurstshot is true
        /// </summary>
        public int burstAmount;
        public int clipSize;
        /// <summary>
        /// If the orbital can shoot
        /// </summary>
        public bool canShoot;
        /// <summary>
        /// If the orbital's projectiles are post processed
        /// </summary>
        public bool doesPostProcess;
        /// <summary>
        /// If the orbital has a lifetime. !!Not Implemented Yet!!
        /// </summary>
        public bool hasLifetime;
        /// <summary>
        /// Enables shotgun-like shooting. Can be used with burst shooting
        /// </summary>
        public bool doesMultishot; //Enables shotgun-like shooting
        /// <summary>
        /// Enables burst shooting. Can be used with multishot
        /// </summary>
        public bool doesBurstshot; //Enables burst-like shooting. Can be used with multishot.
        /// <summary>
        /// If the cooldown between shots is based on your rate of fire
        /// </summary>
        public bool cooldownAffectedByPlayerStats;
        /// <summary>
        /// If the orbital's projectile damage is based on your damage
        /// </summary>
        public bool damageAffectedByPlayerStats;
        /// <summary>
        /// Prevents sprite outline
        /// </summary>
        public bool preventOutline;
        /// <summary>
        /// If the orbital is affected by Really Special Lute
        /// </summary>
        public bool affectedByLute;
        /// <summary>
        /// If the orbital can shoot the enemy it's orbiting, if it orbits an enemy
        /// </summary>
        public bool canShootEnemyOrbiter; //If the orbital is set to orbit an enemy, this changes if they can shoot that enemy or not.
        /// <summary>
        /// If the sprite should rotate
        /// </summary>
        public bool shouldRotate;
        /// <summary>
        /// If the sprite should rotate towards the target enemy. !!Not Implemented Yet!!
        /// </summary>
        public bool rotatesTowardsTargetEnemy;
        /// <summary>
        /// If the orbital has sin wave movement (Think Baby Dragun)
        /// </summary>
        public bool hasSinWaveMovement; //Gives orbital sin wave movement (think Baby Dragun)
        /// <summary>
        /// If the orbital is affected by Battle Standard
        /// </summary>
        public bool affectedByBattleStandard;
        /// <summary>
        /// If the orbital reloads
        /// </summary>
        public bool doesReload;
        /// <summary>
        /// The player the orbital is tied to
        /// </summary>
        public PlayerController owner;
        /// <summary>
        /// A list of the projectiles the orbital can shoot. 
        /// </summary>
        public Projectile projectile;
        /// <summary>
        /// The enemy the orbital is orbiting, if it can
        /// </summary>
        public AIActor currentOrbitTarget;
        /// <summary>
        /// The gameobject the orbital is orbiting, if it can
        /// </summary>
        public GameObject currentCustomOrbitTarget;
        /// <summary>
        /// The color of the sprite's outline
        /// </summary>
        public Color outlineColor = new Color();
        /// <summary>
        /// Offsets of the specRigidbody
        /// </summary>
        public IntVector2 specBodyOffsets = new IntVector2();
        /// <summary>
        /// Dimensions of the specRigidbody
        /// </summary>
        public IntVector2 specBodyDimensions = new IntVector2();
        /// <summary>
        /// Pixel colliders of the orbital (for when collision events are added) Best left empty for now
        /// </summary>
        public List<PixelCollider> pixelColliders = new List<PixelCollider> { };
        public FiringSequenceMode firingMode;
        public OrbitingMode orbitingMode;
        private Vector2 ownerCenterPos;
        private tk2dSprite sprite;
        private bool initialized;
        private bool isOnCooldown;
        private bool isReloading;
        private bool hasTarget;
        private SpeculativeRigidbody body; //Add collsion with enemies code eventually
        private float cooldownTimer;
        private float currentAngle;
        private float closestEnemyDistance;
        private float retargetOrbiter;
        private float retargetShoot;
        private float cachedReloadTime;
        private float cachedCooldownTime;
        private int currentShootIndex;
        private AIActor targetToShoot;

    }
    
}
