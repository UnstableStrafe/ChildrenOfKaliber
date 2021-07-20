using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace ChestAPI
{
    public static class ChestBuilder
    {
        public enum ChestType
        {
            PassiveActive, Gun, Unspecified
        }
        /// <summary>
        /// Creates a custom chest.
        /// </summary>
        /// <param name="idleSprite">The pathway to the idle sprite. Setup like how you would do an item. Do not put an idle inside of the spritePaths list!</param>
        /// <param name="gameObjectName">The name of the chest's game object</param>
        /// <param name="hitboxOffset">The offset of the hitbox. Idk what this does but you can tinker with it if you need to.</param>
        /// <param name="hitboxDimensions">Start at the size of the chest's idle dimensions and tinker with the values until they feel right</param>
        /// <param name="spritePaths">A list of the chest's sprite paths.</param>
        /// <param name="lootTableIdsAndWeights">Uses a custom class I made called ItemAndWeight that allows me to have a list with mutliple types per item in it. Just make a list of ItemAndWeights like this: new ItemAndWeight {itemID = "the gun/passive/active's id", itemWeight = "x" (Best left at 1), itemCanHaveDupes = false/true (idk what this really means but I would add it.)}; </param>
        /// <param name="sTierChance">The chance to get an S tier item from the chest's loot pool.</param>
        /// <param name="aTierChance">The chance to get an A tier item from the chest's loot pool.</param>
        /// <param name="bTierChance">The chance to get a B tier item from the chest's loot pool.</param>
        /// <param name="cTierChance">The chance to get a C tier item from the chest's loot pool.</param>
        /// <param name="dTierChance">The chance to get a D tier item from the chest's loot pool.</param>
        /// <param name="isLocked">If the chest is locked.</param>
        /// <param name="type">If the chest is a Passive/Active chest, a Gun chest, or unspecified.</param>
        /// <param name="potentialPrefab">If you are using a RealPrefab, put it here. If you don't know what this means or aren't using one, leave this null.</param>
        public static Chest CreateChest(string idleSprite, string gameObjectName, IntVector2 hitboxOffset, IntVector2 hitboxDimensions, List<string> spritePaths, List<ItemAndWeight> lootTableIdsAndWeights, float sTierChance, float aTierChance, float bTierChance, float cTierChance, float dTierChance, ChestType type = ChestType.Unspecified,bool isLocked = true, GameObject potentialPrefab = null)
        {
            try
            {
                GameObject obj = SpriteBuilder.SpriteFromResource(idleSprite, potentialPrefab == null ? new GameObject(gameObjectName) : potentialPrefab);
                obj.SetActive(false);
                FakePrefab.MarkAsFakePrefab(obj);
                UnityEngine.Object.DontDestroyOnLoad(obj);
                tk2dSprite sprite = obj.GetComponent<tk2dSprite>();
                SpeculativeRigidbody body = sprite.SetUpSpeculativeRigidbody(hitboxOffset, hitboxDimensions);
                body.PrimaryPixelCollider.CollisionLayer = CollisionLayer.HighObstacle;
                tk2dSpriteAnimator animator = obj.AddComponent<tk2dSpriteAnimator>();
                animator.Library = animator.gameObject.AddComponent<tk2dSpriteAnimation>();
                animator.Library.clips = new tk2dSpriteAnimationClip[0];
                animator.Library.enabled = true;
                List<int> appearIds = new List<int>();
                List<int> breakIds = new List<int>();
                List<int> openIds = new List<int>();
                string zeroHpName = "";
                foreach (string text in spritePaths)
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
                Chest chestController = obj.AddComponent<Chest>();
                chestController.spawnCurve = new AnimationCurve
                {
                    keys = new Keyframe[] { new Keyframe { time = 0f, value = 0f, inTangent = 3.562501f, outTangent = 3.562501f }, new Keyframe { time = 1f, value = 1.0125f, inTangent = 0.09380959f,
                outTangent = 0.09380959f } }
                };

                GenericLootTable Table;
                Table = UnityEngine.Object.Instantiate<GenericLootTable>(GameManager.Instance.RewardManager.ItemsLootTable);
                Table.defaultItemDrops = new WeightedGameObjectCollection();
                Table.defaultItemDrops.elements = new List<WeightedGameObject>();
                foreach(ItemAndWeight itemAndWeight in lootTableIdsAndWeights)
                {
                    Table.defaultItemDrops.elements.Add(new WeightedGameObject()
                    {
                        pickupId = itemAndWeight.itemID,
                        weight = itemAndWeight.itemWeight,
                        forceDuplicatesPossible = itemAndWeight.itemCanHaveDupes,
                        additionalPrerequisites = new DungeonPrerequisite[0]
                    });
                }
                chestController.openAnimName = "open";
                chestController.spawnAnimName = "appear";
                chestController.majorBreakable = obj.AddComponent<MajorBreakable>();
                chestController.majorBreakable.spriteNameToUseAtZeroHP = zeroHpName;
                chestController.majorBreakable.usesTemporaryZeroHitPointsState = true;
                chestController.breakAnimName = "break";
                chestController.VFX_GroundHit = new GameObject("example thingy");
                chestController.VFX_GroundHit.transform.parent = chestController.transform;
                chestController.VFX_GroundHit.SetActive(false);
                chestController.groundHitDelay = 5f;
                chestController.overrideMimicChance = 0f;
                chestController.lootTable = new LootData();
                chestController.lootTable.lootTable = Table;
                chestController.lootTable.S_Chance = sTierChance;
                chestController.lootTable.A_Chance = aTierChance;
                chestController.lootTable.B_Chance = bTierChance;
                chestController.lootTable.C_Chance = cTierChance;
                chestController.lootTable.D_Chance = dTierChance;
                switch (type)
                {
                    case ChestType.Unspecified:
                        chestController.ChestType = Chest.GeneralChestType.UNSPECIFIED;
                        break;
                    case ChestType.PassiveActive:
                        chestController.ChestType = Chest.GeneralChestType.ITEM;
                        break;
                    case ChestType.Gun:
                        chestController.ChestType = Chest.GeneralChestType.WEAPON;
                        break;
                }
                chestController.IsLocked = isLocked;
                return chestController;
            }
            catch(Exception e)
            {
                ETGModConsole.Log("Something BROKE when making a chestController! Error is: " + e.ToString());
                return null;
            }
        }
        
    }
    public class ItemAndWeight
    {
        public int itemID;
        public float itemWeight;
        public bool itemCanHaveDupes;
        public ItemAndWeight()
        {
            itemWeight = 1;
            itemCanHaveDupes = false;
        }
    }
}
