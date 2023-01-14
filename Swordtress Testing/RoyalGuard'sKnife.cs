using System.Collections;
using System.Collections.Generic;
using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;

namespace Items
{
    class RoyalGuardsKnife : AdvancedGunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Royal Guard's Knife", "royal_guard's_knife");
            Game.Items.Rename("outdated_gun_mods:royal_guard's_knife", "sts:royal_guard's_knife");
            gun.gameObject.AddComponent<RoyalGuardsKnife>();
            gun.SetShortDescription("Stab Stab Stab");
            gun.SetLongDescription("A weak hunting knife used by The Royal Guard for cutting up game or sawing rope.");
            gun.SetupSprite(null, "royal_guard's_knife_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 2);
            gun.AddProjectileModuleFrom("38_special");
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SMALL_BULLET;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0f;
            gun.DefaultModule.angleVariance = 0f;
            gun.DefaultModule.cooldownTime = .2f;
            gun.DefaultModule.numberOfShotsInClip = 350;
            Gun gun2 = PickupObjectDatabase.GetById(151) as Gun;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.SetBaseMaxAmmo(350);
            gun.barrelOffset.transform.localPosition = new Vector3(0.75f, 0f, 0f);
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            gun.encounterTrackable.EncounterGuid = "Fuckfuckfuckfuckpleaseworkpleasefuckingworkpleasepleasework.";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.NONE;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage *= .5f;
            //VFXPool fuckyouyoustupidfuckingvfxihateyousofuckingmuchyougoddamnmistakefuckyourubelaswellihateyousomuchformakingthevfxpoolcodesofuckingbadAAAAAAAAAAAAAAAAAAAAAAAAA = VFXLibrary.CreateMuzzleflash("royal_guard's_knife_slash", new List<string> { "royal_guard's_knife_slash_001", "royal_guard's_knife_slash_002", "royal_guard's_knife_slash_003", "royal_guard's_knife_slash_004", }, 10, new List<IntVector2> { new IntVector2(27, 27), new IntVector2(27, 27), new IntVector2(27, 27), new IntVector2(27, 27), }, new List<tk2dBaseSprite.Anchor> {
            //    tk2dBaseSprite.Anchor.MiddleLeft, tk2dBaseSprite.Anchor.MiddleLeft, tk2dBaseSprite.Anchor.MiddleLeft, tk2dBaseSprite.Anchor.MiddleLeft}, new List<Vector2> { Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, }, false, false, false, false, 0, VFXAlignment.Fixed, true, new List<float> { 0, 0, 0, 0}, new List<Color> { VFXLibrary.emptyColor, VFXLibrary.emptyColor, VFXLibrary.emptyColor, VFXLibrary.emptyColor, });

            //ProjectileSlashingBehaviour slashingBehaviour = projectile.gameObject.AddComponent<ProjectileSlashingBehaviour>();
            //slashingBehaviour.SlashVFX = fuckyouyoustupidfuckingvfxihateyousofuckingmuchyougoddamnmistakefuckyourubelaswellihateyousomuchformakingthevfxpoolcodesofuckingbadAAAAAAAAAAAAAAAAAAAAAAAAA;
            //slashingBehaviour.SlashDimensions = 65;
            //slashingBehaviour.SlashRange = 2.5f;
            
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
            gun.AddToSubShop(ItemBuilder.ShopType.Goopton);
            gun.AddToSubShop(ItemBuilder.ShopType.Trorc);
        }
        
        private bool HasReloaded;

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

            gun.PreventNormalFireAudio = true;

        }



        public RoyalGuardsKnife()
        {

        }
    }
}
