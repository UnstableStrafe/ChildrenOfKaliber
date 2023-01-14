using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;
using System.Collections;

namespace Items
{
    class RiskStatManager 
    {
    }
    public class RiskStat : MonoBehaviour
    {
        private RiskStat()
        {
            RiskAMT = 0;
        }
        public void Start()
        {
            player = base.GetComponent<PlayerController>();
        }
        private void FixedUpdate()
        {
            
            
        }


        public float RiskAMT;
        private PlayerController player;
    }
   
    public class RiskParticles : MonoBehaviour
    {
        public RiskParticles()
        {
            zOffset = 0;
        }
        private void Start()
        {
            sprite = base.GetComponent<tk2dSprite>();
            
        }
        private void FixedUpdate()
        {
            if (sprite)
            {
                vector = sprite.WorldBottomLeft.ToVector3ZisY(zOffset);
                vectorDos = sprite.WorldTopRight.ToVector3ZisY(zOffset);
                float num = (vectorDos.y - vector.y) * (vectorDos.x - vector.x);
                float num2 = 25f * num;
                int num3 = Mathf.CeilToInt(Mathf.Max(1f, num2 * BraveTime.DeltaTime));
                int num4 = num3;
                Vector3 minPosition = vector;
                Vector3 maxPosition = vectorDos;
                Vector3 direction = Vector3.up / 2f;
                float angleVariance = 120f;
                float magnitudeVariance = 0.2f;
                float? startLifetime = new float?(UnityEngine.Random.Range(0.8f, 1.25f));
                GlobalSparksDoer.DoRandomParticleBurst(num4, minPosition, maxPosition, direction, angleVariance, magnitudeVariance, null, startLifetime, null, GlobalSparksDoer.SparksType.BLOODY_BLOOD);
            }
            
        }
        public float zOffset;
        private Vector3 vector;
        private Vector3 vectorDos;
        private tk2dSprite sprite;
    }
    public class PerilousParticles : MonoBehaviour
    {
        public PerilousParticles()
        {
            zOffset = 0;
            doParticles = true;
            DidOnPickup = false;
        }
        private void Start()
        {
            sprite = base.GetComponent<tk2dSprite>();
            
            if(base.GetComponent<PickupObject>() != null)
            {
               
            }
            if(base.GetComponent<Gun>() != null)
            {
                gun = base.GetComponent<Gun>();
            }
            if (gun.HasShootStyle(ProjectileModule.ShootStyle.Beam))
            {
                do
                {
                    perilMod = (PerilousParticles.PerilousMods)(UnityEngine.Random.Range(1, 7));
                } while (perilMod == PerilousMods.JAMMED);
            }
            else
            {
                perilMod = (PerilousParticles.PerilousMods)(UnityEngine.Random.Range(1, 7));
            }
            switch (perilMod)
            {
                case PerilousMods.HAUNTED:
                    TextLine = PerilModTextLines[1];
                    break;
                case PerilousMods.JAMMED:
                    TextLine = PerilModTextLines[2];
                    break;
                case PerilousMods.FRAIL:
                    TextLine = PerilModTextLines[3];
                    break;
                case PerilousMods.EXPLOSIVE:
                    TextLine = PerilModTextLines[4];
                    break;

                case PerilousMods.RISEN:
                    TextLine = PerilModTextLines[5];
                    break;
                case PerilousMods.CHAOTIC:
                    TextLine = PerilModTextLines[6];
                    break;


            }
        }
        private void FixedUpdate()
        {
            if (sprite && doParticles)
            {
                vector = sprite.WorldBottomLeft.ToVector3ZisY(zOffset);
                vectorDos = sprite.WorldTopRight.ToVector3ZisY(zOffset);
                float num = (vectorDos.y - vector.y) * (vectorDos.x - vector.x);
                float num2 = 12f * num;
                int num3 = Mathf.CeilToInt(Mathf.Max(1f, num2 * BraveTime.DeltaTime));
                int num4 = num3;
                
                Vector3 minPosition = vector;
                Vector3 maxPosition = vectorDos;
                Vector3 direction = Vector3.up / 2f;
                float angleVariance = 120f;
                float magnitudeVariance = 0.2f;
                float? startLifetime = new float?(UnityEngine.Random.Range(0.8f, 1.25f));
                GlobalSparksDoer.DoRandomParticleBurst(num4, minPosition, maxPosition, direction, angleVariance, magnitudeVariance, null, startLifetime, null, GlobalSparksDoer.SparksType.STRAIGHT_UP_FIRE);
                if(gun.gameObject.GetComponent<MimicGunMimicModifier>() != null)
                {
                    Destroy(gun.gameObject.GetComponent<MimicGunMimicModifier>());
                }
            }
            if (gun.HasBeenPickedUp)
            {
                doParticles = false;
                HandleAddAmmo();
                if (!DidOnPickup)
                {
                    PseudoOnPickup();
                    DidOnPickup = true;
                }
                
                
            }

        }
        private void PseudoOnPickup()
        {
            if (gun.CurrentOwner)
            {
                if (gun.CurrentOwner is PlayerController)
                {
                    player = gun.CurrentOwner as PlayerController;

                    if (perilMod == PerilousMods.CHAOTIC)
                    {
                        ETGModConsole.Log("Chaotic");
                        perilMod = (PerilousParticles.PerilousMods)(UnityEngine.Random.Range(1, 6));
                        
                        
                    }

                    if (perilMod == PerilousMods.JAMMED)
                    {
                        ETGModConsole.Log("Jammed");
                        player.PostProcessProjectile += PostProcessJamShots;
                    }
                    else if (perilMod == PerilousMods.FRAIL)
                    {
                        ETGModConsole.Log("Frail");
                        player.healthHaver.ModifyDamage += DealDamage;
                    }
                    else if (perilMod == PerilousMods.EXPLOSIVE || perilMod == PerilousMods.RISEN)
                    {
                        ETGModConsole.Log("Explosive or Risen");
                        ETGMod.AIActor.OnPreStart = (Action<AIActor>)Delegate.Combine(ETGMod.AIActor.OnPreStart, new Action<AIActor>(HandleApplyEffect));
                    }
                    else if (perilMod == PerilousMods.HAUNTED)
                    {
                        ETGModConsole.Log("Haunted");
                        IntVector2? spawnPosMaybe = player.CurrentRoom.GetRandomVisibleClearSpot(1, 1);
                        if(spawnPosMaybe != null)
                        {
                            GameObject PJ = UnityEngine.Object.Instantiate(PrinceOfTheJammed.PJPrefab, spawnPosMaybe.Value.ToCenterVector2(), Quaternion.Euler(0, 0, 0));
                            GameManager.Instance.OnNewLevelFullyLoaded += StartPJSpawn;
                            AkSoundEngine.PostEvent("Play_ENM_reaper_spawn_01", gameObject);
                        }
                        else if( spawnPosMaybe == null)
                        {
                            ETGModConsole.Log($"How the FUCK did you manage to make spawnPosMaybe null?");
                        }
                    }
                    player.gameObject.GetOrAddComponent<RiskStat>().RiskAMT += 1;
                    player.inventory.GunLocked.AddOverride("perilous gun", null);
                }
            }
        }

        

        private bool DidOnPickup;

        private void HandleAddAmmo()
        {
            if(gun.ammo <= Mathf.FloorToInt(gun.AdjustedMaxAmmo * .10f))
            {
                gun.GainAmmo(gun.AdjustedMaxAmmo);
            }
        }
        //----------------------------
        //JAMMED code
        private void PostProcessJamShots(Projectile proj, float eff)
        {
            if((proj.Owner as PlayerController).CurrentGun == base.GetComponent<Gun>())
            {
                proj.collidesWithProjectiles = true;

                proj.specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(proj.specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(ControlJammingShots));
            }
            
        }
        private void ControlJammingShots(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            if (otherRigidbody && otherRigidbody.projectile)
            {
                if (otherRigidbody.projectile.Owner is AIActor)
                {

                    if (!otherRigidbody.projectile.IsBlackBullet && otherRigidbody.projectile.CanBecomeBlackBullet)
                    {
                        otherRigidbody.projectile.BecomeBlackBullet();
                    }

                }
                bool isShootableBullet = otherRigidbody.projectile.collidesWithProjectiles;

                PhysicsEngine.SkipCollision = true;
            }
        }
        //----------------------------
        //FRAIL code
        private void DealDamage(HealthHaver healthHaver, HealthHaver.ModifyDamageEventArgs args)
        {
            if (args == EventArgs.Empty)
            {
                return;
            }
            float num = UnityEngine.Random.Range(0f, 1f);
            if (num <= 0.25f && args.InitialDamage == .5f)
            {
                args.ModifiedDamage = 1;

            }
        }
        //----------------------------
        //EXPLOSIVE/RISEN code
        private void HandleApplyEffect(AIActor aiActor)
        {
            if(perilMod == PerilousMods.EXPLOSIVE)
            {
                SpawnRPGOnDeath spawnRPG = new SpawnRPGOnDeath()
                {
                    duration = 9999999999,
                    AffectsEnemies = true,
                    AffectsPlayers = false, // I cannot imagine why it would be applied to a player, but just in case.
                };
                aiActor.ApplyEffect(spawnRPG);
            }   
            else if(perilMod == PerilousMods.RISEN) //It's "else if" in case of some godforsaken reason that this method gets applied when it shouldn't, it won't accidentally happen. I hope.
            {
                SpawnSpentOnDeathEffect spawnSpent = new SpawnSpentOnDeathEffect()
                {
                    duration = 9999999999,
                    AffectsEnemies = true,
                    AffectsPlayers = false, // I cannot imagine why it would be applied to a player, but just in case.
                };
                aiActor.ApplyEffect(spawnSpent);
            }
        }

        //----------------------------
        //HAUNTED code
        private void StartPJSpawn()
        {
            GameManager.Instance.StartCoroutine(DelayedSpawnPJ());
        }
        private IEnumerator DelayedSpawnPJ()
        {
            yield return new WaitForSeconds(4);
            IntVector2? spawnPosMaybe2 = player.CurrentRoom.GetRandomVisibleClearSpot(1, 1);
            if (spawnPosMaybe2 != null)
            {
                GameObject PJ = UnityEngine.Object.Instantiate(PrinceOfTheJammed.PJPrefab, spawnPosMaybe2.Value.ToCenterVector2(), Quaternion.Euler(0, 0, 0));
                AkSoundEngine.PostEvent("Play_ENM_reaper_spawn_01", gameObject);
            }
            else if (spawnPosMaybe2 == null)
            {
                ETGModConsole.Log($"How the FUCK did you manage to make spawnPosMaybe2 null?");
            }
            yield break;
        }
        //----------------------------
        private void OnDestroy()
        {
            player.PostProcessProjectile -= PostProcessJamShots;
            player.healthHaver.ModifyDamage -= DealDamage;
            ETGMod.AIActor.OnPreStart = (Action<AIActor>)Delegate.Remove(ETGMod.AIActor.OnPreStart, new Action<AIActor>(HandleApplyEffect));
            GameManager.Instance.OnNewLevelFullyLoaded -= StartPJSpawn;
            player.inventory.GunLocked.RemoveOverride("perilous gun");
        }
        public float zOffset;
        private bool doParticles;
        private Vector3 vector;
        private Vector3 vectorDos;
        private tk2dSprite sprite;
        private PlayerController player;
        private Gun gun;




        private PerilousMods perilMod;

        public enum PerilousMods
        {
            NONE, //purely debug
            HAUNTED, //ghost-like enemy follows the player until the Peril Mod is removed from the gun. Acts like a melee only LJ
            JAMMED, //Shots that the player's projectiles travel over will become jammed
            FRAIL, //Taking damage has a chance to deal double damage (only works with half-heart damage)
            EXPLOSIVE, //Enemies shoot an RPG projectile at the player on death
            RISEN, //Enemies spawn Spent upon death
            CHAOTIC, //Chooses another mod from this list. Cannot pick itself
        }
        public string TextLine;
        private List<string> PerilModTextLines = new List<string>
        {

            "None:\nThis is for debug only.\nHow did you find this?",
            "Haunted:\nSummons the Prince of The Jammed to hunt you.",
            "Jammed:\nShots that your projectiles touch become jammed.",
            "Frail:\n1/2 heart damage has a chance to be doubled.",
            "Explosive:\n Most enemies shoot an RPG on death.",
            "Risen:\nMost enemies spawn Spent on death.",
            "Chaotic:\nBecomes another random Peril mod on pickup."

        };
    }

    public class SpawnSpentOnDeathEffect : GameActorEffect
    {
        public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1)
        {
            base.OnEffectApplied(actor, effectData, partialAmount);
            try
            {
                effectData.OnActorPreDeath = delegate (Vector2 dir)
                {
                    if (actor is AIActor)
                    {
                        AIActor aiActor = actor as AIActor;
                        if (aiActor.GetAbsoluteParentRoom() == GameManager.Instance.GetRandomActivePlayer().CurrentRoom && !aiActor.IgnoreForRoomClear && !noNoGUIDs.Contains(aiActor.EnemyGuid))
                        {

                            GameManager.Instance.StartCoroutine(DelayedSpawnSpent(actor.CenterPosition));
                        }
                    }
                };
                actor.healthHaver.OnPreDeath += effectData.OnActorPreDeath;
            }
            catch(Exception e)
            {
                ETGModConsole.Log(e.ToString());
            }
        }

        private IEnumerator DelayedSpawnSpent(Vector2 vector)
        {
            yield return new WaitForSeconds(.01f);
            IntVector2 intV = vector.ToIntVector2(VectorConversions.Floor);
            AIActor aiactor2 = AIActor.Spawn(EnemyDatabase.GetOrLoadByGuid("e21ac9492110493baef6df02a2682a0d"), intV, GameManager.Instance.GetRandomActivePlayer().CurrentRoom, true, AIActor.AwakenAnimationType.Default, true);
            yield break;
        }
        private List<string> noNoGUIDs = new List<string>
        {
            "249db525a9464e5282d02162c88e0357",
            "42be66373a3d4d89b91a35c9ff8adfec",
            "042edb1dfb614dc385d5ad1b010f2ee3",
            "d1c9781fdac54d9e8498ed89210a0238",
            "b8103805af174924b578c98e95313074",
            "fe3fe59d867347839824d5d9ae87f244",
            "0ff278534abb4fbaaa65d3f638003648",
            "e21ac9492110493baef6df02a2682a0d",
            "2feb50a6a40f4f50982e89fd276f6f15",
            "2d4f8b5404614e7d8b235006acde427a",
            "b4666cb6ef4f4b038ba8924fd8adf38",
            "7ec3e8146f634c559a7d58b19191cd43",

        };
    }
    public class SpawnRPGOnDeath : GameActorEffect
    {
        public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1)
        {
            base.OnEffectApplied(actor, effectData, partialAmount);
            effectData.OnActorPreDeath = delegate (Vector2 dir)
            {
                if (actor is AIActor)
                {
                    AIActor aiActor = actor as AIActor;
                    if (aiActor.GetAbsoluteParentRoom() == GameManager.Instance.GetActivePlayerClosestToPoint(actor.CenterPosition).CurrentRoom && !aiActor.IgnoreForRoomClear && !noNoGUIDs.Contains(aiActor.EnemyGuid))
                    {
                        Vector2 dirVec = UnityEngine.Random.insideUnitCircle;
                        GameObject gameObject = SpawnManager.SpawnProjectile(((Gun)ETGMod.Databases.Items[39]).DefaultModule.projectiles[0].gameObject, actor.CenterPosition, Quaternion.Euler(0, 0, 0));
                        Projectile proj = gameObject.GetComponent<Projectile>();
                        if(proj != null)
                        {
                            proj.Owner = actor;
                            proj.Shooter = aiActor.specRigidbody;
                            proj.collidesWithEnemies = false;
                            proj.collidesWithPlayer = true;
                            proj.MakeLookLikeEnemyBullet(true);
                            proj.gameObject.AddComponent<PierceDeadActors>();
                            PlayerController closestToPos = BraveUtility.GetClosestToPosition<PlayerController>(GameManager.Instance.AllPlayers.ToList(), aiActor.CenterPosition);
                            if (closestToPos)
                            {
                                dirVec = closestToPos.CenterPosition - proj.transform.position.XY();
                            }
                            proj.SendInDirection(dirVec, false, true);
                        }

                    }
                }
            };
            actor.healthHaver.OnPreDeath += effectData.OnActorPreDeath;


        }

        
        private List<string> noNoGUIDs = new List<string>
        {
            "249db525a9464e5282d02162c88e0357",
            "42be66373a3d4d89b91a35c9ff8adfec",
            "042edb1dfb614dc385d5ad1b010f2ee3",
            "d1c9781fdac54d9e8498ed89210a0238",
            "b8103805af174924b578c98e95313074",
            "fe3fe59d867347839824d5d9ae87f244",
            "0ff278534abb4fbaaa65d3f638003648",
            "e21ac9492110493baef6df02a2682a0d",
            "2feb50a6a40f4f50982e89fd276f6f15",
            "2d4f8b5404614e7d8b235006acde427a",
            "b4666cb6ef4f4b038ba8924fd8adf38",
            "7ec3e8146f634c559a7d58b19191cd43",


        };
    }


}
