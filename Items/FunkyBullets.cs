using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;


namespace Items
{
    public class FunkyBullets : PassiveItem
    {
        public static void Init()
        {
            
            string itemName = "Funky Bullets";
            string resourceName = "Items/Resources/funky_bullets.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<FunkyBullets>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Who Shot Jonny?";
            string longDesc = "Makes bullet stats weird.\n\n" +
                "A horribly failed attempt at forging a bullet that could kill the past. The creator threw them into the Gungeon some time later, where they remained until your grubby little hands put them in a magazine.";   

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ProjectileSpeed, .85f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Accuracy, .90f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, .90f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ReloadSpeed, .7f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.RateOfFire, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalShotBounces, 1);

            item.quality = PickupObject.ItemQuality.C;
            item.sprite.IsPerpendicular = true;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
        }
        public override void Pickup(PlayerController player)
        {
            if (this.m_pickedUp)
            {
                return;
            }
            base.Pickup(player);
          //  player.OnReloadedGun += FunkyFresh;
        }
     /*   private void FunkyFresh(PlayerController player, Gun gun)
        {
            bool flag = player.HasPickupID(119) || player.HasPickupID(149) || player.HasPickupID(702) || player.HasPickupID(482) || player.HasPickupID(506);
            if(gun.ClipShotsRemaining == 0 && flag)
            {
                //spawn a burst of music notes on reload
            }
        }
        */
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<FunkyBullets>().m_pickedUpThisRun = true;
            //player.OnReloadedGun -= FunkyFresh;
            return debrisObject;
        }
    }
}