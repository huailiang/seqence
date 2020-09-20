using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
#if UNITY_EDITOR
using UnityEditor;

#endif
namespace UnityEngine.Seqence
{
    public class CurveBind<T> where T : struct
    {
        public string key;

        public T v;

        public Action<T> cb;

        public CurveBind(string key, Action<T> cb = null)
        {
            this.key = key;
            this.cb = cb;
        }
    }

    public struct XFrame<T> where T : struct
    {
        public float t;
        public T v;

        public XFrame(float time, T value)
        {
            this.v = value;
            this.t = time;
        }
    }

    public class XCurve<T> where T : struct
    {
        public string bind;
        public XFrame<T>[] frames;
        public int length;

        public XCurve(int len, string bind)
        {
            length = len;
            this.bind = bind;
            frames = new XFrame<T>[len];
        }

        public XCurve(int len, CurveBind<T> cb)
        {
            length = len;
            this.bind = cb.key;
            frames = new XFrame<T>[len];
        }

        public T Sample(float time)
        {
            if (time <= frames[0].t)
            {
                return frames[0].v;
            }
            if (time >= frames[length - 1].t)
            {
                return frames[length - 1].v;
            }
            for (int i = 1; i < length; i++)
            {
                if (time < frames[i].t && time > frames[i - 1].t)
                {
                    float p = (time - frames[i - 1].t) / (frames[i].t - frames[i - 1].t);
                    T t1 = frames[i - 1].v;
                    T t2 = frames[i].v;
                    T diff = (dynamic) t2 - t1;
                    return t1 + (dynamic) diff * p; //lerp
                }
            }
            return default(T);
        }

        public void AddKey(float time, T v, int idx)
        {
            var n = new XFrame<T>[length + 1];
            for (int i = 0; i <= length; i++)
            {
                if (i < idx)
                    n[i] = frames[i];
                else if (i == idx)
                    n[i] = new XFrame<T>(time, v);
                else
                    n[i] = frames[i - 1];
            }
            frames = n;
            length++;
        }

        public void AddOrUpdateKey(float time, T v)
        {
            int idx = length;
            if (length > 0 && time < frames[0].t)
            {
                idx = 0;
            }
            else
            {
                for (int i = 1; i < length; i++)
                {
                    if (time > frames[i - 1].t && time < frames[i].t)
                    {
                        idx = i;
                        break;
                    }
                    else if (frames[i].t == time)
                    {
                        idx = -1;
                        frames[i].v = v;
                        break;
                    }
                }
            }
            if (idx != -1)
            {
                AddKey(time, v, idx);
            }
        }

        public void RemvKey(float time)
        {
            for (int i = 0; i < length; i++)
            {
                if (frames[i].t == time)
                {
                    RemvAt(i);
                    break;
                }
            }
        }

        public void RemvAt(int index)
        {
            if (index < length)
            {
                frames = SeqenceUtil.Remv(frames, index);
                length--;
            }
        }
    }

    public class XAnimation
    {
        public Dictionary<string, object> curves = new Dictionary<string, object>();

        public void AddKey<T>(float t, CurveBind<T> bind) where T : struct
        {
            if (curves.ContainsKey(bind.key))
            {
                (curves[bind.key] as XCurve<T>).AddOrUpdateKey(t, bind.v);
            }
            else
            {
                XCurve<T> cv = new XCurve<T>(1, bind);
                cv.frames[0] = new XFrame<T>(t, bind.v);
                curves.Add(bind.key, cv);
            }
        }

        public bool RemvAt<T>(string bind, int i) where T : struct
        {
            if (curves.ContainsKey(bind))
            {
                (curves[bind] as XCurve<T>).RemvAt(i);
                return true;
            }
            return false;
        }

        public void Remove<T>(string bind, float t) where T : struct
        {
            if (curves.ContainsKey(bind))
            {
                var cv = curves[bind] as XCurve<T>;
                int len = cv.length;
                for (int i = 0; i < len; i++)
                {
                    if (cv.frames[i].t == t)
                    {
                        cv.RemvAt(i);
                        break;
                    }
                }
            }
        }

        public T Sample<T>(string key, float t) where T : struct
        {
            return (curves[key] as XCurve<T>).Sample(t);
        }

        public void Sample<T>(CurveBind<T> bind, float t) where T : struct
        {
            if (HasKey(bind.key))
            {
                bind.v = Sample<T>(bind.key, t);
                bind.cb(bind.v);
            }
        }

        public bool HasKey(string key)
        {
            return curves.ContainsKey(key);
        }

        public bool HasKey<T>(CurveBind<T> b) where T : struct
        {
            return HasKey(b.key);
        }
    }

    public abstract class CurveBindObject
    {
        protected XAnimation animation = new XAnimation();

        public abstract void Evaluate(float t);

        public abstract void Inspector();

        protected static void Draw<T>(CurveBind<T> b) where T : struct
        {
#if UNITY_EDITOR
            if (typeof(T) == typeof(float))
            {
                b.v = EditorGUILayout.FloatField(b.key, (dynamic) b.v);
            }
            else if (typeof(T) == typeof(Color))
            {
                b.v = EditorGUILayout.ColorField(b.key, (dynamic) b.v);
            }
            else if (typeof(T) == typeof(Vector2))
            {
                b.v = EditorGUILayout.Vector2Field(b.key, (dynamic) b.v);
            }
            else if (typeof(T) == typeof(Vector3))
            {
                b.v = EditorGUILayout.Vector3Field(b.key, (dynamic) b.v);
            }
            else if (typeof(T) == typeof(Vector4))
            {
                b.v = EditorGUILayout.Vector4Field(b.key, (dynamic) b.v);
            }
#endif
        }
    }

    public class CurveBindObject<T> : CurveBindObject where T : struct
    {
        public List<CurveBind<T>> binds = new List<CurveBind<T>>();

        public void Add(CurveBind<T> b)
        {
            binds.Add(b);
        }

        public override void Evaluate(float t)
        {
            for (int i = 0; i < binds.Count; i++)
            {
                animation.Sample(binds[i], t);
            }
        }

        public override void Inspector()
        {
            for (int i = 0; i < binds.Count; i++)
            {
                Draw(binds[i]);
            }
        }

        public byte[] ToBytes()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, binds);
                return ms.GetBuffer();
            }
        }

        public void ToObject(byte[] Bytes)
        {
            using (MemoryStream ms = new MemoryStream(Bytes))
            {
                IFormatter formatter = new BinaryFormatter();
                binds = (List<CurveBind<T>>) formatter.Deserialize(ms);
            }
        }
    }

    public class CurveBindObject<T1, T2> : CurveBindObject where T1 : struct where T2 : struct
    {
        public List<CurveBind<T1>> bind1 = new List<CurveBind<T1>>();
        public List<CurveBind<T2>> bind2 = new List<CurveBind<T2>>();

        public void Add(CurveBind<T1> b)
        {
            bind1.Add(b);
        }

        public void Add(CurveBind<T2> b)
        {
            bind2.Add(b);
        }

        public override void Evaluate(float t)
        {
            for (int i = 0; i < bind1.Count; i++)
            {
                animation.Sample(bind1[i], t);
            }
            for (int i = 0; i < bind2.Count; i++)
            {
                animation.Sample(bind2[i], t);
            }
        }

        public override void Inspector()
        {
            for (int i = 0; i < bind1.Count; i++)
            {
                Draw(bind1[i]);
            }
            for (int i = 0; i < bind2.Count; i++)
            {
                Draw(bind2[i]);
            }
        }
    }

    public class CurveBindObject<T1, T2, T3> : CurveBindObject where T1 : struct where T2 : struct where T3 : struct
    {
        private List<CurveBind<T1>> bind1 = new List<CurveBind<T1>>();
        private List<CurveBind<T2>> bind2 = new List<CurveBind<T2>>();
        private List<CurveBind<T3>> bind3 = new List<CurveBind<T3>>();

        public void Add(CurveBind<T1> b)
        {
            bind1.Add(b);
        }

        public void Add(CurveBind<T2> b)
        {
            bind2.Add(b);
        }

        public void Add(CurveBind<T3> b)
        {
            bind3.Add(b);
        }

        public override void Evaluate(float t)
        {
            for (int i = 0; i < bind1.Count; i++)
            {
                animation.Sample(bind1[i], t);
            }
            for (int i = 0; i < bind2.Count; i++)
            {
                animation.Sample(bind2[i], t);
            }
            for (int i = 0; i < bind3.Count; i++)
            {
                animation.Sample(bind3[i], t);
            }
        }

        public override void Inspector()
        {
            for (int i = 0; i < bind1.Count; i++)
            {
                Draw(bind1[i]);
            }
            for (int i = 0; i < bind2.Count; i++)
            {
                Draw(bind2[i]);
            }
            for (int i = 0; i < bind3.Count; i++)
            {
                Draw(bind3[i]);
            }
        }
    }
}
