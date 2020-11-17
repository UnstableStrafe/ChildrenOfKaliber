using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using Gungeon;
using ItemAPI;
using MonoMod;
using MonoMod.RuntimeDetour;
using Random = UnityEngine.Random;
using UnityEngine;
using FullInspector;


namespace Items
{
    public class Replacement : MonoBehaviour
    {
        public static void RunReplace(List<AGDEnemyReplacementTier> m_cachedReplacementTiers)
        {
            if (m_cachedReplacementTiers != null)
            {
                for (int i = 0; i < m_cachedReplacementTiers.Count; i++)
                {
                    if (m_cachedReplacementTiers[i].Name.ToLower().EndsWith("_celsewers") | m_cachedReplacementTiers[i].Name.ToLower().EndsWith("_celmines") | m_cachedReplacementTiers[i].Name.ToLower().EndsWith("_celhell"))
                    {
                        m_cachedReplacementTiers.Remove(m_cachedReplacementTiers[i]);
                    }
                }
                InitReplacementEnemiesForSewers(m_cachedReplacementTiers);
                InitReplacementEnemiesForMines(m_cachedReplacementTiers);
                InitReplacementEnemiesForHell(m_cachedReplacementTiers);
            }
        }

        private static void InitReplacementEnemiesForSewers(List<AGDEnemyReplacementTier> agdEnemyReplacementTiers)
        {
            GlobalDungeonData.ValidTilesets TargetTileset = GlobalDungeonData.ValidTilesets.SEWERGEON;
            string nameAppend = "_celsewers";
            agdEnemyReplacementTiers.Add(GenerateEnemyReplacementTier("hollowpointReplace" + nameAppend, new DungeonPrerequisite[0], TargetTileset, HollowPoint, Spectre, 1f));
            agdEnemyReplacementTiers.Add(GenerateEnemyReplacementTier("poisblobReplace" + nameAppend, new DungeonPrerequisite[0], TargetTileset, PoisBlob, Spectre, .5f));
            return;
        }

        private static void InitReplacementEnemiesForMines(List<AGDEnemyReplacementTier> agdEnemyReplacementTiers)
        {
            GlobalDungeonData.ValidTilesets TargetTileset = GlobalDungeonData.ValidTilesets.MINEGEON;
            string nameAppend = "_celmines";
            agdEnemyReplacementTiers.Add(GenerateEnemyReplacementTier("hollowpointReplace" + nameAppend, new DungeonPrerequisite[0], TargetTileset, HollowPoint, Spectre, .8f));
            agdEnemyReplacementTiers.Add(GenerateEnemyReplacementTier("poisblobReplace" + nameAppend, new DungeonPrerequisite[0], TargetTileset, PoisBlob, Spectre, .3f));
            return;
        }
        private static void InitReplacementEnemiesForHell(List<AGDEnemyReplacementTier> agdEnemyReplacementTiers)
        {
            GlobalDungeonData.ValidTilesets TargetTileset = GlobalDungeonData.ValidTilesets.HELLGEON;
            string nameAppend = "_celhell";
            agdEnemyReplacementTiers.Add(GenerateEnemyReplacementTier("hollowpointReplace" + nameAppend, new DungeonPrerequisite[0], TargetTileset, HollowPoint, Spectre, 1f));
            agdEnemyReplacementTiers.Add(GenerateEnemyReplacementTier("poisblobReplace" + nameAppend, new DungeonPrerequisite[0], TargetTileset, PoisBlob, Spectre, .5f));
            return;
        }

        public static AGDEnemyReplacementTier GenerateEnemyReplacementTier(string m_name, DungeonPrerequisite[] m_Prereqs, GlobalDungeonData.ValidTilesets m_TargetTileset, List<string> m_TargetGuids, List<string> m_ReplacementGUIDs, float m_ChanceToReplace = 1)
        {
            AGDEnemyReplacementTier m_cachedEnemyReplacementTier = new AGDEnemyReplacementTier()
            {
                Name = m_name,
                Prereqs = m_Prereqs,
                TargetTileset = m_TargetTileset,
                ChanceToReplace = m_ChanceToReplace,
                MaxPerFloor = -1,
                MaxPerRun = -1,
                TargetAllNonSignatureEnemies = false,
                TargetAllSignatureEnemies = false,
                TargetGuids = m_TargetGuids,
                ReplacementGuids = m_ReplacementGUIDs,
                RoomMustHaveColumns = false,
                RoomMinEnemyCount = -1,
                RoomMaxEnemyCount = -1,
                RoomMinSize = -1,
                RemoveAllOtherEnemies = false,
                RoomCantContain = new List<string>()
            };
            return m_cachedEnemyReplacementTier;
        }
        public static List<string> PoisBlob = new List<string>()
        {
            "e61cab252cfb435db9172adc96ded75f"
        };
        public static List<string> HollowPoint = new List<string>() {
                "4db03291a12144d69fe940d5a01de376"
        };
        public static List<string> Spectre = new List<string>() {
                "56f5a0f2c1fc4bc78875aea617ee31ac"
        };
    }
    class Enemies : EnemyDatabase
    {


        public static Hook loadEnemyGUIDHook;
        public static Dictionary<string, GameObject> enemyPrefabDictionary = new Dictionary<string, GameObject>();

        //Spectre Stuff
        public static AIActor Spectre;
        public static AIActor HollowPoint;
        //ammoconda dots
        public static AIActor AmmoDots;
        public static AIActor FallenKin;

        public static void AmmoDotBehav()
        {
            FallenKin = EnemyDatabase.GetOrLoadByGuid("5f3abc2d561b4b9c9e72b879c6f10c7e");
            AmmoDots = EnemyDatabase.GetOrLoadByGuid("f38686671d524feda75261e469f30e0b");


            AmmoDots.healthHaver.SetHealthMaximum(20);
            AmmoDots.healthHaver.ForceSetCurrentHealth(20);
        }
        public static void SpectreBehav()
        {
            Gun cached_poison = PickupObjectDatabase.GetById(208) as Gun;
            Spectre = EnemyDatabase.GetOrLoadByGuid("56f5a0f2c1fc4bc78875aea617ee31ac");
            HollowPoint = EnemyDatabase.GetOrLoadByGuid("4db03291a12144d69fe940d5a01de376");
            AIShooter SpecShoot = Spectre.gameObject.AddComponent<AIShooter>();
            Spectre.gameObject.GetComponent<AIShooter>().gunAttachPoint.localPosition += new Vector3(0, 0.2f, 0);
            GameObject targetPoison = Spectre.gameObject;
            Spectre.behaviorSpeculator.AttackBehaviors.Remove(Spectre.behaviorSpeculator.AttackBehaviors[1]);
            Spectre.behaviorSpeculator.AttackBehaviors.Add(HollowPoint.behaviorSpeculator.AttackBehaviors[1]);
            Spectre.CollisionDamage = .5f;
            Spectre.SetIsFlying(true, "ghost", true, true);            
            var actor = Spectre.gameActor;
            actor.EffectResistances = new ActorEffectResistance[]
            {
                new ActorEffectResistance()
                {
                    resistAmount = 1,
                    resistType = EffectResistanceType.Poison
                }
            };

            Spectre.healthHaver.SetHealthMaximum(30);
            Spectre.healthHaver.ForceSetCurrentHealth(30);
            
            
          
            if (cached_poison)
            {
                GoopDoer Gooper = targetPoison.AddComponent<GoopDoer>();
                Gooper.goopDefinition = cached_poison.singleModule.projectiles[0].gameObject.GetComponent<GoopModifier>().goopDefinition;
                Gooper.positionSource = GoopDoer.PositionSource.HitBoxCenter;
                Gooper.updateTiming = GoopDoer.UpdateTiming.Always;
                Gooper.updateFrequency = 0.05f;
                Gooper.isTimed = false;
                Gooper.goopTime = 1;
                Gooper.updateOnDeath = false;
                Gooper.updateOnPreDeath = false;
                Gooper.updateOnAnimFrames = false;
                Gooper.updateOnCollision = false;
                Gooper.updateOnGrounded = false;
                Gooper.updateOnDestroy = false;
                Gooper.defaultGoopRadius = 1.2f;
                Gooper.suppressSplashes = false;
                Gooper.goopSizeVaries = false;
                Gooper.varyCycleTime = 0.9f;
                Gooper.radiusMax = 1.2f;
                Gooper.radiusMin = 1.2f;
                Gooper.goopSizeRandom = false;
                Gooper.UsesDispersalParticles = false;
                Gooper.DispersalDensity = 3;
                Gooper.DispersalMinCoherency = 0.2f;
                Gooper.DispersalMaxCoherency = 1;
            }
            
          
        }

        //--------------------------------------------------------------------------------------------------------------
        //Midas Bulletkin
        //--------------------------------------------------------------------------------------------------------------
        public static GameObject MidasPrefab;
        public static string MidasGuid;
        public static GameObject MidasCorpse;

        //--------------------------------------------------------------------------------------------------------------
        //Gungeoneer kin
        //--------------------------------------------------------------------------------------------------------------

        //ConvictKin


    }
}
