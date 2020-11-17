using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;

namespace Items
{
    [MonoMod.MonoModAdded]
    public class Vindication : GunBehaviour
    {
        // Token: 0x06000031 RID: 49 RVA: 0x00003360 File Offset: 0x00001560
        [MonoMod.MonoModAdded]
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

        [MonoMod.MonoModAdded]
        public override void PostProcessProjectile(Projectile projectile)
        {
            PlayerController playerController = this.gun.CurrentOwner as PlayerController;
            bool flag = playerController == null;
            if (flag)
            {
                this.gun.ammo = this.gun.GetBaseMaxAmmo();
            }
            projectile.baseData.damage *= 1f;
            projectile.baseData.speed *= 1.04347826087f;
            projectile.baseData.force *= .33f;
            this.gun.DefaultModule.ammoCost = 1;
            base.StartCoroutine(this.EjectFrom(playerController));
            base.PostProcessProjectile(projectile);
            projectile.gameObject.AddComponent<VindicationProjectile>();
        }

        [MonoMod.MonoModAdded]
        public IEnumerator EjectFrom(PlayerController player)
        {
            yield return new WaitForSeconds(3f);
            yield break;
        }

        [MonoMod.MonoModAdded]
        public Vindication()
        {
        }
    }
}
