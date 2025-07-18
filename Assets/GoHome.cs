using UnityEngine;
using UnityEngine.SceneManagement;

public class GoHome : MonoBehaviour
{
    public void Home()
    {
        SceneManager.LoadScene("Title");
    }
}
