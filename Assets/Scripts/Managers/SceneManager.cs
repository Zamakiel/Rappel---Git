using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public GlobalScene GlobalSceneToLoad;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
    }

    public void LoadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(GlobalSceneToLoad.Value.name);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
