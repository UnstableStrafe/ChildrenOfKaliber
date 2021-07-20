using ItemAPI;
using UnityEngine;

namespace Items
{
    class EliteBullets : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Elite Bullets";

            string resourceName = "Items/Resources/elite_bullets.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<EliteBullets>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Made Of The Finest Lead";
            string longDesc = "Increases damage. Projectiles are automatically aimed at the nearest enemy.\n\nThe lead for these bullets was mined in the heart of a dying planet, the gunpowder" +
                "created from the ashes of a Dragun, and forged in the fire of Gunymede's star. No creature has more skill than them.";
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 1.10f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            item.quality = ItemQuality.S;
            item.sprite.IsPerpendicular = true;


        }
        private float dis;

        private void Aiming(Projectile projectile, float effectChanceScalar)
        {
            if (!Owner.CurrentGun.HasShootStyle(ProjectileModule.ShootStyle.Beam))
            {
                HomingModifier homingModifier = projectile.gameObject.AddComponent<HomingModifier>();
                homingModifier.HomingRadius = 100f;
                homingModifier.AngularVelocity = 4000f;
            }
        }
        public override void Pickup(PlayerController player)
        {

            base.Pickup(player);
            player.PostProcessProjectile += this.Aiming;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<EliteBullets>().m_pickedUpThisRun = true;
            player.PostProcessProjectile -= this.Aiming;

            return debrisObject;
        }
    }
}
