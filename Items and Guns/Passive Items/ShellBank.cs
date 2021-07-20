using ItemAPI;
using UnityEngine;
using System.Collections;
namespace Items
{
    class ShellBank : PassiveItem
    {

        public static void Init()
        {
            string itemName = "Shell Bank";

            string resourceName = "Items/Resources/shell_bank.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<ShellBank>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "retoStonks";
            string longDesc = "While held, all collected casings will be banked. Dropping the item will destroy it and give the player the stored currency, with 2x intrest! Taking damage while the item is held will reduce the stored currency by 15%.\n\n" +
                "Bullet kin are taught to save their money at a young age for when they want to buy a new gun. While this coin bank has nothing in it right now, it's never too late to start investing!";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.C;
            item.sprite.IsPerpendicular = true;

        }
        private void Oof(PlayerController player)
        {
            Stored = Mathf.FloorToInt(Stored * .85f);
        }
        protected override void Update()
        {
            base.Update();
            if(Baseline != Owner.carriedConsumables.Currency)
            {
                int num1 = Owner.carriedConsumables.Currency - Baseline;
                Owner.carriedConsumables.Currency -= num1;
                Stored += num1;
            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnReceivedDamage += this.Oof;
            player.OnItemPurchased += Player_OnItemPurchased;
            player.gameObject.AddComponent<RiskStat>();
            Baseline = player.carriedConsumables.Currency;
        }

        private void Player_OnItemPurchased(PlayerController player, ShopItemController arg2)
        {
            Baseline = player.carriedConsumables.Currency;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            ShellBank item = debrisObject.gameObject.GetComponent<ShellBank>();
            
            debrisObject.GetComponent<ShellBank>().m_pickedUpThisRun = true;
            player.OnReceivedDamage -= this.Oof;
            if (Stored > 0)
            {
                LootEngine.SpawnCurrency(player.specRigidbody.UnitCenter, Stored * 2);
                Destroy(this.gameObject, 1f);

                //Make cool spell guns yo ufucking nerd!!

            }  
            return debrisObject;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        private int Stored = 0;
        private int Baseline;
    }
}
