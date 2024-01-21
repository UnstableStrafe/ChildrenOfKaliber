using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;

namespace Items
{
    class Tesla : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Tesla", "tesla");
            Game.Items.Rename("outdated_gun_mods:tesla", "ck:tesla");
            gun.gameObject.AddComponent<Tesla>();
            gun.SetShortDescription("Static Fields");
            gun.SetLongDescription("Shoots bolts of energy.\n\n Kinda tastes metallic.\n\n\n (Disclaimer: Do not lick tesla coils)");
            gun.SetupSprite(null, "tesla_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 10);

            tk2dSpriteAnimationClip animationclipShoot = gun.sprite.spriteAnimator.GetClipByName(gun.shootAnimation);
            float[] shootOffsetsX = new float[] { -0.0625f, -0.1250f, -0.9375f, -1.0000f, -1.0000f, -1.0000f, -0.4375f, 0.0000f, 0.0000f };
            float[] shootOffsetsY = new float[] { -0.1250f, -0.1875f, 0.3750f, 0.2500f, 0.1875f, 0.3750f, 0.3125f, -0.1250f, 0.0000f };

            tk2dSpriteAnimationClip animationclipReload = gun.sprite.spriteAnimator.GetClipByName(gun.reloadAnimation);
            float[] reloadOffsetsX = new float[] { 0.0000f, 0.2500f, 0.3750f, 0.3750f, 0.3750f, 0.3750f, 0.3750f, 0.3750f, 0.3750f, 0.3750f, 0.3750f, 0.3750f, 0.3750f, 0.3750f, 0.0000f, 0.0000f, 0.0000f };
            float[] reloadOffsetsY = new float[] { 0.0000f, -0.6875f, -0.6875f, -0.6250f, -0.6250f, -0.6250f, -0.6250f, -0.6250f, -0.6250f, -0.6250f, -0.6250f, -0.6250f, -0.6250f, -0.6250f, -0.1250f, 0.0625f, 0.0000f };

            for (int i = 0; i < reloadOffsetsX.Length && i < reloadOffsetsY.Length && i < animationclipReload.frames.Length; i++)
            {
                int id = animationclipReload.frames[i].spriteId;
                Vector3 vector3 = new Vector3(reloadOffsetsX[i], reloadOffsetsY[i]);
                animationclipReload.frames[i].spriteCollection.spriteDefinitions[id].position0 += vector3;
                animationclipReload.frames[i].spriteCollection.spriteDefinitions[id].position1 += vector3;
                animationclipReload.frames[i].spriteCollection.spriteDefinitions[id].position2 += vector3;
                animationclipReload.frames[i].spriteCollection.spriteDefinitions[id].position3 += vector3;
            }

            for (int i = 0; i < shootOffsetsX.Length && i < shootOffsetsY.Length && i < animationclipShoot.frames.Length; i++)
            {
                int id = animationclipShoot.frames[i].spriteId;
                Vector3 vector3 = new Vector3(shootOffsetsX[i], shootOffsetsY[i]);
                animationclipShoot.frames[i].spriteCollection.spriteDefinitions[id].position0 += vector3;
                animationclipShoot.frames[i].spriteCollection.spriteDefinitions[id].position1 += vector3;
                animationclipShoot.frames[i].spriteCollection.spriteDefinitions[id].position2 += vector3;
                animationclipShoot.frames[i].spriteCollection.spriteDefinitions[id].position3 += vector3;
            }

            gun.usesContinuousFireAnimation = false;
            gun.AddProjectileModuleFrom("electric_rifle");
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.4f;
            gun.DefaultModule.burstShotCount = 5;
            gun.DefaultModule.burstCooldownTime = .1f;
            gun.DefaultModule.cooldownTime = .8f;
            gun.DefaultModule.numberOfShotsInClip = 50;
            gun.DefaultModule.angleVariance = 5;
            gun.gunClass = GunClass.FULLAUTO;
            gun.gunHandedness = GunHandedness.OneHanded;
            Gun gun2 = PickupObjectDatabase.GetById(153) as Gun;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.SetBaseMaxAmmo(500);
            gun.sprite.IsPerpendicular = true;
            gun.quality = PickupObject.ItemQuality.S;
            gun.encounterTrackable.EncounterGuid = "Please do not lick electricty. k thanks.";
            gun.barrelOffset.localPosition = new Vector3(2.375f, 0.4375f, 0f);
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(153) as Gun).gunSwitchGroup;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 1.25f;
            projectile.baseData.speed *= 1f;
            projectile.transform.parent = gun.barrelOffset;

        }


        private bool HasReloaded;
        private PlayerController lastOwner = null;
        private DamageTypeModifier electricityImmunity = null;
        public override void Update()
        {
            PlayerController player = gun.CurrentOwner as PlayerController;
            if (gun.CurrentOwner)
            {
                bool flag5 = this.gun.CurrentOwner != null && this.gun.CurrentOwner is PlayerController;
                if (flag5)
                {
                    this.lastOwner = (this.gun.CurrentOwner as PlayerController);
                    bool flag6 = this.electricityImmunity == null;
                    if (flag6)
                    {
                        this.electricityImmunity = new DamageTypeModifier
                        {
                            damageMultiplier = 0f,
                            damageType = CoreDamageTypes.Electric
                        };
                        this.lastOwner.healthHaver.damageTypeModifiers.Add(this.electricityImmunity);
                    }
                }
                else
                {
                    bool flag7 = this.electricityImmunity != null;
                    if (flag7)
                    {
                        this.lastOwner.healthHaver.damageTypeModifiers.Remove(this.electricityImmunity);
                        this.electricityImmunity = null;
                    }
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


       



        public Tesla()
        {

        }
    }
}
