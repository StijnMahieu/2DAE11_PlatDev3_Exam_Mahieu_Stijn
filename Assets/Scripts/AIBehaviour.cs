using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : MonoBehaviour {
    private INode _rootNode;
    // Use this for initialization
    void Start()
    {
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
                new ConditionNode(IsHit),
                new ActionNode(PlayHitAnimation)
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
        return true;
    }
    bool IsHit()
    {
        return true;
    }
    bool SeesPlayer()
    {
        return true;
    }
    IEnumerator <NodeResult> PlayDeathAnimation()
    {
        yield return NodeResult.Succes;
    }
    IEnumerator<NodeResult> PlayHitAnimation()
    {
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
}
