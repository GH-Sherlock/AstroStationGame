using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float coolTime = 6f;
    private float elapsedCoolTime = 0f;
    private TimeManager timeManager;
    public GameObject railgun;
    private int usageCount;


    private Railgun[] arrRailgun;
    private Vector2[] firePositions;
    private EnermyBulletPool bulletPool;
    private CometController cometPool;

    public BombUI bombUI;
    private void Awake()
    {
        
    }

    private void Start()
    {
        usageCount = 3;
        elapsedCoolTime = 0;
        timeManager = TimeManager.instance;

        float sizeX = GameManager.instance.GetBgBoundSizeX();
        float startX = -sizeX + (-sizeX / 2);
        float interval = (sizeX * 3) / 63;


        arrRailgun = new Railgun[64];
        firePositions = new Vector2[64];

        for (int i = 0; i < 64; i++)
        {
            GameObject bullet = Instantiate(railgun, this.transform);
            firePositions[i] = new Vector2(startX + (interval * i), -6.4f);
            bullet.transform.localPosition = firePositions[i];
            arrRailgun[i] = bullet.GetComponent<Railgun>();
            arrRailgun[i].SetBulletPool(this.transform);
            bullet.SetActive(false);
        }

        bulletPool = FindObjectOfType<EnermyBulletPool>();
        cometPool = FindObjectOfType<CometController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(elapsedCoolTime >= 0)
        {
            elapsedCoolTime -= timeManager.GetTimeScale();
        }
    }

    public void BombExplode()
    {
        if (usageCount <= 0)
            return;

        if (elapsedCoolTime > 0f)
            return;

        usageCount--;
        if(bombUI)
            bombUI.BombIconUpdate(usageCount);

        elapsedCoolTime = coolTime;

        for (int i = 0; i < 64; i++)
        {
            arrRailgun[i].Fire(arrRailgun[i].transform, Quaternion.identity);
            arrRailgun[i].transform.localPosition = firePositions[i];
            arrRailgun[i].transform.SetParent(null);
        }

        //적대적 총알 모두 파괴
        if (bulletPool)
            bulletPool.AllBulletDestroy();

        //행성 파편 모두 파괴
        if (cometPool)
            cometPool.AllDisableFragment();

        //
    }

    public int UsageCount
    {
        get { return usageCount; }
        set 
        { 
            usageCount = value;

            if (usageCount > 3) 
                usageCount = 3;

            if(bombUI)
                bombUI.BombIconUpdate(usageCount);
        }
    }
}
