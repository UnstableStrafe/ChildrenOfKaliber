using System;
using Alexandria.ItemAPI;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Gungeon;
using Dungeonator;

namespace Items
{
    class CrownOfTheEnduring : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Crown Of The Enduring";

            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<CrownOfTheEnduring>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "A'Tuin's Power";
            string longDesc = "Gives two max hp. Missing health increases damage.";

			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 2, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
           
            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;

        }
		public override void Update()
        {
            base.Update();
            this.EvaluateStats();
        }
		private float healthPercent = 0f;
		private float lastHealthPercent = -1f;
		private void EvaluateStats()
		{
			PlayerController owner = base.Owner;
			bool flag = !((owner != null) ? owner.healthHaver : null) || !base.Owner.stats;
			if (!flag)
			{
				this.healthPercent = base.Owner.healthHaver.GetCurrentHealthPercentage();
				bool flag2 = this.healthPercent == this.lastHealthPercent;
				if (!flag2)
				{
					this.RemoveStat(PlayerStats.StatType.Damage);
					bool flag3 = this.healthPercent <= 0.75f;
					if (flag3)
					{
						this.AddStat(PlayerStats.StatType.Damage, (healthPercent <= .25f) ? 2 : (this.healthPercent <= 0.50f) ? 0.66f : 0.33f, StatModifier.ModifyMethod.MULTIPLICATIVE);
					}
					base.Owner.stats.RecalculateStats(base.Owner, true, false);
					this.lastHealthPercent = this.healthPercent;
				}
			}

			
		}
		public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<CrownOfTheEnduring>().m_pickedUpThisRun = true;
			RemoveStat(PlayerStats.StatType.Damage);
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
