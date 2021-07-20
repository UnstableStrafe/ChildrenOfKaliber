using ItemAPI;
using UnityEngine;

namespace Items
{
    class MercuryAmmolet : SpecialBlankModificationItem
    {
        public static void Init()
        {
            string itemName = "Mercury Ammolet";

            string resourceName = "Items/Resources/mercury_ammolet.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<MercuryAmmolet>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "The Liquid Metal";
            string longDesc = "Blanks create a pool of venomous goop. Gives one extra blank per floor.\n\nMercury is commonly known for its cannibalistic appetite for other metals. If only there were creatures made of metal in the Gungeon...";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalBlanksPerFloor, 1, StatModifier.ModifyMethod.ADDITIVE);

            item.quality = ItemQuality.B;
            item.sprite.IsPerpendicular = true;

        }
        protected override void OnBlank(SilencerInstance silencerInstance, Vector2 centerPoint, PlayerController user)
        {
            var ddgm = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(Library.VenomGoop);
            ddgm.TimedAddGoopCircle(Owner.sprite.WorldCenter, 8f, .35f);
            base.OnBlank(silencerInstance, centerPoint, user);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<MercuryAmmolet>().m_pickedUpThisRun = true;
            return debrisObject;
        }
    }
}
