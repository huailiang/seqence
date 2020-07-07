using Unity.Collections;
using UnityEngine;

#if UNITY_2019_3_OR_NEWER
using UnityEngine.Animations;
#else
using UnityEngine.Experimental.Animations;
#endif

namespace UnityEngine.Timeline
{
    public struct MixerJob : IAnimationJob
    {
        public NativeArray<TransformStreamHandle> handles;
        public NativeArray<float> boneWeights;
        public float weight;

        public void ProcessRootMotion(AnimationStream stream)
        {
            var streamA = stream.GetInputStream(0);
            var streamB = stream.GetInputStream(1);
            if (streamA.isValid && streamB.isValid)
            {
                var velocity = Vector3.Lerp(streamA.velocity, streamB.velocity, weight);
                var angularVelocity = Vector3.Lerp(streamA.angularVelocity, streamB.angularVelocity, weight);
                stream.velocity = velocity;
                stream.angularVelocity = angularVelocity;
            }
            else if (streamA.isValid)
            {
                ProcessMotion(stream, streamA);

            }
            else if (streamB.isValid)
            {
                ProcessMotion(stream, streamB);
            }
        }

        private void ProcessMotion(AnimationStream stream, AnimationStream input)
        {
            var velocity = input.velocity;
            var angularVelocity = input.angularVelocity;
            stream.velocity = velocity;
            stream.angularVelocity = angularVelocity;
        }

        public void ProcessAnimation(AnimationStream stream)
        {
            var streamA = stream.GetInputStream(0);
            var streamB = stream.GetInputStream(1);
            var numHandles = handles.Length;
            if (streamA.isValid && streamB.isValid)
            {
                for (var i = 0; i < numHandles; ++i)
                {
                    var handle = handles[i];

                    var posA = handle.GetLocalPosition(streamA);
                    var posB = handle.GetLocalPosition(streamB);
                    handle.SetLocalPosition(stream, Vector3.Lerp(posA, posB, weight * boneWeights[i]));

                    var rotA = handle.GetLocalRotation(streamA);
                    var rotB = handle.GetLocalRotation(streamB);
                    handle.SetLocalRotation(stream, Quaternion.Slerp(rotA, rotB, weight * boneWeights[i]));
                }
            }
            else if (streamA.isValid)
            {
                ProcessAnimation(stream, streamA);
            }
            else if (streamB.isValid)
            {
                ProcessAnimation(stream, streamB);
            }
        }

        private void ProcessAnimation(AnimationStream stream, AnimationStream input)
        {
            var numHandles = handles.Length;
            for (var i = 0; i < numHandles; ++i)
            {
                var handle = handles[i];
                var posA = handle.GetLocalPosition(input);
                handle.SetLocalPosition(stream, posA);
                var rotA = handle.GetLocalRotation(input);
                handle.SetLocalRotation(stream, rotA);
            }
        }

    }

}
