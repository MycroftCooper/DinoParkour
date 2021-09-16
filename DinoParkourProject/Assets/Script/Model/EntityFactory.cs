using System.Collections.Generic;
using UnityEngine;

public abstract class CarrierFactory
{
    public float width;
    //基础速度
    public float baseSpeed = 15f;
    //增速步长
    public float addSpeedStep = 0.05f;

    //可放置位置
    public List<Vector3> placeablePosition;
    public List<float> heightPosition;

    //地块预制体
    public GameObject prefab;

    //初始化位置 31.5
    public Vector3 initPosition;
    //销毁位置
    public Vector3 destroyPosition;
    //默认缩放
    public Vector3 defaultScale;

    public abstract CarrierController getCarrier(bool isEmpty);
    public abstract List<CarrierController> getStartCarriers();
    public abstract void putEntity(CarrierController cc);
}

public class LandFactory : CarrierFactory
{
    public LandFactory()
    {
        width = 24f;
        //可放置位置
        placeablePosition = new List<Vector3> { new Vector3(-1f, -0.1f, -2f), new Vector3(1f, -0.1f, -2f) };
        heightPosition = new List<float> { 0, 0.05f, 0.2f };

        //地块预制体
        prefab = Resources.Load<GameObject>("Prefabs/LandPrefab");

        //初始化位置 31.5
        initPosition = new Vector3(100f, 1.3f, 0f);
        //销毁位置
        destroyPosition = new Vector3(-999f, 1.3f, 0f);
        //默认缩放
        defaultScale = new Vector3(16f, 16f, 1f);
    }
    //是仙人掌的概率
    public static float isCactu = 0.6f;
    //是翼龙的概率
    public static float isPterosaur = 1 - isCactu;

    public override CarrierController getCarrier(bool isEmpty)
    {
        GameObject land = Object.Instantiate(prefab);
        CarrierController lc = land.GetComponent<CarrierController>();
        lc.init(initPosition, destroyPosition);
        if (!isEmpty)
            putEntity(lc);
        lc.transform.localScale = defaultScale;
        return lc;
    }
    public override List<CarrierController> getStartCarriers()
    {
        List<CarrierController> landList = new List<CarrierController>(3);
        CarrierController startLand = getCarrier(true);
        startLand.setPosition(new Vector3(16f, 1.3f, 0f));
        landList.Add(startLand);
        landList.Add(getCarrier(false));
        landList.Add(getCarrier(false));
        return landList;
    }
    public override void putEntity(CarrierController lc)
    {
        int entityNum = 0;
        float f = Random.Range(0f, 1f);
        if (f < 0.2)
            return;
        if (f < 0.4)
            entityNum = 1;
        else
            entityNum = 2;
        List<int> positionIndexList = new List<int> { 0, 1 };
        for (int i = 0; i < entityNum; i++)
        {
            int positionIndex = positionIndexList[Random.Range(0, positionIndexList.Count)];
            positionIndexList.Remove(positionIndex);

            if (Random.Range(0f, 1f) < isCactu)
            {
                GameObject cactu = CactuFactory.getCactu();
                lc.addEntity(cactu, placeablePosition[positionIndex]);
            }
            else
            {
                GameObject pterosaur = PterosaurFactory.getPterosaur();
                float height = heightPosition[Random.Range(0, heightPosition.Count)];
                Vector3 position = placeablePosition[positionIndex];
                position.y = height;
                lc.addEntity(pterosaur, position);
            }
        }
    }
}

public static class CactuFactory
{
    public static GameObject CactuPrefab = Resources.Load<GameObject>("Prefabs/CactuPrefab");
    public static List<Sprite> cactuSprites = new List<Sprite>(Resources.LoadAll<Sprite>("EntityRes/Cactus"));
    public static Vector3 defaultScale = new Vector3(1.25f, 1.25f);

    public static GameObject getCactu()
    {
        GameObject cactu = Object.Instantiate(CactuPrefab);
        cactu.transform.localScale = defaultScale;

        int spriteIndex = Random.Range(0, 10);
        Sprite sprite = CactuFactory.cactuSprites.Find(p => p.name == ("Cactus_" + spriteIndex.ToString()));
        cactu.GetComponent<SpriteRenderer>().sprite = sprite;

        cactu.AddComponent<PolygonCollider2D>();
        return cactu;
    }
}

public static class PterosaurFactory
{
    public static Vector3 defaultScale = new Vector3(1f, 1f);
    public static GameObject PterosaurPrefab = Resources.Load<GameObject>("Prefabs/PterosaurPrefab");

    public static GameObject getPterosaur()
    {
        GameObject pterosaur = Object.Instantiate(PterosaurPrefab);
        pterosaur.transform.localScale = defaultScale;
        return pterosaur;
    }
}

public class SkyFactory : CarrierFactory
{
    public SkyFactory()
    {
        width = 12.25f;
        baseSpeed = 8f;
        //可放置位置
        placeablePosition = new List<Vector3> { new Vector3(-0.6f, 0, 1f), new Vector3(0.6f, 0, 1f) };
        heightPosition = new List<float> { -2,2 };
        //地块预制体
        prefab = Resources.Load<GameObject>("Prefabs/SkyPrefab");
        //初始化位置
        initPosition = new Vector3(100f, 5f, 1f);
        //销毁位置
        destroyPosition = new Vector3(-999f, 5f, 1f);
        //默认缩放
        defaultScale = new Vector3(10f, 1.5f, 1f);
    }

    public override CarrierController getCarrier(bool isEmpty)
    {
        GameObject sky = Object.Instantiate(prefab);
        CarrierController sc = sky.GetComponent<CarrierController>();
        sc.init(initPosition, destroyPosition);
        if (!isEmpty)
            putEntity(sc);
        sc.transform.localScale = defaultScale;
        return sc;
    }
    public override List<CarrierController> getStartCarriers()
    {
        List<CarrierController> skyList = new List<CarrierController>(3);
        CarrierController cc = getCarrier(true);
        cc.setPosition(new Vector3(6,5,1));
        skyList.Add(cc);
        skyList.Add(getCarrier(false));
        skyList.Add(getCarrier(false));
        return skyList;
    }
    public override void putEntity(CarrierController sc)
    {
        int entityNum = 0;
        float f = Random.Range(0f, 1f);
        if (f < 0.4)
            entityNum = 1;
        else
            entityNum = 2;
        List<int> positionIndexList = new List<int> { 0, 1 };
        for (int i = 0; i < entityNum; i++)
        {
            int positionIndex = positionIndexList[Random.Range(0, positionIndexList.Count)];
            positionIndexList.Remove(positionIndex);

            GameObject cloud = CloudFactory.getCloud();
            float height = heightPosition[Random.Range(0, heightPosition.Count)];
            Vector3 position = placeablePosition[positionIndex];
            position.y = height;
            sc.addEntity(cloud, position);
        }
    }
}

public static class CloudFactory
{
    public static GameObject CloudPrefab = Resources.Load<GameObject>("Prefabs/CloudPrefab");
    public static List<Sprite> cloudSprites = new List<Sprite>(Resources.LoadAll<Sprite>("EntityRes/Cloud"));
    public static Vector3 defaultScale = new Vector3(1f, 1f);

    public static GameObject getCloud()
    {
        GameObject cloud = Object.Instantiate(CloudPrefab);
        cloud.transform.localScale = defaultScale;

        int spriteIndex = Random.Range(0, 3);
        Sprite sprite = CloudFactory.cloudSprites[spriteIndex];
        cloud.GetComponent<SpriteRenderer>().sprite = sprite;
        if(spriteIndex == 0)
            cloud.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));

        cloud.AddComponent<PolygonCollider2D>();
        return cloud;
    }
}