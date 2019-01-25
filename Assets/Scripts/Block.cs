using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Block : MonoBehaviour {
    bool stack = false;
    bool onGround = false;
    BlockEvent onGroundListener;
    BlockOnBlockEvent onBlockListener;
    Rigidbody body;
    public void Init()
    {
        stack = false;
        onGround = false;
        onGroundListener = new BlockEvent();
        onBlockListener = new BlockOnBlockEvent();
    }

    private void Awake()
    {
        this.body = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(string.Format("OnCollisionEnter: {0}, {1}", collision.contacts.Length, collision.transform.gameObject.name));
        foreach(ContactPoint point in collision.contacts)
        {
            Debug.Log(string.Format("- Point: {0}", point.point.ToString()));
        }
        //Vector3 edge = collision.contacts[0].point - collision.contacts[1].point;

        ///Debug.Log(string.Format("- Edge: {0}", edge.ToString()));
        stack = true;
        Rigidbody blockBody = GetComponent<Rigidbody>();
        if (collision.transform.gameObject.name.Equals("Ground"))
        {
            onGround = true;
            if (onGroundListener != null)
            {
                onGroundListener.Invoke(this);
            }
        }
        else if (collision.transform.gameObject.tag.Equals("Block"))
        {
            Block targetBlock = collision.transform.gameObject.GetComponent<Block>();
            onBlockListener.Invoke(this, targetBlock);
        }
    }
    void OnCollisionExit(Collision collision)
    {
        Debug.Log(string.Format("OnCollisionExit: {0}", collision.gameObject.transform.position.ToString()));
    }
    public BlockEvent OnGroundListener
    {
        get { return this.onGroundListener; }
    }
    public BlockOnBlockEvent OnBlockListener
    {
        get { return this.onBlockListener; }
    }
    public void SetStatic()
    {
        this.body.isKinematic = true;
    }
}
