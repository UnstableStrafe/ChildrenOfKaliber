using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using Gungeon;
using ItemAPI;
using MonoMod;
using MonoMod.RuntimeDetour;
using Random = UnityEngine.Random;
using UnityEngine;
using Dungeonator;

namespace Items
{
    class WaterCup : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Cup of Water";

            string resourceName = "Items/Resources/water_cup.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<WaterCup>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Cracking Open A Cold Water";
            string longDesc = "On use, refills the current gun's clip and activates any 'on reload' effects. Charges are restored on purchasing an item, but only when out of charges. Increases coolness by 2 while held.\n\nSometimes in a firefight, all you need is a glass of cool water. Thankfully Bello has a steady supply.";
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Coolness, 2, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.A;
            item.sprite.IsPerpendicular = true;
            item.CanBeDropped = true;
            item.CanBeSold = true;

            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.PerRoom, 99999f);
            item.consumable = false;
            item.UsesNumberOfUsesBeforeCooldown = true;
            item.numberOfUses = 10;
            item.AddToSubShop(ItemBuilder.ShopType.Goopton);
        }

        protected override void DoEffect(PlayerController user)
        {
            base.DoEffect(user);
            AddStat(PlayerStats.StatType.ReloadSpeed, -1000f);
            user.stats.RecalculateStats(user, false, false);
            user.CurrentGun.Reload();
            RemoveStat(PlayerStats.StatType.ReloadSpeed);
            user.stats.RecalculateStats(user, false, false);
            
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnItemPurchased += this.ResetCooldown;
        }
        public DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnItemPurchased -= this.ResetCooldown;
            return debrisObject;
        }
        private void ResetCooldown(PlayerController player, ShopItemController shop)
        {
            base.ClearCooldowns();
            if(player.HasPickupID(170) || player.HasPickupID(278))
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(73).gameObject, player);
            }
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
