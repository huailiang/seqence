using UnityEngine;
#if UNITY_2019_3_OR_NEWER
using UnityEngine.Animations;
#else
using UnityEngine.Experimental.Animations;
#endif
using UnityEngine.Timeline;
using UnityEngine.Timeline.Data;
using UnityEngine.Playables;

public class Entity : ISharedObject<Entity>
{
    private GameObject go;
    private Animator ator;
    private uint uid;
    private Character ch;
    private XTimeline timeline;


    public Entity next { get; set; }

    public uint UID { get { return uid; } }

    public Character Character { get { return ch; } }

    public Animator Ator { get { return ator; } }

    public void Initial(uint uid, Character ch)
    {
        this.uid = uid;
        this.ch = ch;
        Load();
    }

    private void Load()
    {
        go = XResources.LoadGameObject(ch.prefab);
        ator = go.GetComponent<Animator>();
    }

    public void Dispose()
    {
        timeline?.Dispose();
        XResources.DestroyGameObject(ch.prefab, go);
    }

    public void SetPos(Vector3 pos)
    {
        if (go != null)
        {
            go.transform.localPosition = pos;
        }
    }

    public void SetRot(Quaternion rot)
    {
        if (go != null)
        {
            go.transform.localRotation = rot;
        }
    }


    public void SetScale(Vector3 scale)
    {
        if (go)
        {
            go.transform.localScale = scale;
        }
    }

    public void PlaySkill(string skill)
    {
        string path = "Assets/skill/" + skill + ".xml";
        if (timeline == null)
        {
            timeline = new XTimeline(path, UnityEngine.Timeline.PlayMode.Skill, ator);
            timeline.SetPlaying(true);
        }
        else
        {
            timeline.BlendTo(path);
        }
    }

}