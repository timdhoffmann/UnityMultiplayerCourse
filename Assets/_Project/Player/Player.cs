using _Project.Units;
using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Player
{
    public class Player : NetworkBehaviour
    {
        [SerializeField] private List<Unit> _ownedUnits = new List<Unit>();

        #region Server Methods

        public override void OnStartServer()
        {
            /// Event subscribtions.
            Unit.ServerOnUnitSpawned += ServerHandleUnitSpawned;
            Unit.ServerOnUnitDeSpawned += ServerHandleUnitDeSpawned;
        }

        public override void OnStopServer()
        {
            /// Event un-subscriptions.
            Unit.ServerOnUnitSpawned -= ServerHandleUnitSpawned;
            Unit.ServerOnUnitDeSpawned -= ServerHandleUnitDeSpawned;
        }

        private void ServerHandleUnitSpawned(Unit unit)
        {
            // Checks if the unit is this player's unit (are the unit and this player owned by
            // the same connection?).
            if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

            _ownedUnits.Add(unit);
        }

        private void ServerHandleUnitDeSpawned(Unit unit)
        {
            // Checks if the unit is this player's unit (are the unit and this player owned by
            // the same connection?).
            if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

            _ownedUnits.Remove(unit);
        }

        #endregion

        #region Client Methods

        public override void OnStartClient()
        {
            // Returns if we are the server.
            if (!isClientOnly) { return; }

            /// Event subscriptions.
            Unit.AuthorityOnUnitSpawned += AuthorityHandleUnitSpawned;
            Unit.AuthorityOnUnitSpawned += AuthorityHandleUnitDeSpawned;
        }

        public override void OnStopClient()
        {
            // Returns if we are the server.
            if (!isClientOnly) { return; }

            /// Event un-subscriptions.
            Unit.AuthorityOnUnitSpawned -= AuthorityHandleUnitSpawned;
            Unit.AuthorityOnUnitSpawned -= AuthorityHandleUnitDeSpawned;
        }

        private void AuthorityHandleUnitSpawned(Unit unit)
        {
            if (!hasAuthority) { return; }

            _ownedUnits.Add(unit);
        }

        private void AuthorityHandleUnitDeSpawned(Unit unit)
        {
            if (!hasAuthority) { return; }

            _ownedUnits.Remove(unit);
        }

        #endregion
    }
}