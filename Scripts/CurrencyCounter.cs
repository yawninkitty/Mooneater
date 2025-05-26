using TMPro;
using UnityEngine;

public class CurrencyCounter : MonoBehaviour
{
    public TMP_Text currencyText;

    private void Start()
    {
        // ������������� �� �������
        CurrencyManager.OnCurrencyChanged += UpdateCurrencyText;
        // ��������� ����� ����� (���������� ��������)
        UpdateCurrencyText(CurrencyManager.Currency);
    }

    private void UpdateCurrencyText(int amount)
    {
        currencyText.text = amount.ToString();
    }
}