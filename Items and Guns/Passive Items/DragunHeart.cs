using Alexandria.ItemAPI;
using UnityEngine;

namespace Items
{
    class DragunHeart : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Dragun Heart";

            string resourceName = "Items/Resources/ItemSprites/Passives/dragun_heart.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<DragunHeart>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Burns Any Who Touch";
            string longDesc = "Increases health. Taking damage creates of pool of flaming oil. Gives fire immunity.\n\nThis Dragun heart feels warm in your hands, despite no longer beating." +
                "\n\nWhen the Dragun champion killed the Past, thereby preventing the death of his homeworld, he was mortified to find that the event occured regardless of if he intervened. He tried over and over to no avail.";
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 1f, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.C;
            item.sprite.IsPerpendicular = true;
            item.CanBeDropped = true;
            item.CanBeSold = true;
            item.AddToSubShop(ItemBuilder.ShopType.Goopton);
            dragunHeartId = item.PickupObjectId;
        }

        private void FireBurst(PlayerController player)
        {
            var bundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            var goop = bundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopquickignite.asset");
            var ddgm = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goop);
            ddgm.TimedAddGoopCircle(Owner.sprite.WorldCenter, 8f, .2f);

        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            
            this.m_fireImmunity = new DamageTypeModifier();
            this.m_fireImmunity.damageMultiplier = 0f;
            this.m_fireImmunity.damageType = CoreDamageTypes.Fire;
            player.healthHaver.damageTypeModifiers.Add(this.m_fireImmunity);
            player.OnReceivedDamage += this.FireBurst;

            if (player.HasPickupID(DragunSkull.dragunSkullId) && player.HasPickupID(DragunWing.dragunWingId) && player.HasPickupID(DragunClaw.dragunClawID) && !player.HasPickupID(SpiritOfTheDragun.gunID))
            {
                AkSoundEngine.PostEvent("Play_VO_dragun_death_01", gameObject);
                player.inventory.AddGunToInventory(PickupObjectDatabase.GetById(SpiritOfTheDragun.gunID) as Gun, true);
            }
        }



        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnReceivedDamage -= this.FireBurst;

            player.healthHaver.damageTypeModifiers.Remove(this.m_fireImmunity);
            debrisObject.GetComponent<DragunHeart>().m_pickedUpThisRun = true;
           
            return debrisObject;
        }
        private DamageTypeModifier m_fireImmunity;
        public static int dragunHeartId;
        
    }

}
