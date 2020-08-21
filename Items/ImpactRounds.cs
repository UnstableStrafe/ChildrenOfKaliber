using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;
using Dungeonator;

namespace Items
{
    class ImpactRounds : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Ballistic Rounds";

            string resourceName = "Items/Resources/impact_rounds.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<ImpactRounds>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Osmium Core";
            string longDesc = "Projectiles break the boss damage cap, but boss damage is slashed in half (I know this may seem like a bad thing, but trust me when I say without it, bosses would be a joke. 50% might not be low enough, honestly)\n\nBuilt from one of the densest" +
                " metals, these bullets don't have armor-piercing shells, but instead are meant to deal crushing damage to objects with a bigger surface area.";
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.DamageToBosses, .5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.S;
            item.sprite.IsPerpendicular = true;


        }
        private void PostProj(Projectile proj, float eff)
        {
            proj.ignoreDamageCaps = true;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += PostProj;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<ImpactRounds>().m_pickedUpThisRun = true;
            player.PostProcessProjectile -= PostProj;
            return debrisObject;
        }
    }
}
