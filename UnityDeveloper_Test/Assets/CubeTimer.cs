using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CubeTimer : MonoBehaviour
{
    public float timer = 2f;
    public int playerScore;
    public int maxScore = 6;
    string timeInMinutes;
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Canvas congratsCanvas;
    FallDamage kill;
    PlayerController playa;
    // Start is called before the first frame update
    void Start()
    {
        congratsCanvas.gameObject.SetActive(false);
        playerScore = 0;
        kill = GetComponent<FallDamage>();
        playa = GetComponent<PlayerController>();
        Debug.Log("STARTING NOWWW");
    }

    // Update is called once per frame
    void Update()
    {
        FormatTime(timer);
        timerText.text = timeInMinutes;
        ScoreText.text = new string(playerScore.ToString() + "/6 cubes");
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            kill.GameOver();
        }
        if(playerScore == maxScore)
        {
            congratsCanvas.gameObject.SetActive(true);
        }
    }

    public void FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timeInMinutes = new string(minutes + ":" + seconds);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Pickups")
        {
            Debug.Log("touching cube");
            other.gameObject.SetActive(false);
            playerScore++;
        }
    }
}
