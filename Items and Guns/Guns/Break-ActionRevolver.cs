using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;
using Brave;
using MonoMod;
using Dungeonator;
using System.Collections;


namespace Items
{
    class Break_ActionRevolver : AdvancedGunBehaviour
    {
     
       
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Break-Action Revolver", "break-action_revolver");
            Game.Items.Rename("outdated_gun_mods:break-action_revolver", "ck:break-action_revolver");
            gun.gameObject.AddComponent<Break_ActionRevolver>();
            gun.SetShortDescription("Rust And Ruin");
            gun.SetLongDescription("A powerful break-action revolver. Hold <Dodge Roll> to bring up a selection of powerful abilities. Each ability needs to be recharged to be used again.\n\nThe Exhumed's signature revolver, powerful in its own right, but it takes a skilled gunslinger to bring out its full power.");
            gun.SetupSprite(null, "break-action_revolver_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 18);
            gun.SetAnimationFPS(gun.reloadAnimation, 10);
            gun.AddProjectileModuleFrom("38_special");
            gun.SetBaseMaxAmmo(180);
            /*
            tk2dSpriteAnimationClip reloadClip = gun.sprite.spriteAnimator.GetClipByName(gun.reloadAnimation);
            float[] offsetsX = new float[] { -0.1250f, -0.1250f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, -0.0625f };
            float[] offsetsY = new float[] { 0.0625f, 0.0625f, 0.0000f, -0.3125f, -0.3125f, -0.3125f, -0.3125f, -0.3125f, 0.1250f, 0.0625f };
            for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < reloadClip.frames.Length; i++) { int id = reloadClip.frames[i].spriteId; reloadClip.frames[i].spriteCollection.spriteDefinitions[id].position0.x += offsetsX[i]; reloadClip.frames[i].spriteCollection.spriteDefinitions[id].position0.y += offsetsY[i]; reloadClip.frames[i].spriteCollection.spriteDefinitions[id].position1.x += offsetsX[i]; reloadClip.frames[i].spriteCollection.spriteDefinitions[id].position1.y += offsetsY[i]; reloadClip.frames[i].spriteCollection.spriteDefinitions[id].position2.x += offsetsX[i]; reloadClip.frames[i].spriteCollection.spriteDefinitions[id].position2.y += offsetsY[i]; reloadClip.frames[i].spriteCollection.spriteDefinitions[id].position3.x += offsetsX[i]; reloadClip.frames[i].spriteCollection.spriteDefinitions[id].position3.y += offsetsY[i]; }
            tk2dSpriteAnimationClip fireClip = gun.sprite.spriteAnimator.GetClipByName(gun.shootAnimation);
            float[] offsetsX2 = new float[] { 0.0000f, -0.1250f, -0.1250f };
            float[] offsetsY2 = new float[] { 0.0625f, 0.0625f, 0.0625f};
            for (int i = 0; i < offsetsX2.Length && i < offsetsY2.Length && i < fireClip.frames.Length; i++) { int id = fireClip.frames[i].spriteId; fireClip.frames[i].spriteCollection.spriteDefinitions[id].position0.x += offsetsX2[i]; fireClip.frames[i].spriteCollection.spriteDefinitions[id].position0.y += offsetsY2[i]; fireClip.frames[i].spriteCollection.spriteDefinitions[id].position1.x += offsetsX2[i]; fireClip.frames[i].spriteCollection.spriteDefinitions[id].position1.y += offsetsY2[i]; fireClip.frames[i].spriteCollection.spriteDefinitions[id].position2.x += offsetsX2[i]; fireClip.frames[i].spriteCollection.spriteDefinitions[id].position2.y += offsetsY2[i]; fireClip.frames[i].spriteCollection.spriteDefinitions[id].position3.x += offsetsX2[i]; fireClip.frames[i].spriteCollection.spriteDefinitions[id].position3.y += offsetsY2[i]; }
            */
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.2f;
            gun.InfiniteAmmo = true;
            gun.DefaultModule.cooldownTime = 0.25f;
            gun.gunClass = GunClass.PISTOL;
            gun.DefaultModule.numberOfShotsInClip = 3;
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            Gun gun2 = PickupObjectDatabase.GetById(38) as Gun;
            Gun gun3 = PickupObjectDatabase.GetById(23) as Gun;
            gun.muzzleFlashEffects = gun3.muzzleFlashEffects;
            gun.barrelOffset.transform.localPosition = new Vector3(1.5625f, 0.875f, 0f);
            gun.DefaultModule.angleVariance = 6f;
            gun.gunSwitchGroup = gun2.gunSwitchGroup;
            gun.encounterTrackable.EncounterGuid = "LETS FUCKIN GOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO!!!!!1!";
            gun.sprite.IsPerpendicular = true;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 1f;
            projectile.baseData.speed *= 1f;
            projectile.baseData.force *= 1f;
            projectile.transform.parent = gun.barrelOffset;
            ETGMod.Databases.Items.Add(gun.GetComponent<PickupObject>());
            SetupCollection();
        }

        private bool HasReloaded;
        public bool locked = false;
        public bool shown = false;
        public int Current_Select = 0;
        private bool initialShow = false;

        protected override void Update()
        {
            if (gun.CurrentOwner)
            {

                
                if (!gun.IsReloading && !HasReloaded)
                {
                    this.HasReloaded = true;
                }
                PlayerController player = this.gun.CurrentOwner as PlayerController;

                if(player != null)
                {
                    if (Key(GungeonActions.GungeonActionType.DodgeRoll, player) && KeyTime(GungeonActions.GungeonActionType.DodgeRoll, player) > .33f && !locked)
                    {
                        shown = true;
                        locked = true;
                    }

                    if (!Key(GungeonActions.GungeonActionType.DodgeRoll, player) && locked == true)
                    {
                        locked = false;
                        shown = false;
                        initialShow = false;
                        HideIndicator(player);
                        DoEffect(player);
                        LastIndicator = -4;
                        Current_Select = 0;
                    }


                    if (shown)
                    {

                        if (player.CurrentGun.CurrentAngle <= 45 && player.CurrentGun.CurrentAngle >= -45)// right
                        {
                            Current_Select = 0;
                        }
                        if (player.CurrentGun.CurrentAngle > 45 && player.CurrentGun.CurrentAngle <= 135)// up
                        {
                            Current_Select = 1;
                        }
                        if (player.CurrentGun.CurrentAngle > 135 || player.CurrentGun.CurrentAngle <= -135)// left
                        {
                            Current_Select = 2;
                        }
                        if (player.CurrentGun.CurrentAngle < -45 && player.CurrentGun.CurrentAngle >= -135)// down
                        {
                            Current_Select = 3;
                        }

                        if (Current_Select != LastIndicator || initialShow == false)
                        {
                            initialShow = true;
                            ShowIndicator(player, Current_Select);
                            LastIndicator = Current_Select;
                        }
                    }
                }
                
            }
        }

        public List<GameObject> extantSprites = new List<GameObject> { };
        private static tk2dSpriteCollectionData GunVFXCollection;
        private static GameObject VFXScapegoat;

        private static int Meter1ID;
        private static int Meter2ID;
        private static int Meter3ID;
        private static int Meter4ID;
        private static void SetupCollection()
        {
            VFXScapegoat = new GameObject();
            UnityEngine.Object.DontDestroyOnLoad(VFXScapegoat);
            GunVFXCollection = SpriteBuilder.ConstructCollection(VFXScapegoat, "Break-Action_Ability_VFX_Collection");
            UnityEngine.Object.DontDestroyOnLoad(GunVFXCollection);

            Meter1ID = SpriteBuilder.AddSpriteToCollection("Items/Resources/VFX/AbilitySelector/ability_selector_001", GunVFXCollection); // burst shot
            Meter2ID = SpriteBuilder.AddSpriteToCollection("Items/Resources/VFX/AbilitySelector/ability_selector_004", GunVFXCollection); // coins
            Meter3ID = SpriteBuilder.AddSpriteToCollection("Items/Resources/VFX/AbilitySelector/ability_selector_003", GunVFXCollection); // bullet time
            Meter4ID = SpriteBuilder.AddSpriteToCollection("Items/Resources/VFX/AbilitySelector/ability_selector_002", GunVFXCollection); // power shot
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

        public float KeyTime(GungeonActions.GungeonActionType action, PlayerController user)
        {
            return BraveInput.GetInstanceForPlayer(user.PlayerIDX).ActiveActions.GetActionFromType(action).PressedDuration;
        }

        public bool KeyDown(GungeonActions.GungeonActionType action, PlayerController user)
        {
            return BraveInput.GetInstanceForPlayer(user.PlayerIDX).ActiveActions.GetActionFromType(action).WasPressed;
        }

        public bool Key(GungeonActions.GungeonActionType action, PlayerController user)
        {
            return BraveInput.GetInstanceForPlayer(user.PlayerIDX).ActiveActions.GetActionFromType(action).IsPressed;
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
            newSprite.transform.position = (gunOwner.transform.position + new Vector3(0.8f, -1.3f));
            tk2dSprite m_ItemSprite = newSprite.AddComponent<tk2dSprite>();
            extantSprites.Add(newSprite);
            int spriteID = -1;
            switch (chargeLevel)
            {

                case 0:
                    spriteID = Meter1ID;
                    break;
                case 1:
                    spriteID = Meter2ID;
                    break;
                case 2:
                    spriteID = Meter3ID;
                    break;
                case 3:
                    spriteID = Meter4ID;
                    break;


            }
            m_ItemSprite.SetSprite(GunVFXCollection, spriteID);
            m_ItemSprite.PlaceAtPositionByAnchor(newSprite.transform.position, tk2dBaseSprite.Anchor.LowerCenter);
            m_ItemSprite.transform.localPosition = m_ItemSprite.transform.localPosition.Quantize(0.0625f);
            newSprite.transform.parent = gunOwner.transform;
            if (m_ItemSprite)
            {
                //sprite.AttachRenderer(m_ItemSprite);
                m_ItemSprite.depthUsesTrimmedBounds = true;
                m_ItemSprite.UpdateZDepth();
            }
            LastIndicator = chargeLevel;
            
            
        }
        private void HideIndicator(PlayerController player)
        {
            if (extantSprites.Any())
            {
                foreach(GameObject obj in extantSprites)
                {
                    
                    Destroy(obj);
                }
                extantSprites.Clear();
            }
        }
        private void DoEffect(PlayerController player)
        {
            if (LastIndicator == 0)
            {
                GameManager.Instance.StartCoroutine(DoBurst(player));
            } 
            else if (LastIndicator == 1)
            {
                
                GameManager.Instance.StartCoroutine(DoPowerShot(player));
            }
            else if (LastIndicator == 2)
            {
                DoBulletTime(player);
            }
            else if (LastIndicator == 3)
            {
                DoCoins(player);
            }
        }
        private IEnumerator DoBurst(PlayerController player)
        {
            int i = 0;
            yield return new WaitForSeconds(.5f);
            while (i < 5)
            {
                float f = UnityEngine.Random.Range(-6, 6);
                gun.spriteAnimator.Play(gun.shootAnimation);
                AkSoundEngine.PostEvent("Play_WPN_magnum_shot_01", base.gameObject);
                GameObject gameObject = SpawnManager.SpawnProjectile(gun.DefaultModule.projectiles[0].gameObject, player.CurrentGun.barrelOffset.transform.position, Quaternion.Euler(0f, 0f, (player.CurrentGun == null) ? 0f : player.CurrentGun.CurrentAngle + f), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                if (component != null)
                {
                    component.Owner = player;
                    component.Shooter = player.specRigidbody;
                    player.DoPostProcessProjectile(component);

                }
                i++;
                yield return new WaitForSeconds(.15f);
            }
            yield break;
        }
        private IEnumerator DoPowerShot(PlayerController player)
        {
            yield return new WaitForSeconds(.5f);
            float f = UnityEngine.Random.Range(-6, 6);
            gun.spriteAnimator.Play(gun.shootAnimation);
            AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", base.gameObject);
            GameObject gameObject = SpawnManager.SpawnProjectile((PickupObjectDatabase.GetById(519) as Gun).DefaultModule.projectiles[0].gameObject, player.CurrentGun.barrelOffset.transform.position, Quaternion.Euler(0f, 0f, (player.CurrentGun == null) ? 0f : player.CurrentGun.CurrentAngle + f), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            if (component != null)
            {
                component.Owner = player;
                component.Shooter = player.specRigidbody;
                component.baseData.damage *= 5;
                component.baseData.speed *= 2;
                player.DoPostProcessProjectile(component);

            }
            yield break;
        }
        private void DoBulletTime(PlayerController player)
        {
           RadialSlowInterface Rad = new RadialSlowInterface
            {
                RadialSlowHoldTime = 6f,
                RadialSlowOutTime = .8f,
                RadialSlowTimeModifier = 0.4f,
                DoesSepia = true,
                UpdatesForNewEnemies = true,
                audioEvent = "Play_OBJ_time_bell_01",
            };
            Rad.DoRadialSlow(player.CenterPosition, player.CurrentRoom);
            player.stats.RecalculateStats(player, false, false);
        }
        private void DoCoins(PlayerController player)
        {
            Library.DoAmbientTalk(player.transform, new Vector3(1, 2, 0), "Not done yet, fuck you.", 4f);
        }
        public int LastIndicator = -4;

        public Break_ActionRevolver()
        {
        }


        
    }
}
