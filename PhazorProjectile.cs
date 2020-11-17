using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Items
{
    internal class PhazorProjectile : MonoBehaviour
    {
        public void Start()
        {
            this.projectile = base.GetComponent<Projectile>();
            this.player = (this.projectile.Owner as PlayerController);
            Projectile proj = this.projectile;
            this.projectile.sprite.spriteId = this.projectile.sprite.GetSpriteIdByName("phazor_projectile_002");
            

        }


        private Projectile projectile;

        private PlayerController player;
    }
}

