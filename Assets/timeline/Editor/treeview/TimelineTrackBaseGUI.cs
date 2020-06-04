using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    abstract class TimelineTrackBaseGUI : TreeViewItem, IBounds
    {

        public XTrack track { get; }

        public abstract Rect boundingRect { get; }

        public abstract void Draw(Rect headerRect, Rect contentRect);

        public abstract void OnGraphRebuilt();

        public virtual float GetVerticalSpacingBetweenTracks()
        {
            return 3.0f;
        }

        static class Styles
        {
            public static readonly GUIContent s_LockedAndMuted = EditorGUIUtility.TrTextContent("Locked / Muted");
            public static readonly GUIContent s_LockedAndPartiallyMuted = EditorGUIUtility.TrTextContent("Locked / Partially Muted");
            public static readonly GUIContent s_Locked = EditorGUIUtility.TrTextContent("Locked");
            public static readonly GUIContent s_Muted = EditorGUIUtility.TrTextContent("Muted");
            public static readonly GUIContent s_PartiallyMuted = EditorGUIUtility.TrTextContent("Partially Muted");
        }

        protected TimelineTrackBaseGUI(int id, int depth, TreeViewItem parent, string displayName, XTrack trackAsset)
           : base(id, depth, parent, displayName)
        {
            track = trackAsset;
        }

        protected TimelineTrackBaseGUI(XTrack track)
        {
            this.track = track;
        }


        static void DrawLockTrackBG(Rect trackRect)
        {
            var texture = TimelineStyles.lockedBG.normal.background;
            Graphics.DrawTextureRepeated(trackRect, texture);
        }

        public void DrawLockState(XTrack track, Rect trackRect)
        {
            if (track.locked)
            {
                DrawLockTrackBG(trackRect);
                DrawTrackStateBox(trackRect, track);
            }
        }

        protected static void DrawTrackStateBox(Rect trackRect, XTrack track)
        {
            const float k_LockTextPadding = 40f;

            bool locked = track.locked && !track.parentLocked;
            bool muted = track.mute && !track.parentMute;
            bool allSubTrackMuted = track.IsAllSubTrackMuted();

            GUIContent content = null;
            if (locked && muted)
            {
                content = Styles.s_LockedAndMuted;
                if (!allSubTrackMuted)
                    content = Styles.s_LockedAndPartiallyMuted;
            }
            else if (locked) content = Styles.s_Locked;
            else if (muted)
            {
                content = Styles.s_Muted;
                if (!allSubTrackMuted)
                    content = Styles.s_PartiallyMuted;
            }

            // the track could be locked, but we only show the 'locked portion' on the upper most track
            //  that is causing the lock
            if (content == null)
                return;

            var textRect = trackRect;
            textRect.width = TimelineStyles.fontClip.CalcSize(content).x + k_LockTextPadding;
            textRect.x += (trackRect.width - textRect.width) / 2f;
            textRect.height -= 4f;
            textRect.y += 2f;

            using (new GUIColorOverride(TimelineStyles.colorLockTextBG))
            {
                GUI.Box(textRect, GUIContent.none, TimelineStyles.displayBackground);
            }

            Timeline.Graphics.ShadowLabel(textRect, content, TimelineStyles.fontClip, Color.white, Color.black);
        }
    }
}