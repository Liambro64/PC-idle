using System;
using System.Linq;
using TMPro;
using Unity.Entities.UniversalDelegates;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    Image sp;
    TextMeshProUGUI text;
    Button button;
    public virtual descriptorVariables dv {
        get => null;
    }
    public bool bought;
    public Sprite sprite{
        get => ((sp == null) ? getsp() : sp).sprite;
        set => ((sp == null) ? getsp() : sp).sprite = value;
    }
    public Image getsp() {
        return sp = gameObject.GetComponent<Image>();
    }
    public string Name {
        get => ((text == null) ? getText() : text).text;
        set => ((text == null) ? getText() : text).text = value;
    }
    public TextMeshProUGUI getText() {
        return text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }
    public event Action OnPress{
        add     => ((button == null) ? getButton() : button).onClick.AddListener(() => value());
        remove  => ((button == null) ? getButton() : button).onClick.RemoveListener(() => value());
    }
    public Button getButton() {
        return button = gameObject.GetComponent<Button>();
    }
    public GameObject shortcut;
    public Shortcut windowType{
        get => shortcut != null ? shortcut.GetComponent<Shortcut>() : null;
    }
    public void Start()
	{
		enabled = false;
        extraStart();
	}
    public virtual void extraStart() {}
}