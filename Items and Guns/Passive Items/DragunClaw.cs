using Alexandria.ItemAPI;
using UnityEngine;

namespace Items
{
    class DragunClaw : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Dragun Claw";

            string resourceName = "Items/Resources/ItemSprites/Passives/dragun_claw.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<DragunClaw>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Sharper Than Steel";
            string longDesc = "Increases reload speed. Reloading creates flaming oil. Gives fire immunity.\n\nThe claw of the First Dragun, it is covered in a flammable liquid that never seems to run out." +
                "\n\nThe Draguns were not originally from the Gungeon. They came from a fiery world that no longer exists.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ck");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ReloadSpeed, 1.10f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.quality = ItemQuality.C;
            item.sprite.IsPerpendicular = true;
            item.CanBeDropped = true;
            item.CanBeSold = true;
            item.AddToSubShop(ItemBuilder.ShopType.Goopton);
            dragunClawID = item.PickupObjectId;

        }

        private void Oil(PlayerController player, Gun gun)
        {
            
            var ddgm = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(Library.goopDefs[0]);
                ddgm.TimedAddGoopCircle(Owner.sprite.WorldCenter, 3f, .1f);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            
            this.m_fireImmunity = new DamageTypeModifier();
            this.m_fireImmunity.damageMultiplier = 0f;
            this.m_fireImmunity.damageType = CoreDamageTypes.Fire;
            player.healthHaver.damageTypeModifiers.Add(this.m_fireImmunity);
            player.OnReloadedGun += this.Oil;
           
            if (player.HasPickupID(DragunSkull.dragunSkullId) && player.HasPickupID(DragunWing.dragunWingId) && player.HasPickupID(DragunHeart.dragunHeartId) && !player.HasPickupID(SpiritOfTheDragun.gunID))
            {
                AkSoundEngine.PostEvent("Play_VO_dragun_death_01", gameObject);
                player.inventory.AddGunToInventory(PickupObjectDatabase.GetById(SpiritOfTheDragun.gunID) as Gun, true);
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnReloadedGun -= this.Oil;
            player.healthHaver.damageTypeModifiers.Remove(this.m_fireImmunity);
            debrisObject.GetComponent<DragunClaw>().m_pickedUpThisRun = true;
           
            return debrisObject;
        }
        private DamageTypeModifier m_fireImmunity;
        public static int dragunClawID;


        
    }
    
}
