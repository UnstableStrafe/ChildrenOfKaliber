using System;
using System.Reflection;
using ItemAPI;
using MonoMod.RuntimeDetour;
using UnityEngine;

namespace Items
{
    class PactSeal : PassiveItem
    {
        private static Action<CompanionController, PlayerController> _companionAdded;
        private static Action<CompanionController, PlayerController> _companionRemoving;
        
        private static readonly Func<CompanionController, PlayerController> GetOwner
            = ReflectionHelper.CreateFieldGetter<CompanionController, PlayerController>("m_owner");

        public PactSeal()
        {
            this.PactSealCharmDurationMultiplier = 3f;
            this.PactSealCompanionDamageMultiplier = 3f;
        }

        public static void Init()
        {
            {
                void InitializeCompanionHook(Action<CompanionController, PlayerController> orig, CompanionController receiver, PlayerController owner)
                {
                    orig(receiver, owner);
                    _companionAdded?.Invoke(receiver, owner);
                }

                Action<Action<CompanionController, PlayerController>, CompanionController, PlayerController> target = InitializeCompanionHook;
                var hook = new Hook(typeof(CompanionController).GetMethod("Initialize"), target);
            }

            {
                void OnDestroyCompanionHook(Action<CompanionController> orig, CompanionController receiver)
                {
                    var owner = GetOwner(receiver);

                    _companionRemoving?.Invoke(receiver, owner);
                    orig(receiver);
                }

                Action<Action<CompanionController>, CompanionController> target = OnDestroyCompanionHook;
                var hook = new Hook(typeof(CompanionController).GetMethod("OnDestroy", BindingFlags.NonPublic | BindingFlags.Instance), target);
            }

            string itemName = "Pact Seal";
            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<PactSeal>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "My Life Become Yours...";
            string longDesc = "Triples follower damage, but increases curse.\n\n" +
                "Foriegn magics work oddly in the Gungeon, unless one inscribes certain runes into the item they desire to enchant.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1);

            item.quality = PickupObject.ItemQuality.A;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            PactSeal.PactCharmDurationMultiplier = this.PactSealCharmDurationMultiplier;
            PactSeal.PactCompanionDamageMultiplier = this.PactSealCompanionDamageMultiplier;
            

            foreach (var companion in player.companions)
            {
                AddJammedEffect(companion);
                
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            foreach (var companion in player.companions)
            {
                RemoveJammedEffect(companion);
            }
            
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<PactSeal>().m_pickedUpThisRun = true;
            return debrisObject;
        }

        protected override void OnDestroy()
        {
            _companionAdded -= HandleCompanionAdded;
            _companionRemoving -= HandleCompanionRemoving;
            base.OnDestroy();
        }

        // called directly by unity engine
        private void Start()
        {
            _companionAdded += HandleCompanionAdded;
            _companionRemoving += HandleCompanionRemoving;
        }

        private void HandleCompanionAdded(CompanionController companion, PlayerController owner)
        {
            if (m_owner && m_owner == owner)
            {
                foreach (var comp in owner.companions)
                {
                    
                    AddJammedEffect(comp);
                }
            }
        }

        private void HandleCompanionRemoving(CompanionController companion, PlayerController owner)
        {
            if (m_owner && m_owner == owner)
            {
                RemoveJammedEffect(companion.aiActor);
            }
        }

        private void AddJammedEffect(AIActor companion)
        {
            if (!companion.IsBlackPhantom)
            {
                companion.BecomeBlackPhantom();
            }
        }

        private void RemoveJammedEffect(AIActor companion)
        {
            if (companion.IsBlackPhantom)
            {
                companion.UnbecomeBlackPhantom();
            }
        }

        protected void HandleCompanionPostProcessProjectile(Projectile obj)
        {
            if (obj)
            {
                obj.collidesWithPlayer = false;
                obj.TreatedAsNonProjectileForChallenge = true;
            }
            if (this.m_owner)
            {
                if (PassiveItem.IsFlagSetForCharacter(this.m_owner, typeof(PactSeal)))
                {
                    obj.baseData.damage *= PactSeal.PactCompanionDamageMultiplier;
                }
                this.m_owner.DoPostProcessProjectile(obj);
            }
        }
        static PactSeal()
        {
            PactSeal.PactCharmDurationMultiplier = 3f;
            PactSeal.PactCompanionDamageMultiplier = 3f;
        }

        // Token: 0x04006F7C RID: 28540
        public static float PactCharmDurationMultiplier;

        // Token: 0x04006F7D RID: 28541
        public static float PactCompanionDamageMultiplier;

        // Token: 0x04006F7E RID: 28542
        public float PactSealCharmDurationMultiplier;

        // Token: 0x04006F7F RID: 28543
        public float PactSealCompanionDamageMultiplier;
    }



    internal static class ReflectionHelper
    {
        public static Func<T, TResult> CreateFieldGetter<T, TResult>(string fieldName)
        {
            var field = typeof(T).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            Func<T, TResult> wrapper = target => (TResult)field.GetValue(target);
            return wrapper;
        }
    }
}
