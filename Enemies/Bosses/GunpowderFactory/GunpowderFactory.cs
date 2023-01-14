using System;
using System.Collections.Generic;
using Gungeon;
using UnityEngine;
using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = Alexandria.EnemyAPI.EnemyBuilder.AnimationType;
using System.Collections;
using Dungeonator;
using System.Linq;
using Brave.BulletScript;
using Alexandria.DungeonAPI;
using static DirectionalAnimation;
using Alexandria.EnemyAPI;
using Alexandria;
using AlexandriaLib;
using Alexandria.ItemAPI;
using Alexandria.NPCAPI;

namespace Items
{
    class GunpowderFactory : AIActor
    {
        public static GameObject prefab;
        public static readonly string guid = "gunpowder_foreman";
        public static tk2dSpriteCollectionData ForemanCollection;

        private static readonly string basePath = "Items/Resources/Enemies/Bosses/GunpowderFactory/Foreman/foreman_";

        private static Texture2D BossCardTexture = ResourceExtractor.GetTextureFromResource("Items/Resources/Enemies/Bosscards/gunpowder_factory_bosscard.png");

        //need a bosscard

        private static string[] spritePaths = new string[]
        {
            //Boulder Summon Attack 
            "big_ass_button_001.png", //0
            "big_ass_button_002.png",
            "big_ass_button_003.png",
            "big_ass_button_004.png",
            "big_ass_button_005.png",
            "big_ass_button_006.png",
            "big_ass_button_007.png",
            "big_ass_button_008.png", //7
            //Death
            "death_001.png", //8
            "death_002.png",
            "death_003.png",
            "death_004.png",
            "death_005.png",
            "death_006.png",
            "death_007.png",
            "death_009.png", 
            "death_010.png", //16
            "death_011.png",
            "death_012.png",
            "death_013.png",
            "death_014.png",
            "death_015.png",
            "death_016.png",
            "death_017.png",
            "death_018.png",
            "death_019.png",
            "death_020.png", //26
            "death_021.png",
            "death_022.png",
            "death_023.png",
            "death_024.png",
            "death_025.png",
            "death_026.png",
            "death_027.png",
            "death_028.png",
            "death_029.png",
            "death_030.png", //36
            "death_031.png",
            "death_032.png",
            "death_033.png",
            "death_034.png",
            "death_035.png",
            "death_036.png",
            "death_037.png",
            "death_038.png",
            "death_039.png", //45
            //Idle
            "idle_001.png", //46
            "idle_002.png",
            "idle_003.png",
            "idle_004.png",
            "idle_005.png", //50
            //Intro
            "intro_001.png", //51
            "intro_002.png",
            "intro_003.png",
            "intro_004.png",
            "intro_005.png",
            "intro_006.png",
            "intro_007.png",
            "intro_008.png",
            "intro_009.png",
            "intro_010.png", //60
            "intro_011.png",
            "intro_012.png",
            "intro_013.png",
            "intro_014.png",
            "intro_015.png",
            "intro_016.png",
            "intro_017.png", //67
            //Bullet Barrels
            "lever_001.png", //68
            "lever_002.png", //69
            "lever_003.png",
            "lever_004.png",
            "lever_005.png",
            "lever_006.png",
            "lever_007.png",
            "lever_008.png", //75
            //Poison Grates
            "panel_001.png", //76
            "panel_002.png",
            "panel_003.png",
            "panel_004.png",
            "panel_005.png",
            "panel_006.png", //81

        };
        public static void Init()
        {
            if(prefab == null && !EnemyBuilder.Dictionary.ContainsKey(guid))
            {
                string[] array = Library.JoinAtStart(basePath, spritePaths);
                prefab = EnemyBuilder.BuildPrefab("Gunpowder Factory", guid, basePath + "idle_001", new IntVector2(0, 0), new IntVector2(0, 0), false);
                var enemy = prefab.AddComponent<EnemyBehavior>();
                AIActor actor = enemy.aiActor;
                AIAnimator aiAnimator = enemy.aiAnimator;
                HandleWaveBehavior waveBehavior = prefab.AddComponent<HandleWaveBehavior>();
                waveBehavior.factoryForeman = actor;
                actor.MovementSpeed = 0;
                actor.IgnoreForRoomClear = false;
                actor.aiAnimator.HitReactChance = 0f;
                actor.specRigidbody.CollideWithOthers = true;
                actor.specRigidbody.CollideWithTileMap = true;
                actor.knockbackDoer.SetImmobile(false, "fat fuck");
                actor.HasShadow = false;
                actor.CollisionDamage = 0;
                actor.healthHaver.ForceSetCurrentHealth(1000);
                actor.healthHaver.SetHealthMaximum(1000);
                actor.CanTargetPlayers = true;
                actor.PreventFallingInPitsEver = true;
                actor.healthHaver.PreventAllDamage = true;

                aiAnimator.IdleAnimation = new DirectionalAnimation
                {
                    Type = DirectionalAnimation.DirectionType.Single,
                    Prefix = "idle",
                    AnimNames = new string[1],
                    Flipped = new DirectionalAnimation.FlipType[1]
                };

                //==== Big Ass Button

                DirectionalAnimation slap_that_button_daddyo = new DirectionalAnimation
                {
                    Type = DirectionalAnimation.DirectionType.Single,
                    AnimNames = new string[]
                    {
                        "big_ass_button",

                    },
                    Flipped = new DirectionalAnimation.FlipType[1]
                };

                aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
                {
                    new AIAnimator.NamedDirectionalAnimation
                    {
                        name = "big_ass_button",
                        anim = slap_that_button_daddyo
                    }
                };

                //==== Panel

                DirectionalAnimation bop_it = new DirectionalAnimation
                {
                    Type = DirectionalAnimation.DirectionType.Single,
                    AnimNames = new string[]
                    {
                        "panel",

                    },
                    Flipped = new DirectionalAnimation.FlipType[1]
                };

                aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
                {
                    new AIAnimator.NamedDirectionalAnimation
                    {
                        name = "panel",
                        anim = bop_it
                    }
                };
                //==== Lever

                DirectionalAnimation pull_it = new DirectionalAnimation
                {
                    Type = DirectionalAnimation.DirectionType.Single,
                    AnimNames = new string[]
                    {
                        "lever",

                    },
                    Flipped = new DirectionalAnimation.FlipType[1]
                };

                aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
                {
                    new AIAnimator.NamedDirectionalAnimation
                    {
                        name = "lever",
                        anim = pull_it
                    }
                };

                //==== Intro

                DirectionalAnimation wake_me_up = new DirectionalAnimation
                {
                    Type = DirectionalAnimation.DirectionType.Single,
                    Prefix = "intro",
                    AnimNames = new string[1],
                    Flipped = new DirectionalAnimation.FlipType[1]
                };
                aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
                {
                    new AIAnimator.NamedDirectionalAnimation
                    {
                        name = "intro",
                        anim = wake_me_up
                    }
                };

                //==== DEATH
                DirectionalAnimation darkness_take_me = new DirectionalAnimation
                {
                    Type = DirectionalAnimation.DirectionType.Single,
                    Prefix = "death",
                    AnimNames = new string[1],
                    Flipped = new DirectionalAnimation.FlipType[1]
                };
                aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
                {
                    new AIAnimator.NamedDirectionalAnimation
                    {
                        name = "death",
                        anim = darkness_take_me
                    }
                };

                if(ForemanCollection == null)
                {
                    ForemanCollection = SpriteBuilder.ConstructCollection(prefab, "GunpowderForemanCollection");
                    UnityEngine.Object.DontDestroyOnLoad(ForemanCollection);
                    for (int i = 0; i < array.Length; i++)
                    {
                        SpriteBuilder.AddSpriteToCollection(array[i], ForemanCollection);
                    }
                    SpriteBuilder.AddAnimation(enemy.spriteAnimator, ForemanCollection, new List<int>
                    {

                        0,
                        1,
                        2,
                        3,
                        4,
                        5,
                        6,
                        7

                    }, "big_ass_button", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;
                    SpriteBuilder.AddAnimation(enemy.spriteAnimator, ForemanCollection, new List<int>
                    {
                        8,
                        9,
                        10,
                        11,
                        12,
                        13,
                        14,
                        15,
                        16,
                        17,
                        18,
                        19,
                        20,
                        21,
                        22,
                        23,
                        24,
                        25,
                        26,
                        27,
                        28,
                        29,
                        30,
                        31,
                        32,
                        33,
                        34,
                        35,
                        36,
                        37,
                        38,
                        39,
                        40,
                        41,
                        42,
                        43,
                        44,
                        45,

                    }, "death", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;
                    SpriteBuilder.AddAnimation(enemy.spriteAnimator, ForemanCollection, new List<int>
                    {

                        46,
                        47,
                        48,
                        49,
                        50,

                    }, "idle", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 8f;
                    SpriteBuilder.AddAnimation(enemy.spriteAnimator, ForemanCollection, new List<int>
                    {

                        51,
                        52,
                        53,
                        54,
                        55,
                        56,
                        57,
                        58,
                        59,
                        60,
                        61,
                        62,
                        63,
                        64,
                        65,
                        66,
                        67,

                    }, "intro", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;
                    SpriteBuilder.AddAnimation(enemy.spriteAnimator, ForemanCollection, new List<int>
                    {

                        68,
                        69,
                        70,
                        71,
                        72,
                        73,
                        74,
                        75,

                    }, "lever", tk2dSpriteAnimationClip.WrapMode.Once).fps = 6f;
                    SpriteBuilder.AddAnimation(enemy.spriteAnimator, ForemanCollection, new List<int>
                    {

                        76,
                        77,
                        78,
                        79,
                        80,
                        81

                    }, "panel", tk2dSpriteAnimationClip.WrapMode.Once).fps = 6f;

                }
                actor.specRigidbody.PixelColliders.Clear();
                enemy.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
                {
                    ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                    CollisionLayer = CollisionLayer.EnemyCollider,
                    IsTrigger = false,
                    BagleUseFirstFrameOnly = false,
                    SpecifyBagelFrame = string.Empty,
                    BagelColliderNumber = 0,
                    ManualOffsetX = 0,
                    ManualOffsetY = 21,
                    ManualWidth = 92,
                    ManualHeight = 70,
                    ManualDiameter = 0,
                    ManualLeftX = 0,
                    ManualLeftY = 0,
                    ManualRightX = 0,
                    ManualRightY = 0
                });

                enemy.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
                {

                    ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                    CollisionLayer = CollisionLayer.EnemyHitBox,
                    IsTrigger = false,
                    BagleUseFirstFrameOnly = false,
                    SpecifyBagelFrame = string.Empty,
                    BagelColliderNumber = 0,
                    ManualOffsetX = 0,
                    ManualOffsetY = 21,
                    ManualWidth = 92,
                    ManualHeight = 70,
                    ManualDiameter = 0,
                    ManualLeftX = 0,
                    ManualLeftY = 0,
                    ManualRightX = 0,
                    ManualRightY = 0,
                });

                enemy.aiActor.PreventBlackPhantom = false;
                var bs = prefab.GetComponent<BehaviorSpeculator>();
                BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").behaviorSpeculator;

                bs.OverrideBehaviors = behaviorSpeculator.OverrideBehaviors;
                bs.OtherBehaviors = behaviorSpeculator.OtherBehaviors;
                bs.TargetBehaviors = new List<TargetBehaviorBase>
                {
                    new TargetPlayerBehavior
                    {
                        Radius = 45f,
                        LineOfSight = false,
                        ObjectPermanence = true,
                        SearchInterval = 0.25f,
                        PauseOnTargetSwitch = false,
                        PauseTime = 0.25f
                    },

                };

                bs.AttackBehaviorGroup.AttackBehaviors = new List<AttackBehaviorGroup.AttackGroupItem>
                {
                    new AttackBehaviorGroup.AttackGroupItem
                    {
                        Probability = 1,
                        NickName = "Poison Grates",
                        Behavior = new PoisonGrateBehavior()
                        {
                            InitialCooldown = 8,
                            Cooldown = 15f,
                            CooldownVariance = 1.5f,
                            AttackCooldown = 7f,
                            AttackAnimation = "panel"
                        }
                    },
                    new AttackBehaviorGroup.AttackGroupItem
                    {
                        Probability = 1,
                        NickName = "Barrel Spawners",
                        Behavior = new SummonRollingBarrelsBehavior()
                        {
                            InitialCooldown = 8,
                            Cooldown = 15f,
                            CooldownVariance = 1.5f,
                            AttackCooldown = 7f,
                            AttackAnimation = "lever",
                            MinDelayBetweenBarrels = .2f,
                            MaxDelayBetweenBarrels = .4f,
                            
                        }
                    }
                };

                bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
                bs.TickInterval = behaviorSpeculator.TickInterval;
                bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
                bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
                bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
                bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
                bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
                Game.Enemies.Add("ck:gunpowder_foreman", enemy.aiActor);

                if (enemy.GetComponent<EncounterTrackable>() != null)
                {
                    UnityEngine.Object.Destroy(enemy.GetComponent<EncounterTrackable>());
                }
                GenericIntroDoer miniBossIntroDoer = prefab.AddComponent<GenericIntroDoer>();
                prefab.AddComponent<GunpowderFactoryIntroController>();
                miniBossIntroDoer.triggerType = GenericIntroDoer.TriggerType.PlayerEnteredRoom;
                miniBossIntroDoer.initialDelay = 0.15f;
                miniBossIntroDoer.cameraMoveSpeed = 14;
                miniBossIntroDoer.specifyIntroAiAnimator = null;
                miniBossIntroDoer.BossMusicEvent = "Play_MUS_Boss_Theme_Beholster";
                miniBossIntroDoer.PreventBossMusic = false;
                miniBossIntroDoer.InvisibleBeforeIntroAnim = false;
                miniBossIntroDoer.preIntroAnim = string.Empty;
                miniBossIntroDoer.preIntroDirectionalAnim = string.Empty;
                miniBossIntroDoer.introAnim = "intro";
                miniBossIntroDoer.introDirectionalAnim = string.Empty;
                miniBossIntroDoer.continueAnimDuringOutro = false;
                miniBossIntroDoer.cameraFocus = null;
                miniBossIntroDoer.roomPositionCameraFocus = Vector2.zero;
                miniBossIntroDoer.restrictPlayerMotionToRoom = false;
                miniBossIntroDoer.fusebombLock = false;
                miniBossIntroDoer.AdditionalHeightOffset = 0;
                ChildrenOfKaliberModule.Strings.Enemies.Set("#GUNPOWDERFACTORY_NAME", "GUNPOWDER FACTORY");
                ChildrenOfKaliberModule.Strings.Enemies.Set("#GUNPOWDERFACTORY_NAME_SMALL", "Gunpowder Factory");

                ChildrenOfKaliberModule.Strings.Enemies.Set("MEANS_OF_PRODUCTION", "THE MEANS OF PRODUCTION");
                ChildrenOfKaliberModule.Strings.Enemies.Set("#QUOTE", "");
                enemy.aiActor.OverrideDisplayName = "#GUNPOWDERFACTORY_NAME_SMALL";
                miniBossIntroDoer.portraitSlideSettings = new PortraitSlideSettings()
                {
                    bossNameString = "#GUNPOWDERFACTORY_NAME",
                    bossSubtitleString = "MEANS_OF_PRODUCTION",
                    bossQuoteString = "#QUOTE",
                    bossSpritePxOffset = IntVector2.Zero,
                    topLeftTextPxOffset = IntVector2.Zero,
                    bottomRightTextPxOffset = IntVector2.Zero,
                    bgColor = Color.blue
                };
                if (BossCardTexture)
                {
                    miniBossIntroDoer.portraitSlideSettings.bossArtSprite = BossCardTexture;
                    miniBossIntroDoer.SkipBossCard = false;
                    enemy.aiActor.healthHaver.bossHealthBar = HealthHaver.BossBarType.MainBar;
                }
                else
                {
                    miniBossIntroDoer.SkipBossCard = true;
                    enemy.aiActor.healthHaver.bossHealthBar = HealthHaver.BossBarType.MainBar;
                }

                SpriteBuilder.AddSpriteToCollection("Items/Resources/Enemies/Ammonomicon/gunpowder_factory_boss_icon", SpriteBuilder.ammonomiconCollection);
                if (enemy.GetComponent<EncounterTrackable>() != null)
                {
                    UnityEngine.Object.Destroy(enemy.GetComponent<EncounterTrackable>());
                }
                enemy.encounterTrackable = enemy.gameObject.AddComponent<EncounterTrackable>();
                enemy.encounterTrackable.journalData = new JournalEntry();
                enemy.encounterTrackable.EncounterGuid = "ck:gunpowder_foreman";
                enemy.encounterTrackable.prerequisites = new DungeonPrerequisite[0];
                enemy.encounterTrackable.journalData.SuppressKnownState = false;
                enemy.encounterTrackable.journalData.IsEnemy = true;
                enemy.encounterTrackable.journalData.SuppressInAmmonomicon = false;
                enemy.encounterTrackable.ProxyEncounterGuid = "";
                enemy.encounterTrackable.journalData.AmmonomiconSprite = "Items/Resources/Enemies/Ammonomicon/gunpowder_factory_boss_icon";
                enemy.encounterTrackable.journalData.enemyPortraitSprite = ResourceExtractor.GetTextureFromResource("Items/Resources/Enemies/Ammonomicon/gunpowder_factory_ammonomicon.png");
                ChildrenOfKaliberModule.Strings.Enemies.Set("#GUNPOWDERFACTORYAMMONOMICON", "Gunpowder Factory");
                ChildrenOfKaliberModule.Strings.Enemies.Set("#GUNPOWDERFACTORYAMMONOMICONSHORT", "The Means Of Production");
                ChildrenOfKaliberModule.Strings.Enemies.Set("#GUNPOWDERFACTORYAMMONOMICONLONG", "One of the largest of the Gundead's many mining operations in the Blackpowder Mines, it produces about 45% of all gunpowder used in the Gungeon, lead by the ill-tempered Foreman. Sabotaging the Factory's production would throw a wrench into Kaliber's plans. While the Foreman himself is nearly defenseless, each of the workers at the factory are ready to dispatch any intruders.");
                enemy.encounterTrackable.journalData.PrimaryDisplayName = "#GUNPOWDERFACTORYAMMONOMICON";
                enemy.encounterTrackable.journalData.NotificationPanelDescription = "#GUNPOWDERFACTORYAMMONOMICONSHORT";
                enemy.encounterTrackable.journalData.AmmonomiconFullEntry = "#GUNPOWDERFACTORYAMMONOMICONLONG";
                EnemyBuilder.AddEnemyToDatabase(enemy.gameObject, "ck:gunpowder_foreman");
                EnemyDatabase.GetEntry("ck:gunpowder_foreman").ForcedPositionInAmmonomicon = 8;
                EnemyDatabase.GetEntry("ck:gunpowder_foreman").isInBossTab = true;
                EnemyDatabase.GetEntry("ck:gunpowder_foreman").isNormalEnemy = true;

                miniBossIntroDoer.SkipFinalizeAnimation = true;
                miniBossIntroDoer.RegenerateCache();

                //==================
                //Important for not breaking basegame stuff!
                StaticReferenceManager.AllHealthHavers.Remove(enemy.aiActor.healthHaver);
                //==================
            }
        }

        

        class HandleWaveBehavior : BraveBehaviour
        {
            List<EnemyGroup> possibleStaff = new List<EnemyGroup> { };
            List<AIActor> actorsOnShift = new List<AIActor> { };

            RoomHandler parentRoom;
            List<AIActor> activeEnemies = new List<AIActor> { };
            public AIActor factoryForeman;
            private void Start()
            {
                if (factoryForeman)
                {
                    parentRoom = factoryForeman.GetAbsoluteParentRoom();
                }
                else
                {
                    ETGModConsole.Log("THE FORE MAN ISNT SET YOU DOINKUS");
                    Destroy(this);
                }
                possibleStaff.Add(staffGroup1);
                foreach(EnemyGroup g in possibleStaff)
                {
                    if (g.actorGUIDs.Any())
                    {
                        foreach(string s in g.actorGUIDs)
                        {
                            AIActor actor = EnemyDatabase.GetOrLoadByGuid(s);
                            if (actor)
                            {
                                g.actorsInGroup.Add(actor);
                            }
                        }
                    }
                }
                PickShift();
            }

            private void PickShift()
            {
                int i = UnityEngine.Random.Range(0, possibleStaff.Count);
                actorsOnShift = possibleStaff[i].actorsInGroup;
            }

            private void Update()
            {
                List<AIActor> activeA = parentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                if (activeA == activeEnemies) return;
                if (factoryForeman.aiAnimator.IsPlaying("intro")) return;
                if (GameManager.Instance.PrimaryPlayer.CurrentRoom != parentRoom) return;
                if (activeA.Count == 1)
                {
                    if (actorsOnShift.Any())
                    {
                        SpawnEnemies();
                    }
                    else if (!actorsOnShift.Any())
                    {
                        factoryForeman.healthHaver.Die(Vector2.zero);
                    }
                }

                activeEnemies = activeA;
            }

            private void SpawnEnemies()
            {
                //int i = Mathf.Clamp(UnityEngine.Random.Range(1, 2), 1, actorsOnShift.Count);

                for (int l = 0; l < GameManager.Instance.AllPlayers.Length; l++)
                {
                    if (!GameManager.Instance.AllPlayers[l].IsGhost)
                    {
                        GameManager.Instance.AllPlayers[l].healthHaver.TriggerInvulnerabilityPeriod(1f);
                        GameManager.Instance.AllPlayers[l].knockbackDoer.TriggerTemporaryKnockbackInvulnerability(1f);
                    }
                }

                int i = 1;
                List<AIActor> actorsToSpawn = Library.RandomRemoveChosen(actorsOnShift, i);
                foreach(AIActor actor in actorsToSpawn)
                {
                    IntVector2? spot = parentRoom.GetRandomAvailableCell(new IntVector2?(actor.Clearance), new CellTypes?(actor.PathableTiles), false);
                    AIActor.Spawn(actor, spot.Value, parentRoom, true, AwakenAnimationType.Default, true);
                }
            }


            EnemyGroup staffGroup1 = new EnemyGroup 
            {
                //5 bullet kin
                actorGUIDs = new List<string> 
                {
                    "01972dee89fc4404a5c408d50007dad5", //bullet kin
                    "01972dee89fc4404a5c408d50007dad5", //bullet kin
                    "01972dee89fc4404a5c408d50007dad5", //bullet kin
                    "01972dee89fc4404a5c408d50007dad5", //bullet kin
                    "01972dee89fc4404a5c408d50007dad5", //bullet kin
                },
                actorsInGroup = new List<AIActor> { }
            };
            EnemyGroup staffGroup2 = new EnemyGroup
            {
                
                actorGUIDs = new List<string> 
                {

                },
                actorsInGroup = new List<AIActor> { }
            };
            EnemyGroup staffGroup3 = new EnemyGroup
            {
                actorGUIDs = new List<string> 
                {
                
                },
                actorsInGroup = new List<AIActor> { }
            };
            EnemyGroup staffGroup4 = new EnemyGroup
            {
                actorGUIDs = new List<string> 
                {
                
                },
                actorsInGroup = new List<AIActor> { }
            };
            EnemyGroup staffGroup5 = new EnemyGroup
            {
                actorGUIDs = new List<string> 
                {
                
                },
                actorsInGroup = new List<AIActor> { }
            };
        }

        class EnemyGroup
        {
            public List<string> actorGUIDs;
            public List<AIActor> actorsInGroup;
        }

        class EnemyBehavior : BraveBehaviour
        {
            //This determines that the enemy is active when a player is in the room
            private RoomHandler m_StartRoom;
            private void Update()
            {
                if (!base.aiActor.HasBeenEngaged) { CheckPlayerRoom(); }
            }
            private void CheckPlayerRoom()
            {
                if (GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() != null && GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() == m_StartRoom) { base.aiActor.HasBeenEngaged = true; }
            }
            private void Start()
            {
                m_StartRoom = aiActor.GetAbsoluteParentRoom();
                //This line determines what happens when an enemy dies. For now it's something simple like playing a death sound.
                //A full list of all the sounds can be found in the SFX.txt document that comes with this github.
                //base.aiActor.healthHaver.OnPreDeath += (obj) => { AkSoundEngine.PostEvent("Play_VO_kali_death_01", base.aiActor.gameObject); };
            }
        }

        class PoisonGrateBehavior : BasicAttackBehavior
        {
            public override void Start()
            {
                base.Start();
                if (Actor.aiAnimator)
                {
                    Animator = Actor.aiAnimator;
                }
            }

            public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
            {
                base.Init(gameObject, aiActor, aiShooter);
                Actor = aiActor;
            }

            public override BehaviorResult Update()
            {
                base.Update();
                BehaviorResult behaviorResult = base.Update();
                if (behaviorResult != BehaviorResult.Continue)
                {
                    return behaviorResult;
                }
                if (!this.IsReady())
                {
                    return BehaviorResult.Continue;
                }
                SpawnGoops();
                this.UpdateCooldowns();
                return BehaviorResult.SkipRemainingClassBehaviors; //if the behav doesnt work, mess with this ig idfk
            }
            private void SpawnGoops()
            {
                if (!string.IsNullOrEmpty(AttackAnimation))
                {
                    Animator.EndAnimation();
                    Animator.PlayUntilFinished(AttackAnimation, false, null, -1f, false);
                }                
                RoomHandler room = Actor.GetAbsoluteParentRoom();
                List<SewerGratePoisonController> grates = room.GetComponentsAbsoluteInRoom<SewerGratePoisonController>();
                List<SewerGratePoisonController> targets = new List<SewerGratePoisonController> { };
                float f = UnityEngine.Random.value;
                if(f <= .35f)
                {
                    targets = Library.RandomNoRepeats(grates, 3);
                }
                else if(f >= .36f)
                {
                    targets = Library.RandomNoRepeats(grates, 2);
                }
                if (targets.Any())
                {
                    foreach (SewerGratePoisonController sewer in targets)
                    {
                        sewer.DoPoison();
                    }
                }
                
                
            }
            
            public string AttackAnimation;
            public AIActor Actor;
            private AIAnimator Animator;
        }
        class BouldersSummonBehavior : BasicAttackBehavior
        {

        }
        class SummonRollingBarrelsBehavior : BasicAttackBehavior
        {
            public override void Start()
            {
                base.Start();
                if (Actor.aiAnimator)
                {
                    Animator = Actor.aiAnimator;
                }
            }

            public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
            {
                base.Init(gameObject, aiActor, aiShooter);
                Actor = aiActor;
            }

            public override BehaviorResult Update()
            {
                base.Update();
                BehaviorResult behaviorResult = base.Update();
                if (behaviorResult != BehaviorResult.Continue)
                {
                    return behaviorResult;
                }
                if (!this.IsReady())
                {
                    return BehaviorResult.Continue;
                }
                GameManager.Instance.StartCoroutine(SpawnBarrels());
                this.UpdateCooldowns();
                return BehaviorResult.SkipRemainingClassBehaviors; //if the behav doesnt work, mess with this ig idfk
            }
            private IEnumerator SpawnBarrels()
            {
                if (!string.IsNullOrEmpty(AttackAnimation))
                {
                    Animator.EndAnimation();
                    Animator.PlayUntilFinished(AttackAnimation, false, null, -1f, false);
                }
                RoomHandler room = Actor.GetAbsoluteParentRoom();
                List<BarrelSpawnerController> spawners = room.GetComponentsAbsoluteInRoom<BarrelSpawnerController>();
                List<BarrelSpawnerController> targets = new List<BarrelSpawnerController> { };
                float f = UnityEngine.Random.value;
                if(f <= .33f)
                {
                    targets = Library.RandomNoRepeats(spawners, 6);
                    targets.Shuffle();
                }
                else
                {
                    targets = Library.RandomNoRepeats(spawners, 4);
                    targets.Shuffle();
                }
                if(MinDelayBetweenBarrels == -3) { MinDelayBetweenBarrels = .2f; }
                if(MaxDelayBetweenBarrels == -3) { MaxDelayBetweenBarrels = .5f; }
                foreach(BarrelSpawnerController spawner in targets)
                {
                    spawner.gameObject.GetComponent<tk2dSpriteAnimator>().Play("open");
                    while (spawner.gameObject.GetComponent<tk2dSpriteAnimator>().IsPlaying("open"))
                    {
                        yield return null;
                    }
                    float t = UnityEngine.Random.Range(MinDelayBetweenBarrels, MaxDelayBetweenBarrels);
                    while(t > 0)
                    {
                        t -= Time.deltaTime;
                    }
                    spawner.SpawnBarrel();
                    spawner.gameObject.GetComponent<tk2dSpriteAnimator>().Play("idle");
                }

                yield break;
            }


            public float MaxDelayBetweenBarrels = -3;
            public float MinDelayBetweenBarrels = -3;
            public string AttackAnimation;
            public AIActor Actor;
            private AIAnimator Animator;
        }
        
    }
    class GunpodwerFactorySewerGrate
    {
        public static GameObject gratePrefab;

        public static void Init()
        {
            GameObject obj = Alexandria.PrefabAPI.PrefabBuilder.BuildObject("GunpowderFactorySewerGrate");
            SpriteBuilder.SpriteFromResource("Items/Resources/Enemies/Bosses/GunpowderFactory/SewerGrate/gunpowder_factory_sewergrate", obj);
            obj.AddComponent<SewerGratePoisonController>();
            gratePrefab = obj;
            tk2dSprite sprite = obj.GetComponent<tk2dSprite>();
            obj.layer = 20;
            sprite.SortingOrder = 2;
            sprite.HeightOffGround = 0;
            StaticReferences.StoredRoomObjects.Add("GunpowderFactorySewerGrate", gratePrefab);
        }
    }    
    public class SewerGratePoisonController : MonoBehaviour
    {
        private Vector2 middlePos;

        private void Start()
        {
            middlePos = base.gameObject.transform.position;
            middlePos.y += .95f;
            middlePos.x += .95f;

        }

        public void DoPoison()
        {
            var bundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            var goop = bundle.LoadAsset<GoopDefinition>("assets/data/goops/poison goop.asset");
            var ddgm = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goop);
            ddgm.TimedAddGoopCircle(middlePos, 8f, 1f);
        }
    }
    class GunpodwerFactoryBarrelSpawner
    {
        public static GameObject spawnerPrefab;

        public static void Init()
        {
            GameObject obj = Alexandria.PrefabAPI.PrefabBuilder.BuildObject("GunpowderFactoryBarrelSpawner");
            SpriteBuilder.SpriteFromResource("Items/Resources/Enemies/Bosses/GunpowderFactory/BarrelSpawner/gunpowder_factory_barrel_spawner", obj);
            obj.AddAnimation("open", "Items/Resources/Enemies/Bosses/GunpowderFactory/BarrelSpawner/Open", 4, Library.AnimationType.Other, DirectionType.None, FlipType.None, tk2dSpriteAnimationClip.WrapMode.Once);
            obj.AddAnimation("idle", "Items/Resources/Enemies/Bosses/GunpowderFactory/BarrelSpawner/Idle", 6, Library.AnimationType.Other, DirectionType.None, FlipType.None, tk2dSpriteAnimationClip.WrapMode.Loop);
            obj.AddComponent<BarrelSpawnerController>();
            SpeculativeRigidbody rb = ShopAPI.GenerateOrAddToRigidBody(obj, CollisionLayer.Trap, PixelCollider.PixelColliderGeneration.Tk2dPolygon, false, false, false, false, false, false);            
            BarrelSpawnerController spawner = obj.AddComponent<BarrelSpawnerController>();
            spawner.rollDirection = IntVector2.South;
            spawner.barrelPrefab = BulletDrum.prefab;
            spawnerPrefab = obj;
            tk2dSprite sprite = obj.GetComponent<tk2dSprite>();
            obj.layer = 20;
            sprite.SortingOrder = 2;
            sprite.HeightOffGround = 0;
            StaticReferences.StoredRoomObjects.Add("GunpowderFactoryBarrelSpawner", spawnerPrefab);
            spawner.gameObject.GetComponent<tk2dSpriteAnimator>().Play("idle");
        }
    }
    public class BarrelSpawnerController : MonoBehaviour
    {
        private Vector2 middlePos;
        public GameObject barrelPrefab;
        public IntVector2 rollDirection;
        private void Start()
        {
            middlePos = base.gameObject.transform.position;
            middlePos.y += 0f;
            middlePos.x += 0f;
        }
        public void SpawnBarrel()
        {
            GameObject obj = Instantiate<GameObject>(BulletDrum.prefab, middlePos, Quaternion.identity);
            SpeculativeRigidbody componentInChildren = obj.GetComponentInChildren<SpeculativeRigidbody>();
            KickableObject component = obj.GetComponent<KickableObject>();
            component.transform.position.XY().GetAbsoluteRoom().RegisterInteractable(component);
            component.ConfigureOnPlacement(component.transform.position.XY().GetAbsoluteRoom());
            componentInChildren.Initialize();
            PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(componentInChildren, null, false);
           
            DelayedBreakKickable behav = obj.AddComponent<DelayedBreakKickable>();
            behav.obj = obj;
            behav.maxDelay = 4;
            behav.minDelay = 2;
            
        }
    }
    public class DelayedBreakKickable : MonoBehaviour
    {
        public GameObject obj;
        public float maxDelay, minDelay;
        private float delay;
        private DelayedBreakKickable()
        {
            maxDelay = 1;
            minDelay = 1;
        }
        private void Start()
        {
            if(obj == null)
            {
                obj = base.gameObject;
            }
            delay = UnityEngine.Random.Range(minDelay, maxDelay);
        }
        private void Update()
        {
            delay -= Time.deltaTime;
            if(delay <= 0)
            {
                obj.GetComponent<MinorBreakable>().Break();
            }
        }
    }

}
