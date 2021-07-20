using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using UnityEngine;
using ItemAPI;

namespace Items
{
    class SliverBeam : AdvancedGunBehaviour
    {
        public static void Add()
        {

            Gun gun = ETGMod.Databases.Items.NewGun("Sliver Beam", "sliver_beam");
            Game.Items.Rename("outdated_gun_mods:sliver_beam", "cel:sliver_beam");
            var behav = gun.gameObject.AddComponent<SliverBeam>();
            behav.overrideNormalFireAudio = "Play_ENM_shelleton_beam_01";
            behav.preventNormalFireAudio = true;
            gun.SetShortDescription("Like A Pike Through Butter");
            gun.SetLongDescription("Can cut across rooms, assuming there is no walls in the way.");
            gun.SetupSprite(null, "sliver_beam_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 8);
            gun.isAudioLoop = true;
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.doesScreenShake = false;
            gun.DefaultModule.ammoCost = 10;
            gun.DefaultModule.angleVariance = 0;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Beam;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.DefaultModule.cooldownTime = 0.001f;
            gun.DefaultModule.numberOfShotsInClip = 3000;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.BEAM;
            gun.barrelOffset.transform.localPosition = new Vector3(1.75f, 0.5f, 0f);
            gun.SetBaseMaxAmmo(3000);
            gun.ammo = 3000;

            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).loopStart = 1;

            List<string> BeamAnimPaths = new List<string>()
            {
                "Items/Resources/Beams/sliver_beam_mid_001",
            };
            List<string> BeamEndPaths = new List<string>()
            {
                "Items/Resources/Beams/sliver_beam_end_001",
            };

            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0]);

            BasicBeamController beamComp = projectile.GenerateBeamPrefab(
                "Items/Resources/Beams/sliver_beam_mid_001",
                new Vector2(10, 4),
                new Vector2(0, 1),
                BeamAnimPaths,
                9,
                //Impact
                null,
                -1,
                null,
                null,
                //End
                BeamEndPaths,
                9,
                new Vector2(10, 3),
                new Vector2(0, 1),
                //Beginning
                null,
                -1,
                null,
                null
                );

            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            projectile.baseData.damage = 15f;
            projectile.baseData.force *= 0.1f;
            projectile.baseData.range = float.MaxValue;
            projectile.baseData.speed *= 5f;

            beamComp.penetration = 10000;
            beamComp.boneType = BasicBeamController.BeamBoneType.Straight;
            beamComp.interpolateStretchedBones = false;

            gun.DefaultModule.projectiles[0] = projectile;
            gun.encounterTrackable.EncounterGuid = "FWOZZOOOOOMMMMM";
            gun.quality = PickupObject.ItemQuality.C; //D
            ETGMod.Databases.Items.Add(gun, null, "ANY");

        }
        public SliverBeam()
        {

        }
    }
}
