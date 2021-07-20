using UnityEngine;
using ItemAPI;

namespace Items
{
    class GattlingBullets : ComplexProjectileModifier
    {
        public static void Init()
        {

            string itemName = "Gattling Bullets";
            string resourceName = "Items/Resources/gattling_bullets.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<SyncDiode>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Who Touched Sasha!?";
            string longDesc = "Increases rate of fire, magazine size, and maximum ammo capacity, but reduces shot size and drastically reduces damage.\n\nThese bullets are much smaller than normal bullets, and thus you can fit more " +
                "in your gun; it's only logical!";

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, .4f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.RateOfFire, 3.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.PlayerBulletScale, .7f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalClipCapacityMultiplier, 4.0f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AmmoCapacityMultiplier, 1.50f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            item.quality = PickupObject.ItemQuality.A;



        }
        private void PostProcessProjectile(Projectile obj, float effectChanceScalar)
        {
            base.Owner.SpawnShadowBullet(obj, true);
        }

        public override void Pickup(PlayerController player)
        {
            if (this.m_pickedUp)
            {
                return;
            }
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<GattlingBullets>().m_pickedUpThisRun = true;
            player.PostProcessProjectile -= this.PostProcessProjectile;

            return debrisObject;
        }
    }
}
