using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Script
{
    public class MoveController : MonoBehaviour
    {
        public Vector2 destination;
        
        [SerializeField] private Collider2D obstacleCheckCollider;
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private float speed;

        private Collider2D _myCollider2D;
        private List<Transform> _obstacles;

        private ContactFilter2D _filter2D;
        private Vector2[] _angles;
        private Vector2 _mouse;
        
        private void Start()
        {
            _myCollider2D = GetComponent<Collider2D>();

            //방향
            _angles = new[]
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

            _filter2D = new ContactFilter2D()
            {
                useLayerMask = true,
                layerMask = targetLayer
            };
        }

        private void Update()
        {
            _obstacles = CheckObstacle(obstacleCheckCollider);
            var collisionObstacle = CheckObstacle(_myCollider2D);
            var distances = new float[_angles.Length];
            
            for (var i = 0; i < _angles.Length; i++)
            {
                distances[i] = SetAngleToTarget(_angles[i], destination);
            }
            foreach (var val in _obstacles)
            {
                for (var i = 0; i < _angles.Length; i++)
                {
                    distances[i] *= AddWeightToAngle(_angles[i], val.position);
                }

                distances = NormalizeDirection(distances);
            }

            foreach (var val in collisionObstacle)
            {
                for (var i = 0; i < _angles.Length; i++)
                {
                    distances[i] *= ObstacleCollisionEnter(_angles[i],val.position);
                }
            }

            var lastDistance = 0f;
            var selectedDirection = 0;

            for (var i = 0; i < distances.Length; i++)
            {
                if (distances[i] > lastDistance)
                {
                    lastDistance = distances[i];
                    selectedDirection = i;
                }
            }

            for (var i = 0; i < _angles.Length; i++)
            {
                if (i == selectedDirection)
                {
                    var position = transform.position;
                    Debug.DrawRay(position, _angles[i].normalized * distances[i], Color.blue);
                    position += (Vector3)_angles[i].normalized * (speed * Time.deltaTime);
                    transform.position = position;
                }

                else
                {
                    Debug.DrawRay(transform.position, _angles[i].normalized * distances[i], Color.green);
                }
            }
        }

        // 목적지로 가기에 적합한 방향 순으로 가중치 부여
        private float SetAngleToTarget(Vector2 angle, Vector2 target)
        {
            var myAngle = (target - (Vector2)transform.position).normalized;
            var dotProduct = Vector2.Dot(angle.normalized, myAngle);

            dotProduct += (1 - dotProduct) / 2;
            
            return dotProduct;
        }

        //가장 큰 방향을 1로 바꾸고 그에 맞게 다른 방향들의 크기 조절
        private static float[] NormalizeDirection(float[] distances)
        {
            var lastDistance = distances.Prepend(0f).Max();

            var weight = lastDistance / 1;
            for (var i = 0; i < distances.Length; i++)
            {
                distances[i] *= weight;
            }

            return distances;
        }
        
        //주위의 장애물로부터 멀어지도록 가중치 부여
        private float AddWeightToAngle(Vector2 angle, Vector2 target)
        {
            var thisPos = transform.position;
            var thisAngle = (target - (Vector2)thisPos).normalized;
            var dotProduct = Vector2.Dot(angle.normalized, thisAngle);
            
            dotProduct *= -1;
            dotProduct += (1 - dotProduct) / 2;
            dotProduct += (1 - dotProduct) - (1 - dotProduct) * (1 / Vector2.Distance(thisPos, target));         
            dotProduct = 1 - Math.Abs(dotProduct - 0.65f);
            
            return dotProduct;
        }

        //장애물과 충돌하지 않도록 가중치 부여
        private float ObstacleCollisionEnter(Vector2 angle, Vector2 target)
        {
            var myAngle = (target - (Vector2)transform.position).normalized;
            var dotProduct = Vector2.Dot(angle.normalized, myAngle);
            
            dotProduct *= -1;
            dotProduct += (1 - dotProduct)/2;
            
            return dotProduct;
        }

        // 범위 안에 있는 장애물 검출
        private List<Transform> CheckObstacle(Collider2D myCollider)
        {
            var detectObstacles = new List<Collider2D>();
            var obstaclesTransform = new List<Transform>();
            myCollider.OverlapCollider(_filter2D, detectObstacles);

            foreach (var val in detectObstacles)
            {
                obstaclesTransform.Add(val.transform);
                if (Vector2.Distance(transform.position, val.transform.position) <
                    Vector2.Distance(transform.position, destination))
                {
                }
            }
            
            return obstaclesTransform;
        }
    }
}