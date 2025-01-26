using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy 
{
    public void GetDamage(BubbleType bubble);
    public void GetDamageAmmount(int damage);
}
