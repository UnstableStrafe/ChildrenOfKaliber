using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using UnityEngine;
using Alexandria.ItemAPI;

namespace Items
{
    class SharpenedGrindstone : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Sharpened Grindstone";

            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<SharpenedGrindstone>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Only A Bit Overkill";
            string longDesc = "Attacks may make enemies bleed.\n\nWhile it seems like a redundant idea to sharpen a grindstone, it is in fact redundant.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "sts");

            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;

        }
        private SwordtressBleedingEffect bleedingEffect = new SwordtressBleedingEffect
        {
            DamagePerSecondToEnemies = 5,
            duration = 8,
            AffectsEnemies = true,
            effectIdentifier = "Grindstone Bleeding",
            ignitesGoops = false,
        };
        private void FuckYou(Projectile proj, float eff)
        {
            proj.AppliesPoison = true;
            proj.PoisonApplyChance = .35f * eff;
            proj.healthEffect = bleedingEffect;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += FuckYou;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<SharpenedGrindstone>().m_pickedUpThisRun = true;
            player.PostProcessProjectile -= FuckYou;
            return debrisObject;
        }
    }
}
