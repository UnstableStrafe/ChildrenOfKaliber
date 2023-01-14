using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gungeon;
using ItemAPI;
using GungeonAPI;
using MonoMod;
using Dungeonator;
using System.Collections;
using System.Reflection;


namespace Items.FloorStuff
{
    class ModRoomPrefabs
    {
        public static PrototypeDungeonRoom Mod_Entrance_Room;
        public static PrototypeDungeonRoom Mod_Exit_Room;
        public static PrototypeDungeonRoom[] Mod_Rooms;
        public static PrototypeDungeonRoom Mod_Boss;
        public static List<string> Mod_RoomList; // this will contain all of our mods rooms.
        public static void InitCustomRooms()
        {
            Mod_RoomList = new List<string>()
            {
                //the names of all of our rooms once we make them.
            };
            // Mod_Entrance_Room = RoomFactory.BuildFromResource("Mod/Resources/ModRooms/floorEntrance.room");
            // Mod_Exit_Room = RoomFactory.BuildFromResource("Mod/Resources/ModRooms/floorExit.room");

            List<PrototypeDungeonRoom> m_falseChamberRooms = new List<PrototypeDungeonRoom>();

            foreach (string name in Mod_RoomList)
            {
                PrototypeDungeonRoom m_room = RoomFactory.BuildFromResource("Items/FloorStuff/Rooms/FalseChamber/" + name).room;
                m_falseChamberRooms.Add(m_room);
            }

            Mod_Rooms = m_falseChamberRooms.ToArray();

            foreach (PrototypeDungeonRoom room in Mod_Rooms)
            {
                ModPrefabs.FalseChamberRoomTable.includedRooms.elements.Add(GenerateWeightedRoom(room, 1));
            }

            Mod_Boss = RoomFactory.BuildFromResource("mod/Resources/ModRooms/BossRoom.room").room;
            Mod_Boss.category = PrototypeDungeonRoom.RoomCategory.BOSS;
            Mod_Boss.subCategoryBoss = PrototypeDungeonRoom.RoomBossSubCategory.FLOOR_BOSS;
            Mod_Boss.subCategoryNormal = PrototypeDungeonRoom.RoomNormalSubCategory.COMBAT;
            Mod_Boss.subCategorySpecial = PrototypeDungeonRoom.RoomSpecialSubCategory.STANDARD_SHOP;
            Mod_Boss.subCategorySecret = PrototypeDungeonRoom.RoomSecretSubCategory.UNSPECIFIED_SECRET;
            Mod_Boss.roomEvents = new List<RoomEventDefinition>() {
                new RoomEventDefinition(RoomEventTriggerCondition.ON_ENTER_WITH_ENEMIES, RoomEventTriggerAction.SEAL_ROOM),
                new RoomEventDefinition(RoomEventTriggerCondition.ON_ENEMIES_CLEARED, RoomEventTriggerAction.UNSEAL_ROOM),
            };
            Mod_Boss.associatedMinimapIcon = ModPrefabs.doublebeholsterroom01.associatedMinimapIcon;
            Mod_Boss.usesProceduralLighting = false;
            Mod_Boss.usesProceduralDecoration = false;
            Mod_Boss.rewardChestSpawnPosition = new IntVector2(25, 20); //Where the reward pedestal spawns, should be changed based on room size
            Mod_Boss.overriddenTilesets = GlobalDungeonData.ValidTilesets.JUNGLEGEON;

            //foreach (PrototypeRoomExit exit in Mod_Boss.exitData.exits) { exit.exitType = PrototypeRoomExit.ExitType.ENTRANCE_ONLY; }
            //    RoomBuilder.AddExitToRoom(Mod_Boss, new Vector2(26, 37), DungeonData.Direction.NORTH, PrototypeRoomExit.ExitType.EXIT_ONLY, PrototypeRoomExit.ExitGroup.B);
        }

        public static WeightedRoom GenerateWeightedRoom(PrototypeDungeonRoom Room, float Weight = 1, bool LimitedCopies = true, int MaxCopies = 1, DungeonPrerequisite[] AdditionalPrerequisites = null)
        {
            if (Room == null) { return null; }
            if (AdditionalPrerequisites == null) { AdditionalPrerequisites = new DungeonPrerequisite[0]; }
            return new WeightedRoom() { room = Room, weight = Weight, limitedCopies = LimitedCopies, maxCopies = MaxCopies, additionalPrerequisites = AdditionalPrerequisites };
        }
    }
}
