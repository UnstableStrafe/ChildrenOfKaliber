using Alexandria.ItemAPI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Items
{
    class Sawblade : GunVolleyModificationItem
    {

        public static void Init()
        {

            string itemName = "Sawblade";
            string resourceName = "Items/Resources/ItemSprites/Passives/sawblade.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<Sawblade>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);


            string shortDesc = "Covered in Blood...";
            string longDesc = "Chance to shoot a sawblade with each shot.\n\n.These sawblades hunger for brutality. Whoever owned them last put them to use quite well.";
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1, StatModifier.ModifyMethod.ADDITIVE);

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            item.quality = PickupObject.ItemQuality.B;
            item.sprite.IsPerpendicular = true;

        }

        
        private float chanceActivate = .20f;




        private void PostProcessProjectile(Projectile projectile, float effectChanceScalar)
        {

            if (Random.value < (chanceActivate * effectChanceScalar))
            {
                Projectile projectile2 = ((Gun)ETGMod.Databases.Items[341]).DefaultModule.projectiles[0];
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, Owner.CurrentGun.transform.position, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
                Projectile component = gameObject.GetComponent<Projectile>();


                if (component != null)
                {
                    if (Owner.PlayerHasActiveSynergy("Bleeding Edge"))
                    {
                        GameActorHealthEffect bleed = (PickupObjectDatabase.GetById(204) as BulletStatusEffectItem).HealthModifierEffect;
                        bleed.effectIdentifier = "Cel sawblade bleed";
                        bleed.resistanceType = EffectResistanceType.None;
                        bleed.TintColor = new Color(201, 0, 0, .25f);
                        component.healthEffect = bleed;
                        component.PoisonApplyChance = 1;
                        component.AppliesPoison = true;
                        bleed.PlaysVFXOnActor = false;
                    }
                    component.Owner = base.Owner;
                    component.Shooter = base.Owner.specRigidbody;
                    component.baseData.speed = 25f;
                    component.baseData.damage = 7f;
                        

                }
            }
            
        }

        private void PostProcessBeamChanceTick(BeamController beamController)
        {
            if (UnityEngine.Random.value < this.chanceActivate)
            {
                Projectile projectile2 = ((Gun)ETGMod.Databases.Items[341]).DefaultModule.projectiles[0];
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
                Projectile component = gameObject.GetComponent<Projectile>();

                if (component != null)
                {
                    if (Owner.PlayerHasActiveSynergy("Bleeding Edge"))
                    {
                        GameActorHealthEffect bleed = (PickupObjectDatabase.GetById(204) as BulletStatusEffectItem).HealthModifierEffect;
                        bleed.effectIdentifier = "Cel sawblade bleed";
                        bleed.resistanceType = EffectResistanceType.None;
                        bleed.TintColor = new Color(201, 0, 0, .25f);
                        component.healthEffect = bleed;
                        component.PoisonApplyChance = 1;
                        component.AppliesPoison = true;
                    }
                    component.Owner = base.Owner;
                    component.Shooter = base.Owner.specRigidbody;
                    component.baseData.speed = 25f;
                    component.baseData.damage = 7f;
                }
            }
        }



        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeamChanceTick += this.PostProcessBeamChanceTick;
     //       LootEngine.GivePrefabToPlayer((PickupObjectDatabase.GetById(510) as Gun).gameObject, player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<Sawblade>().m_pickedUpThisRun = true;
            player.PostProcessProjectile -= this.PostProcessProjectile;
            player.PostProcessBeamChanceTick -= this.PostProcessBeamChanceTick;
            return debrisObject;
        }
    }

    
}
