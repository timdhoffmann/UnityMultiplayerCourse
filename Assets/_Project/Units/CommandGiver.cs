using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace _Project.Units
{
    public class CommandGiver : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerMask = new LayerMask();

        private SelectionHandler _selectionHandler = null;
        private Camera _camera = null;

        // Called before the first frame update.
        private void Start()
        {
            _camera = Camera.main;
            Assert.IsNotNull(_camera);

            _selectionHandler = GetComponent<SelectionHandler>();
            Assert.IsNotNull(_selectionHandler);
        }

        // Called once per frame.
        private void Update()
        {
            if (!Mouse.current.rightButton.wasPressedThisFrame) { return; }

            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(
                    ray,
                    out RaycastHit hit,
                    maxDistance: Mathf.Infinity,
                    _layerMask
                ))
            { return; }

            TryMove(hit.point);
        }

        private void TryMove(Vector3 position)
        {
            foreach (var unit in _selectionHandler.SelectedUnits)
            {
                var mover = unit.GetComponent<Mover>();
                if (mover == null) { return; }
                mover.CmdMoveToPosition(position);
            }
        }
    }
}