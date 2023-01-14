using System;
using Alexandria.ItemAPI;
using UnityEngine;
using Gungeon;
using Dungeonator;

namespace Items
{
    class TheElectricQueen : PassiveItem
    {
        public static void Init()
        {
            string itemName = "The Electric Queen";

            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<TheElectricQueen>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Funeral Pyre";
            string longDesc = "Dealing damage periodically unleashes powerful electric attacks. Grants electricity immunity.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;

        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);

        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<TheElectricQueen>().m_pickedUpThisRun = true;

            return debrisObject;
        }
    }
}
