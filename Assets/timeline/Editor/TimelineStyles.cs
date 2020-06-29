using UnityEngine;

namespace UnityEditor.Timeline
{
    class TimelineStyles
    {
        public readonly static GUIStyle bottomShadow = "Icon.Shadow";
        public readonly static GUIStyle lockedBG = "Icon.LockedBG";
        public readonly static GUIStyle fontClip = "Font.Clip";
        public readonly static GUIStyle mute = "Icon.Mute";
        public readonly static GUIStyle locked = "Icon.Locked";
        public readonly static GUIStyle autoKey = "Icon.AutoKey";
        public readonly static GUIStyle keyframe = "Icon.Keyframe";
        public static readonly GUIStyle timeCursor = "Icon.TimeCursor";
        public static readonly GUIStyle blendMixIn = "Icon.BlendMixIn";
        public static readonly GUIStyle blendMixOut = "Icon.BlendMixOut";
        public static readonly GUIStyle timelineClip = "Icon.Clip";

        public static readonly GUIContent sequenceSelectorIcon = EditorGUIUtility.IconContent("TimelineSelector");

        public static readonly GUIContent playContent =
            EditorGUIUtility.TrIconContent("Animation.Play", "Play the timeline (Space)");

        public static readonly GUIContent gotoBeginingContent =
            EditorGUIUtility.TrIconContent("Animation.FirstKey", "Go to the beginning of the timeline (Shift+<)");

        public static readonly GUIContent gotoEndContent =
            EditorGUIUtility.TrIconContent("Animation.LastKey", "Go to the end of the timeline (Shift+>)");

        public static readonly GUIContent nextFrameContent =
            EditorGUIUtility.TrIconContent("Animation.NextKey", "Go to the next frame");

        public static readonly GUIContent previousFrameContent =
            EditorGUIUtility.TrIconContent("Animation.PrevKey", "Go to the previous frame");

        public static readonly GUIContent noTimelineAssetSelected =
            EditorGUIUtility.TrTextContent("To start creating a timeline, select a GameObject");

        public static readonly GUIContent createTimelineOnSelection =
            EditorGUIUtility.TrTextContent("To begin a new timeline with {0}, create {1}");

        public static readonly GUIContent noTimelinesInScene =
            EditorGUIUtility.TrTextContent("No timeline found in the scene");

        public static readonly GUIContent createNewTimelineText =
            EditorGUIUtility.TrTextContent("Create a new Timeline to start");

        public static readonly GUIContent addContent = EditorGUIUtility.TrTextContent("Add", "Add new tracks.");

        public static readonly GUIContent previewContent =
            EditorGUIUtility.TrTextContent("Preview", "Enable/disable scene preview mode");

        public static readonly GUIContent inspectBtn =
            EditorGUIUtility.IconContent("TimelineEditModeMixON", "| timeline inspector");

        public static readonly GUIContent showMarkersOn = EditorGUIUtility.TrTextContentWithIcon(string.Empty,
            "Show / Hide Timeline Markers", "TimelineMarkerAreaButtonEnabled");

        public static readonly GUIContent showMarkersOff = EditorGUIUtility.TrTextContentWithIcon(string.Empty,
            "Show / Hide Timeline Markers", "TimelineMarkerAreaButtonDisabled");

        public static readonly GUIContent showMarkersOnTimeline = EditorGUIUtility.TrTextContent("Show markers");

        public static readonly GUIContent timelineMarkerTrackHeader =
            EditorGUIUtility.TrTextContentWithIcon("Markers", string.Empty, "TimelineHeaderMarkerIcon");

        public static readonly GUIContent markerCollapseButton =
            EditorGUIUtility.TrTextContent(string.Empty, "Expand / Collapse Track Markers");

        public static readonly GUIContent empty = new GUIContent();


        public readonly static Color colorDuration = new Color(0.66f, 0.66f, 0.66f, 0.4f);

        public readonly static Color BackgroundColor = new Color(0.2f, 0.2f, 0.2f, 1.0f);

        public readonly static Color colorTooltipBackground = new Color(29.0f / 255.0f, 32.0f / 255.0f, 33.0f / 255.0f);

        public readonly static Color colorSubSequenceBackground = new Color(0.129f, 0.176f, 0.176f, 1.0f);

        public readonly static Color colorSequenceBackground = new Color(0.16f, 0.16f, 0.16f, 1.0f);


        private static readonly string _edit_img = @"Assets/timeline/Editor/StyleSheets/Images/";

        private static Texture2D _new_ico, _save_ico, _open_ico,_warn_ico;

        public static Texture2D new_ico
        {
            get
            {
                if (_new_ico == null)
                {
                    _new_ico = AssetDatabase.LoadAssetAtPath<Texture2D>(_edit_img + "btn_editor_new.png");
                }
                return _new_ico;
            }
        }

        public static Texture2D open_ico
        {
            get
            {
                if (_open_ico == null)
                {
                    _open_ico = AssetDatabase.LoadAssetAtPath<Texture2D>(_edit_img + "btn_editor_open.png");
                }
                return _open_ico;
            }
        }

        public static Texture2D save_ico
        {
            get
            {
                if (_save_ico == null)
                {
                    _save_ico = AssetDatabase.LoadAssetAtPath<Texture2D>(_edit_img + "btn_editor_save.png");
                }
                return _save_ico;
            }
        }
        
        public static Texture2D warn_ico
        {
            get
            {
                if (_warn_ico == null)
                {
                    _warn_ico = AssetDatabase.LoadAssetAtPath<Texture2D>(_edit_img + "Warning-Overlay.png");
                }
                return _warn_ico;
            }
        }
        

        public static readonly GUIContent newContent = new GUIContent(new_ico, "new.");
        public static readonly GUIContent openContent = new GUIContent(open_ico, "open.");
        public static readonly GUIContent saveContent = new GUIContent(save_ico, "save.");

        private static GUIStyle _titleStyle, _labelStyle;

        public static GUIStyle titleStyle
        {
            get
            {
                if (_titleStyle == null)
                {
                    _titleStyle = new GUIStyle(EditorStyles.label);
                    _titleStyle.fontStyle = FontStyle.Bold;
                }
                return _titleStyle;
            }
        }

        public static GUIStyle btnLableStyle
        {
            get
            {
                if (_labelStyle == null)
                {
                    _labelStyle = new GUIStyle(EditorStyles.label);
                    _labelStyle.alignment = TextAnchor.MiddleLeft;
                    _labelStyle.fixedHeight = 18;
                    _labelStyle.padding.left = 10;
                }
                return _labelStyle;
            }
        }
    }
}
