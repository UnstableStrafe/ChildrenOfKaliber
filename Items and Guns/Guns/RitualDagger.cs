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
    class RitualDagger : AdvancedGunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Ritual Dagger", "ritual_dagger");
            Game.Items.Rename("outdated_gun_mods:ritual_dagger", "cel:ritual_dagger");
            gun.gameObject.AddComponent<RitualDagger>();
            gun.SetShortDescription("Blood For The Bird God");
            gun.SetLongDescription("Gains a small, permanent damage upgrade for each enemy killed with it.\n\nAn ornate ritual dagger that is used to elevate the status of members of am avian cult.");
            gun.SetupSprite(null, "ritual_dagger_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 16);
            gun.SetAnimationFPS(gun.reloadAnimation, 2);
            gun.AddProjectileModuleFrom("38_special");
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "white";
            gun.reloadTime = 0;
            gun.DefaultModule.angleVariance = 0;
            gun.DefaultModule.cooldownTime = .5f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.SetBaseMaxAmmo(1);
            gun.ammo = 1;
            gun.InfiniteAmmo = true;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.quality = PickupObject.ItemQuality.A;
            gun.encounterTrackable.EncounterGuid = "Ritual Dagger is a Colorless Card that can only be obtained from The Nest Event. Killing an enemy with Ritual Dagger will increase the card's damage by 3(5) for the rest of the run. Using it will exhaust it, whether it kills an enemy or not. Some enemies do not count for kills to increase Ritual Dagger's damage, listed below under Interactions.";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.NONE;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(417) as Gun).gunSwitchGroup;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 3;
            projectile.baseData.speed *= 1f;
            projectile.baseData.force *= 2f;
            projectile.baseData.range *= 1;

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
            player.OnKilledEnemyContext += OnKill;
            cached_player = player;
        }

        private void OnKill(PlayerController player, HealthHaver idiot)
        {
            bool checkIfHeld = player.CurrentGun == this || player.CurrentSecondaryGun == this;
            if (!idiot.aiActor.IsHarmlessEnemy && idiot.aiActor.IsWorthShootingAt && !idiot.aiActor.IgnoreForRoomClear && checkIfHeld)
            {
                storedDamage += 5;
            }
        }

        protected override void OnPostDroppedByPlayer(PlayerController player)
        {
            base.OnPostDroppedByPlayer(player);
            player.OnKilledEnemyContext -= OnKill;
            cached_player = null;
        }

        private void OnDestroy()
        {
            if(cached_player != null)
            {
                cached_player.OnKilledEnemyContext -= OnKill;
                cached_player = null;
            }
        }

        public RitualDagger()
        {

        }
        
        private PlayerController cached_player = null;

        [SerializeField]
        public float storedDamage = 0;

    }

}
