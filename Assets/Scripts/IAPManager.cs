using System;
using UnityEngine;
using UnityEngine.Purchasing;


public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager instance;

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;
    private static CardSetManager csManager;


    //Step 1 create your products




    //************************** Adjust these methods **************************************
    public void InitializePurchasing()
    {
        if (IsInitialized()) { return; }
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //Step 2 choose if your product is a consumable or non consumable
        builder.AddProduct("com.errgamesltd.errgame.cs_Family00_EN", ProductType.NonConsumable);
        builder.AddProduct("com.errgamesltd.errgame.cs_Family01_EN", ProductType.NonConsumable);
        builder.AddProduct("com.errgamesltd.errgame.cs_Family02_EN", ProductType.NonConsumable);
        builder.AddProduct("com.errgamesltd.errgame.cs_Family00_FR", ProductType.NonConsumable);


        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    //Step 3 Create methods
    public void BuyProduct (string productID)
    {
        BuyProductID(productID);
    }



    //Step 4 modify purchasing
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        string productID = args.purchasedProduct.definition.id;
        //productID = productID.Replace("cs_", "");
        CardSet csPurchased = csManager.csAll.Find(x => x.cardSetProductID == productID);

        if (csPurchased != null)
            csPurchased.purchased = true;
        else
            Debug.Log("Purchase Failed: " + productID);

        return PurchaseProcessingResult.Complete;

        /*        switch (productID)
                {
                    case "cs_Family00_EN":
                        csPurchased = csManager.csAll.Find(x => x.name == "Family00_EN");
                        break;

                    case "cs_Family01_EN":
                        csPurchased = csManager.csAll.Find(x => x.name == "Family01_EN");
                        break;

                    case "cs_Family02_EN":
                        csPurchased = csManager.csAll.Find(x => x.name == "Family02_EN");
                        break;

                    case "cs_Family00_FR":
                        csPurchased = csManager.csAll.Find(x => x.name == "Family00_FR");
                        break;

                    default:
                        Debug.Log("Purchase failed: " + productID);
                        break;
                }
        */

/*        if (String.Equals(args.purchasedProduct.definition.id, "", StringComparison.Ordinal))
        {
            Debug.Log("");
        }
        else
        {
            Debug.Log("Purchase Failed");
        }
        return PurchaseProcessingResult.Complete;
*/
    }










    //**************************** Dont worry about these methods ***********************************
    private void Awake()
    {
        TestSingleton();
    }

    void Start()
    {
        if (m_StoreController == null) { InitializePurchasing(); }
        csManager = CardSetManager.instance;
    }

    private void TestSingleton()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}