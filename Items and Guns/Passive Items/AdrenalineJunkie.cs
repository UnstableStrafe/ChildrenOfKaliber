using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using UnityEngine;
using Alexandria.ItemAPI;


namespace Items
{
    class AdrenalineJunkie : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Adrenaline Junkie";

            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<AdrenalineJunkie>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "So You Like To Play Hardball, Do Ya?";
            string longDesc = "Grants increased damage, rate of fire, coolness, movement speed, and a chance to block damage when at half a heart. Gives Spare Blood Jar on pickup.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;
        }
        float timeSinceChecked = 0;
        public override void Update()
        {
            base.Update();
            if(Owner != null)
            {
                if(timeSinceChecked == 0)
                {
                    timeSinceChecked += Time.deltaTime;
                }
                if(timeSinceChecked >= 2)
                {
                    timeSinceChecked = 0;
                    RunHPCheck();
                }
            }
        }
        public void RunHPCheck()
        {
            if(Owner != null)
            {
                DoStatUpdates();
            }
        }
        private float lastHpAmount = -2;
        private void DoStatUpdates()
        {
            if (Owner.healthHaver.GetCurrentHealth() == lastHpAmount) return;
           
            RemoveStat(PlayerStats.StatType.Damage);
            RemoveStat(PlayerStats.StatType.RateOfFire);
            RemoveStat(PlayerStats.StatType.MovementSpeed);
            RemoveStat(PlayerStats.StatType.Coolness);
            Owner.stats.RecalculateStats(Owner, true);
            if (Owner.healthHaver.GetCurrentHealth() != .5f) return;
            AddStat(PlayerStats.StatType.Damage, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            AddStat(PlayerStats.StatType.RateOfFire, 1.4f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            AddStat(PlayerStats.StatType.MovementSpeed, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            AddStat(PlayerStats.StatType.Coolness, 3, StatModifier.ModifyMethod.ADDITIVE);
            lastHpAmount = Owner.healthHaver.GetCurrentHealth();
        }
        public override void Pickup(PlayerController player)
        {
            player.healthHaver.OnHealthChanged += OnHpChanged;
            base.Pickup(player);
        }

        private void OnHpChanged(float resultValue, float maxValue)
        {
            RunHPCheck();
        }

    
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<AdrenalineJunkie>().m_pickedUpThisRun = true;
            player.healthHaver.OnHealthChanged -= OnHpChanged;
            RemoveStat(PlayerStats.StatType.Damage);
            RemoveStat(PlayerStats.StatType.RateOfFire);
            RemoveStat(PlayerStats.StatType.MovementSpeed);
            RemoveStat(PlayerStats.StatType.Coolness);
            Owner.stats.RecalculateStats(Owner, true);
            return debrisObject;
        }
        private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
        {
            StatModifier modifier = new StatModifier();
            modifier.amount = amount;
            modifier.statToBoost = statType;
            modifier.modifyType = method;

            foreach (var m in passiveStatModifiers)
            {
                if (m.statToBoost == statType) return; //don't add duplicates
            }

            if (this.passiveStatModifiers == null)
                this.passiveStatModifiers = new StatModifier[] { modifier };
            else
                this.passiveStatModifiers = this.passiveStatModifiers.Concat(new StatModifier[] { modifier }).ToArray();
        }


        //Removes a stat
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
    }
}
