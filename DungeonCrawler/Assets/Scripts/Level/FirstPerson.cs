using System.Collections.Generic;
using UnityEngine;

public class FirstPerson : MonoBehaviour
{
    public List<UnityEngine.Tilemaps.Tilemap> blockTilemaps = new List<UnityEngine.Tilemaps.Tilemap>();
    //public UnityEngine.Tilemaps.Tilemap blockTilemap;
    public UnityEngine.Tilemaps.Tilemap billboardTilemap;

    public PlayerMovement playerMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("building tilemap");
        if (blockTilemaps.Count == 0 && billboardTilemap == null)
        {
            var parent = FindFirstObjectByType<Grid>();
            var children = parent.GetComponentsInChildren<UnityEngine.Tilemaps.Tilemap>();
            foreach (var child in children)
            {
                if (child.gameObject.name.ToLower().Contains("block")) blockTilemaps.Add(child);
                if (child.gameObject.name.ToLower() == "bilboard") billboardTilemap = child;
                if (child.gameObject.name.ToLower() == "billboard") billboardTilemap = child;
            }
        }

        if (playerMovement == null) playerMovement = FindFirstObjectByType<PlayerMovement>();

        var root = new GameObject();
        root.name = "FirstPersonWorldRoot";

        foreach (UnityEngine.Tilemaps.Tilemap blockTilemap in blockTilemaps) 
        {
            float zCoord = blockTilemap.gameObject.transform.position.z;
            for (int i = blockTilemap.cellBounds.xMin; i < blockTilemap.cellBounds.xMax; i++)
            {
                for (int j = blockTilemap.cellBounds.yMin; j < blockTilemap.cellBounds.yMax; j++)
                {
                    var pos = new Vector3Int(i, j, 0);

                    if (blockTilemap.GetSprite(pos) != null)
                    {
                        if (blockTilemap.GetSprite(pos + Vector3Int.up) == null)
                        {
                            var bf = generateBlockface(blockTilemap, pos, Vector3Int.up, zCoord);
                            bf.transform.parent = root.transform;
                        }
                        if (blockTilemap.GetSprite(pos + Vector3Int.down) == null)
                        {
                            var bf = generateBlockface(blockTilemap, pos, Vector3Int.down, zCoord);
                            bf.transform.parent = root.transform;
                        }
                        if (blockTilemap.GetSprite(pos + Vector3Int.left) == null)
                        {
                            var bf = generateBlockface(blockTilemap, pos, Vector3Int.left, zCoord);
                            bf.transform.parent = root.transform;
                        }
                        if (blockTilemap.GetSprite(pos + Vector3Int.right) == null)
                        {
                            var bf = generateBlockface(blockTilemap, pos, Vector3Int.right, zCoord);
                            bf.transform.parent = root.transform;
                        }
                    }
                }
            }
        }
        

        for (int i = billboardTilemap.cellBounds.xMin; i < billboardTilemap.cellBounds.xMax; i++) 
        {
            for (int j = billboardTilemap.cellBounds.yMin; j < billboardTilemap.cellBounds.yMax; j++) 
            {
                var pos = new Vector3Int(i, j, 0);

                if (billboardTilemap.GetSprite(pos) != null) 
                {
                    var billboard = new GameObject();
                    billboard.transform.parent = root.transform;
                    billboard.transform.position = billboardTilemap.CellToWorld(pos) + billboardTilemap.cellSize * 0.5f;

                    billboard.name = "Billboard_" + billboardTilemap.GetSprite(pos).name;

                    var behavior = billboard.AddComponent<Billboard>();
                    behavior.whatToFace = this.transform;

                    var renderer = billboard.AddComponent<SpriteRenderer>();
                    renderer.sprite = billboardTilemap.GetSprite(pos);

                    billboard.layer = 8; // Only viewable by 1st person camera

                    //Debug.Log(pos);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.zero;
        this.transform.LookAt(Vector3.right, Vector3.back);
        this.transform.RotateAround(Vector3.zero, Vector3.back, playerMovement.rotation);
        this.transform.position = new Vector3(playerMovement.transform.position.x, playerMovement.transform.position.y, 0f);
    }

    GameObject generateBlockface(UnityEngine.Tilemaps.Tilemap blockTilemap, Vector3Int pos, Vector3Int facing, float zCoord = 0) 
    {
        var blockFace = new GameObject();

        blockFace.name = "Blockface_" + blockTilemap.GetSprite(pos).name;

        blockFace.transform.position = Vector3.zero;
        blockFace.transform.LookAt((Vector3)facing, new Vector3(0, 0, -1));

        // Why the hell does unity just not do anything with this line
        //blockFace.transform.position.Scale(blockTilemap.cellSize * 0.5f);

        Vector3 position = facing;
        position.Scale(blockTilemap.cellSize * 0.5f);
        position += blockTilemap.CellToWorld(pos) + blockTilemap.cellSize * 0.5f;
        blockFace.transform.position = position + new Vector3(0, 0, zCoord);

        var renderer = blockFace.AddComponent<SpriteRenderer>();
        renderer.sprite = blockTilemap.GetSprite(pos );

        blockFace.layer = 8; // Only viewable by 1st person camera

        return blockFace;
    }
}
