using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using UnityEngine;
using Alexandria.ItemAPI;


namespace Items
{
    class HairGel : RiskPassiveItem
    {
        public static void Init()
        {
            string itemName = "Hair Gel";

            string resourceName = "Items/Resources/ItemSprites/Passives/hair_gel.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<HairGel>();
            obj.AddComponent<RiskParticles>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Best Used Under Pressure";
            string longDesc = "Increases Risk by 1. Increases movement speed and slightly decreases reload speed per Risk point.\n\nThis slick hair gel makes the very air slide off of your body. It also makes it harder to reload bullets into your gun, but who really cares about that?";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.RiskToGive = 1;
            item.quality = ItemQuality.C;
            item.sprite.IsPerpendicular = true;
        }
        public override void Update()
        {
            base.Update();
            CalculateStats();
        }
        private float risk, lastRisk = -1;
        private void CalculateStats()
        {
            risk = Owner.gameObject.GetOrAddComponent<RiskStat>().RiskAMT;
            if (risk == lastRisk) return;
            RemoveStat(PlayerStats.StatType.MovementSpeed);
            RemoveStat(PlayerStats.StatType.ReloadSpeed);
            this.Owner.stats.RecalculateStats(Owner, true);
            float num1 = Mathf.Clamp(risk * .10f, 0, .6f);
            float num2 = Mathf.Clamp(risk * .07f, 0, .4f);
            AddStat(PlayerStats.StatType.MovementSpeed, num1 + 1, StatModifier.ModifyMethod.MULTIPLICATIVE);
            AddStat(PlayerStats.StatType.ReloadSpeed, num2 + 1, StatModifier.ModifyMethod.MULTIPLICATIVE);
            this.Owner.stats.RecalculateStats(Owner, true);
            lastRisk = risk;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<HairGel>().m_pickedUpThisRun = true;
            this.Owner.stats.RecalculateStats(Owner, true);
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
                if (m.statToBoost == statType) return;
            }

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
    }
}
