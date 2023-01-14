using System;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using Alexandria.DungeonAPI;
using Alexandria.ItemAPI;
using UnityEngine;

namespace Items
{
    class PrimeArmsItem : CompanionItem
    {
        public static void Init()
        {
            string itemName = "The Prime Core";

            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<PrimeArmsItem>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "";
            string longDesc = "";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;


        }
        public override void Pickup(PlayerController player)
        {

            base.Pickup(player);

        }
        
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<PrimeArmsItem>().m_pickedUpThisRun = true;

            return debrisObject;
        }

        //Prime Cannon \/
        public static GameObject PrimeCannonPrefab;
        public static readonly string cannonGuid = "prime_cannon";
        private static string[] spritePathsCannon = new string[]
        {
            "Items/Resources/Prime Arms/prime_cannon_idle_001.png"
        };
        //Prime Laser \/
        public static GameObject PrimeLaserPrefab;
        public static readonly string laserGuid = "prime_laser";
        private static string[] spritePathsLaser = new string[]
        {
            "Items/Resources/Prime Arms/prime_laser_idle_001.png"
        };
        //Prime Vice \/
        public static GameObject PrimeVicePrefab;
        public static readonly string viceGuid = "prime_vice";
        private static string[] spritePathsVice = new string[]
        {
            "Items/Resources/Prime Arms/prime_vice_idle_001.png",
        };
        //Prime Saw \/
        public static GameObject PrimeSawPrefab;
        public static readonly string sawGuid = "prime_saw";
        private static string[] spritePathsSaw = new string[]
        {
            "Items/Resources/Prime Arms/prime_saw_idle_001.png",
        };

    }
}
