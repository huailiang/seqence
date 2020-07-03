using UnityEngine;
using UnityEngine.Playables;

public class TestBehavior : PlayableBehaviour
{
    public string name;

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        base.PrepareFrame(playable, info);
        Debug.Log("errr");
    }
}


public class MixBehavior : PlayableBehaviour
{
  
}


public class STX : MonoBehaviour
{
    private PlayableGraph graph;
    private ScriptPlayable<MixBehavior> mixPlayable;
    private ScriptPlayable<TestBehavior> c1;

    void Start()
    {
        graph = PlayableGraph.Create("Test Graph");
        var o = ScriptPlayableOutput.Create(graph, "output");

        mixPlayable = ScriptPlayable<MixBehavior>.Create(graph, new MixBehavior { });
        c1 = ScriptPlayable<TestBehavior>.Create(graph, new TestBehavior {name = "v1"});

        c1.SetInputCount(1);
        o.SetSourcePlayable(mixPlayable, 0);
        graph.Connect(mixPlayable, 0, c1, 0);

        graph.Play();
    }

    private void OnDestroy()
    {
        graph.Destroy();
    }
}
