using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : MonoBehaviour {

    private INode _rootNode;

    private Animator _animator;

    private MoveGateScript _moveGateScript;

    //checkdead
    public bool Dead = false;

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
        _moveGateScript = GameObject.Find("Gate").GetComponent<MoveGateScript>();

        _rootNode =
    new SelectorNode
    (
            new SequenceNode
            (
                new ConditionNode(IsDead),
                new ActionNode(PlayDeathAnimation)
            ),
            new SequenceNode
            (
                new ConditionNode(SeesPlayer),
                new ActionNode(ShootPlayer)
            ),
            new ActionNode(Roaming)
    );
        StartCoroutine(RunTree());
    }
    IEnumerator RunTree()
    {
        while(Application.isPlaying)
        {
            yield return _rootNode.Tick();
        }
    }
    bool IsDead()
    {
        return Dead;
    }
    bool SeesPlayer()
    {
        return true;
    }
    IEnumerator <NodeResult> PlayDeathAnimation()
    {
        Destroy(this.gameObject.GetComponent<CapsuleCollider>());
        _animator.SetBool("EnemyDies", true);
        _moveGateScript.EnemyDead = true;
        yield return NodeResult.Succes;
    }
    IEnumerator<NodeResult> ShootPlayer()
    {
        yield return NodeResult.Succes;
    }
    IEnumerator<NodeResult> Roaming()
    {
        yield return NodeResult.Succes;
    }

    private void OnTriggerEnter(Collider _collision)
    {
        if (_collision.gameObject.tag == "EnemyHitBox")
        {
            Dead = true;
        }
    }
}
