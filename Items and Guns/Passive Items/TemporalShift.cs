using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Alexandria.ItemAPI;
using Gungeon;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Items
{
    class TemporalShift : PassiveItem
    {
        //adds small chance to negate damage. when damage is negated, give player increased rate of fire and apply hologram shader for a few sec
        public static void Init()
        {
            string itemName = "Temporal Shift";

            string resourceName = "Items/Resources/ItemSprites/Passives/temporal_shift.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<TemporalShift>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Going Ghost";
            string longDesc = "Adds a chance to dodge enemy attacks. After dodging, gain an increased rate of fire for a short time.\n\nMade by the Pursued, this gadget allows them to briefly swap places with a version of themselves that hadn't been harmed in the event of an emergency. The shift in timelines allows them to use any projectiles the other one had shot for a short time.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.D;
            item.sprite.IsPerpendicular = true;
            item.CanBeDropped = false;
            item.CanBeSold = false;

        }
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.healthHaver.ModifyDamage += this.PreventDamage;
            zoomy = player.gameObject.AddComponent<ImprovedAfterImage>();
            zoomy.dashColor = new Color(33, 71, 237);
            zoomy.spawnShadows = false;
            zoomy.shadowTimeDelay = 0.05f;
            zoomy.shadowTimeDelay = 0.05f;
            zoomy.shadowLifetime = 0.3f;
            zoomy.minTranslation = 0.05f;
            zoomy.OverrideImageShader = ShaderCache.Acquire("Brave/Internal/DownwellAfterImage");
        }

		public override DebrisObject Drop(PlayerController player)
		{
			player.healthHaver.ModifyDamage -= this.PreventDamage;
			return base.Drop(player);
		}

		public override void OnDestroy()
		{
			base.Owner.healthHaver.ModifyDamage -= this.PreventDamage;
			base.OnDestroy();
		}

		private void PreventDamage(HealthHaver healthHaver, HealthHaver.ModifyDamageEventArgs args)
		{
			if (args == EventArgs.Empty)
			{
				return;
			}
			float num = Random.Range(0f, 1f);
			if (num <= 0.15f)
			{				
				args.ModifiedDamage = 0;
                if (!zoomy.spawnShadows)
                {
                    zoomy.spawnShadows = true;
                }
                StartCoroutine(PhaseBoost());
                
			}
		}
		private IEnumerator PhaseBoost()
        {
			float elapsed = 0;
            PlayerController player = this.m_owner;
            AddStat(PlayerStats.StatType.RateOfFire, 1.3f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            player.stats.RecalculateStats(player, false, false);
            player.sprite.usesOverrideMaterial = true;
            Material material = player.sprite.renderer.material;
            material.shader = ShaderCache.Acquire("Brave/Internal/HologramShader");
            material.SetFloat("_IsGreen", 0f);
            while (elapsed < 2.5)
            {
				elapsed += BraveTime.DeltaTime;
                yield return null;
            }
            player.sprite.usesOverrideMaterial = false;
            player.ClearOverrideShader();
            RemoveStat(PlayerStats.StatType.RateOfFire);
            if (zoomy.spawnShadows)
            {
                zoomy.spawnShadows = false;
            }
            player.stats.RecalculateStats(player, false, false);
            yield break;

        }
        private ImprovedAfterImage zoomy;
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
