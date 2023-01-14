using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using System.Reflection;

namespace Items
{
    public class AudioResourceLoader
    {
        public static readonly string ResourcesDirectoryName = "TCOK_Soundbank";

        public static readonly string pathzip = ChildrenOfKaliberModule.ZipFilePath;
        public static readonly string pathfile = ChildrenOfKaliberModule.FilePath;


        public static void InitAudio()
        {
                LoadAllAutoloadResourcesFromModPath(pathzip);
           
            // LoadAllAutoloadResourcesFromAssembly(Assembly.GetExecutingAssembly(), "ExpandTheGungeon");

            // LoadAllAutoloadResourcesFromPath(FullPathAutoprocess, "ExpandTheGungeon");
        }

        public static void LoadAllAutoloadResourcesFromAssembly(Assembly assembly, string prefix) {
            // this.LoaderText.AutoloadFromAssembly(assembly, prefix);
            // this.LoaderSprites.AutoloadFromAssembly(assembly, prefix, textureSize);
            ResourceLoaderSoundbanks LoaderSoundbanks = new ResourceLoaderSoundbanks();
            LoaderSoundbanks.AutoloadFromAssembly(assembly, prefix);
		}
        
		public static void LoadAllAutoloadResourcesFromPath(string path, string prefix) {
            // this.LoaderText.AutoloadFromPath(path, prefix);
            // this.LoaderSprites.AutoloadFromPath(path, prefix, textureSize);
            ResourceLoaderSoundbanks LoaderSoundbanks = new ResourceLoaderSoundbanks();
            LoaderSoundbanks.AutoloadFromPath(path, prefix);
		}

        public static void LoadAllAutoloadResourcesFromModPath(string path)
        {
                ResourceLoaderSoundbanks LoaderSoundbanks = new ResourceLoaderSoundbanks();
                LoaderSoundbanks.AutoloadFromModZIPOrModFolder(path);
        }

  

    }
}
