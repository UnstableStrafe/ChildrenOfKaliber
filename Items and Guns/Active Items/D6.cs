using Gungeon;
using Dungeonator;
using Alexandria.ItemAPI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;


namespace Items
{
    class D6 : PlayerItem
    {
        public static int itemID;
        public static void Init()
        {

            string itemName = "D6";
            string resourceName = "Items/Resources/ItemSprites/Actives/d6.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<D6>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Isaac And His Mother";
            string longDesc = "Rerolls the nearest item on the ground.\n\nA small die used for tabletop roleplaying games. It seems to be enchanted.";


            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ck");
            item.quality = PickupObject.ItemQuality.A;
            item.sprite.IsPerpendicular = true;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 1000f);
            item.consumable = false;
            itemID = item.PickupObjectId;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
          
        }
        public DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            
            return debrisObject;
        }
        public override void DoEffect(PlayerController user)
        {
            base.DoEffect(user);
            FindItem(user);
            AkSoundEngine.PostEvent("Play_OBJ_Chest_Synergy_Slots_01", base.gameObject);
        }
        private void FindItem(PlayerController user)
        {
            
            pickupsInRoom.Clear();
            DebrisObject[] shitOnGround = FindObjectsOfType<DebrisObject>();
            foreach (DebrisObject debris in shitOnGround)
            {
                bool isValid = DetermineIfValid(debris);
                if (isValid && debris.transform.position.GetAbsoluteRoom() == user.CurrentRoom)
                {
                    pickupsInRoom.Add(debris);
                }
            }
            if(pickupsInRoom.Any())
            {
                DebrisObject target = BraveUtility.GetClosestToPosition(pickupsInRoom, user.CenterPosition);
                if (target)
                {
                    RerollItem(target);
                }
            }
            else
            {
                Gun[] gunsonground = FindObjectsOfType<Gun>();
                foreach (Gun gunDebris in gunsonground)
                {
                    PickupObject itemness = gunDebris.gameObject.GetComponent<PickupObject>();
                    if ((itemness != null) && gunDebris.CurrentOwner == null && gunDebris.gameObject.transform.position != Vector3.zero)
                    {
                        gunsInRoom.Add(gunDebris);

                    }
                }
                if (gunsInRoom.Any())
                {
                    Gun targetGun = BraveUtility.GetClosestToPosition(gunsInRoom, user.CenterPosition);
                    if (targetGun && targetGun != null)
                    {
                        RerollGun(targetGun);
                    }
                }
            }
            
        }
        private void RerollGun(Gun gunDebris)
        {
            int checkLimit = 0;
            PickupObject targetGun = gunDebris.gameObject.GetComponent<PickupObject>();
            if(targetGun != null)
            {
                ItemQuality targetTier = targetGun.quality;
                int chanceToDowngrade = 10;
                int chanceToUpgrade = 80;
                ItemQuality newGunQuality = targetTier;
                PickupObject newGunObject = PickupObjectDatabase.GetByName("ck:r.g.g.");
                int RollToCheckUpgradeStatus = UnityEngine.Random.Range(0, 101);
                if (RollToCheckUpgradeStatus <= chanceToDowngrade && targetTier != ItemQuality.D)
                {
                    newGunQuality = targetTier - 1;
                }
                else if (RollToCheckUpgradeStatus >= chanceToUpgrade && targetTier != ItemQuality.S)
                {
                    newGunQuality = targetTier + 1;
                }
                GenericLootTable lootTableGuns = GameManager.Instance.RewardManager.GunsLootTable;
                do
                {
                    newGunObject = LootEngine.GetItemOfTypeAndQuality<Gun>(newGunQuality, lootTableGuns);
                    checkLimit++;
                } while (newGunObject.PickupObjectId == targetGun.PickupObjectId && checkLimit <10);
                if(UnityEngine.Random.Range(0, 101) <= 1)
                {
                    Chest rainbow_Chest = GameManager.Instance.RewardManager.Rainbow_Chest;
                    Chest chest2 = Chest.Spawn(rainbow_Chest, targetGun.sprite.WorldCenter.ToIntVector2(VectorConversions.Round));
                    chest2.BecomeGlitchChest();
                    chest2.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/RainbowChestShader");

                    chest2.RegisterChestOnMinimap(chest2.GetAbsoluteParentRoom());
                }
                else
                {
                    LootEngine.DoDefaultPurplePoof(targetGun.sprite.WorldCenter);
                    LootEngine.SpawnItem(newGunObject.gameObject, targetGun.sprite.WorldCenter, Vector2.zero, 0);
                }
                Destroy(targetGun.gameObject);
            }
        }
        private void RerollItem(DebrisObject debris)
        {
            
            int checkLimit = 0;
            PickupObject targetedItem = debris.gameObject.GetComponent<PickupObject>();
            if(targetedItem != null)
            {
                ItemQuality targetTier = targetedItem.quality;
                int chanceToDowngrade = 10; 
                int chanceToUpgrade = 80;
                ItemQuality newItemQuality = targetTier;
                PickupObject newItemObject = PickupObjectDatabase.GetByName("ck:test_item");
                int RollToCheckUpgradeStatus = UnityEngine.Random.Range(0, 101); 
                if(RollToCheckUpgradeStatus <= chanceToDowngrade && targetTier != ItemQuality.D)
                {
                    newItemQuality = targetTier - 1;
                }
                else if (RollToCheckUpgradeStatus >= chanceToUpgrade && targetTier != ItemQuality.S)
                {
                    newItemQuality = targetTier + 1;
                }
                GenericLootTable lootTableItems = GameManager.Instance.RewardManager.ItemsLootTable;
               
                if (targetedItem is PassiveItem)
                {
                    do
                    {
                        newItemObject = LootEngine.GetItemOfTypeAndQuality<PassiveItem>(newItemQuality, lootTableItems);
                        checkLimit++;
                    } while (newItemObject.PickupObjectId == targetedItem.PickupObjectId && checkLimit < 10);
                    
                }
                else if (targetedItem is PlayerItem)
                {
                    do
                    {
                        newItemObject = LootEngine.GetItemOfTypeAndQuality<PlayerItem>(newItemQuality, lootTableItems);
                        checkLimit++;
                    } while (newItemObject.PickupObjectId == targetedItem.PickupObjectId && checkLimit < 10);
                    
                }
                
                if(UnityEngine.Random.Range(0, 101) <= 1)
                {

                    Chest rainbow_Chest = GameManager.Instance.RewardManager.Rainbow_Chest;
                    Chest chest2 = Chest.Spawn(rainbow_Chest, targetedItem.sprite.WorldCenter.ToIntVector2(VectorConversions.Round));
                    chest2.BecomeGlitchChest();
                    chest2.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/RainbowChestShader");

                    chest2.RegisterChestOnMinimap(chest2.GetAbsoluteParentRoom());

                }
                else
                {
                    LootEngine.DoDefaultPurplePoof(targetedItem.sprite.WorldCenter);
                    LootEngine.SpawnItem(newItemObject.gameObject, targetedItem.sprite.WorldCenter, Vector2.zero, 0);
                }
                Destroy(targetedItem.gameObject);
            }
        }
        private void EndEffect(PlayerController player)
        {
            AkSoundEngine.PostEvent("Play_OBJ_Chest_Synergy_Win_01", base.gameObject);
        }
        private List<int> invalidIDs = new List<int>()
        {
            78, //Ammo
            600, //Spread Ammo
            565, //Glass Guon Stone
            73, //Half Heart
            85, //Heart
            120, //Armor
            224, //Blank
            67, //Key
            127, //Junk

        };
        
        private bool DetermineIfValid(DebrisObject thing)
        {
            PickupObject itemness = thing.gameObject.GetComponent<PickupObject>();
            if (itemness != null)
            {
                if (!invalidIDs.Contains(itemness.PickupObjectId))
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }
        private List<DebrisObject> pickupsInRoom = new List<DebrisObject>()
        { };
        private List<Gun> gunsInRoom = new List<Gun>() { };

    }
}
