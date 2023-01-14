using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.ItemAPI;
using Gungeon;
using UnityEngine;
using Dungeonator;

namespace Items
{
    class SecondWind : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Second Wind";

            string resourceName = "SlayTheSwordtress/Resources/second_wind.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<SecondWind>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "For The Fallen!";
            string longDesc = "Entering a room with a boss in it grants the player 1 armor.\n\nWhen the Knight was needed most, he froze up. Never again will he stand by and watch others die.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "sts");
            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;
        }
        private bool GaveArmor = false;
        private void TestForBoss()
        {
            GaveArmor = false;
            RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
            List<AIActor> activeEnemies = absoluteRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            bool flag = activeEnemies != null;
            if (flag)
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    if (activeEnemies[i].healthHaver.IsBoss && !GaveArmor)
                    {
                        GaveArmor = true;
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, Owner);
                    }
                }
            }
        }
        
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnEnteredCombat = (Action)Delegate.Combine(player.OnEnteredCombat, new Action(TestForBoss));
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<SecondWind>().m_pickedUpThisRun = true;
            player.OnEnteredCombat = (Action)Delegate.Remove(player.OnEnteredCombat, new Action(TestForBoss));
            return debrisObject;
        }
    }
}
