using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Units
{
    public class Unit : NetworkBehaviour
    {
        #region Events

        public static event Action<Unit> AuthorityOnUnitSpawned;
        public static event Action<Unit> AuthorityOnUnitDeSpawned;
        public static event Action<Unit> ServerOnUnitSpawned;
        public static event Action<Unit> ServerOnUnitDeSpawned;

        #endregion

        [SerializeField]
        private UnityEvent _onSelected = null;
        [SerializeField]
        private UnityEvent _onDeselected = null;

        #region Server Methods

        public override void OnStartServer()
        {
            ServerOnUnitSpawned?.Invoke(this);
        }

        public override void OnStopServer()
        {
            ServerOnUnitDeSpawned?.Invoke(this);
        }

        #endregion

        #region Client Methods

        public override void OnStartClient()
        {
            // Returns if we are the server or don't have authority.
            if (!isClientOnly || !hasAuthority) { return; }
            AuthorityOnUnitSpawned?.Invoke(this);
        }

        public override void OnStopClient()
        {
            // Returns if we are the server or don't have authority.
            if (!isClientOnly || !hasAuthority) { return; }
            AuthorityOnUnitDeSpawned?.Invoke(this);
        }

        [Client]
        public void Select()
        {
            if (!hasAuthority) { return; }

            _onSelected?.Invoke();
            Debug.Log("selected");
        }

        [Client]
        public void DeSelect()
        {
            if (!hasAuthority) { return; }

            _onDeselected?.Invoke();
            Debug.Log("de-selected");
        }

        #endregion
    }
}