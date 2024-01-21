using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;
using System.Collections;
using System.Reflection;
using Dungeonator;
using FullInspector;
using Pathfinding;

namespace Items
{
    class EyesOfStone : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Eyes Of Stone";

            string resourceName = "Items/Resources/ItemSprites/Passives/eyes_of_stone.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<EyesOfStone>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Cold, Cruel";
            string longDesc = "All illusions are dispelled under the bearer's eyes. Increases accuracy.\n\nAn ancient technique that allows one to strip themselves of personal biases and see the world through the perspective of another.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ck");

            item.quality = ItemQuality.D;
            item.sprite.IsPerpendicular = true;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Accuracy, .75f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            //!!
            //Look up encryption cypher techiniques to make hidded secrets for your mod!!
            //!!
        }
        private void ClearIllusions(AIActor aiActor)
        {
            if(aiActor.EnemyGuid == "3e98ccecf7334ff2800188c417e67c15")
            {
                if(aiActor.gameObject.GetComponent<MirrorImageController>())
                {
                    aiActor.gameObject.AddComponent<AlwaysHoloShader>();
                }
            }
            
        }

        public override void Pickup(PlayerController player)
        {

            base.Pickup(player);
            
            ETGMod.AIActor.OnPostStart = (Action<AIActor>)Delegate.Combine(ETGMod.AIActor.OnPostStart, new Action<AIActor>(ClearIllusions));
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<EyesOfStone>().m_pickedUpThisRun = true;
            ETGMod.AIActor.OnPostStart = (Action<AIActor>)Delegate.Remove(ETGMod.AIActor.OnPostStart, new Action<AIActor>(ClearIllusions));
            return debrisObject;
        }
        public override void OnDestroy()
        {
            ETGMod.AIActor.OnPostStart = (Action<AIActor>)Delegate.Remove(ETGMod.AIActor.OnPostStart, new Action<AIActor>(ClearIllusions));
            base.OnDestroy();
        }

    }
    public class AlwaysHoloShader : MonoBehaviour
    {
        public AlwaysHoloShader()
        {
            holoShader = ShaderCache.Acquire("Brave/Internal/HologramShader");
        }
        private void Start() 
        {
            fakery = base.GetComponent<AIActor>();
            Shader baseShader = fakery.sprite.renderer.material.shader;
            if(baseShader != holoShader)
            {
                fakery.sprite.renderer.material.shader = holoShader;
                fakery.sprite.renderer.material.SetFloat("_IsGreen", 0);
            }
        }
        private void Update()
        {
            if (fakery.sprite.renderer.material.shader == holoShader) return;
            fakery.sprite.renderer.material.shader = holoShader;
            fakery.sprite.renderer.material.SetFloat("_IsGreen", 0);
        }
        
        private Shader holoShader;
        private AIActor fakery;
    }


}
