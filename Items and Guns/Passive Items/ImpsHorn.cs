using ItemAPI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Items
{
    class ImpsHorn : BulletStatusEffectItem
    {
        public static void Init()
        {


            string itemName = "Imp's Horn";
            string resourceName = "Items/Resources/imps_horn.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<ImpsHorn>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Don't Die Above Lava!";
            string longDesc = "Shots inflict green fire.\n\n" +
                "From the corpse of one of Bullet Hell's denizens, this horn still retains some of its magical energy.";


            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");


            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1);
            
            item.AppliesFire = true;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);





            var effect = new GameActorFireEffect()
            {
                IsGreenFire = true,
                AffectsEnemies = true,
                DamagePerSecondToEnemies = 10f,
                

            };

           

            item.chanceOfActivating = .08f;
            item.chanceFromBeamPerSecond = .08f;
            item.TintPriority = 5;
            item.FreezeAmountPerDamage = 1f;
            item.TintBeams = true;


            item.FireModifierEffect = effect;

            item.quality = PickupObject.ItemQuality.B;
            item.sprite.IsPerpendicular = true;
        }

        public override void Pickup(PlayerController player)
        {
            if (this.m_pickedUp)
            {
                return;
            }
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeam += this.PostProcessBeam;
            player.PostProcessBeamTick += this.PostProcessBeamTick;

        }


        public void PostProcessBeam(BeamController beam)
        {
            if (this.TintBeams)
            {
                beam.AdjustPlayerBeamTint(Library.LightGreen, 5);
            }
        }

        void PostProcessProjectile(Projectile projectile, float effectChanceScalar)
        {
            if (Random.value < (chanceOfActivating * effectChanceScalar))
            {
			    if (this.AppliesFire)
			    {
				    projectile.statusEffectsToApply.Add(this.FireModifierEffect);
                    projectile.HasDefaultTint = true;
                    projectile.DefaultTintColor = Library.LightGreen;
                }
            }
        }

        public void PostProcessBeamTick(BeamController beam, SpeculativeRigidbody hitRigidbody, float tickRate)
        {
            GameActor gameActor = hitRigidbody.gameActor;
            if (!gameActor)
            {
                return;
            }
            if (UnityEngine.Random.value < BraveMathCollege.SliceProbability(this.chanceFromBeamPerSecond, tickRate))
            {
                
                if (this.AppliesFire)
                {
                    gameActor.ApplyEffect(this.FireModifierEffect, 1f, null);
                    
                }
                
            }
        }


        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<ImpsHorn>().m_pickedUpThisRun = true;
            player.PostProcessProjectile -= this.PostProcessProjectile;
            player.PostProcessBeam -= this.PostProcessBeam;
            player.PostProcessBeamTick -= this.PostProcessBeamTick;
            return debrisObject;
        }
    }
}


