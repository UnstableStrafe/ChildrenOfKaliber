using UnityEngine;
using ItemAPI;

namespace Items
{
    class SteamSale : PassiveItem
    {

        public SteamSale()
        {
            this.Purchases = 0;
        }


        public static void Init()
        {


            string itemName = "Steam Sale";
            string resourceName = "Items/Resources/steam_sale.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<SteamSale>();
            

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);


            string shortDesc = "Breaking The 4th Wall!";
            string longDesc = "Halves shop prices, but is destroyed after 6 purchases.\n\n" +
                    "By holding this, you somehow feel worthless.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.GlobalPriceMultiplier, .5f, StatModifier.ModifyMethod.MULTIPLICATIVE);


            item.quality = PickupObject.ItemQuality.B;
            item.sprite.IsPerpendicular = true;
        }

        public override void Pickup(PlayerController player)
        {
            if (this.m_pickedUp)
            {
                return;
            }
            base.Pickup(player);
            player.OnItemPurchased += this.PurchaseCount;
        }
        public void Break()
        {
            this.m_pickedUp = true;
            UnityEngine.Object.Destroy(base.gameObject, 1f);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject drop = base.Drop(player);
            SteamSale thingy = drop.GetComponent<SteamSale>();
            player.OnItemPurchased -= this.PurchaseCount;
            thingy.m_pickedUpThisRun = true;
            if(Purchases == 5)
            {
                thingy.Break();
            }
            
            return drop;
            
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void PurchaseCount(PlayerController player, ShopItemController shop)
        {
            if(Purchases < 5)
            {
                Purchases += 1;
            }

            if(Purchases == 5)
            {
                player.DropPassiveItem(this);
            }
        }

        private int Purchases;
    }
}
