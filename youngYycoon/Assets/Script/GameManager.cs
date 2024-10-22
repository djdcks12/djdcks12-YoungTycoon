using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Button[] _tabs;
    [SerializeField] private GameObject[] _tabContents;
    [SerializeField] private CookerController _grillController;
    [SerializeField] private CookerController _frierController;
    [SerializeField] private CustomerController _customerController;
    [SerializeField] private Button[] _ingredientButtons;
    [SerializeField] GameObject _dishPlate;
    [SerializeField] Dish m_dish;

    LinkedList<Dish> _dishWaitList = new LinkedList<Dish>();
    Queue<Dish> _dishQueue = new Queue<Dish>();
    public enum CookType 
    {
        Grill,
        Frier,
    }

    public enum IngredientType
    {
        Chicken,
        Pork,
        Camel,
        Eagle,
        Beef,
        Lamb,
        Bat,
        MeerKat,
        DesertFox,
        Alpaca,
        HorseFlesh,
        Pigeon
    }

    private CookType _cookType;
    public void Start()
    {
        for(int i = 0; i < _tabs.Length; i++)
        {
            int index = i;
            _tabs[i].onClick.AddListener(() => ChangeTab(index));
        }
        for(int i = 0; i < _ingredientButtons.Length; i++)
        {
            IngredientType ingredientType = (IngredientType)i;
            _ingredientButtons[i].onClick.AddListener(() => OnTouchIngredient(ingredientType));
        }
        ChangeTab(2);
        _grillController.SetGrill(()=> {_cookType = CookType.Grill; _grillController.SetCookerImage(true); _frierController.SetCookerImage(false);}, (ingredinet) => OnAfterCookAction(CookType.Grill, ingredinet));
        _frierController.SetGrill(()=> {_cookType = CookType.Frier; _grillController.SetCookerImage(false); _frierController.SetCookerImage(true);}, (ingredinet) => OnAfterCookAction(CookType.Frier, ingredinet));
        _customerController.SetCustmoerController();
    }
    void OnTouchIngredient(IngredientType ingredientType)
    {
        switch(_cookType)
        {
            case CookType.Grill:
                _grillController.OnTouchIngredient(ingredientType);
                break;
            case CookType.Frier:
                _frierController.OnTouchIngredient(ingredientType);
                break;
        }
    }
    void OnAfterCookAction(CookType inMode, IngredientType inIngredientType)
    {
        if(_customerController.SellCustomer((inIngredientType, inMode)))
        {
            
        }
        else
        {
            var dish = GetDish();
            dish.gameObject.SetActive(true);
            dish.SetDish((inIngredientType, inMode),() => 
            {
                if(_customerController.SellCustomer((inIngredientType, inMode)))
                {
                    _dishWaitList.Remove(dish);
                    ReturnDish(dish);
                }
            });
            _dishWaitList.AddLast(dish);
        }
    }
    Dish GetDish()
    {
        if(_dishQueue.Count == 0)
        {
            var dish = Instantiate(m_dish, new Vector3(0,0,0), Quaternion.identity, _dishPlate.transform);
            return dish;
        }
        else
        {
            return _dishQueue.Dequeue();
        }
    }
    void ReturnDish(Dish inDish)
    {
        inDish.gameObject.SetActive(false);
        _dishQueue.Enqueue(inDish);
    }
    void ChangeTab(int index)
    {
        for (int i = 0; i < _tabs.Length; i++)
        {
            if (i == index)
            {
                _tabContents[i].SetActive(true);
            }
            else
            {
                _tabContents[i].SetActive(false);
            }
        }
    }

    public static string GetIngredientName(IngredientType inIngredientType)
    {
        return inIngredientType switch            
        {
            IngredientType.Chicken => "닭",
            IngredientType.Pork => "돼지",
            IngredientType.Camel => "낙타",
            IngredientType.Eagle => "독수리",
            IngredientType.Beef => "소",
            IngredientType.Lamb => "양",
            IngredientType.Bat => "박쥐",
            IngredientType.MeerKat => "미어캣",
            IngredientType.DesertFox => "여우",
            IngredientType.Alpaca => "알파카",
            IngredientType.HorseFlesh => "말",
            IngredientType.Pigeon => "비둘기",
            _ => "고기"
        };
    }

    public static string DishName((IngredientType, CookType)inDishType)
    {
        string ingredient = GetIngredientName(inDishType.Item1);
        string cook = inDishType.Item2 switch
        {
            CookType.Grill => "구운",
            CookType.Frier => "튀긴",
            _ => "조리"
        };
        return $"{cook} {ingredient}";
    }
}
