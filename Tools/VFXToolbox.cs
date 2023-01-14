using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;


namespace Items
{
    public static class VFXToolbox
    {
        public static void BuildVFXList()
        {
            blasphemySlash = (ETGMod.Databases.Items["wonderboy"] as Gun).muzzleFlashEffects;
          
        }

        public static VFXPool blasphemySlash;
    }
}
