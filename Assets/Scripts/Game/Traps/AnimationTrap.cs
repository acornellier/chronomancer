using Animancer;

public class AnimationTrap : Trap
{
    SoloAnimation _animation;

    protected override void Awake()
    {
        base.Awake();
        _animation = GetComponent<SoloAnimation>();
    }

    protected override void Update()
    {
        base.Update();
        _animation.Speed = SpeedTracker.Multiplier;
    }
}