using UnityEngine;
using UnityEngine.Rendering;

public class Toggle : MonoBehaviour
{
    public void Start()
    {
    }
    public void toggleActive()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
