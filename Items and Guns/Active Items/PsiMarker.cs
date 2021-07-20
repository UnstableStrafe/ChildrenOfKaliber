using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using ItemAPI;
using UnityEngine;
using Dungeonator;
using System.Collections;

namespace Items
{
    class PsiMarker : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Psi Marker";

            string resourceName = "Items/Resources/psi_marker.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<PsiMarker>();
            obj.AddComponent<PreventOnActiveEffects>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Eyes On You";
            string longDesc = "While held, summons the Psi Gun to fire upon nearby enemies. Using this switches the targetting mode of the Psi Gun between orbiting the player and orbiting enemies.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 3f);

            item.consumable = false;
            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;
            psiGunProj = UnityEngine.Object.Instantiate<Projectile>(((Gun)ETGMod.Databases.Items[56]).DefaultModule.projectiles[0]);
            psiGunProj.SetProjectileSpriteRight("psi_projectile", 8, 8);

        }
        protected override void DoEffect(PlayerController user)
        {
            base.DoEffect(user);
            if(psiGun != null)
            {
                ToggleTargetting(user);
            }
        }
        private void ToggleTargetting(PlayerController player)
        {
            SpecialOrbital orbital = psiGun.GetComponent<SpecialOrbital>();
            switch (orbital.orbitingMode)
            {
                case SpecialOrbital.OrbitingMode.PLAYER:
                    orbital.orbitingMode = SpecialOrbital.OrbitingMode.ENEMY;
                    break;
                case SpecialOrbital.OrbitingMode.ENEMY:
                    orbital.orbitingMode = SpecialOrbital.OrbitingMode.PLAYER;
                    break;
                case SpecialOrbital.OrbitingMode.CUSTOM:
                    orbital.orbitingMode = SpecialOrbital.OrbitingMode.PLAYER;
                    break;
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            cachedOrbitingMode = SpecialOrbital.OrbitingMode.PLAYER;
            CreateOrbital(player);
        }
        private void CreateOrbital(PlayerController player)
        {
            psiGun = SpriteBuilder.SpriteFromResource("Items/Resources/psi_gun_idle_001.png");
            ItemAPI.FakePrefab.MarkAsFakePrefab(psiGun);
            DontDestroyOnLoad(psiGun);
            tk2dSprite sprite = psiGun.GetComponent<tk2dSprite>();
            sprite.HeightOffGround = .1f;
            sprite.UpdateZDepth();
            sprite.PlaceAtPositionByAnchor(player.CenterPosition, tk2dBaseSprite.Anchor.MiddleCenter);
            SpecialOrbital specialOrbital = psiGun.AddComponent<SpecialOrbital>();
            specialOrbital.owner = player;
            specialOrbital.shouldRotate = true;
            specialOrbital.orbitingMode = cachedOrbitingMode;
            specialOrbital.perfectOrbitFactor = 10;
            specialOrbital.canShoot = true;
            specialOrbital.doesBurstshot = false;
            specialOrbital.burstAmount = 3;
            specialOrbital.timeBetweenBurstShots = .08f;
            specialOrbital.canShootEnemyOrbiter = true;
            specialOrbital.doesReload = false;
            specialOrbital.reloadTime = 1.2f;
            specialOrbital.doesPostProcess = true;
            specialOrbital.fireCooldown = .6f;
            specialOrbital.damageAffectedByPlayerStats = true;
            specialOrbital.cooldownAffectedByPlayerStats = true;
            Projectile proj = Instantiate<Projectile>(psiGunProj);
            specialOrbital.projectile = proj;
            //Fix orbital shooting

        }
        private void DestroyOrbital()
        {
            if(!psiGun)
            {
                return;
            }
            Destroy(psiGun);
            psiGun = null;
        }
        protected override void OnPreDrop(PlayerController user)
        {
            base.OnPreDrop(user);
            DestroyOrbital();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            DestroyOrbital();
        }
        public GameObject psiGun;
        private SpecialOrbital.OrbitingMode cachedOrbitingMode;
        private static Projectile psiGunProj;
    }
    

}
