using System;
using System.Collections.Generic;
using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;
using AnimationType = Alexandria.EnemyAPI.EnemyBuilder.AnimationType;
using System.Collections;
using Dungeonator;
using System.Linq;
using Brave.BulletScript;
using Alexandria.DungeonAPI;
using Alexandria;
using DirectionType = DirectionalAnimation.DirectionType;
using static DirectionalAnimation;
using Pathfinding;
using SaveAPI;
using Alexandria.EnemyAPI;

namespace Items
{
    class Terroranchula : AIActor
    {
        public static GameObject prefab;
        public static readonly string guid = "Terroranchula";

        public static GameObject center;
        private static tk2dSpriteCollectionData terroranchulaSpriteCollection;
        private static Texture2D BossCardTexture = ResourceExtractor.GetTextureFromResource("Items/Resources/Enemies/Bosscards/terroranchula_bosscard.png");
        private static Texture2D FakeoutBossCardTexture = ResourceExtractor.GetTextureFromResource("Items/Resources/Enemies/Bosscards/terroranchula_fakeout_bosscard_big.png");

		public static void Init()
        {
			BuildPrefab();
        }
        public static void BuildPrefab()
        {
            bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);

            bool flag2 = flag;

            if (!flag2)
            {
                prefab = BossBuilder.BuildPrefab("Terroranchula", guid, "Items/Resources/Enemies/Terroranchula/terroranchula_idle_001.png", new IntVector2(0, 0), new IntVector2(0, 0), false, false);
				ETGModConsole.Log("Test1");
				var enemy = prefab.AddComponent<EnemyBehavior>();
				AIAnimator aiAnimator = enemy.aiAnimator;

				enemy.aiActor.knockbackDoer.weight = 35;
				enemy.aiActor.MovementSpeed = 5f;
				enemy.aiActor.healthHaver.PreventAllDamage = false;
				enemy.aiActor.CollisionDamage = 1f;
				enemy.aiActor.HasShadow = false;
				enemy.aiActor.IgnoreForRoomClear = false;
				enemy.aiActor.aiAnimator.HitReactChance = 0f;
				enemy.aiActor.specRigidbody.CollideWithOthers = true;
				enemy.aiActor.specRigidbody.CollideWithTileMap = true;
				enemy.aiActor.PreventFallingInPitsEver = false;
				enemy.aiActor.healthHaver.ForceSetCurrentHealth(950f);
				enemy.aiActor.CollisionKnockbackStrength = 10f;
				enemy.aiActor.CanTargetPlayers = true;
				enemy.aiActor.healthHaver.SetHealthMaximum(950f, null, false);

				

				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "idle",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};


				bool flag3 = terroranchulaSpriteCollection == null;
                if (flag3)
                {
					terroranchulaSpriteCollection = SpriteBuilder.ConstructCollection(prefab, "TerroranchulaCollection");
					UnityEngine.Object.DontDestroyOnLoad(terroranchulaSpriteCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], terroranchulaSpriteCollection);
					}
					SpriteBuilder.AddAnimation(enemy.spriteAnimator, terroranchulaSpriteCollection, new List<int>
					{
						0,
						1,
						2,
						3,
					}, "idle", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 8f;
				}

				enemy.aiActor.specRigidbody.PixelColliders.Clear();
				enemy.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
				{
					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyCollider,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 5,
					ManualOffsetY = 0,
					ManualWidth = 20,
					ManualHeight = 20,
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
					ManualOffsetX = 5,
					ManualOffsetY = 0,
					ManualWidth = 20,
					ManualHeight = 20,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0,
				});
				ETGModConsole.Log("Test3");
				enemy.aiActor.PreventBlackPhantom = false;

				var bs = prefab.GetComponent<BehaviorSpeculator>();
				BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").behaviorSpeculator;
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
				bs.MovementBehaviors = new List<MovementBehaviorBase>() 
				{
					new SeekTargetBehavior() 
					{
						StopWhenInRange = false,
						CustomRange = 45f,
						LineOfSight = false,
						ReturnToSpawn = false,
						SpawnTetherDistance = 0f,
						PathInterval = 0.5f,
						SpecifyRange = false,
						MinActiveRange = 0f,
						MaxActiveRange = 0f
					}
				};
				Game.Enemies.Add("ck:terroranchula", enemy.aiActor);

				StaticReferenceManager.AllHealthHavers.Remove(enemy.aiActor.healthHaver);
				ETGModConsole.Log("Test4");
			}
		}
		private static string[] spritePaths = new string[] 
		{
			"Items/Resources/Enemies/Terroranchula/terroranchula_idle_001.png",
			"Items/Resources/Enemies/Terroranchula/terroranchula_idle_002.png",
			"Items/Resources/Enemies/Terroranchula/terroranchula_idle_003.png",
			"Items/Resources/Enemies/Terroranchula/terroranchula_idle_004.png",
		};


		public class EnemyBehavior : BraveBehaviour
		{
			private RoomHandler m_StartRoom;
			public void Update()
			{
				m_StartRoom = aiActor.GetAbsoluteParentRoom();
				if (!base.aiActor.HasBeenEngaged)
				{
					CheckPlayerRoom();
				}
			}
			private void CheckPlayerRoom()
			{
				if (GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() != null && GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() == m_StartRoom && GameManager.Instance.PrimaryPlayer.IsInCombat == true)
				{

					GameManager.Instance.StartCoroutine(LateEngage());
				}
				else
				{
					base.aiActor.HasBeenEngaged = false;
				}
			}
			private IEnumerator LateEngage()
			{
				yield return new WaitForSeconds(0.5f);
				if (GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() != null && GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() == m_StartRoom)
				{
					base.aiActor.HasBeenEngaged = true;
				}
				yield break;
			}

			public void Start()
			{
				this.aiActor.knockbackDoer.SetImmobile(true, "nope.");
				base.aiActor.HasBeenEngaged = false;
				//Important for not breaking basegame stuff!
				StaticReferenceManager.AllHealthHavers.Remove(base.aiActor.healthHaver);


				
			}

		}
	}
}
