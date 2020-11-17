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
        public static NitricAcidHealthEffect NitricAcid = new NitricAcidHealthEffect()
        {
            DamagePerSecondToEnemies = 5f,
            effectIdentifier = "Nitric Acid",
            AffectsEnemies = true,
            resistanceType = EffectResistanceType.Poison,
            duration = 5,
            TintColor = new Color(240 / 110, 230 / 110, 89 / 110),
            AppliesTint = true,
            AppliesDeathTint = true,
        };
        public static GoopDefinition NitricAcidGoop = new GoopDefinition()
        {
            CanBeIgnited = false,
            damagesEnemies = false,
            damagesPlayers = false,
            baseColor32 = new Color32(240, 230, 89, 255),
            goopTexture = ResourceExtractor.GetTextureFromResource("Items/Resources/goop_standard_base_001.png"),
            AppliesDamageOverTime = true,
            HealthModifierEffect = Library.NitricAcid,
        };
        public static CharcoalDustEffect CharcoalDust = new CharcoalDustEffect()
        {
            duration = 6f,
            effectIdentifier = "Charcoal",
            AffectsEnemies = true,
            TintColor = new Color(56 / 100, 59 / 100, 64 / 100),
            AppliesDeathTint = true,
            DeathTintColor = new Color(56 / 85, 59 / 85, 64 / 85),
            AppliesTint = true,       
            resistanceType = EffectResistanceType.Fire,
        };
        public static SulfurFuseEffect SulfurEffect = new SulfurFuseEffect()
        {
            AffectsEnemies = true,
            AffectsPlayers = false,
            duration = 10000000000000000,
            effectIdentifier = "Sulfur",
            AppliesTint = false,
            AppliesDeathTint = false,
            AppliesOutlineTint = true,
            resistanceType = EffectResistanceType.None,
            OutlineTintColor = new Color(252f, 56f, 56f, 50f),
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
        public static int LichEye = 815;
        public static Color LightGreen = (new Color(77f / 140f, 247f / 140f, 122f / 140f));
    }
}
