using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectionType = DirectionalAnimation.DirectionType;
using FlipType = DirectionalAnimation.FlipType;
using UnityEngine;
using ItemAPI;

namespace GungeonAPI
{
    public static class NPCBuilder
    {

        public enum AnimationType { Move, Idle, Fidget, Flight, Hit, Talk, Other }
        public static tk2dSpriteAnimationClip AddAnimation(this GameObject obj, string name, string spriteDirectory, int fps,
            AnimationType type, DirectionType directionType = DirectionType.None, FlipType flipType = FlipType.None)
        {
            obj.AddComponent<tk2dSpriteAnimator>();
            AIAnimator aiAnimator = obj.GetComponent<AIAnimator>();
            if (!aiAnimator)
                aiAnimator = CreateNewAIAnimator(obj);
            DirectionalAnimation animation = aiAnimator.GetDirectionalAnimation(name, directionType, type);
            if (animation == null)
            {
                animation = new DirectionalAnimation()
                {
                    AnimNames = new string[0],
                    Flipped = new FlipType[0],
                    Type = directionType,
                    Prefix = string.Empty
                };
            }

            animation.AnimNames = animation.AnimNames.Concat(new string[] { name }).ToArray();
            animation.Flipped = animation.Flipped.Concat(new FlipType[] { flipType }).ToArray();
            aiAnimator.AssignDirectionalAnimation(name, animation, type);
            return BuildAnimation(aiAnimator, name, spriteDirectory, fps);
        }

        private static AIAnimator CreateNewAIAnimator(GameObject obj)
        {
            var animator = obj.AddComponent<AIAnimator>();
            animator.FlightAnimation = CreateNewDirectionalAnimation();
            animator.HitAnimation = CreateNewDirectionalAnimation();
            animator.IdleAnimation = CreateNewDirectionalAnimation();
            animator.TalkAnimation = CreateNewDirectionalAnimation();
            animator.MoveAnimation = CreateNewDirectionalAnimation();
            animator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>();
            animator.IdleFidgetAnimations = new List<DirectionalAnimation>();
            animator.OtherVFX = new List<AIAnimator.NamedVFXPool>();
            return animator;
        }

        private static DirectionalAnimation CreateNewDirectionalAnimation()
        {
            return new DirectionalAnimation()
            {
                AnimNames = new string[0],
                Flipped = new FlipType[0],
                Type = DirectionType.None
            };
        }

        public static tk2dSpriteAnimationClip BuildAnimation(AIAnimator aiAnimator, string name, string spriteDirectory, int fps)
        {
            tk2dSpriteCollectionData collection = aiAnimator.GetComponent<tk2dSpriteCollectionData>();
            if (!collection)
                collection = SpriteBuilder.ConstructCollection(aiAnimator.gameObject, $"{aiAnimator.name}_collection");

            string[] resources = ResourceExtractor.GetResourceNames();
            List<int> indices = new List<int>();
            for (int i = 0; i < resources.Length; i++)
            {
                if (resources[i].StartsWith(spriteDirectory.Replace('/', '.'), StringComparison.OrdinalIgnoreCase))
                {
                    indices.Add(SpriteBuilder.AddSpriteToCollection(resources[i], collection));
                }
            }
            if (indices.Count == 0) { Tools.PrintError($"No sprites found for animation {name}"); }
            tk2dSpriteAnimationClip clip = SpriteBuilder.AddAnimation(aiAnimator.spriteAnimator, collection, indices, name, tk2dSpriteAnimationClip.WrapMode.Loop);
            clip.fps = fps;
            return clip;
        }

        public static DirectionalAnimation GetDirectionalAnimation(this AIAnimator aiAnimator, string name, DirectionType directionType, AnimationType type)
        {
            DirectionalAnimation result = null;
            switch (type)
            {
                case AnimationType.Idle:
                    result = aiAnimator.IdleAnimation;
                    break;
                case AnimationType.Move:
                    result = aiAnimator.MoveAnimation;
                    break;
                case AnimationType.Flight:
                    result = aiAnimator.FlightAnimation;
                    break;
                case AnimationType.Hit:
                    result = aiAnimator.HitAnimation;
                    break;
                case AnimationType.Talk:
                    result = aiAnimator.TalkAnimation;
                    break;
            }
            if (result != null)
                return result;

            return null;
        }

        public static void AssignDirectionalAnimation(this AIAnimator aiAnimator, string name, DirectionalAnimation animation, AnimationType type)
        {
            switch (type)
            {
                case AnimationType.Idle:
                    aiAnimator.IdleAnimation = animation;
                    break;
                case AnimationType.Move:
                    aiAnimator.MoveAnimation = animation;
                    break;
                case AnimationType.Flight:
                    aiAnimator.FlightAnimation = animation;
                    break;
                case AnimationType.Hit:
                    aiAnimator.HitAnimation = animation;
                    break;
                case AnimationType.Talk:
                    aiAnimator.TalkAnimation = animation;
                    break;
                case AnimationType.Fidget:
                    aiAnimator.IdleFidgetAnimations.Add(animation);
                    break;
                default:
                    aiAnimator.OtherAnimations.Add(new AIAnimator.NamedDirectionalAnimation()
                    {
                        anim = animation,
                        name = name
                    });
                    break;
            }
        }
    }
}
