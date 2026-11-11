using UnityEngine;

// Базовый класс для всех бустеров
public abstract class BaseBooster : ScriptableObject
{
    public string boosterName;
    public Sprite icon;
    
    // Вызывается при активации бустера
    public abstract void Activate();
}