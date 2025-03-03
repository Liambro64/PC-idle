using UnityEngine;
using UnityEngine.EventSystems;

public class WindowHelper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Object windowScript;
    public void OnPointerEnter(PointerEventData ed)
    {
        ((Window)windowScript).overTop = true;
    }
    public void OnPointerExit(PointerEventData ed)
    {
        ((Window)windowScript).overTop = false;
    }
}
