using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using MonoMod.RuntimeDetour;
using UnityEngine;
using ItemAPI;
using Dungeonator;

namespace Items
{
    class AdvancedGunBehavior : MonoBehaviour
    {
        protected virtual void Update()
        {
            if (this.Player != null)
            {
                this.lastPlayer = this.Player;
                if (!this.everPickedUpByPlayer)
                {
                    this.everPickedUpByPlayer = true;
                }
            }
            if (this.Player != null && !this.pickedUpLast)
            {
                this.OnPickup(this.Player);
                this.pickedUpLast = true;
            }
            if (this.Player == null && this.pickedUpLast)
            {
                if (this.lastPlayer != null)
                {
                    this.OnPostDrop(this.lastPlayer);
                    this.lastPlayer = null;
                }
                this.pickedUpLast = false;
            }
            if (this.gun != null && !this.gun.IsReloading && !this.hasReloaded)
            {
                this.hasReloaded = true;
            }
            this.gun.PreventNormalFireAudio = this.preventNormalFireAudio;
            this.gun.OverrideNormalFireAudioEvent = this.overrrideNormalFireAudio;
        }



        public virtual void Start()
        {
            this.gun = base.GetComponent<Gun>();
            this.gun.OnInitializedWithOwner += this.OnInitializedWithOwner;
            this.gun.PostProcessProjectile += this.PostProcessProjectile;
            this.gun.PostProcessVolley += this.PostProcessVolley;
            this.gun.OnDropped += this.OnDropped;
            this.gun.OnAutoReload += this.OnAutoReload;
            this.gun.OnReloadPressed += this.OnReloadPressed;
            this.gun.OnFinishAttack += this.OnFinishAttack;
            this.gun.OnPostFired += this.OnPostFired;
            this.gun.OnAmmoChanged += this.OnAmmoChanged;
            this.gun.OnBurstContinued += this.OnBurstContinued;
            this.gun.OnPreFireProjectileModifier += this.OnPreFireProjectileModifier;
        }

        public virtual void OnInitializedWithOwner(GameActor actor)
        {
        }

        public virtual void PostProcessProjectile(Projectile projectile)
        {
        }

        public virtual void PostProcessVolley(ProjectileVolleyData volley)
        {
        }

        public virtual void OnDropped()
        {
        }

        public virtual void OnAutoReload(PlayerController player, Gun gun)
        {
        }

        public virtual void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (this.hasReloaded)
            {
                this.OnReload(player, gun);
                this.hasReloaded = false;
            }
        }

        public virtual void OnReload(PlayerController player, Gun gun)
        {
            if (this.preventNormalReloadAudio)
            {
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                if (!string.IsNullOrEmpty(this.overrideNormalReloadAudio))
                {
                    AkSoundEngine.PostEvent(this.overrideNormalReloadAudio, base.gameObject);
                }
            }
        }

        public virtual void OnFinishAttack(PlayerController player, Gun gun)
        {
        }

        public virtual void OnPostFired(PlayerController player, Gun gun)
        {
            if (gun.IsHeroSword)
            {
                if ((float)heroSwordCooldown.GetValue(gun) == 0.5f)
                {
                    this.OnHeroSwordCooldownStarted(player, gun);
                }
            }
        }

        public virtual void OnHeroSwordCooldownStarted(PlayerController player, Gun gun)
        {

        }

        public virtual void OnAmmoChanged(PlayerController player, Gun gun)
        {
        }

        public virtual void OnBurstContinued(PlayerController player, Gun gun)
        {
        }

        public virtual Projectile OnPreFireProjectileModifier(Gun gun, Projectile projectile, ProjectileModule mod)
        {
            return projectile;
        }

        public AdvancedGunBehavior()
        {
        }

        protected virtual void OnPickup(PlayerController player)
        {
        }

        protected virtual void OnPostDrop(PlayerController player)
        {
        }

        public bool PickedUp
        {
            get
            {
                return this.gun.CurrentOwner != null;
            }
        }

        public PlayerController Player
        {
            get
            {
                if (this.gun.CurrentOwner is PlayerController)
                {
                    return this.gun.CurrentOwner as PlayerController;
                }
                return null;
            }
        }

        public float HeroSwordCooldown
        {
            get
            {
                if (this.gun != null)
                {
                    return (float)heroSwordCooldown.GetValue(this.gun);
                }
                return -1f;
            }
            set
            {
                if (this.gun != null)
                {
                    heroSwordCooldown.SetValue(this.gun, value);
                }
            }
        }

        public GameActor Owner
        {
            get
            {
                return this.gun.CurrentOwner;
            }
        }

        public bool OwnerIsPlayer
        {
            get
            {
                return this.Player != null;
            }
        }

        private bool pickedUpLast = false;
        private PlayerController lastPlayer = null;
        public bool everPickedUpByPlayer = false;
        protected Gun gun;
        private bool hasReloaded = true;
        protected bool preventNormalFireAudio;
        protected bool preventNormalReloadAudio;
        protected string overrrideNormalFireAudio;
        protected string overrideNormalReloadAudio;
        private static FieldInfo heroSwordCooldown = typeof(Gun).GetField("HeroSwordCooldown", BindingFlags.NonPublic | BindingFlags.Instance);
    }
}
