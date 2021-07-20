using System;
using ItemAPI;
using UnityEngine;

namespace Items
{
    class SyncDiode : PassiveItem
    {

        public static void Init()
        {

            string itemName = "Sync Diode";
            string resourceName = "Items/Resources/sync_diode.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<SyncDiode>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "?kroW nevE sihT seoD woH";
            string longDesc = "Increases damge and firerate, but bullets fire backwards.\n\n" +
                ".elbissopmi smeed scisyhp tahw erac t'nod stellub esuaceB .yawyna stoohs ,esruoc fo ,tellub ehT .redwopnug eht fo noitingi esuac ot noitcerid gnorw eht ni si remirp stI .toohs ot elba eb dluohs stellub eseht yaw on si ereht ,scimanydorea fo swal nwonk lla ot gnidroccA";


            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            item.quality = PickupObject.ItemQuality.EXCLUDED;

            
            
        }

        public static class TurretCounter
        {
            public static Action<int> TurretCountChanged = (t) => { };
        }

        public Action<int> TurretCallback;

        //public override void Pickup()
       // {
         //   TurretCallback = (turret_count) =>
         //   {
                // DO SOMETHING WHEN THE AMOUNT OF TURRETS CHANGES
                // turret_count will have the current turret count
        //    };
        
            //    TurretCounter.TurretCountChanged += TurretCallback;
       // }

      //  public override void Drop()
       // {
      //      TurretCounter.TurretCountChanged -= TurretCallback;
       // }


        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<SyncDiode>().m_pickedUpThisRun = true;
            return debrisObject;
        }

        public static Action<int> TurrentCountChanged = (t) => { };

        // FOR ACTIVE TurretCountChanged.Invoke(current_turret_count);
        

        private float DamageS;
        private float SpeedS;



        
    }

    
}


