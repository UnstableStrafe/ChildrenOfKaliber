
using Alexandria.DungeonAPI;
using Alexandria.ItemAPI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Items
{
    public class LoopManager
    {
        public static int LoopAMT;
        public static bool UsedLoop = false;
    }

    public class TickInteractable : SimpleInteractable, IPlayerInteractable
    {
        bool m_allowMeToIntroduceMyself = true;
        void Start()
        {
            talkPoint = transform.Find("talkpoint");
            m_isToggled = false;
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
            m_canUse = true;
            spriteAnimator.Play("idle");
            ETGModConsole.Log("Test 1");
        }


        public void Interact(PlayerController interactor)
        {
            if (TextBoxManager.HasTextBox(this.talkPoint))
                return;

            m_canUse = CanUse != null ? CanUse.Invoke(interactor, this.gameObject) : m_canUse;
            if (!m_canUse)
            {
                TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, 2f, "It is done...", interactor.characterAudioSpeechTag, instant: false);
                base.spriteAnimator.PlayForDuration("talk", 2f, "idle");
            }
            else
                StartCoroutine(this.HandleConversation(interactor));
        }

        private IEnumerator HandleConversation(PlayerController interactor)
        {
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
            base.spriteAnimator.Play("talk");
            interactor.SetInputOverride("npcConversation");
            Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
            yield return null;

            int conversationIndex = m_allowMeToIntroduceMyself ? 0 : conversation.Count - 1;
            while (conversationIndex < conversation.Count - 1)
            {
             //   Tools.Print($"Index: {conversationIndex}");
                TextBoxManager.ClearTextBox(this.talkPoint);
                TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, -1f, conversation[conversationIndex], interactor.characterAudioSpeechTag, instant: false, showContinueText: true);
                float timer = 0;
                while (!BraveInput.GetInstanceForPlayer(interactor.PlayerIDX).ActiveActions.GetActionFromType(GungeonActions.GungeonActionType.Interact).WasPressed || timer < 0.4f)
                {
                    timer += BraveTime.DeltaTime;
                    yield return null;
                }
                conversationIndex++;
            }
            m_allowMeToIntroduceMyself = false;
            TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, -1f, conversation[conversation.Count - 1], interactor.characterAudioSpeechTag, instant: false, showContinueText: true);

            GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, acceptText, declineText);
            int selectedResponse = -1;
            while (!GameUIRoot.Instance.GetPlayerConversationResponse(out selectedResponse))
                yield return null;

            if (selectedResponse == 0)
            {
                TextBoxManager.ClearTextBox(this.talkPoint);
                base.spriteAnimator.PlayForDuration("do_effect", -1, "talk");

                while (base.spriteAnimator.CurrentFrame < 20) //play do effect anim
                    yield return null;
                OnAccept?.Invoke(interactor, this.gameObject);
                base.spriteAnimator.Play("talk");
                TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, 1f, "It is done...", interactor.characterAudioSpeechTag, instant: false);
                yield return new WaitForSeconds(1f);
            }
            else
            {
                OnDecline?.Invoke(interactor, this.gameObject);
                TextBoxManager.ClearTextBox(this.talkPoint);
            }

            // Free player and run OnAccept/OnDecline actions
            interactor.ClearInputOverride("npcConversation");
            Pixelator.Instance.LerpToLetterbox(1, 0.25f);
            base.spriteAnimator.Play("idle");
        }

        public void OnEnteredRange(PlayerController interactor)
        {
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white, 1f);
            base.sprite.UpdateZDepth();
        }

        public void OnExitRange(PlayerController interactor)
        {
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 1f);
        }

        public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
        {
            shouldBeFlipped = false;
            return string.Empty;
        }

        public float GetDistanceToPoint(Vector2 point)
        {
            if (base.sprite == null)
            {
                return 100f;
            }
            Vector3 v = BraveMathCollege.ClosestPointOnRectangle(point, base.specRigidbody.UnitBottomLeft, base.specRigidbody.UnitDimensions);
            return Vector2.Distance(point, v) / 1.5f;
        }

        public float GetOverrideMaxDistance()
        {
            return -1f;
        }
    }
    public static class Tick
    {
        public static void Add()
        {
            ShrineFactory sf = new ShrineFactory()
            {
                name = "Grandfather Tick",
                modID = "cel",
                //text = "Randomize your stats and loadout?",
                spritePath = "Items/Resources/Tick/Idle/tick_idle_001.png",
                shadowSpritePath = "Items/Resources/Tick/tick_shadow_001.png",
                acceptText = "Uhh... sure.",
                declineText = "I'm good.",
                OnAccept = Accept,
                OnDecline = null,
                CanUse = CanUse,
                offset = new Vector3(184.7f, 19.9f, 20.4f),
                talkPointOffset = new Vector3(12 / 16f, 24 / 16f, 0),
                isToggle = false,
                isBreachShrine = false,
                interactableComponent = typeof(TickInteractable)

            };
            ETGModConsole.Log("Test 2");
            //register shrine
            var obj = sf.Build();
            obj.AddAnimation("idle", "Items/Resources/Tick/Idle/", 5, NPCBuilder.AnimationType.Idle);
            obj.AddAnimation("talk", "Items/Resources/Tick/Talk/", 10, NPCBuilder.AnimationType.Talk);
            obj.AddAnimation("do_effect", "Items/Resources/Tick/DoEffect/", 10, NPCBuilder.AnimationType.Other);
           // var npc = obj.GetComponent<FranseisInteractable>();
            //npc.conversation = new List<string>() {
              //  "I'm Grandfather Tick...",
               // "Do you want to start again...",
               // "I can bring you to when this began...",
               // "But I must warn you, you will lose every item you currently have..."
           // };
            obj.SetActive(false);
            ETGModConsole.Log("Test 3");
        }

        
        
        private static bool CanUse(PlayerController player, GameObject npc)
        {
            return CanUse(player, npc);
        }

        public static void Accept(PlayerController player, GameObject npc)
        {
            npc.GetComponent<tk2dSpriteAnimator>().PlayForDuration("doEffect", -1, "idle");
            player.RemoveAllPassiveItems();
            player.RemoveAllActiveItems();
            player.inventory.DestroyAllGuns();
            LoopManager.LoopAMT += 1;

            RGG.RandomizeStats();
            player.inventory.AddGunToInventory(TimeGun, true);
            player.ownerlessStatModifiers.Clear();
            if (player.characterIdentity != PlayableCharacters.Robot)
            {
                float armor = player.healthHaver.Armor;
                player.healthHaver.Armor -= armor;
            }
            if (player.characterIdentity == PlayableCharacters.Robot)
            {
                float armor = player.healthHaver.Armor;
                if (armor < 6)
                {
                    float ChangeAr = 6 - armor;
                    player.healthHaver.Armor += ChangeAr;
                }
                if (armor > 6)
                {
                    float ChangeAr = armor - 6;
                    player.healthHaver.Armor -= ChangeAr;
                }
            }
            AIActor.HealthModifier *= 1.40f;
            LoopManager.UsedLoop = true;
            GameManager.Instance.LoadCustomLevel("tt_castle");
        }

        private static Gun TimeGun = Gungeon.Game.Items["cel:time_keeper's_pistol"] as Gun;
        
    }
}
