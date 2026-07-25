# nk7-uitk-tween

Лёгкие расширения PrimeTween для Unity UI Toolkit: твины без аллокаций вызываются прямо на `VisualElement`, стартовые значения берутся из `resolvedStyle` — в месте вызова остаётся указать только цель анимации.

## Особенности

- Extension-методы на `VisualElement` для прозрачности, цвета, трансформа, размеров и стиля текста.
- Относительные перегрузки берут стартовое значение из `resolvedStyle`, поэтому весь твин описывается одним аргументом.
- `Rotate` работает в градусах поверх кватернионного API PrimeTween — никаких `Quaternion.Euler` в местах вызова.
- Закрывает свойства, для которых в PrimeTween нет готового UI Toolkit-твина: `Width`, `Height`, `FontSize`, `TextOutlineColor`, `TextOutlineWidth`.
- Каждый метод возвращает `Tween` из PrimeTween, поэтому результат складывается в `Sequence`, `await`, `OnComplete` и `Stop`.
- Полный набор настроек PrimeTween (`Ease`, `cycles`, `CycleMode`, `startDelay`, `endDelay`, `useUnscaledTime`) доступен в каждой перегрузке.
- Null-safe: для `null`-элемента возвращается `default(Tween)` вместо исключения.
- Без аллокаций — запись свойств идёт через `static`-лямбды и встроенные `VisualElement`-твины PrimeTween.

## Содержание

- [Установка](#установка)
  - [Unity Package Manager](#unity-package-manager)
  - [Ручная установка](#ручная-установка)
- [Быстрый старт](#быстрый-старт)
  - [1. Подключите namespace](#1-подключите-namespace)
  - [2. Анимируйте элемент](#2-анимируйте-элемент)
  - [3. Композиция и await](#3-композиция-и-await)
  - [4. Остановка твинов](#4-остановка-твинов)
- [Стартовые значения](#стартовые-значения)
- [Трансформ vs лейаут](#трансформ-vs-лейаут)
- [Справочник методов](#справочник-методов)
- [Runtime API](#runtime-api)
- [Требования](#требования)

## Установка

### Unity Package Manager

1. Откройте Unity Package Manager (`Window → Package Manager`).
2. Нажмите `+ → Add package from git URL…`.
3. Вставьте `https://github.com/lsd7nk/nk7-uitk-tween.git?path=src`.

PrimeTween опубликован в npm, поэтому зависимость резолвится только если в проекте-потребителе объявлен его scoped-реджистри в `Packages/manifest.json`:

```json
"scopedRegistries": [
  {
    "name": "npm",
    "url": "https://registry.npmjs.org/",
    "scopes": [ "com.kyrylokuzyk" ]
  }
]
```

Unity не обновляет git-пакеты автоматически – при необходимости меняйте хеш вручную или используйте [UPM Git Extension](https://github.com/mob-sakai/UpmGitExtension).

### Ручная установка

Скопируйте папку `src` в проект и добавьте `Nk7.UITK.Tween.asmdef` к сборке.

## Быстрый старт

### 1. Подключите namespace

```csharp
using Nk7.UITK.Extensions;
using PrimeTween;
```

Сборка помечена `autoReferenced`, так что ссылка на неё в asmdef проекта не нужна.

### 2. Анимируйте элемент

Передайте целевое значение и длительность – стартовое значение возьмётся из текущего resolved-стиля элемента.

```csharp
var panel = root.Q<VisualElement>("panel");

panel.FadeIn(0.25f);
panel.Scale(1.1f, 0.2f, Ease.OutBack);
panel.MoveY(0f, 0.3f, Ease.OutCubic);
panel.BackgroundColor(Color.red, 0.15f, cycles: 2, cycleMode: CycleMode.Yoyo);
```

Используйте перегрузки с явными `from`/`to`, когда старт не должен зависеть от текущего стиля.

```csharp
panel.Rotate(-15f, 0f, 0.3f, Ease.OutBack);
panel.Width(0f, 320f, 0.4f);
```

### 3. Композиция и await

Возвращаемые твины — обычные хендлы PrimeTween.

```csharp
Sequence.Create()
    .Chain(panel.FadeIn(0.2f))
    .Group(panel.Scale(1f, 0.2f, Ease.OutBack))
    .ChainCallback(() => Debug.Log("shown"));

await panel.FadeOut(0.2f);
panel.style.display = DisplayStyle.None;
```

### 4. Остановка твинов

Останавливайте всё, что запущено на элементе, через target-API PrimeTween — перед переиспользованием или освобождением элемента.

```csharp
Tween.StopAll(panel);     // либо Tween.CompleteAll(panel)
```

## Стартовые значения

- Относительные перегрузки (`Fade`, `Move`, `Scale`, `Rotate(to)`, `Width(to)`, `FontSize(to)`, …) читают `resolvedStyle` в момент вызова.
- `resolvedStyle` осмыслен только после первого прохода лейаута. Вызов до раскладки элемента стартует твин из `0` или `NaN`; запускайте такие анимации после `GeometryChangedEvent` (или когда элемент уже прикреплён к панели) — либо используйте перегрузки с явными `from`/`to`.
- USS-переходы на тех же свойствах конкурируют с твином за значение. Не включайте анимируемые свойства в `transition-property`.

## Трансформ vs лейаут

- `Move`, `MoveX`, `MoveY`, `Scale` и `Rotate` пишут в трансформ элемента (`translate` / `scale` / `rotate`) и не инвалидируют лейаут — самый дешёвый способ анимации.
- `Width` и `Height` пишут лейаут-стили, поэтому каждый кадр пересчитывается раскладка поддерева. Для показа/скрытия предпочитайте `Scale`, а твины размеров оставьте элементам, которым реально нужно переразложиться.
- `FontSize` и `TextOutlineWidth` каждый кадр перегенерируют геометрию текста; на крупном тексте используйте короткие длительности.

## Справочник методов

| Метод | Куда пишет | Стартовое значение (относительная перегрузка) |
| --- | --- | --- |
| `Fade(to)`, `FadeIn()`, `FadeOut()` | `Tween.Alpha` → `opacity` | `resolvedStyle.opacity` |
| `Color(to)` | `Tween.VisualElementColor` → `color` | `resolvedStyle.color` |
| `BackgroundColor(to)` | `Tween.VisualElementBackgroundColor` → `backgroundColor` | `resolvedStyle.backgroundColor` |
| `Move(to)`, `MoveX(to)`, `MoveY(to)` | `Tween.Position` → `translate` | `resolvedStyle.translate` |
| `Scale(Vector2 to)`, `Scale(float to)` | `Tween.Scale` → `scale` | `resolvedStyle.scale` |
| `Rotate(to)`, `Rotate(from, to)` | `style.rotate` в градусах | `resolvedStyle.rotate.angle` |
| `Width(to)`, `Width(from, to)` | `style.width` | `resolvedStyle.width` |
| `Height(to)`, `Height(from, to)` | `style.height` | `resolvedStyle.height` |
| `FontSize(to)`, `FontSize(from, to)` | `style.fontSize` | `resolvedStyle.fontSize` |
| `TextOutlineColor(to)`, `TextOutlineColor(from, to)` | `style.unityTextOutlineColor` | `resolvedStyle.unityTextOutlineColor` |
| `TextOutlineWidth(to)`, `TextOutlineWidth(from, to)` | `style.unityTextOutlineWidth` | `resolvedStyle.unityTextOutlineWidth` |

## Runtime API

Каждый метод — extension на `VisualElement`, возвращает `Tween` и заканчивается одинаковым хвостом необязательных настроек:

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

- Для `null`-элемента возвращается `default(Tween)`: вызов ничего не делает, `Tween.isAlive` равен `false`.
- `Scale(float to)` применяет один и тот же коэффициент к обеим осям.
- `Rotate` интерполирует градусы вокруг `transform-origin` элемента.

## Требования

- Unity 2022.3+
- `com.unity.modules.uielements` 1.0.0
- `com.kyrylokuzyk.primetween` 1.3.8
- Имя UPM-пакета: `com.nk7.uitk.tween`
- Namespace: `Nk7.UITK.Extensions`
