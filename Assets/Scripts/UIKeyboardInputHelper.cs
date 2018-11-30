using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIKeyboardInputHelper : MonoBehaviour, ICancelHandler, ISubmitHandler, IUpdateSelectedHandler {


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnCancel(BaseEventData eventData)
    {
		Debug.Log("OnCancel");
    }

    public void OnSubmit(BaseEventData eventData)
    {
		Debug.Log("OnSubmit");
    }

    public void OnUpdateSelected(BaseEventData eventData)
    {
		Debug.Log("OnUpdateSelected");
    }
}
