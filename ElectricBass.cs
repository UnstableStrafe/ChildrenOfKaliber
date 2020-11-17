
using Gungeon;
using ItemAPI;
using UnityEngine;
using System.Collections;
using Dungeonator;
using System;

namespace Items
{
    class ElectricBass : AdvancedGunBehaviour
    {
        public static void Add()
        {

            Gun gun = ETGMod.Databases.Items.NewGun("Electric Bass", "electric_bass");
            Game.Items.Rename("outdated_gun_mods:electric_bass", "cel:electric_bass");
            gun.gameObject.AddComponent<ElectricBass>();
            gun.SetShortDescription("Atom And Evil");
            gun.SetLongDescription("Creates an eletric AoE instead of shooting projectiles.\n\nA guitar imbued with the power of heavy metal music.");
            gun.SetupSprite(null, "electric_bass_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 24);
            gun.SetAnimationFPS(gun.reloadAnimation, 3);
            gun.AddProjectileModuleFrom("38_special", true, false);
            gun.sprite.IsPerpendicular = true;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0;
            gun.DefaultModule.cooldownTime = .3f;
            gun.SetBaseMaxAmmo(500);
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.MEDIUM_BLASTER;
            gun.DefaultModule.numberOfShotsInClip = gun.GetBaseMaxAmmo();
            gun.quality = PickupObject.ItemQuality.C;
            gun.gunClass = GunClass.FULLAUTO;
            gun.encounterTrackable.EncounterGuid = "AoE Bass gun";
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.AddToSubShop(ItemBuilder.ShopType.Trorc);
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 0;
            projectile.baseData.speed *= 1f;
            projectile.baseData.range *= .00001f;
            projectile.collidesWithEnemies = false;
            projectile.collidesWithPlayer = false;
            projectile.sprite.renderer.enabled = false;
            projectile.hitEffects.suppressMidairDeathVfx = true;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

        }
        private IEnumerator HandleHeatEffectsCR(float Radius, float Duration, GameActorEffect gameActorEffect, tk2dBaseSprite Psprite)
        {

            Psprite = m_owner.sprite;
           
            float elapsed = 0f;
            RoomHandler r = Psprite.transform.position.GetAbsoluteRoom();
            Vector3 tableCenter = Psprite.WorldCenter.ToVector3ZisY(0f);
            Action<AIActor, float> AuraAction = delegate (AIActor actor, float dist)
            {
                actor.healthHaver.ApplyDamage(.3f * (m_owner.stats.GetStatModifier(PlayerStats.StatType.Damage)), Vector2.zero, "primaryPlayer");
            };
            while (elapsed < TDuration)
            {
                elapsed += BraveTime.DeltaTime;
                r.ApplyActionToNearbyEnemies(tableCenter.XY(), TRadius, AuraAction);
                yield return null;
            }
            yield break;
        }
        public override void OnInitializedWithOwner(GameActor actor)
        {
            base.OnInitializedWithOwner(actor);
            PlayerController player = gun.CurrentOwner as PlayerController;
            if (actor == player)
            {
                this.HandleRadialIndicator(TRadius, player.sprite);
            }
        }
        private void HandleRadialIndicator(float Radius, tk2dBaseSprite Psprite)
        {
            Psprite = m_owner.sprite;
            if (!this.m_indicator)
            {
                Vector3 position = Psprite.WorldCenter.ToVector3ZisY(0f);
                this.m_indicator = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/HeatIndicator"), position, Quaternion.identity, m_owner.sprite.transform)).GetComponent<HeatIndicatorController>();
                this.m_indicator.CurrentRadius = TRadius;
                m_indicator.IsFire = false;
                m_indicator.CurrentColor = new Color(10 / 25, 152 / 25, 216 /25);
            }
        }
        private void UnhandleRadialIndicator()
        {
            if (this.m_indicator)
            {
                this.m_indicator.EndEffect();
                this.m_indicator = null;
            }
        }
        private GameActorHealthEffect zappy = new GameActorHealthEffect()
        {
            AffectsEnemies = true,
            effectIdentifier = "zappy",
            DamagePerSecondToEnemies = 10,
            duration = .2f,

        };
        private float TDuration = .3f;
        private float TRadius = 7f;

        public PlayerController m_owner
        {
            get
            {
                bool flag = this.gun.CurrentOwner != null && this.gun.CurrentOwner is PlayerController;
                PlayerController result;
                if (flag)
                {
                    result = (this.gun.CurrentOwner as PlayerController);
                }
                else
                {
                    result = null;
                }
                return result;
            }
        }
        private bool AuraAssigned = false;
        private bool HasReloaded;
        private PlayerController lastOwner = null;
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
        public override void OnGunsChanged(Gun previous, Gun current, bool newGun)
        {
            base.OnGunsChanged(previous, current, newGun);
            if (current == this.gun)
            {
                this.HandleRadialIndicator(TRadius, (this.gun.CurrentOwner as PlayerController).sprite);
            }
            else
            {
                if (current != this.gun)
                {
                    this.UnhandleRadialIndicator();
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


        public override void OnPostFired(PlayerController player, Gun gun)
        {

            gun.PreventNormalFireAudio = true;
            tk2dBaseSprite Psprite;
            Psprite = player.sprite;
            Psprite.StartCoroutine(this.HandleHeatEffectsCR(TRadius, TDuration, zappy, Psprite));
        }
        private HeatIndicatorController m_indicator;
        public ElectricBass()
        {

        }
    }
}
