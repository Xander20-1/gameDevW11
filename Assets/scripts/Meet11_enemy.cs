using System.Collections.Generic;
using UnityEngine;
public class Meet11_enemy : MonoBehaviour
{
    public float speed = 2f;
    public Pathfinding pathfinder;
    List<Node> path;
    int targetIndex;
    void Start()
    {
        InvokeRepeating("UpdatePath", 0f, 1f);
    }

    void UpdatePath()
    {
        if (pathfinder == null) return;

        pathfinder.FindPath(
            transform.position,
            pathfinder.target.position
        );

        path = pathfinder.GetComponent<GridManager>().path;
        targetIndex = 0;
    }

    void Update()
    {
        if (path == null || path.Count == 0) return;
        if (targetIndex < path.Count)
        {
            Vector3 targetPos = path[targetIndex].worldPosition;
            Vector3 moveDir = (targetPos - transform.position).normalized;
            moveDir.y = 0;
            if (moveDir != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation,
                Time.deltaTime * 10f);
            }
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetPos,
            speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPos) < 0.3f)
            {
                targetIndex++;
            }
        }
    }
}