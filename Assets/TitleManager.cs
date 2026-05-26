using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        // Stage1へ遷移
        SceneManager.LoadScene("ho");
    }
}