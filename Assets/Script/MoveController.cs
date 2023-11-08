using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class MoveController : MonoBehaviour
    {
        // private float _angle;
        public Collider2D obstacleCheckCollider;
        private Collider2D _collider;
        public Vector2 target;
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
            _collider = GetComponent<Collider2D>();

            angles = new[]
            {
                new Vector2(0, 1),
                new Vector2(0.2f, 1),
                new Vector2(0.4f, 1),
                new Vector2(0.6f, 1),
                new Vector2(1, 1),
                new Vector2(1, 0.6f),
                new Vector2(1, 0.4f),
                new Vector2(1, 0.2f),
                new Vector2(1, 0),
                new Vector2(1, -0.2f),
                new Vector2(1, -0.4f),
                new Vector2(1, -0.6f),
                new Vector2(1, -1),
                new Vector2(0.6f, -1),
                new Vector2(0.4f, -1),
                new Vector2(0.2f, -1),
                new Vector2(0, -1),
                new Vector2(-0.2f, -1),
                new Vector2(-0.4f, -1),
                new Vector2(-0.6f, -1),
                new Vector2(-1, -1),
                new Vector2(-1, -0.6f),
                new Vector2(-1, -0.4f),
                new Vector2(-1, -0.2f),
                new Vector2(-1, 0),
                new Vector2(-1, 0.2f),
                new Vector2(-1, 0.4f),
                new Vector2(-1, 0.6f),
                new Vector2(-1, 1),
                new Vector2(-0.6f, 1),
                new Vector2(-0.4f, 1),
                new Vector2(-0.2f, 1)
            };
        }

        private void Update()
        {
            print(target);
            obstacles = CheckObstacle(obstacleCheckCollider);
            //var collisionObstacle = CheckObstacle(obstacleCollisionCollider);
            var distances = new float[angles.Length];
            var obstacleDist = new float[angles.Length];


            // for (var i = 0; i < angles.Length; i++)
            // {
            //     var value = 0f;
            //
            //     foreach (var obstacle in obstacles)
            //     {
            //         value = ob(angles[i], obstacle.position);//-value;
            //     }
            //
            //     value /= obstacles.Count;
            //     obstacleDist[i] = value;
            // }
            //
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
            //
            // print(selectedObstacleDirection);
            //
            for (var i = 0; i < angles.Length; i++)
            {
                distances[i] = SetAngleToTarget(angles[i], target);
            }

            foreach (var val in obstacles)
            {
                for (var i = 0; i < angles.Length; i++)
                {
                    distances[i] *= AddWeightToAngle(angles[i], val.position);
                }

                distances = SelectBestDistance(distances);
            }
            // for (var i = 0; i < angles.Length; i++)
            // {
            //     var value = SetAngleToTarget( angles[i], _target.position);
            //     foreach (var val in obstacles)
            //     {
            //         // if (i== selectedObstacleDirection || i == (selectedObstacleDirection + 1)%angles.Length || i== (selectedObstacleDirection - 1) || i == selectedObstacleDirection+angles.Length-1)
            //         // {
            //         //     
            //         // }
            //         //print(AddWeightToAngle(angles[i], val.position));
            //         value *=  AddWeightToAngle(angles[i], val.position); 
            //         
            //     }
            //     distances[i] = value;
            // }

            var lastDistance = 0f;
            var selectedDirection = 0;

            for (var i = 0; i < distances.Length; i++)
            {
                // if (i== selectedObstacleDirection || i == (selectedObstacleDirection + 1)%16 || i== (selectedObstacleDirection - 1) || i == selectedObstacleDirection+15)
                // {
                //     continue;
                // }
                if (distances[i] > lastDistance)
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

            dotProduct += (1 - dotProduct) / 2;
            return dotProduct;
        }

        private float ob(Vector2 angle, Vector2 target)
        {
            var myAngle = (target - (Vector2)transform.position).normalized;
            var dotProduct = Vector2.Dot(angle.normalized, myAngle);
            //dotProduct *= -1;
            dotProduct += (1 - dotProduct) / 2;

            //dotProduct = 1 - Math.Abs(dotProduct - 0.65f);

            return dotProduct;
        }

        private float[] SelectBestDistance(float[] distances)
        {
            var lastDistance = 0f;
            var selectedDirection = 0;

            for (var i = 0; i < distances.Length; i++)
            {
                // if (i== selectedObstacleDirection || i == (selectedObstacleDirection + 1)%16 || i== (selectedObstacleDirection - 1) || i == selectedObstacleDirection+15)
                // {
                //     continue;
                // }
                if (distances[i] > lastDistance)
                {
                    lastDistance = distances[i];
                    selectedDirection = i;
                }
            }

            var weight = lastDistance / 1;
            for (var i = 0; i < distances.Length; i++)
            {
                distances[i] *= weight;
            }

            return distances;
        }

        private float AddWeightToAngle(Vector2 angle, Vector2 target)
        {
            var myAngle = (target - (Vector2)transform.position).normalized;
            var dotProduct = Vector2.Dot(angle.normalized, myAngle);
            dotProduct *= -1;
            dotProduct += (1 - dotProduct) / 2;
            dotProduct *= 1 / (obstacleCheckCollider.bounds.size.x - _collider.bounds.size.x) *
                          (obstacleCheckCollider.bounds.size.x -
                           Vector2.Distance(transform.position,
                               target)-1);
            print(1 / (obstacleCheckCollider.bounds.size.x - _collider.bounds.size.x));
            print(((obstacleCheckCollider.bounds.size.x -
                Vector2.Distance(transform.position,
                    target))));
            print($"dis{Vector2.Distance(transform.position, target)}");
            print(1 / (obstacleCheckCollider.bounds.size.x - _collider.bounds.size.x) *
                  (obstacleCheckCollider.bounds.size.x -
                   Vector2.Distance(transform.position,
                       target)-1));
            //가까울수록 가충치 크게 적용
            //dotProduct += (1 - dotProduct) * (1/ (myCollider.bounds.size.x / 2) -                  / Vector2.Distance(transform.position, this.target));
            // 1 / (b - a) * (x - a)
            //dotProduct += (1 - dotProduct) - (1 - dotProduct) * (1 / Vector2.Distance(transform.position, target));
            //dotProduct = 1 - Math.Abs(dotProduct - 0.65f);
            //value 가 가까울수록 작아져야함
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
            detectObstacles.Remove(myCollider);
            detectObstacles.Remove(_collider);

            var returnValue = new List<Transform>();

            for (var i = 0; i < detectObstacles.Count; i++)
            {
                returnValue.Add(detectObstacles[i].transform);
                if (Vector2.Distance(transform.position, detectObstacles[i].transform.position) <
                    Vector2.Distance(transform.position, target))
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

            returnValue.Remove(obstacleCheckCollider.transform);
            returnValue.Remove(myCollider.transform);
            return returnValue;
        }
    }
}