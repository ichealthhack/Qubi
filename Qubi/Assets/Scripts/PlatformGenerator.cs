using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public float LevelLength = 100f;
    public float PlatformWidth = 2f;
    public float GapWidth = 2f;
    public float HorizontalPosition = 0f;
    public float PerlinFrequency = 1f;
    public float MaxHeight;
    public float MinHeight;
    public int CoinsPerPlatform = 3;

    public GameObject PlatformPrefab;
    public GameObject CoinPrefab;

    // Use this for initialization
    void Start()
    {
        CreatePlatforms();
    }

    // Update is called once per frame
    void Update()
    {
        //DestoryPlatforms();

        //CreatePlatforms();
    }

    void CreatePlatforms()
    {
        while (HorizontalPosition < LevelLength)
        {
            GameObject newPlatform = Instantiate(PlatformPrefab);
            float perlinAmplitude = Mathf.PerlinNoise(0f, HorizontalPosition * PerlinFrequency);

            float platformHeight = perlinAmplitude * (MaxHeight - MinHeight) + MinHeight;

            newPlatform.transform.position = new Vector3(HorizontalPosition, platformHeight, 0f);
            newPlatform.transform.localScale = new Vector3(PlatformWidth, 1f, 1f);
            newPlatform.transform.parent = this.transform;

            for (int i = 0; i < CoinsPerPlatform; i++)
            {
                GameObject newCoin = Instantiate(CoinPrefab);
                float coinOffset = (i + 0.5f) * (PlatformWidth / CoinsPerPlatform);
                newCoin.transform.position = newPlatform.transform.position + Vector3.right * coinOffset + Vector3.up * .5f;
            }


            HorizontalPosition += PlatformWidth + GapWidth;
        }
    }

    void DestoryPlatforms()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
