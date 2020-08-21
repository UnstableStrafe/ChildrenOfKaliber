using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;
using Dungeonator;

namespace Items
{
    class CorruptHeart : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Corrupt Heart";

            string resourceName = "Items/Resources/corrupt_heart.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<CorruptHeart>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Beat of Death";
            string longDesc = "Deals moderate damage to all enemies in the room. \n\nBa-dum... ba-dum... ba-dum... ba-dum.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 10f);

            item.consumable = false;
            item.quality = ItemQuality.C;
            item.sprite.IsPerpendicular = true;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);

        }


        protected override void DoEffect(PlayerController user)
        {
            AkSoundEngine.PostEvent("Play_WPN_seriouscannon_shot_01", base.gameObject);
            EnemyListing();
        }

        private void EnemyListing()
        {
            RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
            List<AIActor> activeEnemies = absoluteRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            bool flag = activeEnemies != null;
            if (flag)
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    this.AffectEnemy(activeEnemies[i]);
                }
            }
        }
        protected void AffectEnemy(AIActor target)
        {
            bool flag = target.IsNormalEnemy || target.healthHaver.IsBoss && !target.IsHarmlessEnemy;
            if (flag)
            {
                target.healthHaver.ApplyDamage(20f, Vector2.zero, "Heart of Doom", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
            }

            
        }



    }
}

