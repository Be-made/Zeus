using System;
using UnityEngine;
/// <summary>
/// Сервис взаимодействия
/// </summary>
public class InteractionService : IService, IUpdater, ILateUpdater
{
    public event Action KeyDownA = () => { };
    public event Action KeyDownS = () => { };
    public event Action KeyDownD = () => { };
    public event Action KeyDownF = () => { };

    public event Action<Vector3> OnClick = (pos) => { };

    public event Action<Vector3> OneTouchBegin             = (pos) => { };
    public event Action<Vector3> OneTouchMove              = (pos) => { };
    public event Action OneTouchEnd                        = () => { };
    public event Action<Vector3, Vector3> DoubleTouchBegin = (pos1, pos2) => { };
    public event Action<Vector3, Vector3> DoubleTouchMoved = (pos1, pos2) => { };
    public event Action DoubleTouchEnd                     = () => { };

    public void StartService(EngineScript instance)
    {
    }

    void IUpdater.Update()
    {
        getKeyDownA();
        getKeyDownS();
        getKeyDownD();
        getKeyDownF();
        getClick();
    }

    void ILateUpdater.Update()
    {
        getTouch();
    }

    private void getKeyDownA()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            KeyDownA?.Invoke();
        }
    }

    private void getKeyDownS()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            KeyDownS?.Invoke();
        }
    }

    private void getKeyDownD()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            KeyDownD?.Invoke();
        }
    }

    private void getKeyDownF()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            KeyDownF?.Invoke();
        }
    }

    private void getTouch()
    {
        if (Input.touches.Length == 1)
        {
            DoubleTouchEnd?.Invoke();

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                OneTouchBegin?.Invoke(Camera.main.ScreenToWorldPoint(Input.touches[0].position));
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                OneTouchMove?.Invoke(Camera.main.ScreenToWorldPoint(Input.touches[0].position));
            }
        }
        else if (Input.touches.Length == 2)
        {
            OneTouchEnd?.Invoke();

            Touch t1 = Input.touches[0];
            Touch t2 = Input.touches[1];

            if (t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began || t1.phase == TouchPhase.Moved || t2.phase == TouchPhase.Moved)
            {
                DoubleTouchMoved?.Invoke(Camera.main.ScreenToWorldPoint(t1.position), Camera.main.ScreenToWorldPoint(t2.position));
            }
        }
        else
        {
            OneTouchEnd?.Invoke();
            DoubleTouchEnd?.Invoke();
        }            
    }

    private void getClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClick?.Invoke(Input.mousePosition);
        }
    }
}
