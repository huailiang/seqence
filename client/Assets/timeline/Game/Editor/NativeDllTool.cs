using System;
using System.Runtime.InteropServices;
using UnityEditor;


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
        XNatives.ecs_dll = XNatives.LoadLibrary(entatis);
        while (XNatives.FreeLibrary(XNatives.ecs_dll.Value)) ;
        XNatives.ecs_dll = null;
        AssetDatabase.Refresh();
    }

}
