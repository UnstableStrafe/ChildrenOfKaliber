using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;
using System.Collections;
using Dungeonator;
using System;
using System.Text;
using System.Collections.Generic;

namespace Items
{

    class MemeticKillAgent : PassiveItem
    {
        public static int itemID;
        public static void Init()
        {
            string itemName = "Memetic Kill Agent";

            string resourceName = "Items/Resources/ItemSprites/Passives/memetic_kill_agent.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<MemeticKillAgent>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "[REDACTED]";
            string longDesc = "Instantly kills any weak enemies. Most low-tier bullet kin have 33% less hp. Has no effect on jammed enemies. Gives 2 curse while held.\n\nAn image cursed with magic that causes any weak creatures to die upon looking at it.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ck");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 2, StatModifier.ModifyMethod.ADDITIVE);
            item.quality = ItemQuality.S;
            item.sprite.IsPerpendicular = true;
            itemID = item.PickupObjectId;

        }
        private void MemeticKill(AIActor actor)
        {
            if(!actor.IsHarmlessEnemy && !actor.IgnoreForRoomClear)
            {
                if (WeakEnemies.Contains(actor.EnemyGuid))
                {
                    if (!actor.IsBlackPhantom)
                    {
                        GameManager.Instance.StartCoroutine(DelayedMemeticKillAgent(actor, true));
                    }

                }
                else if (ModerateKillEnemies.Contains(actor.EnemyGuid))
                {
                    if (!actor.IsBlackPhantom)
                    {
                        GameManager.Instance.StartCoroutine(DelayedMemeticKillAgent(actor, false));
                    }
                }
            }
            
        }
        private IEnumerator DelayedMemeticKillAgent(AIActor actor, bool instaKill)
        {
            actor.behaviorSpeculator.Stun(2);
            if (instaKill)
            {
                actor.healthHaver.IsVulnerable = false;
            }
            yield return new WaitForSeconds(1.8f);

           
            if (instaKill)
            {
                actor.healthHaver.IsVulnerable = true;
                actor.healthHaver.ApplyDamage(1000000, Vector2.zero, "Fun 096 fact's: Run.", CoreDamageTypes.None, DamageCategory.Unstoppable);
            }
            else if (!instaKill)
            {
                int hpToCleave = Mathf.FloorToInt(actor.healthHaver.GetMaxHealth() * .33f);
                actor.healthHaver.ApplyDamage(hpToCleave, Vector2.zero, "This is a bulet. You are a typo.", CoreDamageTypes.None, DamageCategory.Unstoppable);
            }
            yield break;
        }
        public override void Pickup(PlayerController player)
        {
            ETGMod.AIActor.OnPreStart += MemeticKill;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<MemeticKillAgent>().m_pickedUpThisRun = true;
            ETGMod.AIActor.OnPreStart -= MemeticKill;
            return debrisObject;
        }
        public override void OnDestroy()
        {
            ETGMod.AIActor.OnPreStart -= MemeticKill;
            base.OnDestroy();
        }
        public static readonly List<string> WeakEnemies = new List<string>
        {
            "2feb50a6a40f4f50982e89fd276f6f15", //bullat
            "2d4f8b5404614e7d8b235006acde427a", //shotgat
            "b4666cb6ef4f4b038ba8924fd8adf38f", //grenat
            "7ec3e8146f634c559a7d58b19191cd43", //spirat
            "42be66373a3d4d89b91a35c9ff8adfec", //blobulin
            "b8103805af174924b578c98e95313074", //poisbulin
            "249db525a9464e5282d02162c88e0357", //spent
            "4538456236f64ea79f483784370bc62f", //fuse bots
            "be0683affb0e41bbb699cb7125fdded6", //mouser
            "8b43a5c59b854eb780f9ab669ec26b7a", //yolk guy

            //R&G enemies

        };
        public static readonly List<string> ModerateKillEnemies = new List<string>
        {
             "6b7ef9e5d05b4f96b04f05ef4a0d1b18", //rubber kin
             "39e6f47a16ab4c86bec4b12984aece4c", //armored bullet kin
            "226fd90be3a64958a5b13cb0a4f43e97", //musket ball kin
            "143be8c9bbb84e3fb3ab98bcd4cf5e5b", //green fish kin
            "06f5623a351c4f28bc8c6cda56004b80", //blue fish kin
            "906d71ccc1934c02a6f4ff2e9c07c9ec", //office kin
            "9eba44a0ea6c4ea386ff02286dd0e6bd", //other office kin
            "05891b158cd542b1a5f3df30fb67a7ff", //arrow kin
            "01972dee89fc4404a5c408d50007dad5", //bullet kin
            "d4a9836f8ab14f3fadd0f597438b1f1f", //mutant bullet kin
            "e5cffcfabfae489da61062ea20539887", //shroomer
            "7b0b1b6d9ce7405b86b75ce648025dd6", //beadie
        };
    }
}
