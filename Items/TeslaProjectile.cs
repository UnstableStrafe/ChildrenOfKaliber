using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Items
{
    internal class TeslaProjectile : MonoBehaviour
    {
        public void Start()
        {
            this.projectile = base.GetComponent<Projectile>();
            this.player = (this.projectile.Owner as PlayerController);
            Projectile proj = this.projectile;
            //This determines what sprite you want your projectile to use.
            projectile.sprite.GetSpriteIdByName("tesla_projectile_001");

        }


        private Projectile projectile;

        private PlayerController player;
    }
}
