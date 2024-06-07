using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria;
using Alexandria.Misc;
using Alexandria.NPCAPI;
using Alexandria.DungeonAPI;

namespace Items
{
    class CultistShopkeep 
    {
        public static void Init()
        {
            List<string> npcIdleSprites = new List<string> { "Items/Resources/NPCs/CultistShopkeep/cultist_shopkeep_idle_001.png", "Items/Resources/NPCs/CultistShopkeep/cultist_shopkeep_idle_002.png", "Items/Resources/NPCs/CultistShopkeep/cultist_shopkeep_idle_003.png", "Items/Resources/NPCs/CultistShopkeep/cultist_shopkeep_idle_004.png", "Items/Resources/NPCs/CultistShopkeep/cultist_shopkeep_idle_005.png", "Items/Resources/NPCs/CultistShopkeep/cultist_shopkeep_idle_006.png" };

            List<string> npcTalkSprites = new List<string> { "Items/Resources/NPCs/CultistShopkeep/cultist_shopkeep_talk_001.png", "Items/Resources/NPCs/CultistShopkeep/cultist_shopkeep_talk_002.png" };

            var lootTable = LootUtility.CreateLootTable();
            lootTable.AddItemToPool(D6.itemID);            
            lootTable.AddItemToPool(GoldenRecord.itemID);
            lootTable.AddItemToPool(SympathyBullets.itemID);
            //lootTable.AddItemToPool(TemporalRounds.itemID);
            lootTable.AddItemToPool(SuperCrate.itemID);
            lootTable.AddItemToPool(MemeticKillAgent.itemID);
            lootTable.AddItemToPool(Mininomicon.itemID);
            lootTable.AddItemToPool(NeedleBullets.itemID);
            lootTable.AddItemToPool(DragunSkull.dragunSkullId);
            lootTable.AddItemToPool(DragunWing.dragunWingId);
            lootTable.AddItemToPool(DragunHeart.dragunHeartId);
            lootTable.AddItemToPool(DragunClaw.dragunClawID);
            lootTable.AddItemToPool(PrimalCharcoal.primalCharcoalId);
            lootTable.AddItemToPool(PrimalNitricAcid.primalNitricAcidId);
            lootTable.AddItemToPool(PrimalSaltpeter.primalSaltpeterId);
            lootTable.AddItemToPool(PrimalSulfur.primalSulfurId);           
            lootTable.AddItemToPool(TheLastChamber.itemID, .75f);
            lootTable.AddItemToPool(Skeleton.itemID, .9f);
            lootTable.AddItemToPool(VenomRounds.itemID);
            lootTable.AddItemToPool(_66Kaliber.id);
            lootTable.AddItemToPool(Dispenser.DispenserID, .9f);
            lootTable.AddItemToPool(RGG.id);
            lootTable.AddItemToPool(AccursedShackles.id, .9f);
            //lootTable.AddItemToPool(SpiritOfTheDragun.gunID, 1);
            //lootTable.AddItemToPool(TrueGunpowder.itemID, 1);


            //Gunlust
            //Floop Bullets
            //


            ETGMod.Databases.Strings.Core.AddComplex("#KALIBERCULTIST_RUNBASEDMULTILINE_GENERIC", "Care to support your local not-at-all-evil cult?");
            ETGMod.Databases.Strings.Core.AddComplex("#KALIBERCULTIST_RUNBASEDMULTILINE_GENERIC", "We have rare wares for sale.");
            //ETGMod.Databases.Strings.Core.AddComplex("#KALIBERCULTIST_RUNBASEDMULTILINE_GENERIC", "Cookies too, for premium members.");

            ETGMod.Databases.Strings.Core.Set("#KALIBERCULTIST_RUNBASEDMULTILINE_STOPPER", "Buy or begone, flesh!");

            ETGMod.Databases.Strings.Core.Set("#KALIBERCULTIST_SHOP_PURCHASED", "We promise these funds totally will not be used against you in the future.");
            ETGMod.Databases.Strings.Core.Set("#KALIBERCULTIST_SHOP_PURCHASED", "Thank you for the payment, flesh.");

            ETGMod.Databases.Strings.Core.Set("#KALIBERCULTIST_PURCHASE_FAILED", "Bring us more funds, flesh!");

            ETGMod.Databases.Strings.Core.Set("#KALIBERCULTIST_INTRO", "Greetings, fellow being of flesh. Care to buy anything? All proceeds go to funding a 100% non-evil cause.");

            ETGMod.Databases.Strings.Core.Set("#KALIBERCULTIST_ATTACKED", "You cannot hurt us in a way that matters.");
            ETGMod.Databases.Strings.Core.Set("#KALIBERCULTIST_ATTACKED", "Your bullets cannot harm us, flesh.");

            ETGMod.Databases.Strings.Core.Set("#KALIBERCULTIST_STOLEN", "YOU DARE STEAL FROM OUR GODDESS!?");


            CultistShop = ShopAPI.SetUpShop("CultistShopkeep", "ck", npcIdleSprites, 7, npcTalkSprites, 2, lootTable, CustomShopItemController.ShopCurrencyType.COINS,
                "#KALIBERCULTIST_RUNBASEDMULTILINE_GENERIC", "#KALIBERCULTIST_RUNBASEDMULTILINE_STOPPER", "#KALIBERCULTIST_SHOP_PURCHASED", "#KALIBERCULTIST_PURCHASE_FAILED", "#KALIBERCULTIST_INTRO", "#KALIBERCULTIST_ATTACKED", "#KALIBERCULTIST_STOLEN",
                ShopAPI.defaultTalkPointOffset, ShopAPI.defaultNpcPosition, ShopAPI.VoiceBoxes.OLD_MAN, ShopAPI.defaultItemPositions, (5f/6f), false, null, null, null, null, null, null, "", "", true, true, "Items/Resources/NPCs/CultistShopkeep/cultist_shopkeep_carpet_001.png", new Vector3(-.6f, -.6f, 1.7f), true, "Items/Resources/NPCs/CultistShopkeep/cultist_shopkeep_icon_001.png", true, .1f, null).GetOrAddComponent<CustomShopController>();
            StaticReferences.StoredRoomObjects.Add("CultistShopkeep", CultistShop.gameObject);
        }
        public static float RareItemPriceMod(CustomShopController shop, CustomShopItemController shopItem, PickupObject item)
        {
            if (item.PickupObjectId == SpiritOfTheDragun.gunID || item.PickupObjectId == TrueGunpowder.itemID)
            {
                return 3;
            }
            return 5/6;
        }
        public static CustomShopController CultistShop;
    }
}
