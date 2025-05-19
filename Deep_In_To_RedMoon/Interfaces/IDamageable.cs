using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    //데미지를 받을때 실행
    public void TakeDamage(float damage);
}
