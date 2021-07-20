using System.Collections;
using Gungeon;
using UnityEngine;

namespace Items
{
    public class Vindication : GunBehaviour
    {

        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Vindication", "vindication");
            Game.Items.Rename("outdated_gun_mods:vindication", "cel:vindication");
            gun.gameObject.AddComponent<Vindication>();
            gun.SetShortDescription("My Sword...");
            gun.SetLongDescription("Weilded by the famous lawmaker Waxillium Ladrian, this 8-shot revolver features two specialized 'Hazekiller' rounds in its cylinder.");
            gun.SetupSprite(null, "vindication_idle_001", 12);
            gun.SetAnimationFPS(gun.shootAnimation, 24);
            gun.AddProjectileModuleFrom("magnum", true, false);
            gun.SetBaseMaxAmmo(160);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = .6f;
            gun.DefaultModule.cooldownTime = 0.10f;
            gun.InfiniteAmmo = false;
            gun.DefaultModule.numberOfShotsInClip = 8;
            gun.quality = PickupObject.ItemQuality.S;
            gun.DefaultModule.angleVariance = 4f;
            gun.encounterTrackable.EncounterGuid = "vindication";
            gun.sprite.IsPerpendicular = true;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            
        }

        public Vindication()
        {
        }
    }
}
