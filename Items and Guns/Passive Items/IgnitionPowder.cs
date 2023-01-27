using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using Gungeon;

namespace Items
{
    class IgnitionPowder : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Ignition Powder";

            string resourceName = "Items/Resources/ItemSprites/Passives/ignition_powder.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<IgnitionPowder>();
            //obj.AddComponent<RiskParticles>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Thermite Wake-up Call";
            string longDesc = "Increases damage while on fire. Reloading an empty clip spawns fire below the player. Increases the time it takes for the fire gauge to fill up. Gives 2 Risk.\nDoes NOT grant fire immunity!\n\nI feel like there's a joke about the Vietnam War I could make here.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            //item.RiskToGive = 2;
            item.quality = ItemQuality.C;
            item.sprite.IsPerpendicular = true;


        }
        private void Oil(PlayerController player, Gun gun)
        {
            if (player.CurrentGun.ClipShotsRemaining == 0)
            {
                var ddgm = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(Library.goopDefs[0]);
                ddgm.TimedAddGoopCircle(Owner.sprite.WorldCenter, 3f, .1f);
            }
        }
        public override void Update()
        {
            base.Update();
            FireCheck();
        }

        private bool? check = false, lastCheck = null;
        private void FireCheck()
        {
            check = Owner.IsOnFire;
            if (check == lastCheck) return;
            if (check == true)
            {
                AddStat(PlayerStats.StatType.Damage, 1.45f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            }
            else if(check == false)
            {
                RemoveStat(PlayerStats.StatType.Damage);
            }
            this.Owner.stats.RecalculateStats(Owner, true);
            lastCheck = check;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            this.m_fireImmunity = new DamageTypeModifier();
            this.m_fireImmunity.damageMultiplier = .65f;
            this.m_fireImmunity.damageType = CoreDamageTypes.Fire;
            player.healthHaver.damageTypeModifiers.Add(this.m_fireImmunity);
            player.OnReloadedGun += this.Oil;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<IgnitionPowder>().m_pickedUpThisRun = true;
            player.OnReloadedGun -= this.Oil;
            player.healthHaver.damageTypeModifiers.Remove(this.m_fireImmunity);
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
        private DamageTypeModifier m_fireImmunity;
    }
}
