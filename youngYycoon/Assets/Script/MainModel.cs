using UnityEngine;
using CooKing;
using System.Collections.Generic;
public class MainModel : Model
{
    public CookType cookType;

    public LinkedList<Dish> dishWaitList = new LinkedList<Dish>();
    public Queue<Dish> dishQueue = new Queue<Dish>();
}
