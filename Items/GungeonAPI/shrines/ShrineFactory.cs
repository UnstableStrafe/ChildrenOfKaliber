using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Dungeonator;
using CustomCharacters;


namespace GungeonAPI
{
    public class ShrineFactory
    {
        public string
            name,
            modID,
            spritePath, shadowSpritePath,
            text, acceptText, declineText;
        public Action<PlayerController, GameObject>
            OnAccept,
            OnDecline;
        public Func<PlayerController, GameObject, bool> CanUse;
        public Vector3 talkPointOffset;
        public Vector3 offset = new Vector3(43.8f, 42.4f, 42.9f);
        public IntVector2 colliderOffset, colliderSize;
        public bool
            isToggle,
            usesCustomColliderOffsetAndSize;
        public Type interactableComponent = null;
        public bool isBreachShrine = false;
        public PrototypeDungeonRoom room;
        public Dictionary<string, int> roomStyles;

        public static Dictionary<string, GameObject> registeredShrines = new Dictionary<string, GameObject>();
        private static bool m_initialized;

        public static void Init()
        {
            if (m_initialized) return;
            DungeonHooks.OnFoyerAwake += PlaceBreachShrines;
            DungeonHooks.OnPreDungeonGeneration += (generator, dungeon, flow, dungeonSeed) =>
            {
                if (flow.name != "Foyer Flow" && !GameManager.IsReturningToFoyerWithPlayer)
                    CleanupBreachShrines();
            };
            m_initialized = true;
        }

        ///maybe add some value proofing here (name != null, collider != IntVector2.Zero)
        public GameObject Build()
        {
            try
            {
                //Get texture and create sprite
                Texture2D tex = ResourceExtractor.GetTextureFromResource(spritePath);
                var shrine = GungeonAPI.SpriteBuilder.SpriteFromResource(spritePath, null, false);

                //Add (hopefully) unique ID to shrine for tracking
                string ID = $"{modID}:{name}".ToLower().Replace(" ", "_");
                shrine.name = ID;

                //Position sprite 
                var shrineSprite = shrine.GetComponent<tk2dSprite>();
                shrineSprite.IsPerpendicular = true;
                shrineSprite.PlaceAtPositionByAnchor(offset, tk2dBaseSprite.Anchor.LowerCenter);

                //Add speech bubble origin
                var talkPoint = new GameObject("talkpoint").transform;
                talkPoint.position = shrine.transform.position + talkPointOffset;
                talkPoint.SetParent(shrine.transform);

                //Set up collider
                if (!usesCustomColliderOffsetAndSize)
                {
                    IntVector2 spriteDimensions = new IntVector2(tex.width, tex.height);
                    colliderOffset = new IntVector2(0, 0);
                    colliderSize = new IntVector2(spriteDimensions.x, spriteDimensions.y / 2);
                }
                var body = ItemAPI.SpriteBuilder.SetUpSpeculativeRigidbody(shrineSprite, colliderOffset, colliderSize);

                //if (!string.IsNullOrEmpty(shadowSpritePath))
                //{
                //    var shadow = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("DefaultShadowSprite"))).GetComponent<tk2dSprite>();
                //    var shadowSprite = ItemAPI.SpriteBuilder.SpriteFromResource(shadowSpritePath, null, false).GetComponent<tk2dSprite>();
                //    Tools.Print($"Shadow material: {shadow.renderer.material.name}");
                //    Tools.Print($"\tShadow color: {shadow.color}");
                //    Tools.Print($"\tShadow material: {shadowSprite.renderer.material.name}");
                //    Tools.Print($"\tShadow color: {shadowSprite.color}");
                //    Tools.ExportTexture(shadow.renderer.material.mainTexture.GetReadable());
                //    //Tools.ExportTexture(shadowSprite.renderer.material.mainTexture.GetReadable());
                    
                //    shrineSprite.AttachRenderer(shadow);
                //    shadow.PlaceAtPositionByAnchor(shrineSprite.WorldBottomCenter, tk2dBaseSprite.Anchor.UpperCenter);
                //    //shadow.color = new Color(1, 1, 1, .5f);
                //    //shadow.HeightOffGround = -0.1f;
                //    //shadow.renderer.material = defaultSprite.renderer.material;
                //    DepthLookupManager.ProcessRenderer(shadow.GetComponent<Renderer>(), DepthLookupManager.GungeonSortingLayer.BACKGROUND);

                //    //Tools.LogPropertiesAndFields(defaultSprite);

                //}

                var data = shrine.AddComponent<CustomShrineController>();
                data.ID = ID;
                data.roomStyles = roomStyles;
                data.isBreachShrine = true;
                data.offset = offset;
                data.pixelColliders = body.specRigidbody.PixelColliders;
                data.factory = this;
                data.OnAccept = OnAccept;
                data.OnDecline = OnDecline;
                data.CanUse = CanUse;
                data.text = text;
                data.acceptText = acceptText;
                data.declineText = declineText;

                if (interactableComponent == null)
                {
                    var simpInt = shrine.AddComponent<SimpleShrine>();
                    simpInt.isToggle = this.isToggle;
                    simpInt.OnAccept = this.OnAccept;
                    simpInt.OnDecline = this.OnDecline;
                    simpInt.CanUse = CanUse;
                    simpInt.text = this.text;
                    simpInt.acceptText = this.acceptText;
                    simpInt.declineText = this.declineText;
                    simpInt.talkPoint = talkPoint;
                }
                else
                {
                    shrine.AddComponent(interactableComponent);
                }


                shrine.name = ID;
                if (!isBreachShrine)
                {
                    if (!room)
                        room = RoomFactory.CreateEmptyRoom();
                    RegisterShrineRoom(shrine, room, ID, offset);
                }
                registeredShrines.Add(ID, shrine);
                FakePrefab.MarkAsFakePrefab(shrine);
                Tools.Print("Added shrine: " + ID);
                return shrine;
            }
            catch (Exception e)
            {
                Tools.PrintException(e);
                return null;
            }
        }

        public static void RegisterShrineRoom(GameObject shrine, PrototypeDungeonRoom protoroom, string ID, Vector2 offset)
        {

            protoroom.category = PrototypeDungeonRoom.RoomCategory.NORMAL;

            DungeonPrerequisite[] emptyReqs = new DungeonPrerequisite[0];
            Vector2 position = new Vector2(protoroom.Width / 2 + offset.x, protoroom.Height / 2 + offset.y);
            protoroom.placedObjectPositions.Add(position);
            protoroom.placedObjects.Add(new PrototypePlacedObjectData()
            {
                contentsBasePosition = position,
                fieldData = new List<PrototypePlacedObjectFieldData>(),
                instancePrerequisites = emptyReqs,
                linkedTriggerAreaIDs = new List<int>(),
                placeableContents = new DungeonPlaceable()
                {
                    width = 2,
                    height = 2,
                    respectsEncounterableDifferentiator = true,
                    variantTiers = new List<DungeonPlaceableVariant>()
                    {
                        new DungeonPlaceableVariant()
                        {
                            percentChance = 1,
                            nonDatabasePlaceable = shrine,
                            prerequisites = emptyReqs,
                            materialRequirements= new DungeonPlaceableRoomMaterialRequirement[0]
                        }
                    }
                }
            });

            var data = new RoomFactory.RoomData()
            {
                room = protoroom,
                isSpecialRoom = true,
                category = "SPECIAL",
                specialSubCatergory = "UNSPECIFIED_SPECIAL"
            };
            RoomFactory.rooms.Add(ID, data);
            DungeonHandler.Register(data);
        }

        public static void PlaceBreachShrines()
        {
            CleanupBreachShrines();
            Tools.Print("Placing breach shrines: ");
            foreach (var prefab in registeredShrines.Values)
            {
                try
                {
                    var prefabShrineData = prefab.GetComponent<CustomShrineController>();
                    if (!prefabShrineData.isBreachShrine) continue;

                    Tools.Print($"    {prefab.name}");
                    var shrine = GameObject.Instantiate(prefab).GetComponent<CustomShrineController>();
                    shrine.Copy(prefabShrineData);
                    shrine.gameObject.SetActive(true);
                    shrine.sprite.PlaceAtPositionByAnchor(shrine.offset, tk2dBaseSprite.Anchor.LowerCenter);
                    SpriteOutlineManager.AddOutlineToSprite(shrine.sprite, Color.black);
                    var interactable = shrine.GetComponent<IPlayerInteractable>();
                    if (interactable is SimpleInteractable)
                    {
                        ((SimpleInteractable)interactable).OnAccept = shrine.OnAccept;
                        ((SimpleInteractable)interactable).OnDecline = shrine.OnDecline;
                        ((SimpleInteractable)interactable).CanUse = shrine.CanUse;
                    }
                    if (!RoomHandler.unassignedInteractableObjects.Contains(interactable))
                        RoomHandler.unassignedInteractableObjects.Add(interactable);
                }
                catch (Exception e)
                {
                    Tools.PrintException(e);
                }
            }
        }

        private static void CleanupBreachShrines()
        {
            foreach (var cshrine in GameObject.FindObjectsOfType<CustomShrineController>())
            {
                if (!FakePrefab.IsFakePrefab(cshrine))
                    GameObject.Destroy(cshrine.gameObject);
                else
                    cshrine.gameObject.SetActive(false);
            }
        }

        public class CustomShrineController : DungeonPlaceableBehaviour
        {
            public string ID;
            public bool isBreachShrine;
            public Vector3 offset;
            public List<PixelCollider> pixelColliders;
            public Dictionary<string, int> roomStyles;
            public ShrineFactory factory;
            public Action<PlayerController, GameObject>
                OnAccept,
                OnDecline;
            public Func<PlayerController, GameObject, bool> CanUse;
            private RoomHandler m_parentRoom;
            private GameObject m_instanceMinimapIcon;
            public int numUses = 0;
            public string
                text, acceptText, declineText;
            void Start()
            {
                string id = this.name.Replace("(Clone)", "");

                if (ShrineFactory.registeredShrines.ContainsKey(id))
                    Copy(ShrineFactory.registeredShrines[id].GetComponent<CustomShrineController>());
                else
                    Tools.PrintError($"Was this shrine registered correctly?: {id}");
                var si = GetComponent<SimpleInteractable>();
                if (!si) return;
                si.OnAccept = OnAccept;
                si.OnDecline = OnDecline;
                si.CanUse = CanUse;
                si.text = text;
                si.acceptText = acceptText;
                si.declineText = declineText;
                Tools.Print($"Started shrine: {id}");
            }

            public void Copy(CustomShrineController other)
            {
                this.ID = other.ID;
                this.roomStyles = other.roomStyles;
                this.isBreachShrine = other.isBreachShrine;
                this.offset = other.offset;
                this.pixelColliders = other.pixelColliders;
                this.factory = other.factory;
                this.OnAccept = other.OnAccept;
                this.OnDecline = other.OnDecline;
                this.CanUse = other.CanUse;
                this.text = other.text;
                this.acceptText = other.acceptText;
                this.declineText = other.declineText;
            }

            public void ConfigureOnPlacement(RoomHandler room)
            {
                this.m_parentRoom = room;
                this.RegisterMinimapIcon();
            }

            public void RegisterMinimapIcon()
            {
                this.m_instanceMinimapIcon = Minimap.Instance.RegisterRoomIcon(this.m_parentRoom, (GameObject)BraveResources.Load("Global Prefabs/Minimap_Shrine_Icon", ".prefab"), false);
            }

            public void GetRidOfMinimapIcon()
            {
                if (this.m_instanceMinimapIcon != null)
                {
                    Minimap.Instance.DeregisterRoomIcon(this.m_parentRoom, this.m_instanceMinimapIcon);
                    this.m_instanceMinimapIcon = null;
                }
            }
        }
    }
}

