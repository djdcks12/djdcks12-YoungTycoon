using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CookerController : MonoBehaviour
{
    [SerializeField] Button _grillButton;
    [SerializeField] Cook[] _cooks;
    [SerializeField] Image _cookerImage;
    UnityAction<GameManager.IngredientType> _afterCookAction;

    public void SetGrill(UnityAction inGrillTouchEvent, UnityAction<GameManager.IngredientType> inAfterCookAction)
    {
        _grillButton.onClick.AddListener(inGrillTouchEvent);
        _afterCookAction = inAfterCookAction;
        
        for(int i = 0; i < _cooks.Length; i++)
        {
            _cooks[i].gameObject.SetActive(false);
        }
    }
    public void OnTouchIngredient(GameManager.IngredientType ingredientType)
    {
        for(int i = 0; i < _cooks.Length; i++)
        {
            if(_cooks[i].GetStatus() == Cook.Status.Empty)
            {
                _cooks[i].gameObject.SetActive(true);
                _cooks[i].StartCooking(ingredientType, () => _afterCookAction(ingredientType));
                break;
            }
        }
    }
    public void SetCookerImage(bool isSelect)
    {
        _cookerImage.color = isSelect ? Color.gray : Color.white;
    }
}
