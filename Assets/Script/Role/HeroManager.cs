using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : Singleton<HeroManager>
{
    //private Dictionary<int, Hero> _dicHero = new Dictionary<int, Hero>();

    public override void Init()
    {

    }
    
    //public bool Create(int id)
    //{
    //    Hero hero = new Hero();
    //    if (hero.Init(id) == false)
    //    {
    //        return false;
    //    }

    //    _dicHero.Add(hero.Id, hero);

    //    return true;
    //}

    //public void ShowAllHero()
    //{
    //    foreach (var h in _dicHero)
    //    {
    //        Debug.Log("Hero: " + h.Value.Id);
    //    }
    //}
}
