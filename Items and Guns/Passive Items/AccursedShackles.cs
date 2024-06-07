using System.Collections.Generic;
using System.Linq;
using Alexandria.ItemAPI;
using UnityEngine;
namespace Items
{
    class AccursedShackles : PassiveItem
    {
        public static int id;
        public static void Init()
        {

            string itemName = "Accursed Shackles";
            string resourceName = "Items/Resources/ItemSprites/Passives/accursed_shackle.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<AccursedShackles>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Hold Me Close";
            string longDesc = "Whenever you take damage and you have an empty heart container, you lose that heart container. Killing 100 enemies returns all lost heart containers and gives 30% extra rate of fire. Cannot be dropped until 100 enemies have been killed. Gives 1 curse while held." +
                "\n\nThis shackle was formed from the melted-down corpses of Gundead who swore vengeance upon the name of a Gungeoneer who is now long-dead. These souls have lost any sense of indentity, though they still remember their hatred.";
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1, StatModifier.ModifyMethod.ADDITIVE);
            

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ck");
            item.quality = PickupObject.ItemQuality.D;
            item.sprite.IsPerpendicular = true;
            id = item.PickupObjectId;
        }
        private bool Shackled = true;
        private float MaxHp, CurHp, StoredHp;
        [SerializeField]
        private float killed;
        public override void Update()
        {
            base.Update();

        }
        private void OnTookDamage(PlayerController player)
        {
            MaxHp = player.healthHaver.GetMaxHealth();
            CurHp = player.healthHaver.GetCurrentHealth();

            
            float flag1 = MaxHp - 1f;

            if (flag1 == CurHp && Shackled == true)
            {
                StoredHp += 1f;

                
                StatModifier statModifierH = new StatModifier()
                {
                    statToBoost = PlayerStats.StatType.Health,
                    amount = -1,
                    modifyType = StatModifier.ModifyMethod.ADDITIVE,
                };

                Owner.ownerlessStatModifiers.Add(statModifierH);
                Owner.stats.RecalculateStats(Owner, false, false);
                
            }
        }
        
        private void KillTracker(PlayerController player)
        {
            if(killed < 100)
            {
                killed += 1;
            }
            
            if(killed == 100f && Shackled == true)
            {
                StatModifier statModifierH = new StatModifier()
                {
                    statToBoost = PlayerStats.StatType.Health,
                    amount = StoredHp,
                    modifyType = StatModifier.ModifyMethod.ADDITIVE,
                };
                StatModifier statModifierF = new StatModifier()
                {
                    statToBoost = PlayerStats.StatType.RateOfFire,
                    amount = 1.30f,
                    modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE,
                };
                Owner.ownerlessStatModifiers.Add(statModifierF);
                Owner.ownerlessStatModifiers.Add(statModifierH);
                Owner.stats.RecalculateStats(Owner, false, false);
                Shackled = false;
                if(Shackled == false)
                {
                    this.CanBeDropped = true;
                    this.CanBeSold = true;
                }
                StoredHp = 0f;

            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            if (Shackled == true)
            {
                this.CanBeDropped = false;
                this.CanBeSold = false;
            }
            player.OnKilledEnemy += this.KillTracker;
            player.OnReceivedDamage += this.OnTookDamage;
            
        }
        
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnKilledEnemy -= this.KillTracker;
            player.OnReceivedDamage -= this.OnTookDamage;
            debrisObject.GetComponent<AccursedShackles>().m_pickedUpThisRun = true;
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
    }
}
