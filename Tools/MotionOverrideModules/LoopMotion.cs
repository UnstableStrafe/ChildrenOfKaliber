using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using UnityEngine;
using ItemAPI;

namespace Items
{
    class LoopMotion : ProjectileAndBeamMotionModule
    {

        public override void Move(Projectile source, Transform projectileTransform, tk2dBaseSprite projectileSprite, SpeculativeRigidbody specRigidbody, ref float m_timeElapsed, ref Vector2 m_currentDirection, bool Inverted, bool shouldRotate)
        {
            
        }

        public override void UpdateDataOnBounce(float angleDiff)
        {
         
        }
        public override Vector2 GetBoneOffset(BasicBeamController.BeamBone bone, BeamController sourceBeam, bool inverted)
        {
            Vector2 empty = Vector2.zero;
            return empty;
        }

    }
}
