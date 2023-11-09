using System;
using UnityEngine;

namespace Script
{
    public class Navigation : MonoBehaviour
    {
        public Transform player;
        public GameObject mark;
        [SerializeField] private LayerMask obstacleCheckLayer;
        private MoveController _moveController;
        private Collider2D _myCollider;
        private Vector2 _targetPos;

        private void Start()
        {
            _moveController = GetComponent<MoveController>();
            _myCollider = GetComponent<Collider2D>();
        }

        private void Update()
        {
            ObstacleCheck();
            _moveController.target = _targetPos;
            Debug.DrawRay(transform.position, ( _targetPos-(Vector2)transform.position)*Vector2.Distance(transform.position, _targetPos));
            mark.transform.position = _targetPos;
        }

        private void ObstacleCheck()
        {
            var ray = Physics2D.Raycast(transform.position, player.position - transform.position,
                Vector2.Distance(player.position, transform.position), obstacleCheckLayer);
            if (ray.collider == null)
            {
                _targetPos = player.position;
                return;
            }

          
            
            var obj = ray.collider;
            var bounds = obj.bounds;
            
            var i = (obj.transform.position - transform.position).normalized;
            
            var posA = obj.transform.position + new Vector3(i.y, i.x*-1) * (bounds.size.x/2 + _myCollider.bounds.size.x/2);
            var posB = obj.transform.position + new Vector3(i.y*-1, i.x)* (bounds.size.x/2 + _myCollider.bounds.size.x/2);

            _targetPos = SelectDirection(posA, posB);
            
            


            // if (transform.position.y>obj.bounds.size.y/2+obj.transform.position.y)
            // {
            //     if (transform.position.x>obj.bounds.size.x/2+obj.transform.position.x)
            //     {
            //         var bounds = obj.bounds;
            //         var position = obj.transform.position;
            //         var pos1 = (Vector2)position + new Vector2(bounds.size.x / 2*-1 + _myCollider.bounds.size.x/2*-1, bounds.size.y / 2+ _myCollider.bounds.size.y/2);
            //         var pos2 = (Vector2)position + new Vector2(bounds.size.x / 2, bounds.size.y / 2*-1 + _myCollider.bounds.size.y/2*-1);
            //         _targetPos = SelectDirection(pos1, pos2);
            //     }
            //     else if (transform.position.x<obj.bounds.size.x/2+obj.transform.position.x && transform.position.x>obj.transform.position.x-obj.bounds.size.x/2)
            //     {
            //         var bounds = obj.bounds;
            //         var position = obj.transform.position;
            //         var pos1 = (Vector2)position + new Vector2(bounds.size.x / 2, 0);
            //         var pos2 = (Vector2)position + new Vector2(bounds.size.x / 2*-1 + _myCollider.bounds.size.x/2*-1, 0);
            //         _targetPos = SelectDirection(pos1, pos2);
            //     }
            //     else
            //     {
            //         var bounds = obj.bounds;
            //         var position = obj.transform.position;
            //         var pos1 = (Vector2)position + new Vector2(bounds.size.x / 2, bounds.size.y / 2+ _myCollider.bounds.size.y/2);
            //         var pos2 = (Vector2)position + new Vector2(bounds.size.x / 2*-1 + _myCollider.bounds.size.x/2*-1, bounds.size.y / 2*-1 + _myCollider.bounds.size.y/2*-1);
            //         _targetPos = SelectDirection(pos1, pos2);
            //     }
            // }
            // else if (transform.position.y<obj.bounds.size.y/2+obj.transform.position.y && transform.position.y>obj.transform.position.y-obj.bounds.size.y/2 + _myCollider.bounds.size.y/2*-1)
            // {
            //     if (transform.position.x>obj.bounds.size.x/2+obj.transform.position.x)
            //     {
            //         var bounds = obj.bounds;
            //         var position = obj.transform.position;
            //         var pos1 = (Vector2)position + new Vector2(0, bounds.size.y / 2+ _myCollider.bounds.size.y/2);
            //         var pos2 = (Vector2)position + new Vector2(0, bounds.size.y / 2*-1 + _myCollider.bounds.size.y/2*-1);
            //         _targetPos = SelectDirection(pos1, pos2);
            //     }
            //     else
            //     {
            //         var bounds = obj.bounds;
            //         var position = obj.transform.position;
            //         var pos1 = (Vector2)position + new Vector2(0, bounds.size.y / 2+ _myCollider.bounds.size.y/2);
            //         var pos2 = (Vector2)position + new Vector2(0, bounds.size.y / 2*-1 + _myCollider.bounds.size.y/2*-1);
            //         _targetPos = SelectDirection(pos1, pos2);
            //     }
            // }
            // else
            // {
            //     if (transform.position.x>obj.bounds.size.x/2+obj.transform.position.x)
            //     {
            //         var bounds = obj.bounds;
            //         var position = obj.transform.position;
            //         var pos1 = (Vector2)position + new Vector2(bounds.size.x / 2, bounds.size.y / 2+ _myCollider.bounds.size.y/2);
            //         var pos2 = (Vector2)position + new Vector2(bounds.size.x / 2*-1 + _myCollider.bounds.size.x/2*-1, bounds.size.y / 2*-1 + _myCollider.bounds.size.y/2*-1);
            //         _targetPos = SelectDirection(pos1, pos2);
            //     }
            //     else if (transform.position.x<obj.bounds.size.x/2+obj.transform.position.x && transform.position.x>obj.transform.position.x-obj.bounds.size.x/2)
            //     {
            //         var bounds = obj.bounds;
            //         var position = obj.transform.position;
            //         var pos1 = (Vector2)position + new Vector2(bounds.size.x / 2, 0);
            //         var pos2 = (Vector2)position + new Vector2(bounds.size.x / 2*-1 + _myCollider.bounds.size.x/2*-1, 0);
            //         _targetPos = SelectDirection(pos1, pos2);
            //     }
            //     else
            //     {
            //         var bounds = obj.bounds;
            //         var position = obj.transform.position;
            //         var pos1 = (Vector2)position + new Vector2(bounds.size.x / 2*-1 + _myCollider.bounds.size.x/2*-1, bounds.size.y / 2);
            //         var pos2 = (Vector2)position + new Vector2(bounds.size.x / 2, bounds.size.y / 2*-1 + _myCollider.bounds.size.y/2*-1);
            //         _targetPos = SelectDirection(pos1, pos2);
            //     }
            // }
        }

        private Vector2 SelectDirection(Vector2 pos1, Vector2 pos2)
        {
            var distance1 = Vector2.Distance(transform.position, pos1) + Vector2.Distance(pos1, player.position);
            var distance2 = Vector2.Distance(transform.position, pos2) + Vector2.Distance(pos2, player.position);
            return distance1 < distance2 ? pos1 : pos2;
        }
    }
}