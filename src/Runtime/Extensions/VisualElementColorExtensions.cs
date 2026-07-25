using UnityEngine.UIElements;
using UnityEngine;
using PrimeTween;

namespace Nk7.UITK.Extensions
{
    public static class VisualElementColorExtensions
    {
        public static Tween Color(this VisualElement ve, Color to, float duration,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
        {
            if (ve == null)
            {
                return default;
            }

            return Tween.VisualElementColor(ve, ve.resolvedStyle.color, to, duration, ease,
                cycles, cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween BackgroundColor(this VisualElement ve, Color to, float duration,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
        {
            if (ve == null)
            {
                return default;
            }

            return Tween.VisualElementBackgroundColor(ve, ve.resolvedStyle.backgroundColor, to, duration, ease,
                cycles, cycleMode, startDelay, endDelay, useUnscaledTime);
        }
    }
}
