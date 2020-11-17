using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using MonoMod.RuntimeDetour;
using UnityEngine;
using ItemAPI;
using Dungeonator;

namespace Items
{
    public static class Hooks
    {

        public static void Init() 
        {

            try
            {
                Hook foyerCallbacksHook = new Hook(
                    typeof(MainMenuFoyerController).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance),
                    typeof(Hooks).GetMethod("FoyerAwake")
                );

                Hook quickRestartHook = new Hook(
                    typeof(GameManager).GetMethod("DelayedQuickRestart", BindingFlags.Public | BindingFlags.Instance),
                    typeof(Hooks).GetMethod("OnQuickRestart")
                );

                /* Hook pickupHook = new Hook
                     (
                     typeof(PickupObject).GetMethod("Pickup", BindingFlags.Public | BindingFlags.Instance),
                     typeof(Hooks).GetMethod("PickupHooks")
                     );
                 */

                //    Hook newRoom = new Hook(typeof(PlayerController).GetMethod("EnteredNewRoom", BindingFlags.NonPublic | BindingFlags.Instance),
                //     typeof(Hooks).GetMethod("EnterRoom"));
                Hook synHook = new Hook(
                    typeof(StringTableManager).GetMethod("GetSynergyString", BindingFlags.Static | BindingFlags.Public),
                    typeof(Hooks).GetMethod("SynergyStringHook")
                );
                
            }

            catch (Exception e)
            {
                ETGModConsole.Log("Error in Hooks.Init()");
                ETGModConsole.Log(e.Message);
            }
        }

        

       public static List<AGDEnemyReplacementTier> m_cachedReplacementTiers = GameManager.Instance.EnemyReplacementTiers;

        public static string SynergyStringHook(Func<string, int, string> orig, string key, int index = -1)
        {
            string text = orig(key, index);
            bool flag = string.IsNullOrEmpty(text);
            if (flag) text = key;
            return text;
        }
        public static void OnQuickRestart(Action<GameManager, float, QuickRestartOptions> orig, GameManager self, float duration, QuickRestartOptions options = default(QuickRestartOptions))
        {
            orig(self, duration, options);
            RGG.RandomizeStats();
            Replacement.RunReplace(m_cachedReplacementTiers);
            Monogun.MonogunR();
            LoopManager.LoopAMT = 0;
            AIActor.HealthModifier = 1f;
            PrimeSaw.HasGottenVice = false;
        }


        public static void HardmodeTweaks()
        {
            Gun Gunther = PickupObjectDatabase.GetById(338) as Gun;
            Gunther.quality = PickupObject.ItemQuality.EXCLUDED;
            Gun BSG = PickupObjectDatabase.GetById(21) as Gun;
            BSG.quality = PickupObject.ItemQuality.EXCLUDED;
            Gun RaidenCoil = PickupObjectDatabase.GetById(107) as Gun;
            RaidenCoil.quality = PickupObject.ItemQuality.EXCLUDED;
            Gun Tentacle = PickupObjectDatabase.GetById(474) as Gun;
            Tentacle.quality = PickupObject.ItemQuality.EXCLUDED;
            Gun Gunderfury = PickupObjectDatabase.GetById(732) as Gun;
            Gunderfury.quality = PickupObject.ItemQuality.EXCLUDED;
            PlayerItem WeirdEgg = PickupObjectDatabase.GetById(637) as PlayerItem;
            WeirdEgg.quality = PickupObject.ItemQuality.EXCLUDED;
        }
        private static void AffectEnemy(AIActor target)
        {
            bool flag = !target.IsNormalEnemy && target.healthHaver.IsBoss && !target.IsHarmlessEnemy;
            if (flag)
            {
                AIActor target2 = target;
                AIActor.Spawn(target2, target.transform.position, playerS.CurrentRoom, true, AIActor.AwakenAnimationType.Default, true);
                target.healthHaver.ApplyDamage((target.healthHaver.GetMaxHealth() * .4f), Vector2.zero, "Ishar", CoreDamageTypes.None, DamageCategory.Normal, false, null, true);
                target2.healthHaver.ApplyDamage((target2.healthHaver.GetMaxHealth() * .4f), Vector2.zero, "Ishar", CoreDamageTypes.None, DamageCategory.Normal, false, null, true);
            }


        }
        public static bool PlayerHasActiveSynergy(this PlayerController player, string synergyNameToCheck)
        {
            foreach (int num in player.ActiveExtraSynergies)
            {
                AdvancedSynergyEntry advancedSynergyEntry = GameManager.Instance.SynergyManager.synergies[num];
                bool flag = advancedSynergyEntry.NameKey == synergyNameToCheck;
                if (flag)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool OwnerHasSynergy(this Gun gun, string synergyName)
		{
			return gun.CurrentOwner is PlayerController && (gun.CurrentOwner as PlayerController).PlayerHasActiveSynergy(synergyName);
		}
        public static void FoyerAwake(Action<MainMenuFoyerController> orig, MainMenuFoyerController self)
        {
            orig(self);
            RGG.RandomizeStats();
            Replacement.RunReplace(m_cachedReplacementTiers);
            Monogun.MonogunR();
            if(LoopManager.UsedLoop != true)
            {
                LoopManager.LoopAMT = 0;
                AIActor.HealthModifier = 1f;
            }
            if(LoopManager.UsedLoop == true)
            {
                LoopManager.UsedLoop = false;
            }
            PrimeSaw.HasGottenVice = false;
        }
        public static void BuffGuns()
        {
            Gun GildedHydra = PickupObjectDatabase.GetById(231) as Gun;
            GildedHydra.SetBaseMaxAmmo(120);
            GildedHydra.reloadTime = 1.6f;
            GildedHydra.ammo = 120;
            GildedHydra.DefaultModule.numberOfShotsInClip = 3;
        }

        public static int GetID(string console_id)
        {
           return Gungeon.Game.Items[console_id].PickupObjectId;
        }
        
        public static PlayerController playerS;
    }
}
