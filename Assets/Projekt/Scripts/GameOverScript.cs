using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameOverScript : MonoBehaviour
{
    public TMP_Text highScoreText;
    public TMP_Text score;
    // Neustart der aktuellen Szene (Retry)
    public void Retry()
    {
        Time.timeScale = 1f; 
if ( GameManager.instance.gameOverCanvas != null)
        {
            GameManager.instance.gameOverCanvas.SetActive(false);
        }

        
         GameManager.instance.ResetGameValues();

        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);    
    }

    // Zurück zum Startmenü (Exit)
    public void ExitToMenu()
    {
       Application.Quit();
    }
    public void Updategameover()
    { 
        if (highScoreText != null && HighScore.instance.highScores.Count != 0) highScoreText.text = "Highscore: " + HighScore.instance.highScores[0].Score;
        score.text = ""+GameManager.instance.player.score; 
    }
    
    
}