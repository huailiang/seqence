using System;
using System.Collections.Generic;
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
        public float t { get; set; }
        public T v;

        public XFrame(float time, T value)
        {
            this.v = value;
            this.t = time;
        }
    }

    public interface ICurve
    {
        HashSet<float> keyTimes { get; }

        void MoveTime(float t);

        void UpdateTime(float start, float scale);
    }

    public class XCurve<T> : ICurve where T : struct
    {
        public string bind;
        public XFrame<T>[] frames { get; set; }
        public int length;

        public HashSet<float> keyTimes
        {
            get
            {
                HashSet<float> rt = new HashSet<float>();
                int len = frames?.Length ?? 0;
                for (int i = 0; i < len; i++)
                {
                    rt.Add(frames[i].t);
                }
                return rt;
            }
        }

        public void MoveTime(float t)
        {
            int len = frames?.Length ?? 0;
            for (int i = 0; i < len; i++)
            {
                frames[i].t += t;
            }
        }

        public void UpdateTime(float start, float scale)
        {
            int len = frames?.Length ?? 0;
            for (int i = 0; i < len; i++)
            {
                float t = frames[i].t;
                frames[i].t = start + (t - start) * scale;
            }
        }

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
        public Dictionary<string, ICurve> curves = new Dictionary<string, ICurve>();

        public HashSet<float> GetAllKeyTimes()
        {
            HashSet<float> list = new HashSet<float>();
            foreach (var curve in curves)
            {
                foreach (var t in curve.Value.keyTimes)
                {
                    list.Add(t);
                }
            }
            return list;
        }

        public void UpdateAllKeyTimes(float start, float scale)
        {
            foreach (var cv in curves)
            {
                cv.Value.UpdateTime(start, scale);
            }
        }

        public void Move(float delta)
        {
            foreach (var curve in curves)
            {
                curve.Value.MoveTime(delta);
            }
        }

        public void AddKey<T>(float t, CurveBind<T> bind) where T : struct
        {
            AddKey(t, bind.key, bind.v);
        }

        public void AddKey<T>(float t, string key, T v) where T : struct
        {
            if (curves.ContainsKey(key))
            {
                (curves[key] as XCurve<T>).AddOrUpdateKey(t, v);
            }
            else
            {
                XCurve<T> cv = new XCurve<T>(1, key);
                cv.frames[0] = new XFrame<T>(t, v);
                curves.Add(key, cv);
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
        private static Color old;

        protected XAnimation animation = new XAnimation();

        public abstract void Evaluate(float t);

        public abstract void Inspector(bool recdMode, float time);

        protected void Draw<T>(CurveBind<T> b, bool recdMode, float time) where T : struct
        {
#if UNITY_EDITOR
            if (recdMode)
            {
                old = GUI.color;
                GUI.color = Color.red;
                UnityEditor.EditorGUI.BeginChangeCheck();
            }
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

            if (recdMode)
            {
                if (UnityEditor.EditorGUI.EndChangeCheck())
                {
                    AddKey(time, b);
                }
                GUI.color = old;
            }
#endif
        }

        public Dictionary<string, ICurve> GetAnimInfo()
        {
            return animation.curves;
        }
        
        public void AddKey<T>(float time, CurveBind<T> b) where T : struct
        {
            if (time >= 0) this.animation.AddKey(time, b);
        }

        public void AddKey<T>(float time, string k, T v) where T : struct
        {
            if (time >= 0) this.animation.AddKey(time, k, v);
        }

        public HashSet<float> GetAllKeyTimes()
        {
            return animation.GetAllKeyTimes();
        }

        public void UpdateAllKeyTimes(float start, float scale)
        {
            animation.UpdateAllKeyTimes(start, scale);
        }

        public void Move(float delta)
        {
            animation?.Move(delta);
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


        public override void Inspector(bool recdMode, float time)
        {
            for (int i = 0; i < binds.Count; i++)
            {
                Draw(binds[i], recdMode, time);
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

        public override void Inspector(bool recd, float time)
        {
            for (int i = 0; i < bind1.Count; i++)
            {
                Draw(bind1[i], recd, time);
            }
            for (int i = 0; i < bind2.Count; i++)
            {
                Draw(bind2[i], recd, time);
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

        public override void Inspector(bool recdMode, float time)
        {
            for (int i = 0; i < bind1.Count; i++)
            {
                Draw(bind1[i], recdMode, time);
            }
            for (int i = 0; i < bind2.Count; i++)
            {
                Draw(bind2[i], recdMode, time);
            }
            for (int i = 0; i < bind3.Count; i++)
            {
                Draw(bind3[i], recdMode, time);
            }
        }
    }
}
