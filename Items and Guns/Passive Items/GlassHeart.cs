using System.Collections.Generic;
using Alexandria.ItemAPI;
using UnityEngine;
using Dungeonator;

namespace Items
{
    class GlassHeart : PassiveItem
    {
        public static void Init()
        {

            string itemName = "Crystal Heart";
            string resourceName = "Items/Resources/ItemSprites/Passives/crystal_heart.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<GlassHeart>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "But I'm Weak";
            string longDesc = "Grants 1 heart container. Taking damage kills all enemies in the room (Bosses instead take heavy damage.) The item will fully break after 3 hits.\n\nTrying to engineer himself a second heart, a mad scientist ended up with this instead.";
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 1, StatModifier.ModifyMethod.ADDITIVE);


            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            item.quality = PickupObject.ItemQuality.D;
            item.sprite.IsPerpendicular = true;
        }
        private float Damaged;
        private void GlassDamage(PlayerController playerController)
        {
            this.EnemyListing();
            AkSoundEngine.PostEvent("Play_OBJ_crystal_shatter_01", base.gameObject);
            Damaged += 1;
            if(Damaged == 3)
            {
                playerController.DropPassiveItem(this);
            }
        }

        public override void Pickup(PlayerController player)
        {
            if (this.m_pickedUp)
            {
                return;
            }
            base.Pickup(player);
            player.OnReceivedDamage += this.GlassDamage;
        }
        public void Break()
        {
            this.m_pickedUp = true;
            UnityEngine.Object.Destroy(base.gameObject, 1f);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject drop = base.Drop(player);
            GlassHeart thingy = drop.GetComponent<GlassHeart>();
            player.OnReceivedDamage -= this.GlassDamage;
            thingy.m_pickedUpThisRun = true;
            if (Damaged == 3)
            {
                
                thingy.Break();
            }

            return drop;

        }
        private void EnemyListing()
        {
            RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
            List<AIActor> activeEnemies = absoluteRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            bool flag = activeEnemies != null;
            if (flag)
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    this.AffectEnemy(activeEnemies[i]);
                }
            }
        }
        private void AffectEnemy(AIActor target)
        {
            bool flag = target.IsNormalEnemy && !target.healthHaver.IsBoss && !target.IsHarmlessEnemy && !target.healthHaver.IsSubboss;
            bool flag2 = target.healthHaver.IsSubboss || target.healthHaver.IsBoss;
            if (flag)
            {
                target.healthHaver.ApplyDamage(9999f, Vector2.zero, "7 Years Bad Luck", CoreDamageTypes.None, DamageCategory.Normal, false, null, true);
            }
            if (flag2)
            {
               target.healthHaver.ApplyDamage(150f, Vector2.zero, "7 Years Bad Luck", CoreDamageTypes.None, DamageCategory.Normal, false, null, true);
            }

        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

    }
}
