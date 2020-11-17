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
    class DragunHeart : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Dragun Heart";

            string resourceName = "Items/Resources/dragun_heart.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<DragunHeart>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Burns Any Who Touch";
            string longDesc = "Increases health. Taking damage creates of pool of flaming oil. Gives fire immunity.\n\nThis Dragun heart feels warm in your hands, despite no longer beating." +
                "\n\nWhen the Dragun champion killed the Past, thereby preventing the death of his homeworld, he was mortified to find that the event occured regardless of if he intervened. He tried over and over to no avail.";
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 1f, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.C;
            item.sprite.IsPerpendicular = true;
            item.CanBeDropped = true;
            item.CanBeSold = true;
            item.AddToSubShop(ItemBuilder.ShopType.Goopton);
        }

        private void FireBurst(PlayerController player)
        {
            var bundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            var goop = bundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopquickignite.asset");
            var ddgm = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goop);
            ddgm.TimedAddGoopCircle(Owner.sprite.WorldCenter, 8f, .2f);

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
                pickupId = ETGMod.Databases.Items["Dragun Claw"].PickupObjectId,
                weight = 1.7f,
                rawGameObject = ETGMod.Databases.Items["Dragun Claw"].gameObject,
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
            if (!player.HasPickupID(ETGMod.Databases.Items["Dragun Claw"].PickupObjectId))
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
            player.OnReceivedDamage += this.FireBurst;
            if (player.HasPickupID(ETGMod.Databases.Items["Dragun Skull"].PickupObjectId) && player.HasPickupID(ETGMod.Databases.Items["Dragun Wing"].PickupObjectId) && player.HasPickupID(ETGMod.Databases.Items["Dragun Claw"].PickupObjectId) && !player.HasPickupID(ETGMod.Databases.Items["spirit_of_the_dragun"].PickupObjectId))
            {
                AkSoundEngine.PostEvent("Play_VO_dragun_death_01", gameObject);
                player.inventory.AddGunToInventory((ETGMod.Databases.Items["spirit_of_the_dragun"] as Gun), true);
            }
        }



        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnReceivedDamage -= this.FireBurst;

            player.healthHaver.damageTypeModifiers.Remove(this.m_fireImmunity);
            debrisObject.GetComponent<DragunHeart>().m_pickedUpThisRun = true;
            return debrisObject;
        }
        private DamageTypeModifier m_fireImmunity;
        //private int DragunWing = ETGMod.Databases.Items["Dragun Wing"].PickupObjectId;
       // private int DragunClaw = ETGMod.Databases.Items["Dragun Claw"].PickupObjectId;
       // private int DragunSkull = ETGMod.Databases.Items["Dragun Skull"].PickupObjectId;
        
    }

}
