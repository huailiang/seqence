using UnityEngine.Seqence.Data;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.Rendering.PostProcessing;

#endif

namespace UnityEngine.Seqence
{
    public class XPostprocessClip : XClip<XPostprocessTrack, XPostprocessClip>, ISharedObject<XPostprocessClip>
    {
        private PostprocessData Data;

        public override string Display
        {
            get
            {
                if (data != null)
                {
                    Data = data as PostprocessData;
                    return Data.ToString();
                }
                return "post process";
            }
        }

        public override void OnDestroy()
        {
            SharedPool<XPostprocessClip>.Return(this);
            base.OnDestroy();
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            Data = data as PostprocessData;
        }

#if UNITY_EDITOR

        public PostProcessEffectSettings setting;

        public void OnInspector()
        {
            Data = data as PostprocessData;
            EditorGUI.BeginChangeCheck();
            if (Data != null)
            {
                Data.mode = (PostEnum) EditorGUILayout.EnumPopup("Effect", Data.mode);
            }
            if (EditorGUI.EndChangeCheck())
            {
                if (Data.mode > 0) CreateInstance(Data.mode);
            }
            switch (Data.mode)
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
            Bloom bloom = setting as Bloom;
            EditorGUILayout.BeginVertical(EditorStyles.label);

            bloom.intensity.value = EditorGUILayout.FloatField("intensity", bloom.intensity.value);
            bloom.threshold.value = EditorGUILayout.FloatField("threshold", bloom.threshold.value);
            bloom.softKnee.value = EditorGUILayout.FloatField("softknee", bloom.softKnee.value);
            bloom.clamp.value = EditorGUILayout.FloatField("clamp", bloom.clamp.value);
            bloom.color.value = EditorGUILayout.ColorField("color", bloom.color.value);
            EditorGUILayout.EndVertical();
        }

        private void InspectorVignette()
        {
            Vignette vignette = setting as Vignette;
            EditorGUILayout.BeginVertical(EditorStyles.label);
            vignette.color.value = EditorGUILayout.ColorField("color", vignette.color.value);
            vignette.center.value = EditorGUILayout.Vector2Field("center", vignette.center.value);
            vignette.intensity.value = EditorGUILayout.FloatField("intensity", vignette.intensity.value);
            vignette.roundness.value = EditorGUILayout.FloatField("roundness", vignette.roundness.value);
            vignette.rounded.value = EditorGUILayout.Toggle("rounded", vignette.rounded.value);
            EditorGUILayout.EndVertical();
        }

        private void InspectorMotionBlur()
        {
            MotionBlur blur = setting as MotionBlur;
            EditorGUILayout.BeginVertical(EditorStyles.label);
            blur.sampleCount.value = EditorGUILayout.IntField("sampleCount", blur.sampleCount.value);
            blur.shutterAngle.value = EditorGUILayout.FloatField("shutterAngle", blur.shutterAngle.value);
            EditorGUILayout.EndVertical();
        }

        private void InspectorDepthField()
        {
            DepthOfField depth = setting as DepthOfField;
            EditorGUILayout.BeginVertical(EditorStyles.label);
            depth.aperture.value = EditorGUILayout.FloatField("aperture", depth.aperture.value);
            depth.focalLength.value = EditorGUILayout.FloatField("focalLength", depth.focalLength.value);
            depth.focusDistance.value = EditorGUILayout.FloatField("focusDistance", depth.focusDistance.value);
            EditorGUILayout.EndVertical();
        }

        private void InspectorColorGrad()
        {
            ColorGrading grad = setting as ColorGrading;
            EditorGUILayout.BeginVertical(EditorStyles.label);
            grad.brightness.value = EditorGUILayout.FloatField("brightness", grad.brightness.value);
            grad.contrast.value = EditorGUILayout.FloatField("contrast", grad.contrast.value);
            grad.gain.value = EditorGUILayout.Vector4Field("gain", grad.gain.value);
            grad.temperature.value = EditorGUILayout.FloatField("temperature", grad.temperature.value);
            grad.tint.value = EditorGUILayout.FloatField("tint", grad.tint.value);
            grad.hueShift.value = EditorGUILayout.FloatField("hueShift", grad.hueShift.value);
            grad.saturation.value = EditorGUILayout.FloatField("saturation", grad.saturation.value);
            grad.colorFilter.value = EditorGUILayout.ColorField("colorFilter", grad.colorFilter.value);
            EditorGUILayout.EndVertical();
        }

        private void CreateInstance(PostEnum? mode)
        {
            switch (mode)
            {
                case PostEnum.Bloom:
                    setting = ScriptableObject.CreateInstance<Bloom>();
                    break;
                case PostEnum.Vignette:
                    setting = ScriptableObject.CreateInstance<Vignette>();
                    break;
                case PostEnum.ColorGrading:
                    setting = ScriptableObject.CreateInstance<ColorGrading>();
                    break;
                case PostEnum.DepthOfField:
                    setting = ScriptableObject.CreateInstance<DepthOfField>();
                    break;
                case PostEnum.MotionBlur:
                    setting = ScriptableObject.CreateInstance<MotionBlur>();
                    break;
            }
        }

#endif
    }
}
