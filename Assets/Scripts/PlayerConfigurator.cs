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
    [SerializeField] private AssetReferenceGameObject m_HatAssetReference;

    void Start()
    {
        SetHat();
    }

    public void SetHat()
    {
        // m_HatLoadOpHandle = Addressables.LoadAssetAsync<GameObject>(m_Address);
        if (!m_HatAssetReference.RuntimeKeyIsValid())
            return;

        m_HatLoadOpHandle = m_HatAssetReference.LoadAssetAsync<GameObject>();
        m_HatLoadOpHandle.Completed += OnHatLoadComplete;
    }
    private void OnDisable()
    {
        m_HatLoadOpHandle.Completed -= OnHatLoadComplete;
    }
    private void OnHatLoadComplete(AsyncOperationHandle<GameObject> asyncOperationHandle)
    {
        if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Instantiate(asyncOperationHandle.Result, m_HatAnchor);
        }
    }
}
