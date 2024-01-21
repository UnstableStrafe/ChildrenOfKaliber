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
    class RobotUltimate : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Robot Ultimate";

            string resourceName = "Items/Resources/Ultimates/Robot/robot_ultimate.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<RobotUltimate>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "StartCoroutine(DoubleMinigun);";
            string longDesc = "Temporarily dual wield two powerful miniguns for 6 seconds.\nHold <Switch Active Item Up> to bring up the selector for the ultimate. The ultimate ability will need to be recharged after use.";

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
            VFXCollection = SpriteBuilder.ConstructCollection(VFXScapegoat, "Robot_Ability_VFX_Collection");
            UnityEngine.Object.DontDestroyOnLoad(VFXCollection);

            id1 = SpriteBuilder.AddSpriteToCollection("Items/Resources/Ultimates/Robot/robot_ultimate_charge_001", VFXCollection);
            id2 = SpriteBuilder.AddSpriteToCollection("Items/Resources/Ultimates/Robot/robot_ultimate_charge_002", VFXCollection);
            id3 = SpriteBuilder.AddSpriteToCollection("Items/Resources/Ultimates/Robot/robot_ultimate_charge_003", VFXCollection);
            id4 = SpriteBuilder.AddSpriteToCollection("Items/Resources/Ultimates/Robot/robot_ultimate_charge_004", VFXCollection);
            id5 = SpriteBuilder.AddSpriteToCollection("Items/Resources/Ultimates/Robot/robot_ultimate_charge_005", VFXCollection);
            id6 = SpriteBuilder.AddSpriteToCollection("Items/Resources/Ultimates/Robot/robot_ultimate_charge_006", VFXCollection);
            id7 = SpriteBuilder.AddSpriteToCollection("Items/Resources/Ultimates/Robot/robot_ultimate_charge_007", VFXCollection);
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
                GameManager.Instance.StartCoroutine(SummonArms());
                canDoEffect = false;
                rechargeLevel = 0;
            }
        }

        private IEnumerator SummonArms()
        {
            Gun arm1 = PickupObjectDatabase.GetById(MiniUberbotHand.lesserLeftHand) as Gun;
            Owner.inventory.AddGunToInventory(arm1, true);
            Owner.inventory.GunLocked.SetOverride("Robot Ultimate", true, null);
            yield return new WaitForSeconds(duration);
            Owner.inventory.GunLocked.SetOverride("Robot Ultimate", false, null);
            Owner.inventory.DestroyCurrentGun();
            
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

    class MiniUberbotHand : AdvancedGunBehaviour
    {
        public static int lesserLeftHand;
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Robots Lesser Left Hand", "robots_lesser_left_hand");
            Game.Items.Rename("outdated_gun_mods:robots_lesser_left_hand", "ck:robots_lesser_left_hand");
            gun.gameObject.AddComponent<MiniUberbotHand>();
            gun.SetShortDescription("Adios");
            gun.SetLongDescription("Rapidly unloads powerful energy bullets.");

            gun.SetupSprite(null, "robots_lesser_left_hand_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 2);
            for (int i = 0; i < 2; i++)
            {
                GunExt.AddProjectileModuleFrom(gun, PickupObjectDatabase.GetById(576) as Gun);
            }

            foreach (ProjectileModule projectileModule in gun.Volley.projectiles)
            {
                projectileModule.ammoType = GameUIAmmoType.AmmoType.SMALL_BULLET;
                projectileModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
                projectileModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                projectileModule.angleVariance = 13f;
                projectileModule.cooldownTime = .1f;
                projectileModule.numberOfShotsInClip = 1000;
                Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(projectileModule.projectiles[0]);
                projectile.gameObject.SetActive(false);
                FakePrefab.MarkAsFakePrefab(projectile.gameObject);
                UnityEngine.Object.DontDestroyOnLoad(projectile);
                projectileModule.projectiles[0] = projectile;
                projectile.transform.parent = gun.barrelOffset;
                projectile.baseData.damage *= .75f;
                projectile.baseData.speed *= 1f;
                projectile.baseData.force /= .5f;
                bool flag = projectileModule == gun.DefaultModule;
                if (flag)
                {
                    projectileModule.ammoCost = 1;
                }
                else
                {
                    projectileModule.ammoCost = 0;
                }
            }
          
            gun.reloadTime = 0f;
           
            gun.PreventNormalFireAudio = false;
            gun.InfiniteAmmo = true;
            Gun gun2 = PickupObjectDatabase.GetById(576) as Gun;
            gun.gunSwitchGroup = gun2.gunSwitchGroup;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            gun.SetBaseMaxAmmo(150);
            gun.barrelOffset.transform.localPosition = new Vector3(1f, 0.375f, 0f);
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            gun.encounterTrackable.EncounterGuid = "eleedle leedle leedele lee.";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.FULLAUTO;
            gun.gunHandedness = GunHandedness.HiddenOneHanded;
            gun.CanBeDropped = false;
            gun.CanBeSold = false;

            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
            lesserLeftHand = gun.PickupObjectId;

        }

        private bool HasReloaded;

        protected override void Update()
        {
            if (gun.CurrentOwner)
            {


                if (!gun.IsReloading && !HasReloaded)
                {
                    this.HasReloaded = true;
                }
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



        public MiniUberbotHand()
        {

        }
    }
    
}
