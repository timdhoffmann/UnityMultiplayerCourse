using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class LimbusNetworkManager : NetworkManager
{
    [SerializeField]
    private GameObject _unitSpawnerPrefab = null;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        GameObject unitSpawner = Instantiate(
            _unitSpawnerPrefab,
            conn.identity.transform.position,
            conn.identity.transform.rotation);
        NetworkServer.Spawn(unitSpawner, conn);
    }

    public override void Start()
    {
        base.Start();

        Assert.IsNotNull(_unitSpawnerPrefab);
    }
}