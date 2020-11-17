using System;
using System.Reflection;
using Gungeon;
using ItemAPI;
using UnityEngine;
using MonoMod;
using MonoMod.RuntimeDetour;
using System.Collections;
namespace Items
{
    class ShellBank : PlayerItem
    {
        //Hook currencyPickupHook = new Hook
        //    (
        //        typeof(CurrencyPickup).GetMethod("Pickup", BindingFlags.Instance | BindingFlags.Public),
        //        typeof(ShellBank).GetMethod("currencyPickupHookMethod")
        //    );
        //public static int bankedCurrency;
        //public static void currencyPickupHookMethod(Action<CurrencyPickup, PlayerController> orig, CurrencyPickup self, PlayerController player)
        //{
        //    try
        //    {
        //        if ((player.HasPickupID(Gungeon.Game.Items["cel:shell_bank"].PickupObjectId)) && (self.currencyValue > 0) && !self.IsMetaCurrency)
        //        {
        //            bankedCurrency += self.currencyValue;
        //        }
        //    }catch(Exception e)
        //    {
        //        ETGModConsole.Log(e.ToString());
        //    }
            
        //}
        public static void Init()
        {
            string itemName = "Shell Bank";

            string resourceName = "Items/Resources/shell_bank.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<ShellBank>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "retoStonks";
            string longDesc = "While held, all collected casings will be banked. Using the item will destroy it and give the player the stored currency, with 2x intrest! Taking damage while the item is held will break it, however.\n\n" +
                "Bullet kin are taught to save their money at a young age for when they want to buy a new gun. While this coin bank has nothing in it right now, it's never too late to start investing!";

            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.PerRoom, 10000);
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.C;
            item.sprite.IsPerpendicular = true;

        }
        private void Oof(PlayerController player)
        {
            player.RemoveActiveItem(this.PickupObjectId);
           // bankedCurrency = 0;
        }
        public void Break()
        {
            this.m_pickedUp = true;
            UnityEngine.Object.Destroy(this.gameObject, 1f);
        }
        protected override void DoEffect(PlayerController user)
        {
            base.DoEffect(user);
          //  user.carriedConsumables.Currency += bankedCurrency * 2;
            user.RemoveActiveItem(this.PickupObjectId);
          //  bankedCurrency = 0;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnReceivedDamage += this.Oof;
        }
        public DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<ShellBank>().m_pickedUpThisRun = true;
            player.OnReceivedDamage -= this.Oof;
            
            return debrisObject;
        }
        private int Stored = 0;
    }
}
