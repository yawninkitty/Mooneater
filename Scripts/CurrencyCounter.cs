using TMPro;
using UnityEngine;

public class CurrencyCounter : MonoBehaviour
{
    public TMP_Text currencyText;

    private void Start()
    {
        // Подписываемся на событие
        CurrencyManager.OnCurrencyChanged += UpdateCurrencyText;
        // Обновляем текст сразу (актуальное значение)
        UpdateCurrencyText(CurrencyManager.Currency);
    }

    private void UpdateCurrencyText(int amount)
    {
        currencyText.text = amount.ToString();
    }
}