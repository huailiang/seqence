using System.Collections.Generic;
using UnityEditor;


namespace UnityEngine.Timeline
{
    public class XResources
    {
        class Asset : ISharedObject<Asset>
        {
            public Object asset;
            public uint refence;

            public Asset next { get; set; }

            public Asset()
            {
                this.refence = 1;
            }

            public void Dispose()
            {
                refence = 0;
                next = null;
            }
        }

        private static Dictionary<string, Asset> goPool = new Dictionary<string, Asset>();
        private static Dictionary<string, Asset> sharedPool = new Dictionary<string, Asset>();

        public static void Clean()
        {
            sharedPool.Clear();
            goPool.Clear();
            SharedPool<Asset>.Clean();
            Resources.UnloadUnusedAssets();
        }

        public static GameObject LoadGameObject(string path)
        {
#if UNITY_EDITOR
            if (goPool.ContainsKey(path))
            {
                return Object.Instantiate((GameObject) goPool[path].asset);
            }
            else
            {
                var obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (obj)
                {
                    var tmp = SharedPool<Asset>.Get();
                    tmp.asset = obj;
                    goPool.Add(path, tmp);
                    return Object.Instantiate(obj);
                }
                return null;
            }
#else
        // assetbundle implements 
#endif
        }

        public static void DestroyGameObject(string path, GameObject go)
        {
            if (Application.isPlaying)
            {
                Object.Destroy(go);
            }
            else
            {
                Object.DestroyImmediate(go);
            }
            if (goPool.ContainsKey(path))
            {
                var it = goPool[path];
                it.refence--;
                if (it.refence <= 0)
                {
                    goPool.Remove(path);
                    SharedPool<Asset>.Return(it);
                }
            }
        }


        public static T LoadSharedAsset<T>(string path) where T : Object
        {
#if UNITY_EDITOR
            if (sharedPool.ContainsKey(path))
            {
                return sharedPool[path].asset as T;
            }
            else
            {
                var asset = AssetDatabase.LoadAssetAtPath<T>(path);
                var tmp = SharedPool<Asset>.Get();
                tmp.asset = asset;
                sharedPool.Add(path, tmp);
                return asset;
            }
#else
        // assetbundle implements 
#endif
        }

        public static void DestroySharedAsset(string path)
        {
            if (sharedPool.ContainsKey(path))
            {
                var asset = sharedPool[path];
                asset.refence--;
                if (asset.refence <= 0)
                {
#if !UNITY_EDITOR
                     Resources.UnloadAsset(asset.asset);
#endif
                    SharedPool<Asset>.Return(asset);
                }
            }
        }
    }
}
