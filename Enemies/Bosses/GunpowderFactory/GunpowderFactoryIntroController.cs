using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using Dungeonator;

namespace Items
{
    [RequireComponent(typeof(GenericIntroDoer))]
    class GunpowderFactoryIntroController : SpecificIntroDoer
    {
		public void Start()
		{
			tk2dSpriteAnimator component = this.healthHaver.GetComponent<tk2dSpriteAnimator>();
			component.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(component.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
		}

		public void AnimationEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameIdx)
		{
			bool flag = clip.GetFrame(frameIdx).eventInfo == "deathOno" && base.healthHaver != null;
			if (flag)
			{
				//ETGModConsole.Log("FRame Tripped");
				//base.StartCoroutine(this.OnDeathExplosionsCR());
			}
		}
	}
}
