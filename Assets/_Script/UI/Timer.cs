using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerTxt;
    [SerializeField] float remainingTime;

     void Update()
     { 
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }

        else if (remainingTime < 0)
        {
            remainingTime = 0;
            SceneManager.LoadSceneAsync(2);
            timerTxt.color = Color.red;
        }
            
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60); 

        timerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
