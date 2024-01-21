using System.Collections.Generic;
using System.Linq;
using Alexandria.ItemAPI;
using UnityEngine;

namespace Items
{
    class VoidBottle : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Void Bottle";

            string resourceName = "Items/Resources/ItemSprites/Actives/empty_bottle.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<VoidBottle>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "It Whispers of Ancient Things";
            string longDesc = "Trades one full heart of health to cleanse one point of curse. \n\nIt feels like it wants something from you... Perhaps you should give it what it asks for?";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ck");

            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 1f);

            //item.numberofuses = 3;
            item.consumable = false;
            item.quality = ItemQuality.C;
            item.sprite.IsPerpendicular = true;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);



        }
        public override void DoEffect(PlayerController user)
        {
            float curHealth = user.healthHaver.GetCurrentHealth();          
            {
                AkSoundEngine.PostEvent("Play_OBJ_dead_again_01", base.gameObject);
                user.healthHaver.ForceSetCurrentHealth(curHealth - 1);
                StatModifier statModifier = new StatModifier();
                statModifier.statToBoost = PlayerStats.StatType.Curse;
                statModifier.amount = -1f;
                statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE; 
                user.ownerlessStatModifiers.Add(statModifier);
                if (user.HasPickupID(631))
                {
                    StatModifier statModifier2 = new StatModifier();
                    statModifier2.statToBoost = PlayerStats.StatType.Coolness;
                    statModifier2.amount = 1f;
                    statModifier2.modifyType = StatModifier.ModifyMethod.ADDITIVE;
                    user.ownerlessStatModifiers.Add(statModifier2);
                }
                user.stats.RecalculateStats(user, false, false);

                
            }
        }
        

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);

        }
        public DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);

            debrisObject.GetComponent<VoidBottle>().m_pickedUpThisRun = true;
            return debrisObject;
        }


        private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
        {
            foreach (var m in passiveStatModifiers)
            {
                if (m.statToBoost == statType) return;
            }

            StatModifier modifier = new StatModifier
            {
                amount = amount,
                statToBoost = statType,
                modifyType = method
            };

            if (this.passiveStatModifiers == null)
                this.passiveStatModifiers = new StatModifier[] { modifier };
            else
                this.passiveStatModifiers = this.passiveStatModifiers.Concat(new StatModifier[] { modifier }).ToArray();
        }

        private void RemoveStat(PlayerStats.StatType statType)
        {
            var newModifiers = new List<StatModifier>();
            for (int i = 0; i < passiveStatModifiers.Length; i++)
            {
                if (passiveStatModifiers[i].statToBoost != statType)
                    newModifiers.Add(passiveStatModifiers[i]);
            }
            this.passiveStatModifiers = newModifiers.ToArray();
        }


        public override bool CanBeUsed(PlayerController user)
        {
            float Curse = user.stats.GetStatValue(PlayerStats.StatType.Curse);

            return user.healthHaver.GetCurrentHealth() > 1 && Curse > 0;
            
            
        }
    }

}
