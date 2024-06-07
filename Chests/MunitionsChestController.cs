using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using Dungeonator;
using Alexandria.DungeonAPI;

namespace Items
{
    class MunitionsChestController : Chest
    {

        public static List<GenericLootTable> MLoot = new List<GenericLootTable>
        {

        };
        public static Chest munitionsChest = new Chest();



        private void Start()
        {

            RoomHandler room = base.GetAbsoluteParentRoom();
            base.ConfigureOnPlacement(room);
        }

        public static void Init()
        {
            try
            {
                GameObject obj = ChildrenOfKaliberModule.ModAssets.LoadAsset<GameObject>("munitionschestobj");
                SpriteBuilder.SpriteFromResource("Items/Resources/MunitionsChest/munitions_chest_idle_001", obj);
                tk2dSprite sprite = obj.GetComponent<tk2dSprite>();

                sprite.HeightOffGround = -1;

                tk2dSpriteAnimator animator = obj.AddComponent<tk2dSpriteAnimator>();
                animator.Library = animator.gameObject.AddComponent<tk2dSpriteAnimation>();
                animator.Library.clips = new tk2dSpriteAnimationClip[0];
                animator.Library.enabled = true;
                List<int> appearIds = new List<int>();
                List<int> breakIds = new List<int>();
                List<int> openIds = new List<int>();
                string zeroHpName = "";
                foreach (string text in MunitionsChestController.spritePaths)
                {
                    if (text.Contains("appear"))
                    {
                        appearIds.Add(SpriteBuilder.AddSpriteToCollection(text, obj.GetComponent<tk2dBaseSprite>().Collection));
                    }
                    else if (text.Contains("break"))
                    {
                        if (text.EndsWith("001"))
                        {
                            int id = SpriteBuilder.AddSpriteToCollection(text, obj.GetComponent<tk2dBaseSprite>().Collection);
                            zeroHpName = obj.GetComponent<tk2dBaseSprite>().Collection.inst.spriteDefinitions[id].name;
                        }
                        else
                        {
                            breakIds.Add(SpriteBuilder.AddSpriteToCollection(text, obj.GetComponent<tk2dBaseSprite>().Collection));
                        }
                    }
                    else if (text.Contains("open"))
                    {
                        openIds.Add(SpriteBuilder.AddSpriteToCollection(text, obj.GetComponent<tk2dBaseSprite>().Collection));
                    }
                    else
                    {
                        appearIds.Add(SpriteBuilder.AddSpriteToCollection(text, obj.GetComponent<tk2dBaseSprite>().Collection));
                    }
                }
                appearIds.Add(sprite.spriteId);
                tk2dSpriteAnimationClip openClip = new tk2dSpriteAnimationClip { name = "open", fps = 10, frames = new tk2dSpriteAnimationFrame[0] };
                foreach (int id in openIds)
                {
                    tk2dSpriteAnimationFrame frame = new tk2dSpriteAnimationFrame
                    {
                        spriteId = id,
                        spriteCollection = obj.GetComponent<tk2dBaseSprite>().Collection
                    };
                    openClip.frames = openClip.frames.Concat(new tk2dSpriteAnimationFrame[] { frame }).ToArray();
                }
                openClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
                animator.Library.clips = animator.Library.clips.Concat(new tk2dSpriteAnimationClip[] { openClip }).ToArray();
                tk2dSpriteAnimationClip appearClip = new tk2dSpriteAnimationClip { name = "appear", fps = 10, frames = new tk2dSpriteAnimationFrame[0] };
                foreach (int id in appearIds)
                {
                    tk2dSpriteAnimationFrame frame = new tk2dSpriteAnimationFrame
                    {
                        spriteId = id,
                        spriteCollection = obj.GetComponent<tk2dBaseSprite>().Collection
                    };
                    appearClip.frames = appearClip.frames.Concat(new tk2dSpriteAnimationFrame[] { frame }).ToArray();
                }
                appearClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
                animator.Library.clips = animator.Library.clips.Concat(new tk2dSpriteAnimationClip[] { appearClip }).ToArray();
                tk2dSpriteAnimationClip breakClip = new tk2dSpriteAnimationClip { name = "break", fps = 10, frames = new tk2dSpriteAnimationFrame[0] };
                foreach (int id in breakIds)
                {
                    tk2dSpriteAnimationFrame frame = new tk2dSpriteAnimationFrame
                    {
                        spriteId = id,
                        spriteCollection = obj.GetComponent<tk2dBaseSprite>().Collection
                    };
                    breakClip.frames = breakClip.frames.Concat(new tk2dSpriteAnimationFrame[] { frame }).ToArray();
                }
                breakClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
                animator.Library.clips = animator.Library.clips.Concat(new tk2dSpriteAnimationClip[] { breakClip }).ToArray();

                GameObject chestLock = obj.transform.Find("Lock").gameObject;
                SpriteBuilder.SpriteFromResource("Items/Resources/MunitionsChest/Lock/munitions_lock_idle_001", chestLock);
                tk2dSprite lockSprite = chestLock.GetComponent<tk2dSprite>();
                lockSprite.HeightOffGround = -.5f;

                obj.GetComponent<tk2dBaseSprite>().collectionInst = obj.GetComponent<tk2dBaseSprite>().Collection;

                List<string> lockOpen = new List<string>
                {
                    "Items/Resources/MunitionsChest/Lock/munitions_lock_open_001",
                    "Items/Resources/MunitionsChest/Lock/munitions_lock_open_002",
                    "Items/Resources/MunitionsChest/Lock/munitions_lock_open_003",
                    "Items/Resources/MunitionsChest/Lock/munitions_lock_open_004",
                    "Items/Resources/MunitionsChest/Lock/munitions_lock_open_005",
                };
                List<string> lockNoKey = new List<string>
                {
                    "Items/Resources/MunitionsChest/Lock/munitions_lock_idle_001",
                    "Items/Resources/MunitionsChest/Lock/munitions_lock_nokey_001",
                    "Items/Resources/MunitionsChest/Lock/munitions_lock_idle_001",
                    "Items/Resources/MunitionsChest/Lock/munitions_lock_nokey_002",
                    "Items/Resources/MunitionsChest/Lock/munitions_lock_idle_001",
                    "Items/Resources/MunitionsChest/Lock/munitions_lock_nokey_001",
                    "Items/Resources/MunitionsChest/Lock/munitions_lock_idle_001",
                    "Items/Resources/MunitionsChest/Lock/munitions_lock_nokey_002",
                    "Items/Resources/MunitionsChest/Lock/munitions_lock_idle_001",
                    "Items/Resources/MunitionsChest/Lock/munitions_lock_nokey_001",
                    "Items/Resources/MunitionsChest/Lock/munitions_lock_idle_001",
                };
                List<string> lockBreak = new List<string>
                {
                    "Items/Resources/MunitionsChest/Lock/munitions_lock_broke_001",
                };

                tk2dSpriteCollectionData lockCollection = null;
                if (lockCollection == null)
                {
                    lockCollection = SpriteBuilder.ConstructCollection(chestLock, "MunitionsLockCollection");
                    for (int i = 0; i < lockOpen.Count(); i++)
                    {
                        SpriteBuilder.AddSpriteToCollection(lockOpen[i], lockCollection);
                    }
                    for (int i = 0; i < lockNoKey.Count(); i++)
                    {
                        SpriteBuilder.AddSpriteToCollection(lockNoKey[i], lockCollection);
                    }
                    for (int i = 0; i < lockBreak.Count(); i++)
                    {
                        SpriteBuilder.AddSpriteToCollection(lockBreak[i], lockCollection);
                    }
                }

                Library.GenerateSpriteAnimator(chestLock);

                tk2dSpriteAnimator chestLockAnimator = chestLock.GetComponent<tk2dSpriteAnimator>();

                var clip_1 = Library.AddAnimation2(chestLockAnimator, lockCollection, lockOpen, "munitions_lock_open", tk2dSpriteAnimationClip.WrapMode.Once, 12);
                chestLockAnimator.GetClipByName("munitions_lock_open").frames[0].eventAudio = "Play_OBJ_chest_unlock_01";
                chestLockAnimator.GetClipByName("munitions_lock_open").frames[0].triggerEvent = true;
                var clip_2 = Library.AddAnimation2(chestLockAnimator, lockCollection, lockNoKey, "munitions_lock_nokey", tk2dSpriteAnimationClip.WrapMode.Once, 12);
                chestLockAnimator.GetClipByName("munitions_lock_nokey").frames[0].eventAudio = "Play_OBJ_lock_jiggle_01";
                chestLockAnimator.GetClipByName("munitions_lock_nokey").frames[0].triggerEvent = true;
                var clip_3 = Library.AddAnimation2(chestLockAnimator, lockCollection, lockBreak, "munitions_lock_break", tk2dSpriteAnimationClip.WrapMode.Once, 12);
                chestLockAnimator.GetClipByName("munitions_lock_break").frames[0].eventAudio = "Play_OBJ_purchase_unable_01";
                chestLockAnimator.GetClipByName("munitions_lock_break").frames[0].triggerEvent = true;

                tk2dSpriteAnimationClip LockOpen = chestLockAnimator.spriteAnimator.GetClipByName("munitions_lock_open");
                float[] offsetsX2 = new float[] { -0.125f, -0.125f, -0.125f, -0.125f, -0.125f };
                float[] offsetsY2 = new float[] { -.5f, -.5f, -.5f, -.5f, -.5f };
                for (int i = 0; i < offsetsX2.Length && i < offsetsY2.Length && i < LockOpen.frames.Length; i++)
                {
                    int id = LockOpen.frames[i].spriteId;
                    LockOpen.frames[i].spriteCollection.spriteDefinitions[id].position0.x += offsetsX2[i];
                    LockOpen.frames[i].spriteCollection.spriteDefinitions[id].position0.y += offsetsY2[i];
                    LockOpen.frames[i].spriteCollection.spriteDefinitions[id].position1.x += offsetsX2[i];
                    LockOpen.frames[i].spriteCollection.spriteDefinitions[id].position1.y += offsetsY2[i];
                    LockOpen.frames[i].spriteCollection.spriteDefinitions[id].position2.x += offsetsX2[i];
                    LockOpen.frames[i].spriteCollection.spriteDefinitions[id].position2.y += offsetsY2[i];
                    LockOpen.frames[i].spriteCollection.spriteDefinitions[id].position3.x += offsetsX2[i];
                    LockOpen.frames[i].spriteCollection.spriteDefinitions[id].position3.y += offsetsY2[i];
                }

                tk2dSpriteAnimationClip Unable = chestLockAnimator.spriteAnimator.GetClipByName("munitions_lock_break");
                float[] offsetsX3 = new float[] { -0.0625f, };
                float[] offsetsY3 = new float[] { -.125f, };
                for (int i = 0; i < offsetsX3.Length && i < offsetsY3.Length && i < Unable.frames.Length; i++)
                {
                    int id = Unable.frames[i].spriteId;
                    Unable.frames[i].spriteCollection.spriteDefinitions[id].position0.x += offsetsX3[i];
                    Unable.frames[i].spriteCollection.spriteDefinitions[id].position0.y += offsetsY3[i];
                    Unable.frames[i].spriteCollection.spriteDefinitions[id].position1.x += offsetsX3[i];
                    Unable.frames[i].spriteCollection.spriteDefinitions[id].position1.y += offsetsY3[i];
                    Unable.frames[i].spriteCollection.spriteDefinitions[id].position2.x += offsetsX3[i];
                    Unable.frames[i].spriteCollection.spriteDefinitions[id].position2.y += offsetsY3[i];
                    Unable.frames[i].spriteCollection.spriteDefinitions[id].position3.x += offsetsX3[i];
                    Unable.frames[i].spriteCollection.spriteDefinitions[id].position3.y += offsetsY3[i];
                }

                tk2dSpriteAnimationClip Jiggle = chestLockAnimator.spriteAnimator.GetClipByName("munitions_lock_nokey");
                float[] offsetsX4 = new float[] { 0f, 0.125f, 0f, -0.125f, };
                float[] offsetsY4 = new float[] { 0f, -0.125f, 0f, -0.125f };
                for (int i = 0; i < offsetsX4.Length && i < offsetsY4.Length; i++)
                {
                    int id = Jiggle.frames[i].spriteId;
                    Jiggle.frames[i].spriteCollection.spriteDefinitions[id].position0.x += offsetsX4[i];
                    Jiggle.frames[i].spriteCollection.spriteDefinitions[id].position0.y += offsetsY4[i];
                    Jiggle.frames[i].spriteCollection.spriteDefinitions[id].position1.x += offsetsX4[i];
                    Jiggle.frames[i].spriteCollection.spriteDefinitions[id].position1.y += offsetsY4[i];
                    Jiggle.frames[i].spriteCollection.spriteDefinitions[id].position2.x += offsetsX4[i];
                    Jiggle.frames[i].spriteCollection.spriteDefinitions[id].position2.y += offsetsY4[i];
                    Jiggle.frames[i].spriteCollection.spriteDefinitions[id].position3.x += offsetsX4[i];
                    Jiggle.frames[i].spriteCollection.spriteDefinitions[id].position3.y += offsetsY4[i];
                }

                Chest chest = obj.AddComponent<MunitionsChestController>();

                chest.spawnCurve = new AnimationCurve
                {
                    keys = new Keyframe[] { new Keyframe { time = 0f, value = 0f, inTangent = 3.562501f, outTangent = 3.562501f }, new Keyframe { time = 1f, value = 1.0125f, inTangent = 0.09380959f,
                outTangent = 0.09380959f } }
                };
                SpeculativeRigidbody body = obj.AddComponent<SpeculativeRigidbody>();//sprite.SetUpSpeculativeRigidbody(new IntVector2(0, -8), new IntVector2(25, 25));
                body.enabled = true;
                body.PixelColliders = new List<PixelCollider>();
                body.PixelColliders.Add(new PixelCollider
                {
                    ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                    CollisionLayer = CollisionLayer.HighObstacle,
                    IsTrigger = false,
                    BagleUseFirstFrameOnly = false,
                    SpecifyBagelFrame = string.Empty,
                    BagelColliderNumber = 0,
                    ManualOffsetX = 0,
                    ManualOffsetY = -4,
                    ManualWidth = 25,
                    ManualHeight = 21,
                    ManualDiameter = 0,
                    ManualLeftX = 0,
                    ManualLeftY = 0,
                    ManualRightX = 0,
                    ManualRightY = 0,

                });
                body.PixelColliders.Add(new PixelCollider
                {
                    ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                    CollisionLayer = CollisionLayer.PlayerBlocker,
                    IsTrigger = false,
                    BagleUseFirstFrameOnly = false,
                    SpecifyBagelFrame = string.Empty,
                    BagelColliderNumber = 0,
                    ManualOffsetX = 0,
                    ManualOffsetY = -4,
                    ManualWidth = 25,
                    ManualHeight = 21,
                    ManualDiameter = 0,
                    ManualLeftX = 0,
                    ManualLeftY = 0,
                    ManualRightX = 0,
                    ManualRightY = 0,

                });
                body.PixelColliders.Add(new PixelCollider
                {
                    ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                    CollisionLayer = CollisionLayer.BulletBlocker,
                    IsTrigger = false,
                    BagleUseFirstFrameOnly = false,
                    SpecifyBagelFrame = string.Empty,
                    BagelColliderNumber = 0,
                    ManualOffsetX = 0,
                    ManualOffsetY = -4,
                    ManualWidth = 25,
                    ManualHeight = 21,
                    ManualDiameter = 0,
                    ManualLeftX = 0,
                    ManualLeftY = 0,
                    ManualRightX = 0,
                    ManualRightY = 0,

                });

                //body.CollideWithOthers = true;
                List<int> items = new List<int>
                {
                    //Base Game
                    //----------
                    //Items with "Bullet" in the name
                    579,
                    627,
                    298,
                    640,
                    111,
                    113,
                    172,
                    277,
                    278,
                    284,
                    286,
                    288,
                    323,
                    352,
                    373,
                    374,
                    375,
                    410,
                    521,
                    523,
                    528,
                    530,
                    531,
                    532,
                    533,
                    538,
                    568,
                    569,
                    630,
                    655,
                    661,
                    822,
                    //Items with "Rounds" in the name
                    298,
                    304,
                    527,
                    638,
                    //Misc Item names
                    636,
                    241,
                    204,
                    295,
                    241,
                    524,
                    287,
                    //My Mod
                    //------
                    //Items with "Bullet" in the name
                    ETGMod.Databases.Items["Funky Bullets"].PickupObjectId,
                    ETGMod.Databases.Items["Needle Bullets"].PickupObjectId,
                    ETGMod.Databases.Items["Snare Bullets"].PickupObjectId,
                    ETGMod.Databases.Items["[_____] Bullets"].PickupObjectId,
                    ETGMod.Databases.Items["Floop Bullets"].PickupObjectId,
                    ETGMod.Databases.Items["Elite Bullets"].PickupObjectId,
                    //Items with "Rounds" in the name
                    ETGMod.Databases.Items["Venom Rounds"].PickupObjectId,
                    ETGMod.Databases.Items["Ballistic Rounds"].PickupObjectId,
                    ETGMod.Databases.Items["Temporal Rounds"].PickupObjectId,
                    //Misc Item names
                    ETGMod.Databases.Items["Imp's Horn"].PickupObjectId,
                    ETGMod.Databases.Items["Sawblade"].PickupObjectId,
                    ETGMod.Databases.Items["Mininomocon"].PickupObjectId,
                    ETGMod.Databases.Items["Gunlust"].PickupObjectId,
                    ETGMod.Databases.Items["Dragun Skull"].PickupObjectId,
                    ETGMod.Databases.Items["Gravity Well Module"].PickupObjectId,
                    ETGMod.Databases.Items["Primal Nitric Acid"].PickupObjectId,

                };

                var lootTable = LootUtility.CreateLootTable();
                foreach (int i in items)
                {
                    lootTable.AddItemToPool(i);
                }

                //MLoot.Add(Table);
                chest.specRigidbody = body;
                chest.sprite = sprite;

                chest.LockAnimator = chestLockAnimator;
                chest.LockOpenAnim = "munitions_lock_open";
                chest.LockNoKeyAnim = "munitions_lock_nokey";
                chest.LockBreakAnim = "munitions_lock_break";
                chest.openAnimName = "open";
                chest.spawnAnimName = "appear";
                chest.overrideSpawnAnimName = "appear";

                chest.majorBreakable = obj.AddComponent<MajorBreakable>();

                chest.majorBreakable.spriteNameToUseAtZeroHP = zeroHpName;
                chest.majorBreakable.usesTemporaryZeroHitPointsState = true;
                chest.breakAnimName = "break";

                chest.VFX_GroundHit = FakePrefab.InstantiateAndFakeprefab(GameManager.Instance.RewardManager.GetTargetChestPrefab(PickupObject.ItemQuality.A).VFX_GroundHit);
                chest.VFX_GroundHit.transform.parent = chest.transform;
                chest.VFX_GroundHit.transform.localPosition -= new Vector3(1.5f, 0f);
                chest.VFX_GroundHit.SetActive(false);
                chest.ShadowSprite = UnityEngine.Object.Instantiate(GameManager.Instance.RewardManager.GetTargetChestPrefab(PickupObject.ItemQuality.B).transform.GetChild(3).GetComponent<tk2dSprite>());
                chest.ShadowSprite.transform.parent = chest.transform;
                chest.ShadowSprite.transform.localPosition += new Vector3(-1.28125f, 0.3125f);

                chest.majorBreakable.childrenToDestroy = new List<GameObject>()
                {
                     chest.ShadowSprite.gameObject
                };

                chest.groundHitDelay = 0.5f;
                chest.ChestType = GeneralChestType.ITEM;
                chest.overrideMimicChance = 0f;
                chest.lootTable = new LootData();
                chest.lootTable.lootTable = lootTable;
                chest.lootTable.S_Chance = 0.2f;
                chest.lootTable.A_Chance = 0.2f;
                chest.lootTable.B_Chance = 0.2f;
                chest.lootTable.C_Chance = 0.2f;
                chest.lootTable.D_Chance = 0.2f;
                chest.IsLocked = true;
                chest.IsSealed = false;
                chest.IsOpen = false;
                chest.IsBroken = false;

                var Object = SpriteBuilder.SpriteFromResource("Items/Resources/MunitionsChest/munitions_chest_icon_001.png");
                FakePrefab.MakeFakePrefab(Object);

                chest.MinimapIconPrefab = Object;
                FakePrefab.MakeFakePrefab(obj);


                MunitionsChestController.munitionsChest = chest;
                StaticReferences.customObjects.Add("MunitionsChest", chest.gameObject);
            }
            catch (Exception e)
            {
                ETGModConsole.Log("Error in MChest");
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }




        public static string[] spritePaths = new string[]
        {
                "Items/Resources/MunitionsChest/munitions_chest_appear_001",
                "Items/Resources/MunitionsChest/munitions_chest_appear_002",
                "Items/Resources/MunitionsChest/munitions_chest_appear_003",
                "Items/Resources/MunitionsChest/munitions_chest_appear_004",
                "Items/Resources/MunitionsChest/munitions_chest_appear_005",
                "Items/Resources/MunitionsChest/munitions_chest_break_001",
                "Items/Resources/MunitionsChest/munitions_chest_break_002",
                "Items/Resources/MunitionsChest/munitions_chest_break_003",
                "Items/Resources/MunitionsChest/munitions_chest_break_004",
                "Items/Resources/MunitionsChest/munitions_chest_open_001",
                "Items/Resources/MunitionsChest/munitions_chest_open_002",
                "Items/Resources/MunitionsChest/munitions_chest_open_003",
            //"Items/Resources/MunitionsChest/low_chest_shadow_001"
        };




    }




}
