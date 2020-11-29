using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using MonoMod;
using UnityEngine;
using Random = UnityEngine.Random;
using ItemAPI;

namespace Items
{
    class RGG : GunBehaviour
    {
        private static Gun rng_gun;


        public static void RandomizeStats()
        {
            RGG.rng_gun.reloadTime = (Random.Range(.5f, 1.3f));
            RGG.rng_gun.DefaultModule.ammoCost = Random.Range(1, 4);
            RGG.rng_gun.DefaultModule.cooldownTime = (Random.Range(.08f, 0.55f));
            RGG.rng_gun.SetBaseMaxAmmo ((Random.Range(100, 701)));
            RGG.rng_gun.DefaultModule.numberOfShotsInClip = (Random.Range(6, 101));
            RGG.Dmg = (Random.Range(5.5f, 15f));
            RGG.rng_gun.DefaultModule.projectiles[0].baseData.damage *= RGG.Dmg /= 5.5f;
            RGG.PspD = (Random.Range(12f, 50f));
            RGG.rng_gun.DefaultModule.projectiles[0].baseData.speed *= RGG.PspD /= 23f;
            RGG.Frc = (Random.Range(4f, 50f));
            RGG.rng_gun.DefaultModule.projectiles[0].baseData.force *= Frc /= 9f;
            RGG.GltInt = (Random.Range(0.03f,0.1f));
            RGG.DisProb = (Random.Range(0.15f, 0.5f));
            RGG.DisInten = (Random.Range(0.01f, 0.05f)); 
            RGG.ClrProb = (Random.Range(0.15f, 0.5f));
            RGG.ClrInten = (Random.Range(0.01f, 0.05f));
            Gun gun2 = PickupObjectDatabase.GetRandomGun() as Gun;
            RGG.rng_gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
        }


            
        public static void Add()
        {


           
            Gun gun = ETGMod.Databases.Items.NewGun("R.G.G.", "r.g.g.");
            RGG.rng_gun = gun;

            Game.Items.Rename("outdated_gun_mods:r.g.g.", "cel:r.g.g.");
            gun.gameObject.AddComponent<RGG>();
            gun.SetShortDescription("ShortDescription.txt");       
            gun.SetLongDescription("Stats randomize each run. \n\nA gun from the fabled 3rd dimension, it has become unstable in this realm and is constantly shifting."); 
           

            gun.SetupSprite(null, "r.g.g._idle_001", 8);
           
            gun.SetAnimationFPS(gun.shootAnimation, 24);
            gun.SetAnimationFPS(gun.reloadAnimation, 3);

            gun.AddProjectileModuleFrom("ak-47", true, false);
            gun.sprite.usesOverrideMaterial = true;
            Material material = gun.sprite.renderer.material;
            material.shader = ShaderCache.Acquire("Brave/Internal/Glitch");
            material.SetFloat("_GlitchInterval", 0.05f);
            material.SetFloat("_DispProbability", 0.4f);
            material.SetFloat("_DispIntensity", 0.04f);
            material.SetFloat("_ColorProbability", 0.4f);
            material.SetFloat("_ColorIntensity", 0.04f);
            gun.sprite.IsPerpendicular = true;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;        
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;          
            gun.reloadTime = RGG.rng_gun.reloadTime;
            gun.DefaultModule.cooldownTime = RGG.rng_gun.DefaultModule.cooldownTime;
            gun.DefaultModule.numberOfShotsInClip = RGG.rng_gun.DefaultModule.numberOfShotsInClip;
            gun.muzzleFlashEffects = RGG.rng_gun.muzzleFlashEffects;
            gun.SetBaseMaxAmmo(RGG.rng_gun.GetBaseMaxAmmo());
            gun.quality = PickupObject.ItemQuality.D;
            gun.gunClass = GunClass.SILLY;
            gun.encounterTrackable.EncounterGuid = "Thanks for the idea Reto! <3";
            gun.AddToSubShop(ItemBuilder.ShopType.Trorc);
            gun.AddToSubShop(ItemBuilder.ShopType.Cursula);
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            Material material2 = projectile.sprite.renderer.material;
            projectile.sprite.renderer.material = material;
            material2.shader = ShaderCache.Acquire("Brave/Internal/Glitch");
            material2.SetFloat("_GlitchInterval", 0.08f);
            material2.SetFloat("_DispProbability", 0.3f);
            material2.SetFloat("_DispIntensity", 0.014f);
            material2.SetFloat("_ColorProbability", 0.45f);
            material2.SetFloat("_ColorIntensity", 0.033f);
            projectile.transform.parent = gun.barrelOffset;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            RandomizeStats();
        }



        private bool HasReloaded;

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
                AkSoundEngine.PostEvent("Play_WPN_SAA_reload_01", base.gameObject);
            }
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", gameObject);
        }


 


        public static float Dmg;
        public static float PspD; //= (Random.Range(0.43478260869f, 8.69565217391f));
        public static float Frc;// = (Random.Range(0.44444444444f, 5f));
        public static float DmgCal;
        public static float PspDCal;// = PspD *= 23f;
        public static float FrcCal;//  = Frc *= 9f;
        //   public static float FSpd = (Random.Range(.07f, 1.5f));
        // public static float RSpd = (Random.Range(.5f, 2.2f));

        public static int AmmM;
        //  public static int CpS = (Random.Range(6, 101));
        // public static int AmmC = (Random.Range(1, 6));
        private static float GltInt;
        private static float DisProb;
        private static float DisInten;
        private static float ClrProb;
        private static float ClrInten;

        public RGG()
        {

        }
    }
}
