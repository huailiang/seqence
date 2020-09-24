using System.Collections.Generic;
using System.IO;
using UnityEngine.Seqence.Data;
#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine.Rendering.PostProcessing;
#endif

namespace UnityEngine.Seqence
{
    public class XPostprocessClip : XClip<XPostprocessTrack, XPostprocessClip, PostprocessData>,
        ISharedObject<XPostprocessClip>
    {
        public PostProcessEffectSettings setting;

        public override string Display
        {
            get { return data?.ToString() ?? "post process"; }
        }

        public override void OnDestroy()
        {
            SharedPool<XPostprocessClip>.Return(this);
            base.OnDestroy();
        }

        public void OnBuild()
        {
            CreateInstance(data.mode);
            ReadFromBuffer();
        }

        public CurveBindObject curveBindObject;

        protected override void OnUpdate(float tick, bool mix)
        {
            base.OnUpdate(tick, mix);
            curveBindObject?.Evaluate(start + tick);
        }

#if UNITY_EDITOR

        public void OnInspector(Action cb)
        {
            EditorGUI.BeginChangeCheck();
            if (data != null)
            {
                data.mode = (PostEnum) EditorGUILayout.EnumPopup("Effect", data.mode);
            }
            if (EditorGUI.EndChangeCheck() || setting == null)
            {
                if (data.mode > 0)
                {
                    CreateInstance(data.mode);
                    int idx = (int)data.mode;
                    var profile = seqence.postProfile.settings;
                    if (idx < profile.Count)
                    {
                        setting = profile[idx];
                        cb();
                    }
                }
            }

            float t = seqence.Time;
            bool recdMode = track.record & t >= start && t <= end;
            switch (data.mode)
            {
                case PostEnum.Bloom:
                    InspectorBloom(recdMode);
                    break;
                case PostEnum.Vignette:
                    InspectorVignette(recdMode);
                    break;
                case PostEnum.MotionBlur:
                    InspectorMotionBlur(recdMode);
                    break;
                case PostEnum.DepthOfField:
                    InspectorDepthField(recdMode);
                    break;
                case PostEnum.ColorGrading:
                    InspectorColorGrad(recdMode);
                    break;
            }
        }

        private void InspectorBloom(bool recdMode)
        {
            var bloomBinding = curveBindObject as CurveBindObject<float, Color>;
            bloomBinding?.Inspector(recdMode, seqence.Time);
        }

        private void InspectorVignette(bool recdMode)
        {
            var bind = curveBindObject as CurveBindObject<float, Vector2, Color>;
            bind?.Inspector(recdMode, seqence.Time);
        }

        private void InspectorMotionBlur(bool recdMode)
        {
            var bind = curveBindObject as CurveBindObject<float>;
            bind?.Inspector(recdMode, seqence.Time);
        }

        private void InspectorDepthField(bool recdMode)
        {
            var bind = curveBindObject as CurveBindObject<float>;
            bind?.Inspector(recdMode, seqence.Time);
        }

        private void InspectorColorGrad(bool recdMode)
        {
            var bind = curveBindObject as CurveBindObject<float, Vector4, Color>;
            bind?.Inspector(recdMode, seqence.Time);
        }

        private void CreateInstance(PostEnum? mode)
        {
            switch (mode)
            {
                case PostEnum.Bloom:
                    var b1 = new CurveBindObject<float, Color>();
                    b1.Add(new CurveBind<float>("bloom.intensity", x => (setting as Bloom).intensity.value = x));
                    b1.Add(new CurveBind<float>("bloom.threshold", x => (setting as Bloom).threshold.value = x));
                    b1.Add(new CurveBind<float>("bloom.softknee", x => (setting as Bloom).softKnee.value = x));
                    b1.Add(new CurveBind<float>("bloom.clamp", x => (setting as Bloom).clamp.value = x));
                    b1.Add(new CurveBind<Color>("bloom.color", x => (setting as Bloom).color.value = x));
                    curveBindObject = b1;
                    break;
                case PostEnum.Vignette:
                    var b2 = new CurveBindObject<float, Vector2, Color>();
                    b2.Add(new CurveBind<Color>("vign.color", x => (setting as Vignette).color.value = x));
                    b2.Add(new CurveBind<Vector2>("vign.center", x => (setting as Vignette).center.value = x));
                    b2.Add(new CurveBind<float>("vign.intensity", x => (setting as Vignette).intensity.value = x));
                    b2.Add(new CurveBind<float>("vign.roundness", x => (setting as Vignette).rounded.value = x > 0));
                    curveBindObject = b2;
                    break;
                case PostEnum.ColorGrading:
                    var b3 = new CurveBindObject<float, Vector4, Color>();
                    b3.Add(new CurveBind<Color>("grad.colorFilter",
                        x => (setting as ColorGrading).colorFilter.value = x));
                    b3.Add(new CurveBind<float>("grad.brightness",
                        x => (setting as ColorGrading).brightness.value = x));
                    b3.Add(new CurveBind<float>("grad.saturation",
                        x => (setting as ColorGrading).saturation.value = x));
                    b3.Add(new CurveBind<float>("grad.hueShift", x => (setting as ColorGrading).hueShift.value = x));
                    b3.Add(new CurveBind<float>("grad.tint", x => (setting as ColorGrading).tint.value = x));
                    b3.Add(new CurveBind<float>("grad.contrast", x => (setting as ColorGrading).contrast.value = x));
                    b3.Add(new CurveBind<float>("grad.temperature",
                        x => (setting as ColorGrading).temperature.value = x));
                    b3.Add(new CurveBind<Vector4>("grad.gain", x => (setting as ColorGrading).gain.value = x));
                    curveBindObject = b3;
                    break;
                case PostEnum.DepthOfField:
                    var b4 = new CurveBindObject<float>();
                    b4.Add(new CurveBind<float>("depth.aperture", x => (setting as DepthOfField).aperture.value = x));
                    b4.Add(new CurveBind<float>("depth.focalLength",
                        x => (setting as DepthOfField).focalLength.value = x));
                    b4.Add(new CurveBind<float>("depth.focusDistance",
                        x => (setting as DepthOfField).focusDistance.value = x));
                    curveBindObject = b4;
                    break;
                case PostEnum.MotionBlur:
                    var b5 = new CurveBindObject<float>();
                    b5.Add(new CurveBind<float>("blur.sampleCount",
                        x => (setting as MotionBlur).sampleCount.value = (int) x));
                    b5.Add(new CurveBind<float>("blur.shutterAngle",
                        x => (setting as MotionBlur).shutterAngle.value = x));
                    curveBindObject = b5;
                    break;
            }
        }


        public override void OnSave()
        {
            base.OnSave();
            Write2Buffer();
        }

        public void Write2Buffer()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryWriter writer = new BinaryWriter(stream);
                WriteCurves(writer);
                writer.Dispose();
                data.buffer = stream.GetBuffer();
            }
        }

        public void WriteCurves(BinaryWriter writer)
        {
            var info = curveBindObject.GetAnimInfo();
            writer.Write(info.Count);
            foreach (var it in info)
            {
                WriteCurve(writer, it);
            }
        }

        private void WriteCurve(BinaryWriter writer, KeyValuePair<string, ICurve> pair)
        {
            writer.Write(pair.Key);
            switch (pair.Key)
            {
                case "bloom.intensity":
                case "bloom.threshold":
                case "bloom.softknee":
                case "bloom.clamp":
                case "vign.intensity":
                case "vign.roundness":
                case "grad.brightness":
                case "grad.saturation":
                case "grad.hueShift":
                case "grad.tint":
                case "grad.contrast":
                case "depth.aperture":
                case "depth.focalLength":
                case "depth.focusDistance":
                case "blur.sampleCount":
                case "blur.shutterAngle":
                    {
                        var curve = pair.Value as XCurve<float>;
                        int len = curve.frames?.Length ?? 0;
                        writer.Write(len);
                        for (int i = 0; i < len; i++)
                        {
                            writer.Write(curve.frames[i].t);
                            writer.Write(curve.frames[i].v);
                        }
                    }
                    break;
                case "bloom.color":
                case "vign.color":
                case "grad.colorFilter":
                    {
                        var curve = pair.Value as XCurve<Color>;
                        int len = curve.frames?.Length ?? 0;
                        writer.Write(len);
                        for (int i = 0; i < len; i++)
                        {
                            writer.Write(curve.frames[i].t);
                            writer.Write(curve.frames[i].v);
                        }
                    }
                    break;
                case "vign.center":
                    {
                        var curve = pair.Value as XCurve<Vector2>;
                        int len = curve.frames?.Length ?? 0;
                        writer.Write(len);
                        for (int i = 0; i < len; i++)
                        {
                            writer.Write(curve.frames[i].t);
                            writer.Write(curve.frames[i].v);
                        }
                    }
                    break;
                case "grad.gain":
                    {
                        var curve = pair.Value as XCurve<Vector4>;
                        int len = curve.frames?.Length ?? 0;
                        writer.Write(len);
                        for (int i = 0; i < len; i++)
                        {
                            writer.Write(curve.frames[i].t);
                            writer.Write(curve.frames[i].v);
                        }
                    }
                    break;
            }
        }

#endif

        public void ReadFromBuffer()
        {
            if (data.buffer != null)
            {
                using (MemoryStream stream = new MemoryStream(data.buffer))
                {
                    BinaryReader reader = new BinaryReader(stream);
                    ReadCurves(reader);
                    reader.Dispose();
                }
            }
        }


        public void ReadCurves(BinaryReader reader)
        {
            int len = reader.ReadInt32();
            for (int i = 0; i < len; i++)
            {
                ReadCurve(reader);
            }
        }

        private void ReadCurve(BinaryReader reader)
        {
            string k = reader.ReadString();
            int len = reader.ReadInt32();
            switch (k)
            {
                case "bloom.intensity":
                case "bloom.threshold":
                case "bloom.softknee":
                case "bloom.clamp":
                case "vign.intensity":
                case "vign.roundness":
                case "grad.brightness":
                case "grad.saturation":
                case "grad.hueShift":
                case "grad.tint":
                case "grad.contrast":
                case "depth.aperture":
                case "depth.focalLength":
                case "depth.focusDistance":
                case "blur.sampleCount":
                case "blur.shutterAngle":
                    for (int i = 0; i < len; i++)
                    {
                        float t = reader.ReadSingle();
                        float v = reader.ReadSingle();
                        curveBindObject.AddKey(t, k, v);
                    }
                    break;
                case "bloom.color":
                case "vign.color":
                case "grad.colorFilter":
                    for (int i = 0; i < len; i++)
                    {
                        float t = reader.ReadSingle();
                        Color c = reader.ReadColor();
                        curveBindObject.AddKey(t, k, c);
                    }
                    break;
                case "vign.center":
                    for (int i = 0; i < len; i++)
                    {
                        float t = reader.ReadSingle();
                        Vector2 v = reader.ReadV2();
                        curveBindObject.AddKey(t, k, v);
                    }
                    break;
                case "grad.gain":
                    for (int i = 0; i < len; i++)
                    {
                        float t = reader.ReadSingle();
                        Vector4 v = reader.ReadV4();
                        curveBindObject.AddKey(t, k, v);
                    }
                    break;
            }
        }
    }
}
