using System;
using System.Linq;
using System.Collections.Generic;
using Alexandria.ItemAPI;
using System.Collections;
using UnityEngine;
using Gungeon;
using Dungeonator;

namespace Items 
{ 
    class BleakerCrown : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Bleaker Crown";

            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<PaintedCrown>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Cooler Than You";
            string longDesc = "Increases stats based on coolness. Gives 2 coolness.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ck");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Coolness, 2);
            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;

        }
		float Coolness = 0, lastCoolness = -1;
		public override void Update()
        {
            base.Update();
			Coolness = Owner.stats.GetStatValue(PlayerStats.StatType.Coolness);
			if (Coolness == lastCoolness) return;
			RemoveStat(PlayerStats.StatType.Damage);
			RemoveStat(PlayerStats.StatType.RateOfFire);
			RemoveStat(PlayerStats.StatType.Accuracy);

			AddStat(PlayerStats.StatType.Damage, 1 + (Coolness * .08f), StatModifier.ModifyMethod.MULTIPLICATIVE);
			AddStat(PlayerStats.StatType.RateOfFire, 1 + (Coolness * .08f), StatModifier.ModifyMethod.MULTIPLICATIVE);
			AddStat(PlayerStats.StatType.Accuracy, 1 + (Coolness * .10f), StatModifier.ModifyMethod.MULTIPLICATIVE);
			Owner.stats.RecalculateStats(base.Owner, true, false);
			lastCoolness = Coolness;
		}
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<BleakerCrown>().m_pickedUpThisRun = true;
            RemoveStat(PlayerStats.StatType.Damage);
			RemoveStat(PlayerStats.StatType.RateOfFire);
			RemoveStat(PlayerStats.StatType.Accuracy);
            player.stats.RecalculateStats(base.Owner, true, false);
            return debrisObject;
        }
		private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
		{
			StatModifier statModifier = new StatModifier();
			statModifier.amount = amount;
			statModifier.statToBoost = statType;
			statModifier.modifyType = method;
			foreach (StatModifier statModifier2 in this.passiveStatModifiers)
			{
				bool flag = statModifier2.statToBoost == statType;
				if (flag)
				{
					return;
				}
			}
			bool flag2 = this.passiveStatModifiers == null;
			if (flag2)
			{
				this.passiveStatModifiers = new StatModifier[]
				{
					statModifier
				};
				return;
			}
			this.passiveStatModifiers = this.passiveStatModifiers.Concat(new StatModifier[]
			{
				statModifier
			}).ToArray<StatModifier>();
		}

		private void RemoveStat(PlayerStats.StatType statType)
		{
			List<StatModifier> list = new List<StatModifier>();
			for (int i = 0; i < this.passiveStatModifiers.Length; i++)
			{
				bool flag = this.passiveStatModifiers[i].statToBoost != statType;
				if (flag)
				{
					list.Add(this.passiveStatModifiers[i]);
				}
			}
			this.passiveStatModifiers = list.ToArray();
		}
	}
}
