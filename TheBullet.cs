using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;
namespace Items
{
    class TheBullet : PlayerItem
    {
        public static void Init()
        {
            string itemName = "The Bullet";

            string resourceName = "Items/Resources/TheBullet.png";

            GameObject obj = new GameObject();

            var item = obj.AddComponent<TheBullet>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "A Quiet Requiem";
            string longDesc = "Grants power in exchange for health.\n\n" +
                "This bullet was made from a meteorite containing a killer virus. " +
                "Should someone who has the potential for greatness be shot by the Bullet, they will awaken their spiritual power. However, being suffienctly healthy should also prove effective. " +
                "By just being near it, your soul feels stronger.";


            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 1.5f);


            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 1.20f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            item.consumable = true;
            item.quality = ItemQuality.S;
            item.sprite.IsPerpendicular = true;
        }

        protected override void DoEffect(PlayerController user)
        {
            float curHealth = user.healthHaver.GetMaxHealth();
            if (curHealth > 3)
            {
                AkSoundEngine.PostEvent("Play_BOSS_doormimic_death_01", base.gameObject);

                StatModifier statModifierH = new StatModifier()
                {
                    statToBoost = PlayerStats.StatType.Health,
                    amount = -3,
                    modifyType = StatModifier.ModifyMethod.ADDITIVE,
                };
                StatModifier statModifierD = new StatModifier()
                {
                    statToBoost = PlayerStats.StatType.Damage,
                    amount = 2f,
                    modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE,

                };
                StatModifier statModifierR = new StatModifier()
                {
                    statToBoost = PlayerStats.StatType.RateOfFire,
                    amount = 1.5f,
                    modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE,
                };
                StatModifier statModifierM = new StatModifier()
                {
                    statToBoost = PlayerStats.StatType.MovementSpeed,
                    amount = 1.2f,
                    modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE,
                };
                user.ownerlessStatModifiers.Add(statModifierM);
                user.ownerlessStatModifiers.Add(statModifierR);
                user.ownerlessStatModifiers.Add(statModifierD);
                user.ownerlessStatModifiers.Add(statModifierH);
                user.stats.RecalculateStats(user, false, false);

            }
        }




        public override bool CanBeUsed(PlayerController user)
        {
            return user.healthHaver.GetMaxHealth() > 3; //BUILD THE UPDATE NERD
        }
    }
}
