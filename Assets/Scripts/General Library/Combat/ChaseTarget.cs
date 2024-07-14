/*
 * Given an agent and a target, have the agent seek the target
 * Requires NavMeshAgent
 * @ Natasha Badami '20. Added by Nigel Charleston '21
 * 
 * This code may have aspects/assumptions that were specific to its original project and hard coded values 
 * I would recommend using it as a reference (when implementing a new script), and heavily adjusting it based on your team's needs
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft.Combat
{
    public class ChaseTarget : MonoBehaviour
    {
        [SerializeField] string TargetTag;
        Transform target;
        bool ChaseActive;
        UnityEngine.AI.NavMeshAgent agent;
        public bool hit { get; set; }
        void Start()
        {
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            target = GameObject.FindWithTag(TargetTag).transform;
        }

        // Update is called once per frame
        void Update()
        {
            if (target && ChaseActive)
            {
                agent.SetDestination(target.position);
            }
        }

        public void SetMoveSpeed(float speed)
        {
            agent.speed = speed;
        }

        public void SetChaseTrue()
        {
            ChaseActive = true;
        }

        public void SetChaseFalse()
        {
            if (!hit)
            {
                ChaseActive = false;
            }
        }

    }
}