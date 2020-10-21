using UnityEngine;

namespace UnityEditor.Seqence
{
    class SeqenceStyle
    {
        public readonly static GUIStyle bottomShadow = "Icon.Shadow";
        public readonly static GUIStyle lockedBG = "Icon.LockedBG";
        public readonly static GUIStyle keyframe = "Icon.Keyframe";
        public static readonly GUIStyle displayBackground = "sequenceClip";

        private static GUIStyle _fontStyle;
        private static GUIStyle _timeCursor;
        private static GUIStyle _icoStyle;
        private static GUIStyle _keyStyle;

        public static GUIStyle fontClip
        {
            get
            {
                if (_fontStyle == null)
                {
                    _fontStyle = new GUIStyle();
                    _fontStyle.alignment = TextAnchor.MiddleCenter;
                    _fontStyle.imagePosition = ImagePosition.TextOnly;
                    _fontStyle.richText = false;
                    _fontStyle.stretchWidth = false;
                    _fontStyle.clipping = TextClipping.Clip;
                    _fontStyle.fontSize = 10;
                }
                _fontStyle.normal.textColor = pro ? Color.white : Color.black;
                return _fontStyle;
            }
        }

        public static GUIStyle timeCursor
        {
            get
            {
                if (_timeCursor == null)
                {
                    _timeCursor = new GUIStyle();
                    _timeCursor.alignment = TextAnchor.MiddleCenter;
                    _timeCursor.fixedHeight = 28;
                    _timeCursor.fixedWidth = 24;
                    _timeCursor.margin.left = 1;
                    _timeCursor.margin.right = 1;
                    _timeCursor.margin.top = 1;
                    _timeCursor.margin.bottom = 1;
                    _timeCursor.name = "Icon.TimeCursor";
                    _timeCursor.padding.left = -14;
                    _timeCursor.padding.right = 0;
                    _timeCursor.padding.top = -2;
                    _timeCursor.padding.bottom = 4;
                    _timeCursor.richText = false;
                    _timeCursor.stretchWidth = false;
                    _timeCursor.normal.textColor = Color.white;
                }
                return _timeCursor;
            }
        }

        public static GUIStyle icoStyle
        {
            get
            {
                if(_icoStyle==null)
                {
                    _icoStyle = new GUIStyle();
                    _icoStyle.alignment = TextAnchor.MiddleCenter;
                    _icoStyle.fixedHeight = 20;
                    _icoStyle.fixedWidth = 20;
                    _icoStyle.stretchWidth = false;
                }
                return _icoStyle;
            }
        }

        public static GUIStyle keyStyle
        {
            get
            {
                if (_keyStyle == null)
                {
                    _keyStyle = new GUIStyle();
                    _keyStyle.alignment = TextAnchor.UpperCenter;
                    _keyStyle.fixedHeight = 20;
                    _keyStyle.fixedWidth = 8;
                    _keyStyle.stretchWidth = false;
                }
                return _keyStyle;
            }
        }

        public static readonly GUIContent sequenceSelectorIcon = EditorGUIUtility.IconContent("CreateAddNew");

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

        public static readonly GUIContent createNewSeqenceText =
            EditorGUIUtility.TrTextContent("Create a new Seqence to start");

        public static readonly GUIContent addContent = EditorGUIUtility.TrTextContent("Add", "Add new tracks.");


        public static readonly GUIContent empty = new GUIContent();


        public readonly static Color addtiveClip = new Color(0.1f, 0.1f, 0.1f, 0);

        public readonly static Color colorDuration = new Color(0.66f, 0.66f, 0.66f, 0.4f);

        private readonly static Color backgroundColor1 = new Color(0.2f, 0.2f, 0.2f, 1.0f);

        private readonly static Color backgroundColor2 = new Color(0.8f, 0.9f, 0.8f, 1.0f);

        private readonly static Color markerColor = new Color(0.8f, 0.8f, 0.8f, 1);

        private static bool pro { get { return EditorGUIUtility.isProSkin; } }

        public static Color BackgroundColor
        {
            get { return pro ? backgroundColor1 : backgroundColor2; }
        }

        public static Color MarkBackground
        {
            get { return pro ? backgroundColor1 : markerColor; }
        }

        public readonly static Color colorTooltipBackground = new Color(29.0f / 255.0f, 32.0f / 255.0f, 33.0f / 255.0f);

        public readonly static Color colorSubSequenceBackground = new Color(0.13f, 0.17f, 0.17f, 1.0f);

        public readonly static Color colorSequenceBackground = new Color(0.16f, 0.16f, 0.16f, 1.0f);

        public readonly static Color colorEndLine = new Color(0.8f, 0.1f, 0, 0.5f);

        private static readonly string _edit_img = @"Assets/seqence/Editor/StyleSheets/Images/";

        private static string _time_img
        {
            get { return _edit_img + (pro ? "DarkSkin/" : "LightSkin/"); }
        }

        private static Texture2D _new_ico, _save_ico, _open_ico, _warn_ico;
        private static Texture2D _mark_ico, _inspect_ico, _refresh_ico,_eye_ico;
        private static Texture2D _autokey_ico, _autokey_ico2, _lock_ico, _lockBg_ico;
        private static Texture2D _key_ico, _clipin_ico, _clipOut_ico;

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

        public static Texture2D eye_ico
        {
            get
            {
                if(_eye_ico==null)
                {
                    _eye_ico = AssetDatabase.LoadAssetAtPath<Texture2D>(_time_img + "TimelineEye.png");
                }
                return _eye_ico;
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

        public static Texture2D refresh_ico
        {
            get
            {
                if (_refresh_ico == null)
                {
                    _refresh_ico = AssetDatabase.LoadAssetAtPath<Texture2D>(_edit_img + "btn_editor_refresh.png");
                }
                return _refresh_ico;
            }
        }

        public static Texture2D mark_ico
        {
            get
            {
                if (_mark_ico == null)
                {
                    _mark_ico = AssetDatabase.LoadAssetAtPath<Texture2D>(_edit_img + "mark.png");
                }
                return _mark_ico;
            }
        }

        private static Texture2D inspect_ico
        {
            get
            {
                if (_inspect_ico == null)
                {
                    _inspect_ico = AssetDatabase.LoadAssetAtPath<Texture2D>(_edit_img + "inspect.png");
                }
                return _inspect_ico;
            }
        }

        private static Texture2D autokey_ico
        {
            get
            {
                if (_autokey_ico == null)
                {
                    _autokey_ico = AssetDatabase.LoadAssetAtPath<Texture2D>(_time_img + "TimelineAutokey.png");
                }
                return _autokey_ico;
            }
        }

        private static Texture2D autokey_ico2
        {
            get
            {
                if (_autokey_ico2 == null)
                {
                    _autokey_ico2 = AssetDatabase.LoadAssetAtPath<Texture2D>(_time_img + "TimelineAutokey_active.png");
                }
                return _autokey_ico2;
            }
        }

        private static Texture2D lock_ico
        {
            get
            {
                if(_lock_ico==null)
                {
                    _lock_ico = AssetDatabase.LoadAssetAtPath<Texture2D>(_time_img + "TimelineLockButton.png");
                }
                return _lock_ico;
            }
        }

        private static Texture2D lockBg_ico
        {
            get
            {
                if (_lockBg_ico == null)
                {
                    _lockBg_ico = AssetDatabase.LoadAssetAtPath<Texture2D>(_edit_img + "Icons/TimelineClipBG.png");
                }
                return _lockBg_ico;
            }
        }

        private static Texture2D key_ico
        {
            get
            {
                if(_key_ico==null)
                {
                    _key_ico = AssetDatabase.LoadAssetAtPath<Texture2D>(_time_img + "TimelineKeyframe.png");
                }
                return _key_ico;
            }
        }

        private static Texture2D clipIn_ico
        {
            get
            {
                if(_clipin_ico==null)
                {
                    _clipin_ico = AssetDatabase.LoadAssetAtPath<Texture2D>(_time_img + "TimelineIconClipIn.png");
                }
                return _clipin_ico;
            }
        }

        private static Texture2D clipOut_ico
        {
            get
            {
                if (_clipOut_ico == null)
                {
                    _clipOut_ico = AssetDatabase.LoadAssetAtPath<Texture2D>(_time_img + "TimelineIconClipIn.png");
                }
                return _clipOut_ico;
            }
        }


        public static readonly GUIContent newContent = new GUIContent(new_ico, "new.");
        public static readonly GUIContent openContent = new GUIContent(open_ico, "open.");
        public static readonly GUIContent saveContent = new GUIContent(save_ico, "save.");
        public static readonly GUIContent refreshContent = new GUIContent(refresh_ico, "refresh.");
        public static readonly GUIContent inpectContent = new GUIContent(inspect_ico, "inspect");
        public static readonly GUIContent autokeyContentOff = new GUIContent(autokey_ico, "autokeyOff");
        public static readonly GUIContent autokeyContentOn = new GUIContent(autokey_ico2, "autokeyOn");
        public static readonly GUIContent lockContent = new GUIContent(lock_ico, "locked");
        public static readonly GUIContent keyFrameContent = new GUIContent(key_ico, "keyframe");
        public static readonly GUIContent muteContent = new GUIContent(eye_ico, "mute");
        public static readonly GUIContent clipInContent = new GUIContent(clipIn_ico, "");
        public static readonly GUIContent clipOutContent = new GUIContent(clipOut_ico, "");

        private static GUIStyle _titleStyle, _labelStyle, _boldFoldStyle;

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

        public static GUIStyle boldFoldStyle
        {
            get
            {
                if (_boldFoldStyle == null)
                {
                    _boldFoldStyle = new GUIStyle(EditorStyles.foldout);
                    _boldFoldStyle.fontStyle = FontStyle.Bold;
                }
                return _boldFoldStyle;
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
