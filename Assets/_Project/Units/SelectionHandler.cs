using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace _Project.Units
{
    public class SelectionHandler : MonoBehaviour
    {
        public List<Unit> SelectedUnits { get; } = new List<Unit>();

        [SerializeField]
        // TODO: What is this initialized to?
        private LayerMask _layerMask = new LayerMask();

        private Camera _mainCamera = null;

        // Start is called before the first frame update
        private void Start()
        {
            _mainCamera = Camera.main;
            Assert.IsNotNull(_mainCamera);
        }

        // Update is called once per frame
        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                StartAreaSelection();
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                CompleteAreaSelection();
            }
        }

        private void StartAreaSelection()
        {
            foreach (var selectedUnit in SelectedUnits)
            {
                selectedUnit.DeSelect();
            }
            SelectedUnits.Clear();
        }

        private void CompleteAreaSelection()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layerMask)) { return; }

            if (!hit.collider.TryGetComponent<Unit>(out Unit unit)) { return; }
            if (!unit.hasAuthority) { return; }

            SelectedUnits.Add(unit);

            foreach (var selectedUnit in SelectedUnits)
            {
                selectedUnit.Select();
            }
        }
    }
}