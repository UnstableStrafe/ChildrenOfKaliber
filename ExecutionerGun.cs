using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using ItemAPI;

namespace Items
{

    public class ExecutionerGun : GunBehaviour
    {


        public static void Add()
        {
           
            Gun gun = ETGMod.Databases.Items.NewGun("Executioner", "executioner");
            
            Game.Items.Rename("outdated_gun_mods:executioner", "cel:executioner");
            gun.gameObject.AddComponent<ExecutionerGun>();
            
            gun.SetShortDescription("Text");
            gun.SetLongDescription("Text.");

            gun.SetupSprite(null, "executioner_idle_001", 108);

            gun.SetAnimationFPS(gun.shootAnimation, 1);
  
            gun.AddProjectileModuleFrom("ak-47", true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.1f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = 6;
            gun.SetBaseMaxAmmo(250);

            gun.quality = PickupObject.ItemQuality.D;
            gun.encounterTrackable.EncounterGuid = "chopchopstabshootyeet";
            ETGMod.Databases.Items.Add(gun, null, "ANY");

        }



        public override void PostProcessProjectile(Projectile projectile)
        {
            PlayerController playerController = this.gun.CurrentOwner as PlayerController;
            if (playerController == null)
                this.gun.ammo = this.gun.GetBaseMaxAmmo();
            //projectile.baseData allows you to modify the base properties of your projectile module.
            //In our case, our gun uses modified projectiles from the ak-47.
            //Setting static values for a custom gun's projectile stats prevents them from scaling with player stats and bullet modifiers (damage, shotspeed, knockback)
            //You have to multiply the value of the original projectile you're using instead so they scale accordingly. For example if the projectile you're using as a base has 10 damage and you want it to be 6 you use this
            //In our case, our projectile has a base damage of 5.5, so we multiply it by 1.1 so it does 10% more damage from the ak-47.
            projectile.baseData.damage *= 1.10f;
            projectile.baseData.speed *= 1f;
            this.gun.DefaultModule.ammoCost = 1;
            base.PostProcessProjectile(projectile);
            //This is for when you want to change the sprite of your projectile and want to do other magic fancy stuff. But for now let's just change the sprite. 
            //Refer to BasicGunProjectile.cs for changing the sprite.

            projectile.gameObject.AddComponent<ExecutionerProjectile>();

        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            //This determines what sound you want to play when you fire a gun.
            //Sounds names are based on the Gungeon sound dump, which can be found at EnterTheGungeon/Etg_Data/StreamingAssets/Audio/GeneratedSoundBanks/Windows/sfx.txt
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", gameObject);
        }


        //All that's left now is sprite stuff. 
        //Your sprites should be organized, like how you see in the mod folder. 
        //Every gun requires that you have a .json to match the sprites or else the gun won't spawn at all
        //.Json determines the hand sprites for your character. You can make a gun two handed by having both "SecondaryHand" and "PrimaryHand" in the .json file, which can be edited through Notepad or Visual Studios
        //By default this gun is a one-handed weapon
        //If you need a basic two handed .json. Just use the jpxfrd2.json.
        //And finally, don't forget to add your Gun to your ETGModule class!

        public ExecutionerGun()
        {

        }
    }
}
