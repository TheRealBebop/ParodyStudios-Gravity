using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CubeTimer : MonoBehaviour
{
    float timer = 120f;
    public int playerScore;
    public int maxScore = 6;
    string timeInMinutes;
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI congratsText;
    // Start is called before the first frame update
    void Start()
    {
        congratsText.gameObject.SetActive(false);
        playerScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        FormatTime(timer);
        timerText.text = timeInMinutes;
        ScoreText.text = new string(playerScore.ToString() + "/6 cubes");
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            Debug.Log("Time's up!");
        }
        if(playerScore == maxScore)
        {
            congratsText.gameObject.SetActive(true);
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
