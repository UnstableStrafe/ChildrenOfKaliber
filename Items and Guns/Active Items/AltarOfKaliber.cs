using Alexandria.ItemAPI;
using UnityEngine;
namespace Items
{
    class AltarOfKaliber : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Altar Of Kaliber";

            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<AltarOfKaliber>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "The Goddess Of Guns";
            string longDesc = "";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ck");
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 1);
            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }
        int kaliFavor = 0;
        public override void OnPreDrop(PlayerController player)
        {
            if(kaliFavor < 2)
            {
                StatModifier statModifierC = new StatModifier()
                {
                    statToBoost = PlayerStats.StatType.Curse,
                    amount = 5,
                    modifyType = StatModifier.ModifyMethod.ADDITIVE,
                };
                player.ownerlessStatModifiers.Add(statModifierC);
                if (!GameManager.Instance.Dungeon.CurseReaperActive)
                {
                    GameManager.Instance.Dungeon.SpawnCurseReaper();
                }
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(ETGMod.Databases.Items["Broken Altar"].PickupObjectId).gameObject, player);
                this.Break();
            }
            
        }
        private void Break()
        {            
            UnityEngine.Object.Destroy(base.gameObject, 1f);
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        public override bool CanBeUsed(PlayerController user)
        {
            return !user.CurrentGun.InfiniteAmmo;
        }

    }
}
