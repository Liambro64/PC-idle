using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Unity.Entities.UniversalDelegates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;

public static class WindowManager
{
	public static List<GameObject> Windows = new();
	public static GameObject Add(GameObject window)
	{
		int c = Windows.Count;
		Windows.Add(window);
		return Windows[c];
	}
	public static void Remove(GameObject window)
	{
		Windows.Remove(window);
	}
	public static T getWindow<T>()
	{
		if (typeof(T) != typeof(Window))
			return default;
		for (int i = 0; i < Windows.Count; i++)
			if (Windows[i].GetComponent<Window>().GetType() == typeof(T))
				return (T) Windows[i].GetComponent<Window>().ConvertTo(typeof(T));
		return default;
	}
	public static Window getWindow(GameObject window)
	{
		for (int i = 0; i < Windows.Count; i++)
			if (Windows[i] == window)
				return Windows[i].GetComponent<Window>();
		return null;
	}
	public static Window getWindow(string name)
	{
		for (int i = 0; i < Windows.Count; i++)
			if (Windows[i].name == name)
				return Windows[i].GetComponent<Window>();
		return null;
	}
	public static GameObject getWindowObj(GameObject window)
	{
		for (int i = 0; i < Windows.Count; i++)
			if (Windows[i]== window)
				return Windows[i].gameObject;
		return null;
	}
	public static GameObject getWindowObj(string name)
	{
		for (int i = 0; i < Windows.Count; i++)
			if (Windows[i].name == name)
				return Windows[i];
		return null;
	}
	public static bool hasWindow(string name){
		for (int i = 0; i < Windows.Count; i++)
			if (Windows[i].name == name)
				return true;
		return false;
	}
	public static void Open(string name, GameObject window)
	{
		GameObject a;
		if (Windows == null)
			Windows = new();
		if ((a = getWindowObj(name)) != null)
			a.SetActive(true);
		else {
			Windows.Add(a = GameObject.Instantiate(window, Background.transform));
			Debug.Log("made new");
		}
		a.transform.SetAsLastSibling();
	}
	static GameObject background;
	public static GameObject Background
	{
		get => (background == null)
			   ? GetBackground()
			   : background;
	}
	public static GameObject GetBackground()
	{
		
		background = SceneManager.GetActiveScene().GetRootGameObjects()[0].transform.GetChild(0).gameObject;
		Debug.Log(background.name);
			
		return background;
	}
}