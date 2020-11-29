using Mirror;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace _Project.Player
{
    public class Mover : NetworkBehaviour
    {
        #region Fields

        private NavMeshAgent _navMeshAgent = null;
        private Camera _mainCamera = null;

        #endregion

        #region Server Methods

        [Command]
        private void CmdMoveToPosition(Vector3 position)
        {
            if (!NavMesh.SamplePosition(
                sourcePosition: position,
                hit: out NavMeshHit hit,
                maxDistance: 1.0f,
                areaMask: NavMesh.AllAreas))
            {
                return;
            }
            _navMeshAgent.SetDestination(hit.position);
        }

        #endregion

        #region Client Methods

        public override void OnStartAuthority()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            Assert.IsNotNull(_navMeshAgent);

            _mainCamera = Camera.main;
            Assert.IsNotNull(_mainCamera);
        }

        [ClientCallback]
        private void Update()
        {
            if (!hasAuthority) { return; }
            if (!Mouse.current.rightButton.wasPressedThisFrame) { return; }

            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(
                ray: ray,
                hitInfo: out RaycastHit hitInfo,
                maxDistance: Mathf.Infinity
                ))
            { return; }
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1.0f);

            CmdMoveToPosition(hitInfo.point);
        }

        #endregion
    }
}