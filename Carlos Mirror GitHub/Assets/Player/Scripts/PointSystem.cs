using Mirror;
using UnityEngine;

public class PointSystem : NetworkBehaviour
{
    CurrentPoints currentPoints;
    ItemFeed[] itemFeed;
    [SerializeField] private CarlosSetup carlosSetup;
    [SerializeField] private string messageCallback;

    private void Update()
    {
        currentPoints = GameObject.FindGameObjectWithTag("PointHolder").GetComponent<CurrentPoints>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            itemFeed = FindObjectsOfType<ItemFeed>();
            foreach (ItemFeed item in itemFeed)
            {
                item.CmdItemFeedCallback(other.GetComponent<PlayerName>().playerName.text.ToString(), messageCallback + " x" + currentPoints.points);
            }

            carlosSetup.maxVelocity += 0.5f;
            currentPoints.points++;

            NetworkServer.Destroy(gameObject);
        }
    }
}
