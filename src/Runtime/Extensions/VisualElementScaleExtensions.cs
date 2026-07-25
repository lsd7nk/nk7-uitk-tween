using UnityEngine.UIElements;
using UnityEngine;
using PrimeTween;

namespace Nk7.UITK.Extensions
{
    public static class VisualElementScaleExtensions
    {
        public static Tween Scale(this VisualElement ve, Vector2 to, float duration,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
        {
            if (ve == null)
            {
                return default;
            }

            var from = ve.resolvedStyle.scale.value;
            return Tween.Scale(ve, new Vector2(from.x, from.y), to, duration, ease,
                cycles, cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween Scale(this VisualElement ve, float to, float duration,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
        {
            if (ve == null)
            {
                return default;
            }

            return ve.Scale(new Vector2(to, to), duration, ease,
                cycles, cycleMode, startDelay, endDelay, useUnscaledTime);
        }
    }
}
