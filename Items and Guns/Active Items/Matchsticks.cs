using Alexandria.ItemAPI;
using UnityEngine;

namespace Items
{
    class Matchsticks : PlayerItem
    {
        public static void Init()
        {

            string itemName = "Matchsticks";
            string resourceName = "Items/Resources/ItemSprites/Actives/matchsticks.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<Matchsticks>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Smokey Frowns Upon You";
            string longDesc = "Shoots a flare in the direction of the cursor on use.\nCan be used 10 times before needing to be recharged.\n\nIn order to use fire as a weapon, one must throw away years of safety training.";


            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ck");
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 350);

            item.UsesNumberOfUsesBeforeCooldown = true;
            item.numberOfUses = 10;
            item.consumable = false;
            item.quality = PickupObject.ItemQuality.C;
            item.sprite.IsPerpendicular = true;
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);


        }
        
        public override void DoEffect(PlayerController user)
        {

            Projectile projectile2 = ((Gun)ETGMod.Databases.Items[275]).DefaultModule.projectiles[0];
            Vector3 vector = user.unadjustedAimPoint - user.LockedApproximateSpriteCenter;
            Vector3 vector2 = user.specRigidbody.UnitCenter;
            GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, user.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (user.CurrentGun == null) ? 0f : user.CurrentGun.CurrentAngle), true);
            Projectile component = gameObject.GetComponent<Projectile>();

            if (component != null)
            {
                component.Owner = user;
                component.Shooter = user.specRigidbody;
            }

        }
    }
}
