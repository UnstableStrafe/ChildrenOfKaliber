using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
namespace Items
{
    public static class Library
    {
        public static GameActorHealthEffect Venom = new GameActorHealthEffect()
        {
            TintColor = new Color(78 / 255f, 5 / 255f, 120 / 255f),
            DeathTintColor = new Color(78 / 255f, 5 / 255f, 120 / 255f),
            AppliesTint = true,
            AppliesDeathTint = true,
            AffectsEnemies = true,
            DamagePerSecondToEnemies = 20f,
            duration = 2.5f,
            effectIdentifier = "Venom",

        };
        public static GoopDefinition VenomGoop = new GoopDefinition()
        {
            CanBeIgnited = false,
            damagesEnemies = false,
            damagesPlayers = false,
            baseColor32 = new Color32(78, 5, 120, 200),
            goopTexture = ResourceExtractor.GetTextureFromResource("Items/Resources/goop_standard_base_001.png"),
            AppliesDamageOverTime = true,
            HealthModifierEffect = Library.Venom,    
        };
        public static Projectile RandomProjectile()
        {
            int gunID;
            Gun gun;
            Projectile proj;
            do
            {
                gun = PickupObjectDatabase.GetRandomGun();
                gunID = gun.PickupObjectId;
            }
            while (gun.HasShootStyle(ProjectileModule.ShootStyle.Beam));
            proj = ((Gun)ETGMod.Databases.Items[gunID]).DefaultModule.projectiles[0];
            return proj;
        }
        public static Color LightGreen = (new Color(77f / 140f, 247f / 140f, 122f / 140f));
    }
}
