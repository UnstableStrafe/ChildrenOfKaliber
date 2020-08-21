using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace ItemAPI
{
    class ExampleActive : PlayerItem
    {
        //Call this method from the Start() method of your ETGModule extension class
        public static void Init()
        {
            //The name of the item
            string itemName = "Sweating Bullets";
            
            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "CustomItems/Resources/sweating_bullets_icon"; 

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<ExampleActive>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Is it Hot in Here?";
            string longDesc = "While active, triples bullet damage, but reduces health to 1 hit. \n\nDon't get nervous!";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "kts"); 

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 500);

            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1);

            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.C;
        }

        //Add the item's functionality down here! I stole most of this from the Stuffed Star active item code!
        float damageBuff = -1;
        float duration = 10f;
        protected override void DoEffect(PlayerController user)
        {
            //Play a sound effect
            AkSoundEngine.PostEvent("Play_OBJ_power_up_01", base.gameObject);

            //Activates the effect
            StartEffect(user);

            //start a coroutine which calls the EndEffect method when the item's effect duration runs out
            StartCoroutine(ItemBuilder.HandleDuration(this, duration, user, EndEffect));
        }

        //Doubles the damage, makes the next shot kill the player, and stores the amount we buffed the player for later
        private void StartEffect(PlayerController user)
        {
            user.healthHaver.NextShotKills = true;
            float curDamage = user.stats.GetBaseStatValue(PlayerStats.StatType.Damage);
            float newDamage = curDamage * 3f;
            user.stats.SetBaseStatValue(PlayerStats.StatType.Damage, newDamage, user);
            damageBuff = newDamage - curDamage;
        }

        //Resets the player back to their original stats
        private void EndEffect(PlayerController user)
        {
            if (damageBuff <= 0) return;
            user.healthHaver.NextShotKills = false;
            float curDamage = user.stats.GetBaseStatValue(PlayerStats.StatType.Damage);
            float newDamage = curDamage - damageBuff;
            user.stats.SetBaseStatValue(PlayerStats.StatType.Damage, newDamage, user);
            damageBuff = -1;
        }

        //Disable or enable the active whenever you need!
        public override bool CanBeUsed(PlayerController user)
        {
            return base.CanBeUsed(user);
        }
    }
}
