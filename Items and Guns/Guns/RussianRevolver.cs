using Gungeon;
using Alexandria.ItemAPI;
using Random = UnityEngine.Random;
using UnityEngine;
using System.Collections;
using System;

namespace Items
{
    class RussianRevolver : GunBehaviour
    {
        private static Gun RussianRevolv;
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Russian Revolver", "russian_revolver");
            RussianRevolv = gun;
            Game.Items.Rename("outdated_gun_mods:russian_revolver", "ck:russian_revolver");

            //WORK ON RISK MECHANIC IDEA DUMBASS

            gun.SetShortDescription("You See Ivan...");
            gun.SetLongDescription("Shoots powerful shots, but only one shot in each clip deals damage.\n\nWhen you hold peestol like me, you shall never shoot the inacurrate, because of fear of shooting fingers.");
            gun.SetupSprite(null, "russian_revolver_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 16);
            gun.SetAnimationFPS(gun.reloadAnimation, 2);
            gun.AddProjectileModuleFrom("38_special");
            gun.gameObject.AddComponent<RussianRevolver>();
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.MEDIUM_BULLET;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.2f;
            gun.DefaultModule.angleVariance = 0f;
            gun.DefaultModule.cooldownTime = .15f;
            gun.DefaultModule.numberOfShotsInClip = 6;
            Gun gun2 = PickupObjectDatabase.GetById(56) as Gun;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.SetBaseMaxAmmo(480);
            gun.gunSwitchGroup = gun2.gunSwitchGroup;
            gun.quality = PickupObject.ItemQuality.D;
            gun.encounterTrackable.EncounterGuid = "The Soviet Union, officially the Union of Soviet Socialist Republics (USSR), was a federal socialist state in Northern Eurasia that existed from 1922 to 1991. Nominally a union of multiple national Soviet republics, in practice its government and economy were highly centralized until its final years. It was a one-party state governed by the Communist Party, with Moscow as its capital in its largest republic, the Russian SFSR. Other major urban centers were Leningrad, Kiev, Minsk, Tashkent, Alma-Ata and Novosibirsk. It was the largest country in the world by surface area, spanning over 10,000 kilometers (6,200 mi) east to west across 11 time zones and over 7,200 kilometers (4,500 mi) north to south. Its territory included much of Eastern Europe as well as part of Northern Europe and all of Northern and Central Asia. It had five climate zones such as tundra, taiga, steppes, desert, and mountains. Its diverse population was collectively known as Soviet people.";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.PISTOL;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage *= 0;
            projectile.sprite.renderer.enabled = false;
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
            gun.AddToSubShop(ItemBuilder.ShopType.Goopton);
            gun.AddToSubShop(ItemBuilder.ShopType.Trorc);  
        }
        private int Index = 0;
        [SerializeField]
        private int LoadedShot = Random.Range(1, RussianRevolv.ClipCapacity + 1);
        private int LoadedShot2;
        private bool UsedShot = false;
        private bool HasReloaded;

        public override void Update()
        {
            PlayerController player = gun.CurrentOwner as PlayerController;
            if (gun.CurrentOwner)      
            {
                Has7();
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
        private void Has7()
        {
            PlayerController player = gun.CurrentOwner as PlayerController;
            if (player.HasPickupID(289))
            {
                RussianRevolv.DefaultModule.projectiles[0].baseData.damage = 35 * player.stats.GetStatValue(PlayerStats.StatType.Damage);
                RussianRevolv.DefaultModule.projectiles[0].sprite.renderer.enabled = true;
            }
            else
            {
                RussianRevolv.DefaultModule.projectiles[0].baseData.damage = 0;
                RussianRevolv.DefaultModule.projectiles[0].sprite.renderer.enabled = false;
            }
        }
        public override void PostProcessProjectile(Projectile projectile)
        {
            PlayerController player = gun.CurrentOwner as PlayerController;
            if (!player.HasPickupID(289) || player.HasPickupID(815))
            {               
                if (Index == LoadedShot)
                {
                    projectile.sprite.renderer.enabled = true;
                    projectile.baseData.damage = 35 * player.stats.GetStatValue(PlayerStats.StatType.Damage);
                }
                if (Index != LoadedShot)
                {
                    projectile.DieInAir(true);
                }
            }

            
        }

        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && this.HasReloaded)
            {
                LoadedShot = -3;
                Index = 0;
                LoadedShot = Random.Range(1, gun.ClipCapacity + 1);
                UsedShot = false;
                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
            }
            if (player.HasPickupID(116) || player.HasPickupID(134) || player.HasPickupID(131) || player.HasPickupID(815) && gun.ClipShotsRemaining == 0)
            {
                float coin = .25f;
                if (Random.value <= coin)
                {
                    gun.ammo += gun.ClipCapacity;
                }
            }
        }

       
        
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            Index++;
        }



        public RussianRevolver()
        {

        }
    }
}
