using System.Collections.Generic;

namespace Items
{
    class HoveringGunsAdder
    {
        public static void AddHovers()
        {
            /*AdvancedHoveringGunProcessor PrimeSawHover = ETGMod.Databases.Items["prime_saw"].gameObject.AddComponent<AdvancedHoveringGunProcessor>();
            PrimeSawHover.Activate = true;
            PrimeSawHover.ConsumesTargetGunAmmo = false;
            PrimeSawHover.AimType = HoveringGunController.AimType.PLAYER_AIM;
            PrimeSawHover.PositionType = HoveringGunController.HoverPosition.CIRCULATE;
            PrimeSawHover.FireType = HoveringGunController.FireType.ON_FIRED_GUN;
            PrimeSawHover.UsesMultipleGuns = false;
            PrimeSawHover.TargetGunID = ETGMod.Databases.Items["prime_laser"].PickupObjectId;
            PrimeSawHover.FireCooldown = .11f;
            PrimeSawHover.FireDuration = 0;
            AdvancedHoveringGunProcessor PrimeViceHover = ETGMod.Databases.Items["prime_vice"].gameObject.AddComponent<AdvancedHoveringGunProcessor>();
            PrimeViceHover.Activate = true;
            PrimeViceHover.ConsumesTargetGunAmmo = false;
            PrimeViceHover.AimType = HoveringGunController.AimType.PLAYER_AIM;
            PrimeViceHover.PositionType = HoveringGunController.HoverPosition.CIRCULATE;
            PrimeViceHover.FireType = HoveringGunController.FireType.ON_FIRED_GUN;
            PrimeViceHover.UsesMultipleGuns = false;
            PrimeViceHover.TargetGunID = ETGMod.Databases.Items["prime_cannon"].PickupObjectId;
            PrimeViceHover.FireCooldown = .6f;
            PrimeViceHover.FireDuration = 0;
            */
            AdvancedHoveringGunProcessor DroneHover = ETGMod.Databases.Items["drone_controller"].gameObject.AddComponent<AdvancedHoveringGunProcessor>();
            DroneHover.Activate = true;
            DroneHover.ConsumesTargetGunAmmo = false;
            DroneHover.AimType = HoveringGunController.AimType.NEAREST_ENEMY;
            DroneHover.PositionType = HoveringGunController.HoverPosition.CIRCULATE;
            DroneHover.FireType = HoveringGunController.FireType.ON_FIRED_GUN;
            DroneHover.UsesMultipleGuns = true;
            DroneHover.TargetGunIDs = new List<int> { ETGMod.Databases.Items["drone"].PickupObjectId, ETGMod.Databases.Items["drone_2"].PickupObjectId};
            DroneHover.FireCooldown = .11f;
            DroneHover.FireDuration = 0;
            DroneHover.NumToTrigger = 2;
            //AdvancedHoveringGunProcessor LittleDroneHover = ETGMod.Databases.Items["Little Drone Buddy"].gameObject.AddComponent<AdvancedHoveringGunProcessor>();
            //LittleDroneHover.Activate = true;
            //LittleDroneHover.ConsumesTargetGunAmmo = false;
            //LittleDroneHover.AimType = HoveringGunController.AimType.NEAREST_ENEMY;
            //LittleDroneHover.PositionType = HoveringGunController.HoverPosition.CIRCULATE;
            //LittleDroneHover.FireType = HoveringGunController.FireType.ON_RELOAD;
            //LittleDroneHover.UsesMultipleGuns = false;
            //LittleDroneHover.TargetGunID = ETGMod.Databases.Items["little_drone"].PickupObjectId;
            //LittleDroneHover.FireCooldown = .2f;
            //LittleDroneHover.FireDuration = 0;
            //AdvancedHoveringGunProcessor TriggerDroneHover = ETGMod.Databases.Items["Trigger Pulse Drone"].gameObject.AddComponent<AdvancedHoveringGunProcessor>();
            //TriggerDroneHover.Activate = true;
            //TriggerDroneHover.ConsumesTargetGunAmmo = false;
            //TriggerDroneHover.AimType = HoveringGunController.AimType.NEAREST_ENEMY;
            //TriggerDroneHover.PositionType = HoveringGunController.HoverPosition.CIRCULATE;
            //TriggerDroneHover.FireType = HoveringGunController.FireType.ON_COOLDOWN;
            //TriggerDroneHover.UsesMultipleGuns = false;
            //TriggerDroneHover.TargetGunID = ETGMod.Databases.Items["trigger_drone"].PickupObjectId;
            //TriggerDroneHover.FireCooldown = .35f;
            //TriggerDroneHover.FireDuration = 0;
            //AdvancedHoveringGunProcessor VenomDroneHover = ETGMod.Databases.Items["Venom Spit Drone"].gameObject.AddComponent<AdvancedHoveringGunProcessor>();
            //VenomDroneHover.Activate = true;
            //VenomDroneHover.ConsumesTargetGunAmmo = false;
            //VenomDroneHover.AimType = HoveringGunController.AimType.NEAREST_ENEMY;
            //VenomDroneHover.PositionType = HoveringGunController.HoverPosition.CIRCULATE;
            //VenomDroneHover.FireType = HoveringGunController.FireType.ON_COOLDOWN;
            //VenomDroneHover.UsesMultipleGuns = false;
            //VenomDroneHover.TargetGunID = ETGMod.Databases.Items["venom_spit_drone_gun"].PickupObjectId;
            //VenomDroneHover.FireCooldown = 3f;
            //VenomDroneHover.FireDuration = 0;
            //AdvancedHoveringGunProcessor DeathDroneHover = ETGMod.Databases.Items["d.e.a.t.h."].gameObject.AddComponent<AdvancedHoveringGunProcessor>();
            //DeathDroneHover.Activate = true;
            //DeathDroneHover.ConsumesTargetGunAmmo = false;
            //DeathDroneHover.AimType = HoveringGunController.AimType.NEAREST_ENEMY;
            //DeathDroneHover.PositionType = HoveringGunController.HoverPosition.CIRCULATE;
            //DeathDroneHover.FireType = HoveringGunController.FireType.ON_FIRED_GUN;
            //DeathDroneHover.UsesMultipleGuns = true;
            //DeathDroneHover.NumToTrigger = 3;
            //DeathDroneHover.TargetGunIDs = new List<int> { ETGMod.Databases.Items["d.e.a.t.h._drone"].PickupObjectId, ETGMod.Databases.Items["d.e.a.t.h._drone"].PickupObjectId, ETGMod.Databases.Items["d.e.a.t.h._drone"].PickupObjectId };
            //DeathDroneHover.FireCooldown = .22f;
            //DeathDroneHover.FireDuration = 0;
        }
    }
}
