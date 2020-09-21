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
        private PostProcessEffectSettings settings;

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

        private CurveBindObject curveBindObject;

        protected override void OnUpdate(float tick, bool mix)
        {
            base.OnUpdate(tick, mix);
            curveBindObject?.Evaluate(tick);
        }

#if UNITY_EDITOR

        public void OnInspector(Action cb)
        {
            EditorGUI.BeginChangeCheck();
            if (data != null)
            {
                data.mode = (PostEnum) EditorGUILayout.EnumPopup("Effect", data.mode);
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
            bool recdMode = track.record;
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
            bloomBinding?.Inspector(recdMode);
        }

        private void InspectorVignette(bool recdMode)
        {
            CurveBindObject<float, Vector2, Color> bind = curveBindObject as CurveBindObject<float, Vector2, Color>;
            bind?.Inspector(recdMode);
        }

        private void InspectorMotionBlur(bool recdMode)
        {
            CurveBindObject<float> bind = curveBindObject as CurveBindObject<float>;
            bind?.Inspector(recdMode);
        }

        private void InspectorDepthField(bool recdMode)
        {
            CurveBindObject<float> bind = curveBindObject as CurveBindObject<float>;
            bind?.Inspector(recdMode);
        }

        private void InspectorColorGrad(bool recdMode)
        {
            CurveBindObject<float, Vector4, Color> bind = curveBindObject as CurveBindObject<float, Vector4, Color>;
            bind?.Inspector(recdMode);
        }

        private void CreateInstance(PostEnum? mode)
        {
            switch (mode)
            {
                case PostEnum.Bloom:
                    var bloom = settings as Bloom;
                    var b1 = new CurveBindObject<float, Color>();
                    b1.Add(new CurveBind<float>("intensity", x => bloom.intensity.value = x));
                    b1.Add(new CurveBind<float>("threshold", x => bloom.threshold.value = x));
                    b1.Add(new CurveBind<float>("softknee", x => bloom.softKnee.value = x));
                    b1.Add(new CurveBind<float>("clamp", x => bloom.clamp.value = x));
                    b1.Add(new CurveBind<Color>("color", x => bloom.color.value = x));
                    curveBindObject = b1;
                    break;
                case PostEnum.Vignette:
                    var vignette = settings as Vignette;
                    var b2 = new CurveBindObject<float, Vector2, Color>();
                    b2.Add(new CurveBind<Color>("color", x => vignette.color.value = x));
                    b2.Add(new CurveBind<Vector2>("center", x => vignette.center.value = x));
                    b2.Add(new CurveBind<float>("intensity", x => vignette.intensity.value = x));
                    b2.Add(new CurveBind<float>("roundness", x => vignette.rounded.value = x > 0));
                    curveBindObject = b2;
                    break;
                case PostEnum.ColorGrading:
                    var grad = settings as ColorGrading;
                    var b3 = new CurveBindObject<float, Vector4, Color>();
                    b3.Add(new CurveBind<Color>("colorFilter", x => grad.colorFilter.value = x));
                    b3.Add(new CurveBind<float>("brightness", x => grad.brightness.value = x));
                    b3.Add(new CurveBind<float>("saturation", x => grad.saturation.value = x));
                    b3.Add(new CurveBind<float>("hueShift", x => grad.hueShift.value = x));
                    b3.Add(new CurveBind<float>("tint", x => grad.tint.value = x));
                    b3.Add(new CurveBind<float>("contrast", x => grad.contrast.value = x));
                    b3.Add(new CurveBind<float>("temperature", x => grad.temperature.value = x));
                    b3.Add(new CurveBind<Vector4>("gain", x => grad.gain.value = x));
                    curveBindObject = b3;
                    break;
                case PostEnum.DepthOfField:
                    var depth = settings as DepthOfField;
                    var b4 = new CurveBindObject<float>();
                    b4.Add(new CurveBind<float>("aperture", x => depth.aperture.value = x));
                    b4.Add(new CurveBind<float>("focalLength", x => depth.focalLength.value = x));
                    b4.Add(new CurveBind<float>("focusDistance", x => depth.focusDistance.value = x));
                    curveBindObject = b4;
                    break;
                case PostEnum.MotionBlur:
                    var blur = settings as MotionBlur;
                    var b5 = new CurveBindObject<float>();
                    b5.Add(new CurveBind<float>("sampleCount", x => blur.sampleCount.value = (int) x));
                    b5.Add(new CurveBind<float>("shutterAngle", x => blur.shutterAngle.value = x));
                    curveBindObject = b5;
                    break;
            }
        }

#endif
    }
}
