using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {
    [SerializeField]
    public GameObject inGameMenu;
    [SerializeField]
    public GameObject optionsMenu;
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
    [SerializeField]
    public MusicPlayer musicPlayer;
    // Use this for initialization
    void Start() {
        ammoLabel.text = "";
    }

    // Update is called once per frame
    void Update() {
        HandleEsc();
        ammoLabel.text = string.Format("ammo: {0}", bulletCount);
        laserTimer.text = string.Format("TIMER: {0:00.00}", laserTime);
    }

    public void HandleEsc()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowInGameMenu();
        }
    }

    public void ShowInGameMenu() {
        if (optionsMenu.activeSelf)
        {
            ShowMain();
        }
        else {
            inGameMenu.SetActive(!inGameMenu.activeSelf);
            if (!inGameMenu.activeSelf)
            {
                musicPlayer.musicSource.UnPause();
                Time.timeScale = 1;

            }
            else
            {
                musicPlayer.musicSource.Pause();
                Time.timeScale = 0;
            }
        }
    }

    public void ShowOptions()
    {
        musicPlayer.musicSource.UnPause();
        inGameMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
    public void ShowMain()
    {
        inGameMenu.SetActive(true);
        optionsMenu.SetActive(false);
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
