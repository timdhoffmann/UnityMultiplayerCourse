using _Project.Players;
using Mirror;
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

        [SerializeField] private RectTransform _dragSelectionBoxTransform = null;

        private Player _player = null;
        private Camera _mainCamera = null;
        private Vector2 _dragStartPosition = Vector2.zero;

        // Start is called before the first frame update
        private void Start()
        {
            _mainCamera = Camera.main;
            Assert.IsNotNull(_mainCamera);

            //_player = NetworkClient.connection.identity.GetComponent<Player>();
            //Assert.IsNotNull(_player);

            Assert.IsNotNull(_dragSelectionBoxTransform, "Are you missing a reference in the inspector?");
        }

        // Update is called once per frame
        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                StartDragSelection();
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                CompleteDragSelection();
            }
            else if (Mouse.current.leftButton.isPressed)
            {
                UpdateDragSelection();
            }
        }

        private void StartDragSelection()
        {
            Debug.Log("starting selection");
            foreach (var selectedUnit in SelectedUnits)
            {
                selectedUnit.DeSelect();
            }
            SelectedUnits.Clear();

            _dragSelectionBoxTransform.gameObject.SetActive(true);

            _dragStartPosition = Mouse.current.position.ReadValue();

            UpdateDragSelection();
        }

        private void UpdateDragSelection()
        {
            Debug.Log("updating selection");

            Vector2 mousePosition = Mouse.current.position.ReadValue();
            var selectionWidth = mousePosition.x - _dragStartPosition.x;
            var selectionHeight = mousePosition.y - _dragStartPosition.y;

            _dragSelectionBoxTransform.sizeDelta = new Vector2(
                Mathf.Abs(selectionWidth), Mathf.Abs(selectionHeight)
            );

            _dragSelectionBoxTransform.anchoredPosition = _dragStartPosition + new Vector2(
                selectionWidth * 0.5f, selectionHeight * 0.5f
            );
        }

        private void CompleteDragSelection()
        {
            Debug.Log("completing selection");

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