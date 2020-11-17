using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ItemAPI;
using UnityEngine;

namespace Items
{
    class D6 : PlayerItem
    {
        public static void Init()
        {

            string itemName = "D6";
            string resourceName = "Items/Resources/d6.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<D6>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Isaac And His Mother";
            string longDesc = "Rerolls the currently held gun.\n\nA small die used for tabletop roleplaying games. It seems to be enchanted.";


            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            item.quality = PickupObject.ItemQuality.A;
            item.sprite.IsPerpendicular = true;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 1000f);
            item.consumable = false;
        }
        Gun GunToRoll;
        Gun newGun;
        private static void RandomizeRGG(PlayerController player)
        {
            if (player.HasPickupID(ETGMod.Databases.Items["R.G.G."].PickupObjectId))
            {
                RGG.RandomizeStats();
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
           // player.OnNewFloorLoaded += RandomizeRGG;
        }
        public DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            //player.OnNewFloorLoaded -= RandomizeRGG;
            return debrisObject;
        }
        protected override void DoEffect(PlayerController user)
        {
            base.DoEffect(user);
            GunToRoll = user.CurrentGun;
            newGun = PickupObjectDatabase.GetRandomGun();
            user.inventory.GunLocked.SetOverride("Rerolling Gun via d6", true, null);
            user.inventory.DestroyGun(GunToRoll);
            AkSoundEngine.PostEvent("Play_OBJ_Chest_Synergy_Slots_01", base.gameObject);
            StartCoroutine(ItemBuilder.HandleDuration(this, 1.5f, user, EndEffect));
        }
        private void EndEffect(PlayerController player)
        {
            player.inventory.AddGunToInventory(newGun, true);
            AkSoundEngine.PostEvent("Play_OBJ_Chest_Synergy_Win_01", base.gameObject);
            player.inventory.GunLocked.RemoveOverride("Rerolling Gun via d6");
        }
        public override bool CanBeUsed(PlayerController user)
        {
            return !user.CurrentGun.InfiniteAmmo;
        }
    }
}
