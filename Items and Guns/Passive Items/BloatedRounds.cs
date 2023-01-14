using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;

namespace Items
{
    class BloatedRounds : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Bloated Rounds";

            string resourceName = "Items/Resources/ItemSprites/Passives/bloated_rounds.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<BloatedRounds>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Like Birdshot, But Not";
            string longDesc = "Enemies have a chance to explode into friendly bullets on death.\n\nAn extremely deadly and painful disease that causes deformations in a gundead's body. There is no known cure for it.\nBet you don't feel so good about using it now, hmm?";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.C;
            item.sprite.IsPerpendicular = true;
        }


        private float chance = .3f;
        private void PostProcess(Projectile proj, float eff)
        {
            if (!proj.gameObject.GetComponent<PreventBloatDuping>())
            {

                proj.healthEffect = new BloatEffect() { DamagePerSecondToEnemies = 0, duration = 99999, OverheadVFX = SpriteBuilder.SpriteFromResource("Items/Resources/VFX/bloated_rounds_vfx"), PlaysVFXOnActor = true };
                proj.AppliesPoison = true;
                proj.PoisonApplyChance = chance * eff;
            }
        }
        

        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += PostProcess;
            cached_owner = player;
            base.Pickup(player);

        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<BloatedRounds>().m_pickedUpThisRun = true;
            player.PostProcessProjectile -= PostProcess;
            cached_owner = null;
            return debrisObject;
        }
        public override void OnDestroy()
        {
            cached_owner.PostProcessProjectile -= PostProcess;
            cached_owner = null;
            base.OnDestroy();
        }
        private PlayerController cached_owner;
    }
    public class PreventBloatDuping : MonoBehaviour
    {
        private float FUCK;
    }
    public class BloatEffect : GameActorHealthEffect
    {
        public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1)
        {
            base.OnEffectApplied(actor, effectData, partialAmount);
            effectData.OnActorPreDeath = delegate (Vector2 dir)
            {
                
                int amt = UnityEngine.Random.Range(5, 15);
                for(int i = 0; i < amt; i++)
                {
                    float angle = UnityEngine.Random.Range(0, 360);
                    GameObject obj = SpawnManager.SpawnProjectile(((Gun)ETGMod.Databases.Items[15]).DefaultModule.projectiles[0].gameObject, actor.CenterPosition, Quaternion.Euler(0, 0, angle));
                    Projectile proj = obj.GetComponent<Projectile>();
                    if (proj != null)
                    {
                        proj.Owner = GameManager.Instance.PrimaryPlayer;
                        proj.gameObject.AddComponent<PreventBloatDuping>();
                        proj.gameObject.AddComponent<PierceDeadActors>();
                    }
                }
                
            };
            actor.healthHaver.OnPreDeath += effectData.OnActorPreDeath;

        }
        public override void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
        {
            base.OnEffectRemoved(actor, effectData);
            actor.healthHaver.OnPreDeath -= effectData.OnActorPreDeath;

        }
        
    }
    public class PierceDeadActors : MonoBehaviour
    {
        public PierceDeadActors()
        {
        }
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            this.m_projectile.specRigidbody.OnPreRigidbodyCollision += this.PreCollision;
        }
        private void PreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            if (myRigidbody != null && otherRigidbody != null)
            {
                if (otherRigidbody.healthHaver != null && otherRigidbody.healthHaver.IsDead)
                {
                    PhysicsEngine.SkipCollision = true;
                }
            }
        }
        private Projectile m_projectile;
    }
}
