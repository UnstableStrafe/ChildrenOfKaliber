using UnityEngine;
namespace Items
{
    public class NitricAcidHealthEffect : GameActorHealthEffect
    {
        public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1)
        {
            base.OnEffectApplied(actor, effectData, partialAmount);
            effectData.OnActorPreDeath = delegate (Vector2 dir)
            {
                var ddgm = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(Library.NitricAcidGoop);
                ddgm.TimedAddGoopCircle(actor.CenterPosition, 3.5f, .35f);
            };
            actor.healthHaver.OnPreDeath += effectData.OnActorPreDeath;
            
        }
        public override void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
        {
            base.OnEffectRemoved(actor, effectData);
            actor.healthHaver.OnPreDeath -= effectData.OnActorPreDeath;
            
        }
    }
}
