using System;
using System.Collections.Generic;
using UnityEngine;


public class GlobalDebuffManager : UpdateBehaviour
{
    List<DebuffManager> debuffManagerList = new List<DebuffManager>();
    DebuffManager nowDebuffManager = null;

    protected override void FUpdate()
    {
        base.FUpdate();

        if(nowDebuffManager != null)
        {
            nowDebuffManager.DebuffUpdate();
        }
    }

    public void Refresh()
    {
        nowDebuffManager = null;
    }

    public void ChangeDebuff(bool allowSame = false)
    {
        if (debuffManagerList.Count <= 0)
            return;

        int randIndex = UnityEngine.Random.Range(0, debuffManagerList.Count);

        if (allowSame == false)
        {
            if (debuffManagerList[randIndex] == nowDebuffManager)
            {
                ChangeDebuff(allowSame);
                return;
            }
        }

        nowDebuffManager = debuffManagerList[randIndex];
    }

}
