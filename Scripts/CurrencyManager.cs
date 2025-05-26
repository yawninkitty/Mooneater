using UnityEngine;
using System; // Для Action

public static class CurrencyManager
{
    private static int _currency = 0;
    public static int Currency => _currency;

    // Событие, которое вызывается при изменении валюты
    public static event Action<int> OnCurrencyChanged;

    //вызывается при старте PlayerHealth (при возрождении)
    public static void ResetToCheckpointState()
    {            
        Debug.Log($"В памяти: {DataContainer.coinAmount}");
        _currency = DataContainer.coinAmount;
        Debug.Log($"Теперь валюты: {_currency}");
        OnCurrencyChanged?.Invoke(_currency);
    }

    public static void AddCurrency(int amount)
    {
        _currency += amount;
        Debug.Log($"Добавлено {amount} монет. Всего: {_currency}");
        OnCurrencyChanged?.Invoke(_currency); // Уведомляем подписчиков
        DataContainer.pendingCoinAmount = _currency;
    }

    public static bool TrySpendCurrency(int amount)
    {
        if (_currency >= amount)
        {
            _currency -= amount;
            Debug.Log($"Списано {amount} монет. Осталось: {_currency}");
            OnCurrencyChanged?.Invoke(_currency);
            DataContainer.pendingCoinAmount = _currency;
            return true;
        }
        Debug.Log($"Недостаточно монет! Нужно: {amount}, есть: {_currency}");
        return false;
    }
}