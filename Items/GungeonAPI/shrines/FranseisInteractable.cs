using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GungeonAPI
{
    public class FranseisInteractable : SimpleInteractable, IPlayerInteractable
    {
        bool m_allowMeToIntroduceMyself = true;
        void Start()
        {
            talkPoint = transform.Find("talkpoint");
            m_isToggled = false;
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
            m_canUse = true;
            spriteAnimator.Play("idle");
        }


        public void Interact(PlayerController interactor)
        {
            if (TextBoxManager.HasTextBox(this.talkPoint))
                return;

            m_canUse = CanUse != null ? CanUse.Invoke(interactor, this.gameObject) : m_canUse;
            if (!m_canUse)
            {
                TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, 2f, "No takesies backsies!", interactor.characterAudioSpeechTag, instant: false);
                base.spriteAnimator.PlayForDuration("talk", 2f, "idle");
            }
            else
                StartCoroutine(this.HandleConversation(interactor));
        }

        private IEnumerator HandleConversation(PlayerController interactor)
        {
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
            base.spriteAnimator.PlayForDuration("talk_start", 1, "talk");
            interactor.SetInputOverride("npcConversation");
            Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
            yield return null;

            int conversationIndex = m_allowMeToIntroduceMyself ? 0 : conversation.Count - 1;
            while (conversationIndex < conversation.Count - 1)
            {
                Tools.Print($"Index: {conversationIndex}");
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
                TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, 1f, "Bam!", interactor.characterAudioSpeechTag, instant: false);
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
}
