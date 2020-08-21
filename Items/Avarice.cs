
using System.Collections.Generic;
using System.Linq;
using ItemAPI;
using UnityEngine;


namespace Items
{
    class Avarice : PassiveItem
    {
        public static void Init()
        {

            string itemName = "Avarice";
            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<Avarice>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Magnets!";
            string longDesc = "Gives flight for 10 seconds after using a blank.\n\nThe electromagnetic receptors in these boots begin to vibrate when in the presence of blanks.";



            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            item.quality = PickupObject.ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;


        }
        private int currency = 0, lastCurrency = 0, currencyAMT, BankedCur;
        protected override void Update()
        {
            base.Update();
            Stats();
        }
        private void Stats()
        {
            
            float DMG;
            currencyAMT = Owner.carriedConsumables.Currency;
            currency = currencyAMT;
            BankedCur = currencyAMT + BankedCur;
            if (currency == lastCurrency) return;
            RemoveStat(PlayerStats.StatType.Damage);
            float math1 = BankedCur * .005f;
            if(math1 < 1)
            {
                DMG = math1 + 1;
            }
            else
            {
                DMG = math1;
            }
            AddStat(PlayerStats.StatType.Damage, DMG, StatModifier.ModifyMethod.MULTIPLICATIVE);
            Owner.carriedConsumables.Currency = 0;
            currency = 0;
        }
        public override void Pickup(PlayerController player)
        {
            if (this.m_pickedUp)
            {
                return;
            }
            base.Pickup(player);

        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<Avarice>().m_pickedUpThisRun = true;

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
