using UnityEngine;
using System.Collections;
using UnityEngine.Cloud.Analytics;

public class UnityAnalyticsIntegration : MonoBehaviour {

    // Use this for initialization
    void Start () {

        const string projectId = "7b6545ed-e872-4ff4-ba7f-7d96c62b3371";
        UnityAnalytics.StartSDK (projectId);

    }

}