using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class TankArmor : MonoBehaviour, IPunObservable
{
    [SerializeField]
    private int defaultAmor = 100000000;
    [SerializeField]
    private int damage = 10000;

    private PUNManager punManager;
    private PhotonView photonView;
    private int armor;
    public int Armor { get => armor; }
    // Start is called before the first frame update
    void Start()
    {
        punManager = FindObjectOfType<PUNManager>();
        photonView = GetComponent<PhotonView>();
        armor = defaultAmor;
        UpdateArmorUI();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Shell") { return; }
        armor -= damage;
    }

    public void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(armor);
        }
        else
        {
            armor = (int)stream.ReceiveNext();
        }
        UpdateArmorUI();
        if (armor <= 0) { BreakDown(); }
    }

    private void UpdateArmorUI()
    {
        if (photonView.IsMine)
        {
            punManager.SetMyAmorText(armor.ToString());
        }
        else
        {
            punManager.SetEnemyArmorText(armor.ToString());
        }
    }

    private void BreakDown()
    {
        PhotonNetwork.LeaveRoom();
    }
}
