using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyBulletPool : MonoBehaviour
{
    public GameObject m_pBullet;
    public int poolSize;

    private List<EnermyBullet> m_bulletList;
    private void Awake()
    {
        m_bulletList = new List<EnermyBullet>();

        for(int i = 0; i < poolSize; i++)
        {
            if (m_pBullet)
            {
                GameObject bullet = Instantiate(m_pBullet, this.transform);

                EnermyBullet enermyBullet = bullet.GetComponent<EnermyBullet>();
                enermyBullet.SetBulletPool(this.transform);
                enermyBullet.InitBullet();

                m_bulletList.Add(enermyBullet);
            }
        }
        
    }

    public void BulletFire(Transform firePos, Quaternion dir)
    {
       foreach(EnermyBullet b in m_bulletList)
        {
            if(!b.IsFire)
            {
                b.Fire(firePos, dir);
                //b.transform.SetParent(null);
                break;
            }
        }
    }

    public void AllBulletDestroy()
    {
        foreach (EnermyBullet b in m_bulletList)
        {
            if (b.IsFire)
            {
                b.PlayExplosionEffect();
                b.InitBullet();
            }
        }
    }
}
