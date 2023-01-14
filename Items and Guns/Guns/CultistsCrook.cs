using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;


namespace Items
{
    class CultistsCrook : AdvancedGunBehaviour
    {
        public static void Add()
        {

            Gun gun = ETGMod.Databases.Items.NewGun("Cultist's Crook", "cultist's_crook");
            Game.Items.Rename("outdated_gun_mods:cultist's_crook", "cel:cultist's_crook");
            gun.gameObject.AddComponent<CultistsCrook>();
            gun.SetShortDescription("CA-CAW!");
            gun.SetLongDescription("Gains more damage the more rooms that have been cleared whilst carrying it. Damage bonus is reset on going to a new floor.\n\nCA-CAW CA-CAW!");
            gun.SetupSprite(null, "cultist's_crook_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 16);
            gun.SetAnimationFPS(gun.reloadAnimation, 2);
            gun.AddProjectileModuleFrom("38_special");
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "wand";
            gun.reloadTime = 0;
            gun.DefaultModule.angleVariance = 0;
            gun.DefaultModule.cooldownTime = .8f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.SetBaseMaxAmmo(1);
            gun.ammo = 1;
            gun.InfiniteAmmo = true;
            gun.quality = PickupObject.ItemQuality.C;
            gun.encounterTrackable.EncounterGuid = "CA-CAW!!!!!!!!!!!!!!";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.NONE;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(417) as Gun).gunSwitchGroup;
            gun.barrelOffset.transform.localPosition += new Vector3(-0.1f, 0, 0);
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 1.2f;
            projectile.baseData.speed *= 1f;
            projectile.baseData.force *= 2f;
            projectile.baseData.range *= 1;
            ProjectileSlashingBehaviour slashBehav = projectile.gameObject.AddComponent<ProjectileSlashingBehaviour>();
            slashBehav.SlashDamageUsesBaseProjectileDamage = true;
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
        }

        public override void PostProcessProjectile(Projectile projectile)
        {
            base.PostProcessProjectile(projectile);
            projectile.baseData.damage += storedDamage;
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            base.OnPostFired(player, gun);
            PlayerController x = this.gun.CurrentOwner as PlayerController;
            bool flag = x == null;
            bool flag2 = flag;
            if (flag2)
            {
                this.gun.ammo = this.gun.GetBaseMaxAmmo();
            }
            this.gun.ClipShotsRemaining = 2;
            this.gun.GainAmmo(2);
            gun.ClearReloadData();

        }

        protected override void OnPickedUpByPlayer(PlayerController player)
        {
            base.OnPickedUpByPlayer(player);
            player.OnRoomClearEvent += OnRoomClear;
            GameManager.Instance.OnNewLevelFullyLoaded += ClearBoostOnNewLevel;
            cached_player = player;
        }

        private void ClearBoostOnNewLevel()
        {
            storedDamage = 0;
        }

        private void OnRoomClear(PlayerController obj)
        {
            storedDamage += 2.5f;
        }

        protected override void OnPostDroppedByPlayer(PlayerController player)
        {
            base.OnPostDroppedByPlayer(player);
            player.OnRoomClearEvent -= OnRoomClear;
            GameManager.Instance.OnNewLevelFullyLoaded -= ClearBoostOnNewLevel;
            cached_player = null;

        }

        private void OnDestroy()
        {
            if(cached_player != null)
            {
                cached_player.OnRoomClearEvent -= OnRoomClear;
                GameManager.Instance.OnNewLevelFullyLoaded -= ClearBoostOnNewLevel;
                cached_player = null;
            }
        }

        public CultistsCrook()
        {

        }
        private PlayerController cached_player = null;

        [SerializeField]
        private float storedDamage = 0;

    }
   
}
