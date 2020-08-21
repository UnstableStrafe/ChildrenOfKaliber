using Gungeon;
using ItemAPI;
using UnityEngine;

namespace Items
{
    class BananaJamHands : PassiveItem
    {
        public static void Init()
        {

            string itemName = "Banana Jam Hands";
            string resourceName = "Items/Resources/banana_jam_hands.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<BananaJamHands>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Icky";
            string longDesc = "Reloading throws a gun.\n\nHow do you expect to hold a gun with all this jam on your hands?";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            item.quality = PickupObject.ItemQuality.D;
            item.sprite.IsPerpendicular = true;
            item.AddToSubShop(ItemBuilder.ShopType.Goopton);
        }

        private void Toss(PlayerController player, Gun gun)
        {
            if (player.CurrentGun.ClipShotsRemaining == 0)
            {
                Gun gun1;
                Projectile projectile2 = ((Gun)ETGMod.Databases.Items[503]).DefaultModule.projectiles[0];
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
                Projectile component = gameObject.GetComponent<Projectile>();

                if (component != null)
                {
                    component.Owner = base.Owner;
                    component.Shooter = base.Owner.specRigidbody;
                    component.baseData.speed = 25f;
                    component.baseData.damage = 7f;


                }
            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnReloadedGun += this.Toss;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debris = base.Drop(player);
            player.OnReloadedGun -= this.Toss;
            debris.GetComponent<BananaJamHands>().m_pickedUpThisRun = true;
            return debris;
        }
    }
}
