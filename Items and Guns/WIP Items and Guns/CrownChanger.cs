using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;

namespace Items
{
    class CrownChanger
    {
        public static void Change()
        {
            
            PassiveItem BaseCrown = PickupObjectDatabase.GetById(ETGMod.Databases.Items["Crown Of The Chosen"].PickupObjectId) as PassiveItem;
            List<PassiveItem> CrownForms = new List<PassiveItem>()
            {
                PickupObjectDatabase.GetById(ETGMod.Databases.Items["The Electric Queen"].PickupObjectId) as PassiveItem,
                PickupObjectDatabase.GetById(ETGMod.Databases.Items["Crown Of The Nameless King"].PickupObjectId) as PassiveItem,
                PickupObjectDatabase.GetById(ETGMod.Databases.Items["Crown Of The Enduring"].PickupObjectId) as PassiveItem,
                PickupObjectDatabase.GetById(ETGMod.Databases.Items["Painted Crown"].PickupObjectId) as PassiveItem,
                PickupObjectDatabase.GetById(ETGMod.Databases.Items["Circle Of Antimatter"].PickupObjectId) as PassiveItem,
                PickupObjectDatabase.GetById(ETGMod.Databases.Items["Bleaker Crown"].PickupObjectId) as PassiveItem,
            };
            foreach(PassiveItem passive in CrownForms)
            {
                if(passive.quality != PickupObject.ItemQuality.EXCLUDED)
                {
                    passive.quality = PickupObject.ItemQuality.EXCLUDED;
                }
            }
            bool flag = string.IsNullOrEmpty(ChildrenOfKaliberModule.SteamUsername);
            if (ChildrenOfKaliberModule.SteamUsername == "UnstableStrafe")
            {
                BaseCrown.quality = PickupObject.ItemQuality.EXCLUDED;
                CrownForms[0].quality = PickupObject.ItemQuality.S;
            }
            else if (ChildrenOfKaliberModule.SteamUsername == "Nevernamed")
            {
                BaseCrown.quality = PickupObject.ItemQuality.EXCLUDED;
                CrownForms[1].quality = PickupObject.ItemQuality.S;

            }
            else if (ChildrenOfKaliberModule.SteamUsername == "TheTurtleMelon")
            {

                BaseCrown.quality = PickupObject.ItemQuality.EXCLUDED;
                CrownForms[2].quality = PickupObject.ItemQuality.S;
            }
            else if (ChildrenOfKaliberModule.SteamUsername == "SirWow")
            {
                BaseCrown.quality = PickupObject.ItemQuality.EXCLUDED;
                CrownForms[3].quality = PickupObject.ItemQuality.S;
            }
            else if (ChildrenOfKaliberModule.SteamUsername == "Some Bunny")
            {
                BaseCrown.quality = PickupObject.ItemQuality.EXCLUDED;
                CrownForms[4].quality = PickupObject.ItemQuality.S;
            }            
            else if (ChildrenOfKaliberModule.SteamUsername == "BleakBubbles")
            {
                BaseCrown.quality = PickupObject.ItemQuality.EXCLUDED;
                CrownForms[5].quality = PickupObject.ItemQuality.S;
            }
            else if (ChildrenOfKaliberModule.SteamUsername == "LordOfHippos")
            {
                //("Phantom's Crown");
                //("Steal Your Heart!");

                //instantly kills hit bosses that are on low hp

            }
            else if (ChildrenOfKaliberModule.SteamUsername == "Neighborino")
            {
                //("Frostfire Crown");
                //("Wandering...");
                
                //Adds two additional fire and ice projectiles that have orbital bullet effects with each shot.
            }

            else if (ChildrenOfKaliberModule.SteamUsername == "The explosive panda")
            {
                //("Shimmering Crown");
                //("Cult Of Shadows");
                
                //Causes random effects
            }
            else if (ChildrenOfKaliberModule.SteamUsername == "N0tAB0t")
            {

                //("N0R Crown");
                //("!Crown");
                
            }
            else if (ChildrenOfKaliberModule.SteamUsername == "KyleTheScientist")
            {
                //("Creator's Crown");
                //("The One To Rule All");
                
            }
            else if (ChildrenOfKaliberModule.SteamUsername == "Retromation")
            {
                //("Lich Slayer's Crown");
                //("Supreme");
                
                //Massive Boss Damage up, ignores boss dps cap
            }

            else if (ChildrenOfKaliberModule.SteamUsername == "YaBoiLazer")
            {
                //("Laser Crown");
                //("Pew Pew");
                
            }
            else if (ChildrenOfKaliberModule.SteamUsername == "W.D Cipher")
            {
                //("Low-Quality Crown");
                //("Ascendant");
                
                //Like Key Of Chaos, but more random

            }
            else if (ChildrenOfKaliberModule.SteamUsername == "Glaurung4567")
            {
                //("Dragon's Horns");
                //("The First Wyrm");
                 
                //Gives the player 2 permanent orbiting Dragunfires
            }
            else if (ChildrenOfKaliberModule.SteamUsername == "Retrash")
            {
                //("Blunderbeast Helmet");
                //("Cursed");
                 
            }
            
            else if (ChildrenOfKaliberModule.SteamUsername == "An3s")
            {
                //Shoots random projectiles around the player when hit
                //("");
                //("");
                 
            }
            else if (ChildrenOfKaliberModule.SteamUsername == "Skilotar_")
            {
                //spawn several shadowclones on taking damage
                //("");
                //("");

            }
            else if (ChildrenOfKaliberModule.SteamUsername == "Solanum Tuberosum")
            {
                //("");
                //("");

            }
            else if (ChildrenOfKaliberModule.SteamUsername == "blazeykat")
            {
                //("Prismatic Crown");
                //("Queen Of Frogs");
                
            }
            else if (ChildrenOfKaliberModule.SteamUsername == "Round King")
            {
                //("");
                //("");

            }
            else if (ChildrenOfKaliberModule.SteamUsername == "")
            {
                //("");
                //("");

            }
            else if (ChildrenOfKaliberModule.SteamUsername == "")
            {
                //("");
                //("");

            }
            else if (flag)
            {
                BaseCrown.quality = PickupObject.ItemQuality.EXCLUDED;
                int random = UnityEngine.Random.Range(0, CrownForms.Count - 1);
                CrownForms[random].quality = PickupObject.ItemQuality.S;
            }
            else
            {
                BaseCrown.quality = PickupObject.ItemQuality.EXCLUDED;
                int random = UnityEngine.Random.Range(0, CrownForms.Count - 1);
                CrownForms[random].quality = PickupObject.ItemQuality.S;
            }

        }
        public static void InitCrowns()
        {
            CrownOfTheNamelessKing.Init();
            CrownOfTheEnduring.Init();
            TheElectricQueen.Init();
            PaintedCrown.Init();
            CircleOfAntimatter.Init();
            BleakerCrown.Init();
        }
    }
}
