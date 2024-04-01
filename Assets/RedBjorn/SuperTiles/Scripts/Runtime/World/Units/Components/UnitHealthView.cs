using RedBjorn.SuperTiles.UI;
using RedBjorn.SuperTiles.Utils;
using RedBjorn.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// View which represents UnitHealthEntity at unity scene by operating Sprite component
    /// </summary>
    public class UnitHealthView : MonoBehaviour
    {
        float BarFillSpeed;
        BarSprite BarPrefab;
        BarSprite Bar;
        bool ShowChanges;
        HealthChangeView ChangePrefab;
        bool OnDeathBarHide;
        Color OnDeathBarColor;
        UnitEntity Unit;
        TransformTag UiHolderTag;
        Coroutine HealthProcessingCoroutine;
        Queue<float> Values = new Queue<float>();

        UnitStatTag MaxHealthTag;
        float MaxHealth => Unit[MaxHealthTag];

        void OnDestroy()
        {
            if (Unit != null)
            {
                Unit.Health.OnHeathChanged -= OnHealthChanged;
                Unit.View.OnStateChanged -= OnViewStateChanged;
            }
        }

        public void Init(UnitEntity unit,
            UnitStatTag maxHealthTag,
            bool showChanges,
            HealthChangeView changePrefab,
            TransformTag uiHolderTag,
            BarSprite barPrefab,
            float barFillSpeed,
            Color onDeathBarColor,
            bool onDeathBarHide)
        {
            ShowChanges = showChanges;
            UiHolderTag = uiHolderTag;
            MaxHealthTag = maxHealthTag;
            BarFillSpeed = barFillSpeed;
            BarPrefab = barPrefab;
            OnDeathBarColor = onDeathBarColor;
            OnDeathBarHide = onDeathBarHide;
            ChangePrefab = changePrefab;
            var context = new HealthChangeContext();
            if (Unit != null)
            {
                Unit.Health.OnHeathChanged -= OnHealthChanged;
                Unit.View.OnStateChanged -= OnViewStateChanged;
            }
            Unit = unit;
            if (Unit != null)
            {
                context.MaxHealth = MaxHealth;
                context.Current = Unit.Health.ValueCurrent;
                context.Previous = context.Current;
                Unit.Health.OnHeathChanged += OnHealthChanged;
                Unit.View.OnStateChanged += OnViewStateChanged;
            }
            Bar = Spawner.Spawn(BarPrefab, Unit.View.GetTransformHolder(UiHolderTag));
            Bar.transform.localPosition = Vector3.zero;
            Bar.gameObject.SetActive(true);
            OnHealthChanged(context);
        }

        void OnHealthChanged(HealthChangeContext context)
        {
            var val = Mathf.Clamp01(context.Current / MaxHealth);
            var delta = context.Current - context.Previous;
            if(ShowChanges && Mathf.Abs(delta) > 0.01f)
            {
                var change = Spawner.Spawn(ChangePrefab);
                change.transform.SetParent(Unit.View.GetTransformHolder(UiHolderTag));
                change.transform.localPosition = Vector3.zero;
                change.Init(delta);
            }
            Values.Enqueue(val);
            HandleHealthChange();
        }

        void HandleHealthChange()
        {
            if (HealthProcessingCoroutine == null && Values.Count > 0)
            {
                if (gameObject.activeInHierarchy)
                {
                    var val = Values.Dequeue();
                    HealthProcessingCoroutine = StartCoroutine(HealthProcessing(val));
                }
            }
        }

        IEnumerator HealthProcessing(float newVal)
        {
            var original = Bar.WidthCurrent;
            var target = newVal * Bar.WidthMax;
            var speed = Mathf.Abs(original - target) * BarFillSpeed;
            if (Mathf.Abs(speed) > 0.001f)
            {
                var time = 0f;
                while (time < 1f)
                {
                    Bar.WidthCurrent = Mathf.Lerp(original, target, time);
                    yield return null;
                    time += Time.deltaTime * speed;
                }
            }
            Bar.WidthCurrent = target;
            if (Bar.WidthCurrent <= 0f)
            {
                if (OnDeathBarHide)
                {
                    Bar.gameObject.SetActive(false);
                }
                else
                {
                    Bar.Background.color = OnDeathBarColor;
                }
            }
            HealthProcessingCoroutine = null;
            HandleHealthChange();
        }

        void OnViewStateChanged(bool state)
        {
            Bar.gameObject.SetActive(state);
        }
    }
}
