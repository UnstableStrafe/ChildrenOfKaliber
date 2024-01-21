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
    class PsiScales : AdvancedGunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Psi Scale", "psi_scale");
            Game.Items.Rename("outdated_gun_mods:psi_scale", "ck:psi_scale");
            gun.gameObject.AddComponent<PsiScales>();
            gun.SetShortDescription("With Power, Comes Greed");
            gun.SetLongDescription("Summons homing bolts of psychic energy to attack the nearest enemy in range. The energy will fizzle if there isn't a valid target in range.\n\n");
            gun.SetupSprite(null, "psi_scale_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 9);
            gun.SetAnimationFPS(gun.reloadAnimation, 10);
            gun.AddProjectileModuleFrom("ak-47");
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SMALL_BULLET;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.1f;
            gun.DefaultModule.angleVariance = 0;
            gun.DefaultModule.cooldownTime = .35f;
            gun.DefaultModule.numberOfShotsInClip = 10;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(145) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.SetBaseMaxAmmo(350);
            gun.quality = PickupObject.ItemQuality.C;
            gun.encounterTrackable.EncounterGuid = "skiddly biddly boo bop im going fucking insane send help please i am losing my mind i mean uh haha funny guid";
            gun.sprite.IsPerpendicular = true;
            gun.barrelOffset.transform.localPosition = new Vector3(2.25f, 0.3125f, 0f);
            gun.gunClass = GunClass.NONE;


            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage *= 1.1f;
            projectile.baseData.speed *= 1f;
            projectile.baseData.force *= 0f;
            projectile.SetProjectileSpriteRight("psi_scale_projectile", 10, 9);



            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
        }
        private bool HasReloaded;
        public override void PostProcessProjectile(Projectile projectile)
        {
            base.PostProcessProjectile(projectile);
            projectile.DieInAir(true);
        }
        protected override void Update()
        {
            base.Update();
            if (gun.CurrentOwner)
            {

                if (gun.PreventNormalFireAudio)
                {
                    this.gun.PreventNormalFireAudio = true;
                }
                if (!gun.IsReloading && !HasReloaded)
                {
                    this.HasReloaded = true;
                }
            }
        }
       
        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && this.HasReloaded)
            {
                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
            }
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            base.OnPostFired(player, gun);
            
            int shotsToSpawn = 1;
            shotsToSpawn *= GetPlayerShotMultiplier(player);
            float distance = 0;
            bool hasTarget = false;
            AIActor actor = null;
            
            if (player.CurrentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All))
            {
                actor = player.CurrentRoom.GetNearestEnemy(player.CenterPosition, out distance);
                if(actor != null)
                {
                    if(distance <= 13)
                    {
                        hasTarget = true;
                    }
                    
                }
            }
            
            for (int i = 0; i < shotsToSpawn; i++)
            {
                if(hasTarget && actor != null)
                {
                    SpawnTargettedProjectile(gun.DefaultModule.projectiles[0], actor, player);
                }
                else
                {
                    DoNoValidTargetPuff(player);
                }
            }
            
        }

        public AIActor GetNearestValidEnemy(RoomHandler room, Vector2 pos, out float distance)
        {
            AIActor result = null;
            distance = float.MaxValue;
            bool flag = room.HasActiveEnemies(RoomHandler.ActiveEnemyType.All);
            if (!flag)
            {
                return null;
            }
            List<AIActor> actorsInRoom = room.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            for(int i = 0; i < actorsInRoom.Count; i++)
            {
                if (!actorsInRoom[i].healthHaver.IsDead)
                {
                    float num = Vector2.Distance(pos, actorsInRoom[i].CenterPosition);
                    if (num < distance)
                    {
                        distance = num;
                        result = actorsInRoom[i];
                    }
                }
            }
            return result;
        }

        private int GetPlayerShotMultiplier(PlayerController player)
        {
            int num = 1;


            if (player.HasPassiveItem(241))
            {
                num += 2;
            }
            if (player.HasPassiveItem(568))
            {
                num += 1;
            }
            if(player.CurrentItem is ActiveGunVolleyModificationItem)
            {
                ActiveGunVolleyModificationItem activeItem = player.CurrentItem as ActiveGunVolleyModificationItem;
                if (player.CurrentItem.IsActive)
                {
                    num += activeItem.DuplicatesOfBaseModule;
                }
            }
            return num;
        }
        private void SpawnTargettedProjectile(Projectile projectile, AIActor fuckingDipshit, PlayerController player)
        {
            if(fuckingDipshit != null)
            {
                Vector2 pos = (UnityEngine.Random.insideUnitCircle * 6) + fuckingDipshit.CenterPosition;
                float angle = Vector2.Angle(pos, fuckingDipshit.CenterPosition);
                float offset = 0;
                if (player.HasPassiveItem(241))
                {
                    offset = UnityEngine.Random.Range(-6, 7);
                }
                GameObject component = SpawnManager.SpawnProjectile(projectile.gameObject, pos, Quaternion.Euler(0, 0, angle + offset));              
                Projectile proj2 = component.GetComponent<Projectile>();
                if(proj2 != null)
                {
                    proj2.Owner = player;
                    proj2.Shooter = player.specRigidbody;
                    proj2.gameObject.AddComponent<StoredAIActor>().actor = fuckingDipshit;
                    proj2.baseData.damage *= player.stats.GetStatModifier(PlayerStats.StatType.Damage);
                    proj2.specRigidbody.CollideWithTileMap = false;
                    proj2.UpdateCollisionMask();
                    proj2.shouldRotate = true;
                    proj2.SendInDirection(fuckingDipshit.CenterPosition - proj2.transform.position.XY(), false, true);
                   // proj2.OverrideMotionModule = new SendToTarget();
                    player.DoPostProcessProjectile(proj2);
                    if(proj2.gameObject.GetComponent<HomingModifier>() != null)
                    {
                        Destroy(proj2.gameObject.GetComponent<HomingModifier>());
                    }
                    if(proj2.gameObject.GetComponent<PierceProjModifier>() != null)
                    {
                        Destroy(proj2.gameObject.GetComponent<PierceProjModifier>());
                    }
                    if (proj2.gameObject.GetComponent<BounceProjModifier>() != null)
                    {
                        Destroy(proj2.gameObject.GetComponent<BounceProjModifier>());
                    }
                    if(proj2.gameObject.GetComponent<LockOnHomingModifier>() != null)
                    {
                        Destroy(proj2.gameObject.GetComponent<LockOnHomingModifier>()); 
                    }
                    
                    Color specialPink = new Color(254 / 10, 126 / 10, 229 / 10);
                    Color specialWhite = new Color(255 / 10, 236 / 10, 255 / 10);
                    //  trail.BaseColor = specialPink;
                   // Library.AddTrailToObject(proj2.gameObject, specialPink, null, 1, .1f, .5f, 0, specialPink, specialWhite);
                }

            }
        }
        
        private void DoNoValidTargetPuff(PlayerController player)
        {
            Vector2 pos = (UnityEngine.Random.insideUnitCircle * 3) + player.CenterPosition;
            LootEngine.DoDefaultPurplePoof(pos, true);
        }
        

        public PsiScales()
        {

        }
    }
    public class SendToTarget : ProjectileMotionModule
    {
        public override void Move(Projectile source, Transform projectileTransform, tk2dBaseSprite projectileSprite, SpeculativeRigidbody specRigidbody, ref float m_timeElapsed, ref Vector2 m_currentDirection, bool Inverted, bool shouldRotate)
        {
            source.specRigidbody.Velocity = Vector2.zero;
            PlayerController player = source.Owner as PlayerController;
            Vector2 centerPosition = source.Owner.CenterPosition;
            if (source.gameObject.GetComponent<StoredAIActor>() != null)
            {
                if (source.gameObject.GetComponent<StoredAIActor>().actor != null) 
                {
                    centerPosition = source.gameObject.GetComponent<StoredAIActor>().actor.CenterPosition;
                }
            }

            Vector2 vector = centerPosition - source.specRigidbody.UnitCenter;
            float magnitude = vector.magnitude;
            float d = Mathf.Lerp(10, 25, (magnitude - 25) / (12.5f - 25));
            source.specRigidbody.Velocity = vector.normalized * d;
        }

        public override void UpdateDataOnBounce(float angleDiff)
        {
            //throw new NotImplementedException();
        }
    }
    class StoredAIActor : MonoBehaviour
    {
        public AIActor actor;
    }
}
