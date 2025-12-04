using System;
using System.Collections.Generic;
using UnityEngine;


public class GlobalDebuffManager : UpdateBehaviour
{
    [SerializeField] List<DebuffManager> debuffManagerList = new List<DebuffManager>();
    DebuffManager nowDebuffManager = null;

    // Player and background is already inside here
    [SerializeField] List<SpriteBehaviour> spriteBehaviourList = new List<SpriteBehaviour>();

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
        foreach (DebuffManager dm in debuffManagerList) {
            if (dm != null) { dm.OnDebuffExit(); }
        }

        nowDebuffManager = null;
    }

    public void ChangeDebuff(bool allowSame = false)
    {
        GlobalDatas.DebugLog(() => "GDM ChangeDebuff CALLED");

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
        GlobalDatas.DebugLog(() => "CALLING " + nowDebuffManager);

        ShaderDebuff shaderDebuff = nowDebuffManager as ShaderDebuff;
        if (shaderDebuff != null)
        {
            shaderDebuff.Init(spriteBehaviourList);
        }

        nowDebuffManager.OnDebuffEnter();

    }

    public void AddSpriteBehaviour(SpriteBehaviour spriteBehaviour)
    {
        spriteBehaviourList.Add(spriteBehaviour);
    }

    /// <summary>
    /// 현재 활성화된 디버프 매니저를 반환합니다. 없으면 null입니다.
    /// </summary>
    public DebuffManager GetCurrentDebuff()
    {
        return nowDebuffManager;
    }
}


