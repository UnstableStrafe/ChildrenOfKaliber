using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using Gungeon;


namespace Items
{
    class Akffinity : PassiveItem
    {


        public static void Init()
        {
            string itemName = "Akffinity";

            string resourceName = "Items/Resources/ItemSprites/Passives/akffinity.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<Akffinity>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Master of One";
            string longDesc = "Increases the power of any Ak-type weapons the player has. Also increases the chances to find Ak-type weapons.\n\nA necklace with a miniture model of an Ak-47 hanging from it." +
                " Wearing it bestows an esoteric knowledge of all things Ak-related.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.C;
            item.sprite.IsPerpendicular = true;
            List<LootModData> lootModDatas = new List<LootModData> {};
            foreach(int i in akIds)
            {
                LootModData mod = new LootModData
                {
                    AssociatedPickupId = i,
                    DropRateMultiplier = 5,
                };
                lootModDatas.Add(mod);
            }
            item.associatedItemChanceMods = lootModDatas.ToArray();
            

        }
        public override void Update()
        {
            base.Update();
            DoGunCheck();
        }

        private void DoGunCheck()
        {
            Gun gun = Owner.CurrentGun;
            if (gun == cached_gun) return;
            cached_gun = gun;
            if (!akIds.Contains(gun.PickupObjectId)) return;
            RemoveStat(PlayerStats.StatType.Damage);
            RemoveStat(PlayerStats.StatType.Accuracy);
            RemoveStat(PlayerStats.StatType.AdditionalClipCapacityMultiplier);
            this.Owner.stats.RecalculateStats(Owner, true);
            AddStat(PlayerStats.StatType.Damage, 1.25f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            AddStat(PlayerStats.StatType.Accuracy, .90f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            AddStat(PlayerStats.StatType.AdditionalClipCapacityMultiplier, 1.25f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            this.Owner.stats.RecalculateStats(Owner, true);
            
        }

        public override void Pickup(PlayerController player)
        {

            base.Pickup(player);
            
        }
            
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<Akffinity>().m_pickedUpThisRun = true;
           
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

        public static bool didSetupIds = false;

        public static List<string> moddedAks = new List<string> 
        {
           
            "nn:kalashnirang",
            "nn:gayk47",
            "bot:kr82m",
            "ski:ack-ch00",
            "ski:toy_ak",
        };

        public static List<int> akIds = new List<int>
        {
            15,
            29,
            95,
            221,
            510,
            611,
            726,
            AK94.AK94ID,
            AK141.AK141ID,
            AK188.AK188ID,
            InfiniteAK.AKINFID,
            Pray_K47.Id,
        
        };

        private Gun cached_gun = null;

    }
}
