using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
//using Alexandria.DungeonAPI;

namespace Items
{
    class HandleAddUltimate
    {
        public static void GetCharacter()
        {
            foreach(PlayableCharacters player in Enum.GetValues(typeof(PlayableCharacters)))
            {
                var basePrefab = GetPlayerPrefab(player);
                if (basePrefab == null)
                {

                    return;
                }
                PlayerController playerController;
                GameObject gameObject = basePrefab;
                playerController = gameObject.GetComponent<PlayerController>();
                if (playerController.GetComponent("CustomCharacter"))
                {
                    return;
                }
                AddUltimatesToLoadout(playerController);

            }
        }

        public static void AddUltimatesToLoadout(PlayerController player)
        {
            //do code to add each active
        }

        public static GameObject GetPlayerPrefab(PlayableCharacters character)
        {
            string resourceName;

            if (character == PlayableCharacters.Soldier)
                resourceName = "marine";
            else if (character == PlayableCharacters.Pilot)
                resourceName = "rogue";
            else if (character == PlayableCharacters.Eevee)
                resourceName = "paradox";
            else
                resourceName = character.ToString().ToLower();

            return (GameObject)BraveResources.Load("player" + resourceName);

        }
    }
}
