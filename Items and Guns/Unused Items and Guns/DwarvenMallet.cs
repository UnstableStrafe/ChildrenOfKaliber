using Gungeon;

namespace Items
{
    public class DwarvenMallet : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Dwarven Mallet", "dwarven_mallet");
            Game.Items.Rename("outdated_gun_mods:dwarven_mallet", "cel:dwarven_mallet");
            gun.gameObject.AddComponent<DwarvenMallet>();
            GunExt.SetShortDescription(gun, "Trusty, Not Safe");
            GunExt.SetLongDescription(gun, "A simple Dwarven smithing hammer.\n\nWhile it is a techinally used as a melee weapon, it also is used to create firearms. " +
                "This has led the Gungeon to simply ignore the fact it is a melee weapon.");
            GunExt.SetupSprite(gun, null, "dwarven_mallet_idle_001", 1);
            GunExt.SetAnimationFPS(gun, gun.shootAnimation, 8);
            GunExt.SetAnimationFPS(gun, gun.idleAnimation, 1);
            GunExt.SetAnimationFPS(gun, gun.reloadAnimation, 1);
            GunExt.AddProjectileModuleFrom(gun, "wonderboy");
            gun.SetBaseMaxAmmo(100);
            gun.reloadTime = 0f;
            gun.DefaultModule.cooldownTime = .2f;
            gun.InfiniteAmmo = true;
            gun.DefaultModule.numberOfShotsInClip = int.MaxValue;
            gun.quality = PickupObject.ItemQuality.SPECIAL;
            gun.encounterTrackable.EncounterGuid = "dwarven_mallet";
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
            gun.CanBeDropped = false;
            gun.InfiniteAmmo = true;
            //ItemBuilder.AddPassiveStatModifier(gun, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
            Gun gun2 = (Gun)ETGMod.Databases.Items["wonderboy"];
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.IsHeroSword = true;
            gun.HeroSwordDoesntBlank = false;
            gun.DefaultModule.GetCurrentProjectile().baseData.damage = 20f;
        }

        public override void Update()
        {
            bool flag = this.SwordCooldown > 0f;
            if (flag)
            {
                this.SwordCooldown -= BraveTime.DeltaTime;
            }
            bool flag2 = this.gun != null;
            if (flag2)
            {
                bool flag3 = this.gun.ClipShotsRemaining < this.gun.DefaultModule.numberOfShotsInClip;
                if (flag3)
                {
                    this.gun.ForceImmediateReload(false);
                }
            }
        }

        public override void PostProcessProjectile(Projectile projectile)
        {
            PlayerController x = this.gun.CurrentOwner as PlayerController;
            bool flag = x == null;
            bool flag2 = flag;
            bool flag3 = flag2;
            if (flag3)
            {
                this.gun.ammo = this.gun.GetBaseMaxAmmo();
            }
            bool flag4 = projectile != null;
            if (flag4)
            {
                UnityEngine.Object.Destroy(projectile.gameObject);
            }
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            bool flag = this.SwordCooldown <= 0f;
            if (flag)
            {
                gun.PreventNormalFireAudio = true;
                AkSoundEngine.PostEvent("Play_WPN_blasphemy_shot_01", base.gameObject);
                this.SwordCooldown = 0.75f;
            }
        }

        public static ProjectileModule CopyFrom(ProjectileModule origin)
        {
            return new ProjectileModule();
        }

        private float SwordCooldown = 0f;
    }
}