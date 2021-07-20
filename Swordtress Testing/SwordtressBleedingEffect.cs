using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using UnityEngine;
using ItemAPI;
using Dungeonator;

namespace Items
{
    class SwordtressBleedingEffect : GameActorHealthEffect
    {
        private GoopDefinition SwordtressBloodGoop = new GoopDefinition()
        {
            CanBeIgnited = false,
            damagesEnemies = false,
            damagesPlayers = false,
            baseColor32 = new Color32(207, 23, 10, 200),
            goopTexture = ResourceExtractor.GetTextureFromResource("Items/Resources/goop_standard_base_001.png"),
            AppliesDamageOverTime = false,
            
        };
        public override void EffectTick(GameActor actor, RuntimeGameActorEffectData effectData)
        {
            base.EffectTick(actor, effectData);
            var ddgm = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(SwordtressBloodGoop);
            ddgm.TimedAddGoopCircle(actor.sprite.WorldCenter, .45f, .05f);
        }
    }
}
