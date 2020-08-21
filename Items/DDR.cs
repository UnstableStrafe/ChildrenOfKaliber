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
    class DDR : PassiveItem
    {
        public static void Init()
        {

            string itemName = "DDR";
            string resourceName = "Items/Resources/ddr.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<DDR>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Dance Dance Revolution";
            string longDesc = "Moving in a direction increases a stat- Left: Damage, Right: Shot speed, Up: Rate of fire, Down: Reload speed.\n\nThe gundead have been known to play this game in their spare time.";



            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            item.quality = PickupObject.ItemQuality.D;
            item.sprite.IsPerpendicular = true;
            

        }
        private float AngVel, AngVelLast = -1f;
        protected override void Update()
        {
            base.Update();
            this.Movement();
        }

        private void Movement()
        {
            //AngVel = Owner.specRigidbody.Velocity.ToAngle();
            AngVel = Owner.NonZeroLastCommandedDirection.ToAngle();
            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(Owner.sprite);
            if (AngVel == AngVelLast) return;
            RemoveStat(PlayerStats.StatType.ProjectileSpeed);
            RemoveStat(PlayerStats.StatType.Damage);
            RemoveStat(PlayerStats.StatType.RateOfFire);
            RemoveStat(PlayerStats.StatType.ReloadSpeed);

            this.DisableVFX(Owner);
            if (AngVel == 0f)
            {
                AddStat(PlayerStats.StatType.ProjectileSpeed, 1.3f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                outlineMaterial.SetColor("_OverrideColor", new Color(54f, 208f, 255f, 50f));
            }

            if(AngVel == 90f)
            {
                AddStat(PlayerStats.StatType.RateOfFire, 1.3f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                outlineMaterial.SetColor("_OverrideColor", new Color(232f, 255f, 54f, 50f));
            }   

            if(AngVel == -180f)
            {
                AddStat(PlayerStats.StatType.Damage, 1.3f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                outlineMaterial.SetColor("_OverrideColor", new Color(252f, 56f, 56f, 50f));
            }

            if(AngVel == -90f)
            {
                AddStat(PlayerStats.StatType.ReloadSpeed, .7f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                outlineMaterial.SetColor("_OverrideColor", new Color(54f, 255f, 121f, 50f));
            }
            
            this.Owner.stats.RecalculateStats(Owner, true);
            AngVelLast = AngVel;
        }
        private void DisableVFX(PlayerController user)
        {
            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(user.sprite);
            outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
        }
        public override DebrisObject Drop(PlayerController player)
        {
            this.DisableVFX(player);
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<DDR>().m_pickedUpThisRun = true;
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
