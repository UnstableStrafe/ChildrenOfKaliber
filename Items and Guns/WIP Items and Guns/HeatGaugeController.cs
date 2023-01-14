using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;
using Gungeon;
using UnityEngine;
using Alexandria.ItemAPI;
namespace Items
{
    class HeatGaugeController : MonoBehaviour
    {
        public HeatGaugeController()
        {
            this.m_burnCapTime = 0;
            this.m_cooldownTime = 2f;
            this.m_delayRunning = false;
            this.m_runDelay = false;
        }
        public void Awake()
        {
            this.m_gun = base.GetComponent<Gun>();
            if(this.m_gun.CurrentOwner is PlayerController)
            {
                this.m_player = this.m_gun.CurrentOwner as PlayerController;                
            }
            this.m_VFXBody = new GameObject();
            UnityEngine.Object.DontDestroyOnLoad(this.m_VFXBody);
            this.m_GunVFXCollection = SpriteBuilder.ConstructCollection(m_VFXBody, "HeatGaugeVFX_Collection");
            UnityEngine.Object.DontDestroyOnLoad(this.m_GunVFXCollection);
            this.m_HeatGaugeEmpty = SpriteBuilder.AddSpriteToCollection("Items/Resources/VFX/HeatGauge/heat_gauge_empty", this.m_GunVFXCollection);
            this.m_HeatGauge1 = SpriteBuilder.AddSpriteToCollection("Items/Resources/VFX/HeatGauge/heat_gauge_1", this.m_GunVFXCollection);
            this.m_HeatGauge2 = SpriteBuilder.AddSpriteToCollection("Items/Resources/VFX/HeatGauge/heat_gauge_2", this.m_GunVFXCollection);
            this.m_HeatGauge3 = SpriteBuilder.AddSpriteToCollection("Items/Resources/VFX/HeatGauge/heat_gauge_3", this.m_GunVFXCollection);
            this.m_HeatGauge4 = SpriteBuilder.AddSpriteToCollection("Items/Resources/VFX/HeatGauge/heat_gauge_4", this.m_GunVFXCollection);
            this.m_HeatGauge5 = SpriteBuilder.AddSpriteToCollection("Items/Resources/VFX/HeatGauge/heat_gauge_5", this.m_GunVFXCollection);
            this.m_HeatGauge6 = SpriteBuilder.AddSpriteToCollection("Items/Resources/VFX/HeatGauge/heat_gauge_6", this.m_GunVFXCollection);
            this.m_HeatGauge7 = SpriteBuilder.AddSpriteToCollection("Items/Resources/VFX/HeatGauge/heat_gauge_7", this.m_GunVFXCollection);
            this.m_HeatGauge8 = SpriteBuilder.AddSpriteToCollection("Items/Resources/VFX/HeatGauge/heat_gauge_8", this.m_GunVFXCollection);
            this.m_HeatGauge9 = SpriteBuilder.AddSpriteToCollection("Items/Resources/VFX/HeatGauge/heat_gauge_9", this.m_GunVFXCollection);
            this.m_HeatGauge10 = SpriteBuilder.AddSpriteToCollection("Items/Resources/VFX/HeatGauge/heat_gauge_10", this.m_GunVFXCollection);
            this.m_HeatGauge1 = SpriteBuilder.AddSpriteToCollection("Items/Resources/VFX/HeatGauge/heat_gauge_full", this.m_GunVFXCollection);
            this.m_gun.OnPostFired += this.AddHeat;
            this.m_outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(this.m_gun.sprite);
            this.m_outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
        }
        private void Update()
        {
            bool flag = this.m_gun && this.m_gun.isActiveAndEnabled && this.m_gun.CurrentOwner;
            if (flag)
            {
                //ETGModConsole.Log("Its On!");
                DoHeat();
                
            }
            else if (this.m_gun && !this.m_gun.isActiveAndEnabled && this.m_gun.CurrentOwner)
            {
                //disable heat stuff
                //ETGModConsole.Log("Its Off!");
                this.m_burnTime = 0;
                this.m_player.inventory.GunLocked.RemoveOverride("Gun Hot!");
                this.m_delayRunning = false;
                this.m_runDelay = false;
                this.m_outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
            }
        }
        private void OnDisable()
        {
            this.m_burnTime = 0;
            this.m_player.inventory.GunLocked.RemoveOverride("Gun Hot!");
            this.m_delayRunning = false;
            this.m_runDelay = false;
            this.m_outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
        }
        private void DoHeat()
        {
            if(this.m_burnTime > 0)
            {
                this.m_gun.CanBeDropped = false;
                this.m_gun.CanBeSold = false;
                if (!this.m_player.IsGunLocked)
                {
                    this.m_player.inventory.GunLocked.SetOverride("Gun Hot!", true, null);
                }                
            }
            else
            {
                if(this.m_burnTime <= 0 && !this.m_gun.CanBeDropped && this.m_player.CurrentStoneGunTimer <= 0)
                {
                    this.m_gun.CanBeDropped = true;
                    this.m_gun.CanBeSold = true;
                    this.m_player.inventory.GunLocked.RemoveOverride("Gun Hot!");
                    this.m_runDelay = true;
                    this.m_outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
                }
            }

            if (!this.m_gun.IsFiring && this.m_burnTime > 0 && !this.m_gun.IsReloading)
            {  
                if (!this.m_player.IsInCombat)
                {
                    this.m_burnTime = 0;                   
                }
                else
                {
                    if (this.m_runDelay)
                    {
                        StartCoroutine(this.CooldownDelay());
                    }
                    else
                    {
                        if (!this.m_delayRunning)
                        {
                            this.m_burnTime -= BraveTime.DeltaTime * .5f;
                            
                            if (this.m_burnTime < 0)
                            {
                                this.m_burnTime = 0;
                                this.m_outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
                            }
                        }
                    }       
                }
            }
            if(this.m_burnTime >= this.m_burnCapTime)
            {
                this.m_burnTime = 0;
                this.m_player.CurrentStoneGunTimer = this.m_cooldownTime;
               
            }

        }
        private void AddHeat(PlayerController player, Gun gun)
        {
            if (!this.m_runDelay)
            {
                this.m_runDelay = true;
            }
            if (this.m_delayRunning)
            {
                StopCoroutine(CooldownDelay());
            }
            if(player.PlayerHasActiveSynergy("Strong Coolant") && player.CurrentGun == gun && this.m_gun == ETGMod.Databases.Items["heater_assault_rifle"] as Gun)
            {
                this.m_burnTime += gun.DefaultModule.cooldownTime * .5f;
            }
            else
            {
                this.m_burnTime += gun.DefaultModule.cooldownTime;
                
            }
            
            //255, 56, 56
        }
        
        private IEnumerator CooldownDelay()
        {
            this.m_delayRunning = true;
            float delayAmount = this.m_gun.DefaultModule.cooldownTime * 2;
            if (this.m_player.PlayerHasActiveSynergy("Strong Coolant") && this.m_player.CurrentGun == this.m_gun && this.m_gun == ETGMod.Databases.Items["heater_assault_rifle"] as Gun)
            {
                delayAmount = this.m_gun.DefaultModule.cooldownTime * 1.5f;
            }
            float elapsed = 0;
            while(elapsed < delayAmount)
            {
                elapsed += BraveTime.DeltaTime;
                yield return null;
            }
            this.m_delayRunning = false;
            yield break;
        }        
        private bool m_delayRunning;
        private bool m_runDelay;
        private float m_burnTime;
        public float m_burnCapTime;
        public float m_cooldownTime;
        private Gun m_gun;
        private PlayerController m_player;
        public List<GameObject> m_extantSprites;
        private tk2dSpriteCollectionData m_GunVFXCollection;
        private GameObject m_VFXBody;
        private int m_HeatGaugeEmpty;
        private int m_HeatGauge1;
        private int m_HeatGauge2;
        private int m_HeatGauge3;
        private int m_HeatGauge4;
        private int m_HeatGauge5;
        private int m_HeatGauge6;
        private int m_HeatGauge7;
        private int m_HeatGauge8;
        private int m_HeatGauge9;
        private int m_HeatGauge10;
        private Material m_outlineMaterial;
    }
}
