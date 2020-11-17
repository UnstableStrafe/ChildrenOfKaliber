using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using ItemAPI;
using UnityEngine;
namespace Items
{
    class ReloadedRifle : MultiActiveReloadController
    {
        public static void Add()
        {
            string shorthandName = "reloaded_rifle";
            Gun gun = ETGMod.Databases.Items.NewGun("Reloaded Rifle", shorthandName);
            Game.Items.Rename("outdated_gun_mods:"+ shorthandName, "cel:"+shorthandName);
            var behav = gun.gameObject.AddComponent<ReloadedRifle>();
            gun.SetShortDescription("Of The Highest Quality");
            gun.SetLongDescription("Has multiple active reloads with different effects. If the first active reload is performed with an empty clip, the empty clip is thrown, stunning hit enemies.\n\nNothing is more satisfying, more sweet to a gunslinger than a crisp, fluid reload.");
            gun.SetupSprite(null, shorthandName+"_idle_001", 6);
            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.AddProjectileModuleFrom("ak-47", true, false);
            gun.SetBaseMaxAmmo(500);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.4f;
            gun.DefaultModule.cooldownTime = 0.11f;
            gun.gunClass = GunClass.RIFLE;
            gun.DefaultModule.numberOfShotsInClip = 25;
            gun.quality = PickupObject.ItemQuality.S;
            Gun gun2 = PickupObjectDatabase.GetById(15) as Gun;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.gunSwitchGroup = gun2.gunSwitchGroup;
            gun.gunHandedness = GunHandedness.TwoHanded;
            gun.barrelOffset.transform.localPosition = new Vector3(2.5f, .5f, 0f);
            gun.DefaultModule.angleVariance = 4f;
            gun.encounterTrackable.EncounterGuid = "r/HighQualityReloads";
            gun.sprite.IsPerpendicular = true;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 1f;
            projectile.baseData.speed *= 1f;
            projectile.baseData.force *= 1f;            
            projectile.transform.parent = gun.barrelOffset;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            behav.activeReloadEnabled = true;
            behav.reloads = new List<MultiActiveReloadData>
            {
                new MultiActiveReloadData(0.675f, 65, 69, 1, 1 * 3, false, true, new ActiveReloadData
                {
                    damageMultiply = 2f,
                    
                }, true, "DamageUp"),
                new MultiActiveReloadData(.5f, 48, 52, 1, 1 * 3, false, true, new ActiveReloadData
                {
                    damageMultiply = 1,

                }, true, "AccuracyUp"),
                new MultiActiveReloadData(.85f, 83, 87, 1, 1 * 3, true, true, new ActiveReloadData
                {
                    damageMultiply = 1,

                }, true, "FireRateUp")
            };          
        }

        private bool HasReloaded;
        private bool AccReload = false, FireReload = false;
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
        public override void OnReloadEndedSafe(PlayerController player, Gun gun)
        {
            base.OnReloadEnded(player, gun);
            
            if (AccReload == true)
            {
                AccReload = false;
            }
            
            if(FireReload == true)
            {
                FireReload = false;
            }
            player.stats.RecalculateStats(player, false, false);
        }
        public override void PostProcessVolley(ProjectileVolleyData volley)
        {
            base.PostProcessVolley(volley);
        }
        public override void OnActiveReloadSuccess(MultiActiveReload reload)
        {
            base.OnActiveReloadSuccess(reload);
            PlayerController player = gun.CurrentOwner as PlayerController;
            if (reload.Name == "AccuracyUp" && AccReload == false)
            {
                AccReload = true;
                this.AddCurrentGunStatModifier(PlayerStats.StatType.Accuracy, .25f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                if(gun.ClipShotsRemaining == 0)
                {
                    Projectile projectile = ((PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0]);
                    GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
                    Projectile component = gameObject.GetComponent<Projectile>();
                    if (component != null)
                    {
                        component.Owner = gun.CurrentOwner;
                        component.Shooter = gun.CurrentOwner.specRigidbody;
                        component.baseData.speed *= 1f;
                        component.baseData.damage = 7f;
                        component.AppliesStun = true;
                        component.StunApplyChance = 1f;
                        component.AppliedStunDuration = 2f;
                    }
                }
            }
            else
            {
                if (reload.Name == "FireRateUp" && FireReload == false)
                {
                    FireReload = true;
                    this.AddCurrentGunStatModifier(PlayerStats.StatType.RateOfFire, 1.65f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                }
            }
            player.stats.RecalculateStats(player, false, false);
        }

        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && this.HasReloaded)
            {
                HasReloaded = false;
                AccReload = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                this.RemoveCurrentGunStatModifier(PlayerStats.StatType.RateOfFire);
                player.stats.RecalculateStats(player, false, false);
                this.RemoveCurrentGunStatModifier(PlayerStats.StatType.Accuracy);
                player.stats.RecalculateStats(player, false, false);
            }
        }

        
        public ReloadedRifle()
        {
        }
        public void AddCurrentGunStatModifier(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod modifyMethod)
        {
            gun.currentGunStatModifiers = gun.currentGunStatModifiers.Concat(new StatModifier[] { new StatModifier { statToBoost = statType, amount = amount, modifyType = modifyMethod } }).ToArray();
        }

        public void RemoveCurrentGunStatModifier(PlayerStats.StatType statType)
        {
            List<StatModifier> list = new List<StatModifier>();
            foreach (StatModifier mod in gun.currentGunStatModifiers)
            {
                if (mod.statToBoost != statType)
                {
                    list.Add(mod);
                }
            }
            gun.currentGunStatModifiers = list.ToArray();
        }
    }
}
