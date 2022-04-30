using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    Transform[] childObjects;
    public List<Transform> childNodeList = new List<Transform>();
    public LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();

        //FillNodes();
    }

    private void OnDrawGizmos()
    {
        if(this.gameObject.name == "Common Route")
        {
            //LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
            //Gizmos.color = Color.green;

            FillNodes();
            lineRenderer.positionCount = childNodeList.Count + 1;

            for (int i = 0; i < childNodeList.Count; i++)
            {
                Vector3 currentPos = childNodeList[i].position;
                if(i>0)
                {
                    //Vector3 prevPos = childNodeList[i - 1].position;
                    //Gizmos.DrawLine(prevPos, currentPos);

                    lineRenderer.SetPosition(i,childNodeList[i].position);
                }
            }
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, childNodeList[0].position);
        }
    }

    void FillNodes()
    {
        childNodeList.Clear();

        childObjects = GetComponentsInChildren<Transform>();

        foreach(Transform child in childObjects)
        {
            if(child != this.transform && child.tag != "Outline")
            {
                childNodeList.Add(child);
            }
        }
    }

    public int GetPosition(Transform nodeTransform)
    {
        return childNodeList.IndexOf(nodeTransform);
    }
}
