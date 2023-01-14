using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using Gungeon;
using BreakAbleAPI;
using Alexandria.ItemAPI;
using Alexandria.DungeonAPI;
using Dungeonator;

namespace Items
{
    class BulletDrum
    {
        public static void Init()
        {
            string defaultPath = "Items/Resources/Placeables/BulletDrum/";

            string[] idlePaths = new string[]
            {
                defaultPath+"bullet_drum_idle_001.png",
            };

            string[] rollNorthPaths = new string[]
            {
                defaultPath+"bullet_drum_roll_up_001.png",
                defaultPath+"bullet_drum_roll_up_002.png",
                defaultPath+"bullet_drum_roll_up_003.png",
                defaultPath+"bullet_drum_roll_up_004.png",
            };

            string[] rollRightPaths = new string[]
            {
                defaultPath+"bullet_drum_roll_right_001.png",
                defaultPath+"bullet_drum_roll_right_002.png",
                defaultPath+"bullet_drum_roll_right_003.png",
                defaultPath+"bullet_drum_roll_right_004.png",
            };

            string[] rollLeftPaths = new string[]
            {
                defaultPath+"bullet_drum_roll_left_001.png",
                defaultPath+"bullet_drum_roll_left_002.png",
                defaultPath+"bullet_drum_roll_left_003.png",
                defaultPath+"bullet_drum_roll_left_004.png",
            };

            string[] breakNorthPaths = new string[]
            {
                defaultPath+"bullet_drum_roll_up_burst_001.png",
            };

            string[] breakSidePaths = new string[]
            {
                defaultPath+"bullet_drum_side_burst_001.png",
            };

            string[] breakIdlePaths = new string[]
            {
                defaultPath+"bullet_drum_idle_burst_001.png",
            };

            string[] kickPaths = new string[]
            {
                defaultPath+"bullet_drum_kick_001.png",
            };

            KickableObject barrel = BreakableAPIToolbox.GenerateKickableObject("bullet_drum", idlePaths, rollNorthPaths, rollNorthPaths, rollRightPaths, rollLeftPaths, breakNorthPaths, breakNorthPaths, breakSidePaths, breakSidePaths, breakIdlePaths, breakIdlePaths, 10, 10, 10, 10, 10, true, 14, 20, 0, -4, true, false, 1, 1, 0, 0, "Play_OBJ_drum_break_01", 5, new List<CollisionLayer> {CollisionLayer.BulletBlocker, CollisionLayer.PlayerBlocker, CollisionLayer.EnemyBlocker, CollisionLayer.HighObstacle });
            barrel.gameObject.AddComponent<ExplodeBulletsOnDeath>();
            barrel.isPassable = false;
            barrel.RollingDestroysSafely = false;
            

            MinorBreakable breakable = barrel.GetComponent<MinorBreakable>();
            breakable.stopsBullets = true;
            breakable.isInvulnerableToGameActors = true;


            prefab = barrel.gameObject;
            StaticReferences.StoredRoomObjects.Add("BulletDrum", prefab);
        }

        class ExplodeBulletsOnDeath : MonoBehaviour
        {
            public void Start()
            {
                if (base.gameObject.GetComponent<MinorBreakable>())
                {
                    breakable = base.gameObject.GetComponent<MinorBreakable>();
                    breakable.OnBreakContext += SpawnBullets;
                    
                }
            }

            

            private void SpawnBullets(MinorBreakable self)
            {
                for(int i = 0; i < 6; i++)
                {
                    int m = i * 60;
                    GameObject obj = (EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").bulletBank.CreateProjectileFromBank(breakable.CenterPoint, m, "default"));
                    Projectile proj = obj.GetComponent<Projectile>();
                    proj.Shooter = breakable.specRigidbody;
                    proj.specRigidbody.RegisterSpecificCollisionException(breakable.specRigidbody);
                    proj.OwnerName = "A Barrel Of Bullets";
                }
                
            }

            
            MinorBreakable breakable;
        }
        class DelayedBreakAfterSpawn : MonoBehaviour
        {
            public void Start()
            {
                timeUntilDieDie = UnityEngine.Random.Range(5, 9);
                if (base.gameObject.GetComponent<MinorBreakable>())
                {
                    breakable = base.gameObject.GetComponent<MinorBreakable>();                   
                }
            }

            public void Update()
            {
                if(timeUntilDieDie > 0)
                {
                    timeUntilDieDie -= Time.deltaTime;
                }
                
                if(timeUntilDieDie <= 0)
                {
                    breakable.Break();
                }
            }

            float timeUntilDieDie = 1;
            MinorBreakable breakable;
        }
        public static GameObject prefab;
    }
}
