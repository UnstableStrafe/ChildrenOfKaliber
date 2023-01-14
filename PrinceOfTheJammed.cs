using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using UnityEngine;
//using Alexandria.ItemAPI;
using Gungeon;
using Alexandria.DungeonAPI;
using System.Collections;
using Alexandria.ItemAPI;

namespace Items
{
    class PrinceOfTheJammed : MonoBehaviour
    {
		
		public static void Init()
        {
			GameObject gameObject = SpriteBuilder.SpriteFromResource("Items/Enemies/Sprites/potj");
			gameObject.AddAnimation("start", "Items/Enemies/Sprites/POTJ/Start", 8, Library.AnimationType.Idle, DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType.None, tk2dSpriteAnimationClip.WrapMode.Once);
			gameObject.AddAnimation("idle", "Items/Enemies/Sprites/POTJ/Idle", 10, Library.AnimationType.Idle, DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType.None, tk2dSpriteAnimationClip.WrapMode.Loop);
			gameObject.AddComponent<PrinceOfTheJammed>();
			FakePrefab.MarkAsFakePrefab(gameObject);
			gameObject.SetActive(false);
		    

			SpeculativeRigidbody specBody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(+3, +8), new IntVector2(16, 16));
			specBody.PixelColliders.Clear();
			
			specBody.PixelColliders.Add(new PixelCollider
			{
				ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
				CollisionLayer = CollisionLayer.EnemyCollider,
				IsTrigger = false,
				BagleUseFirstFrameOnly = false,
				SpecifyBagelFrame = string.Empty,
				BagelColliderNumber = 0,
				ManualOffsetX = 3,
				ManualOffsetY = 8,
				ManualWidth = 16,
				ManualHeight = 16,
				ManualDiameter = 0,
				ManualLeftX = 0,
				ManualLeftY = 0,
				ManualRightX = 0,
				ManualRightY = 0,
				
			});

			specBody.PixelColliders.Add(new PixelCollider
			{

				ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
				CollisionLayer = CollisionLayer.EnemyHitBox,
				IsTrigger = false,
				BagleUseFirstFrameOnly = false,
				SpecifyBagelFrame = string.Empty,
				BagelColliderNumber = 0,
				ManualOffsetX = 3,
				ManualOffsetY = 8, 
				ManualWidth = 16,
				ManualHeight = 16,
				ManualDiameter = 0,
				ManualLeftX = 0,
				ManualLeftY = 0,
				ManualRightX = 0,
				ManualRightY = 0,
			});

			specBody.CollideWithTileMap = false;
			specBody.CollideWithOthers = true;
			
		
			PJPrefab = gameObject;
		}
		
		
		private void Start()
        {
			Rigidbody = base.gameObject.GetComponent<SpeculativeRigidbody>();
			Rigidbody.OnPreRigidbodyCollision += DoCollision;
			base.gameObject.GetComponent<tk2dSpriteAnimator>().Play("start");
			base.gameObject.GetComponent<tk2dSpriteAnimator>().QueueAnimation("idle");
			this.m_currentTargetPlayer = GameManager.Instance.GetRandomActivePlayer();

		}

		private void Update()
        {
			DoParticles();
			DoMotion();
        }
		private void DoMotion()
        {
			Rigidbody.Velocity = Vector2.zero;
			if (base.gameObject.GetComponent<tk2dSpriteAnimator>().IsPlaying("start"))
			{
				return;
			}
			if (this.m_currentTargetPlayer.healthHaver.IsDead || this.m_currentTargetPlayer.IsGhost)
			{
				this.m_currentTargetPlayer = GameManager.Instance.GetRandomActivePlayer();
			}
			Vector2 centerPosition = this.m_currentTargetPlayer.CenterPosition;
			Vector2 vector = centerPosition - Rigidbody.UnitCenter;
			float magnitude = vector.magnitude;
			float d = Mathf.Lerp(4, 10, (magnitude - 10) / (50 - 10));
			Rigidbody.Velocity = vector.normalized * d;
			
		}
		private void DoCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
		{
			PhysicsEngine.SkipCollision = true;
			if (otherRigidbody.gameObject.GetComponent<PlayerController>())
			{
				otherRigidbody.gameObject.GetComponent<PlayerController>().healthHaver.ApplyDamage(0.5f, Vector2.zero, "Prince Of The Jammed", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
			}


		}
		private void DoParticles()
        {
			if (GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.LOW && GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.VERY_LOW)
            {
				Vector3 vector = Rigidbody.UnitBottomLeft.ToVector3ZisY(0f);
				Vector3 vector2 = Rigidbody.UnitTopRight.ToVector3ZisY(0f);
				float num = (vector2.y - vector.y) * (vector2.x - vector.x);
				float num2 = 40 * num;
				int num3 = Mathf.CeilToInt(Mathf.Max(1f, num2 * BraveTime.DeltaTime));
				int num4 = num3;
				Vector3 minPosition = vector;
				Vector3 maxPosition = vector2;
				Vector3 up = Vector3.up;
				float angleVariance = 120f;
				float magnitudeVariance = 0.5f;
				float? startLifetime = new float?(UnityEngine.Random.Range(1f, 1.65f));
				GlobalSparksDoer.DoRandomParticleBurst(num4, minPosition, maxPosition, up, angleVariance, magnitudeVariance, null, startLifetime, null, GlobalSparksDoer.SparksType.BLACK_PHANTOM_SMOKE);
			}

		}
		
		private static PrinceOfTheJammed m_instance;
		private static SpeculativeRigidbody PJBody;
		public static GameObject PJPrefab = new GameObject();
		private SpeculativeRigidbody Rigidbody;
		private PlayerController m_currentTargetPlayer;
	}
	
}