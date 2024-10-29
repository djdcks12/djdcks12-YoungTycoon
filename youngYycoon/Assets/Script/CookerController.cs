using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using CooKing;

public class ISetGrillParm:IControllerParam
{
    public UnityAction inGrillTouchEvent;
    public UnityAction<IngredientType> inAfterCookAction;
}

public class ITouchIngredientParam:IControllerParam
{
    public IngredientType ingredientType;
}
public class ISetCookerImageParam:IControllerParam
{
    public bool isSelect;
}

public class CookerController : Controller
{
    CookerViwer _cookViwer;
    CookerModel _cookModel;

    CookerViwer Viewer
    {
        get
        {
            if (_cookViwer == null)
                _cookViwer = GetViewer<CookerViwer>();
            return _cookViwer;
        }
    }

    CookerModel Model
    {
        get
        {
            if (_cookModel == null)
                _cookModel = GetModel<CookerModel>();
            return _cookModel;
        }
    }

    protected override void OnRecv_SendMessage(IControllerParam param)
    {
        if(param is ISetGrillParm)
        {
            var setGrillParm = param as ISetGrillParm;
            SetGrill(setGrillParm.inGrillTouchEvent, setGrillParm.inAfterCookAction);
        }
        else if(param is ITouchIngredientParam)
        {
            var touchIngredientParam = param as ITouchIngredientParam;
            TouchIngredient(touchIngredientParam.ingredientType);
        }
        else if(param is ISetCookerImageParam)
        {
            var setCookerImageParam = param as ISetCookerImageParam;
            SetCookerImage(setCookerImageParam.isSelect);
        }
    }

    void SetGrill(UnityAction inGrillTouchEvent, UnityAction<IngredientType> inAfterCookAction)
    {
        Model._afterCookAction = inAfterCookAction;
        Viewer.SetGrillButtonEvent(inGrillTouchEvent);
        Viewer.InitGrill();
    }
    
    void TouchIngredient(IngredientType ingredientType)
    {
        Viewer.OnTouchIngredient(ingredientType, () => GetModel<CookerModel>()._afterCookAction(ingredientType));
    }

    void SetCookerImage(bool isSelect)
    {
        Viewer.OnSetCookerImage(isSelect);
    }
}
