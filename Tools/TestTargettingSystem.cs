using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;
using Dungeonator;

namespace Items
{
    class TestTargettingSystem : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Test Targetting System";

            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<TestTargettingSystem>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Testing";
            string longDesc = "Targets the enemy in range with the most nearby enemies.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ck");

            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 3);
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }
        public override void DoEffect(PlayerController user)
        {
            base.DoEffect(user);
            AIActor Target = TargetEnemies(user, 8, 2);
            if(Target != null)
            {
                Exploder.DoDefaultExplosion(Target.specRigidbody.UnitCenter, Vector2.zero);
            }
        }
        private List<AIActor> PossibleLargestGrouping = new List<AIActor> { };
        
        private AIActor TargetEnemies(PlayerController user, float initialRange, float secondaryRange)
        {
            if (user.CurrentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear))
            {
                if (PossibleLargestGrouping.Any())
                {
                    PossibleLargestGrouping.Clear();
                }
                RoomHandler room = user.CurrentRoom;
                List<AIActor> PossibleTargets = new List<AIActor> { };
                AIActor TrueTarget = null;
                Action<AIActor, float> InitialTargetting = delegate (AIActor actor, float dist)
                {
                    PossibleTargets.Add(actor);
                };
                room.ApplyActionToNearbyEnemies(user.specRigidbody.UnitCenter, initialRange, InitialTargetting);
                foreach (AIActor a in PossibleTargets)
                {
                    List<AIActor> nearbyEnemies = new List<AIActor> { };
                    Action<AIActor, float> FindGroups = delegate (AIActor actor, float dist)
                    {
                        nearbyEnemies.Add(actor);
                    };
                    room.ApplyActionToNearbyEnemies(a.specRigidbody.UnitCenter, secondaryRange, FindGroups);
                    if (PossibleLargestGrouping.Count() < nearbyEnemies.Count())
                    {
                        PossibleLargestGrouping = nearbyEnemies;
                        TrueTarget = a;
                    }
                }
                return TrueTarget;
            }
            else
            {
                return null;
            }
        }
    }
}
