using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using UnityEngine;
using ItemAPI;

namespace Items
{
    class PsiFocus : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Psi Focus";

            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<PsiFocus>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Close Your Eyes To See The World";
            string longDesc = "If the Psi Gun is set to orbit the player, you have a chance to reflect attacks that would otherwise hurt you. If the Psi Gun is set to orbit an enemy, that enemy will recieve take more damage.\n\nThe Blind can focus their psi into an offensive or defensive form to debilitate their foes.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;
            item.CanBeDropped = false;
            item.CanBeSold = false;

        }
        protected override void Update()
        {
            base.Update();
            CheckForItem();
        }
        private void CheckForItem()
        {
            if(!Owner.HasPickupID(ETGMod.Databases.Items["Psi Marker"].PickupObjectId))
            {
                return;
            }
            PsiMarker marker = Owner.activeItems.GetFirst<PlayerItem>().gameObject.GetComponent<PsiMarker>();
            if (marker.psiGun.GetComponent<SpecialOrbital>().orbitingMode == lastOrbitingMode)
            {
                return;
            }
            lastOrbitingMode = marker.psiGun.GetComponent<SpecialOrbital>().orbitingMode;
            if (lastOrbitingMode == SpecialOrbital.OrbitingMode.PLAYER)
            {
                DoPlayerFocus(marker.psiGun.GetComponent<SpecialOrbital>().currentOrbitTarget);
            }
            else if (lastOrbitingMode == SpecialOrbital.OrbitingMode.ENEMY)
            {
                DoEnemyFocus(marker.psiGun.GetComponent<SpecialOrbital>().currentOrbitTarget);
            }
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
                Color specialPink = new Color(254, 126, 229, 50f);
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
            debrisObject.GetComponent<PsiFocus>().m_pickedUpThisRun = true;

            return debrisObject;
        }
        private AIActor currentTarget;
        private SpecialOrbital.OrbitingMode lastOrbitingMode = SpecialOrbital.OrbitingMode.DEBUG_NONE;
    }
}
