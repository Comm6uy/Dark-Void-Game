using UnityEngine;
using System;

public abstract class BaseFlashlight : MonoBehaviour
{
    [Header("Параметры энергии")]
    [SerializeField] protected float maxEnergy = 100f;
    [SerializeField] protected float energyDrainRate = 10f; // Расход в секунду
    [SerializeField] protected float rechargeRate = 15f;    // Восстановление в секунду
    [SerializeField] protected float rechargeCooldown = 2f; // Задержка перед началом зарядки

    public float CurrentEnergy { get; protected set; }
    public bool IsOn { get; protected set; }
    public bool IsEnergyActive { get; set; } = false; // Для бустера

    protected Light flashlightLight; // Если используете компонент Light (3D/2D)
    private float lastTurnOffTime = 0f;
    private bool isOverheated = false; // Состояние "Ушел на перезарядку"

    // Событие для обновления UI
    public Action<float, float> OnEnergyChanged;

    protected virtual void Awake()
    {
        CurrentEnergy = maxEnergy;
        flashlightLight = GetComponentInChildren<Light>(); 
    }

    protected virtual void Update()
    {
        HandleEnergy();
    }

    public virtual void ToggleFlashlight(bool state)
    {
        if (isOverheated) return; // Нельзя включить, пока не зарядится

        IsOn = state;
        
        if (flashlightLight != null)
            flashlightLight.enabled = IsOn;

        if (!IsOn)
            lastTurnOffTime = Time.time;
            
        OnFlashlightToggled(IsOn);
    }

    private void HandleEnergy()
    {
        if (IsOn)
        {
            if (!IsEnergyActive)
            {
                CurrentEnergy -= energyDrainRate * Time.deltaTime;
                if (CurrentEnergy <= 0)
                {
                    CurrentEnergy = 0;
                    isOverheated = true; // Уходит на принудительную перезарядку
                    ToggleFlashlight(false);
                }
                OnEnergyChanged?.Invoke(CurrentEnergy, maxEnergy);
            }
        }
        else
        {
            // Перезарядка начинается только если выключен и прошло время cooldown
            if (CurrentEnergy < maxEnergy && Time.time - lastTurnOffTime >= rechargeCooldown)
            {
                CurrentEnergy += rechargeRate * Time.deltaTime;
                if (CurrentEnergy >= maxEnergy)
                {
                    CurrentEnergy = maxEnergy;
                    isOverheated = false; // Фонарик снова готов к работе
                }
                OnEnergyChanged?.Invoke(CurrentEnergy, maxEnergy);
            }
        }
    }

    // Метод для переопределения в наследниках (например, для смены спрайта, звука и т.д.)
    protected virtual void OnFlashlightToggled(bool state) { }
}