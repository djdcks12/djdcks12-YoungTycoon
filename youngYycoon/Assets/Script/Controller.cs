using UnityEngine;
using System;
using System.Collections.Generic;

public class Controller
{
    Viwer viwer;
    Model _model;
    Action<IControllerParam> _messageDispatcher;
    public Controller()
    {
        _messageDispatcher = OnRecv_Message;
    }

    private void OnRecv_Message(IControllerParam param)
    {
        if (param is ISetViwerParam)
        {
            var setViwerParam = param as ISetViwerParam;
            SetViewer(setViwerParam.Viwer);
        }
        else if (param is IControllerParam)
        {
            OnRecv_SendMessage(param);
        }
    }
    protected virtual void OnRecv_SendMessage(IControllerParam param)
    {

    }

    public void SendMessage(IControllerParam param)
    {
        _messageDispatcher(param);
    }
    
    protected V GetViewer<V>()
            where V : Viwer
    {
        return viwer as V;
    }

    void SetViewer<V>(V viwer)
        where V : Viwer
    {
        this.viwer = viwer;
    }

    protected M GetModel<M>()
            where M : Model, new()
    {
        if(_model == null)
        {
            _model = new M();
        }

        return _model as M;
    }
}