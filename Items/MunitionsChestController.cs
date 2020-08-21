using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using UnityEngine;
using Dungeonator;
using ItemAPI;
using System.Diagnostics;

namespace Items
{
    class MunitionsChestController : Chest
    {
            
        public static List<GenericLootTable> MLoot = new List<GenericLootTable>
        {

        };
        public static Chest MChest;
        public static void Init()
        {
            try
            {
                GameObject obj = SpriteBuilder.SpriteFromResource("Items/Resources/MunitionsChest/munitions_chest_idle_001", new GameObject("MunitionsChest"));
                obj.SetActive(false);
                FakePrefab.MarkAsFakePrefab(obj);
                UnityEngine.Object.DontDestroyOnLoad(obj);
                tk2dSprite sprite = obj.GetComponent<tk2dSprite>();
                SpeculativeRigidbody body = sprite.SetUpSpeculativeRigidbody(new IntVector2(0, -8), new IntVector2(25, 25));
                body.PrimaryPixelCollider.CollisionLayer = CollisionLayer.HighObstacle;
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
                Chest chest = obj.AddComponent<Chest>();
                chest.spawnCurve = new AnimationCurve
                {
                    keys = new Keyframe[] { new Keyframe { time = 0f, value = 0f, inTangent = 3.562501f, outTangent = 3.562501f }, new Keyframe { time = 1f, value = 1.0125f, inTangent = 0.09380959f,
                outTangent = 0.09380959f } }
                };
                List<int> MuniStock = new List<int>
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
                    //Misc Item names
                    ETGMod.Databases.Items["Imp's Horn"].PickupObjectId,
                    ETGMod.Databases.Items["Sawblade"].PickupObjectId,
                    ETGMod.Databases.Items["Mininomocon"].PickupObjectId,
                    ETGMod.Databases.Items["Gunlust"].PickupObjectId,
                    ETGMod.Databases.Items["Dragun Skull"].PickupObjectId,
                    ETGMod.Databases.Items["Gravity Well Module"].PickupObjectId,

                };
                GenericLootTable Table;
                Table = ScriptableObject.CreateInstance<GenericLootTable>();
                Table.defaultItemDrops = new WeightedGameObjectCollection();
                foreach (int id in MuniStock)
                {
                    Table.defaultItemDrops.Add(new WeightedGameObject()
                    {
                        pickupId = id,
                        weight = 1,
                        forceDuplicatesPossible = false,
                        additionalPrerequisites = new DungeonPrerequisite[0]
                    });

                }

                //MLoot.Add(Table);
                MunitionsChestController.MChest = chest;
                chest.openAnimName = "open";
                chest.spawnAnimName = "appear";
                chest.majorBreakable = obj.AddComponent<MajorBreakable>();
                chest.majorBreakable.spriteNameToUseAtZeroHP = zeroHpName;
                chest.majorBreakable.usesTemporaryZeroHitPointsState = true;
                chest.breakAnimName = "break";
                chest.VFX_GroundHit = new GameObject("example thingy");
                chest.VFX_GroundHit.transform.parent = chest.transform;
                chest.VFX_GroundHit.SetActive(false);
                chest.groundHitDelay = 2f / 10f;
                chest.ChestType = GeneralChestType.ITEM;
                chest.overrideMimicChance = 0f;
                chest.lootTable = new LootData();
                
            }
            catch(Exception e)
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
