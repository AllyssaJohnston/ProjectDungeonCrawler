using UnityEngine;

public class FirstPerson : MonoBehaviour
{
    public UnityEngine.Tilemaps.Tilemap blockTilemap;
    public UnityEngine.Tilemaps.Tilemap billboardTilemap;

    public PlayerMovement playerMovement;
    public Transform playerPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

        var root = new GameObject();
        root.name = "FirstPersonWorldRoot";

        for (int i = blockTilemap.cellBounds.xMin; i < blockTilemap.cellBounds.xMax; i++) {
            for (int j = blockTilemap.cellBounds.yMin; j < blockTilemap.cellBounds.yMax; j++) {
                var pos = new Vector3Int(i, j, 0);

                if (blockTilemap.GetSprite(pos) != null) {
                    if (blockTilemap.GetSprite(pos + Vector3Int.up) == null) {
                        var bf = generateBlockface(pos, Vector3Int.up);
                        bf.transform.parent = root.transform;
                    }
                    if (blockTilemap.GetSprite(pos + Vector3Int.down) == null) {
                        var bf = generateBlockface(pos, Vector3Int.down);
                        bf.transform.parent = root.transform;
                    }
                    if (blockTilemap.GetSprite(pos + Vector3Int.left) == null) {
                        var bf = generateBlockface(pos, Vector3Int.left);
                        bf.transform.parent = root.transform;
                    }
                    if (blockTilemap.GetSprite(pos + Vector3Int.right) == null) {
                        var bf = generateBlockface(pos, Vector3Int.right);
                        bf.transform.parent = root.transform;
                    }
                }
            }
        }

        for (int i = billboardTilemap.cellBounds.xMin; i < billboardTilemap.cellBounds.xMax; i++) {
            for (int j = billboardTilemap.cellBounds.yMin; j < billboardTilemap.cellBounds.yMax; j++) {
                var pos = new Vector3Int(i, j, 0);

                if (billboardTilemap.GetSprite(pos) != null) {
                    var billboard = new GameObject();
                    billboard.transform.parent = root.transform;
                    billboard.transform.position = billboardTilemap.CellToWorld(pos) + billboardTilemap.cellSize * 0.5f;

                    billboard.name = "Billboard_" + billboardTilemap.GetSprite(pos).name;

                    var behavior = billboard.AddComponent<Billboard>();
                    behavior.whatToFace = this.transform;

                    var renderer = billboard.AddComponent<SpriteRenderer>();
                    renderer.sprite = billboardTilemap.GetSprite(pos);

                    billboard.layer = 8; // Only viewable by 1st person camera

                    Debug.Log(pos);
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
        this.transform.position = new Vector3(playerPos.position.x, playerPos.position.y, 0f);
    }

    GameObject generateBlockface(Vector3Int pos, Vector3Int facing) {
        var blockFace = new GameObject();

        blockFace.name = "Blockface_" + blockTilemap.GetSprite(pos).name;

        blockFace.transform.position = Vector3.zero;
        blockFace.transform.LookAt((Vector3)facing, new Vector3(0, 0, -1));

        // Why the hell does unity just not do anything with this line
        //blockFace.transform.position.Scale(blockTilemap.cellSize * 0.5f);

        Vector3 position = facing;
        position.Scale(blockTilemap.cellSize * 0.5f);
        position += blockTilemap.CellToWorld(pos) + blockTilemap.cellSize * 0.5f;
        blockFace.transform.position = position;

        var renderer = blockFace.AddComponent<SpriteRenderer>();
        renderer.sprite = blockTilemap.GetSprite(pos);

        blockFace.layer = 8; // Only viewable by 1st person camera

        return blockFace;
    }
}
