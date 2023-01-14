using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Alexandria.ItemAPI;
using Gungeon;
using Dungeonator;
using UnityEngine;
using System.Collections.ObjectModel;

namespace Items
{
    class Vacuum : AdvancedGunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Vacuum", "vacuum");
            Game.Items.Rename("outdated_gun_mods:vacuum", "cel:vacuum");
            gun.gameObject.AddComponent<Vacuum>();
            gun.SetShortDescription("Bullet's Mansion");
            gun.SetLongDescription("Sucks enemies towards the player. Weakened enemies that get too close are sucked up into the vacuum chamber.\n\nThis hardware store vacuum has been outfitted with a \"Hyper\" mode that can suck up gundead with the same power as dust bunnies.");
            gun.SetupSprite(null, "vacuum_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 4);
            gun.SetAnimationFPS(gun.reloadAnimation, 2);
            for (int i = 0; i < 3; i++)
            {
                GunExt.AddProjectileModuleFrom(gun, "ak-47");
            }
            foreach (ProjectileModule projectileModule in gun.Volley.projectiles)
            {
                projectileModule.ammoType = GameUIAmmoType.AmmoType.SMALL_BULLET;
                projectileModule.ammoCost = 1;
                projectileModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
                projectileModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                projectileModule.angleVariance = 35f;
                projectileModule.cooldownTime = .025f;
                projectileModule.numberOfShotsInClip = 240;
                Projectile projectile = Library.CopyFields<RandomMidAirTachyon>(UnityEngine.Object.Instantiate(projectileModule.projectiles[0]));
                projectile.gameObject.SetActive(false);
                projectileModule.projectiles[0] = projectile;
                FakePrefab.MarkAsFakePrefab(projectile.gameObject);
                UnityEngine.Object.DontDestroyOnLoad(projectile);
                projectile.transform.parent = gun.barrelOffset;
                projectile.baseData.damage *= .15f;
                projectile.baseData.speed *= .75f;
                projectile.baseData.force *= 1f;
                projectile.baseData.range *= .7f;
                projectile.AdditionalScaleMultiplier = .5f;
                projectile.hitEffects.suppressMidairDeathVfx = true;
                projectile.hitEffects.HasProjectileDeathVFX = true;
                projectile.shouldRotate = true;
                gun.PreventNormalFireAudio = true;
                ProjectileSpeedChange speedMod = projectile.gameObject.AddComponent<ProjectileSpeedChange>();
                speedMod.incrementRate = .10f;

                bool flag = projectileModule == gun.DefaultModule;
                if (flag)
                {
                    projectileModule.ammoCost = 1;
                }
                else
                {
                    projectileModule.ammoCost = 0;
                }
            }            
            gun.reloadTime = 1.5f;
           
            //gun.muzzleFlashEffects = VFXLibrary.CreateMuzzleflash("succ", new List<string> { "lebigsucc_001", "lebigsucc_002", "lebigsucc_003", "lebigsucc_004" }, 10, new List<IntVector2> { new IntVector2(27,20), new IntVector2(27, 20), new IntVector2(27, 20), new IntVector2(27, 20), }, new List<tk2dBaseSprite.Anchor> {
            //    tk2dBaseSprite.Anchor.MiddleLeft, tk2dBaseSprite.Anchor.MiddleLeft, tk2dBaseSprite.Anchor.MiddleLeft, tk2dBaseSprite.Anchor.MiddleLeft}, new List<Vector2> { Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, }, false, false, false, false, 0, VFXAlignment.Fixed, true, new List<float> { 0, 0, 0, 0 }, new List<Color> { VFXLibrary.emptyColor, VFXLibrary.emptyColor, VFXLibrary.emptyColor, VFXLibrary.emptyColor, });
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.SetBaseMaxAmmo(1200);
            gun.ammo = 1200;
            gun.barrelOffset.transform.localPosition = new Vector3(1.75f, .075f, 0f);
            gun.quality = PickupObject.ItemQuality.D;
            gun.encounterTrackable.EncounterGuid = "vrooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooom";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.SILLY;
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());

        }

        private bool HasReloaded;

        protected override void Update()
        {
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
                AkSoundEngine.PostEvent("Play_WPN_dl45heavylaser_reload", base.gameObject);
            }
        }
        private void ProcessEnemy(AIActor actor, float distance)
        {
            if(actor.healthHaver && actor.healthHaver.GetCurrentHealthPercentage() <= .1f && !actor.healthHaver.IsBoss)
            {
                GameManager.Instance.Dungeon.StartCoroutine(this.HandleEnemySuck(actor.gameObject));
                actor.healthHaver.Die(Vector2.zero);
                actor.EraseFromExistenceWithRewards();
            }
            
        }
        private IEnumerator HandleEnemySuck(GameObject target)
        {
            Transform copySprite = this.CreateEmptySprite(target.gameObject);
            
            Vector3 startPosition = copySprite.transform.position;
            float elapsed = 0f;
            float duration = 0.5f;
            while (elapsed < duration)
            {
                elapsed += BraveTime.DeltaTime;
                if (Owner.CurrentGun && copySprite)
                {
                    Vector3 position = Owner.CurrentGun.PrimaryHandAttachPoint.position;
                    float t = elapsed / duration * (elapsed / duration);
                    copySprite.position = Vector3.Lerp(startPosition, position, t);
                    copySprite.rotation = Quaternion.Euler(0f, 0f, 360f * BraveTime.DeltaTime) * copySprite.rotation;
                    copySprite.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.1f, 0.1f, 0.1f), t);
                }
                yield return null;
            }
            if (copySprite)
            {
                UnityEngine.Object.Destroy(copySprite.gameObject);
            }

            yield break;
        }
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
            player.CurrentRoom.ApplyActionToNearbyEnemies(player.CenterPosition, 5.5f, new Action<AIActor, float>(this.ProcessEnemy));
            

        }
        private void DoBulletProcess(Projectile proj, PlayerController player)
        {
            GameManager.Instance.StartCoroutine(HandleEnemySuck(proj.gameObject));
            Destroy(proj.gameObject);
            if(player.CurrentGun == gun)
            {
                player.CurrentGun.ammo++;
            }
        }
        private Transform CreateEmptySprite(GameObject target)
        {
            if(target.GetComponent<AIActor>() != null)
            {
                AIActor actor = target.GetComponent<AIActor>();
                GameObject gameObject = new GameObject("suck image");
                gameObject.layer = actor.gameObject.layer;
                tk2dSprite tk2dSprite = gameObject.AddComponent<tk2dSprite>();
                gameObject.transform.parent = SpawnManager.Instance.VFX;
                tk2dSprite.SetSprite(actor.sprite.Collection, actor.sprite.spriteId);
                tk2dSprite.transform.position = actor.sprite.transform.position;
                GameObject gameObject2 = new GameObject("image parent");
                gameObject2.transform.position = tk2dSprite.WorldCenter;
                tk2dSprite.transform.parent = gameObject2.transform;
                if (actor.optionalPalette != null)
                {
                    tk2dSprite.renderer.material.SetTexture("_PaletteTex", actor.optionalPalette);
                }
                return gameObject2.transform;
            }
            else if(target.GetComponent<Projectile>() != null)
            {
                Projectile actor = target.GetComponent<Projectile>();
                GameObject gameObject = new GameObject("suck image");
                gameObject.layer = actor.gameObject.layer;
                tk2dSprite tk2dSprite = gameObject.AddComponent<tk2dSprite>();
                gameObject.transform.parent = SpawnManager.Instance.VFX;
                tk2dSprite.SetSprite(actor.sprite.Collection, actor.sprite.spriteId);
                tk2dSprite.transform.position = actor.sprite.transform.position;
                GameObject gameObject2 = new GameObject("image parent");
                gameObject2.transform.position = tk2dSprite.WorldCenter;
                tk2dSprite.transform.parent = gameObject2.transform;
                return gameObject2.transform;
            }
            else
            {
                return null;
            }
        }
        protected override void OnPickup(GameActor owner)
        {
            base.OnPickup(owner);
            shouldDoBulletSucc = false;
            if(owner.GetComponent<PlayerController>() != null)
            {
                PlayerController player = owner.GetComponent<PlayerController>();             
            }
        }
        protected override void OnPostDroppedByPlayer(PlayerController player)
        {
            base.OnPostDroppedByPlayer(player);
            
        }
        
       
        public Vacuum()
        {

        }
        private bool shouldDoBulletSucc = false;
    }
    
    
}
