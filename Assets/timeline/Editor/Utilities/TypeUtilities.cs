using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    public class TypeUtilities
    {
        private static Type[] s_AllTrackTypes;
        private static Type[] s_AllClipTypes;
        private static Type[] s_MarkerTypes;


        public static bool IsConcretePlayableAsset(Type t)
        {
            return typeof(IClip).IsAssignableFrom(t) && !t.IsAbstract && !t.IsGenericType && !t.IsInterface;
        }

        public static IEnumerable<Type> AllClipTypes()
        {
            if (s_AllClipTypes == null)
            {
                s_AllClipTypes = EditorAssemblies.loadedTypes.Where(IsConcretePlayableAsset).ToArray();
            }
            return s_AllClipTypes;
        }

        public static IEnumerable<Type> GetAllMarkerTypes()
        {
            if (s_MarkerTypes == null)
                s_MarkerTypes = EditorAssemblies.loadedTypes.Where(x =>
                        typeof(XMarker).IsAssignableFrom(x) && !x.IsAbstract && !x.IsGenericType && !x.IsInterface)
                    .ToArray();
            return s_MarkerTypes;
        }

        public static IEnumerable<Type> AllTrackTypes()
        {
            if (s_AllTrackTypes == null)
            {
                s_AllTrackTypes = EditorAssemblies.loadedTypes
                    .Where(x => !x.IsAbstract && typeof(XTrack).IsAssignableFrom(x)).ToArray();
            }

            return s_AllTrackTypes;
        }


        public static void AllTrackDescriptor()
        {
            var types = AllTrackTypes();
            foreach (var type in types)
            {
                var desc = (TrackDescriptorAttribute) Attribute.GetCustomAttribute(type,
                    typeof(TrackDescriptorAttribute));
                if (desc != null)
                {
                    Debug.Log(desc.desc);
                }
            }
        }

        public static IEnumerable<Type> AllTracksExcMarkers()
        {
            var tracks = AllTrackTypes();
            return tracks.Where(x => x != typeof(XMarkerTrack));
        }

        public static List<Type> AllRootTrackExcMarkers()
        {
            List<Type> ret = new List<Type>();
            var tracks = AllTracksExcMarkers();
            foreach (var track in tracks)
            {
                var desc = (TrackDescriptorAttribute) Attribute.GetCustomAttribute(track,
                    typeof(TrackDescriptorAttribute));
                if (!desc.isOnlySub)
                {
                    ret.Add(track);
                }
            }
            return ret;
        }

        public static List<Type> GetRootChilds(Type trackType)
        {
            List<Type> list = new List<Type>();
            var tracks = AllTracksExcMarkers();
            foreach (var track in tracks)
            {
                var usage = (UseParentAttribute[]) Attribute.GetCustomAttributes(track, typeof(UseParentAttribute));
                foreach (var use in usage)
                {
                    if (use.parent == trackType)
                    {
                        list.Add(track);
                    }
                }
            }
            return list;
        }


        // [MenuItem("Tools/Test")]
        public static void FetchAllTrack()
        {
            var types = AllTracksExcMarkers();
            foreach (var type in types)
            {
                Debug.Log("track:" + type);
            }
            AllTrackDescriptor();
            types = GetAllMarkerTypes();
            foreach (var type in types)
            {
                Debug.Log("mark:" + type);
            }
            types = AllClipTypes();
            foreach (var type in types)
            {
                Debug.Log("clip: " + type);
            }
        }
    }
}
