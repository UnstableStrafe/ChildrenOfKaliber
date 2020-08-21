using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Dungeonator;

namespace Items
{
    class SuperCrate : PlayerItem
    {
       
        public static void Init()
        {

            //UFF GILDED HYDRA
            //BUFF GILDED HYDRA
            //BUFF GILDED HYDRA
            //BUFF TURBO GUN
            string itemName = "Suspiscious Strongbox";
            string resourceName = "Items/Resources/strang_strongbox.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<SuperCrate>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "All That Glitters is Rainbow";
            string longDesc = "Spawns a Rainbow Chest!\n\n" +
                "A lockbox covered in dozens of seals and runes, each details a different ritual or sacrifice. The name 'Pandora' can be seen carved on the lid.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
         

            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 7500);

           
            item.consumable = true;
            item.quality = PickupObject.ItemQuality.S;
            item.sprite.IsPerpendicular = true;
            item.AddToSubShop(ItemBuilder.ShopType.Flynt);

        }

       // private bool HasBeenPickedUp;

        public override void Pickup(PlayerController player)
        {
            if (this.m_pickedUp)
            {
                
                return;
            }

            
            if (m_pickedUpThisRun == false)
            {
                base.ApplyCooldown(player);
                
            }
            base.Pickup(player);

        }

        protected override void DoEffect(PlayerController user)
        {
            IntVector2 bestRewardLocation = user.CurrentRoom.GetBestRewardLocation(IntVector2.One * 3, RoomHandler.RewardLocationStyle.PlayerCenter, true);
            Chest chest = GameManager.Instance.RewardManager.Rainbow_Chest;
          
            chest.IsLocked = false;
            Chest.Spawn(chest, bestRewardLocation);
            base.DoEffect(user);
        }

        public DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<SuperCrate>().m_pickedUpThisRun = true;
            return debrisObject;
        }
    }
}
