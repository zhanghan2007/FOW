using FOW;
using UnityEngine;

public class WarFog : MonoBehaviour
{
    public static WarFog I;

    private bool isInit;

    private void Awake()
    {
        I = this;
        isInit = false;
        ShowFog();
    }

    private void OnDestroy()
    {
        I = null;
    }

    public void ShowFog()
    {
        isInit = true;
        FOWLogic.I?.Init();
    }

    public void Destroy()
    {
        I = null;

        if (isInit) FOWLogic.I?.Dispose();
    }

    private void Update()
    {
        if (isInit) FOWLogic.I?.Update((int)(Time.deltaTime * 1000f));
    }
}
