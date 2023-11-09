using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public static class TransformEx {

	public static Vector2 ScreenPointToCanvasPosition(this RectTransform rt, Vector2 p) {
		return new Vector2(p.x * 1280.0f / Screen.width, p.y * 720.0f / Screen.height);
	}

	public static EventTrigger OnEventTrigger(this Transform t, int typeEnm, System.Action<PointerEventData> cb) {
		var et = t.GetComponent<EventTrigger>();
		if (et == null)
			et = t.gameObject.AddComponent<EventTrigger>();
		var entry = new EventTrigger.Entry();
		entry.eventID = (EventTriggerType)typeEnm;
		entry.callback.AddListener((data) => { 
			cb(data as PointerEventData); 
		});
		et.triggers.Add(entry);
		return et;
	}

	public static Text SetText(this Transform t, string text) {
		Text textUI = t.GetComponent<Text> ();
		if (textUI == null) {
			Debug.LogError (t.name + " dont have Text cpn");
			return null;
		}
		textUI.text = text;
		return textUI;
	}

	public static Image SetSprite(this Transform t,Sprite sprite) {
		Image imgUI = t.GetComponent<Image> ();
		if (imgUI == null) {
			Debug.LogError (t.name + " dont have Image cpn");
			return null;
		}
		imgUI.sprite = sprite;
		return imgUI;
	}

	public static RawImage SetTexture(this Transform t,UnityEngine.Object texture) {
		RawImage rawImg = t.GetComponent<RawImage> ();
		if (rawImg == null) {
			Debug.LogError (t.name + " dont have RawImage cpn");
			return null;
		}
		rawImg.texture = (Texture2D)texture;
		return rawImg;
	}


	public static Button OnClick_(this Transform t ,UnityEngine.Events.UnityAction act) {
		Button btn = t.GetComponent<Button> ();
		if (btn == null) {
			Debug.LogError (t.name + " dont have button cpn");
			return null;
		}
		btn.onClick.AddListener (act);
		return btn;
	}

	public static Toggle OnToggle(this Transform t,UnityEngine.Events.UnityAction<bool> act) {
		Toggle tgl = t.GetComponent<Toggle>();
		if(tgl == null) {
			Debug.LogError(t.name + " dont have toggle cpn");
			return null;
		}
		tgl.onValueChanged.AddListener(act);
		return tgl;
	}

	public static Dropdown OnDropdownClick(this Transform t,UnityEngine.Events.UnityAction<int> act) {
		Dropdown dd = t.GetComponent<Dropdown> ();
		if (dd == null) {
			Debug.LogError (t.name + " dont have dropdown cpn");
			return null;
		}
		dd.onValueChanged.AddListener (act);
		return dd;
	}
}
