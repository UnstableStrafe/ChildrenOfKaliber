
using Gungeon;
using ItemAPI;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

namespace Items
{
    class Incremental : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Incremental", "incremental");
            Game.Items.Rename("outdated_gun_mods:incremental", "cel:incremental");
            gun.gameObject.AddComponent<Incremental>();
            gun.SetShortDescription("Start Again");
            gun.SetLongDescription("Damage increases the further you are into your clip.\n\nMade by an explorer who liked keeping track of his magazine capacity in style. Ticking can be heard when the gun is idle.");

            gun.SetupSprite(null, "incremental_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 24);
            gun.SetAnimationFPS(gun.reloadAnimation, 4);
            
            gun.AddProjectileModuleFrom("ak-47", true, false);

            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.gunSwitchGroup = "Uzi";
            gun.DefaultModule.angleVariance = 6f;
            gun.DefaultModule.cooldownTime = .1f;
            gun.DefaultModule.numberOfShotsInClip = 30;
            Gun gun2 = PickupObjectDatabase.GetById(15) as Gun;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.SetBaseMaxAmmo(400);
            gun.quality = PickupObject.ItemQuality.B;
            gun.encounterTrackable.EncounterGuid = "hfhajafaassaafsjsapaspfafjadafwwdafx.";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.FULLAUTO;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage *= .54545454545f;
            projectile.shouldRotate = true;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            gun.AddToSubShop(ItemBuilder.ShopType.Trorc);


        }
        private float Inc = 0;
        private bool HasReloaded;

        protected void Update()
        {
            if (gun.CurrentOwner)
            {

                if (gun.PreventNormalFireAudio)
                {
                    this.gun.PreventNormalFireAudio = false;
                }
                if (!gun.IsReloading && !HasReloaded)
                {
                    this.HasReloaded = true;
                }
            }
        }

        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            base.OnReloadPressed(player, gun, bSOMETHING);
            if (gun.IsReloading && this.HasReloaded)
            {
                HasReloaded = false;
                this.Inc = 0;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);   
                
            }
        }
        public override void OnAutoReload(PlayerController player, Gun gun)
        {
            base.OnAutoReload(player, gun);
            this.Inc = 0;
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            base.OnPostFired(player, gun);
            gun.PreventNormalFireAudio = false;
            this.Inc++;
            bool flag = Inc > gun.ClipCapacity;
            if (flag)
            {
                Inc = 0;
            }
        }

        public override void PostProcessProjectile(Projectile projectile)
        {
            PlayerController playerController = this.gun.CurrentOwner as PlayerController;
            base.PostProcessProjectile(projectile);
            float math1 = 1 + .05f;
            float math2 = Convert.ToSingle(Math.Pow(math1, Inc));
            //float math3 = Convert.ToSingle(3 * math2);
            if(playerController.HasPickupID(280) || playerController.HasPickupID(134))
            {
                math2 *= 2f;
            }
            projectile.baseData.damage *= math2;
        //    ETGModConsole.Log(projectile.baseData.damage.ToString());
        }

        public Incremental()
        {

        }
    }
}
