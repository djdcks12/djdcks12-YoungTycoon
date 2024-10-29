using System;
using System.Collections.Generic;
using UnityEngine;

public class IControllerParam
{
    
}

public class ISetViwerParam : IControllerParam
{
    public Viwer Viwer { get; set; }
}


public class ControllerManager : SingletoneClass<ControllerManager>
{
    Dictionary<Type, Controller> _controllers = new Dictionary<Type, Controller>();

    public C Get<C>() where C : Controller, new()
    {
        var type = typeof(C);
        if (_controllers.ContainsKey(type))
            return _controllers[type] as C;

        var newController = new C();

        _controllers.Add(type, newController);

        return newController;
    }

    public void SendMessage<C>(IControllerParam param) where C : Controller, new()
    {
        var controller = Get<C>();

        controller.SendMessage(param);
    }
    
}
