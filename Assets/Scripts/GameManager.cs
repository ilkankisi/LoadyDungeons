using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static int s_CurrentLevel = 0;

    public static int s_MaxAvailableLevel = 5;

    // The value of -1 means no hats have been purchased
    public static int s_ActiveHat = 0;

    [SerializeField] private Image m_gameLogoImage;
    [SerializeField] private AssetReferenceSprite m_LogoAssetReference;

    // [SerializeField] private string m_LogoAddress;

    private AsyncOperationHandle<Sprite> m_LogoLoadOpHandle;

    private static AsyncOperationHandle<SceneInstance> m_SceneLoadOpHandle;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnEnable()
    {
        // When we go to the 
        s_CurrentLevel = 0;

        // m_LogoLoadOpHandle = Addressables.LoadAssetAsync<Sprite>(m_LogoAddress);
        if (!m_LogoAssetReference.RuntimeKeyIsValid())
            return;
        m_LogoLoadOpHandle = Addressables.LoadAssetAsync<Sprite>(m_LogoAssetReference);
        m_LogoLoadOpHandle.Completed += OnLogoLoadComplete;
        // var logoResourceRequest = Resources.LoadAsync<Sprite>("LoadyDungeonsLogo");
        // logoResourceRequest.completed += (asyncOperation) =>
        // {
        //     m_gameLogoImage.sprite = logoResourceRequest.asset as Sprite;
        // };
    }
    private void OnLogoLoadComplete(AsyncOperationHandle<Sprite> asyncOperationHandle)
    {
        if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
            m_gameLogoImage.sprite = asyncOperationHandle.Result;
    }

    public void ExitGame()
    {
        s_CurrentLevel = 0;
    }

    public void SetCurrentLevel(int level)
    {
        s_CurrentLevel = level;
    }

    public static void LoadNextLevel()
    {
        // SceneManager.LoadSceneAsync("LoadingScene");
        m_SceneLoadOpHandle = Addressables.LoadSceneAsync("LoadingScene", activateOnLoad: true);

    }

    public static void LevelCompleted()
    {
        s_CurrentLevel++;

        // Just to make sure we don't try to go beyond the allowed number of levels.
        s_CurrentLevel = s_CurrentLevel % s_MaxAvailableLevel;

        LoadNextLevel();
    }
    private void OnDisable()
    {
        m_LogoLoadOpHandle.Completed -= OnLogoLoadComplete;
    }

    public static void ExitGameplay()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
