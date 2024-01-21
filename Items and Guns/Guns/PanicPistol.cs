using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;
using Dungeonator;

namespace Items
{
    class PanicPistol : AdvancedGunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Panic Pistol", "panic_pistol");
            Game.Items.Rename("outdated_gun_mods:panic_pistol", "ck:panic_pistol");
            gun.gameObject.AddComponent<PanicPistol>();
            gun.SetShortDescription("Random BS, Go!");
            gun.SetLongDescription("Reload speed is increased by 10% for each enemy in the room.\n\nThe Pilot is never far from one of his sidearms as he is also never far from getting into a fight.");

            gun.SetupSprite(null, "panic_pistol_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 10);
            gun.SetAnimationFPS(gun.reloadAnimation, 2);
            gun.AddProjectileModuleFrom("38_special");
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.MEDIUM_BLASTER;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.5f;
            gun.DefaultModule.angleVariance = 6f;
            gun.DefaultModule.cooldownTime = .15f;
            gun.DefaultModule.numberOfShotsInClip = 8;
            Gun gun3 = PickupObjectDatabase.GetById(32) as Gun;
            gun.gunSwitchGroup = gun3.gunSwitchGroup;
            gun.muzzleFlashEffects = gun3.muzzleFlashEffects;
            gun.SetBaseMaxAmmo(480);
            gun.quality = PickupObject.ItemQuality.C;
            gun.encounterTrackable.EncounterGuid = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA AAAAAAAAAAAAAAAAAAAaAAaaaAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAaAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA.";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.PISTOL;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
           
            projectile.baseData.damage *= 1;
            projectile.baseData.speed *= 1f;
            projectile.baseData.force *= 1f;
            projectile.baseData.range *= .75f;
            projectile.SetProjectileSpriteRight("panic_bolt", 9, 5);
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
        }

        private bool HasReloaded;
        private int enemies = 0, lastEnemies = -1;


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
                HandleStats();
            }
        }
        private void HandleStats()
        {
            PlayerController player = gun.CurrentOwner as PlayerController;
            enemies = player.CurrentRoom.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.RoomClear);
            if (enemies == lastEnemies || enemies == 0) return;
            float boost = 1 - (enemies * .1f);
            gun.RemoveCurrentGunStatModifier(PlayerStats.StatType.ReloadSpeed);
            player.stats.RecalculateStats(player, false, false);
            gun.AddCurrentGunStatModifier(PlayerStats.StatType.ReloadSpeed, boost, StatModifier.ModifyMethod.MULTIPLICATIVE);
            player.stats.RecalculateStats(player, false, false);
            lastEnemies = enemies;
        }
        public override void OnSwitchedAwayFromThisGun()
        {
            base.OnSwitchedAwayFromThisGun();
            PlayerController player = gun.CurrentOwner as PlayerController;            
            gun.RemoveCurrentGunStatModifier(PlayerStats.StatType.ReloadSpeed);
            player.stats.RecalculateStats(player, false, false);
        }
        protected override void OnPostDroppedByPlayer(PlayerController player)
        {
            base.OnPostDroppedByPlayer(player);
            gun.RemoveCurrentGunStatModifier(PlayerStats.StatType.ReloadSpeed);
            player.stats.RecalculateStats(player, false, false);
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


        public override void OnPostFired(PlayerController player, Gun gun)
        {

            gun.PreventNormalFireAudio = false;
        }



        public PanicPistol()
        {

        }
    }
}
