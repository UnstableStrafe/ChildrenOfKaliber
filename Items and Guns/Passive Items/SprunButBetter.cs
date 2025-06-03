using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;
using System.Collections;
using HarmonyLib;
using MonoMod.RuntimeDetour;
namespace Items
{
    [HarmonyPatch]
    public class SprunButBetter : PlayerOrbitalItem
    {
        [HarmonyPatch(typeof(SprenOrbitalItem), nameof(SprenOrbitalItem.AssignTrigger))]
        [HarmonyPrefix]
        public static bool FixPart1(SprenOrbitalItem __instance)
        {
            __instance.m_trigger = SprenOrbitalItem.SprenTrigger.USED_LAST_BLANK;
            if (__instance.m_secondaryTrigger == SprenOrbitalItem.SprenTrigger.UNASSIGNED)
            {
                __instance.m_secondaryTrigger = (SprenOrbitalItem.SprenTrigger)Random.Range(1, 10);

                if (__instance.m_secondaryTrigger >= __instance.m_trigger)
                    __instance.m_secondaryTrigger = (SprenOrbitalItem.SprenTrigger)((int)__instance.m_secondaryTrigger + 1);
            }

            return false;
        }

        [HarmonyPatch(typeof(SprenOrbitalItem), nameof(SprenOrbitalItem.HandleBlank))]
        [HarmonyPrefix]
        public static void FixPart2(ref int BlanksRemaining)
        {
            BlanksRemaining = 0;
        }
    }
}

