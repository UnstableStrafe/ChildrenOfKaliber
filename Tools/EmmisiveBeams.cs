using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;
using System.Reflection;
using Random = System.Random;
using FullSerializer;
using System.Collections;
using Gungeon;
using MonoMod.RuntimeDetour;
using MonoMod;
using System.Collections.ObjectModel;

using UnityEngine.Serialization;

namespace Planetside
{
	internal class EmmisiveBeams : MonoBehaviour
	{
		public EmmisiveBeams()
		{
            this.EmissivePower = 100;
            this.EmissiveColorPower = 1.55f;
        }
        public void Start()
        {
            Transform trna = base.transform.Find("beam impact vfx");
            if (trna != null)
            {
                tk2dSprite sproot = trna.GetComponent<tk2dSprite>();
                if (sproot != null)
                {
                    sproot.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
                    sproot.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_ON");
                    sproot.renderer.material.SetFloat("_EmissivePower", EmissivePower);
                    sproot.renderer.material.SetFloat("_EmissiveColorPower", EmissiveColorPower);
                }
            }
            Transform trna1 = base.transform.Find("beam impact vfx 2");
            if (trna1 != null)
            {
                tk2dSprite sproot1 = trna.GetComponent<tk2dSprite>();
                if (sproot1 != null)
                {
                    sproot1.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
                    sproot1.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_ON");
                    sproot1.renderer.material.SetFloat("_EmissivePower", EmissivePower);
                    sproot1.renderer.material.SetFloat("_EmissiveColorPower", EmissiveColorPower);
                }
            }

            for (int i = 0; i < base.transform.childCount; i++)
            {
                Transform child = base.transform.Find("Sprite");
                if (child != null)
                {
                    tk2dSprite sproot2 = child.GetComponent<tk2dSprite>();
                    if (sproot2 != null)
                    {
                        sproot2.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
                        sproot2.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_ON");
                        sproot2.renderer.material.SetFloat("_EmissivePower", EmissivePower);
                        sproot2.renderer.material.SetFloat("_EmissiveColorPower", EmissiveColorPower);
                    }
                }
            }


            this.beamcont = base.GetComponent<BasicBeamController>();
            BasicBeamController beam = this.beamcont;
            beam.sprite.usesOverrideMaterial = true;
            BasicBeamController component = beam.gameObject.GetComponent<BasicBeamController>();
            bool flag = component != null;
            bool flag2 = flag;
            if (flag2)
            {
                component.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
                component.sprite.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_ON");
                component.sprite.renderer.material.SetFloat("_EmissivePower", EmissivePower);
                component.sprite.renderer.material.SetFloat("_EmissiveColorPower", EmissiveColorPower);
            }   
        }

        public void Update()
        {
            Transform trna = base.transform.Find("beam pierce impact vfx");
            if (trna != null)
            {
                tk2dSprite sproot = trna.GetComponent<tk2dSprite>();
                if (sproot != null)
                {
                    sproot.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
                    sproot.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_ON");
                    sproot.renderer.material.SetFloat("_EmissivePower", EmissivePower);
                    sproot.renderer.material.SetFloat("_EmissiveColorPower", EmissiveColorPower);
                }
            }
        }

        private BasicBeamController beamcont;
        public float EmissivePower;
        public float EmissiveColorPower;
    }
}

