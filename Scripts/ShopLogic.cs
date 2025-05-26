using UnityEngine;

public class ShopLogic : MonoBehaviour
{
    private Canvas ShopCanvas;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShopCanvas = GameObject.FindWithTag("Shop").GetComponent<Canvas>();
        ShopCanvas.gameObject.SetActive(false);
    }

    public void OpenShop()
    {
        Time.timeScale = 0f;
        ShopCanvas.gameObject.SetActive(true);
    }
    public void CloseShop()
    {
        Time.timeScale = 1f;
        ShopCanvas.gameObject.SetActive(false);
    }
}
