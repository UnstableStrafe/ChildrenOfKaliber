using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

using UnityEngine;
using Dungeonator;
using MonoMod.RuntimeDetour;

namespace GungeonAPI
{
    //Utility methods
    public static class Tools
    {
        public static bool verbose = false;
        private static string defaultLog = Path.Combine(ETGMod.ResourcesDirectory, "customCharacterLog.txt");
        public static string modID = "CC";

        private static Dictionary<string, float> timers = new Dictionary<string, float>();

        public static void Init()
        {
            if (File.Exists(defaultLog)) File.Delete(defaultLog);
        }

        public static void Print<T>(T obj, string color = "FFFFFF", bool force = false)
        {
            if (verbose || force)
            {
                string[] lines = obj.ToString().Split('\n');
                foreach (var line in lines)
                    LogToConsole($"<color=#{color}>[{modID}] {line}</color>");
            }

            Log(obj.ToString());
        }

        public static void PrintRaw<T>(T obj, bool force = false)
        {
            if (verbose || force)
                LogToConsole(obj.ToString());

            Log(obj.ToString());
        }

        public static void PrintError<T>(T obj, string color = "FF0000")
        {
            string[] lines = obj.ToString().Split('\n');
            foreach (var line in lines)
                LogToConsole($"<color=#{color}>[{modID}] {line}</color>");

            Log(obj.ToString());
        }

        public static void PrintException(Exception e, string color = "FF0000")
        {
            string message = e.Message + "\n" + e.StackTrace;
            {
                string[] lines = message.Split('\n');
                foreach (var line in lines)
                    LogToConsole($"<color=#{color}>[{modID}] {line}</color>");
            }

            Log(e.Message);
            Log("\t" + e.StackTrace);
        }

        public static void Log<T>(T obj)
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(ETGMod.ResourcesDirectory, defaultLog), true))
            {
                writer.WriteLine(obj.ToString());
            }
        }

        public static void Log<T>(T obj, string fileName)
        {
            if (!verbose) return;
            using (StreamWriter writer = new StreamWriter(Path.Combine(ETGMod.ResourcesDirectory, fileName), true))
            {
                writer.WriteLine(obj.ToString());
            }
        }

        public static void LogToConsole(string message)
        {
            message.Replace("\t", "    ");
            ETGModConsole.Log(message);
        }

        private static void BreakdownComponentsInternal(this GameObject obj, int lvl = 0)
        {
            string space = "";
            for (int i = 0; i < lvl; i++)
            {
                space += "\t";
            }

            Log(space + obj.name + "...");
            foreach (var comp in obj.GetComponents<Component>())
            {
                Log(space + "    -" + comp.GetType());
            }

            foreach (var child in obj.GetComponentsInChildren<Transform>())
            {
                if (child != obj.transform)
                    child.gameObject.BreakdownComponentsInternal(lvl + 1);
            }
        }

        public static void BreakdownComponents(this GameObject obj)
        {
            BreakdownComponentsInternal(obj, 0);
        }

        public static void ExportTexture(Texture texture, string folder = "")
        {
            string path = Path.Combine(ETGMod.ResourcesDirectory, folder);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.WriteAllBytes(Path.Combine(path, texture.name + DateTime.Now.Ticks + ".png"), ((Texture2D)texture).EncodeToPNG());
        }

        public static T GetEnumValue<T>(string val) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), val.ToUpper());
        }

        public static void LogPropertiesAndFields<T>(T obj, string header = "")
        {
            Log(header);
            Log("=======================");
            if (obj == null) { Log("LogPropertiesAndFields: Null object"); return; }
            Type type = obj.GetType();
            Log($"Type: {type}");
            PropertyInfo[] pinfos = type.GetProperties();
            Log($"{typeof(T)} Properties: ");
            foreach (var pinfo in pinfos)
            {
                try
                {
                    var value = pinfo.GetValue(obj, null);
                    string valueString = value.ToString();
                    bool isList = obj?.GetType().GetGenericTypeDefinition() == typeof(List<>);
                    if (isList)
                    {
                        var list = value as List<object>;
                        valueString = $"List[{list.Count}]";
                        foreach (var subval in list)
                        {
                            valueString += "\n\t\t" + subval.ToString();
                        }
                    }
                    Log($"\t{pinfo.Name}: {valueString}");
                }
                catch { }
            }
            Log($"{typeof(T)} Fields: ");
            FieldInfo[] finfos = type.GetFields();
            foreach (var finfo in finfos)
            {
                Log($"\t{finfo.Name}: {finfo.GetValue(obj)}");
            }
        }

        public static void StartTimer(string name)
        {
            string key = name.ToLower();
            if (timers.ContainsKey(key))
            {
                PrintError($"Timer {name} already exists.");
                return;
            }
            timers.Add(key, Time.realtimeSinceStartup);
        }

        public static void StopTimerAndReport(string name)
        {
            string key = name.ToLower();
            if (!timers.ContainsKey(key))
            {
                Tools.PrintError($"Could not stop timer {name}, no such timer exists");
                return;
            }
            float timerStart = timers[key];
            int elapsed = (int)((Time.realtimeSinceStartup - timerStart) * 1000);
            timers.Remove(key);
            Tools.Print($"{name} finished in " + elapsed + "ms");
        }
    }
}
