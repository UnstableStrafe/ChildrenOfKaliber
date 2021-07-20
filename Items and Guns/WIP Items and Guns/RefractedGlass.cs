using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;
using Gungeon;

namespace Items
{
    class RefractedGlass : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Refracted Glass";

            string resourceName = "Items/Resources/refracted_glass.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<RefractedGlass>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Glimpses Into Another World";
            string longDesc = "Adds a 50% chance to spawn another projectile upon firing. This effect can happen multiple times per shot.\n\nThis shard of glass reflects glimmers of the 4th gunmension, where all bullets exist and don't exist at the same time.\nIt is said the Phaser Spiders and Killithids originated from there.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;


        }
        public bool isActive;
        private void PostProcess(Projectile bullet, float th) { RecalculateVolley(); }
        private void RecalculateVolley()
        {
            bool shouldBeActive = true;
            if ((shouldBeActive && isActive) || (!shouldBeActive && !isActive)) return;
            if (shouldBeActive && !isActive)
            {
                Owner.stats.AdditionalVolleyModifiers += this.ModifyVolley;
                Owner.stats.RecalculateStats(Owner, false, false);
                isActive = true;
            }
            else if (!shouldBeActive && isActive)
            {
                Owner.stats.AdditionalVolleyModifiers -= this.ModifyVolley;
                Owner.stats.RecalculateStats(Owner, false, false);
                isActive = false;
            }
        }
        public void ModifyVolley(ProjectileVolleyData volleyToModify)
        {
            //Make any changes to the volley you want in here. 
            int capper = 0;
            List<ProjectileModule> newModules = new List<ProjectileModule>{ };
            int count = volleyToModify.projectiles.Count;
            do
            {
                ProjectileModule projectileModule = volleyToModify.projectiles[0];
                ProjectileModule projectileModule2 = ProjectileModule.CreateClone(projectileModule, false);
                projectileModule2.angleFromAim = projectileModule.angleFromAim;
                //Modify your new module here, adjusting stats like angle from aim
                volleyToModify.projectiles.Add(projectileModule2);
            }
            while (UnityEngine.Random.value <= .5f && capper < 25);
            
        }
        private void PostProcessBeam(BeamController beam) { RecalculateVolley(); }
        public override void Pickup(PlayerController player)
        {
            player.PostProcessBeam += this.PostProcessBeam;
            player.PostProcessProjectile += this.PostProcess;
            
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessBeam -= this.PostProcessBeam;
            player.PostProcessProjectile -= this.PostProcess;
            Owner.stats.RecalculateStats(Owner, false, false);
            return base.Drop(player);
        }
        protected override void OnDestroy()
        {
            if (Owner)
            {
                Owner.PostProcessBeam -= this.PostProcessBeam;
                Owner.PostProcessProjectile -= this.PostProcess;
                Owner.stats.RecalculateStats(Owner, false, false);
            }
            
            base.OnDestroy();
        }
    }
    public class PreventDupesFromGlassBehav : MonoBehaviour
    {

    }
}
