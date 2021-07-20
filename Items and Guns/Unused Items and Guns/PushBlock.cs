using Gungeon;
using ItemAPI;
using UnityEngine;
namespace Items
{
    class PushBlock : GunBehaviour
    {
        
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Push Block", "push_block");
            Game.Items.Rename("outdated_gun_mods:push_block", "cel:push_block");
            gun.gameObject.AddComponent<PushBlock>();
            gun.SetShortDescription("More Of A \"Throw Block\"");
            gun.SetLongDescription("Does what you would expect.");
            gun.SetupSprite(null, "push_block_idle_001", 13);
            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.AddProjectileModuleFrom("38_special", true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.5f;
            gun.DefaultModule.angleVariance = 0f;
            gun.DefaultModule.cooldownTime = .11f;
            gun.SetBaseMaxAmmo(150);
            gun.gunClass = GunClass.SILLY;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.quality = PickupObject.ItemQuality.C;
            gun.encounterTrackable.EncounterGuid = "crush thy enemies, peasants";
            gun.barrelOffset.transform.localPosition = new UnityEngine.Vector3(1f, .5f, 0f);
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 5;
            projectile.baseData.speed *= .5f;
            projectile.baseData.force *= 6f;
            projectile.transform.parent = gun.barrelOffset;
            projectile.SetProjectileSpriteRight("push_block_idle_001", 16, 16, null, null);
            gun.sprite.IsPerpendicular = true;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }

        private bool HasReloaded;
        public override void PostProcessProjectile(Projectile projectile)
        {
            base.PostProcessProjectile(projectile);
            projectile.OnPostUpdate += this.ProjUpdate;
        }
        private void ProjUpdate(Projectile proj)
        {
            proj.Speed *= 1.10f;
        }
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

            }
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {

            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_skullgun_shot_01", gameObject);
        }



        public PushBlock()
        {

        }
    }
}
