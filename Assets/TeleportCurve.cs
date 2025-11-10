using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCurve : MonoBehaviour
{
    public Transform teleportCirclelUI;

    LineRenderer lr;

    Vector3 originScale = Vector3.one * 0.02f;

    public int linesmooth = 40;

    public float curveLength = 50;

    public float gravity = -60;

    public float simulationTime = 0.02f;

    List<Vector3> lines = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        teleportCirclelUI.gameObject.SetActive(false);
        lr = GetComponent<LineRenderer>();
        lr.startwidth = 0.0f;
        lr.endWidth = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (ARAVRInput.GetDown(ARAVRInput.GetDown.HandTrigger, ARAVRInput.Controller.RTouch))
        {
            lr.enabled = true;
        }
        else if (ARAVRInput.GetUp(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch))
        {
            lr.enabled = false;
            if (teleportCirclelUI.gameObject.activeSelf)
            {
                GetComponent<CharacterController>(). enabled = false;
                transform.position = teleportCirclelUI.position + Vector3.up;
                GetComponent<CharacterController>(). enabled = true;
            }
            teleportCirclelUI.gameObject.SetActive(false);
        }
        else if (ARAVRInput.Get(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch))
        {
           MkaeLines();
        }
        
    }
    void MkaeLines()
    {
        lines.RemoveRange(0, lines.Count);
        Vector3 dir = ARAVRInput.RHandDirection * curveLength;
        Vector3 pos = ARAVRInput.RHandDirection;
        lines.Add(pos);
    for (int i = 0; i < linesmooth; i++)
    {
        Vector3 lastPos = pos;
        dir.y += gravity * simulationTime;
        pos += dir * simulationTime;
        if (ChekHitRay(lastPos, ref pos))
        {
            lines.Add(pos);
            break;
        }
        else
        {
            teleportCirclelUI.gameObject.SetActive(false);
        }
        lines.Add(pos);
    }
    
    lr.positionCount = lines.Count;
    lr.SetPositions(lines.ToArray());
    }


private bool ChekHitRay(Vector3 lastPos, ref Vector3 pos)
{
    Vector3 rayDir = pos - lastPos;
    Ray ray = new Ray(lastPos, rayDir);
    RaycastHit hitInfo;
    if (Physics.Raycast(ray, out hitinfo, rayDir.magnitude))
    {
        pos = hitInfo.point;
        int layer = LayerMask.NameToLayer("Terrain");
        if (hitInfo.collider.gameObject.layer == layer)
        {
            teleportCirclelUI.gameObject.SetActive(true);
            teleportCirclelUI.position = pos;
            teleportCirclelUI.forward = hitInfo.normal;
            teleportCirclelUI.localScale = originScale * Mathf.Max(1, Vector3.Distance);
        }
        return true;
    }
    return false;
}
}
