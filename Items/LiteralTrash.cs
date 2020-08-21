using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using Gungeon;
using ItemAPI;
using MonoMod;
using MonoMod.RuntimeDetour;
using Random = UnityEngine.Random;
using UnityEngine;

namespace Items
{
    class LiteralTrash : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Test Item";

            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<LiteralTrash>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Test Description.";
            string longDesc = "Test Item. \n\n";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 10f);

            item.consumable = true;
            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;




        }

        protected override void DoEffect(PlayerController user)
        {
            Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
        }
    }
}
