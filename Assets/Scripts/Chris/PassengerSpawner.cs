using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{
    [SerializeField] private Vector3 spawnLocationOffset = new Vector3(0, 0, 0); // Default to no offset

    private PlayerController playerController;
    public GameObject passengerPrefab;
    public int rows = 5;
    public int columns = 2;
    public float spacing = 1.0f; // Adjust the spacing to your preference
    private GameObject[,] passengers; // Use a 2D array to represent the grid
    private readonly Queue<Passenger> passengerPool = new Queue<Passenger>();

    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();

        if (playerController != null)
        {
            // Use the player's health to determine the number of passengers
            int health = playerController.Health;
            int rows = 5; // Assuming you still want to arrange them in 5 rows
            int columns = Mathf.CeilToInt((float)health / rows); // Calculate columns needed

            this.rows = rows;
            this.columns = columns;

            passengers = new GameObject[columns, rows]; // Initialize the array based on health
            SpawnPassengers(health, rows, columns);
        }
        else
        {
            Debug.LogError("PlayerController not found on parent GameObject");
        }
    }

    private void SpawnPassengers(int health, int rows, int columns)
    {
        float xOffset = -(columns * spacing / 2) + (spacing / 2); // Adjust this calculation as needed

        int passengerCount = 0; // Keep track of how many passengers have been instantiated

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (passengerCount >= health) // Stop spawning if we've hit the health count
                    return;

                // Apply the spawn location offset here
                Vector3 position = new Vector3(col * spacing + xOffset, row * -spacing, 0) + spawnLocationOffset;

                Passenger newPassenger = GetPassengerFromPool();
                newPassenger.transform.SetParent(transform, false);
                newPassenger.PrepareForSpawn(this, position, col == 0);
                passengers[col, row] = newPassenger.gameObject; // Store the passenger in the array

                passengerCount++;
            }
        }
    }

    private Passenger GetPassengerFromPool()
    {
        if (passengerPool.Count > 0)
        {
            return passengerPool.Dequeue();
        }

        GameObject newPassenger = Instantiate(passengerPrefab);
        return newPassenger.GetComponent<Passenger>();
    }


    // Call this method to make a passenger fall off
    public void MakePassengerFallOff()
    {
        // Find a passenger that is still active to fall off
        for (int i = 0; i < columns * rows; i++)
        {
            int col = i % columns;
            int row = i / columns;
            GameObject passenger = passengers[col, row];

            if (passenger != null && passenger.activeSelf)
            {
                passenger.GetComponent<Passenger>().FallOff();
                //passenger.GetComponent<BoxCollider2D>().enabled = false;
                passengers[col, row] = null; // Set the array element to null after making the passenger fall off
                break; // Only remove one passenger per call
            }
        }
    }

    public void RecyclePassenger(Passenger passenger)
    {
        if (passenger == null)
        {
            return;
        }

        passenger.gameObject.SetActive(false);
        passengerPool.Enqueue(passenger);
    }
}
