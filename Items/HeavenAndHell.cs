
using Gungeon;
using ItemAPI;
using UnityEngine;
using System.Collections;
using Dungeonator;
using System;

namespace Items
{
    class HeavenAndHell : GunBehaviour
    {
        public static void Add()
        {



            Gun gun = ETGMod.Databases.Items.NewGun("Heaven And Hell", "heaven_and_hell");
            Game.Items.Rename("outdated_gun_mods:heaven_and_hell", "cel:heaven_and_hell");
            gun.gameObject.AddComponent<HeavenAndHell>();
            gun.SetShortDescription("Atom And Evil");
            gun.SetLongDescription("Creates an AoE.\n\nA guitar imbued with the power of molten fire and heavy metal music.");

            gun.SetupSprite(null, "heaven_and_hell_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 24);
            gun.SetAnimationFPS(gun.reloadAnimation, 3);
            gun.AddProjectileModuleFrom("ak-47", true, false);
            gun.sprite.IsPerpendicular = true;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0;
            gun.DefaultModule.cooldownTime = 2f;
            gun.DefaultModule.numberOfShotsInClip = gun.GetBaseMaxAmmo();
            gun.SetBaseMaxAmmo(100);
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            gun.gunClass = GunClass.SILLY;
            gun.encounterTrackable.EncounterGuid = "Heaven and Hell is a real band btw and its super freaking cool so go listen to them.";
            gun.AddToSubShop(ItemBuilder.ShopType.Trorc);
            gun.AddToSubShop(ItemBuilder.ShopType.Cursula);

            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 0;
            projectile.baseData.speed *= 1f;
            projectile.baseData.range *= .00001f;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

        }
        private IEnumerator HandleHeatEffectsCR(float Radius, float Duration, GameActorFireEffect HeatEffect, tk2dBaseSprite Psprite)
        {

            Psprite = m_owner.sprite;
            this.HandleRadialIndicator(TRadius, Psprite);
            float elapsed = 0f;
            RoomHandler r = Psprite.transform.position.GetAbsoluteRoom();
            Vector3 tableCenter = Psprite.WorldCenter.ToVector3ZisY(0f);
            Action<AIActor, float> AuraAction = delegate (AIActor actor, float dist)
            {
                actor.ApplyEffect(THeatEffect, 1f, null);
            };
            while (elapsed < TDuration)
            {
                elapsed += BraveTime.DeltaTime;
                r.ApplyActionToNearbyEnemies(tableCenter.XY(), TRadius, AuraAction);
                yield return null;
            }
            this.UnhandleRadialIndicator();
            yield break;
        }
        private void HandleRadialIndicator(float Radius, tk2dBaseSprite Psprite)
        {
            Psprite = m_owner.sprite;
            if (!this.m_indicator)
            {
                Vector3 position = Psprite.WorldCenter.ToVector3ZisY(0f);
                this.m_indicator = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/HeatIndicator"), position, Quaternion.identity, m_owner.sprite.transform)).GetComponent<HeatIndicatorController>();
                this.m_indicator.CurrentRadius = TRadius;
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

        private float TDuration = 2f;
        private float TRadius = 4.5f;

        private GameActorFireEffect THeatEffect = new GameActorFireEffect()
        {
            IsGreenFire = false,
            AffectsEnemies = true,

            DamagePerSecondToEnemies = 10f,


        };
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
                
            }
        }


        public override void OnPostFired(PlayerController player, Gun gun)
        {

            gun.PreventNormalFireAudio = true;
            tk2dBaseSprite Psprite;
            Psprite = player.sprite;
            Psprite.StartCoroutine(this.HandleHeatEffectsCR(TRadius, TDuration, THeatEffect, Psprite));
        }
        private HeatIndicatorController m_indicator;
        public HeavenAndHell()
        {

        }
    }
}
