using UnityEngine.SceneManagement;
using UnityEngine;

public class Botones : MonoBehaviour
{

    public void CambiarEscena(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
