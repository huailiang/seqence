using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    public class SelectionManager
    {
        private static List<XTrack> selections = new List<XTrack>();

        public static void AddObject(Object obj)
        {
            if (obj == null) return;
            
            if (Selection.Contains(obj)) return;
            Selection.Add(obj);
        }

        public static void Add(XTrack obj)
        {
            if (obj == null) return;
            if (!selections.Contains(obj))
            {
                selections.Add(obj);
            }
        }
        
        public static void Add(IClip item)
        {
            // AddObject(EditorClipFactory.GetEditorClip(item));
        }

        public static void Add(XMarker marker)
        {
            // var markerAsObject = marker as Object;
            // if (markerAsObject != null)
            // {
            //     if (!Selection.Contains(markerAsObject))
            //     {
            //         currentInlineEditorCurve = null;
            //         WindowState state = null;
            //         if (TimelineWindow.instance != null)
            //             state = TimelineWindow.instance.state;
            //
            //         if (!Selection.instanceIDs.Any() && state != null && state.editSequence.director != null)
            //             Selection.SetActiveObjectWithContext(markerAsObject, TimelineWindow.instance.state.editSequence.director);
            //         else
            //             Selection.Add(markerAsObject);
            //     }
            // }
        }
    }
}
