using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Canvas LevelsMenu;
    public Button level_1;
    public Button level_2;
    public Button level_3;
    public Button ClearPrefs;


    void Start()
    {
        ClearPrefs.gameObject.SetActive(false);
        DataContainer.LoadPlayerData();
        if (DataContainer.levelIndex == 0)
        {
            LevelsMenu.gameObject.SetActive(false);
            level_2.interactable = false;
            level_3.interactable = false;
        }
        else if (DataContainer.levelIndex ==  3)
        {
            LevelsMenu.gameObject.SetActive(false);
            level_1.interactable = true;
            level_2.interactable = true;
            level_3.interactable = true;
        }
        else
        {
            LevelsMenu.gameObject.SetActive(true);
            if (DataContainer.levelIndex == 1)
            {
                level_1.interactable = false;
                level_2.interactable = true;
                level_3.interactable = false;
            }
            else if (DataContainer.levelIndex == 2)
            {
                level_1.interactable = false;
                level_2.interactable = false;
                level_3.interactable = true;
            }
        }

    }

    public void OpenLevels()
    {
        LevelsMenu.gameObject.SetActive(true);
    }

    public void CloseLevels()
    {
        LevelsMenu.gameObject.SetActive(false);
    }

    //кнопки
    public void Level_1()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void Level_2()
    {
        SceneManager.LoadScene("Level_2");
    }

    public void Level_3()
    {
        SceneManager.LoadScene("Level_3");
    }

    public void Settings()
    {
        ClearPrefs.gameObject.SetActive(!ClearPrefs.gameObject.activeSelf);
    }

    public void ClearPrefsBtn()
    {
        DataContainer.ClearAllData();
        DataContainer.LoadPlayerData();
    }
}
