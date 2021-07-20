using System;
using UnityEngine;
using ItemAPI;
using Dungeonator;
namespace Items
{
    class PrimalSulfur : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Primal Sulfur";

            string resourceName = "Items/Resources/primal_sulfur.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<PrimalSulfur>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Smells Nice";
            string longDesc = "Upon entering a room up to 4 random enemies are marked. Marked enemies explode on death. These explosions do not harm the player.\n\nIt is said this sulfur was enchanted with the energy of raw firepower.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.B;
            item.sprite.IsPerpendicular = true;
        }
        private void MarkEnemies()
        {
            int i = 0;
            int capper = 0;

            AIActor actor;
            RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
            do
            {
                actor = Owner.CurrentRoom.GetRandomActiveEnemy(false);
                actor.ApplyEffect(Library.SulfurEffect);
                i++;
            }
            while (i < 4);
            
            
        }
        public override void Pickup(PlayerController player)
        {
            player.OnEnteredCombat = (Action)Delegate.Combine(player.OnEnteredCombat, new Action(this.MarkEnemies));
            WeightedGameObject Object1 = new WeightedGameObject
            {
                pickupId = ETGMod.Databases.Items["Primal Saltpeter"].PickupObjectId,
                weight = 1.7f,
                rawGameObject = ETGMod.Databases.Items["Primal Saltpeter"].gameObject,
                forceDuplicatesPossible = false
            };
            WeightedGameObject Object2 = new WeightedGameObject
            {
                pickupId = ETGMod.Databases.Items["Primal Charcoal"].PickupObjectId,
                weight = 1.7f,
                rawGameObject = ETGMod.Databases.Items["Primal Charcoal"].gameObject,
                forceDuplicatesPossible = false
            };
            WeightedGameObject Object3 = new WeightedGameObject
            {
                pickupId = ETGMod.Databases.Items["Primal Nitric Acid"].PickupObjectId,
                weight = 1.8f,
                rawGameObject = ETGMod.Databases.Items["Primal Nitric Acid"].gameObject,
                forceDuplicatesPossible = false
            };
            if (!player.HasPickupID(ETGMod.Databases.Items["Primal Saltpeter"].PickupObjectId))
            {
                GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements.Add(Object1);
            }
            if (!player.HasPickupID(ETGMod.Databases.Items["Primal Charcoal"].PickupObjectId))
            {
                GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements.Add(Object2);
            }
            if (!player.HasPickupID(ETGMod.Databases.Items["Primal Nitric Acid"].PickupObjectId))
            {
                GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements.Add(Object3);
            }
            if (player.HasPickupID(ETGMod.Databases.Items["Primal Saltpeter"].PickupObjectId) && player.HasPickupID(ETGMod.Databases.Items["Primal Nitric Acid"].PickupObjectId) && player.HasPickupID(ETGMod.Databases.Items["Primal Charcoal"].PickupObjectId) && !player.HasPickupID(ETGMod.Databases.Items["True Gunpowder"].PickupObjectId))
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(ETGMod.Databases.Items["True Gunpowder"].PickupObjectId).gameObject, player);
            }
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<PrimalSulfur>().m_pickedUpThisRun = true;
            player.OnEnteredCombat = (Action)Delegate.Remove(player.OnEnteredCombat, new Action(this.MarkEnemies));
            return debrisObject;
        }
    }
}
