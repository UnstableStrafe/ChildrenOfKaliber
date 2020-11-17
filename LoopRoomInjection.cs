using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GungeonAPI;
using UnityEngine;
using Gungeon;
using Dungeonator;

namespace Items
{
    class LoopRoomInjection : FlowDatabase
    {
        public static PrototypeDungeonRoom LoopItemRoomPrefab;
        public static SharedInjectionData ForgeData;
        public static SharedInjectionData BaseSharedInjectionData;
        public static ProceduralFlowModifierData LoopItemRoom;
        public static GameObject TimeItemStand;
        public static AssetBundle sharedAssets2;

        
        public static void ClockClase()
        {
            TimeItemStand = new GameObject("Loop Item Case") { layer = 0 };
            TimeItemStand.SetActive(false);
            ExpandTheGungeon.ItemAPI.ItemBuilder.AddSpriteToObject(TimeItemStand, "Items/Resources/TimeCase", false, false);
            ExpandTheGungeon.ExpandUtilities.ExpandUtility.GenerateOrAddToRigidBody(TimeItemStand, CollisionLayer.LowObstacle, PixelCollider.PixelColliderGeneration.Manual, UsesPixelsAsUnitSize: true, offset: new IntVector2(4, 3), dimensions: new IntVector2(22, 13));
            ExpandTheGungeon.ExpandUtilities.ExpandUtility.GenerateOrAddToRigidBody(TimeItemStand, CollisionLayer.EnemyBlocker, PixelCollider.PixelColliderGeneration.Manual, UsesPixelsAsUnitSize: true, offset: new IntVector2(4, 3), dimensions: new IntVector2(22, 13));
            TimePedestalClass timePedestal = TimeItemStand.AddComponent<TimePedestalClass>();
            timePedestal.ItemID = Pocketwatch;
            ExpandTheGungeon.ItemAPI.FakePrefab.DontDestroyOnLoad(TimeItemStand);
            ExpandTheGungeon.ItemAPI.FakePrefab.MarkAsFakePrefab(TimeItemStand);
        }
        public static void DungeonFlowInit(bool refreshFlows = false)
        {
            LoopItemRoomPrefab = ExpandTheGungeon.ExpandUtilities.RoomFactory.BuildFromResource("Items/Resources/LoopItemRoom.room");
            sharedAssets2 = ResourceManager.LoadAssetBundle("shared_auto_002");

            Dungeon ForgePrefab = DungeonDatabase.GetOrLoadByName("Base_Forge");
            ForgeData = ForgePrefab.PatternSettings.flows[0].sharedInjectionData[1];
            LoopItemRoom = new ProceduralFlowModifierData()
            {
                annotation = "Loop Item Room",
                DEBUG_FORCE_SPAWN = false,
                OncePerRun = false,
                placementRules = new List<ProceduralFlowModifierData.FlowModifierPlacementType>() {
                    ProceduralFlowModifierData.FlowModifierPlacementType.END_OF_CHAIN
                },
                roomTable = null,
                exactRoom = LoopItemRoomPrefab,
                IsWarpWing = false,
                RequiresMasteryToken = false,
                chanceToLock = 0,
                selectionWeight = 1,
                chanceToSpawn = 1,
                RequiredValidPlaceable = null,
                prerequisites = new DungeonPrerequisite[] {
                    new DungeonPrerequisite() {
                        prerequisiteOperation = DungeonPrerequisite.PrerequisiteOperation.EQUAL_TO,
                        prerequisiteType = DungeonPrerequisite.PrerequisiteType.TILESET,
                        requiredTileset = GlobalDungeonData.ValidTilesets.FORGEGEON,
                        requireTileset = true,
                        comparisonValue = 1,
                        encounteredObjectGuid = string.Empty,
                        maxToCheck = TrackedMaximums.MOST_KEYS_HELD,
                        requireDemoMode = false,
                        requireCharacter = false,
                        requiredCharacter = PlayableCharacters.Pilot,
                        requireFlag = false,
                        useSessionStatValue = false,
                        encounteredRoom = null,
                        requiredNumberOfEncounters = -1,
                        saveFlagToCheck = GungeonFlags.TUTORIAL_COMPLETED,
                        statToCheck = TrackedStats.GUNBERS_MUNCHED
                    }
                },
                CanBeForcedSecret = true,
                RandomNodeChildMinDistanceFromEntrance = 0,
                exactSecondaryRoom = null,
                framedCombatNodes = 0,
            };
            ExpandTheGungeon.ExpandObjects.ExpandObjectDatabase objectDatabase = new ExpandTheGungeon.ExpandObjects.ExpandObjectDatabase();


            BaseSharedInjectionData.InjectionData.Add(LoopItemRoom);
            ExpandTheGungeon.ExpandUtilities.RoomBuilder.AddObjectToRoom(LoopItemRoomPrefab, new Vector2(5, 7), objectDatabase.GodRays);
            ExpandTheGungeon.ExpandUtilities.RoomBuilder.AddObjectToRoom(LoopItemRoomPrefab, new Vector2(7, 8), ExpandTheGungeon.ExpandUtilities.ExpandUtility.GenerateDungeonPlacable(TimeItemStand, useExternalPrefab: true));
            objectDatabase = null;
            ForgePrefab = null;
            
        }
        private static int Pocketwatch = Gungeon.Game.Items["cel:grandfather_tick's_pocketwatch"].PickupObjectId;
    }
}
