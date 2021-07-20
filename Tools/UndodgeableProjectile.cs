using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using ItemAPI;
using UnityEngine;
using System.Collections;
using System.Reflection;
using MonoMod.RuntimeDetour;


namespace Items
{
	public class UndodgeableProjectile : Projectile
	{
		protected override void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
		{
			if (otherRigidbody == Shooter && !allowSelfShooting)
			{
				PhysicsEngine.SkipCollision = true;
				return;
			}
			if (otherRigidbody.gameActor != null && otherRigidbody.gameActor is PlayerController && (!collidesWithPlayer || (otherRigidbody.gameActor as PlayerController).IsGhost || (otherRigidbody.gameActor as PlayerController).IsEthereal))
			{
				PhysicsEngine.SkipCollision = true;
				return;
			}
			if (otherRigidbody.aiActor)
			{
				if (Owner is PlayerController && !otherRigidbody.aiActor.IsNormalEnemy)
				{
					PhysicsEngine.SkipCollision = true;
					return;
				}
				if (Owner is AIActor && !collidesWithEnemies && otherRigidbody.aiActor.IsNormalEnemy && !otherRigidbody.aiActor.HitByEnemyBullets)
				{
					PhysicsEngine.SkipCollision = true;
					return;
				}
			}
			if (!GameManager.PVP_ENABLED && Owner is PlayerController && otherRigidbody.GetComponent<PlayerController>() != null && !allowSelfShooting)
			{
				PhysicsEngine.SkipCollision = true;
				return;
			}
			if (GameManager.Instance.InTutorial)
			{
				PlayerController component = otherRigidbody.GetComponent<PlayerController>();
				if (component)
				{
					if (component.spriteAnimator.QueryInvulnerabilityFrame())
					{
						GameManager.BroadcastRoomTalkDoerFsmEvent("playerDodgedBullet");
					}
					else if (component.IsDodgeRolling)
					{
						GameManager.BroadcastRoomTalkDoerFsmEvent("playerAlmostDodgedBullet");
					}
					else
					{
						GameManager.BroadcastRoomTalkDoerFsmEvent("playerDidNotDodgeBullet");
					}
				}
			}
			if (collidesWithProjectiles && collidesOnlyWithPlayerProjectiles && otherRigidbody.projectile && !(otherRigidbody.projectile.Owner is PlayerController))
			{
				PhysicsEngine.SkipCollision = true;
				return;
			}
		}

		protected override HandleDamageResult HandleDamage(SpeculativeRigidbody rigidbody, PixelCollider hitPixelCollider, out bool killedTarget, PlayerController player, bool alreadyPlayerDelayed = false)
		{
			killedTarget = false;
			if (rigidbody.ReflectProjectiles)
			{
				return HandleDamageResult.NO_HEALTH;
			}
			if (!rigidbody.healthHaver)
			{
				return HandleDamageResult.NO_HEALTH;
			}
			if (!alreadyPlayerDelayed && s_delayPlayerDamage && player)
			{
				return HandleDamageResult.HEALTH;
			}
			bool flag = !rigidbody.healthHaver.IsDead;
			float num = ModifiedDamage;
			if (Owner is AIActor && rigidbody && rigidbody.aiActor && (Owner as AIActor).IsNormalEnemy)
			{
				num = ProjectileData.FixedFallbackDamageToEnemies;
				if (rigidbody.aiActor.HitByEnemyBullets)
				{
					num /= 4f;
				}
			}
			int healthHaverHitCount = (int)ProjectileHealthHaverHitCountInfo.GetValue(this);
			if (Owner is PlayerController && m_hasPierced && healthHaverHitCount >= 1)
			{
				int num2 = Mathf.Clamp(healthHaverHitCount - 1, 0, GameManager.Instance.PierceDamageScaling.Length - 1);
				num *= GameManager.Instance.PierceDamageScaling[num2];
			}
			if (OnWillKillEnemy != null && num >= rigidbody.healthHaver.GetCurrentHealth())
			{
				OnWillKillEnemy(this, rigidbody);
			}
			if (rigidbody.healthHaver.IsBoss)
			{
				num *= BossDamageMultiplier;
			}
			if (BlackPhantomDamageMultiplier != 1f && rigidbody.aiActor && rigidbody.aiActor.IsBlackPhantom)
			{
				num *= BlackPhantomDamageMultiplier;
			}
			bool flag2 = false;
			if (DelayedDamageToExploders)
			{
				flag2 = (rigidbody.GetComponent<ExplodeOnDeath>() && rigidbody.healthHaver.GetCurrentHealth() <= num);
			}
			if (!flag2)
			{
				HealthHaver healthHaver = rigidbody.healthHaver;
				float damage = num;
				Vector2 velocity = specRigidbody.Velocity;
				string ownerName = OwnerName;
				CoreDamageTypes coreDamageTypes = damageTypes;
				DamageCategory damageCategory = (!IsBlackBullet) ? DamageCategory.Normal : DamageCategory.BlackBullet;
				healthHaver.ApplyDamage(damage, velocity, ownerName, coreDamageTypes, damageCategory, true, hitPixelCollider, ignoreDamageCaps);
				if (player && player.OnHitByProjectile != null)
				{
					player.OnHitByProjectile(this, player);
				}
			}
			else
			{
				rigidbody.StartCoroutine((IEnumerator)ProjectileHandleDelayedDamageInfo.Invoke(this, new object[] { rigidbody, num, specRigidbody.Velocity, hitPixelCollider }));
			}
			if (Owner && Owner is AIActor && player)
			{
				(Owner as AIActor).HasDamagedPlayer = true;
			}
			killedTarget = (flag && rigidbody.healthHaver.IsDead);
			if (!killedTarget && rigidbody.gameActor != null)
			{
				if (AppliesPoison && UnityEngine.Random.value < PoisonApplyChance)
				{
					rigidbody.gameActor.ApplyEffect(healthEffect, 1f, null);
				}
				if (AppliesSpeedModifier && UnityEngine.Random.value < SpeedApplyChance)
				{
					rigidbody.gameActor.ApplyEffect(speedEffect, 1f, null);
				}
				if (AppliesCharm && UnityEngine.Random.value < CharmApplyChance)
				{
					rigidbody.gameActor.ApplyEffect(charmEffect, 1f, null);
				}
				if (AppliesFreeze && UnityEngine.Random.value < FreezeApplyChance)
				{
					rigidbody.gameActor.ApplyEffect(freezeEffect, 1f, null);
				}
				if (AppliesCheese && UnityEngine.Random.value < CheeseApplyChance)
				{
					rigidbody.gameActor.ApplyEffect(cheeseEffect, 1f, null);
				}
				if (AppliesBleed && UnityEngine.Random.value < BleedApplyChance)
				{
					rigidbody.gameActor.ApplyEffect(bleedEffect, -1f, this);
				}
				if (AppliesFire && UnityEngine.Random.value < FireApplyChance)
				{
					rigidbody.gameActor.ApplyEffect(fireEffect, 1f, null);
				}
				if (AppliesStun && UnityEngine.Random.value < StunApplyChance && rigidbody.gameActor.behaviorSpeculator)
				{
					rigidbody.gameActor.behaviorSpeculator.Stun(AppliedStunDuration, true);
				}
				for (int i = 0; i < statusEffectsToApply.Count; i++)
				{
					rigidbody.gameActor.ApplyEffect(statusEffectsToApply[i], 1f, null);
				}
			}
			ProjectileHealthHaverHitCountInfo.SetValue(this, healthHaverHitCount + 1);
			return (!killedTarget) ? HandleDamageResult.HEALTH : HandleDamageResult.HEALTH_AND_KILLED;
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
		public static BindingFlags AnyBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
		public static FieldInfo ProjectileHealthHaverHitCountInfo = typeof(Projectile).GetField("m_healthHaverHitCount", AnyBindingFlags);
		public static MethodInfo ProjectileHandleDelayedDamageInfo = typeof(Projectile).GetMethod("HandleDelayedDamage", AnyBindingFlags);
	}

}
