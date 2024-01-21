using Alexandria.ItemAPI;
using Gungeon;
using UnityEngine;

namespace Items
{
    class AK188 : AdvancedGunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("AK-188", "ak_188");
            Game.Items.Rename("outdated_gun_mods:ak188", "ck:ak_188");
            gun.gameObject.AddComponent<AK188>();
            gun.SetShortDescription("No God Can Help You");
            gun.SetLongDescription("Good luck, kid.");
            gun.SetupSprite(null, "ak_188_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 9);
            gun.SetAnimationFPS(gun.reloadAnimation, 10);
            gun.AddProjectileModuleFrom("ak-47");
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SMALL_BULLET;
            gun.DefaultModule.ammoCost = 4;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 2f;
            gun.CanBeDropped = false;
            gun.CanBeSold = false;
            gun.DefaultModule.angleVariance = 4f;
            gun.DefaultModule.cooldownTime = .06f;
            gun.DefaultModule.numberOfShotsInClip = 120;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(15) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(15) as Gun).muzzleFlashEffects;
            gun.SetBaseMaxAmmo(2000);
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            gun.encounterTrackable.EncounterGuid = "what the actual fuck";
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
            AK188.AK188ID = gun.PickupObjectId;
            gun.SetTag("kalashnikov");
        }
        public static int AK188ID;
        private bool HasReloaded;
        private void OnDestroy()
        {
            c_playerController.OnKilledEnemy -= Transforming;
        }
        private PlayerController c_playerController;
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
        private float Tracker = 0;
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
                if (Tracker >= 55)
                {
                    Gun ak188 = PickupObjectDatabase.GetById(AK188.AK188ID) as Gun;
                    Gun akinfinity = PickupObjectDatabase.GetById(InfiniteAK.AKINFID) as Gun;
                    player.OnKilledEnemy -= Transforming;
                    player.inventory.AddGunToInventory(akinfinity, true);
                    player.inventory.DestroyGun(ak188);
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
            for (int i = 0; i < 3; i++)
            {
                float v1 = UnityEngine.Random.Range(-4f, 4f);
                Projectile projectile2 = ((Gun)ETGMod.Databases.Items[15]).DefaultModule.projectiles[0];
                GameObject gameObject2 = SpawnManager.SpawnProjectile(projectile2.gameObject, base.Owner.CurrentGun.transform.position, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle + (i * 90) + v1 + 90), true);
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
        }

        public AK188()
        {

        }
    }
}
