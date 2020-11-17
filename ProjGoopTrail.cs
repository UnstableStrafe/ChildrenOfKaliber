using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using UnityEngine;
using ItemAPI;
using System.Runtime.CompilerServices;
using Dungeonator;
namespace Items
{
    class ProjGoopTrail : MonoBehaviour
    {
        public ProjGoopTrail()
        {
            goop = null;
            dur = .35f;
            rad = 1;
            suppressSplash = false;
        }
        private void Awake()
        {
            this.projectile = base.GetComponent<Projectile>();
            this.speculativeRigidBoy = base.GetComponent<SpeculativeRigidbody>();
        }
        private void Update()
        {
            bool flag = this.projectile == null;
            if (flag)
            {
                this.projectile = base.GetComponent<Projectile>();
                
            }
            bool flag2 = this.speculativeRigidBoy == null;
            if (flag2)
            {
                this.speculativeRigidBoy = base.GetComponent<SpeculativeRigidbody>();
            }
            this.elapsed = BraveTime.DeltaTime;
            bool flag3 = this.elapsed > .04f;
            if (flag3)
            {
                this.elapsed = 0;
            }
        }
        private Projectile projectile;
        private GoopDefinition goop;
        private SpeculativeRigidbody speculativeRigidBoy;
        private float elapsed;
        private float dur;
        private float rad;
        private bool suppressSplash;
    }
}
