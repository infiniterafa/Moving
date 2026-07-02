using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance { get; private set; }
    public TextMeshProUGUI ScoreText;

    private void Awake()
    {
       if (instance !=null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    int score = 0;
  

  
    void Start()
    {
        ScoreText.text = score.ToString() +" VIBRA"; 
     
    }

    public void AddPoint()
    {
        score += 1;
        ScoreText.text = score.ToString() + " VIBRA";
    }

    public void SubstractPoint()
    {
        score -= 1;
        ScoreText.text = score.ToString() + " VIBRA";
    }
}
