using UnityEngine;

public class UpdateBehaviour : MonoBehaviour
{
    private void FixedUpdate()
    {
        if(GlobalDatas.UPDATE_AI_CONTROL == false)
        {
            FUpdate();
        }
    }


    virtual protected void FUpdate()
    {

    }
}
