
using UnityEngine;
using ItemAPI;

namespace Items
{
    class ActiveLoopItem : PlayerItem
    {
        public static void Init()
        {

            string itemName = "Grandfather Tick's Pocketwatch";
            string resourceName = "Items/Resources/loop_item.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<ActiveLoopItem>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "I'll Be There Next Time!";
            string longDesc = "Starts a new loop, sending you to the Keep. Removes all items, stat increases, and guns. Gives a new gun that acts as your stater and scales with your loop. Increases enemy HP by 40%." +
                "\n\nAn old brass pocketwatch belonging to the Grandfather of Time, Tick. Use with caution.";


            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            item.quality = PickupObject.ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;

            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 1f);
            item.consumable = true;
            item.numberOfUses = 1;
        }

        protected override void DoEffect(PlayerController user)
        {
            user.RemoveAllPassiveItems();
            user.RemoveAllActiveItems();
            user.inventory.DestroyAllGuns();          
            user.carriedConsumables.Currency -= user.carriedConsumables.Currency;
            user.carriedConsumables.KeyBullets -= user.carriedConsumables.KeyBullets;
            user.carriedConsumables.ResourcefulRatKeys -= user.carriedConsumables.ResourcefulRatKeys;
            user.Blanks -= user.Blanks;
            if(user.IsGunLocked == true)
            {
                user.IsGunLocked = false;
            }           
            LoopManager.LoopAMT += 1;
            RGG.RandomizeStats();
            user.inventory.AddGunToInventory(TimeGun, true);
            user.ownerlessStatModifiers.Clear();
            if (user.characterIdentity != PlayableCharacters.Robot)
            {
                float armor = user.healthHaver.Armor;
                user.healthHaver.Armor -= armor;
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(85).gameObject, user);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(85).gameObject, user);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(85).gameObject, user);
            }
            if(user.characterIdentity == PlayableCharacters.Robot)
            {
                float armor = user.healthHaver.Armor;
                if(armor < 6)
                {
                    float ChangeAr = 6 - armor;
                    user.healthHaver.Armor += ChangeAr;
                }
                if(armor > 6)
                {
                    float ChangeAr = armor - 6;
                    user.healthHaver.Armor -= ChangeAr;
                }
            }
            
            AIActor.HealthModifier *= 1.40f;
            LoopManager.UsedLoop = true;
            GameManager.Instance.LoadCustomLevel("tt_castle");
        }
        private Gun TimeGun = Gungeon.Game.Items["cel:time_keeper's_pistol"] as Gun;
    }
}
