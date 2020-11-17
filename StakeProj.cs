using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Items
{
    internal class StakeProj : MonoBehaviour
    {
        public void Start()
        {
            this.projectile = base.GetComponent<Projectile>();
            this.player = (this.projectile.Owner as PlayerController);
            Projectile proj = this.projectile;
            this.projectile.sprite.spriteId = this.projectile.sprite.GetSpriteIdByName("stake_projectile_001");


        }


        private Projectile projectile;

        private PlayerController player;
    }
}
