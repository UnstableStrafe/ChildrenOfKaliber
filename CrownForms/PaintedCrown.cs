using System;
using System.Collections.Generic;
using ItemAPI;
using System.Collections;
using UnityEngine;
using Gungeon;
using Dungeonator;

namespace Items
{
    class PaintedCrown : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Painted Crown";

            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<PaintedCrown>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Splatfest";
            string longDesc = "Imbues the player's projectiles with random goops. Reloading creates pools of random goops.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;

            possibleGoops.Add(Library.VenomGoop);
            possibleGoops.Add(Library.NitricAcidGoop);
            foreach (GoopDefinition goop in Library.goopDefs)
            {
                if (goop != Library.goopDefs[3])
                {
                    possibleGoops.Add(goop);
                }
            }
        }
        private void GoopReload(PlayerController player, Gun gun)
        {
            if(gun.ClipShotsRemaining == 0)
            {
                int amount = UnityEngine.Random.Range(5, 12);
                
                for (int i = 0; i < amount; i++)
                {
                    float size = UnityEngine.Random.Range(1.5f, 4);
                    float lifetime = UnityEngine.Random.Range(.2f, .4f);
                    Vector2 location = player.sprite.WorldCenter + (UnityEngine.Random.insideUnitCircle * 7);
                    GoopDefinition goop = possibleGoops[UnityEngine.Random.Range(0, possibleGoops.Count)];
                    DeadlyDeadlyGoopManager ddgm = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goop);
                    ddgm.TimedAddGoopCircle(location, size, lifetime);
                    if (goop == Library.goopDefs[5])
                    {
                        ddgm.ElectrifyGoopCircle(player.sprite.WorldCenter, size);

                    }
                }
            }   
        }
        private void GoopProj(Projectile proj, float eff) 
        {
            ProjectileGoopTrailDoer trailDoer = proj.gameObject.AddComponent<ProjectileGoopTrailDoer>();
            trailDoer.goopDefinition = possibleGoops[UnityEngine.Random.Range(0, possibleGoops.Count)];
            
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnReloadedGun += this.GoopReload;
            this.m_fireImmunity.damageMultiplier = 0f;
            this.m_fireImmunity.damageType = CoreDamageTypes.Fire;
            player.healthHaver.damageTypeModifiers.Add(this.m_fireImmunity);
            this.poisonImmunity.damageMultiplier = 0f;
            this.poisonImmunity.damageType = CoreDamageTypes.Poison;
            player.healthHaver.damageTypeModifiers.Add(this.poisonImmunity);
            this.electricImmunity.damageMultiplier = 0f;
            this.electricImmunity.damageType = CoreDamageTypes.Electric;
            player.healthHaver.damageTypeModifiers.Add(this.electricImmunity);
            player.PostProcessProjectile += GoopProj;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<PaintedCrown>().m_pickedUpThisRun = true;
            player.OnReloadedGun -= this.GoopReload;
            player.healthHaver.damageTypeModifiers.Remove(this.m_fireImmunity);
            player.healthHaver.damageTypeModifiers.Remove(this.poisonImmunity);
            player.healthHaver.damageTypeModifiers.Remove(this.electricImmunity);
            player.PostProcessProjectile -= GoopProj;
            return debrisObject;
        }
        private DamageTypeModifier m_fireImmunity = new DamageTypeModifier();
        private DamageTypeModifier electricImmunity = new DamageTypeModifier();
        private DamageTypeModifier poisonImmunity = new DamageTypeModifier();
        private static List<GoopDefinition> possibleGoops = new List<GoopDefinition> { };

    }
    public class ProjectileGoopTrailDoer : MonoBehaviour
    {

        private void Start()
        {
            proj = base.GetComponent<Projectile>();
            proj.OnDestruction += BigGoop;
            
        }
        private void Update()
        {
            if(proj != null)
            {
                if(timer < .008f)
                {
                    timer += BraveTime.DeltaTime;
                }
                if(timer >= .008f)
                {
                    timer = 0;
                    DeadlyDeadlyGoopManager ddgm = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
                    ddgm.TimedAddGoopCircle(proj.sprite.WorldCenter, .6f, .3f);
                    if(goopDefinition == Library.goopDefs[5])
                    {
                        ddgm.ElectrifyGoopCircle(proj.sprite.WorldCenter, .6f);
                        
                    }
                }

            }
        }
        //make a rebar/impaler crossbow that pins enemies to walls
        private void BigGoop(Projectile proj)
        {
            DeadlyDeadlyGoopManager ddgm = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
            ddgm.TimedAddGoopCircle(proj.sprite.WorldCenter, 1.25f, .3f);
            if (goopDefinition == Library.goopDefs[5])
            {
                ddgm.ElectrifyGoopCircle(proj.LastPosition, 1.25f);

            }
        }
        private float timer;
        private Projectile proj;
        public GoopDefinition goopDefinition;

    }
}
