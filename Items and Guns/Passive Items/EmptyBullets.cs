using System.Collections.Generic;
using System.Linq;
using Alexandria.ItemAPI;
using UnityEngine;

namespace Items
{
    class EmptyBullets : PassiveItem
    {
        
        public static void Init()
        {

            string itemName = "[_____] Bullets";
            string resourceName = "Items/Resources/ItemSprites/Passives/empty_bullets.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<EmptyBullets>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Missing Description";
            string longDesc = "Increases damage for each carried blank.\n\nWhoever created these bullets was clearly too lazy to actually make anything beyond a void in time and space.";
            

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            item.quality = PickupObject.ItemQuality.C;
            item.sprite.IsPerpendicular = true;
            item.AddToSubShop(ItemBuilder.ShopType.OldRed);
           
        }


        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);

        }

        public override void Update()
        {
            base.Update();
            BlankAmount();
        }
        private int blanksInt;
        private float blanks = 0, lastBlanks = -1;
        private void BlankAmount()
        {
            if(Owner != null)
            {
                blanksInt = m_owner.Blanks;
                blanks = blanksInt;
                //     ETGModConsole.Log(blanks.ToString());
                if (blanks == lastBlanks) return;

                RemoveStat(PlayerStats.StatType.Damage);
                this.Owner.stats.RecalculateStats(Owner, true);
                AddStat(PlayerStats.StatType.Damage, blanks * .10f);
                this.Owner.stats.RecalculateStats(Owner, true);
       
                lastBlanks = blanks;
            }
           
        }


        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<EmptyBullets>().m_pickedUpThisRun = true;

            return debrisObject;
        }
       // private PlayerController m_owner;



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
