using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]

public class goal : MonoBehaviour
{
    void Start()
    {
    }
    [PunRPC]
    void OnCollsionEnterRPC(Collision collision)
    {
        Destroy(gameObject);
    }
}
