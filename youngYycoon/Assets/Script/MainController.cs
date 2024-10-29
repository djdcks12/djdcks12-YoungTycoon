using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

namespace  CooKing
{
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
    public enum SellType
    {
        Immediate,
        Table
    }

    public class IResponseSellCustomerParm : IControllerParam
    {
        public bool isSell;
        public CookType Mode { get; set; }
        public IngredientType Type { get; set; }
        public SellType SellType { get; set; }
        public Dish Dish { get; set; }
    }
    public class InitMainParm : IControllerParam
    {

    }
    public class MainController : Controller
    {
        MainViewer _viewer;

        MainViewer Viewer
        {
            get
            {
                if (_viewer == null)
                    _viewer = GetViewer<MainViewer>();
                return _viewer;
            }
        }
        MainModel _model;
        MainModel Model
        {
            get
            {
                if (_model == null)
                    _model = GetModel<MainModel>();
                return _model as MainModel;
            }
        }
        public const int DefaultTabIndex = 2;

        protected override void OnRecv_SendMessage(IControllerParam param)
        {
            if(param is InitMainParm)
            {
                InitializeIngredientButtons();
                InitializeCookers();
                Viewer.Initialize(DefaultTabIndex);
                ControllerManager.Instance.SendMessage<MainController>(new ISetCustmoerParm());
            }
            if(param is IResponseSellCustomerParm)
            {
                var responseSellCustomerParm = param as IResponseSellCustomerParm;

                if(responseSellCustomerParm.SellType == SellType.Immediate)
                {
                    if(responseSellCustomerParm.isSell)
                    {
                        // 판매 성공
                    }
                    else // 판매에 실패하면 테이블에 올려 둠
                    {
                        var dish = GetDish();
                        dish.gameObject.SetActive(true);
                        dish.SetDish((responseSellCustomerParm.Type, responseSellCustomerParm.Mode),() => 
                        {
                            OnAfterCookAction(responseSellCustomerParm.Mode, responseSellCustomerParm.Type, SellType.Table);
                        });
                        Model.dishWaitList.AddLast(dish);
                    }
                }
                else if(responseSellCustomerParm.SellType == SellType.Table)
                {
                    Model.dishWaitList.Remove(responseSellCustomerParm.Dish);
                    ReturnDish(responseSellCustomerParm.Dish);
                }
            }
        }

        private void InitializeIngredientButtons()
        {
            for (int i = 0; i < Viewer.ButtonCount; i++)
            {
                Viewer.SetButton(i, () => OnTouchIngredient((IngredientType)i));
            }
        }
        
        private void InitializeCookers()
        {
            ControllerManager.Instance.SendMessage<CookerController>(new ISetGrillParm()
            {
                inGrillTouchEvent = () => SetCookType(CookType.Grill, true, false),
                inAfterCookAction = ingredient => OnAfterCookAction(CookType.Grill, ingredient, SellType.Immediate)
            });
        }
        
        private void SetCookType(CookType inCookType, bool inGrillActive, bool inFrierActive)
        {
            Model.cookType = inCookType;

            ControllerManager.Instance.SendMessage<CookerController>(new ISetCookerImageParam()
            {
                isSelect = inGrillActive
            });
        }

        void OnTouchIngredient(IngredientType ingredientType)
        {
            ControllerManager.Instance.SendMessage<CookerController>(new ITouchIngredientParam()
            {
                ingredientType = ingredientType
            });
        }

        void OnAfterCookAction(CookType inMode, IngredientType inIngredientType, SellType inSellType, Dish inDish = null)
        {
            ControllerManager.Instance.SendMessage<CustomerController>(new ISellSellCustomerParm()
            {
                DishType = (inIngredientType, inMode),
                SellType = inSellType,
                Dish = inDish
            });
        }

        Dish GetDish()
        {
            if(Model.dishQueue.Count == 0)
            {
                var dish = GameObject.Instantiate(Viewer.DishPrefab, new Vector3(0,0,0), Quaternion.identity, Viewer.DishPlate);
                return dish;
            }
            else
            {
                return Model.dishQueue.Dequeue();
            }
        }

        void ReturnDish(Dish inDish)
        {
            inDish.gameObject.SetActive(false);
            Model.dishQueue.Enqueue(inDish);
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
}
