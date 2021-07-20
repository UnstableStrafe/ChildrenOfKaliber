using System.Collections;
using ItemAPI;
using UnityEngine;
namespace Items
{
    class BrokenAltar : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Broken Altar";

            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<BrokenAltar>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Servant of Hundun";
            string longDesc = "";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;
            item.CanBeDropped = false;
            item.CanBeSold = false;
        }
        private float Timer = 0;
        private bool IsRunning = false;
        private void ResetTimer()
        {
            if (IsRunning)
            {
                StopCoroutine(this.GhostTimer());
                IsRunning = false;
            }
            StartCoroutine(this.GhostTimer());
        }
        private IEnumerator GhostTimer()
        {
            Timer = 0;
            IsRunning = true;
            while (Timer < 180)
            {
                Timer += BraveTime.DeltaTime;
                yield return null;
            }
            if (!GameManager.Instance.Dungeon.CurseReaperActive)
            {
                GameManager.Instance.Dungeon.SpawnCurseReaper();
            }
            IsRunning = false;
            yield break;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            GameManager.Instance.OnNewLevelFullyLoaded += ResetTimer;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<BrokenAltar>().m_pickedUpThisRun = true;
            return debrisObject;
        }

    }
}
