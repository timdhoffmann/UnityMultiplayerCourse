using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Units
{
    public class Unit : NetworkBehaviour
    {
        [SerializeField]
        private UnityEvent _onSelected = null;

        [SerializeField]
        private UnityEvent _onDeselected = null;

        #region Client Methods

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