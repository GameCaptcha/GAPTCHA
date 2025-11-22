using System.Collections.Generic;
using UnityEngine;


public class GlobalDebuffManager : UpdateBehaviour
{
    [SerializeField] List<DebuffManager> debuffManagerList = new List<DebuffManager>();
    DebuffManager nowDebuffManager = null;

    protected override void FUpdate()
    {
        base.FUpdate();

        if(nowDebuffManager != null)
        {
            nowDebuffManager.DebuffUpdate();
        }
    }

    public void ChangeDebuff(bool allowSame = false)
    {
        if (debuffManagerList.Count <= 0)
            return;

        int count = debuffManagerList.Count;
        int randIndex;

        if (nowDebuffManager != null)
        {
            nowDebuffManager.OnDebuffExit();
        }


        if (!allowSame && count > 1)
        {
            do
            {
                randIndex = UnityEngine.Random.Range(0, count);
            }
            while (debuffManagerList[randIndex] == nowDebuffManager);
        }
        else
        {

            randIndex = UnityEngine.Random.Range(0, count);
        }

        nowDebuffManager = debuffManagerList[randIndex];

        nowDebuffManager.OnDebuffEnter();
    }

}
