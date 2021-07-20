using System.Collections.Generic;
using System.Linq;
using ItemAPI;
using UnityEngine;

namespace Items
{
    class OverclockingUnit : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Overclocking Unit";

            string resourceName = "Items/Resources/water_cup.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<OverclockingUnit>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Cracking Open A Cold Water";
            string longDesc = "On use, refills the current gun's clip. Charges are restored on purchasing an item, but only when out of charges. Increases coolness by 2 while held.\n\nSometimes in a firefight, all you need is a glass of cool water. Thankfully Bello has a steady supply.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;
            item.CanBeDropped = true;
            item.CanBeSold = true;

            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.PerRoom, 99999f);
            item.consumable = false;

            item.AddToSubShop(ItemBuilder.ShopType.Goopton);
        }

        protected override void DoEffect(PlayerController user)
        {
            base.DoEffect(user);
            int Clip = user.CurrentGun.ClipCapacity;
            AddStat(PlayerStats.StatType.RateOfFire, 100f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            AddStat(PlayerStats.StatType.ChargeAmountMultiplier, 100f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            user.stats.RecalculateStats(user, false, false);
            user.inventory.GunLocked.SetOverride("Overclocked", true, null);
            user.CurrentGun.AdditionalClipCapacity = user.CurrentGun.GetBaseMaxAmmo() - user.CurrentGun.ClipCapacity;
            do
            {
                user.CurrentGun.Attack();
            }
            while (user.CurrentGun.CurrentAmmo > 0);
            user.inventory.GunLocked.RemoveOverride("Overclocked");
            RemoveStat(PlayerStats.StatType.RateOfFire);
            RemoveStat(PlayerStats.StatType.ChargeAmountMultiplier);
            user.stats.RecalculateStats(user, false, false);
            user.CurrentGun.AdditionalClipCapacity = 0;

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
