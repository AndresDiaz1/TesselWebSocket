using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickGameObject : MonoBehaviour {

    [Header("If need all layers")]
    public bool CheckAllLayers = true; //  other layers be ignore;
    [Header("Only one layer")]
    public int ClickableLayer = 0;
    [Header("Distance between camera and object")]
    public float MaxDistanceFromCamToObject = 100F;
    [Header("Show all steps in debug console")]
    public bool IS_DEBUG = true;
    [Header("Time to show debug rays")]
    public float TimeShowDebugRaysSec = 2F;
    [Header("Debug rays color")]
    public Color DebugRayColor = Color.red;

    [Header("Thanks to developer")]
    public string PleaseInstallMyGames  = "https://play.google.com/store/apps/developer?id=dzbz";
    public string note = "Spend a couple of minutes, install my games on the phone. Thank you";

    private void OnClick()
    {
        //Getting clicked object
        GameObject clickedObject = this.GetClickedTransform();

        //if clicked object null ignore other code
        if (clickedObject == null)
        {
            if(IS_DEBUG) Debug.Log("ClickGameObject : OnClick : clicked NOTHING");
            return;
        }
        if (IS_DEBUG) Debug.Log("ClickGameObject : OnClick  Name = " + clickedObject.name);

        //check layer object and layer for click
        bool ok = this.CheckLayer(clickedObject);

        //if layer ok send to handler
        if (ok == true) 
        {
            SendToHandler(clickedObject);
        }
    }

    private void SendToHandler(GameObject g)
    {
        if (ClickGameObjectHandler.Instance)
        {
            ClickGameObjectHandler.Instance.OnClickedObject(g);
        }
        else Debug.LogError("ClickGameObjectHandler is null! PUT ClickGameObjectHandler to scene editor!");
    }

    private GameObject GetClickedTransform()
    {
        RaycastHit hit;
        Ray MyRay;
        MyRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        StartCoroutine(RayShowCo(MyRay.origin, MyRay.direction));
        if (Physics.Raycast(MyRay, out hit, MaxDistanceFromCamToObject))
        {
            return hit.transform.gameObject;
        }
        if (IS_DEBUG) Debug.Log("ClickGameObject : GetClickedTransform : null");
        return null;
    }

    private bool CheckLayer(GameObject g)
    {
        if (IS_DEBUG) Debug.Log("ClickGameObject : CheckLayer...");
        if (g == null)
        {
            if (IS_DEBUG) Debug.LogWarning("ClickGameObject: CheckLayer: game object == null cant get layer from null");
            return false;
        }

        int layer = g.layer;

        if (IS_DEBUG) Debug.Log("ClickGameObject : CheckLayer...clicked obj layer is = " + layer + "\n need layer = " + ClickableLayer.ToString());
        //allow all layers
        if (CheckAllLayers == true)
        {
            if (IS_DEBUG) Debug.Log("ClickGameObject : CheckLayer...CheckAllLayers == true return TRUE");
            return true;
        }

        //one layer check
        if (layer == ClickableLayer)
        {
            if (IS_DEBUG) Debug.Log("ClickGameObject : CheckLayer... layer == ClickableLayer return TRUE");
            return true;
        }
         
        else
        {
            if (IS_DEBUG) Debug.Log("ClickGameObject : CheckLayer... IGNORING CLICK");
            return false;
        }
    }

 
    private IEnumerator RayShowCo(Vector3 from, Vector3 direction)
    {
        float startRayTime = Time.time;
        float timeDiff = 0;
        while (timeDiff < TimeShowDebugRaysSec)
        {
            timeDiff = Time.time - startRayTime;
            Debug.DrawRay(from, direction * MaxDistanceFromCamToObject, DebugRayColor);
            yield return new WaitForFixedUpdate();
        }
    }
 

    void Update () {

        // refresh click
        if (Input.GetMouseButtonDown(0))
        {
            if (IS_DEBUG) Debug.Log("ClickGameObject Update: Pressed left click or tap");
            OnClick();
        }
    }
}
