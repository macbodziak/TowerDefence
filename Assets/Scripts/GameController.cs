using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class GameController : Singleton<GameController> {

	static private GameController _instance;
	//those should not be public, but have an accesor propoerty
	List<Destructable> _playerUnits;
	List<Destructable> _enemyUnits;
	[SerializeField] float gameOverDelay = 3.0f;
	[SerializeField] List<Transform> spawnPoints;
	// [SerializeField] GameObject [] enemyPrefabs = new GameObject [1];
	[SerializeField] public Canvas canvas;
	[SerializeField] public BuildTowerMenu _buildTowerMenu;
	[SerializeField] public ManageTowerMenu _manageTowerMenu;
	[SerializeField] int money;
	[SerializeField] int incomeValue;
	[SerializeField] float incomeDeltaTime;
	[SerializeField] Text moneyText;

	AudioSource menuAudioSource;
	bool paused;


	protected override void Awake()
	{
		base.Awake();
	}

	protected override void OnAwake()
	{
		menuAudioSource = GetComponent<AudioSource>();
		Assert.IsNotNull(menuAudioSource);
		Assert.IsNotNull(_buildTowerMenu);
		Assert.IsNotNull(_manageTowerMenu);
		Assert.IsNotNull(moneyText);
		Assert.IsTrue(incomeDeltaTime > 0);
		Assert.IsNotNull(canvas);

		moneyText.text = "Credits: " + money;
		//Reset the instance to null when reloading level
		SceneManager.sceneUnloaded += scene => Instance = null;
 

		_playerUnits = new List<Destructable>();
		_enemyUnits = new List<Destructable>();
	}

	void Start()
	{
		StartCoroutine(IncrementMoneyRoutine());

		//
		// SceneManager.sceneUnloaded += scene => Time.timeScale = 0.0f;
	}

	void Update()
	{
		//Check if player has clicked on any of the tower platforms
		if(Input.GetMouseButtonDown(0) && Paused == false)
		{
			RaycastHit hit;
            // 1 << 8  ->  raycast only against layer 8, which is towerPlatform
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, 1 << 8)) {
				TowerPlatform tp = hit.collider.gameObject.GetComponent<TowerPlatform>();
				tp.ShowMenu();
			}
		}	
	}

	public AudioSource MenuAudioSource 
	{
		get {
			return menuAudioSource;
		}
	} 
	
	public List<Destructable> playerUnits
	{
		get
		{
			return _playerUnits;
		}
	}

	public List<Destructable> enemyUnits
	{
		get
		{
			return _enemyUnits;
		}
	}

	public ManageTowerMenu manageTowerMenu
	{
		get
		{
			return _manageTowerMenu;
		}
	}

	public BuildTowerMenu buildTowerMenu
	{
		get
		{
			return _buildTowerMenu;
		}
	}

	public int Money
	{
		get {
			return money;
		}

		set {
			money = value;
			moneyText.text = "Credits: " + money.ToString();
		}
	}

	public bool Paused
	{
		get{ 
			return paused;
		}
		set{
			paused = value;
			if(paused)
			{
				Time.timeScale = 0.0f;
			}
			else
			{
				Time.timeScale = 1.0f;
			}
		}
	}

	IEnumerator IncrementMoneyRoutine()
	{
		while(true)
		{
			if(!paused)
				Money += incomeValue;
			yield return new WaitForSeconds(incomeDeltaTime);
		}
		yield return null;
	} 

	void TogglePause()
	{
		Paused = !Paused;
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void RestartLevel()
	{
		// Instance = null;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void GameOver()
	{
		Debug.Log("GAME OVER");
		MainMenuController.Instance.SetState(MainMenuController.MenuState.GameOver);
	}

	IEnumerator OnGameOverCoroutine()
	{
		yield return new WaitForSeconds(gameOverDelay);
		OnGameOver();
	}

	void OnGameOver()
	{
		//maybe play some sound...
		//show message
		// popup menu to replay or quit
	}
}
