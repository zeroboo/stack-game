using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilCube {

    public static void TrimBlockToCollision(Block target, Block pattern, Collision collision)
    {
        Debug.Log(string.Format("TrimBlockToCollision: {0}, {1}, contact={2}", target.name, pattern.name, collision.contacts.Length));
        for (int i = 0; i < collision.contacts.Length; i++)
        {
            Debug.Log(string.Format("\t- CollisionContact: {0}", collision.contacts[i].point));
        }

        float maxX = GetMaxWorldX(pattern);
        float maxZ = GetMaxWorldZ(pattern);
        Debug.Log(string.Format("Pattern limitation: maxX={0}, maxZ={1}", maxX, maxZ));
        MeshFilter meshFilterTarget = target.gameObject.GetComponent<MeshFilter>();
        Vector3[] targetVertices = meshFilterTarget.mesh.vertices;
        if (NeedTrimMaxX(target, maxX))
        {
            Block cutOff = GameObject.Instantiate(target);
            //cutOff.transform.position = target.transform.position;
            //Vector3[] cutOffVertices = targetVertices;
            ///GameObject cutOff = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Vector3 pos = target.transform.position;
            
            cutOff.transform.position = pos;
            cutOff.transform.localScale = target.transform.localScale;
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
                else {
                    cutOffVertices[i] = cutOff.transform.InverseTransformPoint(new Vector3(maxX, worldPos.y, worldPos.z));
                }
            }
            meshFilterTarget.mesh.RecalculateBounds();
            meshFilterTarget.mesh.RecalculateNormals();
            meshFilterTarget.mesh.RecalculateTangents();

            Bounds bounds = meshFilterTarget.mesh.bounds;
            BoxCollider collider = target.gameObject.AddComponent<BoxCollider>();
            collider.center = bounds.center;
            collider.size = bounds.size;

            cutOff.GetComponent<MeshFilter>().mesh.vertices = cutOffVertices;
            cutOff.GetComponent<MeshFilter>().mesh.RecalculateBounds();
            cutOff.GetComponent<MeshFilter>().mesh.RecalculateNormals();
            cutOff.GetComponent<MeshFilter>().mesh.RecalculateTangents();

            Rigidbody cutOffBody = cutOff.GetComponent<Rigidbody>();
            cutOffBody.GetComponent<BoxCollider>().enabled = false;
            cutOffBody.mass = 1;
            cutOffBody.isKinematic = false;
            cutOffBody.useGravity = true;
        }

        for (int i = 0; i < targetVertices.Length; i++)
        {
            Vector3 worldPos = target.transform.TransformPoint(targetVertices[i]);
            if (worldPos.z > maxZ)
            {
                targetVertices[i] = target.transform.InverseTransformPoint(new Vector3(worldPos.x, worldPos.y, maxZ));
                Debug.Log("- Trim vertex by maxZ: " + i);
            }

        }
        meshFilterTarget.mesh.vertices = targetVertices;
    }

    public static float GetMaxWorldX(Block aBlock)
    {
        float maxX = 0;
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
    public static float GetMaxWorldZ(Block aBlock)
    {
        float maxZ = 0;
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
                targetVertices[i] = aBlock.transform.InverseTransformPoint(new Vector3(maxX, worldPos.y, worldPos.z));
                Debug.Log("- Trim vertex by maxX: " + i);
                needTrim = true;
                break;
            }
        }
        return needTrim;
    }

}
