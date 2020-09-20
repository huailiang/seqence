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
            curveBindObject?.Evaluate(tick);
        }

#if UNITY_EDITOR

        // public PostProcessEffectSettings setting;

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
                    cb();
                }
            }
            switch (data.mode)
            {
                case PostEnum.Bloom:
                    InspectorBloom();
                    break;
                case PostEnum.Vignette:
                    InspectorVignette();
                    break;
                case PostEnum.MotionBlur:
                    InspectorMotionBlur();
                    break;
                case PostEnum.DepthOfField:
                    InspectorDepthField();
                    break;
                case PostEnum.ColorGrading:
                    InspectorColorGrad();
                    break;
            }
        }

        private void InspectorBloom()
        {
            CurveBindObject<float, Color> bloomBinding = curveBindObject as CurveBindObject<float, Color>;
            bloomBinding?.Inspector();
        }

        private void InspectorVignette()
        {
            CurveBindObject<float, Vector2, Color> bind = curveBindObject as CurveBindObject<float, Vector2, Color>;
            bind?.Inspector();
        }

        private void InspectorMotionBlur()
        {
            CurveBindObject<float> bind = curveBindObject as CurveBindObject<float>;
            bind?.Inspector();
        }

        private void InspectorDepthField()
        {
            CurveBindObject<float> bind = curveBindObject as CurveBindObject<float>;
            bind?.Inspector();
        }

        private void InspectorColorGrad()
        {
            CurveBindObject<float, Vector4, Color> bind = curveBindObject as CurveBindObject<float, Vector4, Color>;
            bind?.Inspector();
        }

        private void CreateInstance(PostEnum? mode)
        {
            switch (mode)
            {
                case PostEnum.Bloom:
                    var b1 = new CurveBindObject<float, Color>();
                    b1.Add(new CurveBind<float>("intensity"));
                    b1.Add(new CurveBind<float>("threshold"));
                    b1.Add(new CurveBind<float>("softknee"));
                    b1.Add(new CurveBind<float>("clamp"));
                    b1.Add(new CurveBind<Color>("color"));
                    curveBindObject = b1;
                    break;
                case PostEnum.Vignette:
                    var b2 = new CurveBindObject<float, Vector2, Color>();
                    b2.Add(new CurveBind<Color>("color"));
                    b2.Add(new CurveBind<Vector2>("center"));
                    b2.Add(new CurveBind<float>("intensity"));
                    b2.Add(new CurveBind<float>("roundness"));
                    curveBindObject = b2;
                    break;
                case PostEnum.ColorGrading:
                    var b3 = new CurveBindObject<float, Vector4, Color>();
                    b3.Add(new CurveBind<Color>("colorFilter"));
                    b3.Add(new CurveBind<float>("brightness"));
                    b3.Add(new CurveBind<float>("saturation"));
                    b3.Add(new CurveBind<float>("hueShift"));
                    b3.Add(new CurveBind<float>("tint"));
                    b3.Add(new CurveBind<float>("contrast"));
                    b3.Add(new CurveBind<float>("temperature"));
                    b3.Add(new CurveBind<Vector4>("gain"));
                    curveBindObject = b3;
                    break;
                case PostEnum.DepthOfField:
                    var b4 = new CurveBindObject<float>();
                    b4.Add(new CurveBind<float>("aperture"));
                    b4.Add(new CurveBind<float>("focalLength"));
                    b4.Add(new CurveBind<float>("focusDistance"));
                    curveBindObject = b4;
                    break;
                case PostEnum.MotionBlur:
                    var b5 = new CurveBindObject<float>();
                    b5.Add(new CurveBind<float>("sampleCount"));
                    b5.Add(new CurveBind<float>("shutterAngle"));
                    curveBindObject = b5;
                    break;
            }
        }

#endif
    }
}
