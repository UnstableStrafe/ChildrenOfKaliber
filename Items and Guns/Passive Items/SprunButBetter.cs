using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;
using System.Collections;
namespace Items
{
    public class SprunButBetter : PlayerOrbitalItem
    {
        public static SprenOrbitalItem m_SprenOrbitalItem;




        public static void Init()
        {



            m_SprenOrbitalItem = PickupObjectDatabase.GetById(578).GetComponent<SprenOrbitalItem>();

            if (!m_SprenOrbitalItem) { return; }

            GameObject sprunObject = new GameObject("Sprun");
            sprunObject.AddComponent<SprunButBetter>();
            sprunObject.AddComponent<tk2dSprite>();
            DuplicateSprite(sprunObject.GetComponent<tk2dSprite>(), m_SprenOrbitalItem.GetComponent<tk2dSprite>());

            SprunButBetter sprun = sprunObject.GetComponent<SprunButBetter>();
            ItemBuilder.SetupItem(sprun, "The Way of Guns", "Transforms into a powerful weapon upon using a blank.\n\nThis Bullet Sprite will transform into an enormously powerful radiant energy weapon under certain circumstances." +
                " Traditionally, this means showing proficiency in bridge delivery and maintenance. If no bridge is available, however, it will take less important things like courage or determination into consideration.", "ck");


            sprun.itemName = "Sprun But Better";
            sprun.PickupObjectId = 578;
            sprun.quality = m_SprenOrbitalItem.quality;
            sprun.additionalMagnificenceModifier = m_SprenOrbitalItem.additionalMagnificenceModifier;
            sprun.ItemSpansBaseQualityTiers = m_SprenOrbitalItem.ItemSpansBaseQualityTiers;
            sprun.ItemRespectsHeartMagnificence = m_SprenOrbitalItem.ItemRespectsHeartMagnificence;
            sprun.associatedItemChanceMods = m_SprenOrbitalItem.associatedItemChanceMods;
            sprun.contentSource = m_SprenOrbitalItem.contentSource;
            sprun.ShouldBeExcludedFromShops = m_SprenOrbitalItem.ShouldBeExcludedFromShops;
            sprun.CanBeDropped = m_SprenOrbitalItem.CanBeDropped;
            sprun.PreventStartingOwnerFromDropping = m_SprenOrbitalItem.PreventStartingOwnerFromDropping;
            sprun.PersistsOnDeath = m_SprenOrbitalItem.PersistsOnDeath;
            sprun.RespawnsIfPitfall = m_SprenOrbitalItem.RespawnsIfPitfall;
            sprun.PreventStartingOwnerFromDropping = m_SprenOrbitalItem.PreventStartingOwnerFromDropping;
            sprun.IgnoredByRat = m_SprenOrbitalItem.IgnoredByRat;
            sprun.SaveFlagToSetOnAcquisition = m_SprenOrbitalItem.SaveFlagToSetOnAcquisition;
            sprun.ForcedPositionInAmmonomicon = m_SprenOrbitalItem.ForcedPositionInAmmonomicon;
            sprun.UsesCustomCost = m_SprenOrbitalItem.UsesCustomCost;
            sprun.CustomCost = m_SprenOrbitalItem.CustomCost;
            sprun.PersistsOnPurchase = m_SprenOrbitalItem.PersistsOnPurchase;
            sprun.CanBeSold = m_SprenOrbitalItem.CanBeSold;
            sprun.passiveStatModifiers = m_SprenOrbitalItem.passiveStatModifiers;
            sprun.ArmorToGainOnInitialPickup = m_SprenOrbitalItem.ArmorToGainOnInitialPickup;
            sprun.minimapIcon = m_SprenOrbitalItem.minimapIcon;

            sprun.OrbitalPrefab = m_SprenOrbitalItem.OrbitalPrefab;
            sprun.OrbitalFollowerPrefab = m_SprenOrbitalItem.OrbitalFollowerPrefab;
            sprun.HasUpgradeSynergy = m_SprenOrbitalItem.HasUpgradeSynergy;
            sprun.CanBeMimicked = m_SprenOrbitalItem.CanBeMimicked;
            sprun.modifiers = m_SprenOrbitalItem.modifiers;
            sprun.synergyModifiers = m_SprenOrbitalItem.synergyModifiers;
            sprun.BreaksUponContact = m_SprenOrbitalItem.BreaksUponContact;
            sprun.BreaksUponOwnerDamage = m_SprenOrbitalItem.BreaksUponOwnerDamage;
            sprun.BreakVFX = m_SprenOrbitalItem.BreakVFX;

            sprun.LimitGunId = m_SprenOrbitalItem.LimitGunId;
            sprun.LimitDuration = m_SprenOrbitalItem.LimitDuration;
            sprun.IdleAnimation = m_SprenOrbitalItem.IdleAnimation;
            sprun.GunChangeAnimation = m_SprenOrbitalItem.GunChangeAnimation;
            sprun.GunChangeMoreAnimation = m_SprenOrbitalItem.GunChangeMoreAnimation;
            sprun.BackchangeAnimation = m_SprenOrbitalItem.BackchangeAnimation;

            m_SprenOrbitalItem.quality = ItemQuality.EXCLUDED;

            sprunObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(sprunObject);
            DontDestroyOnLoad(sprunObject);
        }


        public static void DuplicateSprite(tk2dSprite targetSprite, tk2dSprite sourceSprite)
        {
            targetSprite.automaticallyManagesDepth = sourceSprite.automaticallyManagesDepth;
            targetSprite.ignoresTiltworldDepth = sourceSprite.ignoresTiltworldDepth;
            targetSprite.depthUsesTrimmedBounds = sourceSprite.depthUsesTrimmedBounds;
            targetSprite.allowDefaultLayer = sourceSprite.allowDefaultLayer;
            targetSprite.attachParent = sourceSprite.attachParent;
            targetSprite.OverrideMaterialMode = sourceSprite.OverrideMaterialMode;
            targetSprite.independentOrientation = sourceSprite.independentOrientation;
            targetSprite.autodetectFootprint = sourceSprite.autodetectFootprint;
            targetSprite.customFootprintOrigin = sourceSprite.customFootprintOrigin;
            targetSprite.customFootprint = sourceSprite.customFootprint;
            targetSprite.hasOffScreenCachedUpdate = sourceSprite.hasOffScreenCachedUpdate;
            targetSprite.offScreenCachedCollection = sourceSprite.offScreenCachedCollection;
            targetSprite.offScreenCachedID = sourceSprite.offScreenCachedID;
            targetSprite.Collection = sourceSprite.Collection;
            targetSprite.color = sourceSprite.color;
            targetSprite.scale = sourceSprite.scale;
            targetSprite.spriteId = sourceSprite.spriteId;
            targetSprite.boxCollider2D = sourceSprite.boxCollider2D;
            targetSprite.boxCollider = sourceSprite.boxCollider;
            targetSprite.meshCollider = sourceSprite.meshCollider;
            targetSprite.meshColliderPositions = sourceSprite.meshColliderPositions;
            targetSprite.meshColliderMesh = sourceSprite.meshColliderMesh;
            targetSprite.CachedPerpState = sourceSprite.CachedPerpState;
            targetSprite.HeightOffGround = sourceSprite.HeightOffGround;
            targetSprite.SortingOrder = sourceSprite.SortingOrder;
            targetSprite.IsBraveOutlineSprite = sourceSprite.IsBraveOutlineSprite;
            targetSprite.IsZDepthDirty = sourceSprite.IsZDepthDirty;
            targetSprite.ApplyEmissivePropertyBlock = sourceSprite.ApplyEmissivePropertyBlock;
            targetSprite.GenerateUV2 = sourceSprite.GenerateUV2;
            targetSprite.LockUV2OnFrameOne = sourceSprite.LockUV2OnFrameOne;
            targetSprite.StaticPositions = sourceSprite.StaticPositions;
        }

        private void Start()
        {
            this.AssignTrigger();
        }

        private void AssignTrigger()
        {
            if (this.m_trigger == SprunButBetter.SprenTrigger.UNASSIGNED)
            {
                this.m_trigger = (SprunButBetter.SprenTrigger)1;
            }

        }

        private bool CheckTrigger(SprunButBetter.SprenTrigger target, bool force = false)
        {
            return force || (this.m_owner && this.m_owner.HasActiveBonusSynergy(CustomSynergyType.SHARDBLADE, false) && this.m_secondaryTrigger == target) || this.m_trigger == target;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            this.m_player = player;
            this.AssignTrigger();
            if (this.CheckTrigger(SprunButBetter.SprenTrigger.USED_BLANK, true))
            {
                player.OnUsedBlank += this.HandleBlank;
            }
        }



        public override void Update()
        {
            if (this.m_transformation == SprunButBetter.SprenTransformationState.TRANSFORMED && (GameManager.Instance.IsLoadingLevel || Dungeon.IsGenerating || (this.m_player && this.m_player.CurrentRoom != null && this.m_player.CurrentRoom.IsWinchesterArcadeRoom)))
            {
                this.DetransformSpren();
            }
            if (this.m_transformation == SprunButBetter.SprenTransformationState.NORMAL && this.m_player && this.m_player.CurrentGun)
            {
                if (!this.m_player.CurrentGun.InfiniteAmmo && this.m_player.CurrentGun.ammo <= 0 && this.m_player.CurrentGun.PickupObjectId == this.m_lastEquippedGunID && this.m_lastEquippedGunAmmo > 0)
                {
                    this.TransformSpren();
                }
                this.m_lastEquippedGunID = this.m_player.CurrentGun.PickupObjectId;
                this.m_lastEquippedGunAmmo = this.m_player.CurrentGun.ammo;
            }
            base.Update();
        }

        private void HandleBlank(PlayerController arg1, int BlanksRemaining)
        {
            if (this.m_transformation != SprunButBetter.SprenTransformationState.NORMAL)
            {
                return;
            }
            if (this.CheckTrigger(SprunButBetter.SprenTrigger.USED_BLANK, false))
            {
                this.TransformSpren();
                //ETGModConsole.Log(m_owner.Blanks.ToString());
            }
        }

        private void Disconnect(PlayerController player)
        {
            player.OnUsedBlank -= this.HandleBlank;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            this.Disconnect(player);
          
            return base.Drop(player);
        }
        protected void TransformSpren()
        {
            if (this.m_transformation != SprunButBetter.SprenTransformationState.NORMAL)
            {
                return;
            }
            if (this.m_player && this.m_player.CurrentRoom != null && this.m_player.CurrentRoom.IsWinchesterArcadeRoom)
            {
                return;
            }
            this.m_transformation = SprunButBetter.SprenTransformationState.PRE_TRANSFORM;
            if (this.m_player && !this.m_player.IsGhost)
            {
                this.m_player.StartCoroutine(this.HandleTransformationDuration());
            }
        }

        private IEnumerator HandleTransformationDuration()
        {
            tk2dSpriteAnimator extantAnimator = this.m_extantOrbital.GetComponentInChildren<tk2dSpriteAnimator>();
            extantAnimator.Play(this.GunChangeAnimation);
            PlayerOrbitalFollower follower = this.m_extantOrbital.GetComponent<PlayerOrbitalFollower>();
            if (follower)
            {
                follower.OverridePosition = true;
            }
            float elapsed = 0f;
            extantAnimator.sprite.HeightOffGround = 5f;
            while (elapsed < 1f)
            {
                elapsed += BraveTime.DeltaTime;
                if (follower && this.m_player)
                {
                    follower.OverrideTargetPosition = this.m_player.CenterPosition;
                }
                yield return null;
            }
            extantAnimator.Play(this.GunChangeMoreAnimation);
            while (extantAnimator.IsPlaying(this.GunChangeMoreAnimation))
            {
                if (follower && this.m_player)
                {
                    follower.OverrideTargetPosition = this.m_player.CenterPosition;
                }
                yield return null;
            }
            if (follower)
            {
                follower.ToggleRenderer(false);
            }
            this.m_player.inventory.GunChangeForgiveness = true;
            this.m_transformation = SprunButBetter.SprenTransformationState.TRANSFORMED;
            Gun limitGun = PickupObjectDatabase.GetById(this.LimitGunId) as Gun;
            this.m_extantGun = this.m_player.inventory.AddGunToInventory(limitGun, true);
            this.m_extantGun.CanBeDropped = false;
            this.m_extantGun.CanBeSold = false;
            this.CanBeDropped = false;
            this.CanBeSold = false;
            this.m_player.inventory.GunLocked.SetOverride("spren gun", true, null);
            elapsed = 0f;
            while (elapsed < this.LimitDuration)
            {
                if (follower && this.m_player)
                {
                    follower.OverrideTargetPosition = this.m_player.CenterPosition;
                }
                elapsed += BraveTime.DeltaTime;
                yield return null;
            }
            if (follower)
            {
                follower.ToggleRenderer(true);
            }
            if (extantAnimator)
            {
                extantAnimator.PlayForDuration(this.BackchangeAnimation, -1f, this.IdleAnimation, false);
            }
            while (extantAnimator.IsPlaying(this.BackchangeAnimation))
            {
                if (follower && this.m_player)
                {
                    follower.OverrideTargetPosition = this.m_player.CenterPosition;
                }
                yield return null;
            }
            follower.OverridePosition = false;
            this.DetransformSpren();
            yield break;
        }
        protected void DetransformSpren()
        {
            if (this.m_transformation != SprunButBetter.SprenTransformationState.TRANSFORMED)
            {
                return;
            }
            if (!this || !this.m_player || !this.m_extantGun)
            {
                return;
            }
            this.m_transformation = SprunButBetter.SprenTransformationState.NORMAL;
            if (this.m_player)
            {
                if (!GameManager.Instance.IsLoadingLevel && !Dungeon.IsGenerating)
                {
                    Minimap.Instance.ToggleMinimap(false, false);
                }
                this.CanBeDropped = true;
                this.CanBeSold = true;
                this.m_player.inventory.GunLocked.RemoveOverride("spren gun");
                this.m_player.inventory.DestroyGun(this.m_extantGun);
                this.m_extantGun = null;
            }
            this.m_player.inventory.GunChangeForgiveness = false;
        }
        public override void OnDestroy()
        {
            if (this.m_player)
            {
                this.Disconnect(this.m_player);
            }
            this.m_player = null;
            base.OnDestroy();
        }
        
        [PickupIdentifier]
        public int LimitGunId = 546;

        public float LimitDuration = 8f;

        public string IdleAnimation;

        public string GunChangeAnimation;

        public string GunChangeMoreAnimation;

        public string BackchangeAnimation;

        private SprunButBetter.SprenTrigger m_trigger;

        private SprunButBetter.SprenTrigger m_secondaryTrigger;

        private PlayerController m_player;

        private Gun m_extantGun;

        private SprunButBetter.SprenTransformationState m_transformation;

        private int m_lastEquippedGunID = -1;

        private int m_lastEquippedGunAmmo =-1;





        public enum SprenTrigger
        {
            
            UNASSIGNED,
          
            USED_BLANK,
           
          
        }


        private enum SprenTransformationState
        {

            NORMAL,

            PRE_TRANSFORM,

            TRANSFORMED
        }

    }



}

