using UnityEngine;
using Alexandria.ItemAPI;

namespace Items
{
    class PetRock : PassiveItem
    {


       // private static readonly string[] spritePaths =
      //  {
      //      "Items/Resources/pet_rock.png",
     //       "Items/Resources/pet_rock_blink.png",
     //   };

       // private static int[] spriteIDs;

        public static void Init()
        {


            string itemName = "Pet Rock";
            string resourceName = "Items/Resources/ItemSprites/Passives/pet_rock.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<PetRock>();
            //spriteIDs = new int[spritePaths.Length];

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
           // spriteIDs[0] = item.sprite.spriteId;
           // spriteIDs[1] = SpriteBuilder.AddSpriteToCollection(spritePaths[1], item.sprite.Collection);


            string shortDesc = "He Believes in You";
            string longDesc = "Gives one coolness and provides emotional support.\n\n" +
                    "For some reason, you know that you can trust this rock.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ck");

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Coolness, 3);


            item.quality = PickupObject.ItemQuality.D;
            item.sprite.IsPerpendicular = true;
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
        }
        private void RoomCleared(PlayerController obj)
        {
            
            MotivateCh = 2;
            int ValRocc = 1;
            if(ValRocc < MotivateCh)
            {
                string header = "";
                string text = "";
                int cursedsay = UnityEngine.Random.Range(0, 101);
                if (cursedsay <= 99)
                {
                    int Saying = UnityEngine.Random.Range(0, 5);
                    if (Saying == 0)
                    {
                        header = "Pet rock cheers you on";

                        text = "You can do it!";


                        
                    }
                    if (Saying == 1)
                    {
                        header = "Pet rock cheers you on";
                        
                        text = "Good job!";
                        
                        
                    }
                    if (Saying == 2)
                    {
                        header = "Pet rock cheers you on";
                       
                        text = "I believe in you!";
                      
                        
                    }
                    if (Saying == 3)
                    {
                        header = "Pet rock cheers you on";
                        
                        text = "You did great!";
                     
                        
                    }
                    if (Saying == 4)
                    {
                        header = "Pet rock cheers you on";
                       
                        text = "Well done!";
                       
                    }

                }
                if (cursedsay >= 100)
                {
                    
                    header = "Ṫ̶͖̫̺͖̞̹͚̫̍͐̕ḩ̵̲̬̥̭̬́̋͜ͅe̷̹̤̺͋ ̸̧͉̻͈̣͚̜̹̹̣͋́̓̊̂̓̐p̷̛̬̃̈́̊͘͝a̶̗͖̰̜̰̤̹̩̹͂̒̃̏̈́͆̕č̶̛͙͕͎̘̭̩̤͕̓̌̕͝ţ̵̢̛̥̹̦̤̝̫̪̼͍̄͂̆͒̀̔́ ̷̨͓̩̤̼͉̉̃̆́͒́h̵͖̣͉̘͓͖͚̗̽̿̀̑̏̊̐̽̈́͘á̴͉͈̖̟̲̫̏ͅs̸̢͎̭͆̎͠ ̵̩̗̥̥̈̈́̌b̶̳̲͙͇͍̭͇̏ͅȩ̵̢̲̯̗͂̿̂̄̉̒́̏͊͝é̴͍͇͔̲͎̀̊̾͒̅̒ṋ̴̰̟͇̣͉̄͋̈̀͑̽̊͘͜͜ ̴̜̝͌̀̀̾̋̓̀̈́̀̿̕s̴̘̻̬̓͂̒̀͝͠ȇ̷͍̥̝̬̫͎̰̉̋̏̆̏͌ͅả̶̖͊̀̿l̸̡̡̛̺͖̫̰͛̓̐͌̾̄͘ͅḛ̸̢̞̟͕͂̐͘̚d̷̢͚̟͔͉͔͔͗́̈̂̽̏ͅ";

                    text = "Your soul is mine!";

                   
                    
                    AkSoundEngine.PostEvent("Play_ENM_reaper_spawn_01", gameObject);
                }
                
                this.Notify(header, text);
            }
            
        }

        public override void Pickup(PlayerController player)
        {
            if (this.m_pickedUp)
            {
                return;
            }
            player.OnRoomClearEvent += this.RoomCleared;
            base.Pickup(player);

        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnRoomClearEvent -= this.RoomCleared;
            debrisObject.GetComponent<PetRock>().m_pickedUpThisRun = true;
            return debrisObject;
        }
        

        private void Notify(string header, string text)
        {
            tk2dSpriteCollectionData IconCollection = AmmonomiconController.Instance.EncounterIconCollection;
            int spriteID = IconCollection.GetSpriteIdByName("Items/Resources/pet_rock");
            GameUIRoot.Instance.notificationController.DoCustomNotification(header, text, IconCollection, spriteID, UINotificationController.NotificationColor.SILVER, false, false);
        }

        //int id;
      /*  private void Blink()
        {
            float ValBlink = UnityEngine.Random.value;
            if(ValBlink < blinkCh)
            {
                id = spriteIDs[1];
                
            }
        }
        */


        private float MotivateCh;
       // private float blinkCh;
    }

}
