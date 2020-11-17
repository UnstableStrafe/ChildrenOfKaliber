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
    class DragunWing : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Dragun Wing";

            string resourceName = "Items/Resources/dragun_wing.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<DragunWing>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Where Eagles Dare";
            string longDesc = "Increases movement speed. Killing an enemy creates a ring of fire around the player.\n\nThe wing of an elderly Dragun that is too weak to allow for flight anymore. It glows in the presence of gunfire." +
                "\n\nRight before their home was destroyed, the Draguns sent their bravest warrior to the Gungeon in an attempt to prevent the onset of the apocolypse.";
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MovementSpeed, 1.15f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            
            item.quality = ItemQuality.B;
            item.sprite.IsPerpendicular = true;
            item.CanBeDropped = true;
            item.CanBeSold = true;
        }

        public void OnKill(PlayerController player)
        {
            if (!Owner.CurrentGun.InfiniteAmmo)
            {
                if (this.m_indicator)
                {
                    return;
                }
                tk2dBaseSprite Psprite;
                Psprite = player.sprite;
                Psprite.StartCoroutine(this.HandleHeatEffectsCR(TRadius, TDuration, THeatEffect, Psprite));
            }
        }
        private IEnumerator HandleHeatEffectsCR(float Radius, float Duration, GameActorFireEffect HeatEffect, tk2dBaseSprite Psprite)
        {

            Psprite = Owner.sprite;
            this.HandleRadialIndicator(TRadius, Psprite);
            float elapsed = 0f;
            RoomHandler r = Psprite.transform.position.GetAbsoluteRoom();
            Vector3 tableCenter = Psprite.WorldCenter.ToVector3ZisY(0f);
            Action<AIActor, float> AuraAction = delegate (AIActor actor, float dist)
            {
                actor.ApplyEffect(THeatEffect, 1f, null);
            };
            while (elapsed < TDuration)
            {
                elapsed += BraveTime.DeltaTime;
                r.ApplyActionToNearbyEnemies(tableCenter.XY(), TRadius, AuraAction);
                yield return null;
            }
            this.UnhandleRadialIndicator();
            yield break;
        }
        private void HandleRadialIndicator(float Radius, tk2dBaseSprite Psprite)
        {
            Psprite = Owner.sprite;
            if (!this.m_indicator)
            {
                Vector3 position = Psprite.WorldCenter.ToVector3ZisY(0f);
                this.m_indicator = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/HeatIndicator"), position, Quaternion.identity, sprite.transform)).GetComponent<HeatIndicatorController>();
                this.m_indicator.CurrentRadius = TRadius;
                
            }
        }
        private void UnhandleRadialIndicator()
        {
            if (this.m_indicator)
            {
                this.m_indicator.EndEffect();
                this.m_indicator = null;
            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnKilledEnemy += this.OnKill;
            WeightedGameObject Object1 = new WeightedGameObject
            {
                pickupId = ETGMod.Databases.Items["Dragun Heart"].PickupObjectId,
                weight = 1.7f,
                rawGameObject = ETGMod.Databases.Items["Dragun Heart"].gameObject,
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
            if (!player.HasPickupID(ETGMod.Databases.Items["Dragun Heart"].PickupObjectId))
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
            if (player.HasPickupID(ETGMod.Databases.Items["Dragun Heart"].PickupObjectId) && player.HasPickupID(ETGMod.Databases.Items["Dragun Skull"].PickupObjectId) && player.HasPickupID(ETGMod.Databases.Items["Dragun Claw"].PickupObjectId) && !player.HasPickupID(ETGMod.Databases.Items["spirit_of_the_dragun"].PickupObjectId))
            {
                AkSoundEngine.PostEvent("Play_VO_dragun_death_01", gameObject);
                player.inventory.AddGunToInventory((ETGMod.Databases.Items["spirit_of_the_dragun"] as Gun), true);
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnKilledEnemy -= this.OnKill;
            debrisObject.GetComponent<DragunWing>().m_pickedUpThisRun = true;
            return debrisObject;
        }
        
        private float TDuration = 8f;
        private float TRadius = 3.5f;

        private GameActorFireEffect THeatEffect = new GameActorFireEffect()
        {
                IsGreenFire = false,
                AffectsEnemies = true,
                
                DamagePerSecondToEnemies = 10f,


        };
        private HeatIndicatorController m_indicator;
       // private int DragunClaw = ETGMod.Databases.Items["Dragun Claw"].PickupObjectId;
       // private int DragunHeart = ETGMod.Databases.Items["Dragun Heart"].PickupObjectId;
       // private int DragunSkull = ETGMod.Databases.Items["Dragun Skull"].PickupObjectId;
        
    }
}
