using UnityEngine.UIElements;
using UnityEngine;
using PrimeTween;

namespace Nk7.UITK.Extensions
{
    public static class VisualElementTranslateExtensions
    {
        public static Tween Move(this VisualElement ve, Vector2 to, float duration,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
        {
            if (ve == null)
            {
                return default;
            }

            var from = ve.resolvedStyle.translate;
            return Tween.Position(ve, new Vector2(from.x, from.y), to, duration, ease,
                cycles, cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween MoveX(this VisualElement ve, float to, float duration,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
        {
            if (ve == null)
            {
                return default;
            }

            var from = ve.resolvedStyle.translate;
            return Tween.Position(ve, new Vector2(from.x, from.y), new Vector2(to, from.y), duration, ease,
                cycles, cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween MoveY(this VisualElement ve, float to, float duration,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
        {
            if (ve == null)
            {
                return default;
            }

            var from = ve.resolvedStyle.translate;
            return Tween.Position(ve, new Vector2(from.x, from.y), new Vector2(from.x, to), duration, ease,
                cycles, cycleMode, startDelay, endDelay, useUnscaledTime);
        }
    }
}
