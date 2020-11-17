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

namespace Items
{
    class Gunlust : PassiveItem
    {
        public static void Init()
        {

            string itemName = "Gunlust";
            string resourceName = "Items/Resources/gunlust.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<Gunlust>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Bloody Work";
            string longDesc = "Killing enemies increases damage up to +15%. Taking damage reduces the boost. Going to a new floor resets the boost. Gives 1.5 curse" +
                "\n\nA representation of the Tracker's bloodlust, given form by the Gungeon.";
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1.5f, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            item.quality = PickupObject.ItemQuality.D;
            item.sprite.IsPerpendicular = true;
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
        }
        private float Boost = 0f;
        //   private float TimerBoost = 0f;
        private float LastCheckBoost = -1;
        private float CheckBoost = 0;
        private float TrueColorBoost;
        private float ColorBoost;
      //  private float ColorAp;
        private PlayerController m_player;
        private void Kill(PlayerController player)
        {

            Boost += .005f;
            CheckBoost += 1;
            ColorBoost += 7;
          //  ColorAp += .033f;
            
         //   if(ColorAp > .99f)
           // {
        //       ColorAp = .99f;
         //   }
            if(ColorBoost > 210)
            {
                ColorBoost = 210;
            }
            if(Boost > .15f)
            {
                Boost = .15f;
            }
           // ETGModConsole.Log("Boost AMT: " + Boost.ToString());
        }

        private void Damaged(PlayerController player)
        {
            Boost -= .05f;
            ColorBoost -= 70f;
          //  ColorAp -= .33f;
          //  if(ColorAp < 0)
          //  {
          //      ColorAp = 0;
          //  }
            if(ColorBoost < 0)
            {
                ColorBoost = 0;
            }
            if (Boost < 0)
            {
                Boost = 0;
            }
            CheckBoost -= 1;
        }
        private void NewFloor(PlayerController player)
        {
            Boost = 0f;
            CheckBoost = 0;
            LastCheckBoost = -1;
        }
    
        private void Stats()
        {
           
            if (CheckBoost == LastCheckBoost) return;

            RemoveStat(PlayerStats.StatType.Damage);
            
            float TrueBoost = Boost + 1f;
            if(ColorBoost == 0)
            {
                TrueColorBoost = 0f;
            }
            else
            {
                TrueColorBoost = ColorBoost +45;
            }
            
         
            
            AddStat(PlayerStats.StatType.Damage, TrueBoost, StatModifier.ModifyMethod.MULTIPLICATIVE);
            this.Owner.stats.RecalculateStats(Owner, true);
            LastCheckBoost = CheckBoost;
        }


        protected override void Update()
        {
            base.Update();
            this.Stats();
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            Boost = 0;
            CheckBoost = 0f;
            LastCheckBoost = -1f;
            player.OnKilledEnemy += this.Kill;
            player.OnReceivedDamage += this.Damaged;
            player.OnNewFloorLoaded += this.NewFloor;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debris = base.Drop(player);

            player.OnKilledEnemy -= this.Kill;
            player.OnReceivedDamage -= this.Damaged;
            player.OnNewFloorLoaded -= this.NewFloor;
            Boost = 0;
            CheckBoost = 0f;
            LastCheckBoost = -1f;
            debris.GetComponent<Gunlust>().m_pickedUpThisRun = true;
            return debris;
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
