using Dungeonator;
using MonoMod.RuntimeDetour;
using SaveAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Items
{
    class FloorGenStuff
    {
        public static void Init()
        {
            Hook floorLoadPlayerHook = new Hook(
                typeof(PlayerController).GetMethod("BraveOnLevelWasLoaded", BindingFlags.Instance | BindingFlags.Public),
                typeof(FloorGenStuff).GetMethod("OnNewFloor", BindingFlags.Static | BindingFlags.Public)
            );
        }
        private static bool hookDoubleUpPrevention;
        public static void OnNewFloor(Action<PlayerController> orig, PlayerController self)
        {
            bool isSecondary = false;
            if (GameManager.Instance.SecondaryPlayer && GameManager.Instance.SecondaryPlayer == self) isSecondary = true;
            bool flag = (!isSecondary) || (isSecondary && !GameManager.Instance.PrimaryPlayer);
            if (flag)
            {
                if (hookDoubleUpPrevention)
                {
                    //ETGModConsole.Log("Level loaded hook ran");
                   // CurrentFloorEnemyPalette = GeneratePalette();
                    Challenges.OnLevelLoaded();
                    hookDoubleUpPrevention = false;
                }
                else
                {
                    hookDoubleUpPrevention = true;
                }
            }
            orig(self);
        }
    }
}
