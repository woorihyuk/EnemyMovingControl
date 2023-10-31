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
        public Transform _target;
        public List<Transform> obstacles;
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
            _camera = Camera.main;
            //_target = transform.position;
        }
        // private void Update()
        // {
        //     _mouse = _camera.ScreenToWorldPoint(Input.mousePosition);
        //     _angle = Mathf.Atan2(_mouse.y - _target.y, _mouse.x - _target.x) * Mathf.Rad2Deg;
        //     var val = Quaternion.AngleAxis(_angle-90, Vector3.forward);
        //     transform.rotation = val;
        //     print(val.normalized);
        // }

        private void Update()
        {
            //var lastDistance = 0;
            foreach (var val in angles)
            {
                var value = SetAngleToTarget(val, _target.position);
                foreach (var obstacle in obstacles)
                {
                    value += AddWeightToAngle(val, obstacle.position);
                }
                Debug.DrawRay(transform.position, val.normalized * value, Color.green);
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
            dotProduct = 1 - Math.Abs(dotProduct - 0.65f);
            dotProduct += (1 - dotProduct)/2;
            return dotProduct;
        }
    }
} //1과 차이값의 반