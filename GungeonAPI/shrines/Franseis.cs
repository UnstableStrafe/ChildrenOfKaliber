
using GungeonAPI;
using ItemAPI;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using UnityEngine;
using HutongGames;
using Random = UnityEngine.Random;
using StatType = PlayerStats.StatType;
namespace CustomCharacters
{
    public static class Franseis
    {
        private static string[] baseCharacters = new string[]
        {
            "convict",
            "guide",
            "marine",
            "rogue",
            "bullet",
            "robot",
            "eevee",
            "gunslinger"
        };
        public static List<string> characters = new List<string>();


        public static void Add()
        {
            ShrineFactory sf = new ShrineFactory()
            {
                name = "Franseis",
                modID = "cc",
                //text = "Randomize your stats and loadout?",
                spritePath = "CustomCharacters/resources/Franseis/Idle/fran_idle_001.png",
                shadowSpritePath = "CustomCharacters/resources/default_shrine_shadow_small.png",
                acceptText = "Lets get weird.",
                declineText = "No thanks.",
                OnAccept = Accept,
                OnDecline = null,
                CanUse = CanUse,
                offset = new Vector3(184.7f, 19.9f, 20.4f),
                talkPointOffset = new Vector3(12 / 16f, 24 / 16f, 0),
                isToggle = false,
                isBreachShrine = true,
                interactableComponent = typeof(FranseisInteractable)
            };
            //register shrine
            var obj = sf.Build();
            obj.AddAnimation("idle", "CustomCharacters/resources/Franseis/Idle/", 5, NPCBuilder.AnimationType.Idle);
            obj.AddAnimation("talk", "CustomCharacters/resources/Franseis/Talk/", 10, NPCBuilder.AnimationType.Talk);
            obj.AddAnimation("talk_start", "CustomCharacters/resources/Franseis/StartTalk/", 7, NPCBuilder.AnimationType.Other);
            obj.AddAnimation("do_effect", "CustomCharacters/resources/Franseis/DoEffect/", 10, NPCBuilder.AnimationType.Other);
            var npc = obj.GetComponent<FranseisInteractable>();
            npc.conversation = new List<string>() {
                "The name's Franseis.",
                "Crazy eyes over there is my uncle.",
                "Wanna get weird?"
            };
            obj.SetActive(false);
        }

        private static bool CanUse(PlayerController player, GameObject npc)
        {
            return player != storedPlayer;
        }

        private static PlayerController storedPlayer;
        public static void Accept(PlayerController player, GameObject npc)
        {
            npc.GetComponent<tk2dSpriteAnimator>().PlayForDuration("doEffect", -1, "idle");
           // CharacterBuilder.StripPlayer(player);
            HandleLoadout(player);
            HandleStats(player);
            storedPlayer = player;
        }

        public static void HandleLoadout(PlayerController player)
        {
            var starterGuns = AllStarterGuns[Random.Range(0, AllStarterGuns.Length)];
            player.inventory.AddGunToInventory(Gungeon.Game.Items[starterGuns.First] as Gun, false);
            if (!string.IsNullOrEmpty(starterGuns.Second))
                player.inventory.AddGunToInventory(Gungeon.Game.Items[starterGuns.Second] as Gun, false);
            if (BraveUtility.RandomBool())
            {
                var gun = GetWeightedRandomItem<Gun>();
                player.startingGunIds.Add(gun.PickupObjectId);
                LootEngine.TryGivePrefabToPlayer(gun.gameObject, player, true);
            }
            else
            {
                var active = GetWeightedRandomItem<PlayerItem>();
                player.startingActiveItemIds.Add(active.PickupObjectId);
                LootEngine.TryGivePrefabToPlayer(active.gameObject, player, true);
            }
            var passive = GetWeightedRandomItem<PassiveItem>();
            passive.CanBeDropped = false;
            player.startingGunIds.Add(passive.PickupObjectId);
            LootEngine.TryGivePrefabToPlayer(passive.gameObject, player, true);
        }

        public static T GetWeightedRandomItem<T>() where T : PickupObject
        {
            float r = Random.Range(0, 1.3f);

            PickupObject.ItemQuality quality;
            if (r < .5f)
                quality = PickupObject.ItemQuality.D;
            else if (r < .9f)
                quality = PickupObject.ItemQuality.C;
            else if (r < 1.2f)
                quality = PickupObject.ItemQuality.B;
            else if (r < 1.25f)
                quality = PickupObject.ItemQuality.A;
            else
                quality = PickupObject.ItemQuality.S;
            var table = typeof(Gun) == typeof(T) ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable;
            return LootEngine.GetItemOfTypeAndQuality<T>(quality, table, false);
        }

        private static string buffColor = "55ff55ff";
        private static string nerfColor = "ff5555ff";
        private static string baseColor = "5555ffff";

        public static void HandleStats(PlayerController player)
        {
            Tools.Print("--------------Randomized Stats--------------", "FF00FF", true);
            foreach (var rs in randomStats)
            {
                float amount = !rs.round ? Random.Range(rs.min, rs.max) : Mathf.RoundToInt(Random.Range(rs.min, rs.max));
                string statName = StatMap[rs.stat.ToString()];
                string formattedValue = amount + "";

                bool isRobot = player.characterIdentity == PlayableCharacters.Robot;
                if (!rs.round)
                    formattedValue = (int)(amount / DefaultValue(rs.stat, isRobot) * 100f) + "%";

                if (rs.stat == PlayerStats.StatType.DodgeRollDistanceMultiplier)
                    player.rollStats.rollDistanceMultiplier = amount;
                else if (rs.stat == PlayerStats.StatType.DodgeRollSpeedMultiplier)
                    player.rollStats.rollTimeMultiplier = amount;
                if(rs.stat == StatType.Health && isRobot)
                {
                    statName = "Armor";
                    amount *= 2;
                    player.healthHaver.Armor = amount;
                    formattedValue = ((int)amount).ToString();
                }
                player.stats.SetBaseStatValue(rs.stat, amount, player);
                player.healthHaver.FullHeal();

                Tools.Print($"{statName}: {formattedValue}", GetGoodnessColor(rs, rs.round ? (int)amount : amount, isRobot), true);
            }
        }

        public static string GetGoodnessColor(RandomStat rs, float value, bool isRobot)
        {
            float defaultValue = DefaultValue(rs.stat, isRobot);
            if (value == defaultValue)
                return baseColor;
            if ((rs.backwards && value < defaultValue) || (!rs.backwards && value > defaultValue))
                return buffColor;
            return nerfColor;
        }

        public static float DefaultValue(StatType stat, bool isRobot)
        {
            if (stat == StatType.MovementSpeed) return 7;
            if (stat == StatType.Coolness || stat == StatType.Curse || stat == StatType.AdditionalBlanksPerFloor) return 0;
            if (stat == StatType.Health) return !isRobot ? 3 : 6;
            return 1;
        }

        public class RandomStat
        {
            public StatType stat;
            public float min = .75f, max = 1.25f;
            public bool round = false;
            public bool backwards = false;
        }

        public static List<RandomStat> randomStats = new List<RandomStat>()
        {
            new RandomStat(){ stat = StatType.Damage },
            new RandomStat(){ stat = StatType.DamageToBosses },

            new RandomStat(){ stat = StatType.MovementSpeed, min = (7f * .75f), max = (7f * 1.25f)},
            new RandomStat(){ stat = StatType.DodgeRollSpeedMultiplier },
            new RandomStat(){ stat = StatType.DodgeRollDistanceMultiplier },

            new RandomStat(){ stat = StatType.Accuracy, backwards = true },
            new RandomStat(){ stat = StatType.RateOfFire },
            new RandomStat(){ stat = StatType.ReloadSpeed, backwards = true},
            new RandomStat(){ stat = StatType.AdditionalClipCapacityMultiplier },
            new RandomStat(){ stat = StatType.RangeMultiplier, min = .9f, max = 1.1f },

            new RandomStat(){ stat = StatType.MoneyMultiplierFromEnemies, min = .9f, max = 1.1f},
            new RandomStat(){ stat = StatType.Coolness, min = 0, max = 3, round = true},
            new RandomStat(){ stat = StatType.Curse, min = 0, max = 3, round = true, backwards = true },
            new RandomStat(){ stat = StatType.Health, min = 2, max = 4, round = true },
            new RandomStat(){ stat = StatType.AdditionalBlanksPerFloor, min = -1, max = 1, round = true},
        };

        public static Tuple<string, string>[] AllStarterGuns = new Tuple<string, string>[]
        {
            new Tuple<string, string>("rusty_sidearm", null),
            new Tuple<string, string>("budget_revolver", null),
            new Tuple<string, string>("robots_right_hand", null),
            new Tuple<string, string>("blasphemy", null),
            new Tuple<string, string>("marine_sidearm", null),
            new Tuple<string, string>("rogue_special", null),
            new Tuple<string, string>("dart_gun", null)
        };

        public static Dictionary<string, string> StatMap = new Dictionary<string, string>()
        {
            { "Accuracy", "Spread" },
            { "AdditionalBlanksPerFloor", "Extra Per-Floor Blanks" },
            { "AdditionalClipCapacityMultiplier", "Clip Size" },
            { "AdditionalGunCapacity", "Gun Slots" },
            { "AdditionalItemCapacity", "Extra Active Slots" },
            { "AdditionalShotBounces", "Shot Bounces" },
            { "AdditionalShotPiercing", "Shot Pierces" },
            { "AmmoCapacityMultiplier", "Ammo Capacity" },
            { "ChargeAmountMultiplier", "Charge Multiplier" },
            { "Coolness", "Coolness" },
            { "Curse", "Curse" },
            { "Damage", "Damage" },
            { "DamageToBosses", "Boss Damage" },
            { "DodgeRollDamage", "Roll Damage" },
            { "DodgeRollDistanceMultiplier", "Roll Distance" },
            { "DodgeRollSpeedMultiplier", "Roll Time" },
            { "EnemyProjectileSpeedMultiplier", "Enemy Shot Speed" },
            { "ExtremeShadowBulletChance", "Y.V. Chance" },
            { "GlobalPriceMultiplier", "Price Multiplier" },
            { "Health", "Heart Containers" },
            { "KnockbackMultiplier", "Knockback" },
            { "MoneyMultiplierFromEnemies", "Money Drop Multiplier" },
            { "MovementSpeed", "Speed" },
            { "PlayerBulletScale", "Bullet Scale" },
            { "ProjectileSpeed", "Shot Speed" },
            { "RangeMultiplier", "Range" },
            { "RateOfFire", "Rate of Fire" },
            { "ReloadSpeed", "Reload Time" },
            { "ShadowBulletChance", "Shadow Bullet Chance" },
            { "TarnisherClipCapacityMultiplier", "Tarnisher Clip Debuff" },
            { "ThrownGunDamage", "Thrown Gun Damage" },
        };
    }
}



