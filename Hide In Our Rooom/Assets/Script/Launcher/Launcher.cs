﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using Photon.Realtime;

namespace GuerhoubaGame
{


    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        [Tooltip("The UI Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;

        [Tooltip("The UI Label to inform the user taht the connection is in progress")]
        [SerializeField]
        private GameObject progressPanel;

        [Header("Wait Room")]
        [SerializeField]
        private GameObject waitRoom;

        [SerializeField]
        private GameObject Wr_Player_One;

        [SerializeField]
        private GameObject Wr_Player_Two;


        #endregion

        #region Private Fields

        private string gameVersion = "1";

        private bool isConnecting;


        [SerializeField]
        private bool launch;

        #endregion




        #region MonoBehavior CallBack

        private void Awake()
        {
            // Changera le level automatiquement pour tous les clients si le master change
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            progressPanel.SetActive(false);
            controlPanel.SetActive(true);
            waitRoom.SetActive(false);
        }
        

        #endregion

        #region MonoBehaviourPunCallback Callbacks
        //Check si on arriver à se connecter au Master Cloud
        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            if (isConnecting)
            {

                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }
        }

        // Indique la raison de l'incapacité à se connecter
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
            progressPanel.SetActive(false);
            controlPanel.SetActive(true);
            isConnecting = false;
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");

            if (PhotonNetwork.CurrentRoom.PlayerCount > 0)
            {
                progressPanel.SetActive(false);
                controlPanel.SetActive(false);
                waitRoom.SetActive(true);
                Debug.Log("We load the 'Room for 1'");

            }
        }



        #endregion


        #region Public Methods

        public void Connect()
        {

            progressPanel.SetActive(true);
            controlPanel.SetActive(false);
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }

        }

        public void LauchGame()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.LoadLevel("GameScene");
                }
            }
        }

      

        #endregion
    }
}
