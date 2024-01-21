using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;

namespace Items
{
    class TheLastChamber : AdvancedGunBehaviour
    {
        public static int itemID;
        public static void Add()
        {

            Gun gun = ETGMod.Databases.Items.NewGun("The Last Chamber", "the_last_chamber");
            Game.Items.Rename("outdated_gun_mods:the_last_chamber", "ck:the_last_chamber");
            gun.gameObject.AddComponent<TheLastChamber>();
            gun.SetShortDescription("One More In The Chamber");
            gun.SetLongDescription("Kills anything in a single shot. Breaks after one shot. There is no way to prevent it breaking.\n\nA gun that once heavily-wounded Kaliber, destroying her seventh arm. In anger, she cursed the gun and its wielder, " +
                "trapping their soul inside of it. Kaliber's vehement hatred imbues it with great power, but ensures it will break once fired.");
            gun.SetupSprite(null, "the_last_chamber_idle_001", 10);
            gun.SetAnimationFPS(gun.shootAnimation, 18);
            gun.SetAnimationFPS(gun.reloadAnimation, 6);
            gun.AddProjectileModuleFrom("38_special");
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0f;
            gun.DefaultModule.angleVariance = 0f;
            gun.DefaultModule.cooldownTime = 100f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.MEDIUM_BULLET;
            Gun gun2 = PickupObjectDatabase.GetById(32) as Gun;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.SetBaseMaxAmmo(1);
            gun.barrelOffset.transform.localPosition = new Vector3(1.2f, .3f);
            gun.quality = PickupObject.ItemQuality.S;
            gun.encounterTrackable.EncounterGuid = "One More In The Chamber";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.PISTOL;
            gun.InfiniteAmmo = true;
            //this doesnt do anything as it breaks after firing, but it prevents it being duct taped
            Gun gun3 = PickupObjectDatabase.GetById(32) as Gun;
            gun.gunSwitchGroup = gun3.gunSwitchGroup;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.speed *= 2.5f;
            //projectile.baseData.damage = 10000000f;
            projectile.baseData.damage = 0;
            projectile.ignoreDamageCaps = true;
            PierceProjModifier orAddComponent = projectile.gameObject.AddComponent<PierceProjModifier>();
            orAddComponent.penetratesBreakables = true;
            orAddComponent.penetration = 100;
            projectile.shouldRotate = true;
            projectile.SetProjectileSpriteRight("the_last_bullet", 10, 6);
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
            itemID = gun.PickupObjectId;
        }

        public override void PostProcessProjectile(Projectile projectile)
        {
            base.PostProcessProjectile(projectile);
            if(projectile.GetComponent<PierceProjModifier>() != null)
            {
                projectile.GetComponent<PierceProjModifier>().penetration = 0;
            }
            projectile.OnHitEnemy += Hit;
        }
        private void Hit(Projectile proj, SpeculativeRigidbody stupidface, bool fatal)
        {
            if(!fatal && stupidface.healthHaver && stupidface.aiActor)
            {
                GameManager.Instance.StartCoroutine(CauseDeath(stupidface.aiActor, proj.Owner as PlayerController));
            }
        }
        private IEnumerator CauseDeath(AIActor aiActor, PlayerController user)
        {
            //GameManager.Instance.PauseRaw(true);
            
            Pixelator.Instance.DoFinalNonFadedLayer = true;
            //int PreviousLayer = aiActor.gameObject.layer;
            float elapsed = 0f;
            float duration = 0.8f;
            user.healthHaver.IsVulnerable = false;
            user.SetInputOverride("tiddy");
            RadialSlowInterface Rad;
            Rad = new RadialSlowInterface
            {
                RadialSlowHoldTime = 6f,
                RadialSlowOutTime = 2f,
                RadialSlowTimeModifier = 0f,
                DoesSepia = false,
                UpdatesForNewEnemies = true,
                audioEvent = "Play_OBJ_time_bell_01",
            };
            Rad.DoRadialSlow(user.CenterPosition, user.CurrentRoom);
            //aiActor.LocalTimeScale = 0;
            GameObject clockhairObject = Instantiate(BraveResources.Load<GameObject>("Clockhair", ".prefab"));
            ClockhairController clockhair = clockhairObject.GetComponent<ClockhairController>();
            elapsed = 0f;
            duration = clockhair.ClockhairInDuration;
            Vector3 clockhairTargetPosition = aiActor.sprite.WorldCenter;
            Vector3 clockhairStartPosition = clockhairTargetPosition + new Vector3(-20f, 5f, 0f);
            clockhair.renderer.enabled = false;
            clockhair.spriteAnimator.Play("clockhair_intro");
            clockhair.hourAnimator.Play("hour_hand_intro");
            clockhair.minuteAnimator.Play("minute_hand_intro");
            clockhair.secondAnimator.Play("second_hand_intro");
            bool hasWobbled = false;
            while (elapsed < duration)
            {
                if (GameManager.INVARIANT_DELTA_TIME == 0f) { elapsed += 0.05f; }
                elapsed += GameManager.INVARIANT_DELTA_TIME;
                float t2 = elapsed / duration;
                float smoothT = Mathf.SmoothStep(0f, 1f, t2);
                Vector3 currentPosition = Vector3.Slerp(clockhairStartPosition, clockhairTargetPosition, smoothT);
                clockhairObject.transform.position = currentPosition.WithZ(0f);
                if (t2 > 0.5f)
                {
                    clockhair.renderer.enabled = true;
                    clockhair.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                }
                if (t2 > 0.75f)
                {
                    clockhair.hourAnimator.GetComponent<Renderer>().enabled = true;
                    clockhair.minuteAnimator.GetComponent<Renderer>().enabled = true;
                    clockhair.secondAnimator.GetComponent<Renderer>().enabled = true;
                    clockhair.hourAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                    clockhair.minuteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                    clockhair.secondAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                }
                if (!hasWobbled && clockhair.spriteAnimator.CurrentFrame == clockhair.spriteAnimator.CurrentClip.frames.Length - 1)
                {
                    clockhair.spriteAnimator.Play("clockhair_wobble");
                    hasWobbled = true;
                }
                clockhair.sprite.UpdateZDepth();
                //aiActor.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                yield return null;
            }
            if (!hasWobbled) { clockhair.spriteAnimator.Play("clockhair_wobble"); }
            clockhair.SpinToSessionStart(clockhair.ClockhairSpinDuration);
            elapsed = 0f;
            duration = clockhair.ClockhairSpinDuration + clockhair.ClockhairPauseBeforeShot;
            while (elapsed < duration)
            {
                if (GameManager.INVARIANT_DELTA_TIME == 0f) { elapsed += 0.05f; }
                elapsed += GameManager.INVARIANT_DELTA_TIME;
                clockhair.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                yield return null;
            }
            elapsed = 0f;
            duration = 0.1f;
            clockhairStartPosition = clockhairObject.transform.position;
            clockhairTargetPosition = clockhairStartPosition + new Vector3(0f, 12f, 0f);
            clockhair.spriteAnimator.Play("clockhair_fire");
            clockhair.hourAnimator.GetComponent<Renderer>().enabled = false;
            clockhair.minuteAnimator.GetComponent<Renderer>().enabled = false;
            clockhair.secondAnimator.GetComponent<Renderer>().enabled = false;
            while (elapsed < duration)
            {
                if (GameManager.INVARIANT_DELTA_TIME == 0f) { elapsed += 0.05f; }
                elapsed += GameManager.INVARIANT_DELTA_TIME;
                clockhair.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                //aiActor.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                yield return null;
            }
            ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
            for (int l = allProjectiles.Count - 1; l >= 0; l--)
            {
                Projectile projectile = allProjectiles[l];
                if (projectile != null)
                {
                    if (projectile.Owner != null)
                    {
                        if (projectile.Owner is AIActor)
                        {
                            if (projectile.GetComponent<ChainLightningModifier>())
                            {
                                Destroy(projectile.GetComponent<ChainLightningModifier>());
                            }
                            projectile.DieInAir(false, false, false, true);
                        }
                    }
                }
            }
            aiActor.healthHaver.ApplyDamage(10000000f, Vector2.zero, "You Died.", CoreDamageTypes.None, DamageCategory.Unstoppable, false, null, true);
            
            elapsed = 0f;
            duration = 1f;
            while (elapsed < duration)
            {
                if (GameManager.INVARIANT_DELTA_TIME == 0f) { elapsed += 0.05f; }
                elapsed += GameManager.INVARIANT_DELTA_TIME;
                if (clockhair.spriteAnimator.CurrentFrame == clockhair.spriteAnimator.CurrentClip.frames.Length - 1)
                {
                    clockhair.renderer.enabled = false;
                }
                else
                {
                    clockhair.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                }
                //aiActor.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                yield return null;
            }
            
            Pixelator.Instance.FadeToColor(0.25f, Pixelator.Instance.FadeColor, true);
            Pixelator.Instance.LerpToLetterbox(1f, 0.25f);
            Pixelator.Instance.DoFinalNonFadedLayer = false;
            // aiActor.gameObject.SetLayerRecursively(PreviousLayer);
           // GameManager.Instance.ForceUnpause();
            //GameManager.Instance.PreventPausing = false;
            BraveTime.ClearMultiplier(GameManager.Instance.gameObject);
            yield return new WaitForSeconds(1f);
            //GameManager.Instance.PrimaryPlayer.CurrentInputState = PlayerInputState.AllInput;
            //if (GameManager.Instance.SecondaryPlayer) { GameManager.Instance.SecondaryPlayer.CurrentInputState = PlayerInputState.AllInput; }
            //GameManager.Instance.MainCameraController.SetManualControl(false, true);
            user.ClearInputOverride("tiddy");

            Destroy(clockhairObject);
            
            user.healthHaver.IsVulnerable = true;
            yield break;
        }
        private bool HasReloaded;

        protected override void Update()
        {
            if (gun.CurrentOwner)
            {

                if (!gun.PreventNormalFireAudio)
                {
                    this.gun.PreventNormalFireAudio = true;
                }
                if (!gun.IsReloading && !HasReloaded)
                {
                    this.HasReloaded = true;
                }
            }
        }
        private IEnumerator InvariantWait(float delay, bool forceAnimationUpdate = true)
        {
            float elapsed = 0f;
            while (elapsed < delay)
            {
                if (GameManager.INVARIANT_DELTA_TIME == 0f) { elapsed += 0.05f; }
                elapsed += GameManager.INVARIANT_DELTA_TIME;
                yield return null;
            }
            yield break;
        }
        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && this.HasReloaded)
            {
                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_WPN_rpg_reload_01", base.gameObject);
            }
        }
        

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_OBJ_bloodybullet_proc_01", gameObject);
            player.inventory.DestroyGun(gun);
        }
        public TheLastChamber()
        {

        }
    }
}
