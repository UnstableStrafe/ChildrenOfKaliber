using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Items
{
    internal class DispenserProjectile : MonoBehaviour
    {
        public void Start()
        {
            this._proj = base.GetComponent<Projectile>();
            this._player = (this._proj.Owner as PlayerController);
            Projectile proj = this._proj;
            this._proj.sprite.spriteId = this._proj.sprite.GetSpriteIdByName("dispenser_projectile_001");
        }

        private Projectile _proj;

        private PlayerController _player;

    }
}