# nk7-uitk-tween

Lightweight PrimeTween extensions for Unity UI Toolkit: allocation-free tweens called directly on `VisualElement`, with start values read from `resolvedStyle` so call sites only declare where the animation goes.

## Features

- Extension methods on `VisualElement` for opacity, color, transform, size, and text style animations.
- Relative overloads capture the start value from `resolvedStyle`, so a single argument describes the whole tween.
- Degree-based `Rotate` on top of PrimeTween's quaternion API — no `Quaternion.Euler` at call sites.
- Covers properties PrimeTween has no built-in UI Toolkit tween for: `Width`, `Height`, `FontSize`, `TextOutlineColor`, `TextOutlineWidth`.
- Every method returns a PrimeTween `Tween`, so results compose into `Sequence`, `await`, `OnComplete`, and `Stop`.
- The full PrimeTween settings tail (`Ease`, `cycles`, `CycleMode`, `startDelay`, `endDelay`, `useUnscaledTime`) is available on every overload.
- Null-safe: a `null` element returns `default(Tween)` instead of throwing.
- No allocations — property writes go through `static` lambdas and PrimeTween's built-in `VisualElement` tween types.

## Table of Contents

- [Installation](#installation)
  - [Unity Package Manager](#unity-package-manager)
  - [Manual Installation](#manual-installation)
- [Quick Start](#quick-start)
  - [1. Import the namespace](#1-import-the-namespace)
  - [2. Animate an element](#2-animate-an-element)
  - [3. Compose and await](#3-compose-and-await)
  - [4. Stop tweens](#4-stop-tweens)
- [Start Values](#start-values)
- [Transform vs Layout](#transform-vs-layout)
- [Extension Reference](#extension-reference)
- [Runtime API](#runtime-api)
- [Requirements](#requirements)

## Installation

### Unity Package Manager

1. Open Unity Package Manager (`Window → Package Manager`).
2. Click `+ → Add package from git URL…`.
3. Enter `https://github.com/lsd7nk/nk7-uitk-tween.git?path=src`.

PrimeTween is published on npm, so the dependency resolves only if the consuming project declares its scoped registry in `Packages/manifest.json`:

```json
"scopedRegistries": [
  {
    "name": "npm",
    "url": "https://registry.npmjs.org/",
    "scopes": [ "com.kyrylokuzyk" ]
  }
]
```

Unity does not auto-update Git-based packages; update the hash manually when needed or use [UPM Git Extension](https://github.com/mob-sakai/UpmGitExtension).

### Manual Installation

Copy the `src` folder into your project and add `Nk7.UITK.Tween.asmdef` to the assembly.

## Quick Start

### 1. Import the namespace

```csharp
using Nk7.UITK.Extensions;
using PrimeTween;
```

The assembly is `autoReferenced`, so no assembly reference is needed in the consuming asmdef.

### 2. Animate an element

Pass the target value and duration; the start value comes from the element's current resolved style.

```csharp
var panel = root.Q<VisualElement>("panel");

panel.FadeIn(0.25f);
panel.Scale(1.1f, 0.2f, Ease.OutBack);
panel.MoveY(0f, 0.3f, Ease.OutCubic);
panel.BackgroundColor(Color.red, 0.15f, cycles: 2, cycleMode: CycleMode.Yoyo);
```

Use the explicit `from`/`to` overloads when the start value must not depend on the current style.

```csharp
panel.Rotate(-15f, 0f, 0.3f, Ease.OutBack);
panel.Width(0f, 320f, 0.4f);
```

### 3. Compose and await

Returned tweens are regular PrimeTween handles.

```csharp
Sequence.Create()
    .Chain(panel.FadeIn(0.2f))
    .Group(panel.Scale(1f, 0.2f, Ease.OutBack))
    .ChainCallback(() => Debug.Log("shown"));

await panel.FadeOut(0.2f);
panel.style.display = DisplayStyle.None;
```

### 4. Stop tweens

Stop everything running on an element through PrimeTween's target API before reusing or releasing it.

```csharp
Tween.StopAll(panel);     // or Tween.CompleteAll(panel)
```

## Start Values

- Relative overloads (`Fade`, `Move`, `Scale`, `Rotate(to)`, `Width(to)`, `FontSize(to)`, …) read `resolvedStyle` at the moment of the call.
- `resolvedStyle` is only meaningful after the first layout pass. Calling these before the element is laid out starts the tween from `0` or `NaN`; trigger them after `GeometryChangedEvent` (or once the panel is attached) — or use the explicit `from`/`to` overloads.
- USS transitions on the same properties fight the tween for the value. Keep tweened properties out of `transition-property`.

## Transform vs Layout

- `Move`, `MoveX`, `MoveY`, `Scale`, and `Rotate` write to the element transform (`translate` / `scale` / `rotate`) and do not dirty layout — the cheapest way to animate.
- `Width` and `Height` write layout styles, so every frame re-runs layout for the subtree. Prefer `Scale` for show/hide effects and keep size tweens for elements that genuinely need to reflow.
- `FontSize` and `TextOutlineWidth` re-generate text geometry each frame; use short durations on large text.

## Extension Reference

| Extension | Writes | Start value (relative overload) |
| --- | --- | --- |
| `Fade(to)`, `FadeIn()`, `FadeOut()` | `Tween.Alpha` → `opacity` | `resolvedStyle.opacity` |
| `Color(to)` | `Tween.VisualElementColor` → `color` | `resolvedStyle.color` |
| `BackgroundColor(to)` | `Tween.VisualElementBackgroundColor` → `backgroundColor` | `resolvedStyle.backgroundColor` |
| `Move(to)`, `MoveX(to)`, `MoveY(to)` | `Tween.Position` → `translate` | `resolvedStyle.translate` |
| `Scale(Vector2 to)`, `Scale(float to)` | `Tween.Scale` → `scale` | `resolvedStyle.scale` |
| `Rotate(to)`, `Rotate(from, to)` | `style.rotate` in degrees | `resolvedStyle.rotate.angle` |
| `Width(to)`, `Width(from, to)` | `style.width` | `resolvedStyle.width` |
| `Height(to)`, `Height(from, to)` | `style.height` | `resolvedStyle.height` |
| `FontSize(to)`, `FontSize(from, to)` | `style.fontSize` | `resolvedStyle.fontSize` |
| `TextOutlineColor(to)`, `TextOutlineColor(from, to)` | `style.unityTextOutlineColor` | `resolvedStyle.unityTextOutlineColor` |
| `TextOutlineWidth(to)`, `TextOutlineWidth(from, to)` | `style.unityTextOutlineWidth` | `resolvedStyle.unityTextOutlineWidth` |

## Runtime API

Every method is an extension on `VisualElement`, returns `Tween`, and ends with the same optional settings tail:

```csharp
public static Tween Fade(this VisualElement ve, float to, float duration,
    Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
    float startDelay = 0f, float endDelay = 0f, bool useUnscaledTime = false);
```

```csharp
// VisualElementOpacityExtensions
Tween Fade(float to, float duration, ...);
Tween FadeIn(float duration, ...);   // to = 1
Tween FadeOut(float duration, ...);  // to = 0

// VisualElementColorExtensions
Tween Color(Color to, float duration, ...);
Tween BackgroundColor(Color to, float duration, ...);

// VisualElementTranslateExtensions
Tween Move(Vector2 to, float duration, ...);
Tween MoveX(float to, float duration, ...);
Tween MoveY(float to, float duration, ...);

// VisualElementScaleExtensions
Tween Scale(Vector2 to, float duration, ...);
Tween Scale(float to, float duration, ...);

// VisualElementRotateExtensions
Tween Rotate(float toDegrees, float duration, ...);
Tween Rotate(float fromDegrees, float toDegrees, float duration, ...);

// VisualElementSizeExtensions
Tween Width(float to, float duration, ...);
Tween Width(float from, float to, float duration, ...);
Tween Height(float to, float duration, ...);
Tween Height(float from, float to, float duration, ...);

// VisualElementTextExtensions
Tween FontSize(float to, float duration, ...);
Tween FontSize(float from, float to, float duration, ...);
Tween TextOutlineColor(Color to, float duration, ...);
Tween TextOutlineColor(Color from, Color to, float duration, ...);
Tween TextOutlineWidth(float to, float duration, ...);
Tween TextOutlineWidth(float from, float to, float duration, ...);
```

- Passing a `null` element returns `default(Tween)`; the call is a no-op and `Tween.isAlive` is `false`.
- `Scale(float to)` applies the same factor to both axes.
- `Rotate` interpolates degrees around the element's `transform-origin`.

## Requirements

- Unity 2022.3+
- `com.unity.modules.uielements` 1.0.0
- `com.kyrylokuzyk.primetween` 1.3.8
- UPM package name: `com.nk7.uitk.tween`
- Namespace: `Nk7.UITK.Extensions`
