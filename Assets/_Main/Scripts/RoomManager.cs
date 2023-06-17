
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Assertions;

namespace Chessticle
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        void Awake()
        {
            Assert.AreEqual(1, FindObjectsOfType<RoomManager>().Length);
        }

        public void JoinOrCreateRoom(string preferredRoomName)
        {
            StopAllCoroutines();
            const float timeoutSeconds = 15f; 
            StartCoroutine(DoCheckTimeout(timeoutSeconds));
            StartCoroutine(DoJoinOrCreateRoom(preferredRoomName));
        }

        IEnumerator DoCheckTimeout(float timeout)
        {
            DidTimeout = false;
            while (!PhotonNetwork.InRoom && timeout >= 0)
            {
                yield return null;
                timeout -= Time.deltaTime;
            }

            if (timeout <= 0)
            {
                DidTimeout = true;
                StopAllCoroutines();
            }
        }

        public bool DidTimeout { private set; get; }

        static IEnumerator DoJoinOrCreateRoom(string preferredRoomName)
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }
            
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
            }

            while (!PhotonNetwork.IsConnectedAndReady)
            {
                yield return null;
            }

            if (!PhotonNetwork.InLobby && PhotonNetwork.NetworkClientState != ClientState.JoiningLobby)
            {
                PhotonNetwork.JoinLobby();
            }

            while (PhotonNetwork.NetworkClientState != ClientState.JoinedLobby)
            {
                yield return null;
            }

            if (preferredRoomName != null)
            {
                PhotonNetwork.JoinOrCreateRoom(preferredRoomName, s_RoomOptions, TypedLobby.Default);
            }
            else
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }

        static readonly RoomOptions s_RoomOptions = new RoomOptions
        {
            MaxPlayers = 2,
            // don't destroy an empty room immediately - it can be rejoined (see GameManager::Start())
            EmptyRoomTtl = 5 * 1000 
        };

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            PhotonNetwork.CreateRoom(null, s_RoomOptions, TypedLobby.Default);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            PhotonNetwork.CreateRoom(null, s_RoomOptions, TypedLobby.Default);
        }
    }
}
