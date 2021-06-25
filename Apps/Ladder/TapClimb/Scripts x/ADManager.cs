using System;
using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;


public class ADManager : MonoBehaviour
{
    public static string App_ID = "ca-app-pub-9727856365139576~7242502872";//
    public static string banner_ID = "ca-app-pub-9727856365139576/1235497726";
    public static string Interstitial_ID = "ca-app-pub-9727856365139576/3806994203";
    public static string Reward_ID = "ca-app-pub-9727856365139576/7980869476";

    static BannerView bannerAd;
    static InterstitialAd InterstitialAd;
    static RewardBasedVideoAd rewardAd;

    [HideInInspector] public enum RewardedType { unlockItem };
    [HideInInspector] public RewardedType rewardedType;
    [HideInInspector] public ItemCard itemToUnlock;

    [HideInInspector] public bool rewardedShows = false;
    [HideInInspector] public bool alreadyRewarded = false;
    int rerequestTime = 1;

    void Start()
    {
        //PlayerPrefs.SetInt("sound", 1);

        //when u publish the game
        MobileAds.Initialize(App_ID);


        RequestBannerAd();
        RequestInterstitialAd();
        RequestRewardedVideoAd();

        Display_Banner();
    }

    void RequestBannerAd()
    {
        if (!(Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork))
        {
            StartCoroutine(RE_RequestBanner());
            return;
        }

        bannerAd = new BannerView(banner_ID, AdSize.SmartBanner, AdPosition.Top);

        //for real ads
        AdRequest adRequest = new AdRequest.Builder().Build();
        //


        //just for testing
        //AdRequest adRequest = new AdRequest.Builder().AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();
        //


        //handl
        // Called when an ad request has successfully loaded.
        bannerAd.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        bannerAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        bannerAd.OnAdOpening += HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        bannerAd.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        bannerAd.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        bannerAd.LoadAd(adRequest);
    }

    static void RequestInterstitialAd()
    {
        if (!(Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork))
        {
            return;
        }
        InterstitialAd = new InterstitialAd(Interstitial_ID);

        //for real
        AdRequest adRequest = new AdRequest.Builder().Build();

        //for testing
        //AdRequest adRequest = new AdRequest.Builder().AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();

        InterstitialAd.LoadAd(adRequest);
    }

    public void RequestRewardedVideoAd()
    {
        if (!(Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork))
        {
            //StartCoroutine(RE_RequestRewarded());
            return;
        }
        //test
        //ca-app-pub-3940256099942544/5224354917


        //string reward_ID = "ca-app-pub-3940256099942544/5224354917";
        rewardAd = RewardBasedVideoAd.Instance;

        //for real
        AdRequest adRequest = new AdRequest.Builder().Build();

        //for testing
        //AdRequest adRequest = new AdRequest.Builder().AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();



        // Called when an ad request has successfully loaded.
        rewardAd.OnAdLoaded += HandleRewardBasedVideoLoaded;
        // Called when an ad request failed to load.
        rewardAd.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        // Called when an ad is shown.
        rewardAd.OnAdOpening += HandleRewardBasedVideoOpened;
        // Called when the ad starts to play.
        rewardAd.OnAdStarted += HandleRewardBasedVideoStarted;
        // Called when the user should be rewarded for watching a video.
        rewardAd.OnAdRewarded += HandleRewardBasedVideoRewarded;
        // Called when the ad is closed.
        rewardAd.OnAdClosed += HandleRewardBasedVideoClosed;
        // Called when the ad click caused the user to leave the application.
        rewardAd.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;


        rewardAd.LoadAd(adRequest, Reward_ID);
    }

    //displaying ads
    public void Display_Banner()
    {
        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            bannerAd.Show();
        }

    }

    public void Display_InterstitialAd()
    {
        if (InterstitialAd.IsLoaded())
        {
            InterstitialAd.Show();
        }
        else
        {
            RequestInterstitialAd();
        }
    }

    public void Display_RewardVideoAd()
    {
        rewardedShows = false;
        if (rewardAd.IsLoaded())
        {
            //alreadyRewarded = false;
            rewardAd.Show();
        }
        else
        {
            RequestRewardedVideoAd();
        }
    }

    //handling ads 
    //banner ad
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleAdLoaded event received");
        Display_Banner();
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "+ args.Message);
        //Display_Banner();
        //RequestBannerAd();
        StartCoroutine(RE_RequestBanner());
    }
    IEnumerator RE_RequestBanner()
    {
        rerequestTime++;
        yield return new WaitForSeconds(2.0f * rerequestTime);
        RequestBannerAd();

    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleAdLeavingApplication event received");
    }




    //reward video

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //MonoBehaviour.print( "HandleRewardBasedVideoFailedToLoad event received with message: "+ args.Message);
        StartCoroutine(RE_RequestRewarded());
        //RequestRewardedVideoAd();
    }
    IEnumerator RE_RequestRewarded()
    {
        rerequestTime++;
        yield return new WaitForSeconds(2.0f * rerequestTime);
        RequestRewardedVideoAd();
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
        //UIManagement_Sc.GameOver();
        StartCoroutine(RE_RequestRewarded());
        //RequestRewardedVideoAd();
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        rewardedShows = true;
        //string type = args.Type;
        //double amount = args.Amount;
        //MonoBehaviour.print( "HandleRewardBasedVideoRewarded event received for " + amount.ToString() + " " + type);

        //*****************************************UIManagement_Sc.ContinuePlaying();
        //FindObjectOfType<UIHandler>().PassTheCard();

        //RequestRewardedVideoAd();

        //GameObject.Find("Canvas").GetComponent<UIOperations>().Add25Diamond();

        if (alreadyRewarded)
        {
            return;
        }
        else
        {
            alreadyRewarded = true;
            StartCoroutine(waitToRewardAgain());
        }

        switch (rewardedType)
        {
            case RewardedType.unlockItem:
                itemToUnlock.Unlock();
                break;
        }
        StartCoroutine(RE_RequestRewarded());
    }

    IEnumerator waitToRewardAgain()
    {
        yield return new WaitForSeconds(2.0f);
        alreadyRewarded = false;
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
    }

    /*private void ShowMediationTestSuite()
    {
        MediationTestSuite.Show();
    }*/
}
