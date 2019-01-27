using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTrimCube : MonoBehaviour
{
    [SerializeField]
    Block blockFall;
    [SerializeField]
    Block blockStatic;

    Rigidbody blockFallBody;
    Rigidbody blockStaticBody;
    // Use this for initialization
    void Start()
    {
        blockFall.Init();
        blockStatic.Init();

        blockFall.IsFlying = false;
        blockFall.IsOnStack = false;

        blockStatic.IsFlying = false;
        blockStatic.IsOnStack = true;

        blockFall.OnBlockListener.AddListener(OnBlockToCollision);



        blockFallBody = blockFall.GetComponent<Rigidbody>();
        blockStaticBody = blockStatic.GetComponent<Rigidbody>();

        Debug.Log(string.Format("Start: {0}, {1}, down={2}", blockFall.name, blockStatic.name, Vector3.down));
        blockFall.StartFlying();
        blockFall.StopFlying();


        blockFall.IsFlying = false;
        blockFallBody.isKinematic = false;
        blockFallBody.velocity = Vector3.down * 20;
        

        Debug.Log(string.Format("StartMove: : {0}", blockFallBody.velocity));


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        blockFallBody.velocity = (Vector3.down) * 100 * Time.fixedDeltaTime;

    }
    public void OnBlockToCollision(Block target, Block pattern, Collision collision)
    {
        target.GetComponent<Block>().IsOnStack = true;
        blockFallBody.isKinematic = true;
        Debug.Log("OnBlockToCollision");
        UtilCube.TrimBlockToCollision(target, pattern, collision);
    }
    
}
