using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;
using Gungeon;
namespace Items
{
    class ALiteralRock : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("A Rock", "a_literal_rock");
            Game.Items.Rename("outdated_gun_mods:a_rock", "cel:a_rock");
            gun.gameObject.AddComponent<ALiteralRock>();

            gun.SetShortDescription("Ooga Booga");
            gun.SetLongDescription("It's just a rock. That's it. Nothing special.\n\nJust a rock someone picked up and put in a chest.");

            gun.SetupSprite(null, "a_literal_rock_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 16);
            gun.SetAnimationFPS(gun.reloadAnimation, 2);
            gun.AddProjectileModuleFrom("magnum", true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0;
            gun.DefaultModule.angleVariance = 0;
            gun.DefaultModule.cooldownTime = 0;
            gun.DefaultModule.numberOfShotsInClip = 0;
            gun.SetBaseMaxAmmo(0);
            gun.ammo = 0;
            gun.InfiniteAmmo = false;
            gun.quality = PickupObject.ItemQuality.D;
            gun.encounterTrackable.EncounterGuid = "Its a rock.";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.SILLY;
            gun.CanGainAmmo = false;
            gun.IgnoredByRat = true;
            gun.encounterTrackable.m_doNotificationOnEncounter = false;

            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 0;
            projectile.baseData.speed *= 1f;
            projectile.baseData.force *= 0f;
            projectile.baseData.range *= 0;

            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }



       


        public override void OnPostFired(PlayerController player, Gun gun)
        {

            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_magnum_shot_01", gameObject);
        }

        

        public ALiteralRock()
        {

        }
    }
}
