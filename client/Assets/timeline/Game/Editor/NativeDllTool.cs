using System;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

static class XNatives
{
    public static IntPtr? ecs_dll = null;

    [DllImport("kernel32.dll")]
    public static extern IntPtr LoadLibrary(string dllToLoad);

    [DllImport("kernel32.dll")]
    public static extern bool FreeLibrary(IntPtr hModule);
}

public class NativeDllTool
{
    const string entatis = "Assets/timeline/Plugins/x86_64/Entitas.dll";

    [MenuItem("Tools/FreeDll")]
    static void FreeLiabrary()
    {

#if UNITY_EDITOR_WIN
        XNatives.ecs_dll = XNatives.LoadLibrary(entatis);
        while (XNatives.FreeLibrary(XNatives.ecs_dll.Value)) ;
        XNatives.ecs_dll = null;
        AssetDatabase.Refresh();
        Debug.Log("FREE END");
#endif
    }


    [MenuItem("Tools/ReloadDll")]
    static void ReloadLiabrary()
    {
#if UNITY_EDITOR_WIN
        XNatives.LoadLibrary(entatis);
        AssetDatabase.Refresh();
        Debug.Log("LOAD END");
#endif
    }

}
