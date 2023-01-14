using System;
using Alexandria.ItemAPI;
using System.Reflection;
using UnityEngine;


namespace Items
{
    class SnareBullets : BulletStatusEffectItem
    {
        public static void Init()
        {

            string itemName = "Snare Bullets";
            string resourceName = "Items/Resources/ItemSprites/Passives/snare_bullets.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<SnareBullets>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Catch and Keep";
            string longDesc = "Shots may slow enemies.\n\n" +
                "To capture something, you must lure it in with something familiar.";
            

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");





            item.chanceOfActivating = .1f;
            item.chanceFromBeamPerSecond = .1f;
            item.sprite.IsPerpendicular = true;

            Gun tripleCrossbow = ETGMod.Databases.Items["triple_crossbow"] as Gun;
         //   new Gun().GetCopyOf(tripleCrossbow);
            var myEffect = new GameActorSpeedEffect();
            foreach (var projmod in tripleCrossbow.RawSourceVolley.projectiles)
            {
                foreach (var proj in projmod.projectiles)
                {
                    if (proj != null)
                    {
                        myEffect.GetCopyOf(proj.speedEffect);
                        myEffect.duration = 10; 
                    }
                }
            }



            item.AppliesSpeedModifier = true;
            item.SpeedModifierEffect = myEffect;
            item.quality = PickupObject.ItemQuality.B;
        }

        
    }

    public static class Extensions
    {
        public static T GetCopyOf<T>(this GameActorSpeedEffect obj, T other) where T : GameActorSpeedEffect
        {
            Type type = obj.GetType();
            if (type != other.GetType()) return null; // type mis-match
            PropertyInfo[] pinfos = type.GetProperties();
            foreach (var pinfo in pinfos)
            {
                if (pinfo.CanWrite)
                {
                    try
                    {
                        var value = pinfo.GetValue(other, null);

                       // ETGModConsole.Log(pinfo.Name + value.ToString());

                        pinfo.SetValue(obj, pinfo.GetValue(other, null), null);
                    }
                    catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
                }
            }
            FieldInfo[] finfos = type.GetFields();
            foreach (var finfo in finfos)
            {
                var value = finfo.GetValue(other);
              //  ETGModConsole.Log(finfo.Name + value.ToString());
                finfo.SetValue(obj, finfo.GetValue(other));
            }
            return obj as T;
        }
    }
    
}