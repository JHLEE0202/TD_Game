using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform bulletImpact;
    ParticleSystem bulletEffect;
    AudioSource bulletAudio;
    // Start is called before the first frame update
    void Start()
    {
        bulletEffect = bulletImpact.GetComponent<ParticleSystem>();
        bulletAudio = bulletImpact.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger))
       {
           
           bulletAudio.Stop();
           bulletAudio.Play();
           
           Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);

           RaycastHit hitInfo;

           int playerLayer = 1 << LayerMask.NameToLayer("Player");
           int towerLayer = 1 << LayerMask.NameToLayer("Tower");
           int layerMask = playerLayer | towerLayer;
           if (Physics.Raycast(ray, out hitInfo, 200f, ~layerMask))
           {
            bulletEffect.Stop();
            bulletEffect.Play();
            bulletImpact.position = hitInfo.point;
            bulletImpact.forward = hitInfo.normal;
           }
       }
    }
}
