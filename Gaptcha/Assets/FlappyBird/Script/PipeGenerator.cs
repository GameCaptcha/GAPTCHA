using UnityEngine;

public class PipeGenerator : UpdateBehaviour
{
    [SerializeField] Transform pipeParent;
    //[SerializeField] GameObject pipe; // 파이프 프리팹
    [SerializeField] PipeFactory pipeFactory;
    const float coolDown = 1.5f; // 파이프 생성 쿨타임
    
    float _timer;
    

    public void Init()
    {
        Refresh();
        _timer = 0f;
    }

    void Refresh()
    {
        pipeFactory.Refresh();
    }

    override protected void FUpdate()
    {
        base.FUpdate();
        _timer += Time.fixedDeltaTime;
        
        if (_timer >= coolDown) { 
            //GameObject newPipe = Instantiate(pipe, pipeParent);
            PipeMove newPipe = pipeFactory.UseObject();
            newPipe.transform.parent = pipeParent;
            newPipe.transform.localPosition = new Vector3(6, Random.Range(-1.1f, 0.8f), 0);
            newPipe.Init(7.0f, pipeFactory.Restore);
            
            _timer -= coolDown;
        } // 쿨타임 돌 때마다 파이프 생성 & 5초 뒤 삭제
    }
}
