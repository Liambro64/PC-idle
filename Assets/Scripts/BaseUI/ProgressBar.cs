
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image outerBar, innerBar;
    public float progress {
        get => outerBar.gameObject.GetComponent<RectTransform>().sizeDelta.x -  (innerBar.gameObject.GetComponent<RectTransform>().sizeDelta.x + 14);
        set => Mathf.Lerp(0, outerBar.gameObject.GetComponent<RectTransform>().sizeDelta.x - 14, value % 1);
    }
}
