using ItemAPI;
using UnityEngine;

namespace Items
{
    class PermafrostRounds : PassiveItem
    {
        public static void Init()
        {
             string itemName = "Permafrost Rounds";

             string resourceName = "Items/Resources/permafrost_rounds.png";

             GameObject obj = new GameObject(itemName);

             var item = obj.AddComponent<PermafrostRounds>();

             ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

             string shortDesc = "Aeons Frozen In A Single Bullet";
             string longDesc = "Shots freeze enemies. Hitting a frozen enemy shatters them, killing them instantly.\n\nThese bullets contain metal from the first shells ever fired inside the Gungeon.";

             ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

             item.quality = ItemQuality.S;
             item.sprite.IsPerpendicular = true;


        }
        private void Projectile(Projectile proj, float eff)
        {
            if(UnityEngine.Random.value < (PickupObjectDatabase.GetById(223) as BulletStatusEffectItem).chanceOfActivating)
            {
                proj.AppliesFreeze = true;
                proj.freezeEffect = (PickupObjectDatabase.GetById(223) as Gun).DefaultModule.projectiles[0].freezeEffect;
                proj.FreezeApplyChance = (PickupObjectDatabase.GetById(223) as Gun).DefaultModule.projectiles[0].FreezeApplyChance;
            }
            proj.OnHitEnemy = Hit;
        }
        private void Hit(Projectile projectile, SpeculativeRigidbody enemy, bool dead)
        {
            bool flag = enemy.aiActor && enemy.aiActor.healthHaver && enemy.aiActor.IsFrozen && !enemy.aiActor.healthHaver.IsBoss;
            if (flag)
            {
                enemy.healthHaver.ApplyDamage(10000000, Vector2.zero, "Frozen Break", CoreDamageTypes.None, DamageCategory.Normal, false, null, true);
            }
        }
        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += Projectile;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<PermafrostRounds>().m_pickedUpThisRun = true;
            player.PostProcessProjectile -= Projectile;
            return debrisObject;
        }
         
    }
}
