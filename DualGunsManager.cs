using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Items
{
    class DualGunsManager
    {
        public static void AddDual()
        {
            DualWieldController PrimeSawController = ETGMod.Databases.Items["prime_saw"].gameObject.AddComponent<DualWieldController>();
            PrimeSawController.PartnerGunID = ETGMod.Databases.Items["prime_vice"].PickupObjectId;
            PrimeSawController.NoHands = true;

            DualWieldController PrimeViceController = ETGMod.Databases.Items["prime_vice"].gameObject.AddComponent<DualWieldController>();
            PrimeViceController.PartnerGunID = ETGMod.Databases.Items["prime_saw"].PickupObjectId;
            PrimeViceController.NoHands = true;

            DualWieldController RussianRevolverController = ETGMod.Databases.Items["russian_revolver"].gameObject.AddComponent<DualWieldController>();
            RussianRevolverController.PartnerGunID = 2;

            DualWieldController TommyGunController = (PickupObjectDatabase.GetById(2) as Gun).gameObject.AddComponent<DualWieldController>();
            TommyGunController.PartnerGunID = ETGMod.Databases.Items["russian_revolver"].PickupObjectId;
        }
    }
}
