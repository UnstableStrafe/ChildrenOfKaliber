using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using Dungeonator;
using UnityEngine;
using System.Collections;
using ItemAPI;

namespace Items
{
    class TrickstersKnife : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Trickster's Knife";

            string resourceName = "Items/Resources/trickster_knife.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<TrickstersKnife>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "A Memento";
            string longDesc = "Use on a boss with less than 1/3 health remaining for a devastating finale!\n\nA knife once wielded by a powerful Trickster who sought defiance against those who abused their powers. A potent energy thrums from within it." +
                "\n\nIt's time for an all out attack!";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 2, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 1);
            ItemBuilder.AddToSubShop(item, ItemBuilder.ShopType.Cursula);

            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;


        }
        public override void Pickup(PlayerController player)
        {

            base.Pickup(player);

        }
        protected override void DoEffect(PlayerController user)
        {
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
            bool flag = target.healthHaver.GetCurrentHealthPercentage() < .3f;
            if (flag)
            {
                target.healthHaver.ApplyDamage(10000, Vector2.zero, "die", CoreDamageTypes.None, DamageCategory.Normal, false, null, true);
            }
        }
        

        public DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<TrickstersKnife>().m_pickedUpThisRun = true;

            return debrisObject;
        }
        
    }
}
