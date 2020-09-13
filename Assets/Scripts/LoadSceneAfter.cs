using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAfter : MonoBehaviour
{
    public float Delay;
    public int SceneIndex;
    void Start()
    {
        StartCoroutine(LoadScene(SceneIndex));
    }
    private IEnumerator LoadScene(int SceneIndex)
    {
        yield return new WaitForSeconds(Delay);
        SceneManager.LoadScene(SceneIndex);
    }
}
