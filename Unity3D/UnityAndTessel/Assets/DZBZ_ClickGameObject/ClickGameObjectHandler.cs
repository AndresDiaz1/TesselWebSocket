using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ClickGameObjectHandler : MonoBehaviour {

    public static ClickGameObjectHandler Instance;


	void Start () {
	}

    public void OnClickedObject(GameObject obj)
    {        /*
         Insert your methods here
         You can do whatever you want
         */
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.AddForce(transform.up * 5F, ForceMode.Impulse);
			StartCoroutine(GetText());
        }
        else Debug.LogError("no rigid body on clicked object name=" + obj.name);
    }

	IEnumerator GetText()
	{
		string url = "http://192.168.0.7:8080/leds/0";
		UnityWebRequest www = UnityWebRequest.Get (url);
		yield return www.Send();

		if(www.isError) {
			Debug.Log(www.error);
		}
		else {
			// Show results as text
			Debug.Log(www.downloadHandler.text);

			// Or retrieve results as binary data
			byte[] results = www.downloadHandler.data;
		}
	}




    void Awake()
    {
        Instance = this;
    }

}
