using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using Gungeon;
using ItemAPI;
using MonoMod;
using MonoMod.RuntimeDetour;
using Random = UnityEngine.Random;
using UnityEngine;
using Dungeonator;

namespace Items
{
    class DragunClaw : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Dragun Claw";

            string resourceName = "Items/Resources/dragun_claw.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<DragunClaw>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Sharper Than Steel";
            string longDesc = "Increases reload speed. Reloading creates flaming oil. Gives fire immunity.\n\nThe claw of the First Dragun, it is covered in a flammable liquid that never seems to run out." +
                "\n\nThe Draguns were not originally from the Gungeon. They came from a fiery world that no longer exists.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ReloadSpeed, 1.10f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.quality = ItemQuality.C;
            item.sprite.IsPerpendicular = true;
            item.CanBeDropped = true;
            item.CanBeSold = true;
            item.AddToSubShop(ItemBuilder.ShopType.Goopton);
        }

        private void Oil(PlayerController player, Gun gun)
        {
            var bundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            var goop = bundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopquickignite.asset");
            var ddgm = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goop);
                ddgm.TimedAddGoopCircle(Owner.sprite.WorldCenter, 3f, .1f);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            WeightedGameObject Object1 = new WeightedGameObject
            {
                pickupId = ETGMod.Databases.Items["Dragun Wing"].PickupObjectId,
                weight = 1.7f,
                rawGameObject = ETGMod.Databases.Items["Dragun Wing"].gameObject,
                forceDuplicatesPossible = false
            };
            WeightedGameObject Object2 = new WeightedGameObject
            {
                pickupId = ETGMod.Databases.Items["Dragun Heart"].PickupObjectId,
                weight = 1.7f,
                rawGameObject = ETGMod.Databases.Items["Dragun Heart"].gameObject,
                forceDuplicatesPossible = false
            };
            WeightedGameObject Object3 = new WeightedGameObject
            {
                pickupId = ETGMod.Databases.Items["Dragun Skull"].PickupObjectId,
                weight = 1.8f,
                rawGameObject = ETGMod.Databases.Items["Dragun Skull"].gameObject,
                forceDuplicatesPossible = false
            };
            if (!player.HasPickupID(ETGMod.Databases.Items["Dragun Wing"].PickupObjectId))
            {
                GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements.Add(Object1);
            }
            if (!player.HasPickupID(ETGMod.Databases.Items["Dragun Heart"].PickupObjectId))
            {
                GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements.Add(Object2);
            }
            if (!player.HasPickupID(ETGMod.Databases.Items["Dragun Skull"].PickupObjectId))
            {
                GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements.Add(Object3);
            }
            this.m_fireImmunity = new DamageTypeModifier();
            this.m_fireImmunity.damageMultiplier = 0f;
            this.m_fireImmunity.damageType = CoreDamageTypes.Fire;
            player.healthHaver.damageTypeModifiers.Add(this.m_fireImmunity);
            player.OnReloadedGun += this.Oil;
            if (player.HasPickupID(ETGMod.Databases.Items["Dragun Skull"].PickupObjectId) && player.HasPickupID(ETGMod.Databases.Items["Dragun Wing"].PickupObjectId) && player.HasPickupID(ETGMod.Databases.Items["Dragun Heart"].PickupObjectId) && !player.HasPickupID(ETGMod.Databases.Items["spirit_of_the_dragun"].PickupObjectId))
            {
                AkSoundEngine.PostEvent("Play_VO_dragun_death_01", gameObject);
                player.inventory.AddGunToInventory((ETGMod.Databases.Items["spirit_of_the_dragun"] as Gun), true);
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnReloadedGun -= this.Oil;
            player.healthHaver.damageTypeModifiers.Remove(this.m_fireImmunity);
            debrisObject.GetComponent<DragunClaw>().m_pickedUpThisRun = true;
            return debrisObject;
        }
        private DamageTypeModifier m_fireImmunity;
        //private int DragunWing = ETGMod.Databases.Items["Dragun Wing"].PickupObjectId;
       // private int DragunHeart = ETGMod.Databases.Items["Dragun Heart"].PickupObjectId;
       // private int DragunSkull = ETGMod.Databases.Items["Dragun Skull"].PickupObjectId;

        
    }
    
}
