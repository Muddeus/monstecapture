using UnityEngine;
public class InputDisableChild : MonoBehaviour
{
    public GameObject highScores;

    private void Start()
    {
        highScores.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            highScores.SetActive(true);
        }
        else
        {
            highScores.SetActive(false);
        }
    }
}
