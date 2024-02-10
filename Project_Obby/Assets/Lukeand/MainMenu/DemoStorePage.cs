
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;

public class DemoStorePage : MonoBehaviour, IStoreListener
{
    //this is all for building the store automaticly.
    //i dont need this. this is for bigger projects.



    IStoreController storeController;
    IExtensionProvider extensionProvider;


    private async void Awake()
    {
        InitializationOptions options = new InitializationOptions()

#if UNITY_EDITOR 
            .SetEnvironmentName("test");
#else
            .SetEnvironmentName("production");  
#endif
        await UnityServices.InitializeAsync(options);
        ResourceRequest operation = Resources.LoadAsync<TextAsset>("IAPProductCattalog");
        operation.completed += HandleIAPProductCatalocLoaded;

    }


    void HandleIAPProductCatalocLoaded(AsyncOperation operation)
    {
        ResourceRequest request = operation as ResourceRequest;
        Debug.Log($"Loaded Asset: { request.asset}");
        ProductCatalog catalog = JsonUtility.FromJson<ProductCatalog>((request.asset as TextAsset).text);
        Debug.Log($"Loaded catalog with {catalog.allProducts.Count} items");

#if UNITY_ANDROID
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(
            StandardPurchasingModule.Instance(AppStore.GooglePlay));


#elif UNITY_IOS
           ConfigurationBuilder builder = ConfigurationBuilder.Instance(
            StandardPurchasingModule.Instance(AppStore.AppleAppStore));
#else
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(
            StandardPurchasingModule.Instance(AppStore.NotSpecified));

#endif



        foreach (ProductCatalogItem item in catalog.allProducts)
        {
            builder.AddProduct(item.id, item.type);
        }

        UnityPurchasing.Initialize(this, builder);      

    }


    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;   
        extensionProvider = extensions;

    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        return PurchaseProcessingResult.Complete;
    }
}
