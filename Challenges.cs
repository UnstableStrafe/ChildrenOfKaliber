using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MonoMod;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Gungeon;
using Alexandria.ItemAPI;

namespace Items
{
    class Challenges
    {
        public static readonly string Color = "#fca4e2";
        public static void Init()
        {
            //setup console logging babeyy
            
            CurrentChallenge = ChallengeType.NONE;
            SetupSprite();
            ETGModConsole.Commands.GetGroup("ck").AddGroup("challenges", delegate (string[] args)
            {
                ETGModConsole.Log("<size=100><color=#fca4e2>List of Custom Challenges</color></size>");
                ETGModConsole.Log("Type 'ck challenges [challenge id]' to start the challenge (can only be done from the Breach).");
                ETGModConsole.Log("Blind Luck:  [id]<color=#fca4e2>blind_luck</color> - All passive and active item sprites and names are hidden and item chest teirs are randomized.");
                ETGModConsole.Log("Challenges will be automatically disabled if Blessed Mode or Rainbow Mode are enabled, if a shortcut is taken from the Breach, or if the player is the Gunslinger or Paradox.", false);

            });
            ETGModConsole.Commands.GetGroup("ck").GetGroup("challenges").AddUnit("clear", delegate (string[] args)
            {
                if (GameManager.Instance.IsFoyer)
                {
                    ETGModConsole.Log("Challenge Removed");
                    CurrentChallenge = ChallengeType.NONE;
                }
                else
                {
                    ETGModConsole.Log("<color=#fca4e2>Challenges can only be activated or deactivated from the Breach.</color>", false);
                }
            });
            ETGModConsole.Commands.GetGroup("ck").GetGroup("challenges").AddUnit("blind_luck", delegate (string[] args)
            {
                string failure = FetchFailureType();
                if (failure == "none")
                {
                    ETGModConsole.Log("The challenge Blind Luck has been enabled! May luck be on your side.");
                    CurrentChallenge = ChallengeType.BLIND_LUCK;
                }
                else
                {
                    ETGModConsole.Log(string.Format("<color=#ff4545>{0}</color>", failure), false);
                }
            });

           
            Hook chestOpenHook = new Hook(
                typeof(Chest).GetMethod("Open", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(Challenges).GetMethod("OnOpen")
            );

        }

        private static tk2dSpriteCollectionData hiddenSpriteCollection;
        private static GameObject VFXScapegoat;
        private static int hiddenItemSpriteID;
        private static void SetupSprite()
        {
            VFXScapegoat = new GameObject();
            UnityEngine.Object.DontDestroyOnLoad(VFXScapegoat);
            hiddenSpriteCollection = SpriteBuilder.ConstructCollection(VFXScapegoat, "HiddenItemSpriteCollection");
            UnityEngine.Object.DontDestroyOnLoad(hiddenSpriteCollection);
            hiddenItemSpriteID = SpriteBuilder.AddSpriteToCollection("Items/Resources/hidden_item", hiddenSpriteCollection);
        }

        public static void OnLevelLoaded()
        {
            if (CurrentChallenge != ChallengeType.NONE)
            {
                //ETGModConsole.Log("Challenge was not null!");
                PlayerController player1 = GameManager.Instance.PrimaryPlayer;
                if (player1 == null) ETGModConsole.Log("Why is the player null!");
                PlayerController player2 = null;
                if (GameManager.Instance.SecondaryPlayer != null) player2 = GameManager.Instance.SecondaryPlayer;
                if (GameStatsManager.Instance.IsRainbowRun)
                {
                    ETGModConsole.Log("<color=#ff4545>Challenge Voided: Played Rainbow Run</color>", false);
                    Library.DoAmbientTalk(player1.transform, new Vector3(1, 2, 0), "Challenge Voided: Played Rainbow Run", 4f);
                    CurrentChallenge = ChallengeType.NONE;
                }
                else if (player1.CharacterUsesRandomGuns || (player2 && player2.CharacterUsesRandomGuns))
                {
                    ETGModConsole.Log("<color=#ff4545>Challenge Voided: Played Blessed Run</color>", false);
                    CurrentChallenge = ChallengeType.NONE;
                    Library.DoAmbientTalk(player1.transform, new Vector3(1, 2, 0), "Challenge Voided: Played Blessed Run", 4f);
                }
                else if (player1.characterIdentity == PlayableCharacters.Gunslinger || (player2 && player2.characterIdentity == PlayableCharacters.Gunslinger))
                {
                    ETGModConsole.Log("<color=#ff4545>Challenge Voided: Played as Gunslinger</color>", false);
                    CurrentChallenge = ChallengeType.NONE;
                    Library.DoAmbientTalk(player1.transform, new Vector3(1, 2, 0), "Challenge Voided: Played as Gunslinger", 4f);
                }
                else if (player1.characterIdentity == PlayableCharacters.Eevee || (player2 && player2.characterIdentity == PlayableCharacters.Eevee))
                {
                    ETGModConsole.Log("<color=#ff4545>Challenge Voided: Played as Paradox</color>", false);
                    CurrentChallenge = ChallengeType.NONE;
                    Library.DoAmbientTalk(player1.transform, new Vector3(1, 2, 0), "Challenge Voided: Played as Paradox", 4f);
                }
                else if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.SHORTCUT)
                {
                    ETGModConsole.Log("<color=#ff4545>Challenge Voided: Took a Shortcut</color>", false);
                    CurrentChallenge = ChallengeType.NONE;
                    Library.DoAmbientTalk(player1.transform, new Vector3(1, 2, 0), "Challenge Voided: Took a Shortcut", 4f);
                }
                else if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.BOSSRUSH)
                {
                    ETGModConsole.Log("<color=#ff4545>Challenge Voided: Entered Bossrush</color>", false);
                    CurrentChallenge = ChallengeType.NONE;
                    Library.DoAmbientTalk(player1.transform, new Vector3(1, 2, 0), "Challenge Voided: Entered Bossrush", 4f);
                }
                else if (GameManager.Instance.InTutorial)
                {
                    ETGModConsole.Log("<color=#ff4545>Challenge Voided: Entered Tutorial</color>", false);
                    CurrentChallenge = ChallengeType.NONE;
                    Library.DoAmbientTalk(player1.transform, new Vector3(1, 2, 0), "Challenge Voided: Entered Tutorial", 4f);
                }
            }
        }

        public static ChallengeType CurrentChallenge;
        private static string FetchFailureType()
        {
            string failureMessage = "none";
            bool skipChecks = false;
            PlayerController player1 = GameManager.Instance.PrimaryPlayer;
            if (player1 == null)
            {
                ETGModConsole.Log("Welp the player is null.");
                failureMessage = "Please select a character before enabling the challenge.";
                skipChecks = true;
            }
            PlayerController player2 = null;
            if (GameManager.Instance.SecondaryPlayer != null) player2 = GameManager.Instance.SecondaryPlayer;

            if (!skipChecks)
            {
                if (!GameManager.Instance.IsFoyer) failureMessage = "Challenges can only be enabled or disabled from the Breach";
                else if (GameStatsManager.Instance.IsRainbowRun) failureMessage = "Challenges cannot be played in Rainbow Mode";
                else if (player1.CharacterUsesRandomGuns || (player2 && player2.CharacterUsesRandomGuns))
                {
                    failureMessage = "Challenges cannot be played in Blessed Mode";
                }
                else if (player1.characterIdentity == PlayableCharacters.Gunslinger || (player2 && player2.characterIdentity == PlayableCharacters.Gunslinger))
                {
                    failureMessage = "Challenges cannot be played as the Gunslinger";
                }
                else if (player1.characterIdentity == PlayableCharacters.Eevee || (player2 && player2.characterIdentity == PlayableCharacters.Eevee))
                {
                    failureMessage = "Challenges cannot be played as the Paradox";
                }
            }
            return failureMessage;
        }
        public static void OnOpen(Action<Chest, PlayerController> orig, Chest self, PlayerController player)
        {

            if (Challenges.CurrentChallenge == ChallengeType.BLIND_LUCK)
            {
                if (!self.IsLocked && !self.IsLockBroken && !self.IsSealed)
                {
                    if(self.ChestType == Chest.GeneralChestType.ITEM)
                    {
                        List<PickupObject> d = self.PredictContents(player);
                        if (d != null)
                        {
                            List<int> contentIds = new List<int>();
                            foreach (PickupObject po in d)
                            {
                                PickupObject.ItemQuality quality = po.quality;
                                float rerollTeir = UnityEngine.Random.value;
                                PickupObject.ItemQuality newQuality = PickupObject.ItemQuality.D;
                                if (rerollTeir <= .20f)
                                {
                                    newQuality = PickupObject.ItemQuality.D; //20%
                                }
                                else if (rerollTeir > .20f && rerollTeir <= .50f)
                                {
                                    newQuality = PickupObject.ItemQuality.C; //30%
                                }
                                else if (rerollTeir > .50f && rerollTeir <= .85f)
                                {
                                    newQuality = PickupObject.ItemQuality.B; //35%
                                }
                                else if (rerollTeir > .85f && rerollTeir <= .95f)
                                {
                                    newQuality = PickupObject.ItemQuality.A; //10%
                                }
                                else if (rerollTeir > .95f)
                                {
                                    newQuality = PickupObject.ItemQuality.S; //5%
                                }
                                PickupObject pickupObject = LootEngine.GetItemOfTypeAndQuality<PickupObject>(newQuality, GameManager.Instance.RewardManager.ItemsLootTable);

                                string newName = blindLuckItemNames[UnityEngine.Random.Range(0, blindLuckItemNames.Count)] + " " + blindLuckItemNames[UnityEngine.Random.Range(0, blindLuckItemNames.Count)];
                                string newShortDesc = blindLuckItemNames[UnityEngine.Random.Range(0, blindLuckItemNames.Count)];
                                for(int i = 0; i < UnityEngine.Random.Range(2, 5); i++)
                                {
                                    newShortDesc += " ";
                                    newShortDesc += blindLuckItemNames[UnityEngine.Random.Range(0, blindLuckItemNames.Count)];
                                }
                                string newLongDesc = blindLuckNewLongDescLines[UnityEngine.Random.Range(0, blindLuckNewLongDescLines.Count)];
                                
                                
                                pickupObject.SetName(newName);
                                pickupObject.SetShortDescription(newShortDesc);
                                pickupObject.SetLongDescription(newLongDesc);
                                pickupObject.sprite.SetSprite(hiddenItemSpriteID);
                                contentIds.Add(pickupObject.PickupObjectId);
                                self.contents = null;
                                self.forceContentIds = contentIds;

                            }

                        }
                    }
                    
                }
            }
            
            orig(self, player);
        }

        public static List<string> blindLuckItemNames = new List<string> 
        {
            "Little",
            "Big",
            "Large",
            "Smol",
            "Item",
            "Kevin",
            "Jammed",
            "Fire",
            "Hot",
            "Bullet",
            "Dragun",
            "Gun",
            "Ultra",
            "Stupid",
            "Rotten",
            "Mom",
            "Dad",
            "Doctor",
            "Lad",
            "Lass",
            "NullReferenceException",
            "Spicy",
            "Brand-Name",
            "Neon",
            "New",
            "Old",
            "Hale's Own",
            "Goober",
            "Chilly",
            "Cold",
            "Golden",
            "Radioactive",
            "Silver",
            "Last",
            "Father",
            "Meat",
            "Spoiled",
            "None",
            "Explosive",
            "Broken",
            "Slippery",
            "Pet Rock",
            "Lil' Fire Boi",
            "[Item Name Not Found]",
            "NaN",
            "[REDACTED]",
            "[DATA EXPUNGED]",
            "Dim",
            "Bright",
            "Wall",
            "Monkey",
            "[Hyperlink Blocked]",
            "Spamton",
            "Potassium"
        };

        public static List<string> blindLuckNewLongDescLines = new List<string>
        {
            "You have angered the gods!",
            "We're no strangers to love\nYou know the rules and so do I\nA full commitment's what I'm thinking of\nYou wouldn't get this from any other guy\nI just wanna tell you how I'm feeling\nGotta make you understand\nNever gonna give you up\nNever gonna let you down\nNever gonna run around and desert you\nNever gonna make you cry\nNever gonna say goodbye\nNever gonna lie and hurt you",
            "HEY EVERY       !! IT'S ME!!!\nEV3RY  BUDDY  'S FAVORITE [[Number 1 Rated Salesman1997]]\nSPAMTON G. SPAMTON!!",
            "What the fuck did you just fucking say about me, you little bitch? I'll have you know I graduated top of my class in the Navy Seals, and I've been involved in numerous secret raids on Al-Quaeda, and I have over 300 confirmed kills. I am trained in gorilla warfare and I'm the top sniper in the entire US armed forces. You are nothing to me but just another target. I will wipe you the fuck out with precision the likes of which has never been seen before on this Earth, mark my fucking words. You think you can get away with saying that shit to me over the Internet? Think again, fucker. As we speak I am contacting my secret network of spies across the USA and your IP is being traced right now so you better prepare for the storm, maggot. The storm that wipes out the pathetic little thing you call your life. You're fucking dead, kid. I can be anywhere, anytime, and I can kill you in over seven hundred ways, and that's just with my bare hands. Not only am I extensively trained in unarmed combat, but I have access to the entire arsenal of the United States Marine Corps and I will use it to its full extent to wipe your miserable ass off the face of the continent, you little shit. If only you could have known what unholy retribution your little 'clever' comment was about to bring down upon you, maybe you would have held your fucking tongue. But you couldn't, you didn't, and now you're paying the price, you goddamn idiot. I will shit fury all over you and you will drown in it. You're fucking dead, kiddo.",
            "It is good day to be not dead!\nPow! You are dead!\nI am dead!",
            "The Soviet Union, officially the Union of Soviet Socialist Republics (USSR), was a federal socialist state in Northern Eurasia that existed from 1922 to 1991. Nominally a union of multiple national Soviet republics, in practice its government and economy were highly centralized until its final years. It was a one-party state governed by the Communist Party, with Moscow as its capital in its largest republic, the Russian SFSR. Other major urban centers were Leningrad, Kiev, Minsk, Tashkent, Alma-Ata and Novosibirsk. It was the largest country in the world by surface area, spanning over 10,000 kilometers (6,200 mi) east to west across 11 time zones and over 7,200 kilometers (4,500 mi) north to south. Its territory included much of Eastern Europe as well as part of Northern Europe and all of Northern and Central Asia. It had five climate zones such as tundra, taiga, steppes, desert, and mountains. Its diverse population was collectively known as Soviet people.",
            "Object Class: Keter\nSpecial Containment Procedures: Run.",
            "The condition, state, or quality of being free or as free as possible from all flaws or defects.",
            "@ edmundmcmillen You litte F**ker You made a shit of piece with your trash Issac it’s f**King Bad this trash game I will become back my money I hope you will in your next time a cow on a trash farm you sucker",
            "Enter the Gungeon is a bullet hell roguelike video game developed by Dodge Roll and published by Devolver Digital. It was released worldwide for Microsoft Windows, OS X, Linux, and PlayStation 4 on April 5, 2016, on Xbox One on April 5, 2017, on Nintendo Switch on December 14, 2017, and on Stadia on December 22, 2020.",
            "[REDACTED][REDACTED][REDACTED][REDACTED] kevin [REDACTED][DATA EXPUNGED]",
        };
        public enum ChallengeType
        {
            NONE,
            BLIND_LUCK,
        }
    }
}
