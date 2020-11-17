
using ItemAPI;
using UnityEngine;

namespace Items
{
    class CitrineAmmolet : PassiveItem
    {
        public static void Init()
        {

            string itemName = "Citrine Ammolet";
            string resourceName = "Items/Resources/citrine_ammolet.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<CitrineAmmolet>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Hot Like Magma";
            string longDesc = "Creates a pool of fire upon using a blank. Gives fire immunity.\n\nThis ammolet was forged from a citrine found in the lava of a volcano.";
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalBlanksPerFloor, 1, StatModifier.ModifyMethod.ADDITIVE);


            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            item.quality = PickupObject.ItemQuality.B;
            item.sprite.IsPerpendicular = true;


        }

        private void FireGoopBlank(PlayerController player, int blanks)
        {
            var bundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            var goop = bundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopquickignite.asset");
            var ddgm = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goop);
            ddgm.TimedAddGoopCircle(Owner.sprite.WorldCenter, 8f, .35f);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            this.m_fireImmunity = new DamageTypeModifier();
            this.m_fireImmunity.damageMultiplier = 0f;
            this.m_fireImmunity.damageType = CoreDamageTypes.Fire;
            player.healthHaver.damageTypeModifiers.Add(this.m_fireImmunity);
            player.OnUsedBlank += this.FireGoopBlank;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnUsedBlank -= this.FireGoopBlank;
            player.healthHaver.damageTypeModifiers.Remove(this.m_fireImmunity);
            debrisObject.GetComponent<CitrineAmmolet>().m_pickedUpThisRun = true;
            return debrisObject;
        }
        private DamageTypeModifier m_fireImmunity;
    }
}
