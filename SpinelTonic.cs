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
    class SpinelTonic : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Spinel Tonic";

            string resourceName = "Items/Resources/spinel_tonic.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<SpinelTonic>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Just A Sip";
            string longDesc = "A vial containing a seemingly endless amount of an empowering fluid. Each use increases the power of the boost. Gives a slight stat down while held." +
                "\n\nBrought from a strange planet caught in a time loop not unlike the Gungeon's itself, this tonic is said to be highly addictive.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.A;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, .9f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.RateOfFire, .9f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MovementSpeed, .9f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ReloadSpeed, 1.1f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 200f);
            item.sprite.IsPerpendicular = true;
            item.CanBeDropped = true;
            item.CanBeSold = true;
            item.AddToSubShop(ItemBuilder.ShopType.Goopton);
        }
        private float duration = 12f;
        protected override void DoEffect(PlayerController user)
        {
            StartEffect(user);
            
            StartCoroutine(ItemBuilder.HandleDuration(this, duration, user, EndEffect));

        }
        private StatModifier DMG;
        private StatModifier FireR;
        private StatModifier SPD;
        private StatModifier RLD;
        private StatModifier ACC;
        private StatModifier DGS;
        private void StartEffect(PlayerController user)
        {
            this.CanBeDropped = false;
            this.CanBeSold = false;
            DMG = new StatModifier {amount = (Uses * .05f), statToBoost = PlayerStats.StatType.Damage, modifyType = StatModifier.ModifyMethod.ADDITIVE};
            FireR = new StatModifier {amount = (Uses * .08f), statToBoost = PlayerStats.StatType.RateOfFire, modifyType = StatModifier.ModifyMethod.ADDITIVE};
            SPD = new StatModifier {amount = (Uses * .035f), statToBoost = PlayerStats.StatType.MovementSpeed, modifyType = StatModifier.ModifyMethod.ADDITIVE};
            RLD = new StatModifier {amount = -(Uses * .05f), statToBoost = PlayerStats.StatType.ReloadSpeed, modifyType = StatModifier.ModifyMethod.ADDITIVE};
            DGS = new StatModifier {amount = (Uses * .035f), statToBoost = PlayerStats.StatType.DodgeRollSpeedMultiplier, modifyType = StatModifier.ModifyMethod.ADDITIVE};

            user.ownerlessStatModifiers.Add(DMG);
            user.ownerlessStatModifiers.Add(FireR);
            user.ownerlessStatModifiers.Add(SPD);
            user.ownerlessStatModifiers.Add(RLD);
            user.ownerlessStatModifiers.Add(DGS);
            user.stats.RecalculateStats(user, false, false);
        }

        private void EndEffect(PlayerController user)
        {

            user.ownerlessStatModifiers.Remove(DMG);
            user.ownerlessStatModifiers.Remove(FireR);
            user.ownerlessStatModifiers.Remove(SPD);
            user.ownerlessStatModifiers.Remove(RLD);
            user.ownerlessStatModifiers.Remove(DGS);
            user.stats.RecalculateStats(user, false, false);
            this.Uses += 1f;
            this.CanBeDropped = true;
            this.CanBeSold = true;
        }
        private float Uses;
        
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        public DebrisObject Drop(PlayerController player)
        {
            return base.Drop(player);
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
