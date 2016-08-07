using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour {

	void OnMouseDown(){
		Debug.Log ("Hizo click");
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(0)){
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if(Physics.Raycast(ray,out hit)){
				BoxCollider bc = hit.collider as BoxCollider;
				if(bc!=null){
					Debug.Log ("Hizo click");
				}
			}
		}
	}
}
