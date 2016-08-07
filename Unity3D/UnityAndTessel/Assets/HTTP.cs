using UnityEngine;
using System.Collections;

public class HTTP : MonoBehaviour {

	public WWW www;

	// Use this for initialization
	void Start () {
		string url = "http://192.168.0.7:8080/analog";
		www = new WWW(url);
		StartCoroutine(WaitForRequest(www));
	}
	
	// Update is called once per frame
	void Update () {
	}

	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;

		// check for errors
		if (www.error == null) {
			Debug.Log ("WWW Ok!: " + www.data);
		} else {
			Debug.Log ("WWW Error: " + www.error);
		}
	}

}
