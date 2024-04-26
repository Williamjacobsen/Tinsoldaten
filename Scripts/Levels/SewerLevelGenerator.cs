using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Generates random sewer routes
/// by spawning "blocks", "keys", "rats" and a "gate"
/// </summary>
public class SewerLevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] BlockPrefabs;
    [SerializeField] private GameObject RatPrefab;
    [SerializeField] private GameObject Player;

    private void Awake()
    {
        Time.timeScale = 1;
        CreateWorld();
    }

    private void CreateWorld()
    {
        try 
        {        
            // first sewer block is at (0, 0) / the center 
            // (vector2 list of positions)
            List<Vector2> blocks = new(){ new Vector2(0, 0) };
            
            GenerateRandomPath(blocks: blocks, newPos: new(0, 0));
            GenerateRandomPath(blocks: blocks, newPos: new(0, 0));
            GenerateRandomPath(blocks: blocks, newPos: blocks[^25]);

            SpawnBlocks(blocks);

            if (!IsValidGeneration())
            {
                endBlocks = new();
                DeleteEnvironment();
                CreateWorld();
                return;
            }

            SortEndBlockPositions();
            SpawnKeys();

            SpawnGate(blocks);

            SelectionSortDistanceBased(blocks);
            SpawnRats(blocks);
        }
        catch (Exception) 
        {
            CreateWorld();
        }
    }

    private void DeleteEnvironment()
    {
        for (int i = 0; i < transform.childCount; i++) 
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private List<Vector2> endBlocks = new();
    private bool IsValidGeneration()
    {
        if (endBlocks.Count < 5)
        {
            return false;
        }

        return true;
    }

    private void SpawnRats(List<Vector2> blocks)
    {
        int amountOfSpawningRats = blocks.Count / 4;

        for (int i = 0; i < amountOfSpawningRats; i++)
        {
            SpawnSingleRat(blocks);
        }

        StartCoroutine(ContinueSpawningRats(blocks));
    }

    List<Vector2> ratSpawnedPositions = new();

    private void SpawnSingleRat(List<Vector2> blocks, int minDistance = 5)
    {
        System.Random rndNum = new();

        Vector2 block = blocks[rndNum.Next(minDistance, blocks.Count)];

        foreach (Vector2 endBlock in endBlocks)
        {
            if (endBlock == block)
            {
                return;
            }
        }

        foreach (Vector2 spawnPos in ratSpawnedPositions)
        {
            if (spawnPos == block)
            {
                return;
            }
        }

        ratSpawnedPositions.Add(block);

        Instantiate(RatPrefab, block, Quaternion.identity);
    }

    private int CountRatsAlive()
    {
        return GameObject.FindGameObjectsWithTag("Rat").Length;
    }

    private IEnumerator ContinueSpawningRats(List<Vector2> blocks)
    {
        int initialAmountOfRatsAlive = CountRatsAlive();

        while (true)
        {
            yield return new WaitForSeconds(2);
            int amountOfRatsAlive = CountRatsAlive();
            if (initialAmountOfRatsAlive > amountOfRatsAlive)
            {
                SelectionSortDistanceBased(blocks);
                SpawnSingleRat(blocks, 10);
            }
        }
    }
    
    [SerializeField] private GameObject KingRatPrefab;
    private void SpawnKingRat(Vector2 spawnPos)
    {
        Instantiate(KingRatPrefab, spawnPos, Quaternion.identity);
    }

    private void GenerateRandomPath(List<Vector2> blocks, Vector2 newPos)
    {
        System.Random rndNum = new();

        int direction = rndNum.Next(4);
        int prevDirection = 0;
        int lastDirectionChange = 0;

        for (int i = 0; i < 25; i++)
        {
            // so it coninutes in same direction for atleast 3 blocks
            if (lastDirectionChange > 2)
            {
                // get random direction
                direction = rndNum.Next(4);

                // make odds of same direction twice as likely
                for (int j = 0; j < 2; j++)
                {
                    if (direction != prevDirection)
                    {
                        direction = rndNum.Next(4);
                    }
                }

                // if changed, reset counter
                if (direction != prevDirection)
                {
                    lastDirectionChange = 0;
                }
            }

            lastDirectionChange++;

            // add coordinates in direction
            if (direction == 0)
            {
                newPos = new Vector2(newPos.x + 10, newPos.y);
            }
            else if (direction == 1)
            {
                newPos = new Vector2(newPos.x - 10, newPos.y);
            }
            else if (direction == 2)
            {
                newPos = new Vector2(newPos.x, newPos.y + 10);
            }
            else if (direction == 3)
            {
                newPos = new Vector2(newPos.x, newPos.y - 10);
            }

            // if there already exists a block in current position
            bool state = false;
            foreach (Vector2 block in blocks)
            {
                if (block == newPos)
                {
                    state = true;
                }
            }
            
            // dont spawn if there already exists a block in current position
            if (!state)
            {
                // add block
                blocks.Add(newPos);
                prevDirection = direction;
            }
        }
    }

    private void SpawnBlocks(List<Vector2> blocks)
    {
        Vector2 prevPosition = Vector2.zero;

        foreach (Vector2 block in blocks)
        {
            bool blockRight = false;
            bool blockLeft = false;
            bool blockAbove = false;
            bool blockBelow = false;
            foreach (Vector2 blockPos in blocks)
            {
                if (blockPos.x == block.x + 10 && blockPos.y == block.y)
                {
                    blockRight = true;
                }
                else if (blockPos.x == block.x - 10 && blockPos.y == block.y)
                {
                    blockLeft = true;
                }
                else if (blockPos.x == block.x && blockPos.y == block.y + 10)
                {
                    blockAbove = true;
                }
                else if (blockPos.x == block.x && blockPos.y == block.y - 10)
                {
                    blockBelow = true;
                }
            }

            GameObject prefab = null;

            if (blockRight && blockLeft && blockAbove && blockBelow)
            {
                prefab = Instantiate(BlockPrefabs[2]);
            }
            else if (blockLeft && blockAbove && blockBelow)
            {
                prefab = Instantiate(BlockPrefabs[3]);
            }
            else if (blockRight && blockAbove && blockBelow)
            {
                prefab = Instantiate(BlockPrefabs[3]);
                prefab.transform.Rotate(new Vector3(0, 0, 180));
            }
            else if (blockRight && blockLeft && blockAbove)
            {
                prefab = Instantiate(BlockPrefabs[3]);
                prefab.transform.Rotate(new Vector3(0, 0, -90));
            }
            else if (blockRight && blockLeft && blockBelow)
            {
                prefab = Instantiate(BlockPrefabs[3]);
                prefab.transform.Rotate(new Vector3(0, 0, 90));
            }
            else if (blockAbove && blockBelow)
            {
                prefab = Instantiate(BlockPrefabs[1]);
            }
            else if (blockRight && blockLeft)
            {
                prefab = Instantiate(BlockPrefabs[1]);
                prefab.transform.Rotate(new Vector3(0, 0, 90));
            }
            else if (blockRight && blockAbove)
            {
                prefab = Instantiate(BlockPrefabs[4]);
                prefab.transform.Rotate(new Vector3(0, 0, 180));
            }
            else if (blockLeft && blockAbove)
            {
                prefab = Instantiate(BlockPrefabs[4]);
                prefab.transform.Rotate(new Vector3(0, 0, -90));
            }
            else if (blockRight && blockBelow)
            {
                prefab = Instantiate(BlockPrefabs[4]);
                prefab.transform.Rotate(new Vector3(0, 0, 90));
            }
            else if (blockLeft && blockBelow)
            {
                prefab = Instantiate(BlockPrefabs[4]);
            }
            else if (blockRight)
            {
                prefab = Instantiate(BlockPrefabs[0]);
                prefab.transform.Rotate(new Vector3(0, 0, 90));
            }
            else if (blockLeft)
            {
                prefab = Instantiate(BlockPrefabs[0]);
                prefab.transform.Rotate(new Vector3(0, 0, -90));
            }
            else if (blockAbove)
            {
                prefab = Instantiate(BlockPrefabs[0]);
                prefab.transform.Rotate(new Vector3(0, 0, 180));
            }
            else if (blockBelow)
            {
                prefab = Instantiate(BlockPrefabs[0]);
            }
            
            prefab.transform.position = block;
            HandleTransformProps(child: prefab);

            prevPosition = block;
        }
    }

    private void SortEndBlockPositions()
    {
        /*List<double> endBlockDistances = new();

        foreach (Vector2 endBlock in endBlocks)
        {
            endBlockDistances.Add(Math.Sqrt(Math.Pow(endBlock.x, 2) + Math.Pow(endBlock.y, 2)));
        }

        // selection sort algorithm
        for (int i = 0; i < endBlockDistances.Count; i++)
        {
            int minIndex = i;
            for (int j = i + 1; j < endBlockDistances.Count; j++)
            {
                if (endBlockDistances[j] < endBlockDistances[minIndex])
                {
                    minIndex = j;
                }
            }
            (endBlocks[minIndex], endBlocks[i]) = (endBlocks[i], endBlocks[minIndex]);
            (endBlockDistances[minIndex], endBlockDistances[i]) = (endBlockDistances[i], endBlockDistances[minIndex]);
        }*/

        SelectionSortDistanceBased(endBlocks);
    }

    private void SelectionSortDistanceBased(List<Vector2> array)
    {
        List<double> distances = new();

        foreach (Vector2 vector in array)
        {
            distances.Add(Vector2.Distance(vector, (Vector2)Player.transform.position));
        }

        // selection sort algorithm
        for (int i = 0; i < distances.Count; i++)
        {
            int minIndex = i;
            for (int j = i + 1; j < distances.Count; j++)
            {
                if (distances[j] < distances[minIndex])
                {
                    minIndex = j;
                }
            }
            (array[minIndex], array[i]) = (array[i], array[minIndex]);
            (distances[minIndex], distances[i]) = (distances[i], distances[minIndex]);
        }
    }

    [SerializeField] private GameObject Gate; 
    private void SpawnGate(List<Vector2> blocks)
    {
        Vector2 pos = endBlocks[^1];
        GameObject gate = Instantiate(Gate, pos, Quaternion.identity);

        foreach (Vector2 block in blocks)
        {
            if (
                (block.x == pos.x && block.y + 10 == pos.y)
                ||
                (block.x == pos.x && block.y - 10 == pos.y)
            )
            {
                gate.transform.Rotate(new Vector3(0, 0, 90));
            }
        }

        Vector2 kingRatPos = new(); 
        foreach (Vector2 block in blocks)
        {
            if (block.y == pos.y && block.x == pos.x + 10)
            {
                kingRatPos = block;
                break;
            }
            else if (block.y == pos.y && block.x == pos.x - 10)
            {
                kingRatPos = block;
                break;
            }
            else if (block.x == pos.x && block.y == pos.y + 10)
            {
                kingRatPos = block;
                break;
            }
            else if (block.x == pos.x && block.y == pos.y - 10)
            {
                kingRatPos = block;
                break;
            }
        }

        StartCoroutine(WaitForSpawningKingRat(kingRatPos));
        //Instantiate(Key, kingRatPos, Quaternion.identity);
    }

    private IEnumerator WaitForSpawningKingRat(Vector2 pos)
    {
        while (PlayerStats.keys != 3)
        {
            yield return new WaitForSeconds(1);
        }
        SpawnKingRat(pos);
    }

    [SerializeField] private GameObject Key;
    private void SpawnKeys()
    {   
        Instantiate(Key, endBlocks[^2], Quaternion.identity);
        Instantiate(Key, endBlocks[^3], Quaternion.identity);
        Instantiate(Key, endBlocks[^4], Quaternion.identity);
    }

    private void HandleTransformProps(GameObject child)
    {
        if (child.name == "End(Clone)")
        {
            endBlocks.Add(new Vector2(child.transform.position.x, child.transform.position.y));
        }

        // make "block" child of "enviorment" and change name from "Block(clone)" to "Block (x, y)"
        child.transform.SetParent(transform);
        child.name = $"{child.name.Replace("(Clone)", "")} ({child.transform.position.x},{child.transform.position.y})";
    }
}

