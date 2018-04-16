using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {
    [SerializeField]
    private GameObject inGameMenu;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        HandleEsc();
    }

    public void HandleEsc()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("test");
            ShowInGameMenu();
        }
    }

    public void ShowInGameMenu() {
        inGameMenu.SetActive(!inGameMenu.activeSelf);
        if (!inGameMenu.activeSelf)
        {
            Time.timeScale = 1;
        }
        else {
            Time.timeScale = 0;
        }
    }

    public void RestartGame() {
        SceneManager.LoadScene("Main");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
