using Gungeon;
using UnityEngine;
using Alexandria.ItemAPI;
namespace Items
{
    class AK94 : AdvancedGunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("AK-94", "ak_94");
            Game.Items.Rename("outdated_gun_mods:ak94", "ck:ak_94");
            gun.gameObject.AddComponent<AK94>();
            gun.SetShortDescription("Accept No SuuS oN tpeccA");
            gun.SetLongDescription("Some idiot decided to create this affront against God by taping two AK-47's together.");
            gun.SetupSprite(null, "ak_94_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 9);
            gun.SetAnimationFPS(gun.reloadAnimation, 10);
            gun.AddProjectileModuleFrom("ak-47");
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SMALL_BULLET;
            gun.DefaultModule.ammoCost = 2;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.angleVariance = 4;
            gun.DefaultModule.cooldownTime = .11f;
            gun.DefaultModule.numberOfShotsInClip = 60;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(15) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(15) as Gun).muzzleFlashEffects;
            gun.SetBaseMaxAmmo(1000);
            gun.quality = PickupObject.ItemQuality.B;
            gun.encounterTrackable.EncounterGuid = "reverse, reverse";
            gun.sprite.IsPerpendicular = true;
            gun.barrelOffset.transform.localPosition = new Vector3(2.25f, 0.3125f, 0f);
            gun.gunClass = GunClass.FULLAUTO;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage *= 1f;
            projectile.baseData.speed *= 1f;
            projectile.baseData.force *= 1f;


            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
            AK94.AK94ID = gun.PickupObjectId;
            gun.SetTag("kalashnikov");
        }
        public static int AK94ID;

        private bool HasReloaded;
        private float Tracker = 0;
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

        private void OnDestroy()
        {
            c_playerController.OnKilledEnemy -= this.Transforming;
        }
        private PlayerController c_playerController;
        protected override void OnPickup(GameActor owner)
        {
            base.OnPickup(owner);
            (owner as PlayerController).OnKilledEnemy += this.Transforming;
            c_playerController = owner as PlayerController;
        }
       
        protected override void OnPostDrop(GameActor owner)
        {
            base.OnPostDrop(owner);
            (owner as PlayerController).OnKilledEnemy -= this.Transforming;
        }

        private void Transforming(PlayerController player)
        {
            if (player != null)
            {
                this.Tracker++;
                if (Tracker >= 30)
                {
                    Gun ak94 = PickupObjectDatabase.GetById(AK94.AK94ID) as Gun;
                    Gun ak141 = PickupObjectDatabase.GetById(AK141.AK141ID) as Gun;
                    player.OnKilledEnemy -= this.Transforming;
                    player.inventory.AddGunToInventory(ak141, true);
                    player.inventory.DestroyGun(ak94);
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

        private float revAngle = 180;
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            base.OnPostFired(player, gun);
            float v1 = UnityEngine.Random.Range(-4f, 4f);
            Projectile projectile2 = ((Gun)ETGMod.Databases.Items[15]).DefaultModule.projectiles[0];
            GameObject gameObject2 = SpawnManager.SpawnProjectile(projectile2.gameObject, base.Owner.CurrentGun.transform.position, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle + revAngle + v1), true);
            Projectile component2 = gameObject2.GetComponent<Projectile>();
            if (component2 != null)
            {
                component2.Owner = player;
                component2.Shooter = player.specRigidbody;
                component2.baseData.speed *= player.stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed);
                component2.baseData.force *= player.stats.GetStatValue(PlayerStats.StatType.KnockbackMultiplier);
                component2.baseData.damage *= player.stats.GetStatValue(PlayerStats.StatType.Damage);
                player.DoPostProcessProjectile(component2);
            }

        }

        public AK94()
        {

        }
    }
}
