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

    //Ai shooting player
    public bool AiSeesPlayer;
    public GameObject Player;
    private float _hitChancePercentage = 2;

    //player hitpoints
    private int _hitPoints = 5;

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
        _moveGateScript = GameObject.Find("Gate").GetComponent<MoveGateScript>();
        _agent = gameObject.GetComponent<NavMeshAgent>();
        Player = GameObject.Find("MainCharacter");

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
        AiSeesPlayer = DoesAiSeePlayer();
        return AiSeesPlayer;
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
        DoesAIHitPlayer();
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
        _animator.SetFloat("AgentVertical", _agent.velocity.x);
        _animator.SetFloat("AgentHorizontal", _agent.velocity.z);
    }

    private bool DoesAiSeePlayer()
    {
        if(Physics.Linecast(this.gameObject.transform.position,Player.transform.position))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    
    private void DoesAIHitPlayer()
    {
        if(DoesAiSeePlayer() && _hitPoints > 0)
        {
            float playerHitChance = Random.Range(0, 1000);
            if(_hitChancePercentage >= playerHitChance)
            {
                Debug.Log("hit");
                _hitPoints = _hitPoints - 1;
                Debug.Log(_hitPoints);
            }
        }
        else if(_hitPoints == 0)
        {
             Player.GetComponent<CharacterControlScript>().State = CharacterControlScript.States.dead;
        }
    }
}
