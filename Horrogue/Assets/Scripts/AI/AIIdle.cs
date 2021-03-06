﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIIdle : StateMachineBehaviour {
    #region Public Variables
    public float MoveChance = 0.025f;
    public float MoveDistance = 3f;
    public float ChanceForBigMovements = 0.25f;
    public float TimeBetweenMovements = 2f;
    #endregion

    #region Private Variables
    private float lastMoveTime;
    private bool isMoving;
    private AIDestinationSetter destinationSetter;
    private AIPath aiPath;
    #endregion

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        aiPath = animator.gameObject.GetComponent<AIPath>();
        destinationSetter = animator.gameObject.GetComponent<AIDestinationSetter>();

        lastMoveTime = Time.timeSinceLevelLoad;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (destinationSetter == null || aiPath == null) return;

        if (aiPath.reachedEndOfPath && isMoving) {
            animator.SetBool("Reached Destination", true);
            isMoving = false;
            lastMoveTime = Time.timeSinceLevelLoad;
        } else {
            if (MoveChance > Random.Range(0f,1f) && (Time.timeSinceLevelLoad - lastMoveTime) > TimeBetweenMovements && !isMoving) {
                NNInfo closest;

                if (ChanceForBigMovements > Random.Range(0f, 1f)) {
                    closest = GetRandomNode();
                } else {
                    var graph = AstarPath.active.data.gridGraph;
                    closest = AstarPath.active.GetNearest(animator.gameObject.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) * MoveDistance);
                }

                Seeker seeker = animator.gameObject.GetComponent<Seeker>();
                seeker.StartPath(animator.gameObject.transform.position, closest.position);

                isMoving = true;
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        destinationSetter.target = null;
    }

    NNInfo GetRandomNode ()  {
        var graph = AstarPath.active.data.gridGraph;
        return AstarPath.active.GetNearest((Vector2)graph.center + graph.size * 0.5f * Random.Range(-1f,1f));
    }
}
