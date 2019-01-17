using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBehaviour : MonoBehaviour {

    private INode _rootNode;

    private Animator _animator;

    private MoveGateScript _moveGateScript;

    private NavMeshAgent _agent;
    //wandering
    private float _maxRoamDistance = 10;

    //checkdead
    public bool Dead = false;

    //movementanimations
    private float _agentHorizontal = Animator.StringToHash("AgentHorizontal");
    private float _agentVertical = Animator.StringToHash("AgentVertical");

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
        _moveGateScript = GameObject.Find("Gate").GetComponent<MoveGateScript>();
        _agent = gameObject.GetComponent<NavMeshAgent>();

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
        return false;
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
        AIAnimations();

        if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                float newTarget = Random.Range(0, 100);
                if (newTarget >= 99)
                {
                    Vector3 newPosition = transform.position + Random.insideUnitSphere * _maxRoamDistance;
                    NavMeshHit hit;
                    NavMesh.SamplePosition(newPosition, out hit, Random.Range(0, _maxRoamDistance), 1);

                    _agent.SetDestination(hit.position);
                }
            }
        yield return NodeResult.Succes;
    }

    private void OnTriggerEnter(Collider _collision)
    {
        if (_collision.gameObject.tag == "EnemyHitBox")
        {
            Dead = true;
        }
    }

    private void AIAnimations()
    {
        _animator.SetFloat("AgentVertical", _agent.velocity.z);
        _animator.SetFloat("AgentHorizontal", _agent.velocity.x);
    }
}
