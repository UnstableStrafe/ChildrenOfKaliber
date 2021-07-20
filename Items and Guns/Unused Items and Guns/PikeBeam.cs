using UnityEngine;
using ItemAPI;
using Gungeon;

namespace Items
{
    class PikeBeam : AdvancedGunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Pike Beam", "pike_beam");
            Game.Items.Rename("outdated_gun_mods:pike_beam", "cel:pike_beam");
            gun.gameObject.AddComponent<PikeBeam>();
            gun.SetShortDescription("Not A Glaive");
            gun.SetLongDescription("Can cut across entire rooms, as long as there are no walls in the way.");
            gun.SetupSprite(null, "pike_beam_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 4);
            gun.SetAnimationFPS(gun.reloadAnimation, 2);
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(121) as Gun, true, false);
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Beam;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0f;
            gun.DefaultModule.customAmmoType = "poison_blob";
            gun.DefaultModule.angleVariance = 0f;
            gun.DefaultModule.cooldownTime = .2f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            Gun gun2 = PickupObjectDatabase.GetById(151) as Gun;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.SetBaseMaxAmmo(150);
            gun.barrelOffset.transform.localPosition = new Vector3(3.5f, 0f, 0f);
            gun.quality = PickupObject.ItemQuality.B;
            gun.encounterTrackable.EncounterGuid = "Hippity hoppity, your spinal cord is my property.";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.BEAM;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            projectile.gameObject.AddComponent<MeshRenderer>();
            projectile.gameObject.AddComponent<MeshFilter>();
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            BasicBeamController BasicBitchBeam = FUCK.GenerateBeamPrefab(gun.DefaultModule.projectiles[0], "");            
            projectile.baseData.speed = -1;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
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
                AkSoundEngine.PostEvent("Play_WPN_dl45heavylaser_reload", base.gameObject);
            }
        }


        public override void OnPostFired(PlayerController player, Gun gun)
        {

            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_energycannon_shot_01", gameObject);
        }



        public PikeBeam()
        {

        }
    }
}
