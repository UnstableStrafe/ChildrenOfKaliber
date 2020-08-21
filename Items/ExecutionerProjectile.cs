using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Items
{

    internal class ExecutionerProjectile : MonoBehaviour
    {

        public void Start()
        {
            this.projectile = base.GetComponent<Projectile>();
            this.player = (this.projectile.Owner as PlayerController);
            Projectile proj = this.projectile;
            //This determines what sprite you want your projectile to use.
            projectile.sprite.GetSpriteIdByName("executioner_projectile_001");

            Material material = projectile.sprite.renderer.material;
            this.projectile.sprite.renderer.material = material;
            material.shader = ShaderCache.Acquire("Brave/Internal/Glitch");
            material.SetFloat("_GlitchInterval", 0.08f);
            material.SetFloat("_DispProbability", 0.3f);
            material.SetFloat("_DispIntensity", 0.014f);
            material.SetFloat("_ColorProbability", 0.45f);
            material.SetFloat("_ColorIntensity", 0.033f);
        }


        private Projectile projectile;

        private PlayerController player;
    }
}
