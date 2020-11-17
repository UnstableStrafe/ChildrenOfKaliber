using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Items
{
    class SynergyFormAdder
    {
        public static void AddForms()
        {
            AdvancedTransformGunSynergyProcessor IonFistTransform = ETGMod.Databases.Items["ion_fist"].gameObject.AddComponent<AdvancedTransformGunSynergyProcessor>();
            IonFistTransform.NonSynergyGunId = ETGMod.Databases.Items["ion_fist"].PickupObjectId;
            IonFistTransform.SynergyGunId = ETGMod.Databases.Items["nen_fist"].PickupObjectId;
            IonFistTransform.SynergyToCheck = "Jajanken";
            AdvancedTransformGunSynergyProcessor CharmedBowTransform = (PickupObjectDatabase.GetById(200) as Gun).gameObject.AddComponent<AdvancedTransformGunSynergyProcessor>();
            CharmedBowTransform.NonSynergyGunId = (PickupObjectDatabase.GetById(200) as Gun).PickupObjectId;
            CharmedBowTransform.SynergyGunId = ETGMod.Databases.Items["evil_charmed_bow"].PickupObjectId;
            CharmedBowTransform.SynergyToCheck = "All Out Of Love";
        }
    }
}
