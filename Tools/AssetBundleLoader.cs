using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Items
{
	class AssetBundleLoader
	{

		public static AssetBundle LoadAssetBundleFromLiterallyAnywhere(string name)
		{

			AssetBundle result = null;
			bool flag = File.Exists(ChildrenOfKaliberModule.ZipFilePath);
			if (flag)
			{
				ZipFile zipFile = ZipFile.Read(ChildrenOfKaliberModule.ZipFilePath);
				bool flag2 = zipFile != null && zipFile.Entries.Count > 0;
				if (flag2)
				{
					foreach (ZipEntry zipEntry in zipFile.Entries)
					{
						bool flag3 = zipEntry.FileName == name;
						if (flag3)
						{
							using (MemoryStream memoryStream = new MemoryStream())
							{
								zipEntry.Extract(memoryStream);
								memoryStream.Seek(0L, SeekOrigin.Begin);
								result = AssetBundle.LoadFromStream(memoryStream);
								ETGModConsole.Log("Successfully loaded assetbundle!");
								break;
							}
						}
					}
				}
			}
			else
			{
				bool flag4 = File.Exists(ChildrenOfKaliberModule.FilePath + "/" + name);
				if (flag4)
				{
					try
					{
						result = AssetBundle.LoadFromFile(Path.Combine(ChildrenOfKaliberModule.FilePath, name));
						ETGModConsole.Log("Successfully loaded assetbundle!");
					}
					catch (Exception ex)
					{
						ETGModConsole.Log("Failed loading asset bundle from file.");
						ETGModConsole.Log(ex.ToString());
					}
				}
				else
				{
					ETGModConsole.Log("AssetBundle NOT FOUND!");
				}
			}

			return result;
		}
	}
}
