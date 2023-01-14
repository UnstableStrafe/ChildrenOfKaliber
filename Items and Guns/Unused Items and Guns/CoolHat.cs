using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using UnityEngine;
using Alexandria.ItemAPI;

namespace Items
{
    class CoolHat : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Cool Hat";

            string resourceName = "Items/Resources/hat_i_stole.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<CoolHat>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Lookin' Cool, Joker!";
            string longDesc = "Increases coolness by 3.\n\nAn extremely cool hat. Any who gaze upon it are jealous of you.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;
            SetupHatSprite();
        }
        private List<GameObject> extantSprites = new List<GameObject> { };
        private static tk2dSpriteCollectionData HatVFXCollection;
        private static GameObject VFXScapegoat;
        private static int Hat1ID;
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            AddHat();
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<CoolHat>().m_pickedUpThisRun = true;
            DestroyHat();

            return debrisObject;
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            DestroyHat();
        }
        private static void SetupHatSprite()
        {
            VFXScapegoat = new GameObject();
            UnityEngine.Object.DontDestroyOnLoad(VFXScapegoat);
            HatVFXCollection = SpriteBuilder.ConstructCollection(VFXScapegoat, "HatVFX_Collection");
            Hat1ID = SpriteBuilder.AddSpriteToCollection("Items/Resources/hat_i_stole", HatVFXCollection);
        }
        private void AddHat()
        {
            if (extantSprites.Count > 0)
            {
                for (int i = extantSprites.Count - 1; i >= 0; i--)
                {
                    UnityEngine.Object.Destroy(extantSprites[i].gameObject);
                }
                extantSprites.Clear();
                
            }
            GameObject newSprite = new GameObject("Hat", new Type[] { typeof(tk2dSprite) }) { layer = Owner.gameObject.layer + 1};
            newSprite.transform.position = (Owner.sprite.WorldCenter + new Vector2(0, .1f));
            newSprite.transform.position += new Vector3(0, 0, 10f);
            tk2dSprite m_ItemSprite = newSprite.AddComponent<tk2dSprite>();
            extantSprites.Add(newSprite);

            m_ItemSprite.SetSprite(HatVFXCollection, Hat1ID);
            m_ItemSprite.PlaceAtPositionByAnchor(newSprite.transform.position, tk2dBaseSprite.Anchor.LowerCenter);
            m_ItemSprite.transform.localPosition = m_ItemSprite.transform.localPosition.Quantize(0.0625f);
            newSprite.transform.parent = Owner.transform;
            if (m_ItemSprite)
            {
                Owner.sprite.AttachRenderer(m_ItemSprite);
                m_ItemSprite.depthUsesTrimmedBounds = true;
                m_ItemSprite.IsPerpendicular = true;
                m_ItemSprite.UpdateZDepth();
                m_ItemSprite.HeightOffGround = 0.1f;
            }
            Owner.sprite.UpdateZDepth();
        }
        private void DestroyHat()
        {
            if (extantSprites.Count > 0)
            {
                for (int i = extantSprites.Count - 1; i >= 0; i--)
                {
                    UnityEngine.Object.Destroy(extantSprites[i].gameObject);
                }
                extantSprites.Clear();

            }
        }
    }
}
