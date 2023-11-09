using UnityEngine;

namespace Script
{
    public class Navigation : MonoBehaviour
    {
        public Transform target;
        
        [SerializeField] private LayerMask obstacleCheckLayer;
        
        private MoveController _moveController;
        private Collider2D _myCollider;

        private void Start()
        {
            _moveController = GetComponent<MoveController>();
            _myCollider = GetComponent<Collider2D>();
        }

        private void Update()
        {
            _moveController.destination = SetByPassPoint();
            //방향 표시
            var position = transform.position;
            Debug.DrawRay(position, (_moveController.destination - (Vector2)position).normalized*Vector2.Distance(position, _moveController.destination));
        }

        //장애물 우회 지점 설정
        private Vector2 SetByPassPoint()
        {
            var thisPos = transform.position;
            var targetPos = target.position;
            var rayHit = Physics2D.Raycast(thisPos, targetPos - thisPos,
                Vector2.Distance(targetPos, thisPos), obstacleCheckLayer);
            
            if (!rayHit)
            {
                return target.position;
            }
            
            var obstacle = rayHit.collider;
            var obstacleBounds = obstacle.bounds;

            var obstaclePos = obstacle.transform.position;
            var distance = (obstaclePos - transform.position).normalized;

            var thisBounds = _myCollider.bounds;
            
            var posA = obstaclePos + new Vector3(distance.y, distance.x*-1) * (obstacleBounds.size.x/2 + thisBounds.size.x/2);
            var posB = obstaclePos + new Vector3(distance.y*-1, distance.x)* (obstacleBounds.size.x/2 + thisBounds.size.x/2);

            return SelectDirection(posA, posB);
        }

        //두 우회 지점 중 빠른 곳으로 목적지 설정
        private Vector2 SelectDirection(Vector2 pos1, Vector2 pos2)
        {
            var thisPos = transform.position;
            var targetPos = target.position;
            var distance1 = Vector2.Distance(thisPos, pos1) + Vector2.Distance(pos1, targetPos);
            var distance2 = Vector2.Distance(thisPos, pos2) + Vector2.Distance(pos2, targetPos);
            return distance1 < distance2 ? pos1 : pos2;
        }
    }
}