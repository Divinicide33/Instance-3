using UnityEngine;

public class PlayerDoubleJump : SkillModule
{
    [SerializeField] private int nbJumpToAdd;

    void Start()
    {
        PlayerJump.onChangeJump?.Invoke(true);
    }
    void OnEnable()
    {
        PlayerJump.onChangeJump?.Invoke(true);
    }

    void OnDisable()
    {
        PlayerJump.onChangeJump?.Invoke(false);
    }
}
