using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class FallDamage : MonoBehaviour
{
    [SerializeField] Canvas gameOverCanvas;

    private void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameOverCanvas.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        gameOverCanvas.gameObject.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
