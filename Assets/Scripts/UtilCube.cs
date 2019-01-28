using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilCube {

    public static void TrimBlockToCollision(Block target, Block pattern, Collision collision, BlockPool blockPool=null)
    {
        Debug.Log(string.Format("TrimBlockToCollision: {0}, {1}, contact={2}", target.name, pattern.name, collision.contacts.Length));
        for (int i = 0; i < collision.contacts.Length; i++)
        {
            Debug.Log(string.Format("\t- CollisionContact: {0}", collision.contacts[i].point));
        }

        float maxX = GetMaxWorldX(pattern);
        float minX = GetMinWorldX(pattern);

        float maxZ = GetMaxWorldZ(pattern);
        float minZ = GetMinWorldZ(pattern);
        Debug.Log(string.Format("Pattern limitation: maxX={0}, maxZ={1}", maxX, maxZ));

        TrimMaxX(target, maxX, blockPool);
        TrimMinX(target, minX, blockPool);

        TrimMaxZ(target, maxZ, blockPool);
        TrimMinZ(target, minZ, blockPool);


    }
    public static void TrimMaxZ(Block target, float maxZ, BlockPool blockPool = null)
    {
        if (NeedTrimMaxZ(target, maxZ))
        {
            MeshFilter meshFilterTarget = target.gameObject.GetComponent<MeshFilter>();
            Vector3[] targetVertices = meshFilterTarget.mesh.vertices;

            Block cutOff = GetBlockFromPool(blockPool, target);
            Vector3 pos = target.transform.position;
            cutOff.transform.position = pos;
            cutOff.transform.localScale = target.transform.localScale;

            ///Create vertices for cutoff block
            Vector3[] cutOffVertices = meshFilterTarget.mesh.vertices;
            for (int i = 0; i < targetVertices.Length; i++)
            {
                Vector3 worldPos = target.transform.TransformPoint(targetVertices[i]);
                if (worldPos.z > maxZ)
                {
                    cutOffVertices[i] = cutOff.transform.InverseTransformPoint(worldPos);
                    targetVertices[i] = target.transform.InverseTransformPoint(new Vector3(worldPos.x, worldPos.y, maxZ));
                    Debug.Log("- Trim vertex by maxZ: " + i);
                }
                else
                {
                    cutOffVertices[i] = cutOff.transform.InverseTransformPoint(new Vector3(worldPos.x, worldPos.y, maxZ));
                }
            }

            ///Recalculate target block
            ApplyNewVerticesToMesh(meshFilterTarget, targetVertices);
           
            ///Recalculate cutoff 
            ApplyNewVerticesToMesh(cutOff.GetComponent<MeshFilter>(), cutOffVertices);
            MakeBlockFallStatic(cutOff);
        }
    }
    public static void MakeBlockFallStatic(Block aBlock)
    {
        Rigidbody body = aBlock.GetComponent<Rigidbody>();
        body.GetComponent<BoxCollider>().enabled = false;
        body.mass = 1;
        body.isKinematic = false;
        body.useGravity = true;
    }
    public static void DestroyBlock(Block aBlock) { 
        GameObject.Destroy(aBlock);
    }
    public static Block GetBlockFromPool(BlockPool pool, Block sample)
    {
        if (pool != null)
        {
            Block newBlock = pool.GetDebrisBlock(sample);
            newBlock.OnGroundListener.AddListener(pool.ReturnBlock);
            return newBlock;
        }
        else
        {
            Block newBlock = GameObject.Instantiate(sample);
            newBlock.OnGroundListener.AddListener(DestroyBlock);
            return newBlock;
        }
    }
    public static void TrimMinZ(Block target, float minZ, BlockPool blockPool = null)
    {
        if (NeedTrimMinZ(target, minZ))
        {
            MeshFilter meshFilterTarget = target.gameObject.GetComponent<MeshFilter>();
            Vector3[] targetVertices = meshFilterTarget.mesh.vertices;

            Block cutOff = GetBlockFromPool(blockPool, target);
            Vector3 pos = target.transform.position;
            cutOff.transform.position = pos;
            cutOff.transform.localScale = target.transform.localScale;

            ///Create vertices for cutoff block
            Vector3[] cutOffVertices = meshFilterTarget.mesh.vertices;
            for (int i = 0; i < targetVertices.Length; i++)
            {
                Vector3 worldPos = target.transform.TransformPoint(targetVertices[i]);
                if (worldPos.z < minZ)
                {
                    cutOffVertices[i] = cutOff.transform.InverseTransformPoint(worldPos);
                    targetVertices[i] = target.transform.InverseTransformPoint(new Vector3(worldPos.x, worldPos.y, minZ));
                    Debug.Log("- Trim vertex by minZ: " + i);
                }
                else
                {
                    cutOffVertices[i] = cutOff.transform.InverseTransformPoint(new Vector3(worldPos.x, worldPos.y, minZ));
                }
            }

            ///Recalculate target block
            ApplyNewVerticesToMesh(meshFilterTarget, targetVertices);

            ///Recalculate cutoff 
            ApplyNewVerticesToMesh(cutOff.GetComponent<MeshFilter>(), cutOffVertices);
            MakeBlockFallStatic(cutOff);
        }
    }
    public static void ApplyNewVerticesToMesh(MeshFilter meshFilter, Vector3[] newVertices)
    {
        meshFilter.mesh.vertices = newVertices;
        meshFilter.mesh.RecalculateBounds();
        meshFilter.mesh.RecalculateNormals();
        meshFilter.mesh.RecalculateTangents();

        Bounds bounds = meshFilter.mesh.bounds;
        BoxCollider collider = meshFilter.gameObject.GetComponent<BoxCollider>();
        if (collider != null)
        {
            collider.center = bounds.center;
            collider.size = bounds.size;
        }
        
    }
    public static float GetMaxWorldX(Block aBlock)
    {
        float maxX = float.MinValue ;
        MeshFilter meshFilter = aBlock.gameObject.GetComponent<MeshFilter>();
        Vector3[] vertices = meshFilter.mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPos = aBlock.transform.TransformPoint(vertices[i]);
            if (worldPos.x > maxX)
            {
                maxX = worldPos.x;
            }
        }
        return maxX;
    }

    public static float GetMinWorldX(Block aBlock)
    {
        float minX = float.MaxValue;
        MeshFilter meshFilter = aBlock.gameObject.GetComponent<MeshFilter>();
        Vector3[] vertices = meshFilter.mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPos = aBlock.transform.TransformPoint(vertices[i]);
            if (worldPos.x < minX)
            {
                minX = worldPos.x;
            }
        }
        return minX;
    }

    public static float GetMaxWorldZ(Block aBlock)
    {
        float maxZ = float.MinValue;
        MeshFilter meshFilter = aBlock.gameObject.GetComponent<MeshFilter>();
        Vector3[] vertices = meshFilter.mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPos = aBlock.transform.TransformPoint(vertices[i]);
            if (worldPos.z > maxZ)
            {
                maxZ = worldPos.z;
            }
        }
        return maxZ;
    }
    public static float GetMinWorldZ(Block aBlock)
    {
        float minZ = float.MaxValue;
        MeshFilter meshFilter = aBlock.gameObject.GetComponent<MeshFilter>();
        Vector3[] vertices = meshFilter.mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPos = aBlock.transform.TransformPoint(vertices[i]);
            if (worldPos.z < minZ)
            {
                minZ = worldPos.z;
            }
        }
        return minZ;
    }

    public static bool NeedTrimMaxX(Block aBlock, float maxX)
    {
        MeshFilter meshFilterTarget = aBlock.gameObject.GetComponent<MeshFilter>();
        Vector3[] targetVertices = meshFilterTarget.mesh.vertices;
        bool needTrim = false;
        for (int i = 0; i < targetVertices.Length; i++)
        {
            Vector3 worldPos = aBlock.transform.TransformPoint(targetVertices[i]);
            if (worldPos.x > maxX)
            {
                needTrim = true;
                break;
            }
        }
        return needTrim;
    }

    public static bool NeedTrimMinX(Block aBlock, float minX)
    {
        MeshFilter meshFilterTarget = aBlock.gameObject.GetComponent<MeshFilter>();
        Vector3[] targetVertices = meshFilterTarget.mesh.vertices;
        bool needTrim = false;
        for (int i = 0; i < targetVertices.Length; i++)
        {
            Vector3 worldPos = aBlock.transform.TransformPoint(targetVertices[i]);
            if (worldPos.x < minX)
            {
                needTrim = true;
                break;
            }
        }
        return needTrim;
    }

    public static bool NeedTrimMaxZ(Block aBlock, float maxZ)
    {
        MeshFilter meshFilterTarget = aBlock.gameObject.GetComponent<MeshFilter>();
        Vector3[] targetVertices = meshFilterTarget.mesh.vertices;
        bool needTrim = false;
        for (int i = 0; i < targetVertices.Length; i++)
        {
            Vector3 worldPos = aBlock.transform.TransformPoint(targetVertices[i]);
            if (worldPos.z > maxZ)
            {
                needTrim = true;
                break;
            }
        }
        return needTrim;
    }
    public static bool NeedTrimMinZ(Block aBlock, float minZ)
    {
        MeshFilter meshFilterTarget = aBlock.gameObject.GetComponent<MeshFilter>();
        Vector3[] targetVertices = meshFilterTarget.mesh.vertices;
        bool needTrim = false;
        for (int i = 0; i < targetVertices.Length; i++)
        {
            Vector3 worldPos = aBlock.transform.TransformPoint(targetVertices[i]);
            if (worldPos.z < minZ)
            {
                needTrim = true;
                break;
            }
        }
        return needTrim;
    }

    public static void TrimMaxX(Block target, float maxX, BlockPool blockPool=null)
    {
        if (NeedTrimMaxX(target, maxX))
        {
            MeshFilter meshFilterTarget = target.gameObject.GetComponent<MeshFilter>();
            Vector3[] targetVertices = meshFilterTarget.mesh.vertices;
            Block cutOff = GetBlockFromPool(blockPool, target);
            //cutOff.transform.position = target.transform.position;
            //Vector3[] cutOffVertices = targetVertices;
            ///GameObject cutOff = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Vector3 pos = target.transform.position;
            cutOff.transform.position = pos;
            cutOff.transform.localScale = target.transform.localScale;

            ///Create vertices for cutoff block
            Vector3[] cutOffVertices = meshFilterTarget.mesh.vertices;
            for (int i = 0; i < targetVertices.Length; i++)
            {
                Vector3 worldPos = target.transform.TransformPoint(targetVertices[i]);
                if (worldPos.x > maxX)
                {
                    cutOffVertices[i] = cutOff.transform.InverseTransformPoint(worldPos);
                    targetVertices[i] = target.transform.InverseTransformPoint(new Vector3(maxX, worldPos.y, worldPos.z));
                    Debug.Log("- Trim vertex by maxX: " + i);
                }
                else
                {
                    cutOffVertices[i] = cutOff.transform.InverseTransformPoint(new Vector3(maxX, worldPos.y, worldPos.z));
                }
            }

            ///Recalculate target block
            ApplyNewVerticesToMesh(meshFilterTarget, targetVertices);

            ///Recalculate cutoff 
            ApplyNewVerticesToMesh(cutOff.GetComponent<MeshFilter>(), cutOffVertices);

            MakeBlockFallStatic(cutOff);
        }
    }
    public static void TrimMinX(Block target, float minX, BlockPool blockPool = null)
    {
        if (NeedTrimMinX(target, minX))
        {
            MeshFilter meshFilterTarget = target.gameObject.GetComponent<MeshFilter>();
            Vector3[] targetVertices = meshFilterTarget.mesh.vertices;
            Block cutOff = GetBlockFromPool(blockPool, target);
            //cutOff.transform.position = target.transform.position;
            //Vector3[] cutOffVertices = targetVertices;
            ///GameObject cutOff = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Vector3 pos = target.transform.position;
            cutOff.transform.position = pos;
            cutOff.transform.localScale = target.transform.localScale;

            ///Create vertices for cutoff block
            Vector3[] cutOffVertices = meshFilterTarget.mesh.vertices;
            for (int i = 0; i < targetVertices.Length; i++)
            {
                Vector3 worldPos = target.transform.TransformPoint(targetVertices[i]);
                if (worldPos.x < minX)
                {
                    cutOffVertices[i] = cutOff.transform.InverseTransformPoint(worldPos);
                    targetVertices[i] = target.transform.InverseTransformPoint(new Vector3(minX, worldPos.y, worldPos.z));
                    Debug.Log("- Trim vertex by maxX: " + i);
                }
                else
                {
                    cutOffVertices[i] = cutOff.transform.InverseTransformPoint(new Vector3(minX, worldPos.y, worldPos.z));
                }
            }

            ///Recalculate target block
            ApplyNewVerticesToMesh(meshFilterTarget, targetVertices);

            ///Recalculate cutoff 
            ApplyNewVerticesToMesh(cutOff.GetComponent<MeshFilter>(), cutOffVertices);

            MakeBlockFallStatic(cutOff);
        }
    }
}
