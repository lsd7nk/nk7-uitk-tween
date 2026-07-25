using UnityEngine.UIElements;
using PrimeTween;

namespace Nk7.UITK.Extensions
{
    public static class VisualElementRotateExtensions
    {
        public static Tween Rotate(this VisualElement ve, float fromDegrees, float toDegrees, float duration,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
        {
            if (ve == null)
            {
                return default;
            }

            return Tween.Custom(ve, fromDegrees, toDegrees, duration,
                static (el, v) => el.style.rotate = new Rotate(new Angle(v, AngleUnit.Degree)),
                ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween Rotate(this VisualElement ve, float toDegrees, float duration,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
        {
            if (ve == null)
            {
                return default;
            }

            float fromDegrees = ve.resolvedStyle.rotate.angle.ToDegrees();
            return ve.Rotate(fromDegrees, toDegrees, duration, ease,
                cycles, cycleMode, startDelay, endDelay, useUnscaledTime);
        }
    }
}
