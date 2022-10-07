using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimationController
{
    void SetCharAnimSpeed(float speed);
    //void ReciveDamageAnimBool(bool isReciveDamage);
    void SetAnimatorVec(Vector2 DirVec);
    void SetDeath();
    Material GetMaterial();
}
