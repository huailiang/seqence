using System;
using UnityEngine;
using UnityEngine.Seqence;

namespace UnityEditor.Seqence
{
    public partial class SeqenceWindow
    {
        void TimelineHeaderGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal(GUILayout.Width(WindowConstants.sliderWidth));
            AddButtonGUI();
            ShowMarkersButton();
            GUILayout.EndHorizontal();
            GUILayout.Label(SeqenceStyle.timelineMarkerTrackHeader);

            GUILayout.EndVertical();
        }


        void AddButtonGUI()
        {
            if (EditorGUILayout.DropdownButton(SeqenceStyle.addContent, FocusType.Passive, "Dropdown"))
            {
                GenCustomMenu();
            }
        }

        public void GenCustomMenu()
        {
            GenericMenu pm = new GenericMenu();
            bool cd = (state.seqence.trackTrees.Length > 1);

            if (SeqenceWindow.inst.tree.AnySelect())
            {
                pm.AddItem(EditorGUIUtility.TrTextContent("UnSelect All tracks \t #u"), false, tree.ResetSelect, false);
                pm.AddDisabledItem(EditorGUIUtility.TrTextContent("Select All tracks \t %s"));
            }
            else
            {
                pm.AddDisabledItem(EditorGUIUtility.TrTextContent("UnSelect All tracks \t %u"));
                pm.AddItem(EditorGUIUtility.TrTextContent("Select All tracks \t #s"), false, tree.ResetSelect, true);
            }

            if (cd)
            {
                pm.AddItem(EditorGUIUtility.TrTextContent("Mute All  tracks \t #m"), false, MuteAll);
                pm.AddItem(EditorGUIUtility.TrTextContent("Lock All tracks \t #l"), false, LockAll);
            }
            else
            {
                pm.AddDisabledItem(EditorGUIUtility.TrTextContent("Mute All  tracks \t #m"), false);
                pm.AddDisabledItem(EditorGUIUtility.TrTextContent("Lock All tracks \t #l"), false);
            }
            var paste = EditorGUIUtility.TrTextContent("Paste Track\t #p");
            if (EditorTrack.clipboardTrack != null)
            {
                pm.AddItem(paste, false, PasteTrack);
            }
            else
            {
                pm.AddDisabledItem(paste, false);
            }
            pm.AddSeparator("");
            pm.AddItem(EditorGUIUtility.TrTextContent("Add XGroupTrack"), false, OnAddTrackItem, typeof(XGroupTrack));

            var types = TypeUtilities.AllRootTrackExcMarkers();
            for (int i = 0; i < types.Count; i++)
            {
                string str = types[i].ToString();
                int idx = str.LastIndexOf('.');
                if (idx >= 0)
                {
                    str = "Add " + str.Substring(idx + 1);
                }
                pm.AddItem(EditorGUIUtility.TrTextContent(str), false, OnAddTrackItem, types[i]);
            }

            Rect rect = new Rect(Event.current.mousePosition, new Vector2(200, 0));
            pm.DropDown(rect);
        }

        private void PasteTrack()
        {
            var clipboard = EditorTrack.clipboardTrack;
            XTrack copyed = clipboard.Clone();
            if (clipboard.parent != null)
            {
                string tip = "select a parent track with type: " + clipboard.parent.AssetType;
                EditorUtility.DisplayDialog("Notice", tip, "OK");
            }
            else
            {
                SeqenceWindow.inst.timeline.AddRootTrack(copyed);
                int idx = tree.hierachy.Count;
                tree.AddTrack(copyed, idx, null, true);
                clipboard = null;
            }
        }

        private void MuteAll()
        {
            var xtree = state.seqence.trackTrees;
            for (int i = 1; i < xtree.Length; i++)
            {
                xtree[i].SetFlag(TrackMode.Mute, false);
            }
        }

        private void LockAll()
        {
            var xtree = state.seqence.trackTrees;
            for (int i = 1; i < xtree.Length; i++)
            {
                xtree[i].SetFlag(TrackMode.Lock, false);
            }
        }

        private void OnAddTrackItem(object arg)
        {
            Type type = (Type) arg;
            EditorFactory.GetTrackByDataType(type, state.seqence, null, (track, data, param) =>
            {
                if (track != null && data != null)
                {
                    tree.AddTrack(track, param);
                    state.seqence.AddRootTrack(track);
                }
            });
        }

        void ShowMarkersButton()
        {
            GUILayout.Toggle(true, SeqenceStyle.showMarkersOn, GUI.skin.button);
        }
        
    }
}
