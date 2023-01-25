
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;
using MonoMod.Utils;
using Dungeonator;
using Brave.BulletScript;
using FullSerializer;
using System.Collections;
using Gungeon;
using Alexandria.DungeonAPI;
using EnemyBulletBuilder;
using SaveAPI;
using Alexandria.CharacterAPI;
using Alexandria.EnemyAPI;
using System.Runtime;
using BepInEx;
using System.IO;
using Alexandria;
using Alexandria.ItemAPI;
using Alexandria.Misc;

namespace Items
{
    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInDependency("etgmodding.etg.mtgapi")]
    public class ChildrenOfKaliberModule : BaseUnityPlugin
    {
        public const string NAME = "Children of Kaliber";
        public const string GUID = "unstablestrafe.etg.childrenofkaliber";
        public const string VERSION = "3.9.3";


        public static readonly string Color = "#FCA4E2";
        public static string ZipFilePath;
        public static string FilePath;
        public static string ModName;
        public static string RoomFilePath;
        public static HellDragZoneController hellDrag;
        public static string SteamUsername;
        public static AssetBundle ModAssets;
        public static AdvancedStringDB Strings;
        //public static ETGModuleMetadata metadata = new ETGModuleMetadata();
        public void Awake()
        {

        }

        public void Start()
        {
            ETGModMainBehaviour.WaitForGameManagerStart(GMStart);
        }

        public void GMStart(GameManager g)
        {
           
            
            FilePath = this.FolderPath();
            ZipFilePath = FilePath;
            RoomFilePath = Path.Combine(FilePath, "rooms");

            ETGMod.Assets.SetupSpritesFromFolder((this.FolderPath()+"/sprites"));

            //EnemyAPI
            Hooks.Init();
            EnemyTools.Init();
            EnemyBuilder.Init();
            BossBuilder.Init();

            SaveAPI.SaveAPIManager.Setup("ck");
            ItemBuilder.Init();
            Hooks.Init();

            ChestReplacementHooks.Init();

            
            //metadata = this.Metadata;
            Library.DefineGoops();

            SpecialBlankModificationItem.InitHooks();
            var forgeDungeon = DungeonDatabase.GetOrLoadByName("Base_Forge");
            hellDrag = forgeDungeon.PatternSettings.flows[0].AllNodes.Where(node => node.overrideExactRoom != null && node.overrideExactRoom.name.Contains("EndTimes")).First().overrideExactRoom.placedObjects.Where(ppod => ppod != null && ppod.nonenemyBehaviour != null).First().nonenemyBehaviour.gameObject.GetComponentsInChildren<HellDragZoneController>()[0];
            forgeDungeon = null;
            MultiActiveReloadManager.SetupHooks();
            CustomClipAmmoTypeToolbox.Init();
            //---  PrinceOfTheJammed.Init();
            //--- PoisonGeistEnemy.Init();
            //---  RiskyAmmoCrate.Register();
            Strings = new AdvancedStringDB();
            AudioResourceLoader.InitAudio();
            ModAssets = AssetBundleLoader.LoadAssetBundleFromLiterallyAnywhere("childrenofkaliberassets");
            /*
            foreach (string str in ModAssets.GetAllAssetNames())
            {
                ETGModConsole.Log(ModAssets.name + ": " + str);
            }
            */
            ETGModConsole.Commands.AddGroup("ck", args =>
            {
            });

            ETGModConsole.Commands.GetGroup("ck").AddUnit("debugflow", (args) =>
            {
                DungeonHandler.debugFlow = !DungeonHandler.debugFlow;
                string status = DungeonHandler.debugFlow ? "enabled" : "disabled";
                string color = DungeonHandler.debugFlow ? "00FF00" : "FF0000";
                ETGModConsole.Log($"Debug flow {status}", false);
            });
            FloorGenStuff.Init();
            Challenges.Init();

            //Items/Guns

            //ACTIVES

            TheBullet.Init();
            CorruptHeart.Init();              
            ShockingBattery.Init();
            IronHeart.Init(); 
            VoidBottle.Init();
            SuperCrate.Init();
            Matchbox.Init();
            ACMESupply.Init();
            Questlog.Init();

            PhaseBurst.Init();

            //PASSIVES

            FunkyBullets.Init(); 
            NeedleBullets.Init(); 
            ImpsHorn.Init(); 
            SnareBullets.Init();
            SprunButBetter.Init();      
            PetRock.Init();
            SteamSale.Init(); 
            EmptyBullets.Init();
            Sawblade.Init();
            Mininomicon.Init();
            TerribleAmmoBag.Init();
            Gunlust.Init();
            BananaJamHands.Init();
            DaggerSpray.Init();
            AuricVial.Init();
            DragunClaw.Init();
            DragunWing.Init();
            DragunHeart.Init();    
            DragunSkull.Init();
            GravityWell.Init();

            TemporalShift.Init();

            //GUNS

            Dispenser.Add();
            RGG.Add(); 
            Phazor.Add();
            Tesla.Add();
            Fallout.Add();
            StakeLauncher.Add();

            PhantomPistol.Add();


            WaterCup.Init(); //36                ACTIVE 10
            SpinelTonic.Init(); //37             ACTIVE 11
            DDR.Init(); //38                     PASSIVE 21
            CitrineAmmolet.Init(); // 39         PASSIVE 22
            MalachiteAmmolet.Init(); //40        PASSIVE 23
            GlassHeart.Init(); // 41             PASSIVE 24
            AccursedShackles.Init();// 42        PASSIVE 25   
            Hooks.BuffGuns(); //43               GUN 7
            D6.Init(); //44                      ACTIVE 12

            //TimeKeepersPistol.Add();
            ALiteralRock.Add(); // 47            GUN 8 
            FloopBullets.Init(); // 48           PASSIVE 26     
            EliteBullets.Init();// 49            PASSIVE 27
            Enemies.AmmoDotBehav();
            FireworkRifle.Add(); // 50           GUN 9
            Skeleton.Add();// 51                 GUN 10
            Incremental.Add();// 52              GUN 11

            VenomRounds.Init();// 53             PASSIVE 28
            BismuthAmmolet.Init();//54           PASSIVE 29
            MercuryAmmolet.Init();//55           PASSIVE 30


            BashelliskRifle.Add();//56           GUN 12
            MarkOfWind.Init();//57               PASSIVE 31
            PrimeSaw.Add();//58a                 GUN 13a                      
            PrimeVice.Add();//58b                GUN 13b
            PrimeLaser.Add();//58c               GUN 13c
            PrimeCannon.Add();//58d              GUN 13d
            RussianRevolver.Add();//59           GUN 14
            ImpactRounds.Init();//60             PASSIVE 31
            DroneController.Add();//61           GUN 15
            Drone.Add();
            Drone2.Add();
            
            //True Gunpowder Set
            PrimalNitricAcid.Init();//66         PASSIVE 33
            PrimalCharcoal.Init();//67           PASSIVE 34
            PrimalSulfur.Init();//68             PASSIVE 35
            PrimalSaltpeter.Init();//69          PASSIVE 36
            TrueGunpowder.Init();//70            PASSIVE 37
            //
            SpiritOfTheDragun.Add();//71         GUN 16
            BilliardBouncer.Add();//72           GUN 17
            //ElectricBass.Add();//73              GUN 18
            IonFist.Add();//74                   GUN 19
            NenFist.Add();//74a
            EvilCharmedBow.Add();
            ReloadedRifle.Add();//75             GUN 20
            AK94.Add();//76                      GUN 21
            LeakingSyringe.Init();//77           PASSIVE 38
            Holoprinter.Init();//78              ACTIVE 14
            Günther.Add();//79                   GUN 22
            AK141.Add();//80                     GUN 23
            AK188.Add();//81                     GUN 24
            InfiniteAK.Add();//82                GUN 25

            PlasmaCannon.Add();//85              GUN 25
            TheLastChamber.Add();//86            GUN 27

            AutoShotgun.Add();//87               GUN 28


            Vacuum.Add();//88                    GUN 29
            StickyLauncher.Add();//89            GUN 20
            SupportContract.Init();//90          ACTIVE 15
            PanicPistol.Add();//91               GUN 31            
            PepperBox.Add();//92           GUN 32
            //ReversedGun.Add();

            SympathyBullets.Init();//93          PASSIVE 39

            BloatedRounds.Init();//94            PASSIVE 40
            EyesOfStone.Init();//95              PASSIVE 41
            ShellBank.Init(); //96                PASSIVE 42
            //HairGel.Init();
            //IgnitionPowder.Init();
            //Magnorbs.Add(); // need to do the orbiting code 
            //PoisonPoltergeist.Init();

            PsiFocus.Init();
            PsiScales.Add();


            //SliverBeam.Add();

            GoldenRecord.Init();//97             ACTIVE 16
            //Baneshee.Add();
            //RebarCrossbow.Add();
            //--MemeticKillAgent.Init();
            //--TemporalRounds.Init();
            // CultistsCrook.Add();
            //  Break_ActionRevolver.Add();

            Pray_K47.Add();




            //Ultimates
            //  MarineUltimate.Init();

            //RobotUltimate.Init();
            //MiniUberbotHand.Add();

            //    HunterUltimate.Init();
            

            //SusieNKris.Init();


            //CHESTS
            MunitionsChestController.Init();


            //NPCS

            CultistShopkeep.Init();


            //OBJECTS
            BulletDrum.Init();
            GunpodwerFactorySewerGrate.Init();
            GunpodwerFactoryBarrelSpawner.Init();


            //ENEMIES

            Enemies.SpectreBehav();
            //--GunpowderFactory.Init();


            //ROOMS



            //CHARACTERS

            var pursued = Loader.BuildCharacter("Items/Characters/Pursued", "ck:pursued", new Vector3(23, 27, 27.1f), false, new Vector3(15.3f, 24.8f, 25.3f), true, false, false, true, false, null, null, 0, false, "");

            /*
            pursued.prerequisites = new DungeonPrerequisite[1]
            {
                    new CustomDungeonPrerequisite
                    {
                        requireCustomFlag = true,
                        customFlagToCheck = CustomDungeonFlags.PURSUED_UNLOCK,
                    }
            };
            */

            //MISC

            LiteralTrash.Init();

            ETGMod.StartGlobalCoroutine(this.DelayedStartCR());

            foreach (AdvancedSynergyEntry syn in GameManager.Instance.SynergyManager.synergies)
            {
                if (syn.NameKey == "#IRONSHOT")
                {
                    syn.OptionalGunIDs.Add(ETGMod.Databases.Items["billiard_bouncer"].PickupObjectId);
                }
            }

            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new ChildrenOKaliberSynergies.ExtravaganceSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new ChildrenOKaliberSynergies.RulebookSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new ChildrenOKaliberSynergies.SturdyAmmoSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new ChildrenOKaliberSynergies.LeadWeightSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new ChildrenOKaliberSynergies.DragunRoarSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new ChildrenOKaliberSynergies.IronManSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new ChildrenOKaliberSynergies.EldritchLoveSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new ChildrenOKaliberSynergies.TwoForOneSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new ChildrenOKaliberSynergies.CriticalMassSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new ChildrenOKaliberSynergies.OverclockedSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new ChildrenOKaliberSynergies.SovietSkillsSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new ChildrenOKaliberSynergies.CommunistIdealsSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new ChildrenOKaliberSynergies.LuckySpinSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new ChildrenOKaliberSynergies.TestGunSyn()
            }).ToArray<AdvancedSynergyEntry>();

            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new ChildrenOKaliberSynergies.TrueGunpowderSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new ChildrenOKaliberSynergies.GoldenSpinSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new ChildrenOKaliberSynergies.LoveLeechSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new ChildrenOKaliberSynergies.JajankenSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new ChildrenOKaliberSynergies.AllOutOfLoveSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new ChildrenOKaliberSynergies.BleedingEdgeSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new ChildrenOKaliberSynergies.AbsoluteChaosSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
               new ChildrenOKaliberSynergies.ShakenSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
               new ChildrenOKaliberSynergies.CallingInTheHeavySupportSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
               new ChildrenOKaliberSynergies.BloodlessSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
{
               new ChildrenOKaliberSynergies.KvotheKingKillerSyn()
            }).ToArray<AdvancedSynergyEntry>();

            
            DungeonHandler.Init();







            ETGModConsole.Commands.GetGroup("ck").AddUnit("mchest", this.SpawnMChest);
            ETGModConsole.Commands.GetGroup("ck").AddUnit("toggle_eyestrain", ToggleEyeStrain);
            //---ETGModConsole.Commands.GetGroup("ck").AddUnit("get_risk_value", GetRiskStatValue);
            Log($"{NAME} Initialized", Color);
            Log($"Link to Changelog https://pastebin.com/0LeBBa57", Color);
        }

        private void CrownOverride(string[] args)
        {
            if (args.Length < 1)
            {
                ETGModConsole.Log("At least 1 arguments required.");
            }
            else
            {
                SteamUsername = args[0];
                ETGModConsole.Log($"Name : {SteamUsername}");
                CrownChanger.Change();
            }
        }
        private void SpawnMChest(string[] args)
        {
            Chest testMChest = MunitionsChestController.munitionsChest;
            if (!testMChest.IsLocked)
            {
                testMChest.IsLocked = true;
                Log("WHY ISNT THE CHEST LOCKED????????");
            }
            Chest.Spawn(testMChest, (GameManager.Instance.PrimaryPlayer.CurrentRoom.GetBestRewardLocation(IntVector2.One * 3, RoomHandler.RewardLocationStyle.PlayerCenter, true)));
            //testMChest.ConfigureOnPlacement(GameManager.Instance.PrimaryPlayer.CurrentRoom);
            
        }
        private void GetRiskStatValue(string[] args)
        {
            foreach(PlayerController player in GameManager.Instance.AllPlayers)
            {
                if(player == GameManager.Instance.PrimaryPlayer)
                {
                    Log("Player One risk value: " + player.gameObject.GetOrAddComponent<RiskStat>().RiskAMT.ToString());
                }
                else
                {
                    Log("Player Two risk value: " + player.gameObject.GetOrAddComponent<RiskStat>().RiskAMT.ToString());
                }
            }
        }
        private void ToggleEyeStrain(string[] args)
        {
            if (SaveAPIManager.GetFlag(CustomDungeonFlags.EYESTRAINDISABLE) == false)
            {
                ETGModConsole.Log("Potentially eye-straining effects turned off!");
                SaveAPIManager.SetFlag(CustomDungeonFlags.EYESTRAINDISABLE, true);
            }
            else
            {
                ETGModConsole.Log("Potentially eye-straining effects turned on!");
                SaveAPIManager.SetFlag(CustomDungeonFlags.EYESTRAINDISABLE, false);
            }
        }
        
        public static void Log(string text, string color = "FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }

        public IEnumerator DelayedStartCR()
        {
            yield return null;
            this.DelayedStart();
            yield break;
        }
        public void DelayedStart()
        {
            foreach (string s in Akffinity.moddedAks)
            {
                if (Game.Items.ContainsID(s))
                {
                    int i = PickupObjectDatabase.GetById(Game.Items.Get(s).PickupObjectId).PickupObjectId;
                    Akffinity.akIds.Add(i);
                }
            }
            Akffinity.Init();
            AddWeightsToCustomItems();
            ModdedIDStuff.SetupIDs();
            ModdedLootAdditions();
            DualGunsManager.AddDual();
            HoveringGunsAdder.AddHovers();
            SynergyFormAdder.AddForms();
            //prob do the ModdedLootAdditions shiz too once i work on chest again.
        }
        
        public void AddWeightsToCustomItems()
        {
            DragunItemLootMods();
            PrimalItemLootMods();
        }
        public void PrimalItemLootMods()
        {
            List<int> primalItems = new List<int>
            {
                PrimalCharcoal.primalCharcoalId,
                PrimalNitricAcid.primalNitricAcidId,
                PrimalSaltpeter.primalSaltpeterId,
                PrimalSulfur.primalSulfurId,
            };
            foreach (int i1 in primalItems)
            {
                if (i1 == PrimalCharcoal.primalCharcoalId)
                {
                    List<LootModData> modDatas = new List<LootModData> { };
                    foreach (int i2 in primalItems)
                    {
                        if (i2 != i1)
                        {
                            LootModData mod = new LootModData
                            {
                                AssociatedPickupId = i2,
                                DropRateMultiplier = 5,
                            };
                            modDatas.Add(mod);
                        }
                    }
                    PickupObjectDatabase.GetById(i1).associatedItemChanceMods = modDatas.ToArray();
                }
                else if (i1 == PrimalNitricAcid.primalNitricAcidId)
                {
                    List<LootModData> modDatas = new List<LootModData> { };
                    foreach (int i2 in primalItems)
                    {
                        if (i2 != i1)
                        {
                            LootModData mod = new LootModData
                            {
                                AssociatedPickupId = i2,
                                DropRateMultiplier = 5,
                            };
                            modDatas.Add(mod);
                        }
                    }
                    PickupObjectDatabase.GetById(i1).associatedItemChanceMods = modDatas.ToArray();
                }
                else if (i1 == PrimalSaltpeter.primalSaltpeterId)
                {
                    List<LootModData> modDatas = new List<LootModData> { };
                    foreach (int i2 in primalItems)
                    {
                        if (i2 != i1)
                        {
                            LootModData mod = new LootModData
                            {
                                AssociatedPickupId = i2,
                                DropRateMultiplier = 5,
                            };
                            modDatas.Add(mod);
                        }
                    }
                    PickupObjectDatabase.GetById(i1).associatedItemChanceMods = modDatas.ToArray();
                }
                else if (i1 == PrimalSulfur.primalSulfurId)
                {
                    List<LootModData> modDatas = new List<LootModData> { };
                    foreach (int i2 in primalItems)
                    {
                        if (i2 != i1)
                        {
                            LootModData mod = new LootModData
                            {
                                AssociatedPickupId = i2,
                                DropRateMultiplier = 5,
                            };
                            modDatas.Add(mod);
                        }
                    }
                    PickupObjectDatabase.GetById(i1).associatedItemChanceMods = modDatas.ToArray();
                }
            }

        }
        public void DragunItemLootMods()
        {
            List<int> dragunItems = new List<int>
            {
                DragunClaw.dragunClawID,
                DragunWing.dragunWingId,
                DragunSkull.dragunSkullId,
                DragunHeart.dragunHeartId,
            };
            foreach (int i1 in dragunItems)
            {
                if (i1 == DragunClaw.dragunClawID)
                {
                    List<LootModData> modDatas = new List<LootModData> { };
                    foreach (int i2 in dragunItems)
                    {
                        if (i2 != i1)
                        {
                            LootModData mod = new LootModData
                            {
                                AssociatedPickupId = i2,
                                DropRateMultiplier = 5,
                            };
                            modDatas.Add(mod);
                        }
                    }
                    PickupObjectDatabase.GetById(i1).associatedItemChanceMods = modDatas.ToArray();
                }
                else if (i1 == DragunWing.dragunWingId)
                {
                    List<LootModData> modDatas = new List<LootModData> { };
                    foreach (int i2 in dragunItems)
                    {
                        if (i2 != i1)
                        {
                            LootModData mod = new LootModData
                            {
                                AssociatedPickupId = i2,
                                DropRateMultiplier = 5,
                            };
                            modDatas.Add(mod);
                        }
                    }
                    PickupObjectDatabase.GetById(i1).associatedItemChanceMods = modDatas.ToArray();
                }
                else if (i1 == DragunSkull.dragunSkullId)
                {
                    List<LootModData> modDatas = new List<LootModData> { };
                    foreach (int i2 in dragunItems)
                    {
                        if (i2 != i1)
                        {
                            LootModData mod = new LootModData
                            {
                                AssociatedPickupId = i2,
                                DropRateMultiplier = 5,
                            };
                            modDatas.Add(mod);
                        }
                    }
                    PickupObjectDatabase.GetById(i1).associatedItemChanceMods = modDatas.ToArray();
                }
                else if (i1 == DragunHeart.dragunHeartId)
                {
                    List<LootModData> modDatas = new List<LootModData> { };
                    foreach (int i2 in dragunItems)
                    {
                        if (i2 != i1)
                        {
                            LootModData mod = new LootModData
                            {
                                AssociatedPickupId = i2,
                                DropRateMultiplier = 5,
                            };
                            modDatas.Add(mod);
                        }
                    }
                    PickupObjectDatabase.GetById(i1).associatedItemChanceMods = modDatas.ToArray();
                }
            }

        }
        
        public static void ModdedLootAdditions()
        {
            try
            {
                if (moddedMunitionsIDs.Any())
                {
                    foreach (int i in moddedMunitionsIDs)
                    {                  
                        MunitionsChestController.munitionsChest.lootTable.lootTable.AddItemToPool(i);                  
                    }
                }
            }
            catch(Exception e)
            {
                ETGModConsole.Log(e.ToString());
                
            }
        }
        public static List<int> moddedMunitionsIDs = new List<int>(){ };

        string credits = "Written by @UnstableStrafe#3928 with help from KyleTheScientist, Neighborino, Glorfindel, Retrash, Retromation, TheTurtleMelon, TankTheta, Spapi, Eternal Frost, Some Bunny, NotABot, BlazeyKat, ExplosivePanda, Nevernamed, Spcreate, Bleak Bubbles, Skilotar_, An3s, my dog Dolly <3, my gf Alice";
      
        //SPINEL TONIC BY TankTheta!
        //Bismuth Ammolet sprite by TankTheta!
        //Spirit spride by TankTheta!
        //Memetic Kill Agent sprite by NN
        //Thanks to ExplosivePanda for helping with the code for Soul Forge

        /*
                                                               ,ggggg$@@@@@@@@@@@@@@@@@F                                  
                                                   g@@@@@$$$$$$$$$$$$$$$$$$$$@@*'                       yg y@L     
                                            ,,g@@@@@$$$$$$$$@@$$$$$$$$$$$@@$@@@,           ,,g@@@@@@@@@@@@@@,,,    
                                        ,gg@@@$$$$$$$$$$$$$$@@@$$$$$$$$@@@$$$$$$@gggggL  g@@@@@@@@@@@@@@@@@@@@@g   
                                    ,,,$@$$$$$$$$$$$$$$$$$$$$$@@@@$$$@@@$&$$$$$$$$$$$@F ,$@@@@@@@@@@@@@@@@@@@@@@L, 
                                 ggg@@@$$$$$$$$$$$$$$$$@@@@@@@@@@@@$@@@@@gg@@@@@@@MMF  |$@@@@@@@@@@@@@@@@@@@@@@@@@ 
                               j@@$$$$$$$$$$$$$$$$$$$@M|||||$@$$@$$@$||||l%@$%M"''    #@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                               ]$@$$$$$$$@@@@@$$$$$$@LlTTTTTTl$lT|||lTTTTTT%%gg       $@@@@@@@$$%@@j$@@@@@@@@@@@@@@
                             ,@@@@@@$$$$$$@@@$$$$$@@*'||,,|||||||||||||||||||'$@@@,   $@@@@@$$l$&Tl$$&MM%@@@@@@@@@@
                     ,g%%%%@@B@gg$@@$$$$$$$$@@@@@@@@||;g@@ggggggggggL||lT|||l|| `j$F  $@@@@@@T|`````````'  |j$@@@@@
                     $@Ll@|j$@@@$$$$$$$$$$$$$$$$$$@@@@$$$$MMMMMMMMM$@@@@@@@@@g,,,$@F  $@@@@@@@L,|@@@@@@L ,,$@@@@@@@
                     %$gg@@@@@@$$$$$$$$$$$$$$$$@@@@%%%%@@@gggggggggg$$$$$$$$@@MMMM`   j%@@@@@@@@gggl|`|ggg@@@@@@@@@
                      ]@@$$$$$$$$$$$$$$$$$$$$@@MM*T||||l*$@@@$$@@@$$$@@@@****'         |$Mj$@@@@@M$@@@@@@@@@@@@j%@*
                       '%B@@$$$$$$$$$$$$$$$@@M"'|||||||||l$@@@@@@@@@$@BL`               ` |$@@@$llllll$$$@L'l%@,`  
                        g@@$$$$$@@@@@@M$@@@ML|||||||gg@@@@@@@@@@@@$@@@                   g@$$$$$lll$$$@@@@@@L *}gg 
                        '}%@@@@@M|"|%@@@@@@@g,g@@@@@@@@@@@@@@$$$$@@@@@                  ,$@$ll$g$@@@@@TTT%%@@@@@L` 
                           *%$@-|||||$@@@@@%@@@$$@@$@@@@@@@@@@@@@@@@@@g,               |$@$@@@@@@@MllTTTTj$@@$$@L  
                             "%@L,|g@@@$j%@$@@$$$@@$@@@@@@@@@@@@@@@@@@@L                '$@T|j$@@L,    ,,|$@@F|j@@ 
                               "%MM%L  $@@@@@$$@@@@@%@@@@@@@@@@@@@@@@@@@@g               $@@$$@@@@@gggg@@@@@@@$$$@ 
                                     ;@@$@$j@@@M$@@@$@@@@@@@@@@@@@@@@@@@@@@@,          |@@$ML'"$@@@@@@@@@@@$"'l&$$@
                                    $@g@@@@g|||g@@@@@@$%%@@@@@@@@@@@@@@@@@@@@@@g,      |%$gL,|l$@@@@@@@@@@@@lL,|g@M
                                  ;@@$$$$$$$@@@@$$$$$$@@@@M$$$$$$@@@@@@@@@@@@@@@@@       "j@@@@@@@@@F$@@@@@@@@@@F' 
                                ,@@$$$$$$$$$$$$$@@@$$$$$$$@@@@@@@$$$$@MMMMMMML```           ```]@@@@Lj@@@@@@````   
                              g@@@$$$$$$$$$$$$$@@@@@$$$$$$$$$$$$$$$@@@                       .g@**$@@@@M***$g      
               @@@@@@L,,,g@@@@@@@@@@@@@@@@@NNNM'```j%$@$$$$$$$@@@@@@@@@@@,                   ]$@, l$$@TL,,,$@L     
               MMMM%@@@@@@@@@@@@@@@@@@MMM*'          *$@@@@@@@@@@@@@@@@@@@@g                yg$MlLl$$@@g|lTj$gg    
                   "TTTl%@@@@@@NNNM'''                '''''''''l$$$@@@@@@@@@                #NN@@@NNBM&&@@@@NBM    
                          ''*M'                            ;g@@@MMMMMMMMM'''                                       
                                                             ---         
           _____  ____  _____    ______ _    _  _____ _  _______ _   _  _____       _                           _ _     _  _______  _____  _____            _                   
          / ____|/ __ \|  __ \  |  ____| |  | |/ ____| |/ /_   _| \ | |/ ____|     | |                         (_) |   | |/ /  __ \|_   _|/ ____|          | |                  
         | |  __| |  | | |  | | | |__  | |  | | |    | ' /  | | |  \| | |  __    __| | __ _ _ __ ___  _ __ ___  _| |_  | ' /| |__) | | | | (___   __      _| |__   ___ _ __ ___ 
         | | |_ | |  | | |  | | |  __| | |  | | |    |  <   | | | . ` | | |_ |  / _` |/ _` | '_ ` _ \| '_ ` _ \| | __| |  < |  _  /  | |  \___ \  \ \ /\ / / '_ \ / _ \ '__/ _ \
         | |__| | |__| | |__| | | |    | |__| | |____| . \ _| |_| |\  | |__| | | (_| | (_| | | | | | | | | | | | | |_  | . \| | \ \ _| |_ ____) |  \ V  V /| | | |  __/ | |  __/
          \_____|\____/|_____/  |_|     \____/ \_____|_|\_\_____|_| \_|\_____|  \__,_|\__,_|_| |_| |_|_| |_| |_|_|\__| |_|\_\_|  \_\_____|_____/    \_/\_/ |_| |_|\___|_|  \___|
          _   _            ______ _    _  _____ _  __                                _ ___                                                                                      
         | | | |          |  ____| |  | |/ ____| |/ /                               | |__ \                                                                                     
         | |_| |__   ___  | |__  | |  | | |    | ' /    __ _ _ __ ___  __      _____| |  ) |                                                                                    
         | __| '_ \ / _ \ |  __| | |  | | |    |  <    / _` | '__/ _ \ \ \ /\ / / _ \ | / /                                                                                     
         | |_| | | |  __/ | |    | |__| | |____| . \  | (_| | | |  __/  \ V  V /  __/_||_|                                                                                      
          \__|_| |_|\___| |_|     \____/ \_____|_|\_\  \__,_|_|  \___|   \_/\_/ \___(_)(_)                                                                                      
                                                                                                                                                                        
         */

    }
}

