using Alexandria.ItemAPI;
using UnityEngine;
using Gungeon;

namespace Items
{
    class PrimeCannon : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Prime Cannon", "prime_cannon");
            Game.Items.Rename("outdated_gun_mods:prime_cannon", "cel:prime_cannon");
            gun.gameObject.AddComponent<PrimeCannon>();
            gun.SetShortDescription("Leedle");
            gun.SetLongDescription("u-u");
            gun.SetupSprite(null, "prime_cannon_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 4);
            gun.SetAnimationFPS(gun.reloadAnimation, 2);
            gun.AddProjectileModuleFrom("38_special");
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SMALL_BULLET;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0f;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(19) as Gun).gunSwitchGroup;
            gun.DefaultModule.angleVariance = 0f;
            gun.DefaultModule.cooldownTime = 1f;
            gun.DefaultModule.numberOfShotsInClip = 150;
            Gun gun2 = PickupObjectDatabase.GetById(15) as Gun;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.SetBaseMaxAmmo(150);
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            gun.encounterTrackable.EncounterGuid = "For what purpose are you reading this????? why would you?? creep.";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.RIFLE;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            gun.barrelOffset.transform.localPosition -= new Vector3(0 , .4f);
            projectile.transform.parent = gun.barrelOffset;
            AIActor Grenat = EnemyDatabase.GetOrLoadByGuid("b4666cb6ef4f4b038ba8924fd8adf38f");
            ExplosiveModifier GetFucked = projectile.gameObject.AddComponent<ExplosiveModifier>();
            ExplosionData die = new ExplosionData
            {
                damageRadius = 1.5f,
                damageToPlayer = 0f,
                doDamage = true,
                damage = 20f,
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
                effect = Grenat.GetComponent<ExplodeOnDeath>().explosionData.effect
            };

            GetFucked.explosionData = die;
            projectile.SetProjectileSpriteRight("prime_bomb", 9, 9);
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
        }

        private bool HasReloaded;

        public override void Update()
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
            }
        }


        public override void OnPostFired(PlayerController player, Gun gun)
        {

        }



        public PrimeCannon()
        {

        }
    }
}
