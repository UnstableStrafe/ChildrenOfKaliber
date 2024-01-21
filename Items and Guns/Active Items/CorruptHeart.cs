using System.Collections.Generic;
using Alexandria.ItemAPI;
using UnityEngine;
using Dungeonator;
using System.Linq;
using System;
using System.Reflection;
namespace Items
{
    class CorruptHeart : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Corrupt Heart";

            string resourceName = "Items/Resources/ItemSprites/Actives/corrupt_heart.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<CorruptHeart>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Beat of Death";
            string longDesc = "Deals moderate damage to all enemies in the room. \n\nBa-dum... ba-dum... ba-dum... ba-dum.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ck");

            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 10f);

            item.consumable = false;
            item.quality = ItemQuality.C;
            item.sprite.IsPerpendicular = true;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);

        }

        private PlayerController player;
        public override void DoEffect(PlayerController user)
        {
            AkSoundEngine.PostEvent("Play_WPN_seriouscannon_shot_01", base.gameObject);
            EnemyListing(user);
            player = user;

            

        }
       
        private void DebugLogging()            
        {
            RoomHandler room = player.CurrentRoom;
            float x = room.GetBoundingRect().width;
            float y = room.GetBoundingRect().height;
            ETGModConsole.Log("Room X = " + x.ToString());
            ETGModConsole.Log("Room Y = " + y.ToString());
        }

        private void EnemyListing(PlayerController user)
        {
            RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
            List<AIActor> activeEnemies = absoluteRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
           
            bool flag = activeEnemies != null;
            if (flag)
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    this.AffectEnemy(activeEnemies[i], user);
                }
            }
        }
        protected void AffectEnemy(AIActor target, PlayerController user)
        {
            bool flag = target.IsNormalEnemy || target.healthHaver.IsBoss && !target.IsHarmlessEnemy;
            if (flag)
            {

                target.healthHaver.ApplyDamage(20f, Vector2.zero, "Heart of Doom", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
              
                if (user.HasPickupID(379))
                {
                    target.healthHaver.OnDeath += this.LoveLeech;
                    

                }
            }
            
        }

      
        private void LoveLeech(Vector2 idk)
        {
            float f = .05f;
            if(UnityEngine.Random.value < f)
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(73).gameObject, player);
            }
        }

    }
}

