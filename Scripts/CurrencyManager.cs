using UnityEngine;
using System; // ��� Action

public static class CurrencyManager
{
    private static int _currency = 0;
    public static int Currency => _currency;

    // �������, ������� ���������� ��� ��������� ������
    public static event Action<int> OnCurrencyChanged;

    //���������� ��� ������ PlayerHealth (��� �����������)
    public static void ResetToCheckpointState()
    {            
        Debug.Log($"� ������: {DataContainer.coinAmount}");
        _currency = DataContainer.coinAmount;
        Debug.Log($"������ ������: {_currency}");
        OnCurrencyChanged?.Invoke(_currency);
    }

    public static void AddCurrency(int amount)
    {
        _currency += amount;
        Debug.Log($"��������� {amount} �����. �����: {_currency}");
        OnCurrencyChanged?.Invoke(_currency); // ���������� �����������
        DataContainer.pendingCoinAmount = _currency;
    }

    public static bool TrySpendCurrency(int amount)
    {
        if (_currency >= amount)
        {
            _currency -= amount;
            Debug.Log($"������� {amount} �����. ��������: {_currency}");
            OnCurrencyChanged?.Invoke(_currency);
            DataContainer.pendingCoinAmount = _currency;
            return true;
        }
        Debug.Log($"������������ �����! �����: {amount}, ����: {_currency}");
        return false;
    }
}