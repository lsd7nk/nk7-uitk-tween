using UnityEngine.UIElements;
using PrimeTween;

namespace Nk7.UITK.Extensions
{
    public static class VisualElementOpacityExtensions
    {
        public static Tween Fade(this VisualElement ve, float to, float duration,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
        {
            if (ve == null)
            {
                return default;
            }

            return Tween.Alpha(ve, ve.resolvedStyle.opacity, to, duration, ease,
                cycles, cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween FadeIn(this VisualElement ve, float duration,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
        {
            if (ve == null)
            {
                return default;
            }

            return ve.Fade(1f, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween FadeOut(this VisualElement ve, float duration,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
        {
            if (ve == null)
            {
                return default;
            }

            return ve.Fade(0f, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime);
        }
    }
}