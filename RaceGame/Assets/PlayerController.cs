using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviour
{
    private PhotonView photonView;
    private bool goal;
    public float moveSpeed = 0;
    public float rotateSpeed = 100;
    private bool fF;
    //public GameObject shellObject;
    //public Transform fireTransform;
    // Start is called before the first frame update
    void Start()
    {
        goal = false;
        fF = false;
        photonView = PhotonView.Get(this);
        if (photonView.IsMine)
        {
            GameObject camera = (GameObject)Resources.Load("CameraPrefab");
            Vector3 camepos = new Vector3(transform.position.x, 3, -10);
            GameObject came = Instantiate(camera, camepos, Quaternion.identity);
            came.transform.parent = gameObject.transform;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (moveSpeed >= 0)
        {
            moveSpeed = moveSpeed - 0.25f;
            transform.Translate(
                Vector3.forward*
                 moveSpeed * Time.deltaTime);
        }
        if (fF&&PhotonNetwork.CurrentRoom.PlayerCount ==1&&!goal)
        {
            PhotonNetwork.LeaveRoom();
        }
        if (photonView.IsMine)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                Move();
            }
        }
        //リタイア機能
        if (Input.GetKey(KeyCode.Escape))
        {
            fF = true;
            PhotonNetwork.LeaveRoom();
        }
        if (goal&&fF)
        {
            PhotonNetwork.LeaveRoom();
        }
        //if (Input.GetKeyDown(KeyCode.Space)) { Shot(); }
    }

    private void Shot()
    {
        //photonView.RPC("RpcShot", RpcTarget.All, fireTransform.position);
    }

    [PunRPC]
    private void RpcShot(Vector3 position)
    {
        //Instantiate(
        //    shellObject,
        //    position,
        //    transform.rotation
        //    );
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            moveSpeed = moveSpeed + 0.5f;
            transform.Translate(
                Vector3.forward * Input.GetAxis("Vertical1") *
                 moveSpeed * Time.deltaTime);
        }

            transform.Translate(
                Vector3.forward * Input.GetAxis("Vertical1") *
                 moveSpeed * Time.deltaTime);
            
        transform.Rotate(
            Vector3.up * Input.GetAxis("Horizontal1") * rotateSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("GOAL");
        fF = true;
        goal = true;
    }
}
