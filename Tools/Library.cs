using System.Linq;
using Alexandria.ItemAPI;
using UnityEngine;
using Dungeonator;
using Gungeon;
using System;
using System.Collections.Generic;
using System.Collections;
using MonoMod.RuntimeDetour;
using MonoMod;
using System.Reflection;
using Alexandria.Misc;

//Something in this class MAJORLY FUCKED UP
namespace Items
{
    public static class Library
    {
        public static GameActorHealthEffect Venom = new GameActorHealthEffect
        {
            TintColor = new Color(0.30588236f, 0.019607844f, 0.47058824f),
            DeathTintColor = new Color(0.30588236f, 0.019607844f, 0.47058824f),
            AppliesTint = true,
            AppliesDeathTint = true,
            AffectsEnemies = true,
            DamagePerSecondToEnemies = 20f,
            duration = 2.5f,
            effectIdentifier = "Venom"
        };
        public static GoopDefinition VenomGoop = new GoopDefinition
        {
            CanBeIgnited = false,
            damagesEnemies = false,
            damagesPlayers = false,
            baseColor32 = new Color32(78, 5, 120, 200),
            goopTexture = ResourceExtractor.GetTextureFromResource("Items/Resources/goop_standard_base_001.png"),
            AppliesDamageOverTime = true,
            HealthModifierEffect = Library.Venom
        };

        public static NitricAcidHealthEffect NitricAcid = new NitricAcidHealthEffect
        {
            DamagePerSecondToEnemies = 5f,
            effectIdentifier = "Nitric Acid",
            AffectsEnemies = true,
            resistanceType = EffectResistanceType.Poison,
            duration = 5f,
            TintColor = new Color(2f, 2f, 0f),
            AppliesTint = true,
            AppliesDeathTint = true
        };

        public static GoopDefinition NitricAcidGoop = new GoopDefinition
        {
            CanBeIgnited = false,
            damagesEnemies = false,
            damagesPlayers = false,
            baseColor32 = new Color32(240, 230, 89, byte.MaxValue),
            goopTexture = ResourceExtractor.GetTextureFromResource("Items/Resources/goop_standard_base_001.png"),
            AppliesDamageOverTime = true,
            HealthModifierEffect = Library.NitricAcid
        };

        public static CharcoalDustEffect CharcoalDust = new CharcoalDustEffect
        {
            duration = 6f,
            effectIdentifier = "Charcoal",
            AffectsEnemies = true,
            TintColor = new Color(0f, 0f, 0f),
            AppliesDeathTint = true,
            DeathTintColor = new Color(0f, 0f, 0f),
            AppliesTint = true,
            resistanceType = EffectResistanceType.Fire
        };

        public static SulfurFuseEffect SulfurEffect = new SulfurFuseEffect
        {
            AffectsEnemies = true,
            AffectsPlayers = false,
            duration = 1E+16f,
            effectIdentifier = "Sulfur",
            AppliesTint = false,
            AppliesDeathTint = false,
            AppliesOutlineTint = true,
            resistanceType = EffectResistanceType.None,
            OutlineTintColor = new Color(252f, 56f, 56f, 50f)
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
        public static Color32 LightGreen = (new Color32(77, 247, 122, 255));
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
                foreach (Component c in gameObject.GetComponents(typeof(Component)))
                {
                    log.Add("Found component: " + c.GetType().Name);
                    log.Add("----------------");
                }
                log.Add("End of debugging! We hope you found what you were looking for! :)");
                log.Add("----------------");
                var retstr = string.Join("\n", log.ToArray());
                ETGModConsole.Log(retstr);
            }
            catch (Exception e)
            {
                ETGModConsole.Log("Something broke when debugging object!\n - Error is: " + e.ToString());
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

        public static T ReflectGetField<T>(Type classType, string fieldName, object o = null)
        {
            FieldInfo field = classType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | ((o != null) ? BindingFlags.Instance : BindingFlags.Static));
            return (T)field.GetValue(o);
        }

        public static List<StickyProjectile> stickiesAlive = new List<StickyProjectile>
        {

        };

        public static Projectile SetupProjectile(int id)
        {
            Projectile proj = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(id) as Gun).DefaultModule.projectiles[0]);
            proj.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(proj.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(proj);

            return proj;
        }

        public static Projectile SetupProjectile(Projectile projToCopy)
        {
            Projectile proj = UnityEngine.Object.Instantiate<Projectile>(projToCopy);
            proj.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(proj.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(proj);

            return proj;
        }
        public static void DisableSuperTinting(AIActor actor)
        {
            Material mat = actor.sprite.renderer.material;
            mat.mainTexture = actor.sprite.renderer.material.mainTexture;
            mat.EnableKeyword("BRIGHTNESS_CLAMP_ON");
            mat.DisableKeyword("BRIGHTNESS_CLAMP_OFF");
        }

        public static List<string> SetupSpritePaths(string filepath, int frameCount)
        {
            List<string> sl = new List<string> { };
            for (int i = 1; i < frameCount; i++)
            {
                string frameNum = "";
                if (i <= 9)
                {
                    frameNum = "_00" + i.ToString();
                }
                else if (i > 9 && i <= 99)
                {
                    frameNum = "_0" + i.ToString();
                }
                else if (i > 99)
                {
                    frameNum = "_" + i.ToString();
                }
                string s = filepath + frameNum + ".png";
                sl.Add(s);
            }

            return sl;
        }
        public static void GenerateSpriteAnimator(GameObject targetObject, tk2dSpriteAnimation library = null, int DefaultClipId = 0, float AdditionalCameraVisibilityRadius = 0, bool AnimateDuringBossIntros = false, bool AlwaysIgnoreTimeScale = false, bool ignoreTimeScale = false, bool ForceSetEveryFrame = false, bool playAutomatically = false, bool IsFrameBlendedAnimation = false, float clipTime = 0, float ClipFps = 15, bool deferNextStartClip = false, bool alwaysUpdateOffscreen = false, bool maximumDeltaOneFrame = false)
        {
            if (targetObject.GetComponent<tk2dSpriteAnimator>()) { UnityEngine.Object.Destroy(targetObject.GetComponent<tk2dSpriteAnimator>()); }
            tk2dSpriteAnimator newAnimator = targetObject.AddComponent<tk2dSpriteAnimator>();
            newAnimator.Library = library;
            newAnimator.DefaultClipId = DefaultClipId;
            newAnimator.AdditionalCameraVisibilityRadius = AdditionalCameraVisibilityRadius;
            newAnimator.AnimateDuringBossIntros = AnimateDuringBossIntros;
            newAnimator.AlwaysIgnoreTimeScale = AlwaysIgnoreTimeScale;
            newAnimator.ignoreTimeScale = ignoreTimeScale;
            newAnimator.ForceSetEveryFrame = ForceSetEveryFrame;
            newAnimator.playAutomatically = playAutomatically;
            newAnimator.IsFrameBlendedAnimation = IsFrameBlendedAnimation;
            newAnimator.clipTime = clipTime;
            newAnimator.ClipFps = ClipFps;
            newAnimator.deferNextStartClip = deferNextStartClip;
            newAnimator.alwaysUpdateOffscreen = alwaysUpdateOffscreen;
            newAnimator.maximumDeltaOneFrame = maximumDeltaOneFrame;

            return;
        }
        public static tk2dSpriteAnimationClip AddAnimation2(tk2dSpriteAnimator targetAnimator, tk2dSpriteCollectionData collection, List<string> spriteNameList, string clipName, tk2dSpriteAnimationClip.WrapMode wrapMode = tk2dSpriteAnimationClip.WrapMode.Once, int frameRate = 15, int loopStart = 0, float minFidgetDuration = 0.5f, float maxFidgetDuration = 1)
        {
            if (!targetAnimator.Library)
            {
                targetAnimator.Library = targetAnimator.gameObject.AddComponent<tk2dSpriteAnimation>();
                targetAnimator.Library.clips = new tk2dSpriteAnimationClip[0];
            }
            List<tk2dSpriteAnimationFrame> animationList = new List<tk2dSpriteAnimationFrame>();
            for (int i = 0; i < spriteNameList.Count; i++)
            {
                tk2dSpriteDefinition spriteDefinition = collection.GetSpriteDefinition(spriteNameList[i]);
                if (spriteDefinition != null && spriteDefinition.Valid)
                {
                    animationList.Add(
                        new tk2dSpriteAnimationFrame
                        {
                            spriteCollection = collection,
                            spriteId = collection.GetSpriteIdByName(spriteNameList[i]),
                            invulnerableFrame = false,
                            groundedFrame = true,
                            requiresOffscreenUpdate = false,
                            eventAudio = string.Empty,
                            eventVfx = string.Empty,
                            eventStopVfx = string.Empty,
                            eventLerpEmissive = false,
                            eventLerpEmissiveTime = 0.5f,
                            eventLerpEmissivePower = 30,
                            forceMaterialUpdate = false,
                            finishedSpawning = false,
                            triggerEvent = false,
                            eventInfo = string.Empty,
                            eventInt = 0,
                            eventFloat = 0,
                            eventOutline = tk2dSpriteAnimationFrame.OutlineModifier.Unspecified,
                        }
                    );
                }
            }

            if (animationList.Count <= 0)
            {
                ETGModConsole.Log("[ExpandTheGungeon] AddAnimation: ERROR! Animation list is empty! No valid sprites found in specified list!");
                return null;
            }

            tk2dSpriteAnimationClip animationClip = new tk2dSpriteAnimationClip()
            {
                name = clipName,
                frames = animationList.ToArray(),
                fps = frameRate,
                wrapMode = wrapMode,
                loopStart = loopStart,
                minFidgetDuration = minFidgetDuration,
                maxFidgetDuration = maxFidgetDuration,
            };
            Array.Resize(ref targetAnimator.Library.clips, targetAnimator.Library.clips.Length + 1);
            targetAnimator.Library.clips[targetAnimator.Library.clips.Length - 1] = animationClip;
            return animationClip;
        }

        public static Vector2 DegreeToVector2(this float degree)
        {
            return (degree * Mathf.Deg2Rad).RadianToVector2();
        }
        public static Vector2 RadianToVector2(this float radian)
        {
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }

        public static float KeyTime(GungeonActions.GungeonActionType action, PlayerController user)
        {
            return BraveInput.GetInstanceForPlayer(user.PlayerIDX).ActiveActions.GetActionFromType(action).PressedDuration;
        }

        public static bool KeyDown(GungeonActions.GungeonActionType action, PlayerController user)
        {
            return BraveInput.GetInstanceForPlayer(user.PlayerIDX).ActiveActions.GetActionFromType(action).WasPressed;
        }

        public static bool Key(GungeonActions.GungeonActionType action, PlayerController user)
        {
            return BraveInput.GetInstanceForPlayer(user.PlayerIDX).ActiveActions.GetActionFromType(action).IsPressed;
        }

        public static GameActorFireEffect fireEffect = PickupObjectDatabase.GetById(295).GetComponent<BulletStatusEffectItem>().FireModifierEffect;
        public static GameActorHealthEffect poisonEffect = PickupObjectDatabase.GetById(204).GetComponent<BulletStatusEffectItem>().HealthModifierEffect;
        public static GameActorCharmEffect charmEffect = PickupObjectDatabase.GetById(527).GetComponent<BulletStatusEffectItem>().CharmModifierEffect;

        public static GameObject laserSightPrefab = LoadHelper.LoadAssetFromAnywhere("assets/resourcesbundle/global vfx/vfx_lasersight.prefab") as GameObject;
        public static GameObject RenderLaserSight(Vector2 position, float length, float width, float angle, bool alterColour = false, Color? colour = null)
        {
            GameObject gameObject = SpawnManager.SpawnVFX(laserSightPrefab, position, Quaternion.Euler(0, 0, angle));

            tk2dTiledSprite component2 = gameObject.GetComponent<tk2dTiledSprite>();
            float newWidth = 1f;
            if (width != -1) newWidth = width;
            component2.dimensions = new Vector2(length, newWidth);
            if (alterColour && colour != null)
            {
                component2.usesOverrideMaterial = true;
                component2.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
                component2.sprite.renderer.material.SetColor("_OverrideColor", (Color)colour);
                component2.sprite.renderer.material.SetColor("_EmissiveColor", (Color)colour);
                component2.sprite.renderer.material.SetFloat("_EmissivePower", 100);
                component2.sprite.renderer.material.SetFloat("_EmissiveColorPower", 1.55f);
            }
            return gameObject;
        }
        public static PlayerController ProjectilePlayerOwner(this Projectile bullet)
        {
            if (bullet && bullet.Owner && bullet.Owner is PlayerController) return bullet.Owner as PlayerController;
            else return null;
        }
        public static Vector2 GetPositionOfNearestEnemy(this Vector2 startPosition, bool canTargetNonRoomClear, bool targetSprite = false, List<AIActor> excludedActors = null)
        {
            List<AIActor> exclude = new List<AIActor>();
            if (excludedActors != null && excludedActors.Count > 0) exclude.AddRange(excludedActors);
            Func<AIActor, bool> isValid = (AIActor a) => a && a.HasBeenEngaged && a.healthHaver && a.healthHaver.IsVulnerable && a.healthHaver.IsAlive && !a.IsGone && !exclude.Contains(a);
            IntVector2 intVectorStartPos = startPosition.ToIntVector2();
            RoomHandler.ActiveEnemyType enemyType = RoomHandler.ActiveEnemyType.RoomClear;
            if (canTargetNonRoomClear) enemyType = RoomHandler.ActiveEnemyType.All;
            AIActor closestToPosition = BraveUtility.GetClosestToPosition<AIActor>(GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVectorStartPos).GetActiveEnemies(enemyType), startPosition, isValid, new AIActor[] { });
            if (closestToPosition == null) return Vector2.zero;
            if (targetSprite && closestToPosition.sprite) return closestToPosition.sprite.WorldCenter;
            else return closestToPosition.specRigidbody.UnitCenter;
        }

        public static Vector2 GetVectorToNearestEnemy(this Vector2 bulletPosition, float angleFromAim = 0, float angleVariance = 0, PlayerController playerToScaleAccuracyOff = null, List<AIActor> excludedActors = null)
        {
            List<AIActor> exclude = new List<AIActor>();
            if (excludedActors != null && excludedActors.Count > 0) exclude.AddRange(excludedActors);
            Vector2 dirVec = UnityEngine.Random.insideUnitCircle;
            Func<AIActor, bool> isValid = (AIActor a) => a && a.HasBeenEngaged && a.healthHaver && a.healthHaver.IsVulnerable && !exclude.Contains(a);
            IntVector2 bulletPositionIntVector2 = bulletPosition.ToIntVector2();
            AIActor closestToPosition = BraveUtility.GetClosestToPosition<AIActor>(GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(bulletPositionIntVector2).GetActiveEnemies(RoomHandler.ActiveEnemyType.All), bulletPosition, isValid, new AIActor[]
            {

            });
            if (closestToPosition)
            {
                dirVec = closestToPosition.CenterPosition - bulletPosition;
            }
            if (angleFromAim != 0)
            {
                dirVec = dirVec.Rotate(angleFromAim);
            }
            if (angleVariance != 0)
            {
                if (playerToScaleAccuracyOff != null) angleVariance *= playerToScaleAccuracyOff.stats.GetStatValue(PlayerStats.StatType.Accuracy);
                float positiveVariance = angleVariance * 0.5f;
                float negativeVariance = positiveVariance * -1f;
                float finalVariance = UnityEngine.Random.Range(negativeVariance, positiveVariance);
                dirVec = dirVec.Rotate(finalVariance);
            }
            return dirVec;
        }
        public static List<T> ConstructListOfSameValues<T>(T value, int length)
        {
            List<T> list = new List<T>();
            for (int i = 0; i < length; i++)
            {
                list.Add(value);
            }
            return list;
        }

        public static string[] JoinAtStart(string prepender, string[] array)
        {
            List<string> l = new List<string> { };
            if (array.Any())
            {
                foreach (string s in array)
                {
                    string newString = prepender + s;

                    l.Add(newString);
                }

            }
            return l.ToArray();
        }

        public static List<string> JoinAtStart(string prepender, List<string> list)
        {
            List<string> l = new List<string> { };
            if (list.Any())
            {
                foreach (string s in list)
                {
                    string newString = prepender + s;

                    l.Add(newString);
                }

            }

            return l;
        }


        public static List<T> RandomNoRepeats<T>(List<T> candidates, int count)
        {
            List<T> outcomes = new List<T> { };
            int i = 0;
            do
            {
                i++;
                int V = UnityEngine.Random.Range(0, candidates.Count);
                if (!outcomes.Contains(candidates[V]))
                {
                    outcomes.Add(candidates[V]);
                }
                

            } while (i < count * 3 && outcomes.Count < count);

            return outcomes;
        }

        public static List<S> RandomRemoveChosen<S>(List<S> candidates, int count)
        {
            List<S> outcomes = new List<S> { };
            int i = 0;
            do
            {
                i++;
                int V = UnityEngine.Random.Range(0, candidates.Count);
                if (!outcomes.Contains(candidates[V]))
                {
                    outcomes.Add(candidates[V]);
                }

            } while (i < count * 3 && outcomes.Count < count);
            foreach(S item in outcomes)
            {
                if (candidates.Contains(item))
                {
                    candidates.Remove(item);
                }
            }
            return outcomes;
        }
        public static GameObject Mines_Cave_In = ResourceManager.LoadAssetBundle("shared_auto_002").LoadAsset<GameObject>("Mines_Cave_In");

        public static SpeculativeRigidbody GenerateOrAddToRigidBody(GameObject targetObject, CollisionLayer collisionLayer, PixelCollider.PixelColliderGeneration colliderGenerationMode = PixelCollider.PixelColliderGeneration.Tk2dPolygon, bool collideWithTileMap = false, bool CollideWithOthers = true, bool CanBeCarried = true, bool CanBePushed = false, bool RecheckTriggers = false, bool IsTrigger = false, bool replaceExistingColliders = false, bool UsesPixelsAsUnitSize = false, IntVector2? dimensions = null, IntVector2? offset = null)
        {
            SpeculativeRigidbody m_CachedRigidBody = GameObjectExtensions.GetOrAddComponent<SpeculativeRigidbody>(targetObject);
            m_CachedRigidBody.CollideWithOthers = CollideWithOthers;
            m_CachedRigidBody.CollideWithTileMap = collideWithTileMap;
            m_CachedRigidBody.Velocity = Vector2.zero;
            m_CachedRigidBody.MaxVelocity = Vector2.zero;
            m_CachedRigidBody.ForceAlwaysUpdate = false;
            m_CachedRigidBody.CanPush = false;
            m_CachedRigidBody.CanBePushed = CanBePushed;
            m_CachedRigidBody.PushSpeedModifier = 1f;
            m_CachedRigidBody.CanCarry = false;
            m_CachedRigidBody.CanBeCarried = CanBeCarried;
            m_CachedRigidBody.PreventPiercing = false;
            m_CachedRigidBody.SkipEmptyColliders = false;
            m_CachedRigidBody.RecheckTriggers = RecheckTriggers;
            m_CachedRigidBody.UpdateCollidersOnRotation = false;
            m_CachedRigidBody.UpdateCollidersOnScale = false;

            IntVector2 Offset = IntVector2.Zero;
            IntVector2 Dimensions = IntVector2.Zero;
            if (colliderGenerationMode != PixelCollider.PixelColliderGeneration.Tk2dPolygon)
            {
                if (dimensions.HasValue)
                {
                    Dimensions = dimensions.Value;
                    if (!UsesPixelsAsUnitSize)
                    {
                        Dimensions = (new IntVector2(Dimensions.x * 16, Dimensions.y * 16));
                    }
                }
                if (offset.HasValue)
                {
                    Offset = offset.Value;
                    if (!UsesPixelsAsUnitSize)
                    {
                        Offset = (new IntVector2(Offset.x * 16, Offset.y * 16));
                    }
                }
            }
            PixelCollider m_CachedCollider = new PixelCollider()
            {
                ColliderGenerationMode = colliderGenerationMode,
                CollisionLayer = collisionLayer,
                IsTrigger = IsTrigger,
                BagleUseFirstFrameOnly = (colliderGenerationMode == PixelCollider.PixelColliderGeneration.Tk2dPolygon),
                SpecifyBagelFrame = string.Empty,
                BagelColliderNumber = 0,
                ManualOffsetX = Offset.x,
                ManualOffsetY = Offset.y,
                ManualWidth = Dimensions.x,
                ManualHeight = Dimensions.y,
                ManualDiameter = 0,
                ManualLeftX = 0,
                ManualLeftY = 0,
                ManualRightX = 0,
                ManualRightY = 0
            };

            if (replaceExistingColliders | m_CachedRigidBody.PixelColliders == null)
            {
                m_CachedRigidBody.PixelColliders = new List<PixelCollider> { m_CachedCollider };
            }
            else
            {
                m_CachedRigidBody.PixelColliders.Add(m_CachedCollider);
            }

            if (m_CachedRigidBody.sprite && colliderGenerationMode == PixelCollider.PixelColliderGeneration.Tk2dPolygon)
            {
                Bounds bounds = m_CachedRigidBody.sprite.GetBounds();
                m_CachedRigidBody.sprite.GetTrueCurrentSpriteDef().colliderVertices = new Vector3[] { bounds.center - bounds.extents, bounds.center + bounds.extents };
                // m_CachedRigidBody.ForceRegenerate();
                // m_CachedRigidBody.RegenerateCache();
            }

            return m_CachedRigidBody;
        }

    }
    //
    // End of first part
    //

    public class StickyProjectile : MonoBehaviour
    {
        private StickyProjectile()
        {
            this.shouldExplodeOnReload = false;
            this.maxLifeTime = 15;
            this.destroyOnGunChanged = false;
            explosionDamageBasedOnProjectileDamage = false;
            hasDetTimer = false;
            shouldExplode = false;
            explosionData = null;
        }
        public void Start()
        {
            currentObject = this.GetComponent<Projectile>();
            if (currentObject)
            {
                currentObject.OnHitEnemy += HandleHit;
            }
            Library.stickiesAlive.Add(this);
            if (Library.stickiesAlive.Count >= 15)
            {
                StickyProjectile oldestSticky = Library.stickiesAlive.First();
                if (oldestSticky.shouldExplode)
                {
                    oldestSticky.Explode();
                }
                else
                {
                    Destroy(oldestSticky);
                }
            }
            if (explosionData == null)
            {
                explosionData = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultSmallExplosionData;
                explosionData.damageToPlayer = 0;
                explosionData.preventPlayerForce = true;
            }
            this.StartCoroutine(Decease());
        }
        private void HandleHit(Projectile projectile, SpeculativeRigidbody otherBody, bool fatal)
        {
            if (otherBody.aiActor != null && !otherBody.healthHaver.IsDead && otherBody.aiActor.behaviorSpeculator && !otherBody.aiActor.IsHarmlessEnemy)
            {
                projectile.DestroyMode = Projectile.ProjectileDestroyMode.DestroyComponent;
                objectToLookOutFor = projectile.gameObject;
                objectToLookOutFor.transform.parent = otherBody.transform;
                PlayerController player = projectile.Owner as PlayerController;
                if (player)
                {
                    sourceGun = player.CurrentGun;
                    this.player = player;
                    player.CurrentGun.OnReloadPressed += OnPlayerReloaded;
                    player.GunChanged += GunChanged;
                }
            }
        }

        private void GunChanged(Gun arg1, Gun arg2, bool arg3)
        {   
            Destroy(this.gameObject);    
        }

        private void OnPlayerReloaded(PlayerController arg1, Gun arg2, bool actual)
        {
            Explode();
        }

        private void Explode()
        {
            Exploder.Explode(parent.CenterPosition, explosionData, Vector2.zero, null, true);
            shouldExplode = false;
            Destroy(this.gameObject);
        }

        private IEnumerator Decease()
        {
            float timer = 0;
            while (timer < maxLifeTime)
            {
                timer += BraveTime.DeltaTime;
                yield return null;
            }
           
            Destroy(this.gameObject);
            yield break;
        }

        private void OnDestroy()
        {
            if (destroyOnGunChanged)
            {
                if (player != null)
                {
                    player.GunChanged -= GunChanged;
                }
            }
            if (shouldExplodeOnReload)
            {
                if (sourceGun != null)
                {
                    sourceGun.OnReloadPressed -= OnPlayerReloaded;
                }
            }
            if (shouldExplode)
            {
                Exploder.Explode(base.GetComponent<AIActor>().CenterPosition, explosionData, Vector2.zero, null, true);
            }
            Library.stickiesAlive.Remove(this);
           
        }

        public Projectile currentObject;
        public GameObject objectToLookOutFor;
        public Material materialToCopy;
        public tk2dSprite objectSprite;
        public AIActor parent;
        public Gun ToCheckReloadFor;
        private PlayerController player;

        public ExplosionData explosionData;
        public bool destroyOnGunChanged;
        public bool shouldExplodeOnReload;
        public bool explosionDamageBasedOnProjectileDamage;
        public bool hasDetTimer;
        public bool shouldExplode;
        public float maxLifeTime;
        public float detTimer;
        private Gun sourceGun;
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

        public override void Move()
        {
            base.Move();
        }
        public override void OnDestroy()
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
            if (timer <= .005f)
            {
                timer += BraveTime.DeltaTime;
            }
            if (accelerate && timer > .005f && proj.Speed != terminalVelocity)
            {
                proj.Speed *= (incrementRate + 1);
            }
            else if (!accelerate && timer > .005f && proj.Speed != terminalVelocity)
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
        
    public class PreventOnActiveEffects : MonoBehaviour
    {
        string pisslord = "yeah this is basically an empty component for making smth work. sue me.";
    }
    
    public class LinkedPickupObjectBehav : MonoBehaviour
    {
        private void Start()
        {
            if (base.gameObject.GetComponent<PickupObject>())
            {
                pickupObject = base.GetComponent<PickupObject>();

            }
        }

        private void TestCode()
        {
            ETGModConsole.Log("test");
        }

        public string linkGroup;

        private PickupObject pickupObject;
    }
    public class LightningProjectileComp : MonoBehaviour
    {
        public LightningProjectileComp()
        {
            initialAngle = float.NegativeInfinity;
            lightningWidth = -1;
            targetEnemies = true;
            lightingColor = new Color(130f / 255f, 230f / 255f, 2255f / 255f);
        }
        private void Start()
        {
            self = base.GetComponent<Projectile>();
            self.OnHitEnemy += OnHitEnemy;
            laserVFX = new List<GameObject>();
            owner = self.ProjectilePlayerOwner();
            if (self.GetComponent<BounceProjModifier>()) self.GetComponent<BounceProjModifier>().OnBounceContext += OnBounce;
            StartCoroutine(moveLightning());

        }
        private void OnBounce(BounceProjModifier mod, SpeculativeRigidbody collider)
        {
            initialAngle = self.Direction.ToAngle();
        }
        private void Update()
        {
            if (self)
            {
                if (self.GetElapsedDistance() > tilesSinceLastCheck)
                {
                    tilesSinceLastCollision += (self.GetElapsedDistance() - tilesSinceLastCheck);

                    tilesSinceLastCheck = self.GetElapsedDistance();
                }
            }
            if (tilesSinceLastCollision > 3 && midEnemyZapping) midEnemyZapping = false;
        }
        private void OnHitEnemy(Projectile self, SpeculativeRigidbody enemy, bool fatal)
        {
            if (enemy && enemy.aiActor)
            {
                if (enemy.aiActor != lastHitEnemy)
                {
                    if (lastHitEnemy != null) secondToLastHitEnemy = lastHitEnemy;
                    lastHitEnemy = enemy.aiActor;
                }
                tilesSinceLastCollision = 0;

                //Check if there's another valid enemy around to arc to
                if (targetEnemies)
                {
                    if (Vector2.Distance(enemy.UnitCenter, enemy.UnitCenter.GetPositionOfNearestEnemy(true, true, new List<AIActor>() { lastHitEnemy, secondToLastHitEnemy })) < 3)
                    {
                        PierceProjModifier piercing = self.gameObject.GetComponent<PierceProjModifier>();
                        if (piercing != null)
                        {
                            piercing.penetration++;
                        }
                        else
                        {
                            self.gameObject.AddComponent<PierceProjModifier>();
                        }

                        //midEnemyZapping = true;
                        float newArc = self.specRigidbody.UnitCenter.GetVectorToNearestEnemy(0, 0, self.ProjectilePlayerOwner(), new List<AIActor>() { lastHitEnemy, secondToLastHitEnemy }).ToAngle();
                        self.SendInDirection(newArc.DegreeToVector2(), true, true); //Send the projectile in the new direction

                        TriggerLightningBreak();

                    }
                }

            }
        }
        private IEnumerator moveLightning()
        {
            while (self)
            {
                yield return new WaitForSeconds(LightningTime);
                if (!midEnemyZapping)
                {

                    //Determine the new direction of the projectile
                    if (initialAngle == float.NegativeInfinity) initialAngle = self.Direction.ToAngle(); //If initial angle is not set to the placeholder, set it
                    float newArc = MathsAndLogicHelper.GetAccuracyAngled(initialAngle, 80, owner); //Determine accuracy

                    if (targetEnemies)
                    {
                        if (Vector2.Distance(self.specRigidbody.UnitCenter, self.specRigidbody.UnitCenter.GetPositionOfNearestEnemy(true, true, new List<AIActor>() { lastHitEnemy, secondToLastHitEnemy })) < 3)
                        {
                            newArc = self.specRigidbody.UnitCenter.GetVectorToNearestEnemy(0, 5, self.ProjectilePlayerOwner(), new List<AIActor>() { lastHitEnemy, secondToLastHitEnemy }).ToAngle();
                        }
                    }

                    self.SendInDirection(newArc.DegreeToVector2(), true, true); //Send the projectile in the new direction


                    TriggerLightningBreak();
                }

            }
            yield break;
        }
        private void TriggerLightningBreak()
        {
            //Erase the TiledSpriteConnector from the last laser created, if the last laser exists.
            if (lastLaser != null)
            {
                if (lastLaser.GetComponent<TiledSpriteConnector>() != null) UnityEngine.Object.Destroy(lastLaser.GetComponent<TiledSpriteConnector>());
                lastLaser = null;
            }

            //Create New Laser Sight
            GameObject laserSight = Library.RenderLaserSight(self.specRigidbody.UnitCenter, 1, lightningWidth, self.Direction.ToAngle(), true, lightingColor);

            TiledSpriteConnector connector = laserSight.AddComponent<TiledSpriteConnector>();
            connector.eraseSpriteIfTargetOrSourceNull = false;
            connector.sourceRigidbody = self.specRigidbody;
            connector.eraseComponentIfTargetOrSourceNull = true;
            connector.targetVector = self.specRigidbody.UnitCenter;
            connector.usesVector = true;

            lastLaser = laserSight;
            laserVFX.Add(laserSight);
        }
        private float LightningTime
        {
            get
            {
                //ETGModConsole.Log(BraveTime.DeltaTime.ToString());
                return (0.005f / Time.timeScale);
            }
        }
        private void OnDestroy()
        {
            if (laserVFX.Count > 0) ETGMod.StartGlobalCoroutine(deleteLasers(laserVFX, LightningTime, logDebug));
        }
        private static IEnumerator deleteLasers(List<GameObject> lasers, float delay, bool logDebug)
        {
            if (logDebug) ETGModConsole.Log("Running laser deletion code");
            yield return new WaitForSeconds(delay);

            List<GameObject> reversedList = new List<GameObject>();
            for (int i = lasers.Count - 1; i >= 0; i--)
            {
                if (logDebug) ETGModConsole.Log($"Checking laser at index ({i}) in laser List.");
                if (lasers[i] != null)
                {
                    reversedList.Add(lasers[i]);
                    if (logDebug) ETGModConsole.Log($"Laser at index ({i}) was valid, adding to reversedList at index ({reversedList.Count - 1}).");
                }
            }
            for (int i = reversedList.Count - 1; i >= 0; i--)
            {
                if (logDebug) ETGModConsole.Log($"Checking laser at index ({i}) in reversedList.");
                if (reversedList[i] != null)
                {
                    if (logDebug) ETGModConsole.Log($"Laser at index ({i}) in reversedList exists and will be destroyed.");

                    UnityEngine.Object.Destroy(reversedList[i]);
                }
                else { if (logDebug) ETGModConsole.Log($"Laser at index ({i}) was NULL in reversedList."); }
                yield return new WaitForSeconds(delay);

            }
            yield break;
        }
        //Public
        public Color32 lightingColor;
        public bool logDebug;
        public float lightningWidth;
        public bool targetEnemies;
        //Private
        private float initialAngle;
        private GameObject lastLaser;
        private List<GameObject> laserVFX;
        private Projectile self;
        private PlayerController owner;

        private AIActor lastHitEnemy;
        private AIActor secondToLastHitEnemy;
        private bool midEnemyZapping;
        private float tilesSinceLastCollision;
        private float tilesSinceLastCheck;
    }
    public class TiledSpriteConnector : MonoBehaviour
    {
        private void Start()
        {
            tiledSprite = base.GetComponent<tk2dTiledSprite>();
        }
        private void Update()
        {
            if (sourceRigidbody)
            {
                Vector2 unitCenter = sourceRigidbody.UnitCenter;
                Vector2 unitCenter2 = Vector2.zero;
                if (usesVector && targetVector != Vector2.zero) unitCenter2 = targetVector;
                else if (targetRigidbody) unitCenter2 = targetRigidbody.UnitCenter;
                if (unitCenter2 != Vector2.zero)
                {
                    tiledSprite.transform.position = unitCenter;
                    Vector2 vector = unitCenter2 - unitCenter;
                    float num = BraveMathCollege.Atan2Degrees(vector.normalized);
                    int num2 = Mathf.RoundToInt(vector.magnitude / 0.0625f);
                    tiledSprite.dimensions = new Vector2((float)num2, tiledSprite.dimensions.y);
                    tiledSprite.transform.rotation = Quaternion.Euler(0f, 0f, num);
                    tiledSprite.UpdateZDepth();

                }
                else
                {
                    if (eraseSpriteIfTargetOrSourceNull) UnityEngine.Object.Destroy(tiledSprite.gameObject);
                    else if (eraseComponentIfTargetOrSourceNull) UnityEngine.Object.Destroy(this);
                }
            }
            else
            {
                if (eraseSpriteIfTargetOrSourceNull) UnityEngine.Object.Destroy(tiledSprite.gameObject);
                else if (eraseComponentIfTargetOrSourceNull) UnityEngine.Object.Destroy(this);
            }
        }

        public SpeculativeRigidbody sourceRigidbody;
        public SpeculativeRigidbody targetRigidbody;
        public Vector2 targetVector;
        public bool usesVector;
        public bool eraseSpriteIfTargetOrSourceNull;
        public bool eraseComponentIfTargetOrSourceNull;
        private tk2dTiledSprite tiledSprite;
    }
    class SpecialBlankModificationItem : BlankModificationItem
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
    public class ImprovedAfterImage : BraveBehaviour
    {

        public ImprovedAfterImage()
        {
            shaders = new List<Shader>
            {
                ShaderCache.Acquire("Brave/Internal/RainbowChestShader"),
                ShaderCache.Acquire("Brave/Internal/GlitterPassAdditive"),
                ShaderCache.Acquire("Brave/Internal/HologramShader"),
                ShaderCache.Acquire("Brave/Internal/HighPriestAfterImage")
            };
            //shaders.Add(ShaderCache.Acquire("Brave/ItemSpecific/MetalSkinShader"));
            this.IsRandomShader = false;
            this.spawnShadows = true;
            this.shadowTimeDelay = 0.1f;
            this.shadowLifetime = 0.6f;
            this.minTranslation = 0.2f;
            this.maxEmission = 800f;
            this.minEmission = 100f;
            this.targetHeight = -2f;
            this.dashColor = new Color(1f, 0f, 1f, 1f);
            this.m_activeShadows = new LinkedList<Shadow>();
            this.m_inactiveShadows = new LinkedList<Shadow>();
        }

        public void Start()
        {
            if (this.OptionalImageShader != null)
            {
                this.OverrideImageShader = this.OptionalImageShader;
            }
            if (base.transform.parent != null && base.transform.parent.GetComponent<Projectile>() != null)
            {
                base.transform.parent.GetComponent<Projectile>().OnDestruction += this.ProjectileDestruction;
            }
            this.m_lastSpawnPosition = base.transform.position;
        }

        private void ProjectileDestruction(Projectile source)
        {
            if (this.m_activeShadows.Count > 0)
            {
                GameManager.Instance.StartCoroutine(this.HandleDeathShadowCleanup());
            }
        }

        public void LateUpdate()
        {
            if (this.spawnShadows && !this.m_previousFrameSpawnShadows)
            {
                this.m_spawnTimer = this.shadowTimeDelay;
            }
            this.m_previousFrameSpawnShadows = this.spawnShadows;
            LinkedListNode<ImprovedAfterImage.Shadow> next;
            for (LinkedListNode<ImprovedAfterImage.Shadow> linkedListNode = this.m_activeShadows.First; linkedListNode != null; linkedListNode = next)
            {
                next = linkedListNode.Next;
                linkedListNode.Value.timer -= BraveTime.DeltaTime;
                if (linkedListNode.Value.timer <= 0f)
                {
                    this.m_activeShadows.Remove(linkedListNode);
                    this.m_inactiveShadows.AddLast(linkedListNode);
                    if (linkedListNode.Value.sprite)
                    {
                        linkedListNode.Value.sprite.renderer.enabled = false;
                    }
                }
                else if (linkedListNode.Value.sprite)
                {
                    float num = linkedListNode.Value.timer / this.shadowLifetime;
                    Material sharedMaterial = linkedListNode.Value.sprite.renderer.sharedMaterial;
                    sharedMaterial.SetFloat("_EmissivePower", Mathf.Lerp(this.maxEmission, this.minEmission, num));
                    sharedMaterial.SetFloat("_Opacity", num);
                }
            }
            if (this.spawnShadows)
            {
                if (this.m_spawnTimer > 0f)
                {
                    this.m_spawnTimer -= BraveTime.DeltaTime;
                }
                if (this.m_spawnTimer <= 0f && Vector2.Distance(this.m_lastSpawnPosition, base.transform.position) > this.minTranslation)
                {
                    this.SpawnNewShadow();
                    this.m_spawnTimer += this.shadowTimeDelay;
                    this.m_lastSpawnPosition = base.transform.position;
                }
            }
        }

        private IEnumerator HandleDeathShadowCleanup()
        {
            while (this.m_activeShadows.Count > 0)
            {
                LinkedListNode<ImprovedAfterImage.Shadow> next;
                for (LinkedListNode<ImprovedAfterImage.Shadow> node = this.m_activeShadows.First; node != null; node = next)
                {
                    next = node.Next;
                    node.Value.timer -= BraveTime.DeltaTime;
                    if (node.Value.timer <= 0f)
                    {
                        this.m_activeShadows.Remove(node);
                        this.m_inactiveShadows.AddLast(node);
                        if (node.Value.sprite)
                        {
                            node.Value.sprite.renderer.enabled = false;
                        }
                    }
                    else if (node.Value.sprite)
                    {
                        float num = node.Value.timer / this.shadowLifetime;
                        Material sharedMaterial = node.Value.sprite.renderer.sharedMaterial;
                        sharedMaterial.SetFloat("_EmissivePower", Mathf.Lerp(this.maxEmission, this.minEmission, num));
                        sharedMaterial.SetFloat("_Opacity", num);
                    }
                }
                yield return null;
            }
            yield break;
        }

        public override void OnDestroy()
        {
            GameManager.Instance.StartCoroutine(this.HandleDeathShadowCleanup());
            base.OnDestroy();
        }


        private void SpawnNewShadow()
        {
            if (this.m_inactiveShadows.Count == 0)
            {
                this.CreateInactiveShadow();
            }
            LinkedListNode<ImprovedAfterImage.Shadow> first = this.m_inactiveShadows.First;
            tk2dSprite sprite = first.Value.sprite;
            this.m_inactiveShadows.RemoveFirst();
            if (!sprite || !sprite.renderer)
            {
                return;
            }
            first.Value.timer = this.shadowLifetime;
            sprite.SetSprite(base.sprite.Collection, base.sprite.spriteId);
            sprite.transform.position = base.sprite.transform.position;
            sprite.transform.rotation = base.sprite.transform.rotation;
            sprite.scale = base.sprite.scale;
            sprite.usesOverrideMaterial = true;
            sprite.IsPerpendicular = true;
            if (sprite.renderer && IsRandomShader)
            {
                sprite.renderer.enabled = true;
                sprite.renderer.material.shader = shaders[(int)UnityEngine.Random.Range(0, shaders.Count)];

                if (sprite.renderer.material.shader == shaders[3])
                {
                    sprite.renderer.sharedMaterial.SetFloat("_EmissivePower", this.minEmission);
                    sprite.renderer.sharedMaterial.SetFloat("_Opacity", 1f);
                    sprite.renderer.sharedMaterial.SetColor("_DashColor", Color.HSVToRGB(UnityEngine.Random.value, 1.0f, 1.0f));
                }
                if (sprite.renderer.material.shader == shaders[0])
                {
                    sprite.renderer.sharedMaterial.SetFloat("_AllColorsToggle", 1f);
                }
            }

            else if (sprite.renderer)
            {

                sprite.renderer.enabled = true;
                sprite.renderer.material.shader = (this.OverrideImageShader ?? ShaderCache.Acquire("Brave/Internal/HighPriestAfterImage"));
                sprite.renderer.sharedMaterial.SetFloat("_EmissivePower", this.minEmission);
                sprite.renderer.sharedMaterial.SetFloat("_Opacity", 1f);
                sprite.renderer.sharedMaterial.SetColor("_DashColor", this.dashColor);
                sprite.renderer.sharedMaterial.SetFloat("_AllColorsToggle", 1f);
            }

            sprite.HeightOffGround = this.targetHeight;
            sprite.UpdateZDepth();
            this.m_activeShadows.AddLast(first);
        }

        public bool IsRandomShader;

        private void CreateInactiveShadow()
        {
            GameObject gameObject = new GameObject("after image");
            if (this.UseTargetLayer)
            {
                gameObject.layer = LayerMask.NameToLayer(this.TargetLayer);
            }
            tk2dSprite sprite = gameObject.AddComponent<tk2dSprite>();
            gameObject.transform.parent = SpawnManager.Instance.VFX;
            this.m_inactiveShadows.AddLast(new ImprovedAfterImage.Shadow
            {
                timer = this.shadowLifetime,
                sprite = sprite
            });
        }


        public bool spawnShadows;

        public float shadowTimeDelay;

        public float shadowLifetime;

        public float minTranslation;

        public float maxEmission;

        public float minEmission;

        public float targetHeight;

        public Color dashColor;

        public Shader OptionalImageShader;

        public bool UseTargetLayer;

        public string TargetLayer;

        [NonSerialized]
        public Shader OverrideImageShader;

        private readonly LinkedList<ImprovedAfterImage.Shadow> m_activeShadows;

        private readonly LinkedList<ImprovedAfterImage.Shadow> m_inactiveShadows;

        private readonly List<Shader> shaders;

        private float m_spawnTimer;

        private Vector2 m_lastSpawnPosition;

        private bool m_previousFrameSpawnShadows;

        private class Shadow
        {
            public float timer;

            public tk2dSprite sprite;
        }
    }


}
