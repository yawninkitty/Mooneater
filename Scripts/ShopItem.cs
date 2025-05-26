using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public enum ItemType { HealthBonus, DamageBonus, MagnetBonus }
    [Header("��� ��������")]
    public ItemType type;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI quanText;
    private int ItemPrice = 0;
    private string ItemName = "";
    private int ItemQuantity = 0;

    private PlayerHealth PlayerHealth;
    private PlayerAttack PlayerAttack;

    private void Start()
    {
        PlayerHealth = FindAnyObjectByType<PlayerHealth>();
        PlayerAttack = FindAnyObjectByType<PlayerAttack>();

        switch (type)
        {
            case ItemType.HealthBonus: ItemPrice = 15; ItemName = "����� ����"; ItemQuantity = 8 - DataContainer.health; break;
            case ItemType.DamageBonus: ItemPrice = 15; ItemName = "����� ������"; ItemQuantity = 6 - DataContainer.damage; break;
            case ItemType.MagnetBonus: ItemPrice = 60; ItemName = "��������"; ItemQuantity = 1 - DataContainer.magnet; break;
            default: ItemPrice = 0; break;
        }

        //�������� �� �������
        switch (DataContainer.levelIndex)
        {
            case 0:
                if (type == ItemType.HealthBonus || type == ItemType.DamageBonus)
                {
                    ItemPrice = 15;
                }
                else
                {
                    ItemPrice = 24;
                }
                break;
            case 1:
                if (type == ItemType.HealthBonus || type == ItemType.DamageBonus)
                {
                    ItemPrice = 18;
                }
                else
                {
                    ItemPrice = 29;
                }
                break;
            case 2:
                if (type == ItemType.HealthBonus || type == ItemType.DamageBonus)
                {
                    ItemPrice = 21;
                }
                else
                {
                    ItemPrice = 34;
                }
                break;

        }

        if (ItemQuantity == 0)
        {
            Debug.Log($"���������� �����: {ItemName}");
            Destroy(gameObject);
        }

        priceText.text = ItemPrice.ToString();
        nameText.text = ItemName.ToString();
        quanText.text = ItemQuantity.ToString() + " ��.";

        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (CurrencyManager.TrySpendCurrency(ItemPrice))
            {
                ItemQuantity -= 1;
                switch (type)
                {
                    case ItemType.HealthBonus: PlayerBonusSystem.Health++; DataContainer.pendingHealth = PlayerBonusSystem.Health; PlayerHealth.ApplyHealthBonus(PlayerBonusSystem.Health); break;
                    case ItemType.DamageBonus: PlayerBonusSystem.Damage++; DataContainer.pendingDamage = PlayerBonusSystem.Damage; PlayerAttack.ApplyDamageBonus(PlayerBonusSystem.Damage); break;
                    case ItemType.MagnetBonus: PlayerBonusSystem.Magnet++; DataContainer.pendingMagnet = PlayerBonusSystem.Magnet; break;
                    default: return;
                }
                    Debug.Log($"�������: {ItemName}, �������� ����: {ItemQuantity}");
                    quanText.text = ItemQuantity.ToString() + " ��.";
                if (ItemQuantity == 0)
                {
                    Debug.Log($"���������� �����: {ItemName}");
                    Destroy(gameObject);
                }
            }
            else
            {
                Debug.Log($"�������� ����: {ItemQuantity}");
                
            }
        });
    }
}
