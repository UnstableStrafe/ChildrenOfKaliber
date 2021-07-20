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

namespace Items
{
    public class BulletDog : AIActor
    {
        public static GameObject prefab;
        public static readonly string guid = "bullet_dog_cel";
        private static tk2dSpriteCollectionData BulletDogCollection;
        public static void Init()
        {
            BulletDog.BuildPrefab();
        }
		public static void BuildPrefab()
        {
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
            if (!flag2)
            {
				prefab = EnemyBuilder.BuildPrefab("Bullet Dog", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false);
				var companion = prefab.AddComponent<EnemyBehavior>();
				companion.aiActor.knockbackDoer.weight = 20;
				companion.aiActor.MovementSpeed = 10f;
				companion.aiActor.healthHaver.PreventAllDamage = false;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = false;
				companion.aiActor.IgnoreForRoomClear = false;
				companion.aiActor.aiAnimator.HitReactChance = 0f;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = true;
				companion.aiActor.PreventFallingInPitsEver = false;
				companion.aiActor.healthHaver.ForceSetCurrentHealth(6f);
				companion.aiActor.CollisionKnockbackStrength = 5f;
				companion.aiActor.CanTargetPlayers = true;
				companion.aiActor.healthHaver.SetHealthMaximum(6f, null, false);
				companion.aiActor.specRigidbody.PixelColliders.Clear();
				companion.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
				{
					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyCollider,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 0,
					ManualOffsetY = 0,
					ManualWidth = 16,
					ManualHeight = 16,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0
				});
				companion.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
				{

					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyHitBox,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 0,
					ManualOffsetY = 0,
					ManualWidth = 16,
					ManualHeight = 16,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0,
				});
				companion.aiActor.PreventBlackPhantom = false;
				AIAnimator aiAnimator = companion.aiAnimator;
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "die",
						anim = new DirectionalAnimation
						{
							Type = DirectionalAnimation.DirectionType.Single,
							Flipped = new DirectionalAnimation.FlipType[1],
							AnimNames = new string[]
							{

								"die",
						   
							}

						}
					}
				};
				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					Flipped = new DirectionalAnimation.FlipType[2],
					AnimNames = new string[]
					{
						"idle_right",
						"idle_left"
					}
				};
				aiAnimator.MoveAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.EightWayOrdinal,
					Flipped = new DirectionalAnimation.FlipType[8],
					AnimNames = new string[]
					{
						"run_north",
						"run_north_east",
						"run_east",
						"run_south_east",
						"run_south",
						"run_south_west",
						"run_west",
						"run_north_west"
					}
				};
				bool flag3 = BulletDogCollection == null;
                if (flag3)
                {
					BulletDogCollection = SpriteBuilder.ConstructCollection(prefab, "Bullet_Dog_Collection");
					UnityEngine.Object.DontDestroyOnLoad(BulletDogCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], BulletDogCollection);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BulletDogCollection, new List<int>
					{
						0,
						1,
						2,
						3,
						4,
					}, "idle_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 8f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BulletDogCollection, new List<int>
					{
						5,
						6,
						7,
						8,
						9,
					}, "idle_right", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 8f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BulletDogCollection, new List<int>
					{
						10,
						11,
						12,
						13,
						14,
					}, "run_south_west", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BulletDogCollection, new List<int>
					{
						15,
						16,
						17,
						18,
						19
					}, "run_south_east", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BulletDogCollection, new List<int>
					{
						20,
						21,
						22,
						23,
						24,
						25,
					}, "run_north", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BulletDogCollection, new List<int>
					{
						26,
						27,
						28,
						29,
						30,
						31,
					}, "run_north_west", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BulletDogCollection, new List<int>
					{
						32,
						33,
						34,
						35,
						36,
						37
					}, "run_north_east", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BulletDogCollection, new List<int>
					{
						38,
						39,
						40,
						41,
						42,
						43
					}, "run_west", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BulletDogCollection, new List<int>
					{
						44,
						45,
						46,
						47,
						48
					}, "run_east", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BulletDogCollection, new List<int>
					{
						49,
						50,
						51,
						52,
						53
					}, "run_south", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BulletDogCollection, new List<int>
					{
						54,
						55,
						56,
						57,
						58,
						59
					}, "die", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;

				}
				var bs = prefab.GetComponent<BehaviorSpeculator>();
				BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").behaviorSpeculator;

				bs.OverrideBehaviors = behaviorSpeculator.OverrideBehaviors;
				bs.OtherBehaviors = behaviorSpeculator.OtherBehaviors;
				bs.TargetBehaviors = new List<TargetBehaviorBase>
				{
					new TargetPlayerBehavior
					{
						Radius = 35f,
						LineOfSight = true,
						ObjectPermanence = true,
						SearchInterval = 0.25f,
						PauseOnTargetSwitch = false,
						PauseTime = 0.25f
					}
				};
				bs.MovementBehaviors = new List<MovementBehaviorBase>
				{
					new SeekTargetBehavior
					{
						StopWhenInRange = false,
						CustomRange = 12f,
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
				Game.Enemies.Add("cel:bullet_dog", companion.aiActor);
			}
		}
		private static string[] spritePaths = new string[]
		{
			
			//idles
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_idle_left_001",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_idle_left_002",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_idle_left_003",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_idle_left_004",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_idle_left_005",
			//
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_idle_right_001",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_idle_right_002",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_idle_right_003",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_idle_right_004",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_idle_right_005",
			//
			//"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_idle_front_001",
			//"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_idle_front_002",
			//"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_idle_front_003",
			//"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_idle_front_004",
			//run
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_south_west_001",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_south_west_002",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_south_west_003",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_south_west_004",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_south_west_005",
			//
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_south_east_001",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_south_east_002",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_south_east_003",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_south_east_004",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_south_east_005",
			//
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_north_001",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_north_002",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_north_003",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_north_004",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_north_005",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_north_006",
			//
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_north_west_001",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_north_west_002",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_north_west_003",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_north_west_004",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_north_west_005",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_north_west_006",
			//
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_north_east_001",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_north_east_002",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_north_east_003",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_north_east_004",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_north_east_005",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_north_east_006",
			//
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_west_001",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_west_002",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_west_003",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_west_004",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_west_005",
			//
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_east_001",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_east_002",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_east_003",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_east_004",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_east_005",
			//
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_south_001",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_south_002",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_south_003",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_south_004",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_south_005",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_run_south_006",
			//death
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_die_001",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_die_002",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_die_003",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_die_004",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_die_005",
			"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_die_006",
			//fall
			//"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_fall_001",
			//"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_fall_002",
			//"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_fall_003",
			//"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_fall_004",
			//"Items/Enemies/Sprites/Bullet_Dog/bullet_dog_fall_005",
			//8fps if u do it
		};
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
	}

}
