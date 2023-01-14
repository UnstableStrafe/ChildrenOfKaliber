using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using UnityEngine;

namespace Items
{
	class FalseChamberDungeonFlows
	{
        public static DungeonFlow LoadCustomFlow(Func<string, DungeonFlow> orig, string target)
        {
            string flowName = target;
            if (flowName.Contains("/")) { flowName = target.Substring(target.LastIndexOf("/") + 1); }
            if (KnownFlows != null && KnownFlows.Count > 0)
            {
                foreach (DungeonFlow flow in KnownFlows)
                {
                    if (flow.name != null && flow.name != string.Empty)
                    {
                        if (flowName.ToLower() == flow.name.ToLower())
                        {
                            DebugTime.RecordStartTime();
                            DebugTime.Log("AssetBundle.LoadAsset<DungeonFlow>({0})", new object[] { flowName });
                            return flow;
                        }
                    }
                }
            }
            return orig(target);
        }

        public static DungeonFlow LoadOfficialFlow(string target)
        {
            string flowName = target;
            if (flowName.Contains("/")) { flowName = target.Substring(target.LastIndexOf("/") + 1); }
            AssetBundle m_assetBundle_orig = ResourceManager.LoadAssetBundle("flows_base_001");
            DebugTime.RecordStartTime();
            DungeonFlow result = m_assetBundle_orig.LoadAsset<DungeonFlow>(flowName);
            DebugTime.Log("AssetBundle.LoadAsset<DungeonFlow>({0})", new object[] { flowName });
            if (result == null)
            {
                Debug.Log("ERROR: Requested DungeonFlow not found!\nCheck that you provided correct DungeonFlow name and that it actually exists!");
                m_assetBundle_orig = null;
                return null;
            }
            else
            {
                m_assetBundle_orig = null;
                return result;
            }
        }

        public static List<DungeonFlow> KnownFlows;

        public static DungeonFlow Foyer_Flow;

        // Default stuff to use with custom Flows
        public static SharedInjectionData BaseSharedInjectionData;
        public static SharedInjectionData GungeonInjectionData;
        public static SharedInjectionData SewersInjectionData;
        public static SharedInjectionData HollowsInjectionData;
        public static SharedInjectionData CastleInjectionData;

        public static ProceduralFlowModifierData SecretFalseChamberEntranceInjector;

        public static DungeonFlowSubtypeRestriction BaseSubTypeRestrictions = new DungeonFlowSubtypeRestriction()
        {
            baseCategoryRestriction = PrototypeDungeonRoom.RoomCategory.NORMAL,
            normalSubcategoryRestriction = PrototypeDungeonRoom.RoomNormalSubCategory.TRAP,
            bossSubcategoryRestriction = PrototypeDungeonRoom.RoomBossSubCategory.FLOOR_BOSS,
            specialSubcategoryRestriction = PrototypeDungeonRoom.RoomSpecialSubCategory.UNSPECIFIED_SPECIAL,
            secretSubcategoryRestriction = PrototypeDungeonRoom.RoomSecretSubCategory.UNSPECIFIED_SECRET,
            maximumRoomsOfSubtype = 1
        };

        // Custom Room Table for Keep Shared Injection Data 
        public static GenericRoomTable m_KeepEntranceRooms;

        // Generate a DungeonFlowNode with a default configuration
        public static DungeonFlowNode GenerateDefaultNode(DungeonFlow targetflow, PrototypeDungeonRoom.RoomCategory roomType, PrototypeDungeonRoom overrideRoom = null, GenericRoomTable overrideTable = null, bool oneWayLoopTarget = false, bool isWarpWingNode = false, string nodeGUID = null, DungeonFlowNode.NodePriority priority = DungeonFlowNode.NodePriority.MANDATORY, float percentChance = 1, bool handlesOwnWarping = true)
        {
            try
            {
                if (string.IsNullOrEmpty(nodeGUID)) { nodeGUID = Guid.NewGuid().ToString(); }

                DungeonFlowNode m_CachedNode = new DungeonFlowNode(targetflow)
                {
                    isSubchainStandin = false,
                    nodeType = DungeonFlowNode.ControlNodeType.ROOM,
                    roomCategory = roomType,
                    percentChance = percentChance,
                    priority = priority,
                    overrideExactRoom = overrideRoom,
                    overrideRoomTable = overrideTable,
                    capSubchain = false,
                    subchainIdentifier = string.Empty,
                    limitedCopiesOfSubchain = false,
                    maxCopiesOfSubchain = 1,
                    subchainIdentifiers = new List<string>(0),
                    receivesCaps = false,
                    isWarpWingEntrance = isWarpWingNode,
                    handlesOwnWarping = handlesOwnWarping,
                    forcedDoorType = DungeonFlowNode.ForcedDoorType.NONE,
                    loopForcedDoorType = DungeonFlowNode.ForcedDoorType.NONE,
                    nodeExpands = false,
                    initialChainPrototype = "n",
                    chainRules = new List<ChainRule>(0),
                    minChainLength = 3,
                    maxChainLength = 8,
                    minChildrenToBuild = 1,
                    maxChildrenToBuild = 1,
                    canBuildDuplicateChildren = false,
                    guidAsString = nodeGUID,
                    parentNodeGuid = string.Empty,
                    childNodeGuids = new List<string>(0),
                    loopTargetNodeGuid = string.Empty,
                    loopTargetIsOneWay = oneWayLoopTarget,
                    flow = targetflow
                };



                return m_CachedNode;
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.ToString());
                return null;
            }
        }


        // Retrieve sharedInjectionData from a specific floor if one is available
        public static List<SharedInjectionData> RetrieveSharedInjectionDataListFromCurrentFloor()
        {
            Dungeon dungeon = GameManager.Instance.CurrentlyGeneratingDungeonPrefab;

            if (dungeon == null)
            {
                dungeon = GameManager.Instance.Dungeon;
                if (dungeon == null) { return new List<SharedInjectionData>(0); }

            }

            if (dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON |
                dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.WESTGEON |
                dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FINALGEON |
                dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.HELLGEON |
                dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.OFFICEGEON |
                dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.RATGEON)
            {
                return new List<SharedInjectionData>(0);
            }

            List<SharedInjectionData> m_CachedInjectionDataList = new List<SharedInjectionData>(0);

            if (dungeon.PatternSettings != null && dungeon.PatternSettings.flows != null && dungeon.PatternSettings.flows.Count > 0)
            {
                if (dungeon.PatternSettings.flows[0].sharedInjectionData != null && dungeon.PatternSettings.flows[0].sharedInjectionData.Count > 0)
                {
                    m_CachedInjectionDataList = dungeon.PatternSettings.flows[0].sharedInjectionData;
                }
            }

            return m_CachedInjectionDataList;
        }

        public static ProceduralFlowModifierData RickRollSecretRoomInjector;

        public static SharedInjectionData CustomSecretFloorSharedInjectionData;


        // Initialize KnownFlows array with custom + official flows.
        public static void InitDungeonFlows(bool refreshFlows = false)
        {

            Dungeon TutorialPrefab = DungeonDatabase.GetOrLoadByName("Base_Tutorial");
            Dungeon CastlePrefab = DungeonDatabase.GetOrLoadByName("Base_Castle");
            Dungeon SewerPrefab = DungeonDatabase.GetOrLoadByName("Base_Sewer");
            Dungeon GungeonPrefab = DungeonDatabase.GetOrLoadByName("Base_Gungeon");
            Dungeon CathedralPrefab = DungeonDatabase.GetOrLoadByName("Base_Cathedral");
            Dungeon MinesPrefab = DungeonDatabase.GetOrLoadByName("Base_Mines");
            Dungeon ResourcefulRatPrefab = DungeonDatabase.GetOrLoadByName("Base_ResourcefulRat");
            Dungeon CatacombsPrefab = DungeonDatabase.GetOrLoadByName("Base_Catacombs");
            Dungeon NakatomiPrefab = DungeonDatabase.GetOrLoadByName("Base_Nakatomi");
            Dungeon ForgePrefab = DungeonDatabase.GetOrLoadByName("Base_Forge");
            Dungeon BulletHellPrefab = DungeonDatabase.GetOrLoadByName("Base_BulletHell");

            BaseSharedInjectionData = ModPrefabs.shared_auto_002.LoadAsset<SharedInjectionData>("Base Shared Injection Data");
            GungeonInjectionData = GungeonPrefab.PatternSettings.flows[0].sharedInjectionData[1];
            SewersInjectionData = SewerPrefab.PatternSettings.flows[0].sharedInjectionData[1];
            HollowsInjectionData = CatacombsPrefab.PatternSettings.flows[0].sharedInjectionData[1];
            CastleInjectionData = CastlePrefab.PatternSettings.flows[0].sharedInjectionData[0];


            m_KeepEntranceRooms = ScriptableObject.CreateInstance<GenericRoomTable>();
            m_KeepEntranceRooms.includedRoomTables = new List<GenericRoomTable>(0);
            m_KeepEntranceRooms.includedRooms = new WeightedRoomCollection()
            {
                elements = new List<WeightedRoom>() {
                    //we will place the entrance to our floor here.
                }
            };


            SecretFalseChamberEntranceInjector = new ProceduralFlowModifierData()
            {
                annotation = "Secret Floor Entrance Room",
                DEBUG_FORCE_SPAWN = false,
                OncePerRun = false,
                placementRules = new List<ProceduralFlowModifierData.FlowModifierPlacementType>() {
                    ProceduralFlowModifierData.FlowModifierPlacementType.RANDOM_NODE_CHILD
                },
                roomTable = m_KeepEntranceRooms,
                // exactRoom = SewersInjectionData.InjectionData[0].exactRoom,
                exactRoom = null,
                RequiresMasteryToken = false,
                chanceToLock = 0,
                selectionWeight = 1,
                chanceToSpawn = 1,
                RequiredValidPlaceable = null,
                prerequisites = new DungeonPrerequisite[0],
                CanBeForcedSecret = true,
                RandomNodeChildMinDistanceFromEntrance = 0,
                exactSecondaryRoom = null,
                framedCombatNodes = 0,
            };

            CastleInjectionData.InjectionData.Add(SecretFalseChamberEntranceInjector);

            // Don't build/add flows until injection data is created!
            Foyer_Flow = FlowHelpers.DuplicateDungeonFlow(ModPrefabs.shared_auto_002.LoadAsset<DungeonFlow>("Foyer Flow"));

            // List<DungeonFlow> m_knownFlows = new List<DungeonFlow>();
            KnownFlows = new List<DungeonFlow>();

            //we will add our custom flow here soon.
            //KnownFlows.Add(f1b_FloorName_flow_01.F1b_FloorName_flow_01());


            // Fix issues with nodes so that things other then MainMenu can load Foyer flow
            Foyer_Flow.name = "Foyer_Flow";
            Foyer_Flow.AllNodes[1].handlesOwnWarping = true;
            Foyer_Flow.AllNodes[2].handlesOwnWarping = true;
            Foyer_Flow.AllNodes[3].handlesOwnWarping = true;

            KnownFlows.Add(Foyer_Flow);

            // Add official flows to list (flows found in Dungeon asset bundles after AG&D)
            for (int i = 0; i < TutorialPrefab.PatternSettings.flows.Count; i++)
            {
                KnownFlows.Add(FlowHelpers.DuplicateDungeonFlow(TutorialPrefab.PatternSettings.flows[i]));
            }
            for (int i = 0; i < CastlePrefab.PatternSettings.flows.Count; i++)
            {
                KnownFlows.Add(FlowHelpers.DuplicateDungeonFlow(CastlePrefab.PatternSettings.flows[i]));
            }
            for (int i = 0; i < SewerPrefab.PatternSettings.flows.Count; i++)
            {
                KnownFlows.Add(FlowHelpers.DuplicateDungeonFlow(SewerPrefab.PatternSettings.flows[i]));
            }
            for (int i = 0; i < GungeonPrefab.PatternSettings.flows.Count; i++)
            {
                KnownFlows.Add(FlowHelpers.DuplicateDungeonFlow(GungeonPrefab.PatternSettings.flows[i]));
            }
            for (int i = 0; i < CathedralPrefab.PatternSettings.flows.Count; i++)
            {
                KnownFlows.Add(FlowHelpers.DuplicateDungeonFlow(CathedralPrefab.PatternSettings.flows[i]));
            }
            for (int i = 0; i < MinesPrefab.PatternSettings.flows.Count; i++)
            {
                KnownFlows.Add(FlowHelpers.DuplicateDungeonFlow(MinesPrefab.PatternSettings.flows[i]));
            }
            for (int i = 0; i < ResourcefulRatPrefab.PatternSettings.flows.Count; i++)
            {
                KnownFlows.Add(FlowHelpers.DuplicateDungeonFlow(ResourcefulRatPrefab.PatternSettings.flows[i]));
            }
            for (int i = 0; i < CatacombsPrefab.PatternSettings.flows.Count; i++)
            {
                KnownFlows.Add(FlowHelpers.DuplicateDungeonFlow(CatacombsPrefab.PatternSettings.flows[i]));
            }
            for (int i = 0; i < NakatomiPrefab.PatternSettings.flows.Count; i++)
            {
                KnownFlows.Add(FlowHelpers.DuplicateDungeonFlow(NakatomiPrefab.PatternSettings.flows[i]));
            }
            for (int i = 0; i < ForgePrefab.PatternSettings.flows.Count; i++)
            {
                KnownFlows.Add(FlowHelpers.DuplicateDungeonFlow(ForgePrefab.PatternSettings.flows[i]));
            }
            for (int i = 0; i < BulletHellPrefab.PatternSettings.flows.Count; i++)
            {
                KnownFlows.Add(FlowHelpers.DuplicateDungeonFlow(BulletHellPrefab.PatternSettings.flows[i]));
            }


            TutorialPrefab = null;
            CastlePrefab = null;
            SewerPrefab = null;
            GungeonPrefab = null;
            CathedralPrefab = null;
            MinesPrefab = null;
            ResourcefulRatPrefab = null;
            CatacombsPrefab = null;
            NakatomiPrefab = null;
            ForgePrefab = null;
            BulletHellPrefab = null;
        }
    }
}

