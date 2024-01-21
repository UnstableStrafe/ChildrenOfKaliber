using Alexandria.ItemAPI;
using UnityEngine;

namespace Items
{
    class LittleDroneBuddy : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Little Drone Buddy";

            string resourceName = "Items/Resources/ItemSprites/Passives/little_drone.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<LittleDroneBuddy>();
            
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "D-1";
            string longDesc = "Shoots automatically at nearby enemies while reloading. Slightly increases reload speed.\n\nFirst of Isaac Fogcutter's foray into friendly flying fellows fashioned for fighting foes.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ck");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ReloadSpeed, 1.15f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.quality = ItemQuality.C;
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
                pickupId = ETGMod.Databases.Items["Venom Spit Drone"].PickupObjectId,
                weight = 1.7f,
                rawGameObject = ETGMod.Databases.Items["Venom Spit Drone"].gameObject,
                forceDuplicatesPossible = false
            };
            if (!player.HasPickupID(ETGMod.Databases.Items["Trigger Pulse Drone"].PickupObjectId))
            {
                GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements.Add(Object1);
            }
            if (!player.HasPickupID(ETGMod.Databases.Items["Venom Spit Drone"].PickupObjectId))
            {
                GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements.Add(Object2);
            }
            if (player.HasPickupID(ETGMod.Databases.Items["Trigger Pulse Drone"].PickupObjectId) && player.HasPickupID(ETGMod.Databases.Items["Venom Spit Drone"].PickupObjectId))
            {
                player.inventory.AddGunToInventory(ETGMod.Databases.Items["d.e.a.t.h."] as Gun, true);
            }
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<LittleDroneBuddy>().m_pickedUpThisRun = true;
            return debrisObject;
        }
    }
}
