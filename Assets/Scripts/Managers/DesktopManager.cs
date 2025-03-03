using System;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;

public static class DesktopManager
{
	public static List<GameObject> Icons = new();
	public static IEnumerator<Shortcut> shortcuts{
		get => shortcutIEnumerator();
	}	
	static IEnumerator<Shortcut> shortcutIEnumerator() {
		int i = 0;
		while (i < Icons.Count)
			yield return Icons[i++].GetComponent<Shortcut>();
	}
	static RectTransform background;
	public static RectTransform Background{
		get => (background != null) ? background : GetBackground();
	}
	public static RectTransform GetBackground() {
		return background = (RectTransform)SceneManager.GetActiveScene().GetRootGameObjects()[0].transform.GetChild(0);
	}
	public static void Add(GameObject add) {
		if (Icons.Contains(add))
			return;
		RectTransform rt = add.GetComponent<RectTransform>();
		Vector2 Pos = pos(Icons.Count);
		Debug.Log(Pos);
		rt.localPosition = Pos;
		Icons.Add(add);
	}
	public static GameObject Add(GameObject shortcut, Func<GameObject, Transform, GameObject> instantiate)
	{
		if (Icons.Contains(shortcut))
			return null;
		if (background == null)
			GetBackground();
		GameObject gm;
		Icons.Add(gm = instantiate(shortcut, background));
		RectTransform rt = gm.GetComponent<RectTransform>();
		Vector2 Pos = pos(Icons.Count - 1);
		Debug.Log(Pos);
		rt.localPosition = pos(Icons.Count - 1);
		return gm;
	}
	static int up = 1030/175;
	public static Vector2 pos(int i){
		return new(
			25 + (i / up * 175),
			-(25 + (i % up * 175))
		);
	}
}