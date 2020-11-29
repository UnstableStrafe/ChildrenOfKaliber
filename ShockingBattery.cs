using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
namespace Items
{
    class ShockingBattery : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Shocking Battery";

            string resourceName = "Items/Resources/shocking_battery.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<ShockingBattery>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Unlimited Power!";
            string longDesc = "While active, increases movement speed, reload speed, rate of fire, shot speed, and dodge roll speed. \n\nOriginally a prototype for infinite energy, this battery became super-charged " +
                "after a careless scientist spilled a packet of sugar into the battery acid.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 700);

            item.consumable = false;
            item.quality = ItemQuality.S;
            item.sprite.IsPerpendicular = true;
        }

        float speedBuff = -1;
        float duration = 8f;
        protected override void DoEffect(PlayerController user)
        {

            AkSoundEngine.PostEvent("Play_WPN_zapper_shot_01", base.gameObject);

            StartEffect(user);

            StartCoroutine(ItemBuilder.HandleDuration(this, duration, user, EndEffect));
        }

        private void StartEffect(PlayerController user)
        {
            AddStat(PlayerStats.StatType.MovementSpeed, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            AddStat(PlayerStats.StatType.ReloadSpeed, .5f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            AddStat(PlayerStats.StatType.DodgeRollSpeedMultiplier, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            AddStat(PlayerStats.StatType.RateOfFire, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            AddStat(PlayerStats.StatType.ProjectileSpeed, 1.3f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            speedBuff = 2;
            user.stats.RecalculateStats(user, false, false);
        }

        private void EndEffect(PlayerController user)
        {
            if (speedBuff <= 0) return;
            RemoveStat(PlayerStats.StatType.MovementSpeed);

            if (speedBuff <= 0) return;
            RemoveStat(PlayerStats.StatType.ReloadSpeed);

            if (speedBuff <= 0) return;
            RemoveStat(PlayerStats.StatType.DodgeRollSpeedMultiplier);

            if (speedBuff <= 0) return;
            RemoveStat(PlayerStats.StatType.RateOfFire);

            if (speedBuff <= 0) return;
            RemoveStat(PlayerStats.StatType.ProjectileSpeed);

            speedBuff = -1;
            user.stats.RecalculateStats(user, false, false);
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

        /*public override void Pickup(PlayerController player)
        {
            if (this.m_pickedUp)
            {
                return;
            }
            this.m_player = player;
            if (this.ConfersElectricityImmunity)
            {
                this.m_electricityImmunity = new DamageTypeModifier();
                this.m_electricityImmunity.damageMultiplier = 0f;
                this.m_electricityImmunity.damageType = CoreDamageTypes.Electric;
                player.healthHaver.damageTypeModifiers.Add(this.m_electricityImmunity);
            }

        }
            
            private DamageTypeModifier m_electricityImmunity;
            public bool ConfersElectricityImmunity;
           */// private PlayerController m_player;
    }       

}

