using UnityEngine.Advertisements;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdHandler : MonoBehaviour
{
    //
    BannerView bannerView;
    InterstitialAd interstitial;
    RewardedInterstitialAd rewardedAd;

    string adUnitId = "";


    private void Start()
    {
        MobileAds.Initialize(initStatus => { });

        adUnitId = "ca-app-pub-3940256099942544/9257395921";


        //RequestBanner();
        LoadInterstitialAd();
        LoadRewardAd();
    }


    void RequestBanner()
    {      
        //THis is the android version.      
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);

        AdRequest request = new AdRequest.Builder().Build();

        bannerView.LoadAd(request);

        Debug.Log("this was called");
    }

    #region INTERSTITIAL AD
    //interstitial ads are for between levels.
    //i want that before loading the next scene we show then and once its done we close them. everytime we go from one scene to another.
    void LoadInterstitialAd()
    {
        if (interstitial != null)
        {
            interstitial.Destroy();
            interstitial = null;
        }

        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                interstitial = ad;
            });
    }

    [ContextMenu("DEBUG SHOW INTE_AD")]
    public void RequestInterstitial()
    {
        if (interstitial != null && interstitial.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            interstitial.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }


    }

    #endregion


    #region REWARD AD

    //these ads works as reward to gain another chance at teh game or for making double the amount of gold.

    void LoadRewardAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the rewarded interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        RewardedInterstitialAd.Load(adUnitId, adRequest,
            (RewardedInterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("rewarded interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAd = ad;
            });
    }

    public void RequestRewardAd()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                //Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
    }

    #endregion




}
