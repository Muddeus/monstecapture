using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public int lives;
    [SerializeField] GameObject oneLives;
    [SerializeField] GameObject twoLives;
    [SerializeField] GameObject threeLives;

    private void Start()
    {
        lives = 3;
    }

    void Update()
    {
        if (lives <= 0)
        {
            Debug.Log("Dead");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (lives == 1)
        {
            oneLives.SetActive(true);
            twoLives.SetActive(false);
            threeLives.SetActive(false);
        }

        if (lives == 2)
        {
            oneLives.SetActive(false);
            twoLives.SetActive(true);
            threeLives.SetActive(false);
        }

        if (lives == 3)
        {
            oneLives.SetActive(false);
            twoLives.SetActive(false);
            threeLives.SetActive(true);
        }
    }

    public void TakeDamage()
    {
        lives--;
    }
}
