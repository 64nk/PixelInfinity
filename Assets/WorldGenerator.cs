using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public Transform player;
    public GameObject[] chunkPrefabs;

    public float chunkBreite = 30f;
    public float spawnVorausDistanz = 35f;
    public float despawnDistanz = 35f;
    public float ersterChunkStartX = 30f;

    public Sprite blockSprite;
    public Sprite spitzeSprite;
    public PhysicsMaterial2D rutschigMaterial;

    public int abschnitteProChunk = 3;
    public float chanceRoteGruppe = 0.6f;

    public Color rotFarbe = new Color(0.545f, 0f, 0f);
    public float bodenOberkante = -1.5f;

    public float[] blockHoehen = new float[] { 0.5f, 1.3f };
    public float minBlockBreite = 1.5f;
    public float maxBlockBreite = 3.5f;
    public Color blockFarbe = new Color(0.2f, 0.6f, 1f);

    private float naechstesSpawnX;
    private List<GameObject> gespawnteChunks = new List<GameObject>();
    private bool letzteWarRot = false;

    void Start()
    {
        naechstesSpawnX = ersterChunkStartX;
    }

    void Update()
    {
        if (player == null || chunkPrefabs.Length == 0)
        {
            return;
        }

        if (naechstesSpawnX - player.position.x < spawnVorausDistanz)
        {
            Spawn();
        }

        AlteChunksLoeschen();
    }

    void Spawn()
    {
        GameObject prefab = chunkPrefabs[Random.Range(0, chunkPrefabs.Length)];
        GameObject neuerChunk = Instantiate(prefab, new Vector3(naechstesSpawnX, 0f, 0f), Quaternion.identity);
        neuerChunk.transform.SetParent(transform);
        gespawnteChunks.Add(neuerChunk);

        AbschnitteFuellen(neuerChunk);

        naechstesSpawnX += chunkBreite;
    }

    void AbschnitteFuellen(GameObject chunk)
    {
        if (blockSprite == null || spitzeSprite == null)
        {
            return;
        }

        float abschnittBreite = chunkBreite / abschnitteProChunk;

        for (int i = 0; i < abschnitteProChunk; i++)
        {
            float x = chunk.transform.position.x - chunkBreite / 2f + abschnittBreite * (i + 0.5f);

            if (!letzteWarRot && Random.value < chanceRoteGruppe)
            {
                RoteGruppeSpawnen(chunk.transform, x);
                letzteWarRot = true;
            }
            else
            {
                BlauenBlockSpawnen(chunk.transform, x);
                letzteWarRot = false;
            }
        }
    }

    void RoteGruppeSpawnen(Transform chunk, float x)
    {
        int art = Random.Range(0, 5);

        if (art == 0)
        {
            int anzahl = Random.Range(3, 7);
            for (int i = 0; i < anzahl; i++)
            {
                Spitze(chunk, x + i * 0.6f, bodenOberkante);
            }
        }
        else if (art == 1)
        {
            Block(chunk, x, 0.7f, 0.4f);
            Block(chunk, x + 0.7f, 0.7f, 0.8f);
            Block(chunk, x + 1.4f, 0.7f, 1.2f);
        }
        else if (art == 2)
        {
            Block(chunk, x, 0.6f, 1.0f);
            Block(chunk, x + 3.5f, 0.6f, 1.0f);
        }
        else if (art == 3)
        {
            Block(chunk, x, Random.Range(2.5f, 4f), 0.5f);
        }
        else
        {
            Block(chunk, x, 2f, 0.4f);
            Spitze(chunk, x - 0.6f, bodenOberkante + 0.4f);
            Spitze(chunk, x, bodenOberkante + 0.4f);
            Spitze(chunk, x + 0.6f, bodenOberkante + 0.4f);
        }
    }

    void Block(Transform chunk, float x, float breite, float hoehe)
    {
        Vector3 position = new Vector3(x, bodenOberkante + hoehe / 2f, -1f);
        GameObject block = NeuesObjekt("RoterBlock", chunk, blockSprite, position, new Vector3(breite, hoehe, 1f), rotFarbe);
        block.AddComponent<BoxCollider2D>();
        block.AddComponent<Obstacle>();
    }

    void Spitze(Transform chunk, float x, float y)
    {
        Vector3 position = new Vector3(x, y + 0.3f, -1f);
        GameObject spitze = NeuesObjekt("Spitze", chunk, spitzeSprite, position, new Vector3(0.6f, 0.6f, 1f), rotFarbe);
        spitze.AddComponent<PolygonCollider2D>();
        spitze.AddComponent<Obstacle>();
    }

    void BlauenBlockSpawnen(Transform chunk, float x)
    {
        float y = blockHoehen[Random.Range(0, blockHoehen.Length)];
        float breite = Random.Range(minBlockBreite, maxBlockBreite);

        GameObject block = NeuesObjekt("SchwebenderBlock", chunk, blockSprite, new Vector3(x, y, -1f), new Vector3(breite, 0.5f, 1f), blockFarbe);
        BoxCollider2D col = block.AddComponent<BoxCollider2D>();
        col.size = new Vector2(0.9f, 0.8f);
        col.edgeRadius = 0.1f;
        col.sharedMaterial = rutschigMaterial;
    }

    GameObject NeuesObjekt(string name, Transform chunk, Sprite sprite, Vector3 position, Vector3 groesse, Color farbe)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(chunk);
        obj.transform.position = position;
        obj.transform.localScale = groesse;

        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.color = farbe;
        return obj;
    }

    void AlteChunksLoeschen()
    {
        for (int i = gespawnteChunks.Count - 1; i >= 0; i--)
        {
            GameObject chunk = gespawnteChunks[i];
            if (player.position.x - chunk.transform.position.x > despawnDistanz)
            {
                gespawnteChunks.RemoveAt(i);
                Destroy(chunk);
            }
        }
    }
}
