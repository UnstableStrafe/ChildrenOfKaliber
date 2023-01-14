using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using Gungeon;
using SaveAPI;
using System.IO;
using System.Collections;


namespace Items
{
    class GoldenRecord : PlayerItem
    {
        public static int itemID;
        public static void Init()
        {
            string itemName = "Golden Record";

            string resourceName = "Items/Resources/ItemSprites/Actives/golden_record.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<GoldenRecord>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Savestate";
            string longDesc = "On use, saves the following items from the user's inventory:\n-The player's current gun and a random gun.\n-2 Random passives\n-A random active item \n\nThen the previous savestate is loaded." +
                "\n\nIn 1977AD Sol, Nasa launched two probes, each carrying a record of various aspects of life on Earth. The record was meant to be a way to show potential future extraterrestrial life what Earth was like. Unfortunately, one of them ended up in the Gungeon.";


            //Find out how to code read/write to a file.
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalItemCapacity, 1);
            item.quality = ItemQuality.S;
            item.sprite.IsPerpendicular = true;
            item.consumable = true;
            itemID = item.PickupObjectId;
        }
        public override void Pickup(PlayerController player)
        {

            base.Pickup(player);
        }
        public override void DoEffect(PlayerController user)
        {
            string path = Path.Combine(ETGMod.ResourcesDirectory, "../ChildrenOfKaliberData/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string savestate = Path.Combine(path, "GoldenRecord.txt");
            DoRead(savestate, user);
            
            base.DoEffect(user);
        }
        private void DoRead(string path, PlayerController user)
        {
            bool doLoad = true;
            string[] items = new string[] { };
            if (!File.Exists(path))
            {
                doLoad = false;
                string[] piss = new string[] {""};
                File.WriteAllLines(path, piss);
            }
            else
            {
                items = File.ReadAllLines(path);
            }
            GameManager.Instance.StartCoroutine(DoWrite(path, doLoad, items, user));

        }
        private IEnumerator DoWrite(string path, bool doLoad, string[] items, PlayerController player)
        {
            yield return new WaitForSeconds(.15f);
            if (File.Exists(path))
            {
                List<string> stuffToSave = new List<string> { player.CurrentGun.EncounterNameOrDisplayName };
                List<string> currentPassives = new List<string> { };
                List<string> currentActives = new List<string> { };
                List<string> currentGuns= new List<string> { };
               
                if (player.passiveItems.Any())
                {
                    foreach (PassiveItem passive in player.passiveItems)
                    {
                        bool flag = passive.CanBeDropped;
                        bool flag2 = passive.quality != ItemQuality.EXCLUDED;
                        if (flag && flag2)
                        {
                            currentPassives.Add(passive.EncounterNameOrDisplayName);
                        }
                    }
                }                
                if (player.activeItems.Any())
                {
                    foreach (PlayerItem item in player.activeItems)
                    {
                        bool flag = item.CanBeDropped;
                        bool flag2 = item.quality != ItemQuality.EXCLUDED;
                        bool flag3 = item != this;
                        if (flag && flag2 && flag3)
                        {
                            currentActives.Add(item.EncounterNameOrDisplayName);
                        }
                    }
                }
                if (player.inventory.AllGuns.Any())
                {
                    foreach (Gun gun in player.inventory.AllGuns)
                    {
                        bool flag = gun.CanBeDropped;
                        bool flag2 = gun.quality != ItemQuality.EXCLUDED;
                        bool flag3 = gun != player.CurrentGun;
                        if (flag && flag2 && flag3)
                        {
                            currentGuns.Add(gun.EncounterNameOrDisplayName);
                        }
                    }
                }   
                if (currentGuns.Any())
                {
                    currentGuns.Remove(player.CurrentGun.EncounterNameOrDisplayName);
                }
                if (currentPassives.Any())
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (currentPassives.Any())
                        {
                            string s = currentPassives[UnityEngine.Random.Range(0, currentPassives.Count)];
                            stuffToSave.Add(s);
                            currentPassives.Remove(s);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (currentActives.Any())
                {
                    stuffToSave.Add(currentActives[UnityEngine.Random.Range(0, currentActives.Count)]);
                }
                if (currentGuns.Any())
                {
                    stuffToSave.Add(currentGuns[UnityEngine.Random.Range(0, currentGuns.Count)]);
                }
                if (doLoad)
                {
                    string [] lines = new string[] { };
                    lines = File.ReadAllLines(path);
                    foreach (string s in lines)
                    {
                        LootEngine.TryGivePrefabToPlayer(PickupObjectDatabase.GetByEncounterName(s).gameObject, player);
                    }

                }
                
                string[] writeData = stuffToSave.ToArray();
                File.WriteAllLines(path, writeData);
   
            }
            yield break;
            
        }
    }
}
