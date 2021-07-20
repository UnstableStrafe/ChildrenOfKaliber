using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Gungeon;
using System.Collections;
using Dungeonator;
using GungeonAPI;



namespace Items
{
    class PoisonPoltergeist : RiskPassiveItem
    {
        public static void Init()
        {
            string itemName = "Poison Poltergeist";

            string resourceName = "Items/Resources/poison_poltergeist.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<PoisonPoltergeist>();
            obj.AddComponent<RiskParticles>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Toxic Relationship";
            string longDesc = "Gives a chance to inflict poison on enemies based on your Risk, but makes a poisonous spectre hunt you down. The spectre gains speed based on your Risk.\n\nThis devious little spectre enjoys making mischief around the Gungeon."; 
            //prob just like make a sprite prefab that leaves a poison trail, like PJ, but poison and no contact damage

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            item.RiskToGive = 1;
            item.quality = ItemQuality.B;
            item.sprite.IsPerpendicular = true;
        }
        protected override void Update()
        {
            base.Update();

        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            IntVector2? spawnPosMaybe = player.CurrentRoom.GetRandomVisibleClearSpot(1, 1);
            if (spawnPosMaybe != null)
            {
                poisonGeist = UnityEngine.Object.Instantiate(PoisonGeistEnemy.poisonGeistPrefab, spawnPosMaybe.Value.ToCenterVector2(), Quaternion.Euler(0, 0, 0));
                
            }
            else if (spawnPosMaybe == null)
            {
                ETGModConsole.Log($"How the FUCK did you manage to make spawnPosMaybe null?");
            }
            player.PostProcessProjectile += PostProcess;
            GameManager.Instance.OnNewLevelFullyLoaded += SpawnOnLevelLoad;
        }

        private void SpawnOnLevelLoad()
        {
            GameManager.Instance.StartCoroutine(DelaySpawn());
        }

        private IEnumerator DelaySpawn()
        {
            yield return new WaitForSeconds(4);
            IntVector2? spawnPosMaybe = Owner.CurrentRoom.GetRandomVisibleClearSpot(1, 1);
            if (spawnPosMaybe != null)
            {
                poisonGeist = UnityEngine.Object.Instantiate(PoisonGeistEnemy.poisonGeistPrefab, spawnPosMaybe.Value.ToCenterVector2(), Quaternion.Euler(0, 0, 0));

            }
            else if (spawnPosMaybe == null)
            {
                ETGModConsole.Log($"How the FUCK did you manage to make spawnPosMaybe null?");
            }
            yield break;
        }
        private void PostProcess(Projectile proj, float scalar)
        {
            GameActorHealthEffect effect = (PickupObjectDatabase.GetById(204) as BulletStatusEffectItem).HealthModifierEffect;
            //effect.DamagePerSecondToEnemies *= (Mathf.Clamp((Owner.gameObject.GetOrAddComponent<RiskStat>().RiskAMT * .3f) + 1, 1, 2.5f));
            float chance = Mathf.Clamp(Owner.gameObject.GetOrAddComponent<RiskStat>().RiskAMT * .3f, .3f, 1f);
            if(UnityEngine.Random.value <= chance)
            {
                proj.AppliesPoison = true;
                proj.healthEffect = effect;
                proj.AdjustPlayerProjectileTint(Color.green.WithAlpha(Color.green.a / 2f), 5, 0f);
                proj.PoisonApplyChance = 1;
            }
            
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.PostProcessProjectile -= PostProcess;
            GameManager.Instance.OnNewLevelFullyLoaded -= SpawnOnLevelLoad;
            Destroy(poisonGeist);
            return debrisObject;
        }
        GameObject poisonGeist = new GameObject();
    }
    
}
