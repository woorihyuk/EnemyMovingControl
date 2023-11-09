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
            
            SetAngleToTarget(ref distances, destination);
            AddWeightToAngle(ref distances);
            ObstacleCollisionEnter(ref distances, collisionObstacle);
            
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
        private void SetAngleToTarget(ref float[] distanceArray, Vector2 target)
        {
            for (var i = 0; i < _angles.Length; i++)
            {
                var targetDistance = (target - (Vector2)transform.position).normalized;
                var dotProduct = Vector2.Dot(_angles[i].normalized, targetDistance);
                dotProduct += (1 - dotProduct) / 2;
                distanceArray[i] = dotProduct;
            } 
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
        private void AddWeightToAngle(ref float[] distanceArray)
        {
            foreach (var val in _obstacles)
            {
                for (var i = 0; i < _angles.Length; i++)
                {
                    var thisPos = transform.position;
                    var obstaclePos = val.position;
                    var thisAngle = (obstaclePos - thisPos).normalized;
                    var dotProduct = Vector2.Dot(_angles[i].normalized, thisAngle);
            
                    dotProduct *= -1;
                    dotProduct += (1 - dotProduct) / 2;
                    dotProduct += (1 - dotProduct) - (1 - dotProduct) * (1 / Vector2.Distance(thisPos, obstaclePos));         
                    dotProduct = 1 - Math.Abs(dotProduct - 0.65f);


                    distanceArray[i] *= dotProduct;
                }
                distanceArray = NormalizeDirection(distanceArray);
            }
            
            
            //var thisPos = transform.position;
            //var thisAngle = (target - (Vector2)thisPos).normalized;
            //var dotProduct = Vector2.Dot(angle.normalized, thisAngle);
        }

        //장애물과 충돌하지 않도록 가중치 부여
        private void ObstacleCollisionEnter(ref float[] distanceArray, List<Transform> collisionObstacle)
        {
            foreach (var val in collisionObstacle)
            {
                
                
                for (var i = 0; i < _angles.Length; i++)
                {
                    var myAngle = (val.position - transform.position).normalized;
                    var dotProduct = Vector2.Dot(_angles[i].normalized, myAngle);
            
                    dotProduct *= -1;
                    dotProduct += (1 - dotProduct)/2;
                    distanceArray[i] *= dotProduct;
                }
            }
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