using System.Collections.Generic;
using System.Collections;
using System.Linq;
using ItemAPI;
using UnityEngine;

namespace Items
{
    class Hoverboard : PassiveItem
    {
        public Hoverboard()
        {
            
        }

        public static void Init()
        {

            string itemName = "Hoverboots";
            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<Hoverboard>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Magnets!";
            string longDesc = "Gives flight for 10 seconds after using a blank.\n\nThe electromagnetic receptors in these boots begin to vibrate when in the presence of blanks.";



            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            item.quality = PickupObject.ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;


        }

        private void Activate(PlayerController arg1, int BlanksRemaining)
        {
            this.m_player.StartCoroutine(this.ActiveDuration());
        }
       
        private IEnumerator ActiveDuration()
        {
            float timer = 0f;
            m_player.SetIsFlying(true, "wings", true, false);
            

            AddStat(PlayerStats.StatType.MovementSpeed, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            m_player.stats.RecalculateStats(m_player, true);
            while (timer < this.LimitDuration)
            {
              //  ETGModConsole.Log(Convert.ToInt32(timer).ToString());
                timer += BraveTime.DeltaTime;
                yield return null;
            }
            RemoveStat(PlayerStats.StatType.MovementSpeed);
            m_player.stats.RecalculateStats(m_player, true);
            m_player.SetIsFlying(false, "wings", true, false);
            ETGModConsole.Log("Hoverboard End");
            yield break;
        }

        public override void Pickup(PlayerController player)
        {
            if (this.m_pickedUp)
            {
                return;
            }
            base.Pickup(player);
            this.m_player = player;
            player.OnUsedBlank += this.Activate;
            PassiveItem.IncrementFlag(player, typeof(WingsItem));
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<Hoverboard>().m_pickedUpThisRun = true;
            PassiveItem.DecrementFlag(player, typeof(WingsItem));
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
        private PlayerController m_player;
        private float LimitDuration = 10f;

        public OverridableBool AdditionalCanDodgeRollWhileFlying;
    }


}
