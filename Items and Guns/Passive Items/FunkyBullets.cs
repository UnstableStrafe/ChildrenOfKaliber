using UnityEngine;
using Alexandria.ItemAPI;


namespace Items
{
    public class FunkyBullets : PassiveItem
    {
        public static void Init()
        {
            
            string itemName = "Funky Bullets";
            string resourceName = "Items/Resources/ItemSprites/Passives/funky_bullets.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<FunkyBullets>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Who Shot Jonny?";
            string longDesc = "Makes bullet stats weird.\n\n" +
                "A horribly failed attempt at forging a bullet that could kill the past. The creator threw them into the Gungeon some time later, where they remained until your grubby little hands put them in a magazine.";   

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ck");

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ProjectileSpeed, .85f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Accuracy, .75f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, .80f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ReloadSpeed, .7f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.RateOfFire, 1.35f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalShotBounces, 1);

            item.quality = PickupObject.ItemQuality.C;
            item.sprite.IsPerpendicular = true;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
        }
        public override void Pickup(PlayerController player)
        {
            if (this.m_pickedUp)
            {
                return;
            }
            base.Pickup(player);
          // --- player.PostProcessProjectile += ApplyProjMotion;
        // ---   player.PostProcessBeam += ApplyBeamMotion;
        }

        private void ApplyBeamMotion(BeamController obj)
        {
           
        }

        private void ApplyProjMotion(Projectile projectile, float eff)
        {
            
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<FunkyBullets>().m_pickedUpThisRun = true;
           //--- player.PostProcessBeam -= ApplyBeamMotion;
           //--- player.PostProcessProjectile -= ApplyProjMotion;
            return debrisObject;
        }
    }
}