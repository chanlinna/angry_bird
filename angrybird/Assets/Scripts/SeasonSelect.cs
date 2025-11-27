using UnityEngine;
using UnityEngine.SceneManagement;

public class SeasonSelect : MonoBehaviour
{
    public void PlayDefault()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void PlayWinter()
    {
        SceneManager.LoadScene("WinterScene");
    }
}
