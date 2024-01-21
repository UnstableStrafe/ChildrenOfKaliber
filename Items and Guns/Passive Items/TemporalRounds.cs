using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.ItemAPI;
using Gungeon;
using Dungeonator;
using UnityEngine;
using System.Collections;

namespace Items
{
    class TemporalRounds : PassiveItem
    {
        public static int itemID;
        public static void Init()
        {
            string itemName = "Temporal Rounds";

            string resourceName = "Items/Resources/ItemSprites/Passives/temporal_rounds.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<TemporalRounds>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Relics Of The Past";
            string longDesc = "Summons some projectiles from the past to aid you in the present.\n\nThe Pursued's temporal control tech, embeded into bullets.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ck");

            item.quality = ItemQuality.S;
            item.sprite.IsPerpendicular = true;
            itemID = item.PickupObjectId;

        }
        public override void Pickup(PlayerController player)
        {
            storedProjectiles.Clear();
            player.PostProcessProjectile += StoreProjectile;
            player.OnRoomClearEvent += ClearStoredProjectilesOnRoomClear;
            player.OnReloadedGun += HandleReload;
            base.Pickup(player);

        }

        private void HandleReload(PlayerController player, Gun gun)
        {
            GameManager.Instance.StartCoroutine(SpawnClones(player));
        }
        private IEnumerator SpawnClones(PlayerController player)
        {
            
            int count = storedProjectiles.Count;
            for (int i = 0; i < count; i++)
            {
                SpawnProjectile(storedProjectiles[i]);
                yield return new WaitForSeconds(.08f);
                yield return null;
            }
            
            yield break;
        }
        private void SpawnProjectile(ProjAndPositionData data)
        {
            //UnityEngine.Object.Instantiate<GameObject>(TempStorage.GreenLaserCircleVFX, new Vector3(data.position.x, data.position.y), Quaternion.identity);
            GameObject obj = SpawnManager.SpawnProjectile(data.projectile, new Vector3(data.position.x, data.position.y, 0), Quaternion.Euler(0, 0, data.angle));
            Projectile component = obj.GetComponent<Projectile>();
            if (component != null)
            {
                component.Owner = Owner;
                component.Shooter = Owner.specRigidbody;
                component.collidesWithPlayer = false;
                component.sprite.usesOverrideMaterial = true;
                Material material = component.sprite.renderer.material;
                material.shader = ShaderCache.Acquire("Brave/Internal/HologramShader");
                material.SetFloat("_IsGreen", 0f);
            }
        }
        private void ClearStoredProjectilesOnRoomClear(PlayerController player)
        {
            storedProjectiles.Clear();
        }

        private void StoreProjectile(Projectile proj, float eff)
        {
            float chance = .15f;
            float trueChance = chance * eff;
            if(UnityEngine.Random.value <= trueChance)
            {
                GameManager.Instance.StartCoroutine(HandleAddToList(proj));
            }

        }
        private IEnumerator HandleAddToList(Projectile proj)
        {
            yield return null;
            ProjAndPositionData newData = new ProjAndPositionData();
            newData.projectile = FakePrefab.Clone(proj.gameObject);
            newData.position = proj.specRigidbody.UnitCenter;
            newData.angle = proj.Direction.ToAngle();
            yield return new WaitForSeconds(0.01f);
            storedProjectiles.Add(newData);
            yield break;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<TemporalRounds>().m_pickedUpThisRun = true;
            player.PostProcessProjectile -= StoreProjectile;
            player.OnRoomClearEvent -= ClearStoredProjectilesOnRoomClear;
            player.OnReloadedGun -= HandleReload;
            storedProjectiles.Clear();
            return debrisObject;
        }

        private List<ProjAndPositionData> storedProjectiles = new List<ProjAndPositionData>();
        public class ProjAndPositionData
        {
            public GameObject projectile;
            public Vector2 position;
            public float angle;
        }
    }
}
