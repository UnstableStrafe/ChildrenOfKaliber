using UnityEngine;
using Alexandria.ItemAPI;

namespace Items
{
    class Sash : HealthPickup
    {
        public static void Init()
        {

            string itemName = "Crystal Heart";
            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<Sash>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Sharp!";
            string longDesc = "Makes bullet stats weird.\n\n" +
                "A horribly failed attempt at forging a bullet that could kill the past. The creator threw them into the Gungeon some time later, where they remained until your grubby little hands put them in a magazine.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = PickupObject.ItemQuality.SPECIAL;
        }
    }
    
}

