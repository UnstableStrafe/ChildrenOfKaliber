using System.Collections.Generic;
using System.Linq;
using Alexandria.ItemAPI;
using UnityEngine;

namespace Items
{
    class DwarvenHammer : PlayerItem
    {
        private float ScrapCount = 0;
        private static string Rewards = "1 scrapped gun: 1 armor\n2 scrapped guns: 4 glass guon stones\n3 scrapped guns: Spawns 2 ammo crates\n4 guns scrapped: 3 armor\n5 scrapped guns: gives 1 of 3 powerful guns\n6+ guns scrapped: 1 armor and 2 glass guon stones";


        public static void Init()
        {
            string itemName = "Dwarven Hammer";

            string resourceName = "Items/Resources/dwarven_hammer.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<DwarvenHammer>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Scrap Metal";
            string longDesc = "Destroys currently held gun and gives a reward based on amount of scrapped guns. \n\nHave too many guns and you don't know which one to use? " +
                "Just rip them apart and glue them together into a horrible amalgamation of metal and gunpowder!\n\nRewards:\n" + Rewards;

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 2);

            item.consumable = false;
            item.quality = ItemQuality.SPECIAL;

        }

        public override void Pickup(PlayerController player)
        {
            if (this.m_pickedUp)
            {
                return;
            }
            base.Pickup(player);

        }


        public override void DoEffect(PlayerController user)
        {

            ScrapCount += 1;

            AkSoundEngine.PostEvent("Play_OBJ_chest_open_01", base.gameObject);

            user.CurrentGun.ammo = 0;
            user.inventory.DestroyCurrentGun();
           
            if (ScrapCount == 1)
            {

                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, user);
             //   LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(821).gameObject, user);

            }

            if (ScrapCount == 2)
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(565).gameObject, user);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(565).gameObject, user);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(565).gameObject, user);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(565).gameObject, user);
            }

            if (ScrapCount == 3)
            {
                PickupObject byId = PickupObjectDatabase.GetById(78);
                LootEngine.SpawnItem(byId.gameObject, user.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
                LootEngine.SpawnItem(byId.gameObject, user.specRigidbody.UnitCenter, Vector2.up, 1.5f, false, true, false);
            }

            if (ScrapCount == 4)
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, user);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, user);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, user);
            }

            if (ScrapCount == 5)
            {
                if(this.GunRando == FiGun.NONE)
                {
                    Gun fallout = PickupObjectDatabase.GetByName(this.falloutGun) as Gun;
                    Gun tesla = PickupObjectDatabase.GetByName(this.teslaGun) as Gun;
                    Gun phazor = PickupObjectDatabase.GetByName(this.phazorGun) as Gun;

                    this.GunRando = (DwarvenHammer.FiGun)UnityEngine.Random.Range(1, 4);

                    if(this.GunRando == FiGun.TESLA)
                    {
                        this.m_player.inventory.AddGunToInventory(tesla, true);
                    }
                    if(this.GunRando == FiGun.PHAZOR)
                    {
                        this.m_player.inventory.AddGunToInventory(phazor, true);
                    }
                    if(GunRando == FiGun.FALLOUT)
                    {
                        this.m_player.inventory.AddGunToInventory(fallout, true);
                    }
                }
                
            }
            if(ScrapCount >= 6)
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, user);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(565).gameObject, user);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(565).gameObject, user);
            }
        }

        public override bool CanBeUsed(PlayerController user)
        {
            return user.CurrentGun.InfiniteAmmo == false;
        }

        private DwarvenHammer.FiGun GunRando;
        public enum FiGun
        {
            NONE,

            TESLA,// animate fire at 21fps

            PHAZOR, //animate idle at 13fps

            FALLOUT, 

            


        }

        private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
        {
            foreach (var m in passiveStatModifiers)
            {
                if (m.statToBoost == statType) return;
            }

            StatModifier modifier = new StatModifier
            {
                amount = amount,
                statToBoost = statType,
                modifyType = method
            };

            if (this.passiveStatModifiers == null)
                this.passiveStatModifiers = new StatModifier[] { modifier };
            else
                this.passiveStatModifiers = this.passiveStatModifiers.Concat(new StatModifier[] { modifier }).ToArray();
        }

        private void RemoveStat(PlayerStats.StatType statType)
        {
            var newModifiers = new List<StatModifier>();
            for (int i = 0; i < passiveStatModifiers.Length; i++)
            {
                if (passiveStatModifiers[i].statToBoost != statType)
                    newModifiers.Add(passiveStatModifiers[i]);
            }
            this.passiveStatModifiers = newModifiers.ToArray();
        }

        private string falloutGun = "cel:fallout";
        private string teslaGun = "cel:tesla";
        private string phazorGun = "cel:phazor";

        private PlayerController m_player;
    }
    


}

