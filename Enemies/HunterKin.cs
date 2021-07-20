using System;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;
using AnimationType = ItemAPI.EnemyBuilder.AnimationType;
using System.Collections;
using Dungeonator;
using System.Linq;
using Brave.BulletScript;
using GungeonAPI;
using SpriteBuilder = ItemAPI.SpriteBuilder;
using DirectionType = DirectionalAnimation.DirectionType;
using static DirectionalAnimation;

namespace Items
{
	class HunterKin : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "hunter_kin_cel";
		public static GameObject shootpoint;
		public static void Init()
		{
			HunterKin.BuildPrefab();
		}
		public static void BuildPrefab()
		{
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			
			bool flag2 = flag;
			if (!flag2)
			{
				prefab = EnemyBuilder.BuildPrefab("Hunter Kin", guid, "Items/Enemies/Sprites/Hunter_Kin/Idle_Front_Right/hunter_kin_idle_front_right_001", new IntVector2(0, 0), new IntVector2(0, 0), true, true);
				var enemy = prefab.AddComponent<EnemyBehavior>();
				
				enemy.aiActor.knockbackDoer.weight = 35;
				enemy.aiActor.MovementSpeed = 4f;
				enemy.aiActor.healthHaver.PreventAllDamage = false;
				enemy.aiActor.CollisionDamage = 1f;
				enemy.aiActor.HasShadow = false;
				enemy.aiActor.IgnoreForRoomClear = false;
				enemy.aiActor.aiAnimator.HitReactChance = 0f;
				enemy.aiActor.specRigidbody.CollideWithOthers = true;
				enemy.aiActor.specRigidbody.CollideWithTileMap = true;
				enemy.aiActor.PreventFallingInPitsEver = false;
				enemy.aiActor.healthHaver.ForceSetCurrentHealth(25f);
				enemy.aiActor.CollisionKnockbackStrength = 5f;
				enemy.aiActor.CanTargetPlayers = true;
				enemy.aiActor.healthHaver.SetHealthMaximum(25f, null, false);
				

				prefab.AddAnimation("idle_back_right", "Items/Enemies/Sprites/Hunter_Kin/Idle_Back_Right", fps: 5, AnimationType.Idle, DirectionType.FourWay);
				prefab.AddAnimation("idle_front_right", "Items/Enemies/Sprites/Hunter_Kin/Idle_Front_Right", fps: 5, AnimationType.Idle, DirectionType.FourWay);
				prefab.AddAnimation("idle_front_left", "Items/Enemies/Sprites/Hunter_Kin/Idle_Front_Left", fps: 5, AnimationType.Idle, DirectionType.FourWay);
				prefab.AddAnimation("idle_back_left", "Items/Enemies/Sprites/Hunter_Kin/Idle_Back_Left", fps: 5, AnimationType.Idle, DirectionType.FourWay);
				//
				prefab.AddAnimation("run_back_right", "Items/Enemies/Sprites/Hunter_Kin/Run_Back_Right", fps: 5, AnimationType.Move, DirectionType.FourWay);
				prefab.AddAnimation("run_front_right", "Items/Enemies/Sprites/Hunter_Kin/Run_Front_Right", fps: 5, AnimationType.Move, DirectionType.FourWay);
				prefab.AddAnimation("run_front_left", "Items/Enemies/Sprites/Hunter_Kin/Run_Front_Left", fps: 5, AnimationType.Move, DirectionType.FourWay);
				prefab.AddAnimation("run_back_left", "Items/Enemies/Sprites/Hunter_Kin/Run_Back_Left", fps: 5, AnimationType.Move, DirectionType.FourWay);
				//
				prefab.AddAnimation("die_north", "Items/Enemies/Sprites/Hunter_Kin/Die_North", fps: 5, AnimationType.Move, DirectionType.EightWayOrdinal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				prefab.AddAnimation("die_north_east", "Items/Enemies/Sprites/Hunter_Kin/Die_North_East", fps: 5, AnimationType.Move, DirectionType.EightWayOrdinal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				prefab.AddAnimation("die_east", "Items/Enemies/Sprites/Hunter_Kin/Die_East", fps: 5, AnimationType.Move, DirectionType.EightWayOrdinal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				prefab.AddAnimation("die_south_east", "Items/Enemies/Sprites/Hunter_Kin/Die_South_East", fps: 5, AnimationType.Move, DirectionType.EightWayOrdinal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				prefab.AddAnimation("die_south", "Items/Enemies/Sprites/Hunter_Kin/Die_South", fps: 5, AnimationType.Move, DirectionType.EightWayOrdinal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				prefab.AddAnimation("die_south_west", "Items/Enemies/Sprites/Hunter_Kin/Die_South_West", fps: 5, AnimationType.Move, DirectionType.EightWayOrdinal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				prefab.AddAnimation("die_west", "Items/Enemies/Sprites/Hunter_Kin/Die_West", fps: 5, AnimationType.Move, DirectionType.EightWayOrdinal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				prefab.AddAnimation("die_north_west", "Items/Enemies/Sprites/Hunter_Kin/Die_North_West", fps: 5, AnimationType.Move, DirectionType.EightWayOrdinal, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				//
				prefab.AddAnimation("summon", "Items/Enemies/Sprites/Hunter_Kin/Summon", fps: 7, AnimationType.Move, DirectionType.Single, tk2dSpriteAnimationClip.WrapMode.Once, assignAnimation: false);
				
				DirectionalAnimation die = new DirectionalAnimation()
				{
					AnimNames = new string[] { "die_north", "die_north_east", "die_east", "die_south_east", "die_south", "die_south_west", "die_west", "die_north_west"},
					Flipped = new FlipType[] { FlipType.None, FlipType.None, FlipType.None, FlipType.None, FlipType.None, FlipType.None, FlipType.None, FlipType.None, },
					Type = DirectionType.EightWayOrdinal,
					Prefix = string.Empty
				};
				DirectionalAnimation summon = new DirectionalAnimation()
				{
					AnimNames = new string[] { "summon" },
					Flipped = new FlipType[] { FlipType.None},
					Type = DirectionType.Single,
					Prefix = string.Empty
				};
				
				enemy.aiAnimator.AssignDirectionalAnimation("die", die, AnimationType.Other);
				enemy.aiAnimator.AssignDirectionalAnimation("summon", summon, AnimationType.Other);
				enemy.aiActor.specRigidbody.PixelColliders.Clear();
				enemy.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
				{
					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyCollider,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 0,
					ManualOffsetY = 0,
					ManualWidth = 14,
					ManualHeight = 24,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0
				});
				
				enemy.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
				{

					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyHitBox,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 0,
					ManualOffsetY = 0,
					ManualWidth = 14,
					ManualHeight = 24,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0,
				});
				
				enemy.aiActor.PreventBlackPhantom = false;
				
				AIAnimator aiAnimator = enemy.aiAnimator;
				var yah = enemy.transform.Find("GunAttachPoint").gameObject;
				yah.transform.position = enemy.aiActor.transform.position;
				yah.transform.localPosition = new Vector2(0f, .3f);
				AIActor SourceEnemy = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5");
				EnemyBuilder.DuplicateAIShooterAndAIBulletBank(prefab, SourceEnemy.aiShooter, SourceEnemy.GetComponent<AIBulletBank>(), 12, yah.transform);
				var bs = prefab.GetComponent<BehaviorSpeculator>();
				BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").behaviorSpeculator;
				
				bs.OverrideBehaviors = behaviorSpeculator.OverrideBehaviors;
				bs.OtherBehaviors = behaviorSpeculator.OtherBehaviors;
				bs.TargetBehaviors = new List<TargetBehaviorBase>
				{
					new TargetPlayerBehavior
					{
						Radius = 45f,
						LineOfSight = true,
						ObjectPermanence = true,
						SearchInterval = 0.25f,
						PauseOnTargetSwitch = false,
						PauseTime = 0.25f
					},
				
				};
				
				AIActor Firecracker = EnemyDatabase.GetOrLoadByGuid("5f15093e6f684f4fb09d3e7e697216b4");
				bs.AttackBehaviors = new List<AttackBehaviorBase>()
				{
                    new AttackBehaviorGroup()
                    {

                    }

                };

				bs.AttackBehaviorGroup.AttackBehaviors = new List<AttackBehaviorGroup.AttackGroupItem>()
				{

					new AttackBehaviorGroup.AttackGroupItem()
								{
						Probability = 1.75f,
						Behavior = new ShootGunBehavior()
						{
							WeaponType = WeaponType.BulletScript,
							BulletScript = new CustomBulletScriptSelector(typeof(HunterKinScript)),
							LeadAmount = 0,
							LeadChance = 1,
							AttackCooldown = 1f,
							RequiresLineOfSight = true,
							FixTargetDuringAttack = true,
							StopDuringAttack = true,
							RespectReload = true,
							MagazineCapacity = 5,
							ReloadSpeed = 1f,
							EmptiesClip = true,
							SuppressReloadAnim = false,
							CooldownVariance = 0,
							GlobalCooldown = 0,
							InitialCooldown = 0,
							InitialCooldownVariance = 0,
							GroupName = null,
							GroupCooldown = 0,
							MinRange = 0,
							Range = 7,
							MinWallDistance = 0,
							MaxEnemiesInRoom = -1,
							MinHealthThreshold = 0,
							MaxHealthThreshold = 1,
							HealthThresholds = new float[0],
							AccumulateHealthThresholds = true,
							targetAreaStyle = null,
							IsBlackPhantom = false,
							resetCooldownOnDamage = null,
							MaxUsages = 0,
							UseLaserSight = true,
							PreFireLaserTime = .5f,


						},
						NickName = "Hunter Kin Shoot Arrow"
					},
					new AttackBehaviorGroup.AttackGroupItem()
								{
						Probability = 1.5f,
						Behavior = new SummonEnemyBehavior()
						{
							DefineSpawnRadius = true,
							MinSpawnRadius = 3,
							MaxSpawnRadius = 3,
							MaxRoomOccupancy = 6,
							MaxSummonedAtOnce = 2,
							MaxToSpawn = -1,
							NumToSpawn = 2,
							KillSpawnedOnDeath = false,
							CrazeAfterMaxSpawned = false,
							EnemeyGuids = new List<string>(){ BulletDog.guid },
							SummonTime = .5f,
							DisableDrops = true,
							StopDuringAnimation = true,
							SummonAnim = "Summon",
							HideGun = true,
							Cooldown = 1f,
							RequiresLineOfSight = false,
							selectionType = SummonEnemyBehavior.SelectionType.Random,
							
						},
                        NickName = "Summon dat doggy"
                    }

                };
				bs.MovementBehaviors = new List<MovementBehaviorBase>
				{
				
					new SeekTargetBehavior
					{
						StopWhenInRange = true,
						CustomRange = 7f,
						LineOfSight = false,
						ReturnToSpawn = false,
						SpawnTetherDistance = 0f,
						PathInterval = 0.5f,
						SpecifyRange = false,
						MinActiveRange = 0f,
						MaxActiveRange = 0f
					}
				};
				bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
				bs.TickInterval = behaviorSpeculator.TickInterval;
				bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
				bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
				bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
				bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
				bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
				Game.Enemies.Add("cel:hunter_kin", enemy.aiActor);
			}
			
		}
		
		public class EnemyBehavior : BraveBehaviour
		{
			//determines that the enemy only attacks when a player is in the room
			private RoomHandler m_StartRoom;
			private void Update()
			{
				if (!base.aiActor.HasBeenEngaged) { CheckPlayerRoom(); }
			}
			private void CheckPlayerRoom()
			{
				if (GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() != null && GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() == m_StartRoom) { base.aiActor.HasBeenEngaged = true; }
			}
			private void Start()
			{
				m_StartRoom = aiActor.GetAbsoluteParentRoom();
				base.aiActor.healthHaver.OnPreDeath += (obj) => { AkSoundEngine.PostEvent("Play_ENM_Death", base.aiActor.gameObject); };
			}
		}
        public class HunterKinScript : Script
        {
            protected override IEnumerator Top()
            {
                if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody) { base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("05891b158cd542b1a5f3df30fb67a7ff").bulletBank.GetBullet("default")); }
                for (int i = 0; i < 1; i++)
                {
                    
                    base.Fire(new Direction(0, Brave.BulletScript.DirectionType.Aim, -1f), new Speed(50f, SpeedType.Absolute), new HunterKinBullet());
                    yield return this.Wait(6);
                }
                yield break;
            }
        }
		public class HunterKinBullet : Bullet
		{
			public HunterKinBullet() : base("default", false, false, false)
			{

			}
		}
	}
}

