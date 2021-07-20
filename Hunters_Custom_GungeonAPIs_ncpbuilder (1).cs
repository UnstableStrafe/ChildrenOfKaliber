using System;
using System.Collections.Generic;
using System.Linq;
using ItemAPI;
using UnityEngine;

namespace HunterCustomGungeonAPI
{
	// Token: 0x02000026 RID: 38
	public static class NPCBuilder
	{
		// Token: 0x0600015A RID: 346 RVA: 0x0000D168 File Offset: 0x0000B368
		public static tk2dSpriteAnimationClip AddAnimation(this GameObject obj, string name, string spriteDirectory, int fps, NPCBuilder.AnimationType type, DirectionalAnimation.DirectionType directionType = DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType flipType = DirectionalAnimation.FlipType.None, tk2dSpriteAnimationClip.WrapMode warpmode = tk2dSpriteAnimationClip.WrapMode.Loop)
		{
			obj.AddComponent<tk2dSpriteAnimator>();
			AIAnimator aianimator = obj.GetComponent<AIAnimator>();
			bool flag = !aianimator;
			bool flag2 = flag;
			bool flag3 = flag2;
			if (flag3)
			{
				aianimator = NPCBuilder.CreateNewAIAnimator(obj);
			}
			DirectionalAnimation directionalAnimation = aianimator.GetDirectionalAnimation(name, directionType, type);
			bool flag4 = directionalAnimation == null;
			bool flag5 = flag4;
			bool flag6 = flag5;
			if (flag6)
			{
				directionalAnimation = new DirectionalAnimation
				{
					AnimNames = new string[0],
					Flipped = new DirectionalAnimation.FlipType[0],
					Type = directionType,
					Prefix = string.Empty
				};
			}
			directionalAnimation.AnimNames = directionalAnimation.AnimNames.Concat(new string[]
			{
				name
			}).ToArray<string>();
			directionalAnimation.Flipped = directionalAnimation.Flipped.Concat(new DirectionalAnimation.FlipType[]
			{
				flipType
			}).ToArray<DirectionalAnimation.FlipType>();
			aianimator.AssignDirectionalAnimation(name, directionalAnimation, type);
			return NPCBuilder.BuildAnimation(aianimator, name, spriteDirectory, fps, warpmode);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0000D254 File Offset: 0x0000B454
		private static AIAnimator CreateNewAIAnimator(GameObject obj)
		{
			AIAnimator aianimator = obj.AddComponent<AIAnimator>();
			aianimator.FlightAnimation = NPCBuilder.CreateNewDirectionalAnimation();
			aianimator.HitAnimation = NPCBuilder.CreateNewDirectionalAnimation();
			aianimator.IdleAnimation = NPCBuilder.CreateNewDirectionalAnimation();
			aianimator.TalkAnimation = NPCBuilder.CreateNewDirectionalAnimation();
			aianimator.MoveAnimation = NPCBuilder.CreateNewDirectionalAnimation();
			aianimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>();
			aianimator.IdleFidgetAnimations = new List<DirectionalAnimation>();
			aianimator.OtherVFX = new List<AIAnimator.NamedVFXPool>();
			return aianimator;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000D2C8 File Offset: 0x0000B4C8
		private static DirectionalAnimation CreateNewDirectionalAnimation()
		{
			return new DirectionalAnimation
			{
				AnimNames = new string[0],
				Flipped = new DirectionalAnimation.FlipType[0],
				Type = DirectionalAnimation.DirectionType.None
			};
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000D300 File Offset: 0x0000B500
		public static tk2dSpriteAnimationClip BuildAnimation(AIAnimator aiAnimator, string name, string spriteDirectory, int fps, tk2dSpriteAnimationClip.WrapMode warpmode = tk2dSpriteAnimationClip.WrapMode.Loop)
		{
			tk2dSpriteCollectionData tk2dSpriteCollectionData = aiAnimator.GetComponent<tk2dSpriteCollectionData>();
			bool flag = !tk2dSpriteCollectionData;
			bool flag2 = flag;
			bool flag3 = flag2;
			if (flag3)
			{
				tk2dSpriteCollectionData = SpriteBuilder.ConstructCollection(aiAnimator.gameObject, aiAnimator.name + "_collection");
			}
			string[] resourceNames = ResourceExtractor.GetResourceNames();
			List<int> list = new List<int>();
			for (int i = 0; i < resourceNames.Length; i++)
			{
				bool flag4 = resourceNames[i].StartsWith(spriteDirectory.Replace('/', '.'), StringComparison.OrdinalIgnoreCase);
				bool flag5 = flag4;
				bool flag6 = flag5;
				if (flag6)
				{
					list.Add(SpriteBuilder.AddSpriteToCollection(resourceNames[i], tk2dSpriteCollectionData));
				}
			}
			bool flag7 = list.Count == 0;
			bool flag8 = flag7;
			bool flag9 = flag8;
			if (flag9)
			{
				ETGModConsole.Log("No sprites found for animation " + name);
			}
			tk2dSpriteAnimationClip tk2dSpriteAnimationClip = SpriteBuilder.AddAnimation(aiAnimator.spriteAnimator, tk2dSpriteCollectionData, list, name, warpmode);
			tk2dSpriteAnimationClip.fps = (float)fps;
			return tk2dSpriteAnimationClip;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000D3F8 File Offset: 0x0000B5F8
		public static DirectionalAnimation GetDirectionalAnimation(this AIAnimator aiAnimator, string name, DirectionalAnimation.DirectionType directionType, NPCBuilder.AnimationType type)
		{
			DirectionalAnimation directionalAnimation = null;
			switch (type)
			{
				case NPCBuilder.AnimationType.Move:
					directionalAnimation = aiAnimator.MoveAnimation;
					break;
				case NPCBuilder.AnimationType.Idle:
					directionalAnimation = aiAnimator.IdleAnimation;
					break;
				case NPCBuilder.AnimationType.Flight:
					directionalAnimation = aiAnimator.FlightAnimation;
					break;
				case NPCBuilder.AnimationType.Hit:
					directionalAnimation = aiAnimator.HitAnimation;
					break;
				case NPCBuilder.AnimationType.Talk:
					directionalAnimation = aiAnimator.TalkAnimation;
					break;
			}
			bool flag = directionalAnimation != null;
			bool flag2 = flag;
			bool flag3 = flag2;
			DirectionalAnimation result;
			if (flag3)
			{
				result = directionalAnimation;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000D47C File Offset: 0x0000B67C
		public static void AssignDirectionalAnimation(this AIAnimator aiAnimator, string name, DirectionalAnimation animation, NPCBuilder.AnimationType type)
		{
			switch (type)
			{
				case NPCBuilder.AnimationType.Move:
					aiAnimator.MoveAnimation = animation;
					break;
				case NPCBuilder.AnimationType.Idle:
					aiAnimator.IdleAnimation = animation;
					break;
				case NPCBuilder.AnimationType.Fidget:
					aiAnimator.IdleFidgetAnimations.Add(animation);
					break;
				case NPCBuilder.AnimationType.Flight:
					aiAnimator.FlightAnimation = animation;
					break;
				case NPCBuilder.AnimationType.Hit:
					aiAnimator.HitAnimation = animation;
					break;
				case NPCBuilder.AnimationType.Talk:
					aiAnimator.TalkAnimation = animation;
					break;
				default:
					aiAnimator.OtherAnimations.Add(new AIAnimator.NamedDirectionalAnimation
					{
						anim = animation,
						name = name
					});
					break;
			}
		}

		// Token: 0x0200017D RID: 381
		public enum AnimationType
		{
			// Token: 0x040003E2 RID: 994
			Move,
			// Token: 0x040003E3 RID: 995
			Idle,
			// Token: 0x040003E4 RID: 996
			Fidget,
			// Token: 0x040003E5 RID: 997
			Flight,
			// Token: 0x040003E6 RID: 998
			Hit,
			// Token: 0x040003E7 RID: 999
			Talk,
			// Token: 0x040003E8 RID: 1000
			Other
		}
	}
}
