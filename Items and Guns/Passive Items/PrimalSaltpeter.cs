using System;
using UnityEngine;
using Alexandria.ItemAPI;
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

            string resourceName = "Items/Resources/ItemSprites/Passives/primal_saltpeter.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<PrimalSaltpeter>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Niter Is Cooler!";
            string longDesc = "Purifies non-boss enemy elemental resistances.\n\nOne of the first resources to ever be harvested in the Black Powder Mines.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.D;
            item.sprite.IsPerpendicular = true;
            primalSaltpeterId = item.PickupObjectId;
        }
        public override void Update()
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
           
            if (player.HasPickupID(PrimalNitricAcid.primalNitricAcidId) && player.HasPickupID(PrimalSulfur.primalSulfurId) && player.HasPickupID(PrimalCharcoal.primalCharcoalId) && !player.HasPickupID(TrueGunpowder.itemID))
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(TrueGunpowder.itemID).gameObject, player);
            }
            
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<PrimalSaltpeter>().m_pickedUpThisRun = true;
            ETGMod.AIActor.OnPreStart = (Action<AIActor>)Delegate.Remove(ETGMod.AIActor.OnPreStart, new Action<AIActor>(this.EnergyBending));
          
            return debrisObject;
        }
        public static int primalSaltpeterId;
    }
}
