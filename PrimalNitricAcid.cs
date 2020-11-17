using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;
using Dungeonator;

namespace Items
{
    class PrimalNitricAcid : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Primal Nitric Acid";

            string resourceName = "Items/Resources/primal_nitric_acid.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<PrimalNitricAcid>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Don't Ask How It's Made";
            string longDesc = "Shots can douse enemies in nitric acid, inflicting damage over time and creaing a pool of nitric acid on death.\n\nHarvested from the first bullat to be spawned in the Gungeon, teeming with energy from the Bullet.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.A;
            item.sprite.IsPerpendicular = true;
            
        }
        private void PostProj(Projectile projectile, float eff)
        {
            projectile.AppliesPoison = true;
            projectile.PoisonApplyChance = .33f;
            projectile.healthEffect = Library.NitricAcid;   
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProj;
            WeightedGameObject Object1 = new WeightedGameObject
            {
                pickupId = ETGMod.Databases.Items["Primal Sulfur"].PickupObjectId,
                weight = 1.7f,
                rawGameObject = ETGMod.Databases.Items["Primal Sulfur"].gameObject,
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
                pickupId = ETGMod.Databases.Items["Primal Saltpeter"].PickupObjectId,
                weight = 1.7f,
                rawGameObject = ETGMod.Databases.Items["Primal Saltpeter"].gameObject,
                forceDuplicatesPossible = false
            };
            if (!player.HasPickupID(ETGMod.Databases.Items["Primal Sulfur"].PickupObjectId))
            {
                GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements.Add(Object1);
            }
            if (!player.HasPickupID(ETGMod.Databases.Items["Primal Charcoal"].PickupObjectId))
            {
                GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements.Add(Object2);
            }
            if (!player.HasPickupID(ETGMod.Databases.Items["Primal Saltpeter"].PickupObjectId))
            {
                GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements.Add(Object3);
            }
            if (player.HasPickupID(ETGMod.Databases.Items["Primal Saltpeter"].PickupObjectId) && player.HasPickupID(ETGMod.Databases.Items["Primal Sulfur"].PickupObjectId) && player.HasPickupID(ETGMod.Databases.Items["Primal Charcoal"].PickupObjectId) && !player.HasPickupID(ETGMod.Databases.Items["True Gunpowder"].PickupObjectId))
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(ETGMod.Databases.Items["True Gunpowder"].PickupObjectId).gameObject, player);
            }
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<PrimalNitricAcid>().m_pickedUpThisRun = true;
            player.PostProcessProjectile -= this.PostProj;
            return debrisObject;
        }
    }
}
