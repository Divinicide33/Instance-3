using UnityEngine;
using BehaviorTree;

public class BTAction_PatrolHermes : BTNode
{
    private HermèsBehaviorTree Tree;
    private Vector2 _direction;
    private float _detectionDistance = 0.75f;  
    private bool _initialized = false;

    public BTAction_PatrolHermes(HermèsBehaviorTree btParent)
    {
        Tree = btParent;
    }

    public override BTNodeState Evaluate()
    {
        if (!_initialized)
        {
            _direction = Tree.lastDashDirection != Vector2.zero ? Tree.lastDashDirection : Vector2.right;
            bool facingRight = _direction.x > 0f;
            Tree.Tree.eulerAngles = new Vector3(0f, facingRight ? 0f : 180f, 0f);

            _initialized = true;
        }
        RaycastHit2D hitObstacle = Physics2D.Raycast(Tree.Tree.position, _direction, _detectionDistance, LayerMask.GetMask("Platform"));
        if (hitObstacle.collider != null)
        {
            FlipDirection();
        }

        Tree.Tree.position += (Vector3)(_direction.normalized * Tree.speed * Time.deltaTime);

        return BTNodeState.RUNNING;
    }

    public void FlipDirection()
    {
        _direction *= -1;
        Vector3 currentEuler = Tree.Tree.eulerAngles;
        Tree.Tree.eulerAngles = new Vector3(currentEuler.x, currentEuler.y + 180f, currentEuler.z);
    }
}
