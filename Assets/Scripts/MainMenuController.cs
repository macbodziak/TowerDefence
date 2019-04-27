using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class MainMenuController : Singleton<MainMenuController>
{
    public enum MenuState { NewGame = 0, Playing = 1, GameOver = 2 };
    [SerializeField] State currentState;
    State[] states;
    [Range(0.001f, 0.1f)]
    [Tooltip("Determines how fast the Game Over background fades away")]
    [SerializeField]
    float gameOverFadeSpeed = 0.01f;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject PlayButton;
    [SerializeField] GameObject ResumeButton;
    [SerializeField] GameObject RestartButton;
    [SerializeField] GameObject QuitButton;
    [SerializeField] GameObject gameOverBackground;
    [SerializeField] Text gameOverText;
    MenuState menuState = MenuState.NewGame;

    bool gameStarted = false;

    protected override void Awake()
    {
        base.Awake();
    }

protected override void OnAwake()
	{
        states = new State[MenuState.GetNames(typeof(MenuState)).Length];
        states[(int)MenuState.NewGame] = new NewGameState(this, MenuState.NewGame);
        states[(int)MenuState.Playing] = new PlayingState(this, MenuState.Playing);
        states[(int)MenuState.GameOver] = new GameOverState(this, MenuState.GameOver);
        currentState = states[0];
        Assert.IsNotNull(mainMenu);
        Assert.IsNotNull(PlayButton);
        Assert.IsNotNull(ResumeButton);
        Assert.IsNotNull(RestartButton);
        Assert.IsNotNull(QuitButton);
        DontDestroyOnLoad(transform.gameObject);
	}

    void Start()
    {
        //state initialization
        currentState = states[0];
        currentState.OnEnterState();

        UpdateGameController();
        SceneManager.sceneLoaded += ((scene, loadSceneMode) => UpdateGameController());
        PlayButton.GetComponent<Button>().onClick.AddListener(() => SetState(MenuState.Playing));
        RestartButton.GetComponent<Button>().onClick.AddListener(() => SetState(MenuState.Playing));
    }

    void Update()
    {
        currentState.HandleInput();
    }

    public void SetState(MenuState arg)
    {
        menuState = arg;
        currentState.OnExitState();
        currentState = states[(int)arg];
        currentState.OnEnterState();
    }

    public MenuState GetState()
    {
        return menuState;
    }

    public void ToggleMainMenu()
    {
        GameController.Instance.Paused = !GameController.Instance.Paused;
        mainMenu.SetActive(!mainMenu.activeSelf);
    }

    //this should be called every time the scene is loaded, in order to update the new GameController
    public void UpdateGameController()
    {
        RestartButton.GetComponent<Button>().onClick.AddListener(GameController.Instance.RestartLevel);
        QuitButton.GetComponent<Button>().onClick.AddListener(GameController.Instance.QuitGame);
    }

    public class State
    {
        protected MainMenuController mainMenuController;
        protected MenuState thisMenuState;
        protected State(MainMenuController mainMenuController_, MenuState thisMenuState_)
        {
            mainMenuController = mainMenuController_;
            thisMenuState = thisMenuState_;
        }

        public virtual void OnEnterState()
        {
            mainMenuController.menuState = thisMenuState;
        }

        public virtual void OnExitState() { }

        public virtual void HandleInput() { }
    }

    class NewGameState : State
    {
        public NewGameState(MainMenuController mainMenuController_, MenuState thisMenuState_) : base(mainMenuController_, thisMenuState_) { }

        public override void OnEnterState()
        {
            base.OnEnterState();

            mainMenuController.PlayButton.SetActive(true);
            GameController.Instance.Paused = true;
            mainMenuController.mainMenu.SetActive(true);
            mainMenuController.RestartButton.SetActive(false);
            mainMenuController.ResumeButton.SetActive(false);
            mainMenuController.QuitButton.SetActive(true);
        }

        public override void OnExitState()
        {
            mainMenuController.PlayButton.SetActive(false);
        }
    }

    class PlayingState : State
    {
        public PlayingState(MainMenuController mainMenuController_, MenuState thisMenuState_) : base(mainMenuController_, thisMenuState_) { }

        public override void OnEnterState()
        {
            base.OnEnterState();
            mainMenuController.RestartButton.SetActive(true);
            mainMenuController.ResumeButton.SetActive(true);
            mainMenuController.ToggleMainMenu();
        }

        public override void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                mainMenuController.ToggleMainMenu();
            }
        }
    }

    class GameOverState : State
    {
        Coroutine fadeRoutine;
        public GameOverState(MainMenuController mainMenuController_, MenuState thisMenuState_) : base(mainMenuController_, thisMenuState_) { }

        public override void OnEnterState()
        {

            mainMenuController.mainMenu.SetActive(true);
            // GameController.Instance.Paused = true;

            mainMenuController.gameOverBackground.SetActive(true);
            mainMenuController.gameOverText.gameObject.SetActive(true);

            mainMenuController.PlayButton.SetActive(false);
            mainMenuController.RestartButton.SetActive(false);
            mainMenuController.QuitButton.SetActive(false);
             mainMenuController.ResumeButton.SetActive(false);

            RawImage image = mainMenuController.gameOverBackground.GetComponent<RawImage>();
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);
            fadeRoutine = mainMenuController.StartCoroutine(GameOverBackgroundFade());
        }

        public override void OnExitState()
        {
            mainMenuController.gameOverBackground.SetActive(false);
            mainMenuController.gameOverText.gameObject.SetActive(false);
            mainMenuController.StopCoroutine(fadeRoutine);
        }

        void PopUpButtons()
        {
            GameController.Instance.Paused = true;
            mainMenuController.PlayButton.SetActive(false);
            mainMenuController.RestartButton.SetActive(true);
            mainMenuController.QuitButton.SetActive(true);
            mainMenuController.ResumeButton.SetActive(false);
            mainMenuController.gameOverBackground.SetActive(false);
            mainMenuController.gameOverText.gameObject.SetActive(false);
        }

        IEnumerator GameOverBackgroundFade(/*RawImage image*/)
        {
            RawImage image = mainMenuController.gameOverBackground.GetComponent<RawImage>();
            if (image != null)
            {
                float startTime = Time.realtimeSinceStartup;
                while (image.color.a > 0.0f)
                {
                    Debug.Log(Time.realtimeSinceStartup - startTime);
                    image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - (Time.realtimeSinceStartup - startTime) * mainMenuController.gameOverFadeSpeed);
                    yield return null;
                }
            }

			PopUpButtons();
        }
    }

}
