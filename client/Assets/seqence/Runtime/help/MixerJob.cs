using Unity.Collections;
#if UNITY_2019_3_OR_NEWER
using UnityEngine.Animations;
#else
using UnityEngine.Experimental.Animations;
#endif

namespace UnityEngine.Seqence
{
    public struct MixerJob : IAnimationJob
    {
        public NativeArray<TransformStreamHandle> handles;
        public float weight;
        public int clipA, clipB;

        public void ProcessRootMotion(AnimationStream stream)
        {
            if (clipA >= 0 && clipB >= 0)
            {
                var streamA = stream.GetInputStream(clipA);
                var streamB = stream.GetInputStream(clipB);
                if (streamA.isValid && streamB.isValid)
                {
                    var velocity = Vector3.Lerp(streamA.velocity, streamB.velocity, weight);
                    var angularVelocity = Vector3.Lerp(streamA.angularVelocity, streamB.angularVelocity, weight);
                    stream.velocity = velocity;
                    stream.angularVelocity = angularVelocity;
                }
            }
            else if (clipA >= 0)
            {
                var streamA = stream.GetInputStream(clipA);
                if (streamA.isValid)
                {
                    ProcessMotion(stream, streamA);
                }
            }
            else if (clipB > 0)
            {
                var streamB = stream.GetInputStream(clipB);
                if (streamB.isValid)
                {
                    ProcessMotion(stream, streamB);
                }
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
            if (clipA >= 0 && clipB >= 0)
            {
                var streamA = stream.GetInputStream(clipA);
                var streamB = stream.GetInputStream(clipB);

                var numHandles = handles.Length;
                if (streamA.isValid && streamB.isValid)
                {
                    for (var i = 0; i < numHandles; ++i)
                    {
                        var handle = handles[i];

                        var posA = handle.GetLocalPosition(streamA);
                        var posB = handle.GetLocalPosition(streamB);
                        handle.SetLocalPosition(stream, Vector3.Lerp(posA, posB, weight));

                        var rotA = handle.GetLocalRotation(streamA);
                        var rotB = handle.GetLocalRotation(streamB);
                        handle.SetLocalRotation(stream, Quaternion.Slerp(rotA, rotB, weight));
                    }
                }
            }
            else if (clipA >= 0)
            {
                var streamA = stream.GetInputStream(clipA);
                if (streamA.isValid)
                {
                    ProcessAnimation(stream, streamA);
                }
            }
            else if (clipB > 0)
            {
                var streamB = stream.GetInputStream(clipB);
                if (streamB.isValid)
                {
                    ProcessAnimation(stream, streamB);
                }
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

        public void Dispose()
        {
            clipA = -1;
            clipB = -1;
            handles.Dispose();
        }
    }
}
