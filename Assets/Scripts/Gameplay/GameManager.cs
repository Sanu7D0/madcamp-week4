using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;
using Players = System.Collections.Generic.SortedDictionary<int, IPlayer>;
using UnityEngine.UI;

sealed public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject defaultPlayerPrefab;
    [SerializeField] private int _gameGoalScore;
    private LobbyManager lobbyManager;
    private readonly Players players = 
		new Players();

    public UnityEvent<Players> onChangePlayer;

    public int gameGoalScore {
     get { return _gameGoalScore; }
    }
    
    [SerializeField] private int _gameLimitTime;
        public int gameLimitTime {
            get { return _gameLimitTime; }
        }
    public float gameStartTime;
    public float gameElapsedTime {
        get { return Time.time - gameStartTime; }
    }
    
    public List<GameObject> missions;
    public List<Image> gauges;
    public Transform pos;

    public bool isPlaying {
        get;
        private set;
    }

    protected override void Awake() {
	    base.Awake();
        lobbyManager = LobbyManager.Instance;
    }

    void Start() {
        StartGame();
    }

    private void StartGame() {
        isPlaying = true;

        GameObject player;
        if(PhotonNetwork.IsConnected) { 
          player = PhotonNetwork.Instantiate(lobbyManager.selectedCharacterName ?? defaultPlayerPrefab.name, GameObject.Find("Spawn0").transform.position, Quaternion.identity);
        } else { 
          player = Instantiate(defaultPlayerPrefab, GameObject.Find("Spawn0").transform.position, Quaternion.identity);
        }

        AttachMainCamera(player);
        gameStartTime = Time.time;
        StartCoroutine(StartTimer());
        Debug.Log("Game started");

        // TODO Mission object create
        // Mission Prefab에 프리팹넣기
        float RandomX = UnityEngine.Random.Range(-12, 13);
        float RandomY = UnityEngine.Random.Range(-16, 14);
        Vector2 RandomPos = new Vector2(RandomX, RandomY);
        GameObject box = Instantiate(missionPrefab, RandomPos, Quaternion.identity);
        // box.transform.position = new Vector3(0,0,0);
        missions.Add(box);

        Debug.Log("Game started");
    }

    private void EndGame() {
        isPlaying = false;
        Debug.Log("Game ended");
    }

    private void AttachMainCamera(GameObject target) {
        GameObject camera = GameObject.Find("Main Camera");
        camera.GetComponent<CameraController>().targetTransform = target.transform;
    }

    IEnumerator StartTimer() {
        while (gameElapsedTime < gameLimitTime) {
            // Debug.Log("Time elapsed for " + gameElapsedTime);
            yield return new WaitForSeconds(1);
        }
        Debug.Log("Time limit reached!");
        EndGame();
    }

    public void AddPlayer(IPlayer player) {
        players.Add(player.id, player);
        onChangePlayer.Invoke(players);
    }

    public void RemovePlayer(IPlayer player) {
        players.Remove(player.id);
        onChangePlayer.Invoke(players);
    }
    
    public void OnChangePlayerState(IPlayer player) {
        players.Add(player.id, player);
        onChangePlayer.Invoke(players);
    }   
     
}   
