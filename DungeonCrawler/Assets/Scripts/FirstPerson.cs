using UnityEngine;

public class FirstPerson : MonoBehaviour
{
    public UnityEngine.Tilemaps.Tilemap blockTilemap;
    public UnityEngine.Tilemaps.Tilemap billboardTilemap;

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
                    billboard.transform.position = billboardTilemap.CellToWorld(pos);

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
        
    }

    GameObject generateBlockface(Vector3Int pos, Vector3Int facing) {
        var blockFace = new GameObject();

        blockFace.name = "Blockface_" + blockTilemap.GetSprite(pos).name;

        blockFace.transform.position = Vector3.zero;
        blockFace.transform.LookAt((Vector3)facing, new Vector3(0, 0, -1));
        blockFace.transform.position = facing;
        blockFace.transform.position.Scale(blockTilemap.cellSize);
        blockFace.transform.position += blockTilemap.CellToWorld(pos);

        var renderer = blockFace.AddComponent<SpriteRenderer>();
        renderer.sprite = blockTilemap.GetSprite(pos);

        blockFace.layer = 8; // Only viewable by 1st person camera

        return blockFace;
    }
}
