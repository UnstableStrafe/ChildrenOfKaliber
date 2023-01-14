using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx;
using BepInEx.Bootstrap;

namespace Items
{
    class ModdedIDStuff
    {
        public static void SetupIDs()
        {
            foreach (var mod in Chainloader.PluginInfos.Values)
            {
                if (mod != null)
                {
                    if (mod.Metadata != null && mod.Metadata.Name != null)
                    {
                        if (mod.Metadata.Name.Contains("Planetside Of Gunymede"))
                        {
                            ModInstallFlags.PlanetsideOfGunymededInstalled = true;                      
                            //"Bullets"
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Aura Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Corrupt Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Derpy Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Hull-Breaker Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Oscillating Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Unsta-bullets").PickupObjectId);

                            //"Rounds"
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Frailty Rounds").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Injector Rounds").PickupObjectId);

                            //"Shells"

                            //"Shots"

                            //Misc
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Shell-snake Oil").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Teleporting Gunfire").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Shells Of The Mountain").PickupObjectId);
                        }
                        if (mod.Metadata.Name.Contains("FrostAndGunfireItems"))
                        {
                            ModInstallFlags.FrostAndGunfireInstalled = true;
                            //"Bullets"
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Void Bullets").PickupObjectId);

                            //"Rounds"
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Holy Rounds").PickupObjectId);

                            //"Shells"

                            //"Shots"
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Longshot").PickupObjectId);

                            //Misc
                        }
                        if (mod.Metadata.Name.Contains("Prismatismas"))
                        {
                            ModInstallFlags.PrismatismInstalled = true;
                            //"Bullets"

                            //"Rounds"

                            //"Shells"

                            //"Shots"

                            //Misc

                        }
                        if (mod.Metadata.Name.Contains("SpecialAPI's Stuff"))
                        {
                            ModInstallFlags.SpecialAPIsStuffInstalled = true;
                            //"Bullets"

                            //"Rounds"

                            //"Shells"

                            //"Shots"

                            //Misc
                        }
                        if (mod.Metadata.Name.Contains("ExpandTheGungeon"))
                        {
                            ModInstallFlags.ExpandTheGungeonInstalled = true;
                            //"Bullets"

                            //"Rounds"

                            //"Shells"

                            //"Shots"

                            //Misc

                        }

                        if (mod.Metadata.Name.Contains("KTS Item Pack"))
                        {
                            ModInstallFlags.KylesItemsInstalled = true;
                            //"Bullets"
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Boss Bullets").PickupObjectId);

                            //"Rounds"

                            //"Shells"

                            //"Shots"

                            //Misc
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Adhesive Grenade").PickupObjectId);

                        }

                        if (mod.Metadata.Name.Contains("Gundustrial  Revolution"))
                        {
                            ModInstallFlags.GundustrialInstalled = true;
                            //"Bullets"
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Barrel Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Robullets").PickupObjectId);

                            //"Rounds"

                            //"Shells"

                            //"Shots"

                            //Misc

                        }

                        if (mod.Metadata.Name.Contains("[Retrash's] Custom Items Collection"))
                        {
                            ModInstallFlags.RetrashItemsInstalled = true;

                            //"Bullets"
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Tiny Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Locked Bullets").PickupObjectId);

                            //"Rounds"
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Malediction Rounds").PickupObjectId);

                            //"Shells"

                            //"Shots"

                            //Misc
                        }
                        if (mod.Metadata.Name.Contains("OnceMoreIntoTheBreach"))
                        {
                            ModInstallFlags.OnceMoreIntoTheBreachInstalled = true;
                            //"Bullets"
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Warp Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Wooden Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Backwards Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Bashing Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Bloodthirsty Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Cross Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Drill Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Epimethean Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Hallowed Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Hard Reload Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Junkllets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Knightly Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Laser Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Lockdown Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Mirror Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Mistake Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Nitro Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Promethean Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Snail Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Spectre Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Tabullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Titan Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Unpredictabullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Voodoollets").PickupObjectId);

                            //"Rounds"
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("50. Cal Rounds").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Glass Rounds").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Hellfire Rounds").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Hematic Rounds").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Rando Rounds").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Tracer Rounds").PickupObjectId);

                            //"Shells"
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Blight Shell").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Bombardier Shells").PickupObjectId);

                            //"Shots"
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Bashful Shot").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Birdshot").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Longsword Shot").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Shade Shot").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Shrinkshot").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Splattershot").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Supersonic Shots").PickupObjectId);

                            //Misc
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Bullets With Guns").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Chemical Burn").PickupObjectId);                           
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Crowded Clips").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Eargesplitten Loudenboomers").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Eraser").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Foam Darts").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Full Armour Jacket").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Gilded Lead").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Moonrock").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Pestiferous Lead").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Rocket Man").PickupObjectId);                           
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("The Shell").PickupObjectId);


                            //Insta Killers
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Alkali Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Antimagic Rounds").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("ERROR Shells").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Miners Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Osteoporosis Bullets").PickupObjectId);
                            ChildrenOfKaliberModule.moddedMunitionsIDs.Add(PickupObjectDatabase.GetByEncounterName("Shutdown Shells").PickupObjectId);
                        }
                        if (mod.Metadata.Name.Contains("Enter The Beyond"))
                        {
                            ModInstallFlags.EnterTheBeyondInstalled = true;

                        }
                    }
                }
            }
        }
    }
    public static class ModInstallFlags
    {

        public static bool PrismatismInstalled = false;
        public static bool RetrashItemsInstalled = false;
        public static bool SpecialAPIsStuffInstalled = false;
        public static bool KylesItemsInstalled = false;
        public static bool FrostAndGunfireInstalled = false;
        public static bool PlanetsideOfGunymededInstalled = false;
        public static bool ExpandTheGungeonInstalled = false;
        public static bool OnceMoreIntoTheBreachInstalled = false;
        public static bool EnterTheBeyondInstalled = false;
        public static bool GundustrialInstalled = false;

    }
    
}
