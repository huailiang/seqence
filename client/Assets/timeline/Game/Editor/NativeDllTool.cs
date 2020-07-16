using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

static class XNatives
{
    public static IntPtr? ecs_dll = null;

    [DllImport("kernel32.dll")]
    public static extern IntPtr LoadLibraryEx(string dllToLoad);

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
        XNatives.ecs_dll = XNatives.LoadLibraryEx(entatis);
        if (XNatives.ecs_dll == null)
        {
            Debug.LogError("Load native dll failed.");
        }
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
        XNatives.LoadLibraryEx(entatis);
        if (XNatives.ecs_dll == null)
        {
            Debug.LogError("Load native dll failed.");
        }
        else
        {
            AssetDatabase.Refresh();
            Debug.Log("LOAD END");
        }
#endif
    }


#if UNITY_EDITOR_WIN
    [MenuItem("Tools/RestartUnity")]
    private static void RestartUnity()
    {
        string install = Path.GetDirectoryName(EditorApplication.applicationContentsPath);
        string path = Path.Combine(install, "Unity.exe");
        string[] args = path.Split('\\');
        System.Diagnostics.Process po = new System.Diagnostics.Process();
        Debug.Log("install: " + install + " path: " + path);
        po.StartInfo.FileName = path;
        po.Start();

        System.Diagnostics.Process[] pro = System.Diagnostics.Process.GetProcessesByName(args[args.Length - 1].Split('.')[0]);//Unity
        foreach (var item in pro)
        {
            item.Kill();
        }
    }
#endif

}
