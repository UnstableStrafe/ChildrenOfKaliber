using Alexandria.ItemAPI;
using Random = UnityEngine.Random;
using UnityEngine;


namespace Items
{
    class RGB : PassiveItem
    {
        public static float RGB_DMG;
        public static float RGB_ROF;
        public static float RGB_ACC;
        public static float RGB_CLP;
        public static int RGB_BNC;
        public static int RGB_PRC;
        public static float RGB_AMM;
        public static float RGB_CHR;
        public static float RGB_DMGBSS;
        public static float RGB_KBM;
        public static float RGB_SCL;
        public static float RGB_RSP;
        public static void RGBStatsRandomize()
        {
            RGB_ACC = Random.Range(0.30f, 0.90f);
            RGB_AMM = Random.Range(1.15f, 1.35f);
            RGB_BNC = Random.Range(0, 3);
            RGB_CHR = Random.Range(1.10f, 1.25f);
            RGB_CLP = Random.Range(1.20f, 1.45f);
            RGB_DMG = Random.Range(1.10f, 1.25f);
            RGB_DMGBSS = Random.Range(1.20f, 1.35f);
            RGB_KBM = Random.Range(1.10f, 1.30f);
            RGB_SCL = Random.Range(1.25f, 1.4f);
            RGB_ROF = Random.Range(1.10f, 1.25f);
            RGB_RSP = Random.Range(.65f, .9f);
        }
        public static void Init()
        {

            string itemName = "R.G.B.";
            string resourceName = "Items/Resources/ddr.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<RGB>();


            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "You're Welcome";
            string longDesc = "Provides random stat increases each run.\n\nThese bullets were coded to fit into a 3D game, not a 2D one and are prone to changing. It is odd they even work in Enter The Gungeon at all...";
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Accuracy, RGB_ACC, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalClipCapacityMultiplier, RGB_CLP, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalShotBounces, RGB_BNC, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalShotPiercing, RGB_PRC, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AmmoCapacityMultiplier, RGB_AMM, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ChargeAmountMultiplier, RGB_CHR, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, RGB_DMG, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.DamageToBosses, RGB_DMGBSS, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.KnockbackMultiplier, RGB_KBM, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.PlayerBulletScale, RGB_SCL, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.RateOfFire, RGB_ROF, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ReloadSpeed, RGB_RSP, StatModifier.ModifyMethod.MULTIPLICATIVE);

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            item.quality = PickupObject.ItemQuality.D;
            item.sprite.IsPerpendicular = true;
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
            Material material = item.sprite.renderer.material;
            material.shader = ShaderCache.Acquire("Brave/Internal/Glitch");
            material.SetFloat("_GlitchInterval", 0.05f);
            material.SetFloat("_DispProbability", 0.4f);
            material.SetFloat("_DispIntensity", 0.04f);
            material.SetFloat("_ColorProbability", 0.4f);
            material.SetFloat("_ColorIntensity", 0.04f);

        }

        
    }
}
