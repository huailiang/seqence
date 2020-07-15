using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

public class NativeInterface
{
    public delegate void PosDelegate(uint uid, float x, float y, float z, float w);

    public delegate void CreateRoleDelegate(uint uid, int confID);

    public delegate void PlayDelegate(uint uid, string skill);

    public delegate void LogDelegate(string msg, int types);


#if UNITY_IPHONE || UNITY_XBOX360
    [DllImport("__Internal")]
#else
    [DllImport("Entitas")]
#endif
    static extern void InitNative(int rate, string assets, PosDelegate cb, CreateRoleDelegate cb2, PlayDelegate cb3, LogDelegate cb4);

#if UNITY_IPHONE || UNITY_XBOX360
    [DllImport("__Internal")]
#else
    [DllImport("Entitas")]
#endif
    static extern void NativeRecv(uint id, byte[] buffer, int len);

#if UNITY_IPHONE || UNITY_XBOX360
    [DllImport("__Internal")]
#else
    [DllImport("Entitas")]
#endif
    static extern void NativeUpdate(float delta);

#if UNITY_IPHONE || UNITY_XBOX360
    [DllImport("__Internal")]
#else
    [DllImport("Entitas")]
#endif
    static extern void NativeDestroy();

    public static void Init(int rate, string assets)
    {
        InitNative(rate, assets, OnPosSync, OnRoleSync, OnPlaySync, OnLogSync);
    }

    public static void Recv(uint id, byte[] buffer, int len)
    {
        NativeRecv(id, buffer, len);
    }

    public static void Update(float delta)
    {
        NativeUpdate(delta);
    }

    public static void Quit()
    {
        NativeDestroy();
    }


    [MonoPInvokeCallback(typeof(PosDelegate))]
    static void OnPosSync(uint id, float x, float y, float z, float w)
    {
        Vector3 pos = new Vector3(x, y, z);
        Quaternion rot = Quaternion.Euler(0, w, 0);
        Debug.Log("OnPosSync: " + pos + " " + rot);
        EntityMgr.Instance.SyncPos(id, pos, rot);
    }

    [MonoPInvokeCallback(typeof(CreateRoleDelegate))]
    static void OnRoleSync(uint id, int confid)
    {
        Debug.Log("OnRoleSync: " + id + " " + confid);
        EntityMgr.Instance.Create(id, confid);
    }

    [MonoPInvokeCallback(typeof(PlayDelegate))]
    static void OnPlaySync(uint id, string skill)
    {
        Debug.Log(skill);
        EntityMgr.Instance.Play(id, skill);
    }

    [MonoPInvokeCallback(typeof(LogDelegate))]
    static void OnLogSync(string msg, int types)
    {
        if (types == 0)
            Debug.Log(msg);
        else if (types == 1)
            Debug.LogWarning(msg);
        else
            Debug.LogError(msg);
    }
}