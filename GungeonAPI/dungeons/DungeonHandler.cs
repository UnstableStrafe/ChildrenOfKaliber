using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using Dungeonator;
using Random = UnityEngine.Random;
using CustomShrineData = GungeonAPI.ShrineFactory.CustomShrineController;
using RoomData = GungeonAPI.RoomFactory.RoomData;
using RoomCategory = PrototypeDungeonRoom.RoomCategory;
using RoomNormalSubCategory = PrototypeDungeonRoom.RoomNormalSubCategory;
using RoomBossSubCategory = PrototypeDungeonRoom.RoomBossSubCategory;
using RoomSpecialSubCategory = PrototypeDungeonRoom.RoomSpecialSubCategory;
namespace GungeonAPI
{

    public static class DungeonHandler
    {
        //public static float GlobalRoomWeight = 1.5f;
        public static float GlobalRoomWeight = 100f;
        private static bool initialized = false;
        public static bool debugFlow = false;

        public static void Init()
        {
            if (!initialized)
            {
                RoomFactory.LoadRoomsFromRoomDirectory();
                DungeonHooks.OnPreDungeonGeneration += OnPreDungeonGen;
                initialized = true;
            }
        }

        public static void OnPreDungeonGen(LoopDungeonGenerator generator, Dungeon dungeon, DungeonFlow flow, int dungeonSeed)
        {
            Tools.Print("Attempting to override floor layout...", "5599FF");
            //CollectDataForAnalysis(flow, dungeon);
            if (flow.name != "Foyer Flow" && !GameManager.IsReturningToFoyerWithPlayer)
            {
                if (debugFlow)
                {
                    //flow = SampleFlow.CreateDebugFlow(dungeon);
                    generator.AssignFlow(flow);
                }
                Tools.Print("Dungeon name: " + dungeon.name);
                Tools.Print("Override Flow set to: " + flow.name);
            }
            dungeon = null;
        }

        public static void Register(RoomData roomData)
        {
            var room = roomData.room;
            var wRoom = new WeightedRoom()
            {
                room = room,
                additionalPrerequisites = new DungeonPrerequisite[0],
                weight = roomData.weight == 0 ? GlobalRoomWeight : roomData.weight
            };

            switch (room.category)
            {
                case RoomCategory.SPECIAL:
                    switch (room.subCategorySpecial)
                    {
                        case RoomSpecialSubCategory.STANDARD_SHOP:  //shops
                            StaticReferences.RoomTables["shop"].includedRooms.Add(wRoom);
                            break;
                        case RoomSpecialSubCategory.WEIRD_SHOP:    //subshops
                            StaticReferences.subShopTable.InjectionData.Add(GetFlowModifier(roomData));
                            break;
                        default:
                            StaticReferences.RoomTables["special"].includedRooms.Add(wRoom);
                            break;
                    }
                    break;
                case RoomCategory.SECRET:
                    StaticReferences.RoomTables["secret"].includedRooms.Add(wRoom);
                    break;
                case RoomCategory.BOSS:
                    //TODO - Boss rooms
                    break;
                default:
                    var tilesetPrereqs = new List<DungeonPrerequisite>();
                    foreach (var p in room.prerequisites)
                    {
                        if (p.requireTileset)
                        {
                            StaticReferences.GetRoomTable(p.requiredTileset).includedRooms.Add(wRoom);
                            tilesetPrereqs.Add(p);
                        }
                    }
                    foreach(var p in tilesetPrereqs)
                        room.prerequisites.Remove(p);
                    break;
            }
            //Tools.Print($"Registering {roomData.room.name} with weight {wRoom.weight} as {roomData.category}");
        }

        public static ProceduralFlowModifierData GetFlowModifier(RoomData roomData)
        {
            ProceduralFlowModifierData flowData = new ProceduralFlowModifierData()
            {

                annotation = roomData.room.name,
                placementRules = new List<ProceduralFlowModifierData.FlowModifierPlacementType>()
                {
                    ProceduralFlowModifierData.FlowModifierPlacementType.END_OF_CHAIN,
                    ProceduralFlowModifierData.FlowModifierPlacementType.HUB_ADJACENT_NO_LINK
                },
                exactRoom = roomData.room,
                selectionWeight = roomData.weight,
                chanceToSpawn = 1,
                prerequisites = roomData.room.prerequisites.ToArray(),
                CanBeForcedSecret = true,
            };
            return flowData;
        }

        public static bool BelongsOnThisFloor(RoomData data, string dungeonName)
        {
            if (data.floors == null || data.floors.Length == 0) return true;
            bool onThisFloor = false;
            foreach (var floor in data.floors)
            {
                if (floor.ToLower().Equals(dungeonName.ToLower())) { onThisFloor = true; break; }
            }
            return onThisFloor;
        }

        public static GenericRoomTable GetSpecialRoomTable()
        {
            foreach (var entry in GameManager.Instance.GlobalInjectionData.entries)
                if (entry.injectionData?.InjectionData != null)
                    foreach (var data in entry.injectionData.InjectionData)
                        if (data.roomTable != null && data.roomTable.name.ToLower().Contains("basic special rooms"))
                            return data.roomTable;
            return null;
        }

        public static void CollectDataForAnalysis(DungeonFlow flow, Dungeon dungeon)
        {
            try
            {
                foreach (var room in flow.fallbackRoomTable.includedRooms.elements)
                {
                    Tools.Print("Fallback table: " + room?.room?.name);
                }
            }
            catch (Exception e)
            {
                Tools.PrintException(e);
            }
            dungeon = null;
        }

        public static void LogProtoRoomData(PrototypeDungeonRoom room)
        {
            int i = 0;
            Tools.LogPropertiesAndFields(room, "ROOM");
            foreach (var placedObject in room.placedObjects)
            {
                Tools.Log($"\n----------------Object #{i++}----------------");
                Tools.LogPropertiesAndFields(placedObject, "PLACED OBJECT");
                Tools.LogPropertiesAndFields(placedObject?.placeableContents, "PLACEABLE CONTENT");
                Tools.LogPropertiesAndFields(placedObject?.placeableContents?.variantTiers[0], "VARIANT TIERS");
            }

            Tools.Print("==LAYERS==");
            foreach (var layer in room.additionalObjectLayers)
            {
                //Tools.LogPropertiesAndFields(layer);
            }
        }
    }
}
