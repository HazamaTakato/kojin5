using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
[RequireComponent(typeof(PhotonView))]
public class GoalManager : MonoBehaviour,IPunObservable
{
    [SerializeField]
    private int defaultInt = 1;
    [SerializeField]
    private int downInt = 1;
    private PUNManager PUNManager;
    private PhotonView photonView;
    private int a;
    private int am;
    public bool endF;
    // Start is called before the first frame update
    void Start()
    {
        PUNManager = FindObjectOfType<PUNManager>();
        photonView = GetComponent<PhotonView>();
        endF = false;
        am = defaultInt;
        UpdateUI();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Goal")
        { return; }
        am -= downInt;   
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(am);
        }
        else
        {
            am = (int)stream.ReceiveNext();
        }
        UpdateUI();
        if (am <= 0) { BreakDown(); }
    }

    // Update is called once per frame
    void UpdateUI()
    {
        if (photonView.IsMine)
        {
            PUNManager.SetMyAmorText(am.ToString());
        }
        else
        {
            PUNManager.SetEnemyArmorText(am.ToString());
        }
    }

    private void BreakDown()
    {
        PhotonNetwork.LeaveRoom();
    }
}
