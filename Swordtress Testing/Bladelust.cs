using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace Items
{
    class Bladelust : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Bladelust";

            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<Bladelust>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "You're Mine!";
            string longDesc = "Killing an enemy provides a damage up to your next attack.\n\nThis curse incurs an unstoppable lust for combat in those who recieve it.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "sts");

            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;
        }
        private void KillBoost(PlayerController player)
        {
            if (!hasBoost)
            {
                hasBoost = true;
                AddStat(PlayerStats.StatType.Damage, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(Owner.sprite);
                outlineMaterial.SetColor("_OverrideColor", new Color(252f, 56f, 56f, 50f));
            }
        }
        
        private void UseBoost(PlayerController player, float num, bool fatal, HealthHaver spapi)
        {
            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(Owner.sprite);
            outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f, 0f));
            RemoveStat(PlayerStats.StatType.Damage);
        }
        public override void Pickup(PlayerController player)
        {
            player.OnKilledEnemy += KillBoost;
            player.OnDealtDamageContext += UseBoost;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<Bladelust>().m_pickedUpThisRun = true;
            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(Owner.sprite);
            outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f, 0f));
            player.OnKilledEnemy -= KillBoost;
            player.OnDealtDamageContext -= UseBoost;
            RemoveStat(PlayerStats.StatType.Damage);
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
        private bool hasBoost = false;
    }
}
