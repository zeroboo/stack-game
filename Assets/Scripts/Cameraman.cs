using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameraman : MonoBehaviour {
    Block focusBlock;
    Vector3 offset;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (focusBlock != null)
        {
            transform.position = focusBlock.transform.position + offset;
        }
	}

    public void SetFocusBlock(Block focusBlock)
    {
        offset = transform.position - focusBlock.gameObject.transform.position;
    }
}
