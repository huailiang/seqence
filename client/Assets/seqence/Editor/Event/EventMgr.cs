using System.Collections.Generic;


namespace UnityEditor.Seqence
{
    public class EventMgr
    {
        public static EditorTrackTree tree
        {
            get { return SeqenceWindow.inst.tree; }
        }

        public static List<EditorTrack> tracks
        {
            get { return tree?.hierachy; }
        }

        public static void Send(EventData d)
        {
            int len = tracks?.Count ?? 0;
            for (int i = 0; i < len; i++)
            {
                var track = tracks[i];
                track.RecvEvent(d);
            }
        }
    }
}
