using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using Dungeonator;
using MonoMod.RuntimeDetour;

namespace GungeonAPI
{
    public static class DungeonHooks
    {
        public static event Action<LoopDungeonGenerator, Dungeon, DungeonFlow, int> OnPreDungeonGeneration;
        public static event Action OnPostDungeonGeneration, OnFoyerAwake;
        private static GameManager targetInstance;
        public static FieldInfo m_assignedFlow =
            typeof(LoopDungeonGenerator).GetField("m_assignedFlow", BindingFlags.Instance | BindingFlags.NonPublic);

        private static Hook preDungeonGenHook = new Hook(
           typeof(LoopDungeonGenerator).GetConstructor(new Type[] { typeof(Dungeon), typeof(int) }),
           typeof(DungeonHooks).GetMethod("LoopGenConstructor")
        );

        private static Hook foyerAwakeHook = new Hook(
            typeof(MainMenuFoyerController).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance),
            typeof(DungeonHooks).GetMethod("FoyerAwake") //this no longer exists
        );

        public static void FoyerAwake(Action<MainMenuFoyerController> orig, MainMenuFoyerController self)
        {
            orig(self);
            OnFoyerAwake?.Invoke();
        }

        public static void LoopGenConstructor(Action<LoopDungeonGenerator, Dungeon, int> orig, LoopDungeonGenerator self, Dungeon dungeon, int dungeonSeed)
        {
            Tools.Print("-Loop Gen Called-", "5599FF");
            orig(self, dungeon, dungeonSeed);

            if (GameManager.Instance != null && GameManager.Instance != targetInstance)
            {
                targetInstance = GameManager.Instance;
                targetInstance.OnNewLevelFullyLoaded += OnLevelLoad;
            }

            var flow = (DungeonFlow)m_assignedFlow.GetValue(self);
            OnPreDungeonGeneration?.Invoke(self, dungeon, flow, dungeonSeed);
            dungeon = null;
        }

        public static void OnLevelLoad()
        {
            Tools.Print("-Post Gen Called-", "5599FF");
            OnPostDungeonGeneration?.Invoke();
        }
    }
}
