using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace Items
{
    class PiggyBank : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Piggy Bank";

            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<PiggyBank>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Stonks";
            string longDesc = "While held, picking up casings will invest an equal amount into the bank for free. Using the item will destroy it and give the player every invested casing. Taking damage while the item is held will break it, however.\n\n" +
                "Bullet kin are taught to save their money at a young age for when they want to buy a new gun. While this piggy bank has nothing in it right now, it's never too late to start investing!";

            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.PerRoom, 10000);
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;

        }
        bool damaged = false;
        private PlayerController player;
        private void Oof(PlayerController player)
        {
            player.RemoveActiveItem(this.PickupObjectId);
        }
        public void Break()
        {
            this.m_pickedUp = true;
            UnityEngine.Object.Destroy(this.gameObject, 1f);
        }
        private int Money = 0, lastMoney = -1, Diff;
        public override void Update()
        {
            base.Update();
            this.Stonks();
        }
        private void Stonks()
        {
            Money = player.carriedConsumables.Currency;

            if (Money == lastMoney) return;

            Diff = Money - lastMoney;
            if (Diff > 0)
            {
                Stored += Diff;
            }
            lastMoney = Money;
            Diff = 0;
            ETGModConsole.Log("S:" + Stored.ToString());
        }
        protected override void DoEffect(PlayerController user)
        {
            base.DoEffect(user);
            user.carriedConsumables.Currency += Stored;
            user.RemoveActiveItem(this.PickupObjectId);
        }
        public override void Pickup(PlayerController player)
        {

            base.Pickup(player);
            player.OnReceivedDamage += this.Oof;
        }
        public DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<PiggyBank>().m_pickedUpThisRun = true;
            player.OnReceivedDamage -= this.Oof;
            
            return debrisObject;
        }
        private int Stored = 0;
    }
}
