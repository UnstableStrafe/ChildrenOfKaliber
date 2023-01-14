using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using UnityEngine;
using Alexandria.ItemAPI;
using System.Collections;

namespace Items
{
    static class BeamStuff
    {
        public static BasicBeamController GenerateBeamPrefab(this Projectile projectile, string spritePath, Vector2 colliderDimensions, Vector2 colliderOffsets, List<string> beamAnimationPaths = null, int beamFPS = -1, List<string> impactVFXAnimationPaths = null, int beamImpactFPS = -1, Vector2? impactVFXColliderDimensions = null, Vector2? impactVFXColliderOffsets = null, List<string> endVFXAnimationPaths = null, int beamEndFPS = -1, Vector2? endVFXColliderDimensions = null, Vector2? endVFXColliderOffsets = null, List<string> muzzleVFXAnimationPaths = null, int beamMuzzleFPS = -1, Vector2? muzzleVFXColliderDimensions = null, Vector2? muzzleVFXColliderOffsets = null)
        {
            try
            {
                float convertedColliderX = colliderDimensions.x / 16f;
                float convertedColliderY = colliderDimensions.y / 16f;
                float convertedOffsetX = colliderOffsets.x / 16f;
                float convertedOffsetY = colliderOffsets.y / 16f;

                int spriteID = SpriteBuilder.AddSpriteToCollection(spritePath, ETGMod.Databases.Items.ProjectileCollection);
                tk2dTiledSprite tiledSprite = projectile.gameObject.GetOrAddComponent<tk2dTiledSprite>();

               

                tiledSprite.SetSprite(ETGMod.Databases.Items.ProjectileCollection, spriteID);
                tk2dSpriteDefinition def = tiledSprite.GetCurrentSpriteDef();
                def.colliderVertices = new Vector3[]{
                    new Vector3(convertedOffsetX, convertedOffsetY, 0f),
                    new Vector3(convertedColliderX, convertedColliderY, 0f)
                };

                def.ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.MiddleLeft);

                //tiledSprite.anchor = tk2dBaseSprite.Anchor.MiddleCenter;
                tk2dSpriteAnimator animator = projectile.gameObject.GetOrAddComponent<tk2dSpriteAnimator>();
                tk2dSpriteAnimation animation = projectile.gameObject.GetOrAddComponent<tk2dSpriteAnimation>();
                animation.clips = new tk2dSpriteAnimationClip[0];
                animator.Library = animation;
                UnityEngine.Object.Destroy(projectile.GetComponentInChildren<tk2dSprite>());
                projectile.sprite = tiledSprite;
                BasicBeamController beamController = projectile.gameObject.GetOrAddComponent<BasicBeamController>();

                //---------------- Sets up the animation for the main part of the beam
                if (beamAnimationPaths != null)
                {
                    tk2dSpriteAnimationClip clip = new tk2dSpriteAnimationClip() { name = "beam_idle", frames = new tk2dSpriteAnimationFrame[0], fps = beamFPS };
                    List<string> spritePaths = beamAnimationPaths;

                    List<tk2dSpriteAnimationFrame> frames = new List<tk2dSpriteAnimationFrame>();
                    foreach (string path in spritePaths)
                    {
                        tk2dSpriteCollectionData collection = ETGMod.Databases.Items.ProjectileCollection;
                        int frameSpriteId = SpriteBuilder.AddSpriteToCollection(path, collection);
                        tk2dSpriteDefinition frameDef = collection.spriteDefinitions[frameSpriteId];
                        frameDef.ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.MiddleLeft);
                        frameDef.colliderVertices = def.colliderVertices;
                        frames.Add(new tk2dSpriteAnimationFrame { spriteId = frameSpriteId, spriteCollection = collection });
                    }
                    clip.frames = frames.ToArray();
                    animation.clips = animation.clips.Concat(new tk2dSpriteAnimationClip[] { clip }).ToArray();
                    beamController.beamAnimation = "beam_idle";
                }

                //------------- Sets up the animation for the part of the beam that touches the wall
                if (endVFXAnimationPaths != null && endVFXColliderDimensions != null && endVFXColliderOffsets != null)
                {
                    SetupBeamPart(animation, endVFXAnimationPaths, "beam_end", beamEndFPS, (Vector2)endVFXColliderDimensions, (Vector2)endVFXColliderOffsets);
                    beamController.beamEndAnimation = "beam_end";
                }
                else
                {
                    SetupBeamPart(animation, beamAnimationPaths, "beam_end", beamFPS, null, null, def.colliderVertices);
                    beamController.beamEndAnimation = "beam_end";
                }

                //---------------Sets up the animaton for the VFX that plays over top of the end of the beam where it hits stuff
                if (impactVFXAnimationPaths != null && impactVFXColliderDimensions != null && impactVFXColliderOffsets != null)
                {
                    SetupBeamPart(animation, impactVFXAnimationPaths, "beam_impact", beamImpactFPS, (Vector2)impactVFXColliderDimensions, (Vector2)impactVFXColliderOffsets);
                    beamController.impactAnimation = "beam_impact";
                }

                //--------------Sets up the animation for the very start of the beam
                if (muzzleVFXAnimationPaths != null && muzzleVFXColliderDimensions != null && muzzleVFXColliderOffsets != null)
                {
                    SetupBeamPart(animation, muzzleVFXAnimationPaths, "beam_start", beamMuzzleFPS, (Vector2)muzzleVFXColliderDimensions, (Vector2)muzzleVFXColliderOffsets);
                    beamController.beamStartAnimation = "beam_start";
                }
                else
                {
                    SetupBeamPart(animation, beamAnimationPaths, "beam_start", beamFPS, null, null, def.colliderVertices);
                    beamController.beamStartAnimation = "beam_start";
                }

                return beamController;
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.ToString());
                return null;
            }
        }
        private static void SetupBeamPart(tk2dSpriteAnimation beamAnimation, List<string> animSpritePaths, string animationName, int fps, Vector2? colliderDimensions = null, Vector2? colliderOffsets = null, Vector3[] overrideVertices = null)
        {
            tk2dSpriteAnimationClip clip = new tk2dSpriteAnimationClip() { name = animationName, frames = new tk2dSpriteAnimationFrame[0], fps = fps };
            List<string> spritePaths = animSpritePaths;

            List<tk2dSpriteAnimationFrame> frames = new List<tk2dSpriteAnimationFrame>();
            foreach (string path in spritePaths)
            {
                tk2dSpriteCollectionData collection = ETGMod.Databases.Items.ProjectileCollection;
                int frameSpriteId = SpriteBuilder.AddSpriteToCollection(path, collection);
                tk2dSpriteDefinition frameDef = collection.spriteDefinitions[frameSpriteId];
                frameDef.ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.MiddleCenter);
                if (overrideVertices != null)
                {
                    frameDef.colliderVertices = overrideVertices;
                }
                else
                {
                    if (colliderDimensions == null || colliderOffsets == null)
                    {
                        ETGModConsole.Log("<size=100><color=#ff0000ff>BEAM ERROR: colliderDimensions or colliderOffsets was null with no override vertices!</color></size>", false);
                    }
                    else
                    {
                        Vector2 actualDimensions = (Vector2)colliderDimensions;
                        Vector2 actualOffsets = (Vector2)colliderDimensions;
                        frameDef.colliderVertices = new Vector3[]{
                            new Vector3(actualOffsets.x / 16, actualOffsets.y / 16, 0f),
                            new Vector3(actualDimensions.x / 16, actualDimensions.y / 16, 0f)
                        };
                    }
                }
                frames.Add(new tk2dSpriteAnimationFrame { spriteId = frameSpriteId, spriteCollection = collection });
            }
            clip.frames = frames.ToArray();
            beamAnimation.clips = beamAnimation.clips.Concat(new tk2dSpriteAnimationClip[] { clip }).ToArray();
        }
        public static BeamController FreeFireBeamFromAnywhere(Projectile projectileToSpawn, PlayerController owner, GameObject otherShooter, Vector2 fixedPosition, bool usesFixedPosition, float targetAngle, float duration, bool skipChargeTime = false)
        {
            Vector2 sourcePos;
            if (usesFixedPosition) sourcePos = fixedPosition;
            else sourcePos = otherShooter.GetComponent<SpeculativeRigidbody>().UnitCenter;
            if (sourcePos != null)
            {

                GameObject gameObject = SpawnManager.SpawnProjectile(projectileToSpawn.gameObject, sourcePos, Quaternion.identity, true);
                Projectile component = gameObject.GetComponent<Projectile>();
                component.Owner = owner;
                BeamController component2 = gameObject.GetComponent<BeamController>();
                if (skipChargeTime)
                {
                    component2.chargeDelay = 0f;
                    component2.usesChargeDelay = false;
                }
                component2.Owner = owner;
                component2.HitsPlayers = false;
                component2.HitsEnemies = true;
                Vector3 vector = BraveMathCollege.DegreesToVector(targetAngle, 1f);
                component2.Direction = vector;
                component2.Origin = sourcePos;
                GameManager.Instance.Dungeon.StartCoroutine(BeamStuff.HandleFreeFiringBeam(component2, otherShooter, fixedPosition, usesFixedPosition, targetAngle, duration));
                return component2;
            }
            else
            {
                
                ETGModConsole.Log("ERROR IN BEAM FREEFIRE CODE. SOURCEPOS WAS NULL, EITHER DUE TO INVALID FIXEDPOS OR SOURCE GAMEOBJECT.");
                return null;
            }
        }
        private static IEnumerator HandleFreeFiringBeam(BeamController beam, GameObject otherShooter, Vector2 fixedPosition, bool usesFixedPosition, float targetAngle, float duration)
        {
            float elapsed = 0f;
            yield return null;
            while (elapsed < duration)
            {
                Vector2 sourcePos;
                if (otherShooter == null || otherShooter.GetComponent<SpeculativeRigidbody>() == null) { break; }
                if (usesFixedPosition) sourcePos = fixedPosition;
                else sourcePos = otherShooter.GetComponent<SpeculativeRigidbody>().UnitCenter;

                elapsed += BraveTime.DeltaTime;
                if (sourcePos != null)
                {
                    beam.Origin = sourcePos;
                    beam.LateUpdatePosition(sourcePos);

                }
                else { ETGModConsole.Log("SOURCEPOS WAS NULL IN BEAM FIRING HANDLER"); }
                yield return null;
            }
            beam.CeaseAttack();
            yield break;
        }
    }
}
