using ItemAPI;
using UnityEngine;

namespace Items
{
    class VenomSpitDrone : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Venom Spit Drone";

            string resourceName = "Items/Resources/venom_spit_drone.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<VenomSpitDrone>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "D-10";
            string longDesc = "Shoots beams of venom towards enemies.\n\nTenth of Isaac Fogcutter's trifiling travels of tinkering tenacious turrets.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            item.quality = ItemQuality.A;
            item.sprite.IsPerpendicular = true;

        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            WeightedGameObject Object1 = new WeightedGameObject
            {
                pickupId = ETGMod.Databases.Items["Trigger Pulse Drone"].PickupObjectId,
                weight = 1.6f,
                rawGameObject = ETGMod.Databases.Items["Trigger Pulse Drone"].gameObject,
                forceDuplicatesPossible = false
            };
            WeightedGameObject Object2 = new WeightedGameObject
            {
                pickupId = ETGMod.Databases.Items["Little Drone Buddy"].PickupObjectId,
                weight = 1.6f,
                rawGameObject = ETGMod.Databases.Items["Little Drone Buddy"].gameObject,
                forceDuplicatesPossible = false
            };
            if (!player.HasPickupID(ETGMod.Databases.Items["Trigger Pulse Drone"].PickupObjectId))
            {
                GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements.Add(Object1);
            }
            if (!player.HasPickupID(ETGMod.Databases.Items["Little Drone Buddy"].PickupObjectId))
            {
                GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements.Add(Object2);
            }
            if (player.HasPickupID(ETGMod.Databases.Items["Trigger Pulse Drone"].PickupObjectId) && player.HasPickupID(ETGMod.Databases.Items["Little Drone Buddy"].PickupObjectId))
            {
                player.inventory.AddGunToInventory(ETGMod.Databases.Items["d.e.a.t.h."] as Gun, true);
            }
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<VenomSpitDrone>().m_pickedUpThisRun = true;

            return debrisObject;
        }
    }
}

