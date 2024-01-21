using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;
using System.Collections;

namespace Items
{
    class ConvictUltimate : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Convict Ultimate";

            string resourceName = "Items/Resources/Ultimates/Convict/convict_ultimate.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<ConvictUltimate>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Ring Of Fire";
            string longDesc = "Throw a ring of 8 molotovs around you in all directions.\nHold <Switch Active Item Up> to bring up the selector for the ultimate. The ultimate ability will need to be recharged after use.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ck");

            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;

            SetupSprites();
        }

        private float duration = 6;
        public List<GameObject> extantSprites = new List<GameObject> { };
        private static tk2dSpriteCollectionData VFXCollection;
        private static GameObject VFXScapegoat;

        private static int id1;
        private static int id2;
        private static int id3;
        private static int id4;
        private static int id5;
        private static int id6;
        private static int id7;

        [SerializeField]
        private int rechargeLevel = 0;

        private static void SetupSprites()
        {
            VFXScapegoat = new GameObject();
            UnityEngine.Object.DontDestroyOnLoad(VFXScapegoat);
            VFXCollection = SpriteBuilder.ConstructCollection(VFXScapegoat, "Convict_Ability_VFX_Collection");
            UnityEngine.Object.DontDestroyOnLoad(VFXCollection);

            id1 = SpriteBuilder.AddSpriteToCollection("Items/Resources/Ultimates/Convict/convict_ultimate_charge_001", VFXCollection);
            id2 = SpriteBuilder.AddSpriteToCollection("Items/Resources/Ultimates/Convict/convict_ultimate_charge_002", VFXCollection);
            id3 = SpriteBuilder.AddSpriteToCollection("Items/Resources/Ultimates/Convict/convict_ultimate_charge_003", VFXCollection);
            id4 = SpriteBuilder.AddSpriteToCollection("Items/Resources/Ultimates/Convict/convict_ultimate_charge_004", VFXCollection);
            id5 = SpriteBuilder.AddSpriteToCollection("Items/Resources/Ultimates/Convict/convict_ultimate_charge_005", VFXCollection);
            id6 = SpriteBuilder.AddSpriteToCollection("Items/Resources/Ultimates/Convict/convict_ultimate_charge_006", VFXCollection);
            id7 = SpriteBuilder.AddSpriteToCollection("Items/Resources/Ultimates/Convict/convict_ultimate_charge_007", VFXCollection);
        }
        public bool locked = false;
        public bool shown = false;
        public int Current_Select = 0;
        private bool initialShow = false;
        private bool canDoEffect = false;
        private int LastIndicator = -4;
        public override void Update()
        {
            base.Update();
            if (Owner != null)
            {
                if (Library.Key(GungeonActions.GungeonActionType.CycleItemUp, Owner) && Library.KeyTime(GungeonActions.GungeonActionType.CycleItemUp, Owner) > .33f && !locked)
                {
                    shown = true;
                    locked = true;
                }

                if (!Library.Key(GungeonActions.GungeonActionType.CycleItemUp, Owner) && locked == true)
                {
                    locked = false;
                    shown = false;
                    initialShow = false;
                    HideIndicator(Owner);
                    DoEffect();
                    LastIndicator = -4;
                    Current_Select = 0;
                }


                if (shown)
                {

                    if (Owner.CurrentGun.CurrentAngle <= 45 && Owner.CurrentGun.CurrentAngle >= -45)// right
                    {
                        Current_Select = 0;
                    }
                    if (Owner.CurrentGun.CurrentAngle > 45 && Owner.CurrentGun.CurrentAngle <= 135)// up
                    {
                        Current_Select = 1;
                    }
                    if (Owner.CurrentGun.CurrentAngle > 135 || Owner.CurrentGun.CurrentAngle <= -135)// left
                    {
                        Current_Select = 0;
                    }
                    if (Owner.CurrentGun.CurrentAngle < -45 && Owner.CurrentGun.CurrentAngle >= -135)// down
                    {
                        Current_Select = 0;
                    }

                    if (Current_Select != LastIndicator || initialShow == false)
                    {
                        initialShow = true;
                        ShowIndicator(Owner, Current_Select);
                        LastIndicator = Current_Select;
                    }
                }
            }
        }
        private void ShowIndicator(GameActor gunOwner, int chargeLevel)
        {
            if (extantSprites.Count > 0)
            {
                for (int i = extantSprites.Count - 1; i >= 0; i--)
                {
                    UnityEngine.Object.Destroy(extantSprites[i].gameObject);
                }
                extantSprites.Clear();
            }
            GameObject newSprite = new GameObject("Level Popup", new Type[] { typeof(tk2dSprite) }) { layer = 0 };
            newSprite.transform.position = (gunOwner.transform.position + new Vector3(.8f, 1.4f));
            tk2dSprite m_ItemSprite = newSprite.AddComponent<tk2dSprite>();
            extantSprites.Add(newSprite);
            int spriteID = -1;
            switch (chargeLevel)
            {

                case 0:
                    spriteID = id1;
                    break;
                case 1:
                    spriteID = id7;
                    break;
            }
            if (spriteID != id7)
            {
                switch (rechargeLevel)
                {
                    case 0:
                        spriteID = id1;
                        break;
                    case 1:
                        spriteID = id2;
                        break;
                    case 2:
                        spriteID = id3;
                        break;
                    case 3:
                        spriteID = id4;
                        break;
                    case 4:
                        spriteID = id5;
                        break;
                    case 5:
                        spriteID = id6;
                        break;
                }
            }
            if (spriteID == id7 && rechargeLevel != 5)
            {
                switch (rechargeLevel)
                {
                    case 0:
                        spriteID = id1;
                        break;
                    case 1:
                        spriteID = id2;
                        break;
                    case 2:
                        spriteID = id3;
                        break;
                    case 3:
                        spriteID = id4;
                        break;
                    case 4:
                        spriteID = id5;
                        break;
                    case 5:
                        spriteID = id6;
                        break;
                }
            }
            else if (spriteID == id7 && rechargeLevel >= 5)
            {

                canDoEffect = true;
            }
            m_ItemSprite.SetSprite(VFXCollection, spriteID);
            m_ItemSprite.PlaceAtPositionByAnchor(newSprite.transform.position, tk2dBaseSprite.Anchor.LowerCenter);
            m_ItemSprite.transform.localPosition = m_ItemSprite.transform.localPosition.Quantize(0.0625f);
            newSprite.transform.parent = gunOwner.transform;
            if (m_ItemSprite)
            {
                m_ItemSprite.depthUsesTrimmedBounds = true;
                m_ItemSprite.UpdateZDepth();
            }
            LastIndicator = chargeLevel;


        }
        private void HideIndicator(PlayerController player)
        {
            if (extantSprites.Any())
            {
                foreach (GameObject obj in extantSprites)
                {

                    Destroy(obj);
                }
                extantSprites.Clear();
            }
        }
        private void DoEffect()
        {

            if (canDoEffect && LastIndicator == 1)
            {
                
                canDoEffect = false;
                rechargeLevel = 0;
            }
        }

      
        private IEnumerator ThrowMolotovs()
        {
            GameObject molotov = (PickupObjectDatabase.GetById(366) as SpawnObjectPlayerItem).objectToSpawn;

            for(int i = 0; i < 8; i++)
            {
                
            }

            yield break;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnRoomClearEvent += IncreaseChargeLevel;
        }
        private void IncreaseChargeLevel(PlayerController obj)
        {
            if (rechargeLevel < 5)
            {
                rechargeLevel++;
            }
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnRoomClearEvent -= IncreaseChargeLevel;

            return debrisObject;
        }
    }
}

