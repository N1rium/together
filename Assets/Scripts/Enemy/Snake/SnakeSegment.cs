using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Snake
{
    public class SnakeSegment : MonoBehaviour
    {
        public Transform target;
        public float followSpeed = 5f;
        public float distanceToKeep = 1f;

        private Vector3 _targetLastPos;

        private void Start()
        {
            _targetLastPos = target.position;
        }

        void Update()
        {
            if (target == null) return;
            // Calculate direction to target
            /*Vector3 direction = target.position - transform.position;*/
            var targetDir = (_targetLastPos - target.position).normalized;
            var targetPos =  (target.position + targetDir) * distanceToKeep;
            
            /*float distance = direction.magnitude;*/
            
            // Only move if we're further than the desired distance
                /*direction.Normalize();*/
            transform.position = Vector3.Lerp(
                transform.position, 
                targetPos, 
                followSpeed * Time.deltaTime
            );

            _targetLastPos = target.position;
        }
    }
}