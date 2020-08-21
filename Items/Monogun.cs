using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using ItemAPI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Items
{
    class Monogun : GunBehaviour
    {
        public static int T1;
        public static Gun Mono;
        public static void MonogunR()
        {
            T1 = UnityEngine.Random.Range(0, 5);
            if(T1 == 0)
            {
                Monogun.Mono.DefaultModule.numberOfShotsInClip = 15;
                Monogun.Mono.DefaultModule.cooldownTime = 0.15f;
            }
            if (T1 == 1)
            {
                Monogun.Mono.DefaultModule.numberOfShotsInClip = 5;
                Monogun.Mono.DefaultModule.cooldownTime = .8f;

            }
            if (T1 == 2)
            {
                Monogun.Mono.DefaultModule.numberOfShotsInClip = 25;
                Monogun.Mono.DefaultModule.cooldownTime = 0.10f;
            }
            if (T1 == 3)
            {
                Monogun.Mono.DefaultModule.numberOfShotsInClip = 1;
                Monogun.Mono.DefaultModule.cooldownTime = 0.25f;
                Monogun.Mono.DefaultModule.angleVariance = 0f;
            }
            if (T1 == 4)
            {
                Monogun.Mono.DefaultModule.numberOfShotsInClip = 800;
                Monogun.Mono.DefaultModule.cooldownTime = 0.04f;
                Monogun.Mono.DefaultModule.angleVariance = 7.53f;
            }
        }
        public static void Add()
        {

            Gun gun = ETGMod.Databases.Items.NewGun("The Monogun", "monogun");
            Monogun.Mono = gun;
            Game.Items.Rename("outdated_gun_mods:the_monogun", "cel:the_monogun");
            gun.gameObject.AddComponent<Monogun>();
            gun.SetShortDescription("Null");
            gun.SetLongDescription("On pickup, gains 1 Weapon Type and 2 Keywords. Type determines the base gun aspects. Keywords add modifiers to the gun's properties.\n\nOn a distant planet riddled with danger, lies the Power Eternal. Many seek it" +
                ", but none can withstand the energy it contains.\n\nThis is what you wanted, isn't it?");
            gun.SetupSprite(null, "monogun_idle_001", 13);
            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.AddProjectileModuleFrom("38_special", true, false);
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0f;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(97) as Gun).gunSwitchGroup;
            gun.SetBaseMaxAmmo(1);
            gun.gunClass = GunClass.FULLAUTO;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.quality = PickupObject.ItemQuality.C;
            gun.encounterTrackable.EncounterGuid = "The Monogun";
            gun.gunHandedness = GunHandedness.HiddenOneHanded;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.shouldRotate = true;
            gun.sprite.IsPerpendicular = true;
            gun.CanBeDropped = false;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            MonogunR();
        }

        

        private bool HasReloaded;
        private bool Assigned;
        protected void Update()
        {
            if (gun.CurrentOwner)
            {
                  
                if (gun.PreventNormalFireAudio)
                {
                    this.gun.PreventNormalFireAudio = true;
                }
                if (!gun.IsReloading && !HasReloaded)
                {
                    this.HasReloaded = true;
                }
            }
            FUCK();
        }
        
        private void FUCK()
        {
            PlayerController player = this.gun.CurrentOwner as PlayerController;
            bool flag = !Assigned;
            if (flag)
            {

                Assigned = true;
                int T = T1;
                if (T == 0)
                {
                    gun.SetBaseMaxAmmo(450);
                    gun.ammo = 450;
                    gun.reloadTime = 1.1f;
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(Types[T]).gameObject, player);
                    gun.DefaultModule.projectiles[0] = ((Gun)ETGMod.Databases.Items[341]).DefaultModule.projectiles[0];
                    PierceProjModifier orAddComponent = gun.DefaultModule.projectiles[0].gameObject.GetOrAddComponent<PierceProjModifier>();
                    orAddComponent.penetration = 1;
                    BounceProjModifier bounceProj = gun.DefaultModule.projectiles[0].gameObject.GetOrAddComponent<BounceProjModifier>();
                    bounceProj.numberOfBounces = 0;
                }
                if (T == 1)
                {
                    gun.SetBaseMaxAmmo(150);
                    gun.ammo = 150;
                    gun.reloadTime = 1.8f;
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(Types[T]).gameObject, player);
                    gun.DefaultModule.projectiles[0] = ((Gun)ETGMod.Databases.Items[336]).DefaultModule.projectiles[0];
                    PierceProjModifier orAddComponent = gun.DefaultModule.projectiles[0].gameObject.GetOrAddComponent<PierceProjModifier>();
                    orAddComponent.penetration = 0;
                    gun.DefaultModule.projectiles[0].AppliesFire = false;
                    AIActor Grenat = EnemyDatabase.GetOrLoadByGuid("b4666cb6ef4f4b038ba8924fd8adf38f");
                    ExplosiveModifier GetFucked = gun.DefaultModule.projectiles[0].gameObject.AddComponent<ExplosiveModifier>();
                    ExplosionData die = new ExplosionData
                    {
                        damageRadius = 1.5f,
                        damageToPlayer = 0f,
                        doDamage = true,
                        damage = 20f,
                        doExplosionRing = true,
                        doDestroyProjectiles = true,
                        doForce = true,
                        debrisForce = 5f,
                        pushRadius = 1.6f,
                        force = 8f,
                        preventPlayerForce = true,
                        explosionDelay = 0f,
                        usesComprehensiveDelay = false,
                        doScreenShake = false,
                        playDefaultSFX = true,
                        effect = Grenat.GetComponent<ExplodeOnDeath>().explosionData.effect
                        //VFX_Explosion_Tiny,  VFX_Explosion_Newtiny BOTH IN SHARED_AUTO
                    };

                    GetFucked.explosionData = die;

                }
                if (T == 2)
                {

                    gun.SetBaseMaxAmmo(600);
                    gun.ammo = 600;
                    gun.reloadTime = 1.4f;
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(Types[T]).gameObject, player);
                    gun.DefaultModule.projectiles[0] = ((Gun)ETGMod.Databases.Items[229]).DefaultModule.projectiles[0];
                }
                if (T == 3)
                {
                    gun.SetBaseMaxAmmo(100);
                    gun.ammo = 100;
                    gun.reloadTime = 1.25f;
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(Types[T]).gameObject, player);
                    gun.DefaultModule.projectiles[0] = ((Gun)ETGMod.Databases.Items[27]).DefaultModule.projectiles[0];
                    gun.DefaultModule.projectiles[0].baseData.damage *= 8.57142857143f;
                    PierceProjModifier orAddComponent = gun.DefaultModule.projectiles[0].gameObject.GetOrAddComponent<PierceProjModifier>();
                    orAddComponent.penetratesBreakables = true;
                    orAddComponent.penetration = 10000;
                }
                if (T == 4)
                {

                    gun.SetBaseMaxAmmo(800);
                    gun.ammo = 800;
                    gun.reloadTime = 0f;
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(Types[T]).gameObject, player);
                    gun.DefaultModule.projectiles[0] = ((Gun)ETGMod.Databases.Items[124]).DefaultModule.projectiles[0];
                    gun.DefaultModule.projectiles[0].baseData.damage *= 0.6f;
                }
                int K1 = 0;
                if(K1 == 0)
                {
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(KeyWordsUni[K1]).gameObject, player);
                    HomingModifier homingModifier = gun.DefaultModule.projectiles[0].gameObject.AddComponent<HomingModifier>();
                    homingModifier.HomingRadius = 5f;
                    homingModifier.AngularVelocity = 50f;
                }
                if (K1 == 1)
                {
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(KeyWordsUni[K1]).gameObject, player);
                    gun.DefaultModule.projectiles[0].AdditionalScaleMultiplier *= 1.25f;
                }
                if (K1 == 2)
                {
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(KeyWordsUni[K1]).gameObject, player);
                    Gun tripleCrossbow = ETGMod.Databases.Items["triple_crossbow"] as Gun;
                    var myEffect = new GameActorSpeedEffect();
                    foreach (var projmod in tripleCrossbow.RawSourceVolley.projectiles)
                    {
                        foreach (var proj in projmod.projectiles)
                        {
                            if (proj != null)
                            {
                                myEffect.GetCopyOf(proj.speedEffect);
                                myEffect.duration = 10;
                            }
                        }
                    }
                    gun.DefaultModule.projectiles[0].AppliesSpeedModifier = true;
                    gun.DefaultModule.projectiles[0].speedEffect = myEffect;
                    gun.DefaultModule.projectiles[0].SpeedApplyChance = .4f;
                }
                if (K1 == 3)
                {
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(KeyWordsUni[K1]).gameObject, player);
                    gun.SetBaseMaxAmmo(Convert.ToInt32((gun.GetBaseMaxAmmo() * 1.25f)));
                    gun.ammo = gun.GetBaseMaxAmmo();
                }
                int K2;
                if (T == 0)
                {
                    K2 = 1;
                    if(K2 == 0)
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(KeyWordsGunSaw[K2]).gameObject, player);
                        gun.DefaultModule.projectiles[0].baseData.damage *= 1.35f;
                        Mono.DefaultModule.cooldownTime *= 1.25f;
                    }
                    if (K2 == 1)
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(KeyWordsGunSaw[K2]).gameObject, player);
                        BounceProjModifier bounceProj = gun.DefaultModule.projectiles[0].gameObject.GetOrAddComponent<BounceProjModifier>();
                        bounceProj.numberOfBounces += 2;
                    }
                    if (K2 == 2)
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(KeyWordsGunSaw[K2]).gameObject, player);
                        Mono.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
                        Mono.DefaultModule.burstShotCount = 5;
                        Mono.DefaultModule.burstCooldownTime = .05f;
                        Mono.DefaultModule.cooldownTime = .4f;
                    }
                    if (K2 == 3)
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(KeyWordsGunSaw[K2]).gameObject, player);
                        gun.DefaultModule.projectiles[0].AppliesPoison = true;
                        gun.DefaultModule.projectiles[0].healthEffect = (PickupObjectDatabase.GetById(204) as BulletStatusEffectItem).HealthModifierEffect;
                        gun.DefaultModule.projectiles[0].PoisonApplyChance = .25f;
                    }
                }
                if (T == 1)
                {
                    K2 = UnityEngine.Random.Range(0, 4);
                    //Blazing, toxic, flare, burst
                    if (K2 == 0)
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(KeyWordsFire[K2]).gameObject, player);
                        gun.DefaultModule.projectiles[0].OnDestruction += this.FirePool;
                    }
                    if (K2 == 1)
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(KeyWordsFire[K2]).gameObject, player);
                        gun.DefaultModule.projectiles[0].OnDestruction += this.VenomPool;
                        gun.DefaultModule.projectiles[0].DefaultTintColor = new Color(78 / 100f, 5 / 100f, 120 / 100f);
                        gun.DefaultModule.projectiles[0].HasDefaultTint = true;
                    }
                    if (K2 == 2)
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(KeyWordsFire[K2]).gameObject, player);
                        gun.DefaultModule.projectiles[0].OnDestruction += Flares;
                    }
                    if (K2 == 3)
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(KeyWordsFire[K2]).gameObject, player);
                        Mono.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
                        Mono.DefaultModule.burstShotCount = 5;
                        Mono.DefaultModule.burstCooldownTime = .2f;
                        Mono.DefaultModule.cooldownTime = 1f;
                    }
                }
                if (T == 2)
                {
                    //High-caliber, ricochet, burst, poison
                    K2 = UnityEngine.Random.Range(0, 5);
                    if (K2 == 0)
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(KeyWordsGunSaw[K2]).gameObject, player);
                        gun.DefaultModule.projectiles[0].baseData.damage *= 3f;
                        Mono.DefaultModule.cooldownTime *= 2f;
                    }
                    if (K2 == 0)
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(KeyWordsGunSaw[K2]).gameObject, player);
                        BounceProjModifier bounceProj = gun.DefaultModule.projectiles[0].gameObject.GetOrAddComponent<BounceProjModifier>();
                        bounceProj.numberOfBounces += 2;
                    }
                    if (K2 == 0)
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(KeyWordsGunSaw[K2]).gameObject, player);
                        Mono.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
                        Mono.DefaultModule.burstShotCount = 5;
                        Mono.DefaultModule.burstCooldownTime = .05f;
                        Mono.DefaultModule.cooldownTime = .4f;
                    }
                    if (K2 == 0)
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(KeyWordsGunSaw[K2]).gameObject, player);
                        gun.DefaultModule.projectiles[0].AppliesPoison = true;
                        gun.DefaultModule.projectiles[0].healthEffect = (PickupObjectDatabase.GetById(204) as BulletStatusEffectItem).HealthModifierEffect;
                        gun.DefaultModule.projectiles[0].PoisonApplyChance = .25f;
                    }
                }
                if (T == 3)
                {
                    //Quick, power up, sidearm
                    K2 = UnityEngine.Random.Range(0, 4);
                    if (K2 == 0)
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(KeyWordsRail[K2]).gameObject, player);
                        Mono.reloadTime *= .6f;
                    }
                    if (K2 == 0)
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(KeyWordsRail[K2]).gameObject, player);
                        Mono.DefaultModule.projectiles[0].baseData.damage *= 1.25f;
                    }
                    if (K2 == 0)
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(KeyWordsRail[K2]).gameObject, player);
                        Mono.DefaultModule.numberOfShotsInClip = 5;
                        Mono.DefaultModule.projectiles[0].baseData.damage *= .33f;
                    }
                }
                if (T == 4)
                {
                    //Critical, backshot, chain gun
                    K2 = UnityEngine.Random.Range(0, 4);
                    if (K2 == 0)
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(KeyWordsRapid[K2]).gameObject, player);
                        gun.PostProcessProjectile += Proj;
                    }
                    if (K2 == 0)
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(KeyWordsRapid[K2]).gameObject, player);
                        GunVolleyModificationItem.AddDuplicateOfBaseModule(gun.Volley, player, 1, 7.35f, 180);
                    }
                    if (K2 == 0)
                    {
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(KeyWordsRapid[K2]).gameObject, player);
                        Mono.SetBaseMaxAmmo(Mono.GetBaseMaxAmmo() * 2);
                        Mono.ammo = Mono.GetBaseMaxAmmo();
                        Mono.DefaultModule.cooldownTime = .001f;
                    }
                }

            }
        }
        GameActorHealthEffect venom = new GameActorHealthEffect()
        {
            TintColor = new Color(78 / 255f, 5 / 255f, 120 / 255f),
            DeathTintColor = new Color(78 / 255f, 5 / 255f, 120 / 255f),
            AppliesTint = true,
            AppliesDeathTint = true,
            AffectsEnemies = true,
            DamagePerSecondToEnemies = 20f,
            duration = 2.5f,
            effectIdentifier = "Venom",
        };
        private void VenomPool(Projectile projectile)
        {
            GoopDefinition goopDefinition;
            goopDefinition = new GoopDefinition();
            goopDefinition.CanBeIgnited = false;
            goopDefinition.damagesEnemies = false;
            goopDefinition.damagesPlayers = false;
            goopDefinition.baseColor32 = new Color32(78, 5, 120, 200);
            goopDefinition.goopTexture = ResourceExtractor.GetTextureFromResource("Items/Resources/goop_standard_base_001.png");
            goopDefinition.AppliesDamageOverTime = true;
            goopDefinition.HealthModifierEffect = venom;
            var ddgm = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
            ddgm.TimedAddGoopCircle(projectile.sprite.WorldCenter, 2.5f, .1f);
        }
        private void FirePool(Projectile projectile)
        {
            var bundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            var goop = bundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopquickignite.asset");
            var ddgm = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goop);
            ddgm.TimedAddGoopCircle(projectile.sprite.WorldCenter, 2.5f, .1f);
        }
        private void Flares(Projectile projectile)
        {
            float d = 0;
            PlayerController player = gun.CurrentOwner as PlayerController;
            for(int i = 0; i < 4; i++)
            {
                Projectile projectile2 = ((Gun)ETGMod.Databases.Items[275]).DefaultModule.projectiles[0];
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, projectile.sprite.WorldCenter, Quaternion.Euler(0f, 0f, d), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                if (component != null)
                {
                    component.Owner = player;
                    HomingModifier homingModifier = component.gameObject.AddComponent<HomingModifier>();
                    homingModifier.HomingRadius = 100f;
                    homingModifier.AngularVelocity = 500f;
                    component.Shooter = player.specRigidbody;
                    component.baseData.speed = 20f;
                    component.baseData.damage = 2.5f;
                    component.AdditionalScaleMultiplier = .3120069f;
                    GameActorFireEffect fire = component.fireEffect;
                    fire.duration = .5f;
                }
                d += 90f;
            }
            d = 0;
        }
        private void Proj(Projectile projectile)
        {
            float i = UnityEngine.Random.value;
            if (i < .05f)
            {
                projectile.baseData.damage = 75;
                Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(projectile.sprite);
                outlineMaterial.SetColor("_OverrideColor", new Color(252f, 56f, 56f, 50f));
            }
        }
        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && this.HasReloaded)
            {
                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);

            }
        }

        

        public override void OnPostFired(PlayerController player, Gun gun)
        {

           
        }
        private List<int> Types = new List<int>
        {
            ETGMod.Databases.Items["Sawblade Type"].PickupObjectId,
            ETGMod.Databases.Items["Fire Type"].PickupObjectId,
            ETGMod.Databases.Items["Gun Type"].PickupObjectId,
            ETGMod.Databases.Items["Rail Type"].PickupObjectId,
            ETGMod.Databases.Items["Rapid Type"].PickupObjectId
        };
        

        
        private List<int> KeyWordsUni = new List<int>
        {
            ETGMod.Databases.Items["Homing Keyword"].PickupObjectId,
            ETGMod.Databases.Items["Large Keyword"].PickupObjectId,
            ETGMod.Databases.Items["Slow Keyword"].PickupObjectId,
            ETGMod.Databases.Items["Ammo Keyword"].PickupObjectId,
                //Homing, large, slow, ammo
        };
        private List<int> KeyWordsGunSaw = new List<int>
        {
            ETGMod.Databases.Items["Damage Keyword"].PickupObjectId,
            ETGMod.Databases.Items["Bounce Keyword"].PickupObjectId,
            ETGMod.Databases.Items["Burst Keyword"].PickupObjectId,
            ETGMod.Databases.Items["Poison Keyword"].PickupObjectId,
                //High-caliber, ricochet, burst, poison
        };
        private List<int> KeyWordsRapid = new List<int>
        {
            ETGMod.Databases.Items["Critical Keyword"].PickupObjectId,
            ETGMod.Databases.Items["Backshot Keyword"].PickupObjectId,
            ETGMod.Databases.Items["Chain Keyword"].PickupObjectId,
                //Critical, swamer, backshot, chain gun
        };
        private List<int> KeyWordsRail = new List<int>
        {
            ETGMod.Databases.Items["Quick Keyword"].PickupObjectId,
            ETGMod.Databases.Items["Power Keyword"].PickupObjectId,
            ETGMod.Databases.Items["Sidearm Keyword"].PickupObjectId,
                //Quick, power up, sidearm
        };
        private List<int> KeyWordsFire = new List<int>
        {
                //Blazing, toxic, flare, burst
                ETGMod.Databases.Items["Blazing Keyword"].PickupObjectId,
                ETGMod.Databases.Items["Toxic Keyword"].PickupObjectId,
                ETGMod.Databases.Items["Flare Keyword"].PickupObjectId,
                ETGMod.Databases.Items["Burst Keyword"].PickupObjectId,
        };
        public Monogun()
        {
            
        }
    }
}
