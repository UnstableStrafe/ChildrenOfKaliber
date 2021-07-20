using System;
using ItemAPI;
using UnityEngine;
using Dungeonator;


namespace Items
{
    class FloopBullets : PassiveItem
    {
        public FloopBullets()
        {
            this.ChanceToSeekEnemyOnBounce = 0.70f;
        }
        public static void Init()
        {
            string itemName = "Floop Bullets";

            string resourceName = "Items/Resources/floop_bullets.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<FloopBullets>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Don't Eat It";
            string longDesc = "Decreases shot speed by 25%. Bullets have a high chance to ricochet to another enemy. Bullets gain speed with each ricochet, but lose damage." +
                "\n\nThese bullets are coated in a totally non-descript and non-copyright elastic goo which breaks all known laws of kenetics, gaining energy with each bounce, until the material wears itself out." +
                " This goo, as mentioned earlier, is in no way related to any substances in media whatsoever and have no connections to an Earth film from 1997. To think such is the case would be absurd.";

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ProjectileSpeed, .75f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.A;
            item.sprite.IsPerpendicular = true;
        }

        public void PostProjectile(Projectile projectile, float effectChanceScalar)
        {
            projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HitEnemy));   
        }
        private void HitEnemy(Projectile projectile, SpeculativeRigidbody enemy, bool killed)
        {
            projectile.Speed *= 1.2f;
            projectile.baseData.damage *= .8f;
            
            PierceProjModifier orAddComponent = projectile.gameObject.GetOrAddComponent<PierceProjModifier>();
            orAddComponent.penetratesBreakables = true;
            orAddComponent.penetration++;
            Vector2 dirVec = UnityEngine.Random.insideUnitCircle;
            float num = this.ChanceToSeekEnemyOnBounce;

            if (UnityEngine.Random.value < num && enemy.aiActor)
            {
                Func<AIActor, bool> isValid = (AIActor a) => a && a.HasBeenEngaged && a.healthHaver && a.healthHaver.IsVulnerable;
                AIActor closestToPosition = BraveUtility.GetClosestToPosition<AIActor>(enemy.aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All), enemy.UnitCenter, isValid, new AIActor[]
                {
                    enemy.aiActor
                });
                if (closestToPosition)
                {
                    dirVec = closestToPosition.CenterPosition - projectile.transform.position.XY();
                }
            }
            projectile.SendInDirection(dirVec, false, true);
        }
        private float ChanceToSeekEnemyOnBounce;
        public override void Pickup(PlayerController player)
        {
            if (this.m_pickedUp)
            {
                return;
            }
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProjectile;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.PostProcessProjectile -= this.PostProjectile;
            debrisObject.GetComponent<FloopBullets>().m_pickedUpThisRun = true;
            return debrisObject;
        }
    }
}
