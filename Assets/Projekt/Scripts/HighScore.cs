using System.Collections.Generic;
using UnityEngine;

public class HighScore : MonoBehaviour
{  
    public static HighScore instance;
    public List<HighScores> highScores = new List<HighScores>();

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
