using System;
using System.Collections.Generic;
using System.Reflection;
using MonoMod.RuntimeDetour;
using UnityEngine;
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
                Hook hook = new Hook(
                    typeof(PlayerController).GetProperty("CanDetectHiddenEnemies", BindingFlags.Public | BindingFlags.Instance).GetGetMethod(),
                    typeof(Hooks).GetMethod("HiddenDetectionHook"));
               // Hook gunSpawnHook = new Hook(typeof(LootEngine).GetMethod("PostprocessGunSpawn", BindingFlags.Static | BindingFlags.NonPublic), typeof(Hooks).GetMethod("HandleGunModHook"));
                Hook activeUseHook = new Hook(typeof(PlayerController).GetMethod("UseItem", BindingFlags.Instance | BindingFlags.NonPublic), typeof(Hooks).GetMethod("OnUsedActiveHook"));
                
            }
            catch (Exception e)
            {
                ETGModConsole.Log("Error in Hooks.Init()");
                ETGModConsole.Log(e.Message);
            }
        }
        


        public static void OnUsedActiveHook(Action<PlayerController> orig, PlayerController player)
        {
            PlayerItem active = player.CurrentItem;
            if(active != null)
            {
                if(active.gameObject.GetComponent<PreventOnActiveEffects>() != null)
                {
                    if (!active.CanBeUsed(player))
                    {
                        return;
                    }
                    float num = -1f;
                    bool flag = active.Use(player, out num);

                    player.DoVibration(Vibration.Time.Quick, Vibration.Strength.Medium);
                }
                else
                {
                    orig(player);
                }
            }
        }
        public static bool HiddenDetectionHook(Func<PlayerController, bool> orig, PlayerController self)
        {
            return orig(self) || self.HasPickupID(ETGMod.Databases.Items["Eyes Of Stone"].PickupObjectId);
        }
        public static void HandleGunModHook(Action< Gun> orig, Gun gun)
        {
            gun.gameObject.SetActive(true);
            PlayerController player1 = GameManager.Instance.PrimaryPlayer;
            PlayerController player2 = null;
            if (GameManager.Instance.SecondaryPlayer != null)
            {
               player2 = GameManager.Instance.SecondaryPlayer;
            }
            float p1Risk = player1.gameObject.GetOrAddComponent<RiskStat>().RiskAMT;
            float p2Risk = 0;
            if (player2 != null)
            {
                p2Risk = player2.gameObject.GetOrAddComponent<RiskStat>().RiskAMT;
            }
            float basePerilChance = .075f;
            float perilChancePerRisk = .05f;
            float riskAmount = 0;
            if (p1Risk > p2Risk)
            {
                riskAmount = p1Risk;
            }
            else if (p1Risk < p2Risk)
            {
                riskAmount = p2Risk;
            }
            else if (p1Risk == p2Risk)
            {
                riskAmount = p1Risk;
            }
            float num1 = 0;
            if(riskAmount > 0) { Mathf.Clamp01(num1 = perilChancePerRisk * riskAmount); }
            float num2 = Mathf.Clamp01(basePerilChance + num1);
            if (UnityEngine.Random.value < num2)
            {
                gun.gameObject.AddComponent<PerilousParticles>();
            }
            orig(gun);
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
            PrimeSaw.HasGottenVice = false;
            ChestReplacementHooks.amountPerRun = 0;
            //CrownChanger.Change();
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
            PrimeSaw.HasGottenVice = false;
            ChestReplacementHooks.amountPerRun = 0;
            //CrownChanger.Change();
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
