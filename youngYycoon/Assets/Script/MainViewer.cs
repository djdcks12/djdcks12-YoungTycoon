using UnityEngine;
using UnityEngine.UI;
using CooKing;
using UnityEngine.Events;

public class MainViewer : Viwer
{
    [SerializeField] private Button[] _tabs;
    [SerializeField] private GameObject[] _tabContents;
    [SerializeField] private CustomerController _customerController;
    [SerializeField] private Button[] _ingredientButtons;
    [SerializeField] Transform _dishPlate;
    [SerializeField] Dish _dishPrefab;

    public Dish DishPrefab => _dishPrefab;
    public int ButtonCount => _ingredientButtons.Length;
    public Transform DishPlate => _dishPlate;

    void Awake()
    {
        ISetViwerParam setViwerParam = new ISetViwerParam()
        {
            Viwer = this,
        };
        ControllerManager.Instance.SendMessage<MainController>(setViwerParam);
    }

    public void Initialize(int inDefaultTabIndex)
    {
        InitializeTabs();
        ChangeTab(inDefaultTabIndex);
    }

    private void InitializeTabs()
    {
        for (int i = 0; i < _tabs.Length; i++)
        {
            int index = i;
            _tabs[i].onClick.AddListener(() => ChangeTab(index));
        }
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

    public void SetButton(int index, UnityAction action)
    {
        _ingredientButtons[index].onClick.AddListener(action);
    }
}
