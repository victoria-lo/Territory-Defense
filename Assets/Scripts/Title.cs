using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    string sceneName;
    Animator transition;
    void Start()
    {
        transition = GameObject.Find("Transition").GetComponent<Animator>();
    }
    public void ChangeScene(string scene)
    {
        sceneName = scene;
        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(1f);
        transition.SetTrigger("Exit");
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sceneName);
    }

}
