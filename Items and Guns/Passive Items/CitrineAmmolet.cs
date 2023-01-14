
using Alexandria.ItemAPI;
using UnityEngine;

namespace Items
{
    class CitrineAmmolet : SpecialBlankModificationItem
    {
        public static void Init()
        {

            string itemName = "Citrine Ammolet";
            string resourceName = "Items/Resources/ItemSprites/Passives/citrine_ammolet.png";

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
        protected override void OnBlank(SilencerInstance silencerInstance, Vector2 centerPoint, PlayerController user)
        {
            var bundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            var goop = bundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopquickignite.asset");
            var ddgm = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goop);
            ddgm.TimedAddGoopCircle(Owner.sprite.WorldCenter, 8f, .35f);
            base.OnBlank(silencerInstance, centerPoint, user);
        }

        

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            this.m_fireImmunity = new DamageTypeModifier();
            this.m_fireImmunity.damageMultiplier = 0f;
            this.m_fireImmunity.damageType = CoreDamageTypes.Fire;
            player.healthHaver.damageTypeModifiers.Add(this.m_fireImmunity);

        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);

            player.healthHaver.damageTypeModifiers.Remove(this.m_fireImmunity);
            debrisObject.GetComponent<CitrineAmmolet>().m_pickedUpThisRun = true;
            return debrisObject;
        }
        private DamageTypeModifier m_fireImmunity;
    }
}
