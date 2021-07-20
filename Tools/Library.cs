using System.Linq;
using ItemAPI;
using Steamworks;
using UnityEngine;
using Dungeonator;
using Gungeon;
using System;
using System.Collections.Generic;
using System.Collections;
using MonoMod.RuntimeDetour;
using MonoMod;
using System.Reflection;
namespace Items
{
    public static class Library
    {
        public static GameActorHealthEffect Venom = new GameActorHealthEffect()
        {
            TintColor = new Color(78 / 255f, 5 / 255f, 120 / 255f),
            DeathTintColor = new Color(78 / 255f, 5 / 255f, 120 / 255f),
            AppliesTint = true,
            AppliesDeathTint = true,
            AffectsEnemies = true,
            DamagePerSecondToEnemies = 20f,
            duration = 2.5f,
            effectIdentifier = "Venom",
        };
        public static GoopDefinition VenomGoop = new GoopDefinition()
        {
            CanBeIgnited = false,
            damagesEnemies = false,
            damagesPlayers = false,
            baseColor32 = new Color32(78, 5, 120, 200),
            goopTexture = ResourceExtractor.GetTextureFromResource("Items/Resources/goop_standard_base_001.png"),
            AppliesDamageOverTime = true,
            HealthModifierEffect = Library.Venom,    
        };
        public static NitricAcidHealthEffect NitricAcid = new NitricAcidHealthEffect()
        {
            DamagePerSecondToEnemies = 5f,
            effectIdentifier = "Nitric Acid",
            AffectsEnemies = true,
            resistanceType = EffectResistanceType.Poison,
            duration = 5,
            TintColor = new Color(240 / 110, 230 / 110, 89 / 110),
            AppliesTint = true,
            AppliesDeathTint = true,
        };
        public static GoopDefinition NitricAcidGoop = new GoopDefinition()
        {
            CanBeIgnited = false,
            damagesEnemies = false,
            damagesPlayers = false,
            baseColor32 = new Color32(240, 230, 89, 255),
            goopTexture = ResourceExtractor.GetTextureFromResource("Items/Resources/goop_standard_base_001.png"),
            AppliesDamageOverTime = true,
            HealthModifierEffect = Library.NitricAcid,
        };
        public static CharcoalDustEffect CharcoalDust = new CharcoalDustEffect()
        {
            duration = 6f,
            effectIdentifier = "Charcoal",
            AffectsEnemies = true,
            TintColor = new Color(56 / 100, 59 / 100, 64 / 100),
            AppliesDeathTint = true,
            DeathTintColor = new Color(56 / 85, 59 / 85, 64 / 85),
            AppliesTint = true,       
            resistanceType = EffectResistanceType.Fire,
        };
        public static SulfurFuseEffect SulfurEffect = new SulfurFuseEffect()
        {
            AffectsEnemies = true,
            AffectsPlayers = false,
            duration = 10000000000000000,
            effectIdentifier = "Sulfur",
            AppliesTint = false,
            AppliesDeathTint = false,
            AppliesOutlineTint = true,
            resistanceType = EffectResistanceType.None,
            OutlineTintColor = new Color(252f, 56f, 56f, 50f),
        };
        public static GoopDefinition SlowGoop = new GoopDefinition()
        {
            CanBeIgnited = false,
            damagesEnemies = false,
            damagesPlayers = false,
            baseColor32 = new Color32(78, 5, 120, 200),
            goopTexture = ResourceExtractor.GetTextureFromResource("Items/Resources/goop_standard_base_001.png"),
            AppliesDamageOverTime = true,
            SpeedModifierEffect = (ETGMod.Databases.Items["triple_crossbow"] as Gun).DefaultModule.projectiles[0].speedEffect,
        };
        
        public static Projectile RandomProjectile()
        {
            int gunID;
            Gun gun;
            Projectile proj;
            do
            {
                gun = PickupObjectDatabase.GetRandomGun();
                gunID = gun.PickupObjectId;
            }
            while (gun.HasShootStyle(ProjectileModule.ShootStyle.Beam));
            proj = ((Gun)ETGMod.Databases.Items[gunID]).DefaultModule.projectiles[0];
            return proj;
        }
        public static bool ReturnCoinFlip()
        {
            int v = UnityEngine.Random.Range(1, 3);
            if (v == 1)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        public static int LichEye = 815;        
        public static Color LightGreen = (new Color(77f / 140f, 247f / 140f, 122f / 140f));
        public static void DefineGoops()
        {
            var assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            
            foreach (string text in Library.goops)
            {               
                GoopDefinition goopDefinition;                
                try
                {
                    GameObject gameObject2 = assetBundle.LoadAsset(text) as GameObject;                   
                    goopDefinition = gameObject2.GetComponent<GoopDefinition>();
                }
                catch
                {
                    goopDefinition = (assetBundle.LoadAsset(text) as GoopDefinition);
                }
                goopDefinition.name = text.Replace("assets/data/goops/", "").Replace(".asset", "");
                Library.goopDefs.Add(goopDefinition);
                
            }
            
            List<GoopDefinition> list = Library.goopDefs;

            FireDef = Library.goopDefs[0];
            OilDef = Library.goopDefs[1];
            PoisonDef = Library.goopDefs[2];
            BlobulonGoopDef = Library.goopDefs[3];
            WebGoop = Library.goopDefs[4];
            WaterGoop = Library.goopDefs[5];
            
            GoopDefinition midInitWeb = UnityEngine.Object.Instantiate<GoopDefinition>(WebGoop);
            midInitWeb.playerStepsChangeLifetime = false;
            midInitWeb.SpeedModifierEffect = FriendlyWebGoopSpeedMod;
            midInitWeb.CanBeIgnited = false;
            PlayerFriendlyWebGoop = midInitWeb;
            goopDefs.Add(CharmGoopDef);
            goopDefs.Add(GreenFireDef);
            goopDefs.Add(CheeseDef);
        }
        
        static Gun TripleCrossbow = ETGMod.Databases.Items["triple_crossbow"] as Gun;
        static GameActorSpeedEffect TripleCrossbowEffect = TripleCrossbow.DefaultModule.projectiles[0].speedEffect;
        public static GameActorSpeedEffect FriendlyWebGoopSpeedMod = new GameActorSpeedEffect
        {
            duration = 1,
            TintColor = TripleCrossbowEffect.TintColor,
            DeathTintColor = TripleCrossbowEffect.DeathTintColor,
            effectIdentifier = "FriendlyWebSlow",
            AppliesTint = false,
            AppliesDeathTint = false,
            resistanceType = EffectResistanceType.None,
            SpeedMultiplier = 0.40f,

            //Eh
            OverheadVFX = null,
            AffectsEnemies = true,
            AffectsPlayers = false,
            AppliesOutlineTint = false,
            OutlineTintColor = TripleCrossbowEffect.OutlineTintColor,
            PlaysVFXOnActor = false, 
        };
        public static void DebugGrabComponents(GameObject gameObject)
        {
            try
            {
                List<string> log = new List<string>()
                {

                };
                log.Add("Beginning debugging object!");
                log.Add(" -Debug mode: Grab Components");
                log.Add("----------------");
                foreach(Component c in gameObject.GetComponents(typeof(Component)))
                {
                    log.Add("Found component: " + c.GetType().Name);
                    log.Add("----------------");
                }
                log.Add("End of debugging! We hope you found what you were looking for! :)");
                log.Add("----------------");
                var retstr = string.Join("\n", log.ToArray());
                CelsItems.Log(retstr);
            }
            catch(Exception e)
            {
                CelsItems.Log("Something broke when debugging object!\n - Error is: " + e.ToString(), "#FF0000");
            }
        }
        public static T CopyFields<T>(Projectile sample2) where T : Projectile
        {
            T sample = sample2.gameObject.AddComponent<T>();
            sample.PossibleSourceGun = sample2.PossibleSourceGun;
            sample.SpawnedFromOtherPlayerProjectile = sample2.SpawnedFromOtherPlayerProjectile;
            sample.PlayerProjectileSourceGameTimeslice = sample2.PlayerProjectileSourceGameTimeslice;
            sample.BulletScriptSettings = sample2.BulletScriptSettings;
            sample.damageTypes = sample2.damageTypes;
            sample.allowSelfShooting = sample2.allowSelfShooting;
            sample.collidesWithPlayer = sample2.collidesWithPlayer;
            sample.collidesWithProjectiles = sample2.collidesWithProjectiles;
            sample.collidesOnlyWithPlayerProjectiles = sample2.collidesOnlyWithPlayerProjectiles;
            sample.projectileHitHealth = sample2.projectileHitHealth;
            sample.collidesWithEnemies = sample2.collidesWithEnemies;
            sample.shouldRotate = sample2.shouldRotate;
            sample.shouldFlipVertically = sample2.shouldFlipVertically;
            sample.shouldFlipHorizontally = sample2.shouldFlipHorizontally;
            sample.ignoreDamageCaps = sample2.ignoreDamageCaps;
            sample.baseData = sample2.baseData;
            sample.AppliesPoison = sample2.AppliesPoison;
            sample.PoisonApplyChance = sample2.PoisonApplyChance;
            sample.healthEffect = sample2.healthEffect;
            sample.AppliesSpeedModifier = sample2.AppliesSpeedModifier;
            sample.SpeedApplyChance = sample2.SpeedApplyChance;
            sample.speedEffect = sample2.speedEffect;
            sample.AppliesCharm = sample2.AppliesCharm;
            sample.CharmApplyChance = sample2.CharmApplyChance;
            sample.charmEffect = sample2.charmEffect;
            sample.AppliesFreeze = sample2.AppliesFreeze;
            sample.FreezeApplyChance = sample2.FreezeApplyChance;
            sample.freezeEffect = (sample2.freezeEffect);
            sample.AppliesFire = sample2.AppliesFire;
            sample.FireApplyChance = sample2.FireApplyChance;
            sample.fireEffect = (sample2.fireEffect);
            sample.AppliesStun = sample2.AppliesStun;
            sample.StunApplyChance = sample2.StunApplyChance;
            sample.AppliedStunDuration = sample2.AppliedStunDuration;
            sample.AppliesBleed = sample2.AppliesBleed;
            sample.bleedEffect = (sample2.bleedEffect);
            sample.AppliesCheese = sample2.AppliesCheese;
            sample.CheeseApplyChance = sample2.CheeseApplyChance;
            sample.cheeseEffect = (sample2.cheeseEffect);
            sample.BleedApplyChance = sample2.BleedApplyChance;
            sample.CanTransmogrify = sample2.CanTransmogrify;
            sample.ChanceToTransmogrify = sample2.ChanceToTransmogrify;
            sample.TransmogrifyTargetGuids = sample2.TransmogrifyTargetGuids;
            sample.BossDamageMultiplier = sample2.BossDamageMultiplier;
            sample.SpawnedFromNonChallengeItem = sample2.SpawnedFromNonChallengeItem;
            sample.TreatedAsNonProjectileForChallenge = sample2.TreatedAsNonProjectileForChallenge;
            sample.hitEffects = sample2.hitEffects;
            sample.CenterTilemapHitEffectsByProjectileVelocity = sample2.CenterTilemapHitEffectsByProjectileVelocity;
            sample.wallDecals = sample2.wallDecals;
            sample.persistTime = sample2.persistTime;
            sample.angularVelocity = sample2.angularVelocity;
            sample.angularVelocityVariance = sample2.angularVelocityVariance;
            sample.spawnEnemyGuidOnDeath = sample2.spawnEnemyGuidOnDeath;
            sample.HasFixedKnockbackDirection = sample2.HasFixedKnockbackDirection;
            sample.FixedKnockbackDirection = sample2.FixedKnockbackDirection;
            sample.pierceMinorBreakables = sample2.pierceMinorBreakables;
            sample.objectImpactEventName = sample2.objectImpactEventName;
            sample.enemyImpactEventName = sample2.enemyImpactEventName;
            sample.onDestroyEventName = sample2.onDestroyEventName;
            sample.additionalStartEventName = sample2.additionalStartEventName;
            sample.IsRadialBurstLimited = sample2.IsRadialBurstLimited;
            sample.MaxRadialBurstLimit = sample2.MaxRadialBurstLimit;
            sample.AdditionalBurstLimits = sample2.AdditionalBurstLimits;
            sample.AppliesKnockbackToPlayer = sample2.AppliesKnockbackToPlayer;
            sample.PlayerKnockbackForce = sample2.PlayerKnockbackForce;
            sample.HasDefaultTint = sample2.HasDefaultTint;
            sample.DefaultTintColor = sample2.DefaultTintColor;
            sample.IsCritical = sample2.IsCritical;
            sample.BlackPhantomDamageMultiplier = sample2.BlackPhantomDamageMultiplier;
            sample.PenetratesInternalWalls = sample2.PenetratesInternalWalls;
            sample.neverMaskThis = sample2.neverMaskThis;
            sample.isFakeBullet = sample2.isFakeBullet;
            sample.CanBecomeBlackBullet = sample2.CanBecomeBlackBullet;
            sample.TrailRenderer = sample2.TrailRenderer;
            sample.CustomTrailRenderer = sample2.CustomTrailRenderer;
            sample.ParticleTrail = sample2.ParticleTrail;
            sample.DelayedDamageToExploders = sample2.DelayedDamageToExploders;
            sample.OnHitEnemy = sample2.OnHitEnemy;
            sample.OnWillKillEnemy = sample2.OnWillKillEnemy;
            sample.OnBecameDebris = sample2.OnBecameDebris;
            sample.OnBecameDebrisGrounded = sample2.OnBecameDebrisGrounded;
            sample.IsBlackBullet = sample2.IsBlackBullet;
            sample.statusEffectsToApply = sample2.statusEffectsToApply;
            sample.AdditionalScaleMultiplier = sample2.AdditionalScaleMultiplier;
            sample.ModifyVelocity = sample2.ModifyVelocity;
            sample.CurseSparks = sample2.CurseSparks;
            sample.PreMoveModifiers = sample2.PreMoveModifiers;
            sample.OverrideMotionModule = sample2.OverrideMotionModule;
            sample.Shooter = sample2.Shooter;
            sample.Owner = sample2.Owner;
            sample.Speed = sample2.Speed;
            sample.Direction = sample2.Direction;
            sample.DestroyMode = sample2.DestroyMode;
            sample.Inverted = sample2.Inverted;
            sample.LastVelocity = sample2.LastVelocity;
            sample.ManualControl = sample2.ManualControl;
            sample.ForceBlackBullet = sample2.ForceBlackBullet;
            sample.IsBulletScript = sample2.IsBulletScript;
            sample.OverrideTrailPoint = sample2.OverrideTrailPoint;
            sample.SkipDistanceElapsedCheck = sample2.SkipDistanceElapsedCheck;
            sample.ImmuneToBlanks = sample2.ImmuneToBlanks;
            sample.ImmuneToSustainedBlanks = sample2.ImmuneToSustainedBlanks;
            sample.ForcePlayerBlankable = sample2.ForcePlayerBlankable;
            sample.IsReflectedBySword = sample2.IsReflectedBySword;
            sample.LastReflectedSlashId = sample2.LastReflectedSlashId;
            sample.TrailRendererController = sample2.TrailRendererController;
            sample.braveBulletScript = sample2.braveBulletScript;
            sample.TrapOwner = sample2.TrapOwner;
            sample.SuppressHitEffects = sample2.SuppressHitEffects;
            UnityEngine.Object.Destroy(sample2);
            return sample;
        }
        public static void InitVacuumProjectiles()
        {
            Gun gun = PickupObjectDatabase.GetById(56) as Gun;
            for(int i = 0; i < 12; i++)
            {
                Projectile projectile = Library.CopyFields<RandomMidAirTachyon>(UnityEngine.Object.Instantiate(gun.DefaultModule.projectiles[0]));
                projectile.gameObject.SetActive(false);
                FakePrefab.MarkAsFakePrefab(projectile.gameObject);
                UnityEngine.Object.DontDestroyOnLoad(projectile);
                projectile.transform.parent = gun.barrelOffset;
                projectile.baseData.damage *= .25f;
                projectile.baseData.speed *= .75f;
                projectile.baseData.force *= .8f;
                projectile.baseData.range *= 1.4f;
                projectile.shouldRotate = true;
                ProjectileSpeedChange speedMod = projectile.gameObject.AddComponent<ProjectileSpeedChange>();
                speedMod.incrementRate = .10f;
                string filepath = "filler";
                int x = 0;
                int y = 0;
                switch (i)
                {
                    case 0:
                        filepath = "vacuum_debris_001";
                        x = 6;
                        y = 12;
                        break;
                    case 1:
                        filepath = "vacuum_debris_002";
                        x = 5;
                        y = 5;
                        break;
                    case 2:
                        filepath = "vacuum_debris_003";
                        x = 6;
                        y = 5;
                        break;
                    case 3:
                        filepath = "vacuum_debris_004";
                        x = 9;
                        y = 15;
                        break;
                    case 4:
                        filepath = "vacuum_debris_005";
                        x = 5;
                        y = 7;
                        break;
                    case 5:
                        filepath = "vacuum_debris_006";
                        x = 4;
                        y = 6;
                        break;
                    case 6:
                        filepath = "vacuum_debris_007";
                        x = 7;
                        y = 7;
                        break;
                    case 7:
                        filepath = "vacuum_debris_008";
                        x = 11;
                        y = 11;
                        break;
                    case 8:
                        filepath = "vacuum_debris_009";
                        x = 9;
                        y = 6;
                        break;
                    case 9:
                        filepath = "vacuum_debris_010";
                        x = 8;
                        y = 7;
                        break;
                    case 10:
                        filepath = "vacuum_debris_011";
                        x = 5;
                        y = 7;
                        break;
                    case 11:
                        filepath = "vacuum_debris_012";
                        x = 6;
                        y = 8;
                        break;
                }
                int? overrideX = null;
                if(x < 7)
                {
                    overrideX = 7;
                }
                int? overrideY = null;
                if(y < 7)
                {
                    overrideY = 7;
                }
                projectile.SetProjectileSpriteRight(filepath, x, y, overrideX, overrideY);
                vacuumProjectiles.Add(projectile);
                
            }

        }
        public static List<Projectile> vacuumProjectiles = new List<Projectile> { };
        public static List<GoopDefinition> goopDefs = new List<GoopDefinition> { };
        private static string[] goops = new string[]
        {
            "assets/data/goops/napalmgoopthatworks.asset",
            "assets/data/goops/oil goop.asset",
            "assets/data/goops/poison goop.asset",
            "assets/data/goops/blobulongoop.asset",
            "assets/data/goops/phasewebgoop.asset",
            "assets/data/goops/water goop.asset",

        };

        public static GoopDefinition FireDef;
        public static GoopDefinition OilDef;
        public static GoopDefinition PoisonDef;
        public static GoopDefinition BlobulonGoopDef;
        public static GoopDefinition WebGoop;
        public static GoopDefinition PlayerFriendlyWebGoop;
        public static GoopDefinition WaterGoop;
        public static GoopDefinition CharmGoopDef = PickupObjectDatabase.GetById(310)?.GetComponent<WingsItem>()?.RollGoop;
        public static GoopDefinition GreenFireDef = (PickupObjectDatabase.GetById(698) as Gun).DefaultModule.projectiles[0].GetComponent<GoopModifier>().goopDefinition;
        public static GoopDefinition CheeseDef = (PickupObjectDatabase.GetById(808) as Gun).DefaultModule.projectiles[0].GetComponent<GoopModifier>().goopDefinition;

        public static void AddCurrentGunStatModifier(this Gun gun, PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod modifyMethod)
        {
            gun.currentGunStatModifiers = gun.currentGunStatModifiers.Concat(new StatModifier[] { new StatModifier { statToBoost = statType, amount = amount, modifyType = modifyMethod } }).ToArray();
        }

        public static void RemoveCurrentGunStatModifier(this Gun gun, PlayerStats.StatType statType)
        {
            List<StatModifier> list = new List<StatModifier>();
            foreach (StatModifier mod in gun.currentGunStatModifiers)
            {
                if (mod.statToBoost != statType)
                {
                    list.Add(mod);
                }
            }
            gun.currentGunStatModifiers = list.ToArray();
        }

        public static tk2dSpriteAnimationClip AddAnimation(this GameObject obj, string name, string spriteDirectory, int fps, AnimationType type, DirectionalAnimation.DirectionType directionType = DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType flipType = DirectionalAnimation.FlipType.None, tk2dSpriteAnimationClip.WrapMode warpmode = tk2dSpriteAnimationClip.WrapMode.Loop)
        {
            obj.AddComponent<tk2dSpriteAnimator>();
            AIAnimator aianimator = obj.GetComponent<AIAnimator>();
            bool flag = !aianimator;
            bool flag2 = flag;
            bool flag3 = flag2;
            if (flag3)
            {
                aianimator = CreateNewAIAnimator(obj);
            }
            DirectionalAnimation directionalAnimation = aianimator.GetDirectionalAnimation(name, directionType, type);
            bool flag4 = directionalAnimation == null;
            bool flag5 = flag4;
            bool flag6 = flag5;
            if (flag6)
            {
                directionalAnimation = new DirectionalAnimation
                {
                    AnimNames = new string[0],
                    Flipped = new DirectionalAnimation.FlipType[0],
                    Type = directionType,
                    Prefix = string.Empty
                };
            }
            directionalAnimation.AnimNames = directionalAnimation.AnimNames.Concat(new string[]
            {
                name
            }).ToArray<string>();
            directionalAnimation.Flipped = directionalAnimation.Flipped.Concat(new DirectionalAnimation.FlipType[]
            {
                flipType
            }).ToArray<DirectionalAnimation.FlipType>();
            aianimator.AssignDirectionalAnimation(name, directionalAnimation, type);
            return BuildAnimation(aianimator, name, spriteDirectory, fps, warpmode);
        }

        // Token: 0x0600015B RID: 347 RVA: 0x0000D254 File Offset: 0x0000B454
        private static AIAnimator CreateNewAIAnimator(GameObject obj)
        {
            AIAnimator aianimator = obj.AddComponent<AIAnimator>();
            aianimator.FlightAnimation = CreateNewDirectionalAnimation();
            aianimator.HitAnimation = CreateNewDirectionalAnimation();
            aianimator.IdleAnimation = CreateNewDirectionalAnimation();
            aianimator.TalkAnimation = CreateNewDirectionalAnimation();
            aianimator.MoveAnimation = CreateNewDirectionalAnimation();
            aianimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>();
            aianimator.IdleFidgetAnimations = new List<DirectionalAnimation>();
            aianimator.OtherVFX = new List<AIAnimator.NamedVFXPool>();
            return aianimator;
        }

        // Token: 0x0600015C RID: 348 RVA: 0x0000D2C8 File Offset: 0x0000B4C8
        private static DirectionalAnimation CreateNewDirectionalAnimation()
        {
            return new DirectionalAnimation
            {
                AnimNames = new string[0],
                Flipped = new DirectionalAnimation.FlipType[0],
                Type = DirectionalAnimation.DirectionType.None
            };
        }

        // Token: 0x0600015D RID: 349 RVA: 0x0000D300 File Offset: 0x0000B500
        public static tk2dSpriteAnimationClip BuildAnimation(AIAnimator aiAnimator, string name, string spriteDirectory, int fps, tk2dSpriteAnimationClip.WrapMode warpmode = tk2dSpriteAnimationClip.WrapMode.Loop)
        {
            tk2dSpriteCollectionData tk2dSpriteCollectionData = aiAnimator.GetComponent<tk2dSpriteCollectionData>();
            bool flag = !tk2dSpriteCollectionData;
            bool flag2 = flag;
            bool flag3 = flag2;
            if (flag3)
            {
                tk2dSpriteCollectionData = SpriteBuilder.ConstructCollection(aiAnimator.gameObject, aiAnimator.name + "_collection");
            }
            string[] resourceNames = ResourceExtractor.GetResourceNames();
            List<int> list = new List<int>();
            for (int i = 0; i < resourceNames.Length; i++)
            {
                bool flag4 = resourceNames[i].StartsWith(spriteDirectory.Replace('/', '.'), StringComparison.OrdinalIgnoreCase);
                bool flag5 = flag4;
                bool flag6 = flag5;
                if (flag6)
                {
                    list.Add(SpriteBuilder.AddSpriteToCollection(resourceNames[i], tk2dSpriteCollectionData));
                }
            }
            bool flag7 = list.Count == 0;
            bool flag8 = flag7;
            bool flag9 = flag8;

            tk2dSpriteAnimationClip tk2dSpriteAnimationClip = SpriteBuilder.AddAnimation(aiAnimator.spriteAnimator, tk2dSpriteCollectionData, list, name, warpmode);
            tk2dSpriteAnimationClip.fps = (float)fps;
            return tk2dSpriteAnimationClip;
        }

        // Token: 0x0600015E RID: 350 RVA: 0x0000D3F8 File Offset: 0x0000B5F8
        public static DirectionalAnimation GetDirectionalAnimation(this AIAnimator aiAnimator, string name, DirectionalAnimation.DirectionType directionType, AnimationType type)
        {
            DirectionalAnimation directionalAnimation = null;
            switch (type)
            {
                case AnimationType.Move:
                    directionalAnimation = aiAnimator.MoveAnimation;
                    break;
                case AnimationType.Idle:
                    directionalAnimation = aiAnimator.IdleAnimation;
                    break;
                case AnimationType.Flight:
                    directionalAnimation = aiAnimator.FlightAnimation;
                    break;
                case AnimationType.Hit:
                    directionalAnimation = aiAnimator.HitAnimation;
                    break;
                case AnimationType.Talk:
                    directionalAnimation = aiAnimator.TalkAnimation;
                    break;
            }
            bool flag = directionalAnimation != null;
            bool flag2 = flag;
            bool flag3 = flag2;
            DirectionalAnimation result;
            if (flag3)
            {
                result = directionalAnimation;
            }
            else
            {
                result = null;
            }
            return result;
        }

        // Token: 0x0600015F RID: 351 RVA: 0x0000D47C File Offset: 0x0000B67C
        public static void AssignDirectionalAnimation(this AIAnimator aiAnimator, string name, DirectionalAnimation animation, AnimationType type)
        {
            switch (type)
            {
                case AnimationType.Move:
                    aiAnimator.MoveAnimation = animation;
                    break;
                case AnimationType.Idle:
                    aiAnimator.IdleAnimation = animation;
                    break;
                case AnimationType.Fidget:
                    aiAnimator.IdleFidgetAnimations.Add(animation);
                    break;
                case AnimationType.Flight:
                    aiAnimator.FlightAnimation = animation;
                    break;
                case AnimationType.Hit:
                    aiAnimator.HitAnimation = animation;
                    break;
                case AnimationType.Talk:
                    aiAnimator.TalkAnimation = animation;
                    break;
                default:
                    aiAnimator.OtherAnimations.Add(new AIAnimator.NamedDirectionalAnimation
                    {
                        anim = animation,
                        name = name
                    });
                    break;
            }
        }

        // Token: 0x0200017D RID: 381
        public enum AnimationType
        {
            // Token: 0x040003E2 RID: 994
            Move,
            // Token: 0x040003E3 RID: 995
            Idle,
            // Token: 0x040003E4 RID: 996
            Fidget,
            // Token: 0x040003E5 RID: 997
            Flight,
            // Token: 0x040003E6 RID: 998
            Hit,
            // Token: 0x040003E7 RID: 999
            Talk,
            // Token: 0x040003E8 RID: 1000
            Other
        }
        public static void DoAmbientTalk(Transform baseTransform, Vector3 offset, string stringKey, float duration)
        {
            TextBoxManager.ShowTextBox(baseTransform.position + offset, baseTransform, duration, stringKey, string.Empty, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
        }
        /// <summary>
        /// Adds a trail to a GameObject
        /// </summary>
        /// <param name="obj">Object the trail will be added to</param> 
        /// <param name="color">Color of the trail</param>
        /// <param name="texture">Texture the trail will use</param>
        /// <param name="time">The time (in seconds) that the trail will last</param>
        /// <param name="minVertexDistance">"Set the minimum distance the trail can travel before a new vertex is added to it. Smaller values with give smoother trails, consisting of more vertices, but costing more performance." - unity docs</param>
        /// <param name="startWidth">Width at the start of the trail</param>
        /// <param name="endWidth">Width at the end of the trail</param>
        /// <param name="startColor">Color at the start of the trail</param>
        /// <param name="endColor">Color at the end of the trail</param>
        /// <returns></returns>
        public static void AddTrailToObject(GameObject obj, Color color, Texture texture, float time, float minVertexDistance, float startWidth, float endWidth, Color startColor, Color endColor)
        {
            var tr = obj.AddComponent<TrailRenderer>();
            tr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            tr.receiveShadows = false;
            var mat = new Material(Shader.Find("Sprites/Default"));
            mat.mainTexture = texture;
            mat.SetColor("_Color", color);
            tr.material = mat;
            tr.time = time;
            tr.minVertexDistance = minVertexDistance;
            tr.startWidth = startWidth;
            tr.endWidth = endWidth;
            tr.startColor = startColor;
            tr.endColor = endColor;
        }
        

        public static List<StickProjectileToEnemy> stickiesAlive = new List<StickProjectileToEnemy>
        {

        };
    }
    public class StickyProjectile : MonoBehaviour
    {
        public StickyProjectile()
        {
            this.sourceVector = Vector2.zero;
            this.shouldExplodeOnReload = false;
            this.maxLifeTime = 15;
            this.destroyOnGunChanged = false;
            explosionDamageBasedOnProjectileDamage = false;
            hasDetTimer = false;
        }
        private void Start()
        {
            sourceProjectile = base.GetComponent<Projectile>();
            if(sourceProjectile.Owner is PlayerController)
            {
                player = sourceProjectile.Owner as PlayerController;
            }
            sourceGun = player.CurrentGun;
            sourceProjectile.OnHitEnemy += OnHit;
           
        }
        private void OnHit(Projectile self, SpeculativeRigidbody enemy, bool fatal)
        {
            sourceVector = self.LastVelocity;
            if (enemy.aiActor && enemy.aiActor.healthHaver)
            {
                StickProjectileToEnemy sticky = enemy.aiActor.gameObject.AddComponent<StickProjectileToEnemy>();
                sticky.destroyOnGunChanged = destroyOnGunChanged;
                sticky.shouldExplodeOnReload = shouldExplodeOnReload;
                sticky.explosionDamageBasedOnProjectileDamage = explosionDamageBasedOnProjectileDamage;
                sticky.explosionData = explosionData;
                sticky.maxLifeTime = maxLifeTime;
                sticky.sourceProjectile = sourceProjectile;
                sticky.sourceVector = sourceVector;
                sticky.player = player;
                sticky.sourceGun = sourceGun;
                sticky.hasDetTimer = hasDetTimer;
                sticky.detTimer = detTimer;
            }
        }
        public bool destroyOnGunChanged;
        public bool shouldExplodeOnReload;
        public bool explosionDamageBasedOnProjectileDamage;
        public bool hasDetTimer;
        public ExplosionData explosionData;
        public float maxLifeTime;
        public float detTimer;
        private Projectile sourceProjectile;
        private Vector2 sourceVector;
        private PlayerController player;
        private Gun sourceGun;
    }
    public class StickProjectileToEnemy : MonoBehaviour
    {
        private void Start()
        {
            CreateSprite(sourceProjectile);
            Library.stickiesAlive.Add(this);
            if(Library.stickiesAlive.Count >= 15)
            {
                StickProjectileToEnemy oldestSticky = Library.stickiesAlive.First();
                if (oldestSticky.shouldExplodeOnReload)
                {
                    oldestSticky.Explode();
                }
                else
                {
                    Destroy(oldestSticky);
                }
            }
            explosionData = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultSmallExplosionData;
            explosionData.damageToPlayer = 0;
            explosionData.preventPlayerForce = true;
        }
        private void CreateSprite(Projectile source)
        {
            obj = new GameObject("sticky proj");
            
            obj.layer = source.gameObject.layer + 1;
            tk2dSprite sprite = obj.AddComponent<tk2dSprite>();
            sprite.SetSprite(source.sprite.Collection, source.sprite.spriteId);
            base.GetComponent<tk2dSprite>().AttachRenderer(sprite);
            
            sprite.IsPerpendicular = true;
            sprite.HeightOffGround = 0.1f;
            sprite.transform.rotation = Quaternion.Euler(0, 0, sourceVector.ToAngle());
            sprite.transform.parent = base.GetComponent<SpeculativeRigidbody>().transform;
            sprite.GetCurrentSpriteDef().material.shader = ShaderCache.Acquire("Brave/PlayerShader");
            sprite.depthUsesTrimmedBounds = true;
            sprite.UpdateZDepth();
            stickyPrefab = UnityEngine.Object.Instantiate<GameObject>(obj, base.transform.position, Quaternion.Euler(0f, 0f, 0f));
            SpriteOutlineManager.AddOutlineToSprite(sprite, Color.black);
            base.GetComponent<tk2dBaseSprite>().UpdateZDepth();
            if (shouldExplodeOnReload)
            {
                if(sourceGun != null)
                {
                    sourceGun.OnReloadPressed += OnPlayerReloaded;
                }
            }
            if (destroyOnGunChanged)
            {
                if(player != null)
                {
                    player.GunChanged += RemoveOnSwitch;
                }
            }
            this.StartCoroutine(Decease(stickyPrefab, base.GetComponent<SpeculativeRigidbody>()));
        }
        private IEnumerator Decease(GameObject stickyPrefab, SpeculativeRigidbody target)
        {
            float timer = 0;
            while (timer < maxLifeTime)
            {
                timer += BraveTime.DeltaTime;
                stickyPrefab.transform.position = new Vector3(target.UnitCenter.x, target.UnitCenter.y + (target.UnitHeight / 3) * 2, target.transform.position.z);
                stickyPrefab.transform.rotation = Quaternion.Euler(0, 0, sourceVector.ToAngle());
                stickyPrefab.GetComponent<tk2dBaseSprite>().HeightOffGround = base.GetComponent<tk2dBaseSprite>().HeightOffGround + (target.UnitHeight / 3) * 2 + 0.8f;
                stickyPrefab.GetComponent<tk2dBaseSprite>().UpdateZDepth();
                yield return null;
            }
            Destroy(stickyPrefab);
            Destroy(this);
            yield break;
        }
        private void Disconnect()
        {
            if (destroyOnGunChanged)
            {
                if (player != null)
                {
                    player.GunChanged -= RemoveOnSwitch;
                }
            }
            if (shouldExplodeOnReload)
            {
                if (sourceGun != null)
                {
                    sourceGun.OnReloadPressed -= OnPlayerReloaded;
                }
            }
        }
        private void Explode()
        {
            Exploder.Explode(base.GetComponent<AIActor>().CenterPosition, explosionData, Vector2.zero, null, true);

            Destroy(this);
        }
        private void RemoveOnSwitch(Gun arg1, Gun arg2, bool newGun)
        {
            Disconnect();
            if (shouldExplodeOnReload)
            {
                Explode();
            }
            else
            {
               
                Destroy(this);
            }
        }
        private void OnPlayerReloaded(PlayerController arg1, Gun arg2, bool actual)
        {
            Disconnect();
            Explode();
        }
        private void OnDestroy()
        {
            if (destroyOnGunChanged)
            {
                if (player != null)
                {
                    player.GunChanged -= RemoveOnSwitch;
                }
            }
            if (shouldExplodeOnReload)
            {
                if (sourceGun != null)
                {
                    sourceGun.OnReloadPressed -= OnPlayerReloaded;
                }
            }
            Library.stickiesAlive.Remove(this);
            Destroy(stickyPrefab);
        }
        public bool destroyOnGunChanged;
        public bool shouldExplodeOnReload;
        public bool explosionDamageBasedOnProjectileDamage;
        public bool hasDetTimer;
        public ExplosionData explosionData;
        public float maxLifeTime;
        public float detTimer;
        public Projectile sourceProjectile;
        public Vector2 sourceVector;
        public PlayerController player;
        public Gun sourceGun;
        private GameObject stickyPrefab;
        private GameObject obj;

    }
    public class RandomMidAirTachyon : Projectile
    {
        public override void Start()
        {
            base.Start();
            Vector2 unitPosition = base.specRigidbody.Position.UnitPosition;
            Vector2 vector = this.FindExpectedEndPoint();
            float rando = UnityEngine.Random.Range(15, 95) * 0.01f;


            Vector2 newSpot = unitPosition + Direction.normalized * (vector - unitPosition).magnitude * rando;
            vector = newSpot;

            this.baseData.range = Vector2.Distance(vector, base.transform.position.XY());
            base.transform.position = vector.ToVector3ZisY(0f);
            base.specRigidbody.Reinitialize();
            base.Direction = (vector - unitPosition).normalized;
            base.SendInDirection(base.Direction * -1f, true, true);
            this.m_distanceElapsed = 0f;
            base.LastPosition = base.transform.position;
            this.SpawnVFX.SpawnAtPosition(vector.ToVector3ZisY(0f), 0f, null, null, null, null, false, null, null, false);
        }
        public override void Update()
        {
            base.Update();
            Vector2 unitCenter = base.specRigidbody.UnitCenter;
            if (unitCenter.GetAbsoluteRoom() != this.m_room)
            {
                base.DieInAir(false, true, true, false);
            }
        }
        protected Vector2 FindExpectedEndPoint()
        {
            Dungeon dungeon = GameManager.Instance.Dungeon;
            Vector2 unitCenter = base.specRigidbody.UnitCenter;
            Vector2 vector = unitCenter + base.Direction.normalized * this.baseData.range;
            this.m_room = unitCenter.GetAbsoluteRoom();
            bool flag = false;
            Vector2 vector2 = unitCenter;
            IntVector2 intVector = vector2.ToIntVector2(VectorConversions.Floor);
            if (dungeon.data.CheckInBoundsAndValid(intVector))
            {
                flag = dungeon.data[intVector].isExitCell;
            }
            float num = vector.x - unitCenter.x;
            float num2 = vector.y - unitCenter.y;
            float num3 = Mathf.Sign(vector.x - unitCenter.x);
            float num4 = Mathf.Sign(vector.y - unitCenter.y);
            bool flag2 = num3 > 0f;
            bool flag3 = num4 > 0f;
            int num5 = 0;
            while (Vector2.Distance(vector2, vector) > 0.1f && num5 < 10000)
            {
                num5++;
                float num6 = Mathf.Abs((((!flag2) ? Mathf.Floor(vector2.x) : Mathf.Ceil(vector2.x)) - vector2.x) / num);
                float num7 = Mathf.Abs((((!flag3) ? Mathf.Floor(vector2.y) : Mathf.Ceil(vector2.y)) - vector2.y) / num2);
                int num8 = Mathf.FloorToInt(vector2.x);
                int num9 = Mathf.FloorToInt(vector2.y);
                IntVector2 intVector2 = new IntVector2(num8, num9);
                bool flag4 = false;
                if (!dungeon.data.CheckInBoundsAndValid(intVector2))
                {
                    break;
                }
                CellData cellData = dungeon.data[intVector2];
                if (cellData.nearestRoom != this.m_room || cellData.isExitCell != flag)
                {
                    break;
                }
                if (cellData.type != CellType.WALL)
                {
                    flag4 = true;
                }
                if (flag4)
                {
                    intVector = intVector2;
                }
                if (num6 < num7)
                {
                    num8++;
                    vector2.x += num6 * num + 0.1f * Mathf.Sign(num);
                    vector2.y += num6 * num2 + 0.1f * Mathf.Sign(num2);
                }
                else
                {
                    num9++;
                    vector2.x += num7 * num + 0.1f * Mathf.Sign(num);
                    vector2.y += num7 * num2 + 0.1f * Mathf.Sign(num2);
                }
            }
            return intVector.ToCenterVector2();
        }

        protected override void Move()
        {
            base.Move();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        public float ProjectileRadius = 0.3125f;
        public VFXPool SpawnVFX;
        private RoomHandler m_room;
    }
    public class ProjectileSpeedChange : MonoBehaviour
    {
        public ProjectileSpeedChange()
        {
            accelerate = true;
            incrementRate = .05f;
            affectedByShotSpeed = false;
            timer = 0;
        }
        private void Start()
        {
            proj = base.GetComponent<Projectile>();
            if (accelerate)
            {
                terminalVelocity = proj.baseData.speed * 30;
            }
            else if (!accelerate)
            {
                terminalVelocity = proj.baseData.speed * .03f;
            }
            
        }
        private void Update()
        {
            if(timer <= .005f)
            {
                timer += BraveTime.DeltaTime;
            }
            if(accelerate && timer > .005f && proj.Speed != terminalVelocity)
            {
                proj.Speed *= (incrementRate + 1);
            }
            else if(!accelerate && timer > .005f && proj.Speed != terminalVelocity)
            {
                proj.Speed *= (1 - incrementRate);
            }
        }
        public bool accelerate;
        public bool affectedByShotSpeed;
        public float incrementRate;
        public float terminalVelocity;
        private Projectile proj;
        private float timer;
    }
    public class MaintainDamageOnPierce : MonoBehaviour
    {
        public MaintainDamageOnPierce()
        {
            damageMultOnPierce = 1;
        }
        public void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            if (m_projectile)
            {
                this.m_projectile.specRigidbody.OnPreRigidbodyCollision += this.HandlePierce;
            }
        }
        private void HandlePierce(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            FieldInfo field = typeof(Projectile).GetField("m_hasPierced", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(myRigidbody.projectile, false);
            myRigidbody.projectile.baseData.damage *= damageMultOnPierce;
        }
        public float damageMultOnPierce;
        private Projectile m_projectile;
    }
    public class SpecialBlankModificationItem : BlankModificationItem
    {
        public static void InitHooks()
        {
            Hook hook = new Hook(
                typeof(SilencerInstance).GetMethod("ProcessBlankModificationItemAdditionalEffects", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(SpecialBlankModificationItem).GetMethod("BlankModificationHook")
            );
        }

        public static void BlankModificationHook(Action<SilencerInstance, BlankModificationItem, Vector2, PlayerController> orig, SilencerInstance self, BlankModificationItem bmi, Vector2 centerPoint, PlayerController user)
        {
            orig(self, bmi, centerPoint, user);
            if (bmi is SpecialBlankModificationItem)
            {
                (bmi as SpecialBlankModificationItem).OnBlank(self, centerPoint, user);
            }
        }

        protected virtual void OnBlank(SilencerInstance silencerInstance, Vector2 centerPoint, PlayerController user)
        {

        }
    }
    public class PreventOnActiveEffects : MonoBehaviour
    {
        string pisslord = "yeah this is basically an empty component for making smth work. sue me.";
    }
    public class EasyTrailBullet : BraveBehaviour //----------------------------------------------------------------------------------------------
    {
        public EasyTrailBullet()
        {
            //=====
            this.TrailPos = new Vector3(0, 0, 0);
            //======
            this.BaseColor = Color.red;
            this.StartColor = Color.red;
            this.EndColor = Color.white;
            //======
            this.LifeTime = 1f;
            //======
            this.StartWidth = 1;
            this.EndWidth = 0;

        }
        /// <summary>
        /// Lets you add a trail to your projectile.    
        /// </summary>
        /// <param name="TrailPos">Where the trail attaches its center-point to. You can input a custom Vector3 but its best to use the base preset. (Namely"projectile.transform.position;").</param>
        /// <param name="BaseColor">The Base Color of your trail.</param>
        /// <param name="StartColor">The Starting color of your trail.</param>
        /// <param name="EndColor">The End color of your trail. Having it different to the StartColor will make it transition from the Starting/Base Color to its End Color during its lifetime.</param>
        /// <param name="LifeTime">How long your trail lives for.</param>
        /// <param name="StartWidth">The Starting Width of your Trail.</param>
        /// <param name="EndWidth">The Ending Width of your Trail. Not sure why youd want it to be something other than 0, but the options there.</param>
        public void Start()
        {
            proj = base.projectile;
            {
                TrailRenderer tr;
                var tro = base.projectile.gameObject.AddChild("trail object");
                tro.transform.position = base.projectile.transform.position;
                tro.transform.localPosition = TrailPos;

                tr = tro.AddComponent<TrailRenderer>();
                tr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                tr.receiveShadows = false;
                var mat = new Material(Shader.Find("Sprites/Default"));
                mat.mainTexture = _gradTexture;
                tr.material = mat;
                tr.minVertexDistance = 0.1f;
                //======
                mat.SetColor("_Color", BaseColor);
                tr.startColor = StartColor;
                tr.endColor = EndColor;
                //======
                tr.time = LifeTime;
                //======
                tr.startWidth = StartWidth;
                tr.endWidth = EndWidth;
            }

        }

        public Texture _gradTexture;
        private Projectile proj;

        public Vector2 TrailPos;
        public Color BaseColor;
        public Color StartColor;
        public Color EndColor;
        public float LifeTime;
        public float StartWidth;
        public float EndWidth;

    }
}
