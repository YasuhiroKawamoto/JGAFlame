using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsyncLoadSample : MonoBehaviour
{
    [SerializeField]
    private int _button = 0;

    [SerializeField]
    private string _sceneName = string.Empty;

    void Update()
    {
        if (Input.GetMouseButtonDown(_button))
        {
            StartCoroutine(Util.SceneLoader.Instance.LoadSceneWait(_sceneName, () =>
            {
                Util.SceneLoader.Instance.ChangeScene();
            }));
        }
    }
}
