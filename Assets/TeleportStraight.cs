using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportStraight : MonoBehaviour
{
    public Transform teleportCirclelUI;
    LineRenderer lr;
    // Start is called before the first frame update
    void Start()
    {
        teleportCirclelUI.gameObject.SetActive(false);
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    Vector3 originScale = Vector3.one * 0.02f;
    void Update()
    {
        if (ARAVRInput.GetDown(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch))
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
            Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);
            RaycastHit hitInfo;
            int layer = 1 << LayerMask.NameToLayer("Terrain");
            if (Physics.Raycast(ray, out hitInfo, 200, layer))
            {
                lr.SetPosition(0, ray.origin);
                lr.SetPosition(1, hitInfo.point);
                teleportCirclelUI.gameObject.SetActive(true);
                teleportCirclelUI.position = hitInfo.point;
                teleportCirclelUI.forward = hitInfo.normal;
                teleportCirclelUI.localScale = originScale * Mathf.Max(1,hitInfo.distance);
            }
            else
            {
                lr.SetPosition(0, ray.origin);
                lr.SetPosition(1, ray.origin + ARAVRInput.RHandDirection * 200);
                teleportCirclelUI.gameObject.SetActive(false);
            }
        }
    }
}
