
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace Items
{
    class Fallout : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Fallout", "fallout");
            Game.Items.Rename("outdated_gun_mods:fallout", "cel:fallout");
            gun.gameObject.AddComponent<Fallout>();
            gun.SetShortDescription("Keep Out Of Reach Of Children");
            gun.SetLongDescription("Shoots powerful blasts of nuclear energy.\n\n This gun is powered by pure poisonium (not to be confused with the harmless substance, toxicium) which is lethal to any creature in the known universe.");

            gun.SetupSprite(null,"fallout_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 4);
            gun.SetAnimationFPS(gun.reloadAnimation, 2);
            gun.AddProjectileModuleFrom("38_special", true, false);
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.4f;
            gun.DefaultModule.customAmmoType = "poison_blob";
            gun.DefaultModule.angleVariance = 0f;
            gun.DefaultModule.cooldownTime = .75f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            Gun gun2 = PickupObjectDatabase.GetById(151) as Gun;
            Gun gun3 = PickupObjectDatabase.GetById(32) as Gun;
            gun.gunSwitchGroup = gun3.gunSwitchGroup;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.SetBaseMaxAmmo(150);
            gun.barrelOffset.transform.localPosition = new Vector3(3.5f, 0f, 0f);
            gun.quality = PickupObject.ItemQuality.A;
            gun.encounterTrackable.EncounterGuid = "For his nuetral special, gun wields a Joker.";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.RIFLE;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage *= 16;
            projectile.baseData.speed *= 2f;
            projectile.baseData.force *= 4f;
            projectile.baseData.range *= 1000;
            projectile.SetProjectileSpriteRight("fallout_smol_projectile_001", 31, 8, null, null);
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            gun.AddToSubShop(ItemBuilder.ShopType.Goopton);
            gun.AddToSubShop(ItemBuilder.ShopType.Trorc);

            
        }

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
                AkSoundEngine.PostEvent("Play_WPN_dl45heavylaser_reload", base.gameObject);
            }
        }

       
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            
            gun.PreventNormalFireAudio = false;
        }



        public Fallout()
        {

        }
    }
}
