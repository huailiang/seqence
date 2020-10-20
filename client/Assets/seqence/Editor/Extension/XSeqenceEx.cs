using UnityEngine.Seqence;
using UnityEngine.Seqence.Data;

namespace UnityEditor.Seqence
{

    public static class XSeqenceEx 
    {
        public static void BuildConf(this XSeqence seqence)
        {
            var tree = seqence.trackTrees;
            if (tree != null)
            {
                int len = tree.Length;
                seqence.config.tracks = new TrackData[len];
                for (int i = 0; i < len; i++)
                {
                    seqence.config.tracks[i] = tree[i].data;
                }
            }
        }

        public static void OnSave(this XSeqence seqence)
        {
            var tree = seqence.trackTrees;
            if (tree != null)
            {
                foreach (var track in tree)
                {
                    if (track.clips != null)
                    {
                        foreach (var clip in track.clips)
                        {
                            clip.OnSave();
                        }
                    }
                }
            }
        }
    }
}