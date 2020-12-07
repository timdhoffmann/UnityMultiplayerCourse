using Mirror;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace _Project.Units
{
    public class Mover : NetworkBehaviour
    {
        #region Fields

        private NavMeshAgent _navMeshAgent = null;

        #endregion

        #region Server Methods

        [Command]
        public void CmdMoveToPosition(Vector3 position)
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

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            Assert.IsNotNull(_navMeshAgent);
        }
    }
}