using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishManager : MonoBehaviour
{
    public int index;

    private void Start()
    {
        if (index == 3)
        {
            gameObject.SetActive(false);
        }
    }

    public void ActivateFinish()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            DataContainer.UpdateLevel(index);
            SceneManager.LoadScene("MainMenu");
        }
    }
}
