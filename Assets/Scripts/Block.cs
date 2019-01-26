using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Block : MonoBehaviour {
    bool isOnStack = false;
    bool isOnGround = false;
    bool isFlying = false;
    BlockEvent onGroundListener;
    [SerializeField]
    BlockOnBlockEvent onBlockListener;
    Rigidbody body;
    string blockName;

    Collision lastCollision;


    public void Init()
    {
        isOnStack = false;
        isOnGround = false;
        isFlying = false;
        onGroundListener = new BlockEvent();
        onBlockListener = new BlockOnBlockEvent();
        this.body = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    public void StartFlying()
    {
        this.isFlying = true;
        this.body.isKinematic = false;
    }
    public void StopFlying()
    {
        this.isFlying = false;
        body.velocity = Vector3.down*20;
        body.isKinematic = true;
    }
    public void SetOnStack()
    {
        this.isOnStack = true;
      
    }
    void ProcessCollision(Collision collision)
    {
        //Vector3 edge = collision.contacts[0].point - collision.contacts[1].point;

        ///Debug.Log(string.Format("- Edge: {0}", edge.ToString()));
        if (!this.isFlying && !this.isOnStack)
        {
            body = GetComponent<Rigidbody>();

            if (collision.gameObject.name.Equals("Ground"))
            {
                isOnGround = true;
                if (onGroundListener != null)
                {
                    onGroundListener.Invoke(this);
                }
            }
            else if (collision.gameObject.tag.Equals("Block"))
            {
                Block targetBlock = collision.gameObject.GetComponent<Block>();
                if (onBlockListener != null)
                {
                    onBlockListener.Invoke(this, targetBlock, collision);
                }
                foreach (ContactPoint point in collision.contacts)
                {
                    Debug.Log(string.Format("- Point: {0}", point.point.ToString()));
                }
                lastCollision = collision;
            }

        }
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(string.Format("OnCollisionEnter: {0}({4}{5}) on {1}, {2} contacts, velocity={3}, position={6}" 
            , this.name
            , collision.gameObject.name
            , collision.contacts.Length
            , collision.relativeVelocity
            , this.isOnStack?"OnStack":""
            , this.isFlying ? "Flying" : ""
            , collision.gameObject.transform.position
        ));
        ProcessCollision(collision);
    }
    void OnCollisionExit(Collision collision)
    {
        Debug.Log(string.Format("OnCollisionEnter: {0}({4}{5}) on {1}, {2} contacts, velocity={3}, position={6}"
            , this.name
            , collision.gameObject.name
            , collision.contacts.Length
            , collision.relativeVelocity
            , this.isOnStack ? "OnStack" : ""
            , this.isFlying ? "Flying" : ""
            , collision.gameObject.transform.position
        ));
        ProcessCollision(collision);
    }
    public BlockEvent OnGroundListener
    {
        get { return this.onGroundListener; }
    }
    public BlockOnBlockEvent OnBlockListener
    {
        get { return this.onBlockListener; }
    }
    public string BlockName
    {
        get { return this.blockName; }
        set { this.blockName = value; }
    }
    
    public bool IsOnStack
    {
        get { return this.isOnStack; }
        set { this.isOnStack = value; }
    }
    public bool IsFlying
    {
        get { return this.isFlying; }
        set { this.isFlying = value; }
    }

    public bool IsOnGround
    {
        get { return this.IsOnGround; }
        set { this.IsOnGround = value; }
    }

}
