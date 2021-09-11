using System.Collections.Generic;
using UnityEngine;

public static class LandFactory
{
    //是仙人掌的概率
    public static float isCactu = 0.6f;
    //是翼龙的概率
    public static float isPterosaur = 1 - isCactu;

    //基础速度
    public static float baseSpeed = 15f;
    //增速步长
    public static float addSpeedStep = 0.05f;

    //可放置位置
    public static List<Vector3> placeablePosition 
        = new List<Vector3> { new Vector3(-1f, -0.1f, -2f), new Vector3(1f, -0.1f, -2f) };
    public static List<float> heightPosition 
        = new List<float> { 0, 0.05f, 0.2f };

    //地块预制体
    public static GameObject LandPrefab = Resources.Load<GameObject>("Prefabs/LandPrefab");

    //初始化位置 31.5
    public static Vector3 initPosition = new Vector3(100f, 1.3f, 0f);
    //下一个地块启动位置
    public static Vector3 nextLandMovePosition = new Vector3(-6.5f, 1.3f, 0f);
    //销毁位置
    public static Vector3 destroyPosition = new Vector3(-999f, 1.3f, 0f);
    //默认缩放
    public static Vector3 defaultScale = new Vector3(16f, 16f, 1f);

    public static LandController getLand(bool isEmpty)
    {
        GameObject land = Object.Instantiate(LandPrefab);
        LandController lc = land.GetComponent<LandController>();
        lc.init(initPosition, destroyPosition);
        if (!isEmpty)
            putEntity(lc);
        lc.transform.localScale = defaultScale;
        return lc;
    }
    public static List<LandController> getStartLands()
    {
        List<LandController> landList = new List<LandController>(3);
        LandController startLand = getLand(true);
        startLand.setPosition(new Vector3(16f,1.3f,0f));
        landList.Add(startLand);
        landList.Add(getLand(false));
        landList.Add(getLand(false));
        return landList;
    }
    private static void putEntity(LandController lc)
    {
        int entityNum = 0;
        float f = Random.Range(0f, 1f);
        if(f < 0.2)
            return;
        if (f < 0.4)
            entityNum = 1;
        else
            entityNum = 2;
        List<int> positionIndexList = new List<int> { 0, 1};
        for(int i = 0;i < entityNum; i++)
        {
            int positionIndex = positionIndexList[Random.Range(0, positionIndexList.Count)];
            positionIndexList.Remove(positionIndex);

            if(Random.Range(0f, 1f) < isCactu)
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
        Vector2 spriteSize = setSprite(cactu);
        setBoxCollider(cactu, spriteSize);
        return cactu;
    }
    private static Vector2 setSprite(GameObject cactu)
    {
        int spriteIndex = Random.Range(0, 10);
        Sprite sprite = CactuFactory.cactuSprites.Find(p => p.name == ("Cactus_" + spriteIndex.ToString()));
        cactu.GetComponent<SpriteRenderer>().sprite = sprite;
        return new Vector2(sprite.bounds.size.x, sprite.bounds.size.y);
    }
    private static void setBoxCollider(GameObject cactu, Vector2 spriteSize)
    {
        cactu.AddComponent<BoxCollider>();
        BoxCollider bc = cactu.GetComponent<BoxCollider>();
        bc.size = new Vector3(spriteSize.x, spriteSize.y, 1f);
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