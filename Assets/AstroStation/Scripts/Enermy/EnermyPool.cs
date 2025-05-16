using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyPool : MonoBehaviour
{
    public int poolSize = 10;

    public GameObject[] enermyTypes;


    public List<Enermy> arrEnermy;
    private void Awake()
    {
        arrEnermy = new List<Enermy>();
        float delay = 0f;

        for (int j = 0; j < enermyTypes.Length; j++)
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject enermyObject = Instantiate(enermyTypes[j], this.transform);

                float x = Random.Range(-48f, 48f);
                float y = Random.Range(10f, 32f);

                //코드 2줄 추가
                delay = delay + 1f;
                enermyObject.GetComponent<Enermy>().delayTime = delay;

                enermyObject.transform.position = new Vector3(x, y, 0f);
                //enermyObject.GetComponent<Enermy>().MoveTo();

                arrEnermy.Add(enermyObject.GetComponent<Enermy>());
                enermyObject.SetActive(false);
            }
        }

        StartCoroutine(EnermySetUp());
    }


    public void EnermyPositionSort(float minX, float maxX, float moveX)
    {
        foreach(Enermy e in arrEnermy)
        {
            if(e != null)
            {
                Vector3 pos = e.transform.position;
                if(minX <= pos.x && pos.x <= maxX)
                {
                    e.transform.Translate(moveX, 0, 0);
                    e.MoveDestReset(moveX);
                }
            }
        }
    }

    public Transform[] FindClosedEnermy(Vector3 start, int size)
    {
        float minDistance = 1000f;
        Transform[] results = new Transform[size];
        List<Enermy> usingEnermy = new List<Enermy>();

        foreach(Enermy e in arrEnermy)
        {
            if (e.gameObject.activeSelf)
                usingEnermy.Add(e);
        }

        if (usingEnermy.Count <= 0)
            return results;

        if(usingEnermy.Count < size)
        {
            int cnt = 0;
            int enermyLen = usingEnermy.Count;
            while(cnt < size)
            {
                results[cnt] = usingEnermy[cnt % enermyLen].transform;
                cnt++;
            }

            return results;
        }

        for (int i = 0; i < size; i++)
        {
            minDistance = 1000f;
            Enermy closedEnermy = null;

            foreach (Enermy e in usingEnermy)
            {
                if (!e.IsLockOn)
                {
                    float distance = Vector3.Distance(start, e.transform.position);

                    if (distance < minDistance)
                    {
                        if(closedEnermy != null)
                            closedEnermy.IsLockOn = false;

                        minDistance = distance;
                        results[i] = e.transform;
                        e.IsLockOn = true;
                        closedEnermy = e;
                    }
                }
            }
        }

        
        foreach(Enermy e in usingEnermy)
        {
            e.IsLockOn = false;
        }
        

        return results;
    }

    public List<Transform> GetActiveEnermy()
    {
        List<Transform> result = new List<Transform>();

        foreach(Enermy e in arrEnermy)
        {
            if (e.gameObject.activeSelf)
                result.Add(e.transform);
        }

        return result;
    }

    private IEnumerator EnermySetUp()
    {
        while (true)
        {
            int typeNumber = Random.Range(0, enermyTypes.Length);

            foreach (Enermy e in arrEnermy)
            {
                if (typeNumber != (int)e.m_type)
                    continue;

                if (!e.gameObject.activeSelf)
                {
                    e.Init();
                    e.MoveTo();
                    break;
                }
            }
            //SetMovePattern();
            yield return new WaitForSeconds(5f);
        }
    }

    private void OnApplicationQuit()
    {
        StopCoroutine(EnermySetUp());
    }

    private void SetMovePattern()
    {
        int patternNumber = Random.Range(0, 1);
        
        switch(patternNumber)
        {
            case 0:
                StartCoroutine(MovePatternA());
                break;
            case 1:
                StartCoroutine(MovePatternB());
                break;
            case 2:
                StartCoroutine(MovePatternC());
                break;
        }
    }

    private IEnumerator MovePatternA()
    {
        Vector3[] pos = new Vector3[5];

        pos[0] = new Vector3(-28f, 31f, 0f);
        pos[1] = new Vector3(-18f, 22f, 0f);
        pos[2] = new Vector3(-10f, 29f, 0f);
        pos[3] = new Vector3(0f, 20f, 0f);
        pos[4] = new Vector3(12f, 29f, 0f);

        for(int i = 0; i < 5; i++)
        {
            foreach (Enermy e in arrEnermy)
            {
                if (!e.gameObject.activeSelf)
                {
                    Vector3 temp = pos[4];
                    temp.x -= i;

                    e.arrPos[0] = pos[0];
                    e.arrPos[1] = pos[1];
                    e.arrPos[2] = pos[2];
                    e.arrPos[3] = pos[3];
                    e.arrPos[4] = temp;
                    e.Init();
                    break;
                }
            }

            yield return new WaitForSeconds(1f);
        }


        yield return null;
    }

    private IEnumerator MovePatternB()
    {
        yield return null;
    }

    private IEnumerator MovePatternC()
    {
        yield return null;
    }

}
