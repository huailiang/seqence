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

#if UNITY_EDITOR

        public PostProcessEffectSettings setting;

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
            Bloom bloom = setting as Bloom;
            EditorGUILayout.BeginVertical(EditorStyles.textField);
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
            EditorGUILayout.BeginVertical(EditorStyles.textField);
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
