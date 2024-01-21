using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using UnityEngine;
using Alexandria.ItemAPI;

namespace Items
{
    class Pray_K47 : AdvancedGunBehaviour
    {
        public static int Id;
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Pray-k 47", "pray-k_47");
            Game.Items.Rename("outdated_gun_mods:prayk_47", "ck:pray_k_47");
            gun.gameObject.AddComponent<Pray_K47>();
            gun.SetShortDescription("Pray While Shooting");
            gun.SetLongDescription("Deals 20% bonus damage to jammed enemies.\n\nA gun blessed by the Holy Pope Raffles III. For some reason, this affects the Jammed, despite them being from an entirely different religion.");
            gun.SetupSprite(null, "pray-k_47_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 10);
            gun.SetAnimationFPS(gun.reloadAnimation, 10);
            gun.AddProjectileModuleFrom("ak-47");
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SMALL_BULLET;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.7f;
            gun.barrelOffset.transform.localPosition = new Vector3(26f/16f, 7f/16f, 0f);
            gun.DefaultModule.angleVariance = 4f;
            gun.DefaultModule.cooldownTime = .15f;
            gun.DefaultModule.numberOfShotsInClip = 40;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.SetBaseMaxAmmo(500);
            gun.quality = PickupObject.ItemQuality.C;
            gun.encounterTrackable.EncounterGuid = "Lord, may nothing separate me from You today. Teach me how to choose only Your way today so each step will lead me closer to You. Help me walk by the Word and not my feelings. Help me to keep my heart pure and undivided. Protect me from my own careless thoughts, words, and actions. And keep me from being distracted by MY wants, MY desires, MY thoughts on how things should be. Help me to embrace what comes my way as an opportunity... rather than a personal inconvenience. And finally, help me to rest in the truth of Psalm 86:13, 'Great is your love toward me.' You already see the ways I will fall short and mess up. But right now, I consciously tuck Your whisper of absolute love for me into the deepest part of my heart. I recognize Your love for me is not based on my performance. You love me warts and all. That's amazing. But what's most amazing is that the Savior of the world would desire a few minutes with me this morning. Lord, help me to forever remember what a gift it is to sit with You like this. Amen.";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.FULLAUTO;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(15) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(15) as Gun).muzzleFlashEffects;
            // gun.AddPassiveStatModifier(PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE); // for melees/cursed guns
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;

            projectile.baseData.damage *= 1f;
            projectile.baseData.speed *= 1f;
            projectile.baseData.force *= 1f;
            //projectile.SetProjectileSpriteRight("projectile_sprite", 9, 5); // if using a custom projectile sprite
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
            Id = gun.PickupObjectId;
            gun.SetTag("kalashnikov");
        }

        private bool HasReloaded;

        public override void PostProcessProjectile(Projectile projectile)
        {
            base.PostProcessProjectile(projectile);
            projectile.BlackPhantomDamageMultiplier *= 1.2f;
        }

        protected override void Update()
        {
            if (gun.CurrentOwner)
            {
                
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


        



        public Pray_K47()
        {

        }
    }
}
