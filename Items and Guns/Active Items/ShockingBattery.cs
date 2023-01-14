using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Alexandria.ItemAPI;
using System.Collections;
namespace Items
{
    class ShockingBattery : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Shocking Battery";

            string resourceName = "Items/Resources/ItemSprites/Actives/shocking_battery.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<ShockingBattery>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Unlimited Power!";
            string longDesc = "While active, increases movement speed, reload speed, rate of fire, shot speed, and dodge roll speed. \n\nOriginally a prototype for infinite energy, this battery became super-charged " +
                "after a careless scientist spilled a packet of sugar into the battery acid.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 700);

            item.consumable = false;
            item.quality = ItemQuality.S;
            item.sprite.IsPerpendicular = true;
        }

        float speedBuff = -1;
        float duration = 8f;
        public override void DoEffect(PlayerController user)
        {

            AkSoundEngine.PostEvent("Play_WPN_zapper_shot_01", base.gameObject);

            StartEffect(user);

            StartCoroutine(ItemBuilder.HandleDuration(this, duration, user, EndEffect));
        }

        private void StartEffect(PlayerController user)
        {
            AddStat(PlayerStats.StatType.MovementSpeed, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            AddStat(PlayerStats.StatType.ReloadSpeed, .5f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            AddStat(PlayerStats.StatType.DodgeRollSpeedMultiplier, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            AddStat(PlayerStats.StatType.RateOfFire, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            AddStat(PlayerStats.StatType.ProjectileSpeed, 1.3f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            this.m_electricityImmunity = new DamageTypeModifier();
            this.m_electricityImmunity.damageMultiplier = 0f;
            this.m_electricityImmunity.damageType = CoreDamageTypes.Electric;
            user.healthHaver.damageTypeModifiers.Add(this.m_electricityImmunity);
            speedBuff = 2;
            user.stats.RecalculateStats(user, false, false);
            user.StartCoroutine(DoGoop(user));
            CanBeDropped = false;
            CanBeSold = false;
        }
        private DamageTypeModifier m_electricityImmunity;
        private IEnumerator DoGoop(PlayerController player)
        {
            float elapsed = 0;
            while(elapsed < 8)
            {
                elapsed += BraveTime.DeltaTime;
                DeadlyDeadlyGoopManager ddgm = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(Library.goopDefs[5]);
                ddgm.TimedAddGoopCircle(player.sprite.WorldBottomCenter, 1.25f, .3f);
                ddgm.ElectrifyGoopCircle(player.sprite.WorldBottomCenter, 1.25f);
                yield return new WaitForSeconds(0.005f);
            }
            yield break;
        }
        private void EndEffect(PlayerController user)
        {
            if (speedBuff <= 0) return;
            RemoveStat(PlayerStats.StatType.MovementSpeed);

            if (speedBuff <= 0) return;
            RemoveStat(PlayerStats.StatType.ReloadSpeed);

            if (speedBuff <= 0) return;
            RemoveStat(PlayerStats.StatType.DodgeRollSpeedMultiplier);

            if (speedBuff <= 0) return;
            RemoveStat(PlayerStats.StatType.RateOfFire);

            if (speedBuff <= 0) return;
            RemoveStat(PlayerStats.StatType.ProjectileSpeed);

            speedBuff = -1;
            user.StartCoroutine(DelayedRemoveElectricImmunity(user));
            user.stats.RecalculateStats(user, false, false);
            CanBeDropped = true;
            CanBeSold = true;
        }
        private IEnumerator DelayedRemoveElectricImmunity(PlayerController player)
        {
            float elapsed = 0;
            while(elapsed < 3)
            {
                elapsed += BraveTime.DeltaTime;
                yield return null;
            }
            player.healthHaver.damageTypeModifiers.Remove(m_electricityImmunity);
            yield break;
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

        /*public override void Pickup(PlayerController player)
        {
            if (this.m_pickedUp)
            {
                return;
            }
            this.m_player = player;
            if (this.ConfersElectricityImmunity)
            {
                this.m_electricityImmunity = new DamageTypeModifier();
                this.m_electricityImmunity.damageMultiplier = 0f;
                this.m_electricityImmunity.damageType = CoreDamageTypes.Electric;
                player.healthHaver.damageTypeModifiers.Add(this.m_electricityImmunity);
            }

        }
            
            
            public bool ConfersElectricityImmunity;
           */// private PlayerController m_player;
    }       

}

