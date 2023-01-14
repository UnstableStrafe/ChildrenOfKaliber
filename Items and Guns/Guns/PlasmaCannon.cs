using System.Collections;
using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;
namespace Items
{
    class PlasmaCannon : AdvancedGunBehaviour
    {
        public static void Add()
        {

            Gun gun = ETGMod.Databases.Items.NewGun("Plasma Cannon", "plasma_cannon");
            Game.Items.Rename("outdated_gun_mods:plasma_cannon", "cel:plasma_cannon");
            gun.gameObject.AddComponent<PlasmaCannon>();
            gun.SetShortDescription("Spelunk'd");
            gun.SetLongDescription("Shoots extremely powerful explosions. Use with extreme caution.\n\nIt was built by aliens from Earth's moon, but sadly the recoil proved to be too strong for them to use it easily.");
            gun.SetupSprite(null, "plasma_cannon_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 18);
            gun.SetAnimationFPS(gun.reloadAnimation, 6);
            gun.AddProjectileModuleFrom("38_special");
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 2f;
            gun.DefaultModule.angleVariance = 0f;
            gun.DefaultModule.cooldownTime = .2f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.InfiniteAmmo = true;
            gun.DefaultModule.ammoType = (PickupObjectDatabase.GetById(16) as Gun).DefaultModule.ammoType;
            Gun gun2 = PickupObjectDatabase.GetById(32) as Gun;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.SetBaseMaxAmmo(150);
            gun.barrelOffset.transform.localPosition = new Vector3(1.2f, .3f);
            gun.quality = PickupObject.ItemQuality.B;
            gun.encounterTrackable.EncounterGuid = "Good Luck Kiddo.";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.EXPLOSIVE;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage *= 3;
            projectile.baseData.range *= 10000;
            projectile.baseData.speed *= 4;
            projectile.shouldRotate = true;
            projectile.AppliesKnockbackToPlayer = true;
            projectile.PlayerKnockbackForce = 200;
            Gun gun3 = PickupObjectDatabase.GetById(32) as Gun;
            gun.gunSwitchGroup = gun3.gunSwitchGroup;
            projectile.SetProjectileSpriteRight("plasma_shot", 32, 20);
            AIActor Firecracker = EnemyDatabase.GetOrLoadByGuid("4d37ce3d666b4ddda8039929225b7ede");
            ExplosiveModifier Spelunked = projectile.gameObject.AddComponent<ExplosiveModifier>();

            ExplosionData YourAreDecease = new ExplosionData
            {
                damageRadius = 5f,
                damageToPlayer = 0.5f,
                doDamage = true,
                damage = 35f,
                doExplosionRing = true,
                doDestroyProjectiles = true,
                doForce = true,
                debrisForce = 100f,
                pushRadius = 7f,
                force = 50f,
                preventPlayerForce = false,
                explosionDelay = 0f,
                usesComprehensiveDelay = false,
                doScreenShake = false,
                playDefaultSFX = true,
                effect = Firecracker.GetComponent<ExplodeOnDeath>().explosionData.effect,
                forceUseThisRadius = true,
                //AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
                //  GameObject TestingVFX = assetBundle.LoadAsset<GameObject>("VFX_Dust_Explosion");
            };
            Spelunked.explosionData = YourAreDecease;
            Spelunked.IgnoreQueues = true;

            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());

            gun.AddToSubShop(ItemBuilder.ShopType.Trorc);

        }



        private bool HasReloaded;

        protected override void Update()
        {
            if (gun.CurrentOwner)
            {

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
            gun.PreventNormalFireAudio = false;

        }



        public PlasmaCannon()
        {

        }
    }
}
