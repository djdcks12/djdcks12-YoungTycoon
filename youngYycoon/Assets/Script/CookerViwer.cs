using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using CooKing;

public class CookerViwer : Viwer
{
    [SerializeField] Button _grillButton;
    [SerializeField] Cook[] _cooks;
    [SerializeField] Image _cookerImage;
    
    void Awake()
    {
        ISetViwerParam setViwerParam = new ISetViwerParam()
        {
            Viwer = this,
        };
        ControllerManager.Instance.SendMessage<CookerController>(setViwerParam);
    }
    public void SetGrillButtonEvent(UnityAction inGrillTouchEvent)
    {
        _grillButton.onClick.RemoveAllListeners();
        _grillButton.onClick.AddListener(inGrillTouchEvent);
    }
    public void InitGrill()
    {
        for(int i = 0; i < _cooks.Length; i++)
        {
            _cooks[i].gameObject.SetActive(false);
        }
    }
    public void OnTouchIngredient(IngredientType ingredientType, UnityAction afterCookAction)
    {
        for(int i = 0; i < _cooks.Length; i++)
        {
            if(_cooks[i].GetStatus() == Cook.Status.Empty)
            {
                _cooks[i].gameObject.SetActive(true);
                _cooks[i].StartCooking(ingredientType, afterCookAction);
                break;
            }
        }
    }
    public void OnSetCookerImage(bool isSelect)
    {
        _cookerImage.color = isSelect ? Color.gray : Color.white;
    }
}
