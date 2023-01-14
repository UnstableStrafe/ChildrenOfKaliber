using UnityEngine;
using Gungeon;
using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Items
{
    class PhantomPistol : AdvancedGunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Phantom Pistol", "phantom_pistol");
            Game.Items.Rename("outdated_gun_mods:phantom_pistol", "cel:phantom_pistol");
            gun.gameObject.AddComponent<PhantomPistol>();
            gun.SetShortDescription("An Unforseen Death");
            gun.SetLongDescription("Reloading this gun allows you to pull bullets from the past into the future.\n\nLight and compact for easy storage, this modified pistol can shoot bullets it has fired in other timelines.");
            gun.SetupSprite(null, "phantom_pistol_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 10);
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun);
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.MEDIUM_BULLET;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.5f;
            gun.DefaultModule.angleVariance = 5;
            gun.DefaultModule.cooldownTime = .20f;
            gun.DefaultModule.numberOfShotsInClip = 4;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(56) as Gun).gunSwitchGroup;
            gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(56) as Gun).muzzleFlashEffects;
            gun.SetBaseMaxAmmo(350);
            gun.quality = PickupObject.ItemQuality.C;
            gun.encounterTrackable.EncounterGuid = "He's a Phantom / Danny Phantom / He's a Phantom / Danny Phantom, Phantom / Yo Danny Fenton he was just 14 / When his parents built a very strange machine / It was designed to view a world unseen / (He's gonna catch 'em all cause he's Danny Phantom) / When it didn't quite work, his folks they just quit / Then Danny took a look inside of it / There was a great big flash, everything just changed His molecules got all rearranged / (Phantom phantom phantom) / Gonna catch 'em all, gonna catch 'em all / He's a Phantom When he first woke up he realized / He had snow white hair and glowing green eyesHe could walk through walls, disappear, and fly / He was much more unique than the other guys / It was then Danny knew what he had to do / He had to stop all the ghosts that were coming through / He's here to fight for me and you / He's gonna catch 'em all cause he's Danny Phantom / Gonna catch 'em all cause he's Danny Phantom / Gonna catch 'em all cause he's Danny Phantom / Gonna catch 'em all cause he's Danny Phantom / Gonna catch 'em all cause he's Danny Phantom / Gonna catch 'em all cause he's Danny Phantom / Gonna catch 'em all cause he's Danny Phantom / Danny Phantom / Gonna catch 'em all, gonna catch 'em all / Yeah";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.PISTOL;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage *= 1f;
            projectile.baseData.speed *= 1f;
            projectile.baseData.force *= 1f;
            projectile.baseData.range *= .6f;
            projectile.sprite.renderer.enabled = true;
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
        }
        private bool HasReloaded;
        private PlayerController owner;
        public override void PostProcessProjectile(Projectile projectile)
        {
            base.PostProcessProjectile(projectile);
            GameManager.Instance.StartCoroutine(HandleAddToList(projectile));
        }
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
        protected override void OnPickedUpByPlayer(PlayerController player)
        {
            base.OnPickedUpByPlayer(player);
            owner = player;
        }
        protected override void OnPostDroppedByPlayer(PlayerController player)
        {
            base.OnPostDroppedByPlayer(player);
            owner = null;
            storedProjectiles.Clear();
        }
        public override void OnReload(PlayerController player, Gun gun)
        {
            base.OnReload(player, gun);
            if (!DidPhantom)
            {
                player.StartCoroutine(SpawnClones());
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
        public override void OnReloadEnded(PlayerController player, Gun gun)
        {
            base.OnReloadEnded(player, gun);
            DidPhantom = false;
        }
        public override void OnSwitchedAwayFromThisGun()
        {
            storedProjectiles.Clear();
            base.OnSwitchedAwayFromThisGun();
        }
        private IEnumerator SpawnClones()
        {
            DidPhantom = true;
            PlayerController player = this.owner as PlayerController;
            int count = storedProjectiles.Count;
            for (int i = 0; i < count; i++)
            {
                SpawnProjectile(storedProjectiles[i]);
                yield return new WaitForSeconds(this.gun.DefaultModule.cooldownTime / 2);
                yield return null;                
            }
            storedProjectiles.Clear();
            yield break;
        }
        private void SpawnProjectile(ProjAndPositionData data)
        {
            //UnityEngine.Object.Instantiate<GameObject>(TempStorage.GreenLaserCircleVFX, new Vector3(data.position.x, data.position.y), Quaternion.identity);
            GameObject obj = SpawnManager.SpawnProjectile(data.projectile, new Vector3(data.position.x, data.position.y, 0), Quaternion.Euler(0, 0, data.angle));
            Projectile component = obj.GetComponent<Projectile>();
            if (component != null)
            {
                component.Owner = Owner;
                component.Shooter = Owner.specRigidbody;
                component.collidesWithPlayer = false;
                component.sprite.usesOverrideMaterial = true;
                (Owner as PlayerController).DoPostProcessProjectile(component);
                Material material = component.sprite.renderer.material;
                material.shader = ShaderCache.Acquire("Brave/Internal/HologramShader");
                material.SetFloat("_IsGreen", 0f);
            }
        }
        private IEnumerator HandleAddToList(Projectile proj)
        {
            yield return null;
            ProjAndPositionData newData = new ProjAndPositionData();
            newData.projectile = FakePrefab.Clone(proj.gameObject);
            newData.position = proj.specRigidbody.UnitCenter;
            newData.angle = proj.Direction.ToAngle();
            yield return new WaitForSeconds(0.01f);
            storedProjectiles.Add(newData);
            yield break;
        }
        private List<ProjAndPositionData> storedProjectiles = new List<ProjAndPositionData>();
        public class ProjAndPositionData
        {
            public GameObject projectile;
            public Vector2 position;
            public float angle;
        }
        private bool DidPhantom = false;
        public PhantomPistol()
        {

        }
    }
}
