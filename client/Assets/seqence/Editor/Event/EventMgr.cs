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

        public static void Emit(EventData d)
        {
            int len = tracks?.Count ?? 0;
            for (int i = 0; i < len; i++)
            {
                var track = tracks[i];
                track.RecvEvent(d);
            }
        }

        public static void EmitAll(EventData d)
        {
            int len = tracks?.Count ?? 0;
            for (int i = 0; i < len; i++)
            {
                var track = tracks[i];
                track.RecvEvent(d);
                foreach (var c in track.eClips)
                {
                    c.RecvEvent(d);
                }
            }
        }

    }
}
