using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private Animator _footsAnimation;

    private readonly int _grounded = Animator.StringToHash("Grounded"); 
    private readonly int _speed = Animator.StringToHash("Speed"); 

    public void SetGroundState(bool state)
    {
        _footsAnimation.SetBool(_grounded, state);
    }

    public void SetMovementSpeed(float speed)
    {
        speed = Mathf.Clamp(speed, -1, 1);

        _footsAnimation.SetFloat(_speed, speed);
    }
}
