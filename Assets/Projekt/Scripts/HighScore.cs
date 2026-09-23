using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScore : MonoBehaviour
{  
    public static HighScore instance;
    public List<HighScores> highScores = new List<HighScores>();
    public TMP_Text highScoreText;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else Destroy(gameObject);
    }

    public void AddNewHighScore(string name, int score)
    {
        highScores.Add(new HighScores(name, score));
        SortHighScores();
        if (highScoreText != null && highScores.Count != 0) highScoreText.text = "Highscore: " + highScores[0].Score;
    }

    public void SortHighScores()
    {
        highScores.Sort((a, b) => b.Score.CompareTo(a.Score));
    }

    public class HighScores
    {
        public string Name {get;}
        public int Score {get;}

        public HighScores(string name, int score)
        {
            Name = name;
            Score = score;
        }
    }
}
