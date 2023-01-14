using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gungeon;
using System.Collections;
using Dungeonator;
using Alexandria.DungeonAPI;
using Alexandria.ItemAPI;


namespace Items
{
    class PoisonGeistEnemy : MonoBehaviour
    {
        public PoisonGeistEnemy()
        {
            targetPlayer = null;
        }
        public static void Init()
        {
            GameObject gameObject = SpriteBuilder.SpriteFromResource("Items/Enemies/Sprites/poison_geist");
            gameObject.AddAnimation("idle", "Items/Enemies/Sprites/PoisonGeist", 10, Library.AnimationType.Idle, DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType.None, tk2dSpriteAnimationClip.WrapMode.Loop);
            gameObject.AddComponent<PoisonGeistEnemy>();
            FakePrefab.MarkAsFakePrefab(gameObject);
            gameObject.SetActive(false);
            SpeculativeRigidbody specBody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(+1, +3), new IntVector2(6, 8));
            specBody.PixelColliders.Clear();
            specBody.PixelColliders.Add(new PixelCollider
            {
                ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                CollisionLayer = CollisionLayer.EnemyCollider,
                IsTrigger = false,
                BagleUseFirstFrameOnly = false,
                SpecifyBagelFrame = string.Empty,
                BagelColliderNumber = 0,
                ManualOffsetX = 1,
                ManualOffsetY = 3,
                ManualWidth = 6,
                ManualHeight = 8,
                ManualDiameter = 0,
                ManualLeftX = 0,
                ManualLeftY = 0,
                ManualRightX = 0,
                ManualRightY = 0,
            });

            specBody.CollideWithTileMap = false;
            specBody.CollideWithOthers = false;
            poisonGeistPrefab = gameObject;
        }
        public void Start()
        {
            base.gameObject.GetComponent<tk2dSpriteAnimator>().Play("idle");
            if (targetPlayer == null)
            {
                targetPlayer = GameManager.Instance.GetRandomActivePlayer();
            }
            Rigidbody = base.gameObject.GetComponent<SpeculativeRigidbody>();

        }
        private void Update()
        {
            DoMotion();
            GoopMeDaddy();
        }
        private void DoMotion()
        {
            Rigidbody.Velocity = Vector2.zero;
            Vector2 centerPosition = this.targetPlayer.CenterPosition;
            Vector2 vector = centerPosition - Rigidbody.UnitCenter;
            float magnitude = vector.magnitude;
            float targetRisk = targetPlayer.gameObject.GetOrAddComponent<RiskStat>().RiskAMT;
            float riskMult = Mathf.Clamp((targetRisk * .22f) + 1, 1, 2.2f);
            float d = Mathf.Lerp(4 * riskMult, 10 * riskMult, (magnitude - (10 * riskMult)) / ((50 * riskMult) - (10 * riskMult)));
            Rigidbody.Velocity = vector.normalized * d;
        }
        private void GoopMeDaddy()
        {
            if (!BossKillCam.BossDeathCamRunning && !TimeTubeCreditsController.IsTimeTubing && !GameManager.Instance.PreventPausing && !GameManager.IsBossIntro)
            {
                DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(Library.PoisonDef).TimedAddGoopCircle(base.GetComponent<tk2dBaseSprite>().WorldCenter, .75f, .03f);
            }
        }

        private SpeculativeRigidbody Rigidbody;
        private PlayerController targetPlayer;
        public static GameObject poisonGeistPrefab = new GameObject();
    }
}
