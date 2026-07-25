using UnityEngine.UIElements;
using PrimeTween;

namespace Nk7.UITK.Extensions
{
    public static class VisualElementSizeExtensions
    {
        public static Tween Width(this VisualElement ve, float from, float to, float duration,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
        {
            if (ve == null)
            {
                return default;
            }

            return Tween.Custom(ve, from, to, duration,
                static (el, v) => el.style.width = v,
                ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween Width(this VisualElement ve, float to, float duration,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
        {
            if (ve == null)
            {
                return default;
            }

            return ve.Width(ve.resolvedStyle.width, to, duration, ease,
                cycles, cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween Height(this VisualElement ve, float from, float to, float duration,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
        {
            if (ve == null)
            {
                return default;
            }

            return Tween.Custom(ve, from, to, duration,
                static (el, v) => el.style.height = v,
                ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween Height(this VisualElement ve, float to, float duration,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
        {
            if (ve == null)
            {
                return default;
            }

            return ve.Height(ve.resolvedStyle.height, to, duration, ease,
                cycles, cycleMode, startDelay, endDelay, useUnscaledTime);
        }
    }
}
