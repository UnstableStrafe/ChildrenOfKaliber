using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using Dungeonator;
using ItemAPI;
using UnityEngine;
using System.Collections;

namespace Items
{
    class SupportContract : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Support Contract";

            string resourceName = "Items/Resources/support_contract.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<SupportContract>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Call In The Big Guns";
            string longDesc = "Summons a temporary group of Bullet Kin to aid you in battle! \n\nThe Gundead, aside from being irreparably stupid, are extremely loyal. These bullet kin will fight to the death for anyone holding this contract.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 650);

            item.quality = ItemQuality.B;
            item.sprite.IsPerpendicular = true;
        }
        int amountToSummon = 5;
        float duration = 15;
        protected override void DoEffect(PlayerController user)
        {
            base.DoEffect(user);
            SpawnMainWave(user);
            SpawnExtraWaves(user);
            StartCoroutine(ItemBuilder.HandleDuration(this, duration, user, PlaySoundAtEnd));
        }
        private void PlaySoundAtEnd(PlayerController user)
        {
            AkSoundEngine.PostEvent("Play_OBJ_teleport_depart_01", user.gameObject);
        }
        private List<AIActor> SummonsAlive = new List<AIActor> { };
        private void SpawnMainWave(PlayerController user)
        {
            AkSoundEngine.PostEvent("Play_OBJ_teleport_arrive_01", user.gameObject);
            AIActor BulletKin = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5");
            for(int i = 0; i < amountToSummon; i++)
            {
                IntVector2? spawnPos = user.CurrentRoom.GetRandomVisibleClearSpot(1, 1);
                if (spawnPos != null)
                {
                    AIActor TargetActor = AIActor.Spawn(BulletKin, spawnPos.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(spawnPos.Value), true, AIActor.AwakenAnimationType.Default, true);
                    TargetActor.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
                    PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(TargetActor.specRigidbody, null, false);
                    TargetActor.gameObject.AddComponent<GoAwayAfterRoomClear>();
                    TargetActor.IsHarmlessEnemy = true;
                    TargetActor.IgnoreForRoomClear = true;
                    TargetActor.StartCoroutine(HandleTimer(TargetActor, duration));
                    float hpMax = TargetActor.healthHaver.GetMaxHealth();
                    TargetActor.healthHaver.SetHealthMaximum(hpMax * 3f);
                    TargetActor.healthHaver.FullHeal();
                    TargetActor.CanTargetPlayers = false;
                    CompanionisedEnemyBulletModifiers mod = TargetActor.gameObject.AddComponent<CompanionisedEnemyBulletModifiers>();
                    if(user.PlayerHasActiveSynergy("Great Leadership"))
                    {
                        mod.doPostProcess = true;
                    }
                    mod.TintBullets = true;
                    mod.TintColor = new Color(252, 116, 216, 1);
                    TargetActor.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.PlayerCollider));
                    GameObject gameObject = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Teleport_Beam");
                    SpawnManager.SpawnVFX(gameObject, TargetActor.specRigidbody.UnitCenter, Quaternion.Euler(0, 0, 0));
                }
            }
        }
        private void SpawnExtraWaves(PlayerController user)
        {            
            if (user.PlayerHasActiveSynergy(SynNames[0]))
            {
                IntVector2? spawnPos = user.CurrentRoom.GetRandomVisibleClearSpot(1, 1);
                int amount = 2;
                for (int i = 0; i < amount; i++)
                {
                    AIActor HeavyKin = EnemyDatabase.GetOrLoadByGuid("df7fb62405dc4697b7721862c7b6b3cd");
                    AIActor TargetActor = AIActor.Spawn(HeavyKin, spawnPos.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(spawnPos.Value), true, AIActor.AwakenAnimationType.Default, true);
                    (TargetActor.behaviorSpeculator.AttackBehaviors[0] as ShootGunBehavior).Range = (EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").behaviorSpeculator.AttackBehaviors[0] as ShootGunBehavior).Range;
                    TargetActor.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
                    PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(TargetActor.specRigidbody, null, false);
                    TargetActor.gameObject.AddComponent<GoAwayAfterRoomClear>();
                    TargetActor.IsHarmlessEnemy = true;
                    TargetActor.IgnoreForRoomClear = true;
                    TargetActor.StartCoroutine(HandleTimer(TargetActor, duration));
                    float hpMax = TargetActor.healthHaver.GetMaxHealth();
                    TargetActor.healthHaver.SetHealthMaximum(hpMax * 3f);
                    TargetActor.healthHaver.FullHeal();
                    TargetActor.CanTargetPlayers = false;
                    CompanionisedEnemyBulletModifiers mod = TargetActor.gameObject.AddComponent<CompanionisedEnemyBulletModifiers>();
                    if (user.PlayerHasActiveSynergy("Great Leadership"))
                    {
                        mod.doPostProcess = true;
                    }
                    mod.TintBullets = true;
                    mod.TintColor = new Color(252, 116, 216, 1);
                    TargetActor.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.PlayerCollider));
                    GameObject gameObject = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Teleport_Beam");
                    SpawnManager.SpawnVFX(gameObject, TargetActor.specRigidbody.UnitCenter, Quaternion.Euler(0, 0, 0));
                }
            }
        }
        private IEnumerator HandleTimer(AIActor self, float duration)
        {
            float elapsed = 0;
            while(elapsed < duration)
            {
                elapsed += BraveTime.DeltaTime;
                if (self.healthHaver.IsDead)
                {
                    yield break;
                }
                yield return null;
            }
            if (self.healthHaver.IsDead)
            {
                yield break;
            }
            GameObject gameObject = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Teleport_Beam");
            SpawnManager.SpawnVFX(gameObject, self.specRigidbody.UnitCenter, Quaternion.Euler(0, 0, 0));
            self.healthHaver.PreventAllDamage = false;
            self.EraseFromExistence(true);
            yield break;
        }
        private List<string> SynNames = new List<string>
        {
            "Calling In The Heavy Support",
        };
    }
    public class GoAwayAfterRoomClear : BraveBehaviour
    {
        GoAwayAfterRoomClear()
        {
            VFXToUse = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Teleport_Beam");
            soundToUse = "Play_OBJ_teleport_depart_01";
        }
        public void Start()
        {
            RoomHandler parentRoom = base.aiActor.ParentRoom;
            parentRoom.OnEnemiesCleared = (Action)Delegate.Combine(parentRoom.OnEnemiesCleared, new Action(this.RoomCleared));
        }
        protected override void OnDestroy()
        {
            if (base.aiActor && base.aiActor.ParentRoom != null)
            {
                RoomHandler parentRoom = base.aiActor.ParentRoom;
                parentRoom.OnEnemiesCleared = (Action)Delegate.Remove(parentRoom.OnEnemiesCleared, new Action(this.RoomCleared));
            }
            base.OnDestroy();
        }
        private void RoomCleared()
        {
            if (!string.IsNullOrEmpty(this.overrideDeathAnim) && base.aiAnimator)
            {
                bool flag = false;
                for (int i = 0; i < base.aiAnimator.OtherAnimations.Count; i++)
                {
                    if (base.aiAnimator.OtherAnimations[i].name == "death")
                    {
                        base.aiAnimator.OtherAnimations[i].anim.Type = DirectionalAnimation.DirectionType.Single;
                        base.aiAnimator.OtherAnimations[i].anim.Prefix = this.overrideDeathAnim;
                        flag = true;
                    }
                }
                if (!flag)
                {
                    AIAnimator.NamedDirectionalAnimation namedDirectionalAnimation = new AIAnimator.NamedDirectionalAnimation();
                    namedDirectionalAnimation.name = "death";
                    namedDirectionalAnimation.anim = new DirectionalAnimation();
                    namedDirectionalAnimation.anim.Type = DirectionalAnimation.DirectionType.Single;
                    namedDirectionalAnimation.anim.Prefix = this.overrideDeathAnim;
                    namedDirectionalAnimation.anim.Flipped = new DirectionalAnimation.FlipType[1];
                    base.aiAnimator.OtherAnimations.Add(namedDirectionalAnimation);
                }
            }
            if (this.preventExplodeOnDeath)
            {
                ExplodeOnDeath component = base.GetComponent<ExplodeOnDeath>();
                if (component)
                {
                    component.enabled = false;
                }
            }
            AkSoundEngine.PostEvent(soundToUse, gameObject);
            SpawnManager.SpawnVFX(gameObject, base.specRigidbody.UnitCenter, Quaternion.Euler(0, 0, 0));
            base.healthHaver.PreventAllDamage = false;
            base.aiActor.EraseFromExistence(true);
        }
        [CheckAnimation(null)]
        public string overrideDeathAnim;

        public bool preventExplodeOnDeath;
        public GameObject VFXToUse;
        public string soundToUse;
    }
    public class CompanionisedEnemyBulletModifiers : BraveBehaviour 
    {
        public CompanionisedEnemyBulletModifiers()
        {
            this.scaleDamage = false;
            this.scaleSize = false;
            this.scaleSpeed = false;
            this.doPostProcess = false;
            this.baseBulletDamage = 10f;
            this.TintBullets = false;
            this.TintColor = Color.grey;
            this.jammedDamageMultiplier = 2f;
        }
        public void Start()
        {
            enemy = base.aiActor;
            AIBulletBank bulletBank2 = enemy.bulletBank;
            foreach (AIBulletBank.Entry bullet in bulletBank2.Bullets)
            {
                bullet.BulletObject.GetComponent<Projectile>().BulletScriptSettings.preventPooling = true;
            }
            if (enemy.aiShooter != null)
            {
                AIShooter aiShooter = enemy.aiShooter;
                aiShooter.PostProcessProjectile = (Action<Projectile>)Delegate.Combine(aiShooter.PostProcessProjectile, new Action<Projectile>(this.PostProcessSpawnedEnemyProjectiles));
            }

            if (enemy.bulletBank != null)
            {
                AIBulletBank bulletBank = enemy.bulletBank;
                bulletBank.OnProjectileCreated = (Action<Projectile>)Delegate.Combine(bulletBank.OnProjectileCreated, new Action<Projectile>(this.PostProcessSpawnedEnemyProjectiles));
            }
        }
        private void PostProcessSpawnedEnemyProjectiles(Projectile proj)
        {
            if (TintBullets) { proj.AdjustPlayerProjectileTint(this.TintColor, 1); }
            if (enemy != null)
            {
                if (enemy.aiActor != null)
                {
                    proj.collidesWithPlayer = false;
                    
                    proj.UpdateCollisionMask();
                    proj.baseData.damage = baseBulletDamage;
                    if (enemyOwner != null)
                    {
                        if (scaleDamage) proj.baseData.damage *= enemyOwner.stats.GetStatValue(PlayerStats.StatType.Damage);
                        if (scaleSize)
                        {
                            proj.RuntimeUpdateScale(enemyOwner.stats.GetStatValue(PlayerStats.StatType.PlayerBulletScale));
                        }
                        if (scaleSpeed)
                        {
                            proj.baseData.speed *= enemyOwner.stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed);
                            proj.UpdateSpeed();
                        }
                        if (doPostProcess) enemyOwner.DoPostProcessProjectile(proj);
                    }
                    if (enemy.aiActor.IsBlackPhantom) { proj.baseData.damage = baseBulletDamage * jammedDamageMultiplier; }
                }
            }
            else { ETGModConsole.Log("Shooter is NULL"); }
        }

        private AIActor enemy;
        public PlayerController enemyOwner;
        public bool scaleDamage;
        public bool scaleSize;
        public bool scaleSpeed;
        public bool doPostProcess;
        public float baseBulletDamage;
        public float jammedDamageMultiplier;
        public bool TintBullets;
        public Color TintColor;
        

    }
}

