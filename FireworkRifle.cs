using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using Gungeon;
using UnityEngine;

namespace Items
{
    class FireworkRifle : GunBehaviour
    {
        public static void Add()
        {
            
            Gun gun = ETGMod.Databases.Items.NewGun("Firework Rifle", "firework_rifle");
            Game.Items.Rename("outdated_gun_mods:firework_rifle", "cel:firework_rifle");
            gun.gameObject.AddComponent<FireworkRifle>();
            gun.SetShortDescription("Bang, Bang, Bang! Here We Go!");
            gun.SetLongDescription("Shoots powerful fireworks.\n\nThis gun came from a world caught in an eternal struggle for balance. It was created to celebrate the journey of a hero.");

            gun.SetupSprite(null, "firework_rifle_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 18);
            gun.SetAnimationFPS(gun.reloadAnimation, 2);
            gun.AddProjectileModuleFrom("ak-47", true, false);

            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = .8f;
            gun.DefaultModule.angleVariance = 4f;
            gun.DefaultModule.cooldownTime = .2f;
            gun.DefaultModule.numberOfShotsInClip = 12;
            gun.DefaultModule.ammoType = (PickupObjectDatabase.GetById(16) as Gun).DefaultModule.ammoType;
            Gun gun2 = PickupObjectDatabase.GetById(32) as Gun;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.SetBaseMaxAmmo(150);
            gun.barrelOffset.transform.localPosition = new Vector3(2, .3f);
            gun.quality = PickupObject.ItemQuality.S;
            gun.encounterTrackable.EncounterGuid = "pewpewgoboomboomdeath.";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.EXPLOSIVE;

            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.shouldRotate = true;
            projectile.SetProjectileSpriteRight("firework_proj", 13, 7, null, null);
            AIActor Firecracker = EnemyDatabase.GetOrLoadByGuid("5f15093e6f684f4fb09d3e7e697216b4");
            ExplosiveModifier GetFucked = projectile.gameObject.AddComponent<ExplosiveModifier>();

            ExplosionData die = new ExplosionData
            {
                damageRadius = 1.5f,
                damageToPlayer = 0f,
                doDamage = true,
                damage = 15f,
                doExplosionRing = true,
                doDestroyProjectiles = true,
                doForce = true,
                debrisForce = 5f,
                pushRadius = 1.6f,
                force = 8f,
                preventPlayerForce = true,
                explosionDelay = 0f,
                usesComprehensiveDelay = false,
                doScreenShake = false,
                playDefaultSFX = true,
                effect = Firecracker.GetComponent<ExplodeOnDeath>().explosionData.effect,
                //AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
                //  GameObject TestingVFX = assetBundle.LoadAsset<GameObject>("VFX_Dust_Explosion");
        };

            GetFucked.explosionData = die;
            
            
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            gun.AddToSubShop(ItemBuilder.ShopType.Trorc);
            
        }
        

        private static GameObject Death;
        private bool HasReloaded;

        protected void Update()
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
                AkSoundEngine.PostEvent("Play_WPN_rpg_reload_01", base.gameObject);
            }
        }


        public override void OnPostFired(PlayerController player, Gun gun)
        {

            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_energycannon_shot_01", gameObject);
        }



        public FireworkRifle()
        {

        }
    }
}
