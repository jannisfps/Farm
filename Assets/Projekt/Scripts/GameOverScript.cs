using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
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
}