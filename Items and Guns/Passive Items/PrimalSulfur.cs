using System;
using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
namespace Items
{
    class PrimalSulfur : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Primal Sulfur";

            string resourceName = "Items/Resources/ItemSprites/Passives/primal_sulfur.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<PrimalSulfur>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Smells Nice";
            string longDesc = "Upon entering a room up to 4 random enemies are marked. Marked enemies explode on death. These explosions do not harm the player.\n\nIt is said this sulfur was enchanted with the energy of raw firepower.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.B;
            item.sprite.IsPerpendicular = true;
            primalSulfurId = item.PickupObjectId;
        }
        private void MarkEnemies()
        {
            
            List<AIActor> actors = new List<AIActor> { };
            RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
            if (absoluteRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All))
            {
                actors = Library.RandomNoRepeats(absoluteRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All), 4);

            }
            if (actors.Any())
            {
                foreach(AIActor a in actors)
                {
                    a.ApplyEffect(Library.SulfurEffect);
                }
            }
        }
        public override void Pickup(PlayerController player)
        {
            player.OnEnteredCombat = (Action)Delegate.Combine(player.OnEnteredCombat, new Action(this.MarkEnemies));
           
           
            if (player.HasPickupID(PrimalCharcoal.primalCharcoalId) && player.HasPickupID(PrimalSaltpeter.primalSaltpeterId) && player.HasPickupID(PrimalNitricAcid.primalNitricAcidId) && !player.HasPickupID(TrueGunpowder.itemID))
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(TrueGunpowder.itemID).gameObject, player);
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
        public static int primalSulfurId;
    }
    
}
