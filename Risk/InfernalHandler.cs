using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using UnityEngine;
using Alexandria.ItemAPI;
using MonoMod;
using System.Collections;

namespace Items
{
    static class InfernalHandler
    {
        public static void Infernalize(this AIActor actor)
        {
            if(actor != null)
            {
                ApplyInfenalShader(actor);
                actor.gameObject.AddComponent<InfernalSparks>();
            }
        }
        private static void ApplyInfenalShader(AIActor actor)
        {


            actor.gameObject.AddComponent<InfernalSparks>();
            
            
        }
    }
    public class InfernalSparks : MonoBehaviour
    {
        private void Start()
        {
            actor = base.GetComponent<AIActor>();
            ApplyShader();
            if (!actor.healthHaver.IsBoss)
            {
                actor.LocalTimeScale = 2f;
            }
            else if (actor.healthHaver.IsBoss) 
            {
                actor.LocalTimeScale = 1.2f;
            }
            if (actor.gameObject.GetComponent<SpeculativeRigidbody>())
            {
                rigidbody = actor.gameObject.GetComponent<SpeculativeRigidbody>();
            }
           


        }
        private void ApplyShader()
        {
            if (!actor.gameObject.GetComponent<AlwaysHoloShader>())
            {
            
                if(actor.EnemyGuid == "45192ff6d6cb43ed8f1a874ab6bef316")
                {
                    Destroy(this);
                }
                else
                {
                    foreach(tk2dBaseSprite bSprite in actor.healthHaver.bodySprites)
                    {
                        bSprite.usesOverrideMaterial = true;
                        bSprite.renderer.material.shader = ChildrenOfKaliberModule.ModAssets.LoadAsset<Shader>("infernal_shader");
                    }
                    
                    shouldHaveShader = true;
                }
                if (actor.aiShooter && actor.aiShooter.CurrentGun)
                {
                    tk2dBaseSprite sprite = actor.aiShooter.CurrentGun.GetSprite();
                    sprite.usesOverrideMaterial = true;
                    sprite.renderer.material.shader = ChildrenOfKaliberModule.ModAssets.LoadAsset<Shader>("infernal_shader");
                }
            }
        }
        private void Update()
        {
            if(actor.sprite.renderer.material.shader != ChildrenOfKaliberModule.ModAssets.LoadAsset<Shader>("infernal_shader") && shouldHaveShader)
            {
                ApplyShader();
            }
            if (!actor.healthHaver.IsBoss)
            {
                actor.LocalTimeScale = 1.8f;
            }
            else if (actor.healthHaver.IsBoss)
            {
                actor.LocalTimeScale = 1.2f;
            }
            //HandleSparks(actor);
        }
        private void HandleSparks(AIActor actor)
        {
            if(rigidbody != null)
            {
                Vector3 vector = rigidbody.UnitBottomLeft.ToVector3ZisY(0f);
                Vector3 vector2 = rigidbody.UnitTopRight.ToVector3ZisY(0f);
                float num = (vector2.y - vector.y) * (vector2.x - vector.x);
                float num2 = 10 * num;
                int num3 = Mathf.CeilToInt(Mathf.Max(1f, num2 * BraveTime.DeltaTime));
                int num4 = num3;
                Vector3 minPosition = vector;
                Vector3 maxPosition = vector2;
                Vector3 up = Vector3.up;
                float angleVariance = 120f;
                float magnitudeVariance = 0.5f;
                float? startLifetime = new float?(UnityEngine.Random.Range(1f, 1.65f));
                GlobalSparksDoer.DoRandomParticleBurst(num4, minPosition, maxPosition, up, angleVariance, magnitudeVariance, null, startLifetime, null, GlobalSparksDoer.SparksType.STRAIGHT_UP_FIRE);
            }
        }
        private bool shouldHaveShader = false;
        private SpeculativeRigidbody rigidbody = null;
        private AIActor actor;
    }
}
