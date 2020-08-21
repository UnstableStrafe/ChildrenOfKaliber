using ItemAPI;
using UnityEngine;

namespace Items
{

    class ACMESupply : PlayerItem
    {




        public static void Init()
        {

            string itemName = "ACME Supply Crate";
            string resourceName = "Items/Resources/acme_crate.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<ACMESupply>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Not For Coyotes";
            string longDesc = "Gives a random gun for 12 seconds.\n\nA prop from the old days of humanity's cartoon history.";



            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 600);

            item.consumable = false;
            item.quality = PickupObject.ItemQuality.B;
            item.sprite.IsPerpendicular = true;
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);

        }
        private float duration = 12f;

        

        protected override void DoEffect(PlayerController user)
        {

            StartEffect(user);
            
            AkSoundEngine.PostEvent("Play_OBJ_weapon_pickup_01", base.gameObject);
            StartCoroutine(ItemBuilder.HandleDuration(this, duration, user, EndEffect));
        }


        private void StartEffect(PlayerController user)
        {
            this.CanBeDropped = false;
            this.CanBeSold = false;
            Gun Gun;
            int attempts = 10;
            do
            {
                Gun = PickupObjectDatabase.GetRandomGun();
                attempts--;
            } while ( attempts > 0 && user.HasGun(Gun.PickupObjectId));

            if (attempts == 0)
            {
                Gun = PickupObjectDatabase.GetById(734) as Gun;
            }

            this.m_extantGun = user.inventory.AddGunToInventory(Gun, true);
            this.m_extantGun.CanBeDropped = false;
            this.m_extantGun.CanBeSold = false;
            user.inventory.GunLocked.SetOverride("acme gun", true, null);



        }

        private void EndEffect(PlayerController user)
        {
            user.inventory.GunLocked.RemoveOverride("acme gun");
            user.inventory.DestroyGun(this.m_extantGun);
            this.m_extantGun = null;
            this.CanBeSold = true;
            this.CanBeDropped = true;
        }


        private Gun m_extantGun;
        

    }

    //   var gunList = new List<int>()
    //   {
    //       56, 511, 5, 474, 545, 15, 95, 177, 57, 393, 52, 369, 520, 478, 7, 14, 380, 157, 601, 169, 417, 18, 122, 8, 539, 376, 21, 599, 80, 503, 362, 61, 
    //        341, 124, 481, 541, 357, 647, 328, 200, 390, 223
    //   };


}
