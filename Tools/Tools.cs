using System;
using UnityEngine;
using System.Collections.Generic;


namespace Items
{

    public static class Tools
    {
        

        public static void SetProjectileSpriteRight(this Projectile proj, string name, int pixelWidth, int pixelHeight, int? overrideColliderPixelWidth = null, int? overrideColliderPixelHeight = null)
        {
            try
            {
                bool flag = overrideColliderPixelWidth == null;
                if (flag)
                {
                    overrideColliderPixelWidth = new int?(pixelWidth);
                }
                bool flag2 = overrideColliderPixelHeight == null;
                if (flag2)
                {
                    overrideColliderPixelHeight = new int?(pixelHeight);
                }
                float num = (float)pixelWidth / 16f;
                float num2 = (float)pixelHeight / 16f;
                float x = (float)overrideColliderPixelWidth.Value / 16f;
                float y = (float)overrideColliderPixelHeight.Value / 16f;
                proj.GetAnySprite().spriteId = ETGMod.Databases.Items.ProjectileCollection.inst.GetSpriteIdByName(name);
                tk2dSpriteDefinition tk2dSpriteDefinition = ETGMod.Databases.Items.ProjectileCollection.inst.spriteDefinitions[(PickupObjectDatabase.GetById(12) as Gun).DefaultModule.projectiles[0].GetAnySprite().spriteId].CopyDefinitionFrom();
                tk2dSpriteDefinition.boundsDataCenter = new Vector3(num / 2f, num2 / 2f, 0f);
                tk2dSpriteDefinition.boundsDataExtents = new Vector3(num, num2, 0f);
                tk2dSpriteDefinition.untrimmedBoundsDataCenter = new Vector3(num / 2f, num2 / 2f, 0f);
                tk2dSpriteDefinition.untrimmedBoundsDataExtents = new Vector3(num, num2, 0f);
                tk2dSpriteDefinition.position0 = new Vector3(0f, 0f, 0f);
                tk2dSpriteDefinition.position1 = new Vector3(0f + num, 0f, 0f);
                tk2dSpriteDefinition.position2 = new Vector3(0f, 0f + num2, 0f);
                tk2dSpriteDefinition.position3 = new Vector3(0f + num, 0f + num2, 0f);
                tk2dSpriteDefinition.colliderVertices[1].x = x;
                tk2dSpriteDefinition.colliderVertices[1].y = y;
                tk2dSpriteDefinition.name = name;
                ETGMod.Databases.Items.ProjectileCollection.inst.spriteDefinitions[proj.GetAnySprite().spriteId] = tk2dSpriteDefinition;
                proj.baseData.force = 0f;
            }
            catch (Exception ex)
            {
                ETGModConsole.Log("Ooops! Seems like something got very, Very, VERY wrong. Here's the exception:", false);
                ETGModConsole.Log(ex.ToString(), false);
            }
        }

    }
}