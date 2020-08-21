
using ItemAPI;
using UnityEngine;
namespace Items
{
    class TriggerPulseDrone : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Trigger Pulse Drone";

            string resourceName = "Items/Resources/trigger_drone.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<TriggerPulseDrone>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "D-7";
            string longDesc = "Attacks enemies passively. Grants 2 coolness.\n\nSeventh of Isaac Fogcutter's setoff into specialized spacial strike sentries sent seeking stupid suckers.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Coolness, 2, StatModifier.ModifyMethod.ADDITIVE);
            item.quality = ItemQuality.B;
            item.sprite.IsPerpendicular = true;

        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<TriggerPulseDrone>().m_pickedUpThisRun = true;

            return debrisObject;
        }
    }
}
