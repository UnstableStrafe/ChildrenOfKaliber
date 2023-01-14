using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using UnityEngine;
using ItemAPI;

namespace Items
{
    class PsiFocusOld : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Psi Focus";

            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<PsiFocusOld>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Close Your Eyes To See The World";
            string longDesc = "If the Psi Orbs are set to orbit the player, you have a chance to reflect attacks that would otherwise hurt you. If the Psi Orbs are set to orbit an enemy, that enemy will recieve take more damage.\n\nThe Blind can focus their psi into an offensive or defensive form to debilitate their foes.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;
            item.CanBeDropped = false;
            item.CanBeSold = false;

        }
        protected override void Update()
        {
            base.Update();
           
        }
        
        private void DoPlayerFocus(AIActor actor)
        {
            if (actor != null || actor.healthHaver.IsAlive)
            {
                actor.healthHaver.AllDamageMultiplier -= .25f;
                Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(actor.sprite);
                outlineMaterial.SetColor("_OverrideColor", new Color(0, 0, 0));
            }
            if (Owner != null)
            {
                Owner.specRigidbody.OnPreRigidbodyCollision += ReflectProjectile;
            }
        }
        private void DoEnemyFocus(AIActor actor)
        {
            Owner.specRigidbody.OnPreRigidbodyCollision -= ReflectProjectile;
            if(actor != null || actor.healthHaver.IsAlive)
            {
                actor.healthHaver.AllDamageMultiplier += .25f;
                Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(actor.sprite);
                Color32 specialPink = new Color32(254, 126, 229, 50);
                outlineMaterial.SetColor("_OverrideColor", specialPink);
            }
            else
            {
                DoPlayerFocus(actor);
            }
        }
        private void ReflectProjectile(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            Projectile component = otherRigidbody.GetComponent<Projectile>();
            if (component != null && !(component.Owner is PlayerController))
            {
                if(UnityEngine.Random.value < .16f)
                {
                    PassiveReflectItem.ReflectBullet(component, true, Owner.specRigidbody.gameActor, 40f, 1f, .5f, 0f);
                    PhysicsEngine.SkipCollision = true;
                    Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(component.sprite);
                    Color specialPink = new Color(254, 126, 229, 50f);
                    outlineMaterial.SetColor("_OverrideColor", specialPink);
                }
            }
        }
        public override void Pickup(PlayerController player)
        {

            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<PsiFocusOld>().m_pickedUpThisRun = true;

            return debrisObject;
        }
        private AIActor currentTarget;
        private SpecialOrbital.OrbitingMode lastOrbitingMode = SpecialOrbital.OrbitingMode.DEBUG_NONE;
    }
}
