
using GungeonAPI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomCharacters
{
    public static class RandomShrine
    {
        private static string[] baseCharacters = new string[]
        {
            "convict",
            "guide",
            "marine",
            "rogue",
            "bullet",
            "robot",
            "eevee",
            "gunslinger"
        };
        public static List<string> characters = new List<string>();


        public static void Add()
        {
            ShrineFactory sf = new ShrineFactory()
            {

                name = "Random Shrine",
                modID = "cc",
                text = "Randomize your character?",
                spritePath = "CustomCharacters/resources/random_shrine.png",
                shadowSpritePath = "CustomCharacters/resources/default_shrine_shadow_small.png",
                acceptText = "Accept",
                declineText = "Decline",
                OnAccept = Accept,
                OnDecline = null,
                offset = new Vector3(63.9f, 23.3f, 23.8f),
                talkPointOffset = new Vector3(0, 1, 0),
                isToggle = false,
                isBreachShrine = true
            };
            //register shrine
            sf.Build();

            ETGModConsole.Commands.AddUnit("set_hegemony_credits", (args) =>
            {
                try
                {
                    GameStatsManager.Instance.SetStat(TrackedStats.META_CURRENCY, int.Parse(args[0]));
                }
                catch {
                    Tools.PrintError("You did it wrong");
                }
            });
        }

        public static void BuildCharacterList(List<FoyerCharacterSelectFlag> flags)
        {
            characters.Clear();
            foreach(var flag in flags)
            {
                foreach(var baseChar in baseCharacters)
                {
                    if (flag.CharacterPrefabPath.ToLower().Contains(baseChar))
                        characters.Add(baseChar);
                }
            }

           // foreach (var c in CharacterBuilder.storedCharacters.Values)
            //    characters.Add(c.First.nameShort);
        }

        public static void Accept(PlayerController player, GameObject shrine)
        {
            int r = 0;
            string curID = player.name.ToLowerInvariant().RemovePrefix("player").RemoveSuffix("(clone)");
            bool retry = false;
            int currency = (int)GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.META_CURRENCY);
            do
            {
                r = UnityEngine.Random.Range(0, characters.Count);
                retry = (characters[r] == "gunslinger" && currency < 7) || (characters[r] == "eevee" && currency < 5);
            } while (curID.Equals(characters[r]) || retry);
            bool isPrimary = (player == GameManager.Instance.PrimaryPlayer);
            string command = isPrimary ? "character" : "character2";
            ETGModConsole.RunCommand(new String[] { command }, new String[] { characters[r] });

            var newPlayer = isPrimary ? GameManager.Instance.PrimaryPlayer : GameManager.Instance.SecondaryPlayer;
            newPlayer.transform.position = shrine.transform.position + new Vector3(-.5f, -2, 0);
            AkSoundEngine.PostEvent("Play_OBJ_shrine_accept_01", shrine);
        }

        public static string RemoveSuffix(this string s, string suffix)
        {
            if (s.EndsWith(suffix))
                return s.Substring(0, s.Length - suffix.Length);
            return s;
        }
    }
}



