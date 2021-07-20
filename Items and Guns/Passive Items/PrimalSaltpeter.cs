using System;
using UnityEngine;
using ItemAPI;
using Dungeonator;
using System.Linq;
using System.Collections.Generic;
namespace Items
{
    class PrimalSaltpeter : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Primal Saltpeter";

            string resourceName = "Items/Resources/primal_saltpeter.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<PrimalSaltpeter>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Niter Is Cooler!";
            string longDesc = "Purifies non-boss enemy elemental resistances.\n\nOne of the first resources to ever be harvested in the Black Powder Mines.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.D;
            item.sprite.IsPerpendicular = true;
        }
        protected override void Update()
        {
            base.Update();
        }
        
        private void EnergyBending(AIActor idiotstick)
        {
            bool flag = idiotstick.healthHaver && !idiotstick.healthHaver.IsBoss && idiotstick.isActiveAndEnabled;
            if (flag)
            {
                GameActor actor = idiotstick.gameActor;
                
                actor.EffectResistances = new ActorEffectResistance[]
                {
                    new ActorEffectResistance()
                    {
                        resistAmount = 0,
                        resistType = EffectResistanceType.Poison,
                        
                    },
                    new ActorEffectResistance()
                    {
                        resistAmount = 0,
                        resistType = EffectResistanceType.Fire
                    },
                };
                
                
            }

        }
        private DamageTypeModifier m_fireMultiplier;
        private DamageTypeModifier m_poisonMultiplier;
        public override void Pickup(PlayerController player)
        {
            
            base.Pickup(player);
            ETGMod.AIActor.OnPreStart = (Action<AIActor>)Delegate.Combine(ETGMod.AIActor.OnPreStart, new Action<AIActor>(this.EnergyBending));
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
                pickupId = ETGMod.Databases.Items["Primal Nitric Acid"].PickupObjectId,
                weight = 1.8f,
                rawGameObject = ETGMod.Databases.Items["Primal Nitric Acid"].gameObject,
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
            if (!player.HasPickupID(ETGMod.Databases.Items["Primal Nitric Acid"].PickupObjectId))
            {
                GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements.Add(Object3);
            }
            if (player.HasPickupID(ETGMod.Databases.Items["Primal Sulfur"].PickupObjectId) && player.HasPickupID(ETGMod.Databases.Items["Primal Nitric Acid"].PickupObjectId) && player.HasPickupID(ETGMod.Databases.Items["Primal Charcoal"].PickupObjectId) && !player.HasPickupID(ETGMod.Databases.Items["True Gunpowder"].PickupObjectId))
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(ETGMod.Databases.Items["True Gunpowder"].PickupObjectId).gameObject, player);
            }
            
        }



        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<PrimalSaltpeter>().m_pickedUpThisRun = true;
            ETGMod.AIActor.OnPreStart = (Action<AIActor>)Delegate.Remove(ETGMod.AIActor.OnPreStart, new Action<AIActor>(this.EnergyBending));
            return debrisObject;
        }
    }
}
