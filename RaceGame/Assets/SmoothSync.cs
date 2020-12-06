using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
[RequireComponent(typeof(PhotonView))]
public class SmoothSync : MonoBehaviourPun,IPunObservable
{
    public float smoothingDelay = 5;
    private Vector3 correctPosition;
    private Quaternion correctRotation;

    // Update is called once per frame
    void Update()
    {
        //自分自身は無視
        if (photonView.IsMine) { return; }

        //座標をなめらかに
        transform.position = Vector3.Lerp(
            transform.position,
            correctPosition,
            Time.deltaTime * smoothingDelay
            );

        //回転をなめらかに
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            correctRotation,
            Time.deltaTime * smoothingDelay
            );
    }

    public void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //相手に自分の座標を送信する
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            //相手の座標を受け取る
            correctPosition = (Vector3)stream.ReceiveNext();
            correctRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
