using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

public class NativeInterface
{
    public delegate void PosDelegate(uint uid, float x, float y, float z, float w);

    public delegate void CreateRoleDelegate(uint uid, uint confID);

    public delegate void PlayDelegate(uint uid, string skill);

    public delegate void BroadDelegate(byte[] buffer, int len);


#if UNITY_IPHONE || UNITY_XBOX360
    [DllImport("__Internal")]
#else
    [DllImport("Engine")]
#endif
    static extern void InitNative(PosDelegate cb, CreateRoleDelegate cb2, PlayDelegate cb3, BroadDelegate cb4);

#if UNITY_IPHONE || UNITY_XBOX360
    [DllImport("__Internal")]
#else
    [DllImport("Engine")]
#endif
    static extern void NativeRecv(uint id, byte[] buffer, int len);

#if UNITY_IPHONE || UNITY_XBOX360
    [DllImport("__Internal")]
#else
    [DllImport("Engine")]
#endif
    static extern void NativeUpdate(float delta);

#if UNITY_IPHONE || UNITY_XBOX360
    [DllImport("__Internal")]
#else
    [DllImport("Engine")]
#endif
    static extern void NativeDestroy();

    public static void Init()
    {
        InitNative(OnPosSync, OnRoleSync, OnPlaySync, OnBroadSync);
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
        Debug.Log(pos + " " + rot);
    }

    [MonoPInvokeCallback(typeof(CreateRoleDelegate))]
    static void OnRoleSync(uint id, uint confid)
    {
        Debug.Log(id + " " + confid);
    }

    [MonoPInvokeCallback(typeof(PlayDelegate))]
    static void OnPlaySync(uint id, string skill)
    {
        Debug.Log(skill);
    }

    [MonoPInvokeCallback(typeof(BroadDelegate))]
    static void OnBroadSync(byte[] buffer, int len)
    {
        Debug.Log(len);
    }
}
