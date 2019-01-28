using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Block : MonoBehaviour {
    bool isOnStack = false;
    bool isOnGround = false;

    bool isFlying = false;
    bool isFalling = false;
    BlockEvent onGroundListener;
    [SerializeField]
    BlockOnBlockEvent onBlockListener;
    Rigidbody body;
    string blockName;

    Collision lastCollision;
    int type = BLOCK_TYPE_PLAY;
    
    public const int BLOCK_TYPE_PLAY = 1;
    public const int BLOCK_TYPE_DEBRIS = 2;
    float currentSpeed = 10f;
    
    public void Init()
    {
        isOnStack = false;
        isOnGround = false;
        isFlying = false;
        isFalling = false;
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
        this.isFalling = false;
        this.body.isKinematic = false;
    }
    public void StopFlying()
    {
        this.isFlying = false;
        
        body.velocity = Vector3.zero;
        
        Debug.Log(string.Format("BlockStopFlying: {0}", body.velocity));
    }
    public void StartFalling()
    {
        this.isFalling = true;
        
        Vector3 newVelocity = Vector3.down * 1;
        body.velocity = newVelocity;
        body.angularVelocity = Vector3.zero;
        body.angularDrag = 0;
        
        Debug.Log(string.Format("StartFalling: {0}", body.velocity));
    }
    public void SetOnStack()
    {
        this.isOnStack = true;
        body.isKinematic = true;
    }
    void ProcessCollision(Collision collision)
    {
        //Vector3 edge = collision.contacts[0].point - collision.contacts[1].point;

        Debug.Log(string.Format("Block.ProcessCollision: {0} ({1}|{2}) on {3}({4})", name
            , isFlying?"Flying":"", IsOnStack?"OnStack":""
            , collision.gameObject.name, collision.gameObject.tag
            ));
        if (!this.isFlying && !this.isOnStack)
        {
            body = GetComponent<Rigidbody>();
            body.isKinematic = true;
            if (collision.gameObject.tag.Equals("Ground"))
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
                else {
                    Debug.Log(string.Format("- NoListener!"));
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
    public bool IsFalling
    {
        get { return this.isFalling; }
        set { this.isFalling = value; }
    }

    public bool IsOnGround
    {
        get { return this.IsOnGround; }
        set { this.IsOnGround = value; }
    }

    public int GetBlockType
    {
        get { return this.type; }
    }
    public void OnDrawGizmos()
    {
        MeshFilter mesh = GetComponent<MeshFilter>();

        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.TransformPoint(mesh.sharedMesh.vertices[0]), new Vector3(0.1f, 0.1f, 0.1f));
        Gizmos.DrawCube(transform.TransformPoint(mesh.sharedMesh.vertices[1]), new Vector3(0.1f, 0.1f, 0.1f));
        Gizmos.DrawCube(transform.TransformPoint(mesh.sharedMesh.vertices[2]), new Vector3(0.1f, 0.1f, 0.1f));
        Gizmos.DrawCube(transform.TransformPoint(mesh.sharedMesh.vertices[3]), new Vector3(0.1f, 0.1f, 0.1f));
        Gizmos.DrawCube(transform.TransformPoint(mesh.sharedMesh.vertices[4]), new Vector3(0.1f, 0.1f, 0.1f));
        Gizmos.DrawCube(transform.TransformPoint(mesh.sharedMesh.vertices[5]), new Vector3(0.1f, 0.1f, 0.1f));
        Gizmos.DrawCube(transform.TransformPoint(mesh.sharedMesh.vertices[6]), new Vector3(0.1f, 0.1f, 0.1f));
        Gizmos.DrawCube(transform.TransformPoint(mesh.sharedMesh.vertices[7]), new Vector3(0.1f, 0.1f, 0.1f));
       
    }

    public void SetActive()
    {
        this.gameObject.active = true;
        this.enabled = true;
    }

    public void SetInactive()
    {
        this.gameObject.active = false;
    }

    public bool IsActive()
    {
        return this.gameObject.active;
    }

    public void SetBlockTypePlay()
    {
        this.type = BLOCK_TYPE_PLAY;
    }
    public void SetBlockTypeDebris()
    {
        this.type = BLOCK_TYPE_PLAY;
    }

    public string ToString()
    {
        return string.Format("{0}{1}{2}{3}, pos={4}, velocity={5}"
            , isFlying? "flying":""
            , isFalling? "falling":""
            , isOnGround? "onground":""
            , isOnStack? "onstack":""
            , transform.position
            , transform.GetComponent<Rigidbody>().velocity
            );
    }
}
