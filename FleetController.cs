using System;
using UnityEngine;
using Dungeonator;

namespace Items
{
    class FleetController : MonoBehaviour
    {
        public FleetController()
        {
            this.projectileToSpawn = null;
        }
        private void Awake()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            this.speculativeRigidBoy = base.GetComponent<SpeculativeRigidbody>();
        }
        private void Update()
        {

            bool flag = this.m_projectile == null;
            if (flag)
            {
                this.m_projectile = base.GetComponent<Projectile>();
            }
            bool flag2 = this.speculativeRigidBoy == null;
            if (flag2)
            {
                this.speculativeRigidBoy = base.GetComponent<SpeculativeRigidbody>();
            }
            this.elapsed += BraveTime.DeltaTime;
            bool flag3 = this.elapsed > 0.15f;
            if(player.HasPickupID(529) || player.HasPickupID(135) && player.HasPickupID(ETGMod.Databases.Items["Assault Fleet"].PickupObjectId))
            {
                flag3 = this.elapsed > .10f;
            }
            if (flag3)
            {
                Vector2 dirVec = UnityEngine.Random.insideUnitCircle;

                Func<AIActor, bool> isValid = (AIActor a) => a && a.HasBeenEngaged && a.healthHaver && a.healthHaver.IsVulnerable;
                AIActor closestToPosition = BraveUtility.GetClosestToPosition<AIActor>(player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All), m_projectile.sprite.WorldCenter, isValid);
                if (closestToPosition)
                {
                    this.spawnAngle = Vector2.Angle(m_projectile.sprite.WorldCenter, closestToPosition.sprite.WorldCenter);
                }
                
                this.SpawnProjectile(this.projectileToSpawn, this.m_projectile.sprite.WorldCenter, this.m_projectile.transform.eulerAngles.z + this.spawnAngle, null);
                this.elapsed = 0f;
            }
        }
        private void SpawnProjectile(Projectile proj, Vector3 spawnPosition, float zRotation, SpeculativeRigidbody collidedRigidbody = null)
        {
            GameObject gameObject = SpawnManager.SpawnProjectile(proj.gameObject, spawnPosition, Quaternion.Euler(0f, 0f, zRotation), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            bool flag = component;
            if (flag)
            {
                component.SpawnedFromOtherPlayerProjectile = true;
                PlayerController playerController = this.m_projectile.Owner as PlayerController;
                component.baseData.damage *= playerController.stats.GetStatValue(PlayerStats.StatType.Damage);
                component.baseData.speed *= playerController.stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed);
                playerController.DoPostProcessProjectile(component);
            }
        }
        private float spawnAngle = 0f;

        private Projectile m_projectile;

        private SpeculativeRigidbody speculativeRigidBoy;

        public Projectile projectileToSpawn;

        private float elapsed;

        private PlayerController player;
    }
}
