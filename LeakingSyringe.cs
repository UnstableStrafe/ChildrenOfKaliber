using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using UnityEngine;
using ItemAPI;
using System.Threading;

namespace Items
{
    class LeakingSyringe : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Leaking Syringe";

            string resourceName = "Items/Resources/leaking_syringe.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<LeakingSyringe>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Power Not-So-Eternal";
            string longDesc = "Gives a massive damage up, but killing enemies reduces damage until the boost is lost.\n\nInfused with the power of a thousand bullet kin. Unfortunately, due to the laws of conservation of energy, each time the power is used, some is lost as slippage.\nBe careful not to drop.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.B;
            item.sprite.IsPerpendicular = true;


        }
        private float Boost = 3f, lastBoost = -1;
        
        protected override void Update()
        {
            base.Update();
            if (Boost == lastBoost) return;
            RemoveStat(PlayerStats.StatType.Damage);
            AddStat(PlayerStats.StatType.Damage, Boost, StatModifier.ModifyMethod.MULTIPLICATIVE);
            this.Owner.stats.RecalculateStats(Owner, true);
            lastBoost = Boost;
        }
        public void Break()
        {
            this.m_pickedUp = true;
            UnityEngine.Object.Destroy(base.gameObject, 1f);
        }
        private void Drain(PlayerController player)
        {
            Boost -= .025f;
            if(Boost < 1)
            {
                Boost = 1;
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnKilledEnemy += this.Drain;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<LeakingSyringe>().m_pickedUpThisRun = true;
            LeakingSyringe thingy = debrisObject.GetComponent<LeakingSyringe>();
            thingy.Break();
            player.OnKilledEnemy -= this.Drain;
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
    }
}
