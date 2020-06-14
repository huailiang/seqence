namespace UnityEditor.Timeline
{
    class WindowConstants
    {
        public const float rowGap = 4;
        public const float timeAreaYPosition = 18.0f;
        public const float timeAreaHeight = 20.0f;

        public const float sliderWidth = 200.0f;
        public const float rightAreaMargn = 202.0f;

        public const float markerRowHeight = 18.0f;
        public const float markerRowYPosition = timeAreaYPosition + timeAreaHeight + rowGap;
        public const float trackRowYPosition = markerRowYPosition + markerRowHeight + rowGap;

        public const float minTimeCodeWidth = 28; // Enough space to display up to 9999 without clipping

        public const float shadowUnderTimelineHeight = 15;
        public const float RawHeight = 36;
    }
}
