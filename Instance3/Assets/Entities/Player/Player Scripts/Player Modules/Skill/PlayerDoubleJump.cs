public class PlayerDoubleJump : SkillModule
{
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
