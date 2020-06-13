using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;

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
                var flag = (TrackFlagAttribute) Attribute.GetCustomAttribute(track,
                    typeof(TrackFlagAttribute));
                if (!flag.isOnlySub)
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

        public static List<Type> GetBelongMarks(TrackType trackType)
        {
            List<Type> list = new List<Type>();
            var marks = GetAllMarkerTypes();
            foreach (var mark in marks)
            {
                var usage = (MarkUsageAttribute) Attribute.GetCustomAttribute(mark, typeof(MarkUsageAttribute));
                if (usage != null)
                    if ((usage.type & trackType) > 0)
                    {
                        list.Add(mark);
                    }
            }
            return list;
        }

        public static Type GetEditorAsset(Type at)
        {
            var a = Assembly.GetExecutingAssembly();
            var types = a.GetTypes();
            foreach (var type in types)
            {
                var usage = (TimelineEditorAttribute) Attribute.GetCustomAttribute(type,
                    typeof(TimelineEditorAttribute));
                if (usage != null && usage.type == at)
                {
                    return type;
                }
            }
            return null;
        }
    }
}
