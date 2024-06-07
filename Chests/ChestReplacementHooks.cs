using System;
using System.Collections.Generic;
using System.Reflection;
using MonoMod.RuntimeDetour;
using UnityEngine;
using Dungeonator;

namespace Items
{
    public static class ChestReplacementHooks
    {

        public static void Init()
        {
            try
            {
                Hook configureOnPlacementHook = new Hook(typeof(FloorChestPlacer).GetMethod("ConfigureOnPlacement", BindingFlags.Public | BindingFlags.Instance), typeof(ChestReplacementHooks).GetMethod("ConfigureOnPlacementHook"));
                //Hook generationSpawnRewardChestAtHook = new Hook(typeof(RewardManager).GetMethod("GenerationSpawnRewardChestAt", BindingFlags.Public | BindingFlags.Instance), typeof(ChestReplacementHooks).GetMethod("GenerationSpawnRewardChestAtHook"));
            }
            catch (Exception ex)
            {
                ETGModConsole.Log(ex.ToString());
            }

        }

        public static void ConfigureOnPlacementHook(Action<FloorChestPlacer, RoomHandler> orig, FloorChestPlacer self, RoomHandler room)
        {

            if (self.OverrideChestPrefab != MunitionsChestController.munitionsChest && self.ItemQuality != PickupObject.ItemQuality.S) //this migggght break shit im not sure but ill find out ig.
            {
                float f = UnityEngine.Random.value;
                if (f <= munitionsChestOverrideChance)
                {
                    Chest chest = MunitionsChestController.munitionsChest;
                    self.OverrideChestPrefab = chest;
                    self.UseOverrideChest = true;
                    if (chest.GetComponent<SpeculativeRigidbody>() is SpeculativeRigidbody body && body.m_initialized)
                    {
                        body.Reinitialize();
                    }
                    DungeonPrerequisite dungeonPrerequisite = new DungeonPrerequisite()
                    {
                        saveFlagToCheck = GungeonFlags.TUTORIAL_COMPLETED,
                        requireFlag = true,
                        prerequisiteType = DungeonPrerequisite.PrerequisiteType.FLAG,
                    };


                    //fix nulling shit with chest.ConfigureOnPlacement / setting up minimap and collision shiz

                }
            }
            orig(self, room);
        }
        /*
        public static Chest GenerationSpawnRewardChestAtHook(Func<RewardManager, IntVector2, RoomHandler, PickupObject.ItemQuality?, float> orig, RewardManager self, IntVector2 positionInRoom, RoomHandler targetRoom, PickupObject.ItemQuality? targetQuality = null, float overrideMimicChance = -1f)
        {
            System.Random random = (!GameManager.Instance.IsSeeded) ? null : BraveRandom.GeneratorRandom;
            FloorRewardData rewardDataForFloor = self.GetRewardDataForFloor(GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId);

            //Methods/Classes with shit i need/can use
            //GenerationSpawnRewardChestAt
            //ConfigureOnPlacement
            //Chest

        }
        */
        public static readonly float munitionsChestOverrideChance = .05f;
    }
}
