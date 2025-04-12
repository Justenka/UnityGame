using System.Collections;
using UnityEngine;

public class EnableMinimapCameraLate : MonoBehaviour
{
    [SerializeField] private GameObject minimapCameraObject;

    void Start()
    {
        StartCoroutine(EnableNextFrame());
    }

    private IEnumerator EnableNextFrame()
    {
        yield return null;
        minimapCameraObject.SetActive(true);
    }
}