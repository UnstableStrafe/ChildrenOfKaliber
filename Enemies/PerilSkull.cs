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
    class PerilSkull : AIActor
    {
        public static GameObject prefab;
        public static readonly string guid = "peril_skull_mod";
        
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
                prefab = EnemyBuilder.BuildPrefab("Peril Skull", guid, "Items/Enemies/Sprites/Peril_Skull/Idle/peril_skull_idle_001", new IntVector2(0, 0), new IntVector2(0, 0), false);
                var enemy = prefab.AddComponent<EnemyBehavior>();

                enemy.aiActor.knockbackDoer.weight = 35;
                enemy.aiActor.MovementSpeed = 5.5f;
                enemy.aiActor.healthHaver.PreventAllDamage = true;
                enemy.aiActor.CollisionDamage = 1f;
                enemy.aiActor.HasShadow = false;
                enemy.aiActor.IgnoreForRoomClear = true;
                enemy.aiActor.aiAnimator.HitReactChance = 0f;
                enemy.aiActor.specRigidbody.CollideWithOthers = true;
                enemy.aiActor.specRigidbody.CollideWithTileMap = false;
                enemy.aiActor.PreventFallingInPitsEver = true;
                enemy.aiActor.CollisionKnockbackStrength = 5f;
                enemy.aiActor.CanTargetPlayers = true;
                enemy.aiActor.healthHaver.SetHealthMaximum(999f, null, true);
                enemy.aiActor.SetIsFlying(true, "ghost", true, true);

                prefab.AddAnimation("idle", "Items/Enemies/Sprites/Peril_Skull/Idle", fps: 8, AnimationType.Idle, DirectionType.Single);
                prefab.AddAnimation("run", "Items/Enemies/Sprites/Peril_Skull/Run", fps: 8, AnimationType.Idle, DirectionType.Single);
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
                    ManualWidth = 15,
                    ManualHeight = 22,
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
                    ManualWidth = 15,
                    ManualHeight = 22,
                    ManualDiameter = 0,
                    ManualLeftX = 0,
                    ManualLeftY = 0,
                    ManualRightX = 0,
                    ManualRightY = 0,
                });
                enemy.aiActor.PreventBlackPhantom = true;
                var bs = prefab.GetComponent<BehaviorSpeculator>();
                BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").behaviorSpeculator;

                bs.OverrideBehaviors = behaviorSpeculator.OverrideBehaviors;
                bs.OtherBehaviors = behaviorSpeculator.OtherBehaviors;
                bs.TargetBehaviors = new List<TargetBehaviorBase>
                {
                    new TargetPlayerBehavior
                    {
                        Radius = 999f,
                        LineOfSight = false,
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
                        CustomRange = 999f,
                        LineOfSight = false,
                        ReturnToSpawn = false,
                        SpawnTetherDistance = 0f,
                        PathInterval = 0.25f,
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
                Game.Enemies.Add("cel:peril_skull", enemy.aiActor);
            }
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
}
