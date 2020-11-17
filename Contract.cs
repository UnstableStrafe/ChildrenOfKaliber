using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Items
{
    class Contract : PassiveItem
    {
        public static void Init()
        {

            string itemName = "Contract";
            string resourceName = "Items/Resources/contract.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<Contract>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "With The Devil";
            string longDesc = "Shop prices are reduced, but buying curses you.\n\nBello takes his shopkeeping very seriously.";

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.GlobalPriceMultiplier, .70f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            item.quality = PickupObject.ItemQuality.C;
            item.sprite.IsPerpendicular = true;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);

        }

        private void OnBuy(PlayerController player, ShopItemController shop)
        {
            AddStat(PlayerStats.StatType.Curse, 1, StatModifier.ModifyMethod.ADDITIVE);
            player.stats.RecalculateStats(player, false, false);
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
