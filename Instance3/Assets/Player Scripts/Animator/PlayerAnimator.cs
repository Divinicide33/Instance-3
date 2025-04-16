using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] Animator animatorUpBody;

    public static Action<bool> onSetIsAttacking {get; set;}
    public static Action<bool> onSetIsFacingRight {get; set;}
    public static Action<bool> onSetIsFacingUp {get; set;}

    void OnEnable()
    {
        onSetIsAttacking += IsAttacking;
        onSetIsFacingRight += IsFacingRight;
        onSetIsFacingUp += IsFacingUp;
    }

    void OnDisable()
    {
        onSetIsAttacking -= IsAttacking;
        onSetIsFacingRight -= IsFacingRight;
        onSetIsFacingUp -= IsFacingUp;
    }

    private void IsAttacking(bool value)
    {
        animatorUpBody.SetBool(SetAnimatorVariable.isAttacking.ToString(), value);
    }

    private void IsFacingRight(bool value)
    {
        animatorUpBody.SetBool(SetAnimatorVariable.isFacingRight.ToString(), value);
    }

    private void IsFacingUp(bool value)
    {
        animatorUpBody.SetBool(SetAnimatorVariable.isFacingUp.ToString(), value);
    }
}

public enum SetAnimatorVariable
{
    isAttacking,
    isFacingRight,
    isFacingUp

}