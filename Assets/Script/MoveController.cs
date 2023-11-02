using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class MoveController : MonoBehaviour
    {
        // private float _angle;
        public Collider2D obstacleCheckCollider;
        public Collider2D obstacleCollisionCollider;
        public Transform _target;
        public List<Transform> obstacles;
        public ContactFilter2D filter2D;
        public float speed;

        private Vector2 _mouse;

        private Camera _camera;

        private Vector2 a = new(0, 1);
        private Vector2[] angles;

        //
        private void Start()
        {
            angles = new[]
            {
                new Vector2(0, 1),
                new Vector2(0.4f, 1),
                new Vector2(1, 1),
                new Vector2(1, 0.4f),
                new Vector2(1, 0),
                new Vector2(1, -0.4f),
                new Vector2(1, -1),
                new Vector2(0.4f, -1),
                new Vector2(0, -1),
                new Vector2(-0.4f, -1),
                new Vector2(-1, -1),
                new Vector2(-1, -0.4f),
                new Vector2(-1, 0),
                new Vector2(-1, 0.4f),
                new Vector2(-1, 1),
                new Vector2(-0.4f, 1)
            };
        }

        private void Update()
        {
            obstacles = CheckObstacle(obstacleCheckCollider);
            var collisionObstacle = CheckObstacle(obstacleCollisionCollider);
            var distances = new float[16];
            var obstacleDist = new float[16];
            for (var i = 0; i < angles.Length; i++)
            {
                var value = SetAngleToTarget( angles[i], _target.position);
                foreach (var val in obstacles)
                {
                    value *=  AddWeightToAngle(angles[i], val.position);
                }
                distances[i] = value;
            }

            // for (var i = 0; i < angles.Length; i++)
            // {
            //     var value = 0f;
            //
            //     foreach (var obstacle in obstacles)
            //     {
            //         value += AddWeightToAngle(angles[i], obstacle.position)-value;
            //     }
            //
            //     value /= obstacles.Count;
            //     obstacleDist[i] = value;
            // }
            
            // var lastObstacleDistance = 0f;
            // var selectedObstacleDirection = -5;
            //
            // for (var i = 0; i < obstacleDist.Length; i++)
            // {
            //     if (obstacleDist[i]>lastObstacleDistance)
            //     {
            //         lastObstacleDistance = obstacleDist[i];
            //         selectedObstacleDirection = i;
            //     }
            // }
            
            var lastDistance = 0f;
            var selectedDirection = 0;

            for (var i = 0; i < distances.Length; i++)
            {
                // if (i== selectedObstacleDirection || i == (selectedObstacleDirection + 1)%16 || i== (selectedObstacleDirection - 1) || i == selectedObstacleDirection+15)
                // {
                //     continue;
                // }
                if (distances[i]>lastDistance)
                {
                    lastDistance = distances[i];
                    selectedDirection = i;
                }
            }

            for (var i = 0; i < angles.Length; i++)
            {
                // if (i== selectedObstacleDirection || i == (selectedObstacleDirection + 1)%16 || i== (selectedObstacleDirection - 1) || i == selectedObstacleDirection+15)
                // {
                //     Debug.DrawRay(transform.position, angles[i].normalized * obstacleDist[i], Color.red);
                //     continue;
                // }
                if (i == selectedDirection)
                {
                    print(distances[i]);
                    Debug.DrawRay(transform.position, angles[i].normalized * distances[i], Color.blue);
                    transform.position += (Vector3)angles[i].normalized * (speed * Time.deltaTime);
                }
                
                else
                {
                    Debug.DrawRay(transform.position, angles[i].normalized * distances[i], Color.green);
                }
            }
        }

        private float SetAngleToTarget(Vector2 angle, Vector2 target) 
        {
            var myAngle = (target - (Vector2)transform.position).normalized;
            var dotProduct = Vector2.Dot(angle.normalized, myAngle);
            
            dotProduct += (1 - dotProduct)/2;
            return dotProduct;
        }

        private float AddWeightToAngle(Vector2 angle, Vector2 target)
        {
            var myAngle = (target - (Vector2)transform.position).normalized;
            var dotProduct = Vector2.Dot(angle.normalized, myAngle);
            dotProduct *= -1;
            dotProduct += (1 - dotProduct)/2;

            //dotProduct = 1 - Math.Abs(dotProduct - 0.65f);

            return dotProduct;
        }

        // private float ObstacleCollisionEnter(Vector2 angle, Vector2 target)
        // {
        //     var myAngle = (target - (Vector2)transform.position).normalized;
        //     var dotProduct = Vector2.Dot(angle.normalized, myAngle);
        //     dotProduct *= -1;
        //     dotProduct += (1 - dotProduct)/2;
        //
        //     dotProduct = 1 - Math.Abs(dotProduct - 0.65f);
        //     return dotProduct;
        //
        // }

        private List<Transform> CheckObstacle(Collider2D myCollider)
        {
            var detectObstacles = new List<Collider2D>();
            var count = myCollider.OverlapCollider(filter2D, detectObstacles);
            var returnValue = new List<Transform>();

            for (var i = 0; i < detectObstacles.Count; i++)
            {
                returnValue.Add(detectObstacles[i].transform); 
                if (Vector2.Distance(transform.position, detectObstacles[i].transform.position)<Vector2.Distance(transform.position, _target.position))
                {
                    

                }
            }
            //
            // if (myCollider == obstacleCollisionCollider)
            // {
            //     var ray = Physics2D.Raycast(transform.position, _target.position-transform.position,
            //         Vector2.Distance(transform.position, _target.position), filter2D.layerMask);
            //
            //     if (ray.collider != null)
            //     {
            //         returnValue.Add(ray.collider.transform);
            //     }
            // }
            
            return returnValue;
        }
    }
}