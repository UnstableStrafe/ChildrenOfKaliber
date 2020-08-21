using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace Items
{
    class AssaultFleet : GunBehaviour
    {
        public static void Add()
        {

            Gun gun = ETGMod.Databases.Items.NewGun("Assault Fleet", "assault_fleet");
            this_gun = gun;
            Game.Items.Rename("outdated_gun_mods:assault_fleet", "cel:assault_fleet");
            gun.gameObject.AddComponent<AssaultFleet>();
            gun.SetShortDescription("Vola! Vola! Vola! Volare Via!");
            gun.SetLongDescription("Shoots small, remote-control aircrafts which fire upon nearby enemies.\n\nThe Gungeon has often been known to imbue innocent objects with lethal firepower and this children's toy turned deadly weapon is no exception.");
            gun.SetupSprite(null, "assault_fleet_idle_001", 13);
            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.AddProjectileModuleFrom("38_special", true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0f;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(97) as Gun).gunSwitchGroup;
            gun.DefaultModule.angleVariance = 5f;
            gun.DefaultModule.cooldownTime = 0.10f;
            gun.SetBaseMaxAmmo(100);
            gun.gunClass = GunClass.FULLAUTO;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.quality = PickupObject.ItemQuality.C;
            gun.encounterTrackable.EncounterGuid = "Assault Fleet Gun";
            gun.gunHandedness = GunHandedness.HiddenOneHanded;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.shouldRotate = true;
            projectile.baseData.damage *= 2f;
            projectile.baseData.speed *= .75f;
            projectile.PreMoveModifiers = (Action<Projectile>)Delegate.Combine(projectile.PreMoveModifiers, new Action<Projectile>(PreMoveProjectileModifier));
            FleetController fleetController = projectile.gameObject.AddComponent<FleetController>();
            fleetController.projectileToSpawn = ((Gun)ETGMod.Databases.Items["makarov"]).DefaultModule.projectiles[0];
            Projectile proj2 = fleetController.projectileToSpawn;
            proj2.AdditionalScaleMultiplier = .5f;
            gun.sprite.IsPerpendicular = true;
            gun.CanBeDropped = false;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }



        private bool HasReloaded;
        private bool Assigned;
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
        private static Gun this_gun;
        private static void PreMoveProjectileModifier(Projectile p)
        {
            PlayerController player = this_gun.CurrentOwner as PlayerController;
            if (player && p && p.Owner is PlayerController)
            {
                BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(player.PlayerIDX);
                if (instanceForPlayer == null)
                {
                    return;
                }
                Vector2 vector = Vector2.zero;
                if (instanceForPlayer.IsKeyboardAndMouse(false))
                {
                    vector = (p.Owner as PlayerController).unadjustedAimPoint.XY() - p.specRigidbody.UnitCenter;
                }
                else
                {
                    if (instanceForPlayer.ActiveActions == null)
                    {
                        return;
                    }
                    vector = instanceForPlayer.ActiveActions.Aim.Vector;
                }
                float target = vector.ToAngle();
                float current = BraveMathCollege.Atan2Degrees(p.Direction);
                float num = 0f;
                if (p.ElapsedTime < trackingTime)
                {
                    num = trackingCurve.Evaluate(p.ElapsedTime / trackingTime) * trackingSpeed;
                }
                float target2 = Mathf.MoveTowardsAngle(current, target, num * BraveTime.DeltaTime);
                Vector2 vector2 = Quaternion.Euler(0f, 0f, Mathf.DeltaAngle(current, target2)) * p.Direction;
                if (p is HelixProjectile)
                {
                    HelixProjectile helixProjectile = p as HelixProjectile;
                    helixProjectile.AdjustRightVector(Mathf.DeltaAngle(current, target2));
                }
                if (p.OverrideMotionModule != null)
                {
                    p.OverrideMotionModule.AdjustRightVector(Mathf.DeltaAngle(current, target2));
                }
                p.Direction = vector2.normalized;
                if (p.shouldRotate)
                {
                    p.transform.eulerAngles = new Vector3(0f, 0f, p.Direction.ToAngle());
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

        public static float trackingSpeed;

        public static float trackingTime;

        [CurveRange(0f, 0f, 1f, 1f)]
        public static AnimationCurve trackingCurve;

        public override void OnPostFired(PlayerController player, Gun gun)
        {



        }


        public AssaultFleet()
        {

        }
    }
}
