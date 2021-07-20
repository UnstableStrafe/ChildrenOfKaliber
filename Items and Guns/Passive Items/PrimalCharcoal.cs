using System;
using System.Collections;
using ItemAPI;
using UnityEngine;
using Dungeonator;
namespace Items
{
    class PrimalCharcoal : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Primal Charcoal";

            string resourceName = "Items/Resources/primal_charcoal.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<PrimalCharcoal>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Not Quite Asbestos";
            string longDesc = "Reloading an empty clip creates a cloud of charcoal dust around the player, causing nearby enemies to take more damage.\n\nWood from the forests that once surrounded the Gungeon, charred by the power that awakened the Gungeon to what it is now. Some of that power is stored within it.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.B;
            item.sprite.IsPerpendicular = true;
        }
        private void DustCloud(PlayerController player, Gun gun)
        {
            if(gun.ClipShotsRemaining == 0)
            {
                player.sprite.StartCoroutine(this.HandleHeatEffectsCR(6.5f, 2, Library.CharcoalDust, player.sprite));
            }
        }
        private IEnumerator HandleHeatEffectsCR(float Radius, float Duration, CharcoalDustEffect charcoalDust, tk2dBaseSprite Psprite)
        {

            Psprite = Owner.sprite;
            this.HandleRadialIndicator(6.5f, Psprite);
            float elapsed = 0f;
            RoomHandler r = Psprite.transform.position.GetAbsoluteRoom();
            Vector3 tableCenter = Psprite.WorldCenter.ToVector3ZisY(0f);
            Action<AIActor, float> AuraAction = delegate (AIActor actor, float dist)
            {
                actor.ApplyEffect(Library.CharcoalDust, 1f, null);
            };
            while (elapsed < 2)
            {
                elapsed += BraveTime.DeltaTime;
                r.ApplyActionToNearbyEnemies(tableCenter.XY(), 6.5f, AuraAction);
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
                this.m_indicator.CurrentRadius = 6.5f;
                this.m_indicator.CurrentColor = new Color(56 / 100, 59 / 100, 64 / 100);

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
        private HeatIndicatorController m_indicator;
        public override void Pickup(PlayerController player)
        {
            player.OnReloadedGun += DustCloud;
            WeightedGameObject Object1 = new WeightedGameObject
            {
                pickupId = ETGMod.Databases.Items["Primal Sulfur"].PickupObjectId,
                weight = 1.7f,
                rawGameObject = ETGMod.Databases.Items["Primal Sulfur"].gameObject,
                forceDuplicatesPossible = false
            };
            WeightedGameObject Object2 = new WeightedGameObject
            {
                pickupId = ETGMod.Databases.Items["Primal Saltpeter"].PickupObjectId,
                weight = 1.7f,
                rawGameObject = ETGMod.Databases.Items["Primal Saltpeter"].gameObject,
                forceDuplicatesPossible = false
            };
            WeightedGameObject Object3 = new WeightedGameObject
            {
                pickupId = ETGMod.Databases.Items["Primal Nitric Acid"].PickupObjectId,
                weight = 1.8f,
                rawGameObject = ETGMod.Databases.Items["Primal Nitric Acid"].gameObject,
                forceDuplicatesPossible = false
            };
            if (!player.HasPickupID(ETGMod.Databases.Items["Primal Sulfur"].PickupObjectId))
            {
                GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements.Add(Object1);
            }
            if (!player.HasPickupID(ETGMod.Databases.Items["Primal Saltpeter"].PickupObjectId))
            {
                GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements.Add(Object2);
            }
            if (!player.HasPickupID(ETGMod.Databases.Items["Primal Nitric Acid"].PickupObjectId))
            {
                GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements.Add(Object3);
            }
            if (player.HasPickupID(ETGMod.Databases.Items["Primal Saltpeter"].PickupObjectId) && player.HasPickupID(ETGMod.Databases.Items["Primal Nitric Acid"].PickupObjectId) && player.HasPickupID(ETGMod.Databases.Items["Primal Sulfur"].PickupObjectId) && !player.HasPickupID(ETGMod.Databases.Items["True Gunpowder"].PickupObjectId))
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(ETGMod.Databases.Items["True Gunpowder"].PickupObjectId).gameObject, player);
            }
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<PrimalCharcoal>().m_pickedUpThisRun = true;
            player.OnReloadedGun -= DustCloud;
            return debrisObject;
        }
    }
}
