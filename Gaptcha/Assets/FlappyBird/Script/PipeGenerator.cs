using UnityEngine;

public class PipeGenerator : UpdateBehaviour
{
    [SerializeField] Transform pipeParent;
    [SerializeField] PipeFactory pipeFactory;
    [SerializeField] private float coolDown = 1.5f; 
    
    float _timer;
    

    public void Init()
    {
        Refresh();
        _timer = 0f;
        
        PipeMove newPipe = pipeFactory.UseObject();
        newPipe.transform.parent = pipeParent;
        newPipe.transform.localPosition = new Vector3(6, Random.Range(-1.1f, 0.8f), 0);
        newPipe.Init(7.0f, pipeFactory.Restore);
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
        }
    }
}
