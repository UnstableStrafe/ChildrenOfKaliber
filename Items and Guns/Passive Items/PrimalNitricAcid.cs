using Alexandria.ItemAPI;
using UnityEngine;

namespace Items
{
    class PrimalNitricAcid : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Primal Nitric Acid";

            string resourceName = "Items/Resources/ItemSprites/Passives/primal_nitric_acid.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<PrimalNitricAcid>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Don't Ask How It's Made";
            string longDesc = "Shots can douse enemies in nitric acid, inflicting damage over time and creaing a pool of nitric acid on death.\n\nHarvested from the first bullat to be spawned in the Gungeon, teeming with energy from the Bullet.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.A;
            item.sprite.IsPerpendicular = true;
            primalNitricAcidId = item.PickupObjectId;
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
            foreach (WeightedGameObject obj in GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements)
            {
                if (obj.pickupId == PrimalSulfur.primalSulfurId || obj.pickupId == PrimalSaltpeter.primalSaltpeterId || obj.pickupId == PrimalCharcoal.primalCharcoalId && !player.HasPickupID(obj.pickupId))
                {
                    obj.weight = 3;
                }
            }
            if (player.HasPickupID(PrimalCharcoal.primalCharcoalId) && player.HasPickupID(PrimalSaltpeter.primalSaltpeterId) && player.HasPickupID(PrimalSulfur.primalSulfurId) && player.HasPickupID(PrimalCharcoal.primalCharcoalId) && !player.HasPickupID(TrueGunpowder.itemID))
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(TrueGunpowder.itemID).gameObject, player);
            }
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<PrimalNitricAcid>().m_pickedUpThisRun = true;
            player.PostProcessProjectile -= this.PostProj;
          
            return debrisObject;
        }
        public static int primalNitricAcidId;
    }
}
