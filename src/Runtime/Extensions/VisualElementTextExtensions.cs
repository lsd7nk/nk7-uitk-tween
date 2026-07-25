using UnityEngine.UIElements;
using UnityEngine;
using PrimeTween;

namespace Nk7.UITK.Extensions
{
    public static class VisualElementTextExtensions
    {
        public static Tween TextOutlineColor(this VisualElement ve, Color from, Color to, float duration,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
        {
            if (ve == null)
            {
                return default;
            }

            return Tween.Custom(ve, from, to, duration,
                static (el, v) => el.style.unityTextOutlineColor = v,
                ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween TextOutlineColor(this VisualElement ve, Color to, float duration,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
        {
            if (ve == null)
            {
                return default;
            }

            return ve.TextOutlineColor(ve.resolvedStyle.unityTextOutlineColor, to, duration, ease,
                cycles, cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween TextOutlineWidth(this VisualElement ve, float from, float to, float duration,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
        {
            if (ve == null)
            {
                return default;
            }

            return Tween.Custom(ve, from, to, duration,
                static (el, v) => el.style.unityTextOutlineWidth = v,
                ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween TextOutlineWidth(this VisualElement ve, float to, float duration,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
        {
            if (ve == null)
            {
                return default;
            }

            return ve.TextOutlineWidth(ve.resolvedStyle.unityTextOutlineWidth, to, duration, ease,
                cycles, cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween FontSize(this VisualElement ve, float from, float to, float duration,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
        {
            if (ve == null)
            {
                return default;
            }

            return Tween.Custom(ve, from, to, duration,
                static (el, v) => el.style.fontSize = v,
                ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween FontSize(this VisualElement ve, float to, float duration,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false)
        {
            if (ve == null)
            {
                return default;
            }

            return ve.FontSize(ve.resolvedStyle.fontSize, to, duration, ease,
                cycles, cycleMode, startDelay, endDelay, useUnscaledTime);
        }
    }
}