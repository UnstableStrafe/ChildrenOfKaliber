using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;
using Gungeon;
using Dungeonator;

namespace Items
{
    class IsharsSoul : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Ishar's Soul";

            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<IsharsSoul>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Welcome To The Neotheater";
            string longDesc = "Lowers all stats by 3%. Cannot be dropped.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            item.consumable = true;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.PerRoom, 9999);

            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;


        }
        protected override void DoEffect(PlayerController user)
        {
            base.DoEffect(user);
            BossFinder(user);

           // StartCoroutine(ItemBuilder.HandleDuration(this, 1, user, BossCleaver));
            
            
        }

        private void BossFinder(PlayerController user)
        {
            RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
            List<AIActor> activeEnemies = absoluteRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            bool flag = activeEnemies != null;
            if (flag)
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    this.DupeBosses(activeEnemies[i]);
                    
                }
            }
        }
        private void BossCleaver(PlayerController user)
        {
            RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
            List<AIActor> activeEnemies = absoluteRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            bool flag = activeEnemies != null;
            if (flag)
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    this.CleaveBosses(activeEnemies[i]);

                }
            }
        }
        private PlayerController player;
        protected void DupeBosses(AIActor target)
        {
            if(target.healthHaver.IsBoss)
            {
                AIActor target2 = target;
                AIActor.Spawn(target2, target.transform.position, player.CurrentRoom, true, AIActor.AwakenAnimationType.Default, true);
            }
            
        }
        protected void CleaveBosses(AIActor target)
        {
            if (target.healthHaver.IsBoss)
            {
                target.healthHaver.ApplyDamage((target.healthHaver.GetMaxHealth() * .4f), Vector2.zero, "Cleaving Bosses", CoreDamageTypes.None, DamageCategory.Normal, true, null, true);
            }
        }
    }
}
