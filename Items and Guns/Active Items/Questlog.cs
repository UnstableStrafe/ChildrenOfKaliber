using ItemAPI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Items
{
    class Questlog : PlayerItem
    {
        public static void Init()
        {

            string itemName = "Quest Log";
            string resourceName = "Items/Resources/quest_log.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<Questlog>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Filled To The Brim";
            string longDesc = "On use, assigns a quest. Cannot be used again until the quest is completed. Once the quest is completed, using the item will reward you and assign another quest." +
                "\n\nThis old, leatherbound book is filled with lists, wanted posters, job requests and other tasks.";



            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 1f);
            item.consumable = false;
            item.quality = PickupObject.ItemQuality.D;
            item.sprite.IsPerpendicular = true;
        }
        protected override void DoEffect(PlayerController user)
        {
            Kill = false;
            Shop = false;
            Table = false;

            Blank = false;
            Clear = false;
            KillNum = 0;
            ShopNum = 0;
            TableNum = 0;
            BlankNum = 0;
            ClearNum = 0;
            string header = "Quest assigned";
            string text = "";
            int QuestNum = Random.Range(1, 6);
            if(QuestNum == 1)
            {
                KillReq = Random.Range(4, 11);
                text = "Kill " + KillReq + " enemies";
                Kill = true;
            }
            if (QuestNum == 2)
            {
                ShopReq = Random.Range(1, 3);
                text = "Buy "+ ShopReq + " items";
                Shop = true;
            }
            if (QuestNum == 3)
            {
                TableReq = Random.Range(2, 6);
                text = "Flip " + TableReq + " tables";
                Table = true;
            }
            if (QuestNum == 4)
            {
                BlankReq = 1;
                text = "Use "+ BlankReq + " blank";
                Blank = true;
            }
            if (QuestNum == 5)
            {
                ClearReq = Random.Range(3, 8);
                text = "Clear " + ClearReq + " rooms";
                Clear = true;
            }
            this.Notify(header, text);
            QuestComplete = false;
            
            if(RewardDue == true)
            {
                int loop = 0;

                int RewardGive = Random.Range(1, 7);
                if(RewardGive == 1 && RewardDue == true)
                {
                    int currencyGive = Random.Range(10, 26);
                    
                    while(loop < currencyGive)
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(68).gameObject, user);
                        loop += 1;
                    }
                    loop = 0;

                }
                if (RewardGive == 2 && RewardDue == true)
                {
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, user);
                }
                if (RewardGive == 3 && RewardDue == true)
                {
                    int glassGive = Random.Range(1, 4);
                    while(loop < glassGive)
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(565).gameObject, user);
                        loop += 1;
                    }
                    loop = 0;
                }
                if (RewardGive == 4 && RewardDue == true)
                {
                    PickupObject byId = PickupObjectDatabase.GetById(78);
                    LootEngine.SpawnItem(byId.gameObject, user.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
                }
                if (RewardGive == 5 && RewardDue == true)
                {
                    PickupObject ID = PickupObjectDatabase.GetById(600);
                    LootEngine.SpawnItem(ID.gameObject, user.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
                }
                if (RewardGive == 6 && RewardDue == true)
                {
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(127).gameObject, user);
                }

            }
            RewardDue = false;





        }

        private void KillQuest(PlayerController player)
        {
            if(Kill == true)
            {
                KillNum += 1;
                if(KillNum == KillReq)
                {
                    QuestComplete = true;
                    RewardDue = true;
                }
            }
        }

        private void ShopQuest(PlayerController player, ShopItemController shop)
        {
            if (Shop == true)
            {
                ShopNum += 1;
                if (ShopNum == ShopReq)
                {
                    QuestComplete = true;
                    RewardDue = true;
                }
            }
        }

        private void TableQuest(FlippableCover cover)
        {
            if (Table == true)
            {
                TableNum += 1;
                if (TableNum == TableReq)
                {
                    QuestComplete = true;
                    RewardDue = true;
                }
            }
        }



        private void BlankQuest(PlayerController player, int blanks)
        {
            if (Blank == true)
            {
                BlankNum += 1;
                if (BlankNum == BlankReq)
                {
                    QuestComplete = true;
                    RewardDue = true;
                }
            }
        }

        private void ClearQuest(PlayerController player)
        {
            if (Clear == true)
            {
                ClearNum += 1;
                if (ClearNum == ClearReq)
                {
                    QuestComplete = true;
                    RewardDue = true;
                }
            }
        }
        public override void Pickup(PlayerController player)
        {
            if (this.m_pickedUp)
            {
                return;
            }
            base.Pickup(player);
            player.OnItemPurchased += this.ShopQuest;
            player.OnTableFlipped += this.TableQuest;
            player.OnKilledEnemy += this.KillQuest;
            player.OnUsedBlank += this.BlankQuest;
            player.OnRoomClearEvent += this.ClearQuest;
        }
        public DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnItemPurchased -= this.ShopQuest;
            player.OnTableFlipped -= this.TableQuest;
            player.OnKilledEnemy -= this.KillQuest;
            player.OnUsedBlank -= this.BlankQuest;
            player.OnRoomClearEvent -= this.ClearQuest;
          
            debrisObject.GetComponent<Questlog>().m_pickedUpThisRun = true;
            return debrisObject;
        }

        public override bool CanBeUsed(PlayerController user)
        {
            return QuestComplete == true;
        }

        private bool QuestComplete = true;
        private bool Kill = false;
        private bool Shop = false;
        private bool Table = false;
        private bool Ignite = false;
        private bool Blank = false;
        private bool Clear = false;
        private bool RewardDue = false;

        private int KillReq;
        private int ShopReq;
        private int TableReq;
        private int BlankReq;
        private int ClearReq;

        private int KillNum = 0;
        private int ShopNum = 0;
        private int TableNum = 0;
        private int BlankNum = 0;
        private int ClearNum = 0;

        private void Notify(string header, string text)
        {
            tk2dBaseSprite notificationObjectSprite = GameUIRoot.Instance.notificationController.notificationObjectSprite;
            GameUIRoot.Instance.notificationController.DoCustomNotification(header, text, notificationObjectSprite.Collection, notificationObjectSprite.spriteId, UINotificationController.NotificationColor.SILVER, false, false);
        }
    }
}
