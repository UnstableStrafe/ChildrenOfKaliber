using UnityEngine;
using Alexandria.ItemAPI;



namespace Items
{
    class SusieNKris : PassiveItem
    {
        public static void Init()
        {

            string itemName = "GOD FUCKING dammit KRIS";
            string resourceName = "Items/Resources/god_fucking_dammit_kris.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<SusieNKris>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Where the FUCK are we!?";
            string longDesc = "...";
               

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            item.quality = PickupObject.ItemQuality.EXCLUDED;

        }

        
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<SusieNKris>().m_pickedUpThisRun = true;
         
            return debrisObject;
        }

    }

   
}
