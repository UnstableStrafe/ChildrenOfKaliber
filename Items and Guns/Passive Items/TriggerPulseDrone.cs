using Alexandria.ItemAPI;
using UnityEngine;
namespace Items
{
    class TriggerPulseDrone : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Trigger Pulse Drone";

            string resourceName = "Items/Resources/ItemSprites/Passives/trigger_drone.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<TriggerPulseDrone>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "D-7";
            string longDesc = "Attacks enemies passively. Grants 2 coolness.\n\nSeventh of Isaac Fogcutter's setoff into specialized spacial strike sentries sent seeking stupid suckers.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Coolness, 2, StatModifier.ModifyMethod.ADDITIVE);
            item.quality = ItemQuality.B;
            item.sprite.IsPerpendicular = true;

        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            WeightedGameObject Object1 = new WeightedGameObject
            {
                pickupId = ETGMod.Databases.Items["Little Drone Buddy"].PickupObjectId,
                weight = 1.6f,
                rawGameObject = ETGMod.Databases.Items["Little Drone Buddy"].gameObject,
                forceDuplicatesPossible = false
            };
            WeightedGameObject Object2 = new WeightedGameObject
            {
                pickupId = ETGMod.Databases.Items["Venom Spit Drone"].PickupObjectId,
                weight = 1.7f,
                rawGameObject = ETGMod.Databases.Items["Venom Spit Drone"].gameObject,
                forceDuplicatesPossible = false
            };
            if (!player.HasPickupID(ETGMod.Databases.Items["Little Drone Buddy"].PickupObjectId))
            {
                GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements.Add(Object1);
            }
            if (!player.HasPickupID(ETGMod.Databases.Items["Venom Spit Drone"].PickupObjectId))
            {
                GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements.Add(Object2);
            }
            if (player.HasPickupID(ETGMod.Databases.Items["Little Drone Buddy"].PickupObjectId) && player.HasPickupID(ETGMod.Databases.Items["Venom Spit Drone"].PickupObjectId))
            {
                player.inventory.AddGunToInventory(ETGMod.Databases.Items["d.e.a.t.h."] as Gun, true);
            }
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<TriggerPulseDrone>().m_pickedUpThisRun = true;

            return debrisObject;
        }
    }
}
