using Mirror;
using UnityEngine;

public class PointSystem : NetworkBehaviour {
    CurrentPoints currentPoints;
    ItemFeed[] itemFeed;
    [SerializeField] private CarlosSetup carlosSetup;
    [SerializeField] private string messageCallback;

    private void Update() {
        currentPoints = GameObject.FindGameObjectWithTag("PointHolder").GetComponent<CurrentPoints>();
    }

    private void OnTriggerEnter(Collider other) {
        if ( other.gameObject.CompareTag("Player") ) {
            currentPoints.points++;
            NetworkServer.Destroy(gameObject);
        }
    }
}