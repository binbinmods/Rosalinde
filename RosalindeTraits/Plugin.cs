﻿using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using static Obeliskial_Essentials.Essentials;
using Obeliskial_Essentials;
using System.IO;
using UnityEngine;
using System;
using BepInEx.Configuration;

namespace Rosalinde
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("com.stiffmeds.obeliskialessentials")]
    [BepInDependency("com.stiffmeds.obeliskialcontent")]
    [BepInProcess("AcrossTheObelisk.exe")]
    public class Plugin : BaseUnityPlugin
    {
        internal int ModDate = int.Parse(DateTime.Today.ToString("yyyyMMdd"));
        private readonly Harmony harmony = new(PluginInfo.PLUGIN_GUID);
        internal static ManualLogSource Log;

        public static ConfigEntry<bool> EnableDebugging { get; set; }
        public static ConfigEntry<float> XOffset { get; set; }
        public static ConfigEntry<float> YOffset { get; set; }

        public static string characterName = "Rosalinde";
        public static string subclassName = "Augur"; // needs caps
        public static string debugBase = "Binbin - Testing " + characterName + " ";

        private void Awake()
        {
            Log = Logger;
            Log.LogInfo($"{PluginInfo.PLUGIN_GUID} {PluginInfo.PLUGIN_VERSION} has loaded!");
            
            EnableDebugging = Config.Bind(new ConfigDefinition("Rosalinde", "Enable Debugging"), true, new ConfigDescription("Enables debugging logs."));
            XOffset = Config.Bind(new ConfigDefinition("Rosalinde", "X Offset"), 0f, new ConfigDescription("Shifts Rosalinde's position X units to the right (make negative if you want it to go to the left)."));
            YOffset = Config.Bind(new ConfigDefinition("Rosalinde", "Y Offset"), 0f, new ConfigDescription("Shifts Rosalinde's position Y units up (make negative if you want it to go down)."));

            // register with Obeliskial Essentials
            RegisterMod(
                _name: PluginInfo.PLUGIN_NAME,
                _author: "binbin",
                _description: "Rosalinde, the Augur.",
                _version: PluginInfo.PLUGIN_VERSION,
                _date: ModDate,
                _link: @"https://github.com/binbinmods/Rosalinde",
                _contentFolder: "Rosalinde",
                _type: ["content", "hero", "trait"]
            );
            // apply patches
            harmony.PatchAll();
        }

        internal static void LogDebug(string msg)
        {
            if (EnableDebugging.Value)
            {
                Log.LogDebug(debugBase + msg);
            }
        }
        internal static void LogInfo(string msg)
        {
            
            Log.LogInfo(debugBase + msg);
        }
        internal static void LogError(string msg)
        {
            Log.LogError(debugBase + msg);
        }
    }
}
