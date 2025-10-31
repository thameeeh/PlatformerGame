using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelRestarter : MonoBehaviour
{
    [SerializeField] KeyCode key = KeyCode.R;
    [SerializeField] bool resetTimeScale = true;

    void Update()
    {
        if (Input.GetKeyDown(key))
            Restart();
    }

    public void Restart()
    {
        if (resetTimeScale) Time.timeScale = 1f;
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
}
