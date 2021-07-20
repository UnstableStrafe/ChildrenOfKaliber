using ItemAPI;
using UnityEngine;

namespace Items
{
    class DaggerSpray : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Ritual Dagger";

            string resourceName = "Items/Resources/dagger.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<DaggerSpray>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Fatality!";
            string longDesc = "Killing an enemy releases a swarm of homing daggers. \n\nWhen given blood, this dagger vibrates rapidly.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
            item.quality = ItemQuality.A;
            item.sprite.IsPerpendicular = true;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
        }
        protected override void Update()
        {
            base.Update();
            this.CheckDamage();
        }
        private float DaggerBase = 3.5f, DaggerTrue;
        private float Damage, lastDamage = -1;
        private void CheckDamage()
        {
            Damage = Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
            if (Damage == lastDamage) return;
            DaggerTrue = DaggerBase * Damage;
            lastDamage = Damage;

        }
        private void DaggerSprayKill(PlayerController player)
        {
            
            for(int i = 0; i < 4; i++)
            {
                Projectile projectile2 = ((Gun)ETGMod.Databases.Items[377]).DefaultModule.projectiles[0];
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, Owner.CurrentGun.transform.position, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : 0f + (i * 90)), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                if (component != null)
                {
                    component.Owner = base.Owner;
                    HomingModifier homingModifier = component.gameObject.AddComponent<HomingModifier>();
                    homingModifier.HomingRadius = 100f;
                    homingModifier.AngularVelocity = 500f;
                    component.Shooter = base.Owner.specRigidbody;
                    component.baseData.speed = 25f;
                    component.baseData.damage = DaggerTrue;

                }
            }
            
        }
        
        

        public override void Pickup(PlayerController player)
        {
            if (this.m_pickedUp)
            {
                return;
            }
            base.Pickup(player);
            player.OnKilledEnemy += this.DaggerSprayKill;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debris = base.Drop(player);
            debris.GetComponent<DaggerSpray>().m_pickedUpThisRun = true;
            player.OnKilledEnemy -= this.DaggerSprayKill;
            return debris;
        }
    }
}
