using Alexandria.ItemAPI;
using UnityEngine;

namespace Items
{
    class ImpactRounds : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Ballistic Rounds";

            string resourceName = "Items/Resources/ItemSprites/Passives/impact_rounds.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<ImpactRounds>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Osmium Core";
            string longDesc = "Projectiles break the boss damage cap\n\nBuilt from one of the densest" +
                " metals, these bullets don't have armor-piercing shells, but instead are meant to deal crushing damage to objects with a bigger surface area.";
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
