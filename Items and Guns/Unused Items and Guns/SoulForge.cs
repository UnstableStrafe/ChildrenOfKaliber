using System.Collections.Generic;
using Gungeon;
using UnityEngine;
using ItemAPI;

namespace Items
{
    class SoulForge : AdvancedGunBehaviour
    {
        public static void BulletAndShotgunKinProjs()
        {
            //Bullet Kin projectile will just be a strong, big bullet. Prob high damage, high kb. 
            //Shotgun kin will shoot a big shotgun shot

        }
        public static void BlobProjs()
        {
            Projectile normalBlobProj = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0]);
            normalBlobProj.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(normalBlobProj.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(normalBlobProj);
            normalBlobProj.baseData.damage *= 6f;
            normalBlobProj.baseData.speed *= 1f;
            normalBlobProj.baseData.force *= 2f;
            normalBlobProj.baseData.range *= 100;
            //each blob will use this standard format, but each type will have a status effect (e.g. poison for poisblob, etc.)
            //Chance blobs will be in Behemoths
        }
        public static void UndeadProjs()
        {
            //shoots a large, homing, piercing ghost
        }
        public static void MagicProjs()
        {
            //transmog laser like the Hexagun
        }
        public static void LootKinProjs()
        {
            //Key kin and chance kin are in here
            //Key kin will shoot a large, piercing, high-damage, key. Chance on killing an enemy to drop a key
            //Chance kill will shoot a ? box with high damage and kb. Drop a random pickup on killing an enemy
        }
        public static void HellspawnProjs()
        {
            //Big, fire explosion. leaves fire goop
        }
        public static void ExplosiveSniperFungiProjs()
        {
            //these are the smallest 3 groups, so ive put them in one method, for space conservation
            //Explosives will create a large explosion 
            //Snipers will shoot a powerful, piercing shot
            //Fungi will a projectile that leaves a large poison pool on hitting an enemy
        }
        public static void BehemothProjs()
        {
            //Each one has a unique proj
            //Lead maiden shoots a fast projectile with a high chance to bounce off enemies and redirect to another.
            //
        }
        public static void WarriorProjs()
        {

        }
        public static void MimicProjs()
        {

        }
        public static void BeastsProjs()
        {

        }
        public static void Add()
        {
            string shortName = "soul_forge";
            Gun gun = ETGMod.Databases.Items.NewGun("Soul Forge", shortName);
            Game.Items.Rename("outdated_gun_mods:"+shortName, "cel:"+shortName);
            gun.gameObject.AddComponent<SoulForge>();
            gun.SetShortDescription("Hammering Out The Kinks");
            gun.SetLongDescription("Steal the souls of killed enemies.\n\nThere is a tale of a land, long forgotten, that was destroyed in a single night. Nobody survived except for two men.");
            gun.SetupSprite(null, shortName+"_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 9);
            gun.SetAnimationFPS(gun.reloadAnimation, 10);
            gun.AddProjectileModuleFrom("38_special", true, false);
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SMALL_BULLET;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.5f;
            gun.DefaultModule.angleVariance = 0f;
            gun.DefaultModule.cooldownTime = 1f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(15) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(15) as Gun).muzzleFlashEffects;
            gun.SetBaseMaxAmmo(1000);
            gun.quality = PickupObject.ItemQuality.A;
            gun.encounterTrackable.EncounterGuid = "I would have named it a Handvil but Nevernamed held a gun to my head until I changed it.";
            gun.sprite.IsPerpendicular = true;
            gun.barrelOffset.transform.localPosition = new Vector3(2.25f, 0.3125f, 0f);
            gun.gunClass = GunClass.EXPLOSIVE;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage *= 1f;
            projectile.baseData.speed *= 1f;
            projectile.baseData.force *= 1f;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            gun.AddToSubShop(ItemBuilder.ShopType.Trorc);
            gun.AddToSubShop(ItemBuilder.ShopType.Cursula);
        }
        private bool HasReloaded;

        protected override void Update()
        {
            base.Update();
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
        private static List<Projectile> specialProjectiles = new List<Projectile>
        {
            //Bullet kins

            //Shotgun kin

            //Blobs

            //Undead
            
            //Books

            //Gunjurers

            //Loot Kin

            //Hellspawn

            //Explosive

            //Snipers

            //Behemoths (Each will have unique shots)

            //Fungi

            //Warriors

            //Mimics

            //Beasts
        };
        private List<string> soulStone = new List<string>
        {

        };
        public SoulForge()
        {

        }
    }
}
