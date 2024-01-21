using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;



namespace Items
{
    class SympathyBullets : PassiveItem
    {
        public static int itemID;
        public static void Init()
        {
            string itemName = "Sympathy Bullets";

            string resourceName = "Items/Resources/ItemSprites/Passives/sympathy_rounds.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<SympathyBullets>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "The Name Of Gunpowder";
            string longDesc = "Hitting an enemy deals extra damage equal to half of the original damage, evenly split to all other enemies of the same type in the room. If there is only one other enemy of the same type in the room, the extra damage on them is 25% instead of 50%." +
                "\n\nEach of these bullets has intricate sygaldry which creates sympathetic links between any targets identical to the original target, causing pain to be shared between them. It is unknown where the energy to cause the extra damage comes from.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ck");

            item.quality = ItemQuality.S;
            item.sprite.IsPerpendicular = true;
            itemID = item.PickupObjectId;
        }
        private void OnEnemyDamaged(PlayerController player, float damage, bool fatal, HealthHaver enemy)
        {
            AIActor baseActor = enemy.aiActor;
            List<AIActor> actorsInRoom = player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            List<AIActor> targets = new List<AIActor> { };
            foreach(AIActor otherActor in actorsInRoom)
            {
                if(otherActor != baseActor)
                {
                    AIActor check = CheckEnemyType(baseActor, otherActor);
                    if (check != null)
                    {
                        targets.Add(check);
                    }
                }
            }
            if (targets.Any())
            {
                HandleDamaging(targets, player, damage);
            }
        }
        
        private AIActor CheckEnemyType(AIActor origActor, AIActor potentialActor)
        {
            if(origActor != null && potentialActor != null)
            {
                if(origActor.EnemyGuid == potentialActor.EnemyGuid )
                {
                    return potentialActor;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        private void HandleDamaging(List<AIActor> targets, PlayerController source, float origDamage)
        {
            int count = targets.Count();
            float newDamage = 0;
            float minMultiplier = .25f;
            float multiplier = .5f;
            string damageSource = "Sympathy";
            if (Owner.PlayerHasActiveSynergy("Bloodless"))
            {
                
                minMultiplier = .33f;
                multiplier = .65f;
            }
            if (count == 1)
            {
                newDamage = origDamage * minMultiplier;
            }
            else
            {
                newDamage = (origDamage * multiplier) / count;
            }
            foreach(AIActor actor in targets)
            {
                HealthHaver healthHaver = actor.healthHaver;
               
                healthHaver.ApplyDamage(newDamage, Vector2.zero, damageSource);
            }
        }
        public override void Pickup(PlayerController player)
        {
            player.OnDealtDamageContext += OnEnemyDamaged;
            player.PostProcessProjectile += KvotheKingKillerSyn;
            base.Pickup(player);
        }

        private void KvotheKingKillerSyn(Projectile proj, float eff)
        {
            if(Owner.PlayerHasActiveSynergy("Kvothe, Kingkiller"))
            {
                HomingModifier mod = proj.gameObject.GetOrAddComponent<HomingModifier>();
                mod.HomingRadius = 30f;
                mod.AngularVelocity = 300;
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnDealtDamageContext -= OnEnemyDamaged;
            debrisObject.GetComponent<SympathyBullets>().m_pickedUpThisRun = true;
            player.PostProcessProjectile -= KvotheKingKillerSyn;
            return debrisObject;
        }
    }
}
