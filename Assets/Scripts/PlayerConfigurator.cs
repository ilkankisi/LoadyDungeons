using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// Used for the Hat selection logic
public class PlayerConfigurator : MonoBehaviour
{
    [SerializeField]
    private Transform m_HatAnchor;

    // m_HatLoadOpHandle get a generic structure from addresable, 
    private AsyncOperationHandle<GameObject> m_HatLoadOpHandle;

    [SerializeField] private string m_Address;
    // [SerializeField] private AssetReference m_HatAssetReference;
    // [SerializeField] private AssetReferenceGameObject m_HatAssetReference;
    private GameObject m_HatInstance;

    void Start()
    {
        // SetHat();
        LoadInRandomHat();
    }
    private void LoadInRandomHat()
    {
        int randomIndex = Random.Range(0, 6);
        string hatAddress = string.Format("Hat{0:00}", randomIndex);

        m_HatLoadOpHandle = Addressables.LoadAssetAsync<GameObject>(hatAddress);
        m_HatLoadOpHandle.Completed += OnHatLoadComplete;
    }
    public void SetHat()
    {
        // m_HatLoadOpHandle = Addressables.LoadAssetAsync<GameObject>(m_Address);
        // if (!m_HatAssetReference.RuntimeKeyIsValid())
        //     return;

        // m_HatLoadOpHandle = m_HatAssetReference.LoadAssetAsync<GameObject>();
        // m_HatLoadOpHandle.Completed += OnHatLoadComplete;
    }
    private void OnDisable()
    {
        m_HatLoadOpHandle.Completed -= OnHatLoadComplete;
    }
    private void OnHatLoadComplete(AsyncOperationHandle<GameObject> asyncOperationHandle)
    {
        if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            // Instantiate(asyncOperationHandle.Result, m_HatAnchor);
            m_HatInstance = Instantiate(asyncOperationHandle.Result, m_HatAnchor);
        }
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            Destroy(m_HatInstance);
            Addressables.ReleaseInstance(m_HatLoadOpHandle);

            LoadInRandomHat();
        }
    }
}
