using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {
    [SerializeField]
    private GameObject inGameMenu;
    [SerializeField]
    private Text ammoLabel;
    public int bulletCount=0;
    public float laserTime = 0.0f;
    [SerializeField]
    private Text laserTimer;
    [SerializeField] public GameObject pistol;
    [SerializeField] public GameObject laser;
    [SerializeField] public GameObject machine;
    [SerializeField] public GameObject rocket;
    // Use this for initialization
    void Start() {
        ammoLabel.text = "";
        laserTimer.text = "";
    }

    // Update is called once per frame
    void Update() {
        HandleEsc();
        ammoLabel.text = "ammo: " + bulletCount;
        laserTimer.text = "TIMER: " + laserTime;
    }

    public void HandleEsc()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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
        Time.timeScale = 1;
        SceneManager.LoadScene("Main");
    }
    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        //   Application.Quit();
    }
}
