using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using UnityEngine;
using Dungeonator;
namespace Items
{
    public class CharcoalDustEffect : GameActorEffect
    {
        public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1)
        {
            base.OnEffectApplied(actor, effectData, partialAmount);
            actor.healthHaver.AllDamageMultiplier += .3f;
            
        }
        public override void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
        {
            base.OnEffectRemoved(actor, effectData);
            actor.healthHaver.AllDamageMultiplier -= .3f;
        }
    }
}
