using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static float timeUntilStopSlowmo = 3f;
    [SerializeField] private LevelGenerator levelGenerator;
    public static GameState gameState;
    public enum GameState
    {
        WaitingForPlayers,
        Playing,
        GameOver
    }

    //list of players
    public static List<GameObject> activePlayers = new List<GameObject>();
    public static List<int> activePlayersScores = new List<int>();

    void Start()
    {
        gameState = GameState.WaitingForPlayers;
    }

    void Update()
    {

        switch (gameState)
        {
            case (GameState.WaitingForPlayers):
                CheckToStartGame();
                break;
            case (GameState.Playing):
                //Playing();
                break;
            case (GameState.GameOver):
                break;
        }
    }

    private void Playing()
    {
        //get amount of players every frame
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(players);
        if (players.Length < 2)
        {
            //this means one or more player died so restart the match
            PhotonNetwork.LoadLevel("Lobby");


        }

    }



    public void CheckToStartGame()
    {
        GameObject[] amountOfPlayers = GameObject.FindGameObjectsWithTag("Player");
        if (amountOfPlayers.Length > 1)
        {
            gameState = GameState.Playing;
        }
    }





}
