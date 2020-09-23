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
        public PostProcessEffectSettings settings;

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
                data.mode = (PostEnum)EditorGUILayout.EnumPopup("Effect", data.mode);
            }
            if (EditorGUI.EndChangeCheck())
            {
                if (data.mode > 0)
                {
                    CreateInstance(data.mode);
                    int idx = (int)data.mode;
                    var profile = seqence.postProfile.settings;
                    if (idx < profile.Count)
                    {
                        settings = profile[idx];
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
            CurveBindObject<float, Color> bloomBinding = curveBindObject as CurveBindObject<float, Color>;
            bloomBinding?.Inspector(recdMode, seqence.Time);
        }

        private void InspectorVignette(bool recdMode)
        {
            CurveBindObject<float, Vector2, Color> bind = curveBindObject as CurveBindObject<float, Vector2, Color>;
            bind?.Inspector(recdMode, seqence.Time);
        }

        private void InspectorMotionBlur(bool recdMode)
        {
            CurveBindObject<float> bind = curveBindObject as CurveBindObject<float>;
            bind?.Inspector(recdMode, seqence.Time);
        }

        private void InspectorDepthField(bool recdMode)
        {
            CurveBindObject<float> bind = curveBindObject as CurveBindObject<float>;
            bind?.Inspector(recdMode, seqence.Time);
        }

        private void InspectorColorGrad(bool recdMode)
        {
            CurveBindObject<float, Vector4, Color> bind = curveBindObject as CurveBindObject<float, Vector4, Color>;
            bind?.Inspector(recdMode, seqence.Time);
        }

        private void CreateInstance(PostEnum? mode)
        {
            switch (mode)
            {
                case PostEnum.Bloom:
                    var b1 = new CurveBindObject<float, Color>();
                    b1.Add(new CurveBind<float>("intensity", x => (settings as Bloom).intensity.value = x));
                    b1.Add(new CurveBind<float>("threshold", x => (settings as Bloom).threshold.value = x));
                    b1.Add(new CurveBind<float>("softknee", x => (settings as Bloom).softKnee.value = x));
                    b1.Add(new CurveBind<float>("clamp", x => (settings as Bloom).clamp.value = x));
                    b1.Add(new CurveBind<Color>("color", x => (settings as Bloom).color.value = x));
                    curveBindObject = b1;
                    break;
                case PostEnum.Vignette:
                    var b2 = new CurveBindObject<float, Vector2, Color>();
                    b2.Add(new CurveBind<Color>("color", x => (settings as Vignette).color.value = x));
                    b2.Add(new CurveBind<Vector2>("center", x => (settings as Vignette).center.value = x));
                    b2.Add(new CurveBind<float>("intensity", x => (settings as Vignette).intensity.value = x));
                    b2.Add(new CurveBind<float>("roundness", x => (settings as Vignette).rounded.value = x > 0));
                    curveBindObject = b2;
                    break;
                case PostEnum.ColorGrading:
                    var b3 = new CurveBindObject<float, Vector4, Color>();
                    b3.Add(new CurveBind<Color>("colorFilter", x => (settings as ColorGrading).colorFilter.value = x));
                    b3.Add(new CurveBind<float>("brightness", x => (settings as ColorGrading).brightness.value = x));
                    b3.Add(new CurveBind<float>("saturation", x => (settings as ColorGrading).saturation.value = x));
                    b3.Add(new CurveBind<float>("hueShift", x => (settings as ColorGrading).hueShift.value = x));
                    b3.Add(new CurveBind<float>("tint", x => (settings as ColorGrading).tint.value = x));
                    b3.Add(new CurveBind<float>("contrast", x => (settings as ColorGrading).contrast.value = x));
                    b3.Add(new CurveBind<float>("temperature", x => (settings as ColorGrading).temperature.value = x));
                    b3.Add(new CurveBind<Vector4>("gain", x => (settings as ColorGrading).gain.value = x));
                    curveBindObject = b3;
                    break;
                case PostEnum.DepthOfField:
                    var b4 = new CurveBindObject<float>();
                    b4.Add(new CurveBind<float>("aperture", x => (settings as DepthOfField).aperture.value = x));
                    b4.Add(new CurveBind<float>("focalLength", x => (settings as DepthOfField).focalLength.value = x));
                    b4.Add(new CurveBind<float>("focusDistance", x => (settings as DepthOfField).focusDistance.value = x));
                    curveBindObject = b4;
                    break;
                case PostEnum.MotionBlur:
                    var b5 = new CurveBindObject<float>();
                    b5.Add(new CurveBind<float>("sampleCount", x => (settings as MotionBlur).sampleCount.value = (int)x));
                    b5.Add(new CurveBind<float>("shutterAngle", x => (settings as MotionBlur).shutterAngle.value = x));
                    curveBindObject = b5;
                    break;
            }
        }

#endif
    }
}
