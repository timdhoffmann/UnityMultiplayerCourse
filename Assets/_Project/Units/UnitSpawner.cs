using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class UnitSpawner : NetworkBehaviour
{
    [SerializeField]
    private GameObject _unitPrefab = null;

    #region Server

    [Command]
    private void CmdSpawnUnit()
    {
        GameObject unit = Instantiate(_unitPrefab, Vector3.zero, Quaternion.identity);
        NetworkServer.Spawn(unit, connectionToClient);
        Debug.Log($"Spawned unit for client: {connectionToClient.connectionId.ToString()}");
    }

    #endregion

    #region Client

    public override void OnStartAuthority()
    {
        Assert.IsNotNull(_unitPrefab);
    }

    [ClientCallback]
    private void Update()
    {
        if (!hasAuthority) { return; }
        if (Mouse.current.leftButton.wasReleasedThisFrame && Keyboard.current.ctrlKey.isPressed)
        {
            CmdSpawnUnit();
        }
    }

    #endregion
}