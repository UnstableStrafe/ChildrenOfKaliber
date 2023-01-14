using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine.Scripting;
using UnityEngineInternal;
using SaveAPI;

namespace Items
{
    class Gun3DController : MonoBehaviour
    {
        private void Start()
        {
            gun = base.GetComponent<Gun>();
            Gun3DController previousMaybe = gun.GetComponent<Gun3DController>();

            if (previousMaybe != null)
            {
                Destroy(previousMaybe.obj);
            }

            obj = Instantiate<GameObject>(ChildrenOfKaliberModule.ModAssets.LoadAsset<GameObject>("rgg"), gun.transform.position + new Vector3(0.5f, 0, 0), Quaternion.identity, gun.transform);

            obj.transform.Rotate(new Vector3(0f, 1f, 0f), 0f, Space.Self);
            obj.transform.Rotate(new Vector3(-1f, 0f, 0f), 30f, Space.Self);
            obj.transform.localScale *= 1f;

            gun.sprite.renderer.enabled = false;
            /*
            if (SaveAPIManager.GetFlag(CustomDungeonFlags.EYESTRAINDISABLE) == false)
            {

                Material material = obj.GetComponent<Renderer>().material;
                material.shader = ShaderCache.Acquire("Brave/Internal/Glitch");
                material.SetFloat("_GlitchInterval", 0.08f);
                material.SetFloat("_DispProbability", 0.4f);
                material.SetFloat("_DispIntensity", 0.03f);
                material.SetFloat("_ColorProbability", 0.4f);
                material.SetFloat("_ColorIntensity", 0.03f);
            }
            */

                gun.OnPostFired += Fire;

            flip = gun.CurrentOwner.SpriteFlipped;
            if (flip)
            {
                Flip();
            }


        }

        private void Fire(PlayerController player, Gun gun)
        {
            StartCoroutine(Shoot());
        }
        private void Update()
        {
            if (gun != null)
            {
                if (gun.CurrentOwner != null)
                {
                    if (gun.CurrentOwner.SpriteFlipped != flip)
                    {
                        Flip();
                        flip = gun.CurrentOwner.SpriteFlipped;
                    }
                }
                gun.sprite.renderer.enabled = false;
            }
            //base.gameObject.transform.Rotate(new Vector3(0.8f, 0f, 0.2f), 4f, Space.Self);
            if (base.gameObject.layer != LayerMask.NameToLayer("Unpixelated"))
            {
                base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unpixelated"));
            }
           
        }

        private IEnumerator Shoot()
        {
            obj.transform.Rotate(new Vector3(0, -1, 0), 30);
            yield return new WaitForSeconds(0.09f);
            obj.transform.Rotate(new Vector3(0, 1, 0), 30);


        }
     
        private void OnEnable()
        {
            if (gun == null)
            {
                gun = base.GetComponent<Gun>();
            }
            gun.sprite.renderer.enabled = false;
           


        }

        private void Flip()
        {
            obj.transform.localScale = new Vector3(obj.transform.localScale.x, obj.transform.localScale.y, -obj.transform.localScale.z);
        }

        private Gun gun;
     
        public GameObject obj;

        bool flip;
    }
}
