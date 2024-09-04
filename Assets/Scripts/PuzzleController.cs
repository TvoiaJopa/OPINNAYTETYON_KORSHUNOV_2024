using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    private int emptyLocation;
    private int size;
    public List<GameObject> puzzleParts;
    [SerializeField] private Transform gameVerticalTransform;
    [SerializeField] private Transform gameHorizontalTransform;
    public GameObject prefabObject;

    private void Start()
    {
        size = 30;
        CreateGamePieces(0, PuzzleType.Horizontal);
    }

    [SerializeField] private AudioSource audioS;
    public AudioClip audioClip;
    public void PlaySound()
    {
        audioS.PlayOneShot(audioClip);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.GetComponent<RollObject>() != null)
            {
                PlaySound();

                hit.collider.gameObject.GetComponent<RollObject>().RotateObjectBy90();
            }
        }
        else if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.GetComponent<RollObject>() != null)
            {
                PlaySound();

                hit.collider.gameObject.GetComponent<RollObject>().RotateObjectByMinus90();
            }
        }
    }

    public void SpawnVerticalPuzzle()
    {
        CreateGamePieces(0, PuzzleType.Vertical);

    }

    public void SpawnHorizontalPuzzle()
    {
        CreateGamePieces(0, PuzzleType.Horizontal);

    }

    public void ClearPuzzle()
    {
        for (int i = 0; i < puzzleParts.Count; i++)
        {
            Destroy(puzzleParts[i]);
        }
    }
    private void CreateGamePieces(float gapThickness, PuzzleType type)
    {
        ClearPuzzle();

        float width = 1 * 1.8f / (float)size;
        float height = 1 / (float)size;
        float customWidth = 1 / (float)size * 100;
        switch (type)
        {
            case PuzzleType.Vertical:
                width = 1 * 1.8f / (float)size;
                height = 1 / (float)size;
                break;
            case PuzzleType.Horizontal:
                height = 1 * 1.8f / (float)size;
                width = 1 / (float)size;
                break;
        }
        float rowSize = 0, colSize = 0;

        switch (type)
        {
            case PuzzleType.Vertical:
                rowSize = size;
                colSize = size / 1.8f;
                break;
            case PuzzleType.Horizontal:
                rowSize = size / 1.8f;
                colSize = size;
                break;
        }

        for (int row = 0; row < rowSize; row++)
        {
            for (int col = 0; col < colSize; col++)
            {
                //Debug.LogError("Cell " + ((row * size) + col));
                Transform piece = null;
                switch (type)
                {
                    case PuzzleType.Horizontal:
                        //piece = Instantiate(piecePrefab, gameHorizontalTransform);
                        piece = Instantiate(prefabObject).transform;
                        piece.gameObject.transform.SetParent(gameHorizontalTransform);
                        piece.gameObject.SetActive(true);
                        break;
                    case PuzzleType.Vertical:
                        //piece = Instantiate(piecePrefab, gameVerticalTransform);
                        piece = Instantiate(prefabObject).transform;
                        piece.gameObject.transform.SetParent(gameVerticalTransform);

                        piece.gameObject.SetActive(true);
                        break;
                }
                puzzleParts.Add(piece.gameObject);
                piece.GetComponent<RollObject>().controller = this;
                piece.GetComponent<RollObject>().num = (row * size) + col;



                // Pieces will be in a game board going from -1 to +1.
                piece.localPosition = new Vector3(-1 + (1.8f * customWidth * col) + customWidth * 1.8f,
                                                  +1 - (1.8f * customWidth * row) - customWidth, 1.8f);

                piece.localScale = ((1.8f * customWidth) - gapThickness) * Vector3.one;
                piece.name = $"{(row * size) + col}";
                // We want an empty space in the bottom right.

                // We want to map the UV coordinates appropriately, they are 0->1.
                float gap = gapThickness / 2;
                Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                Vector2[] uv = new Vector2[4];


                switch (type)
                {
                    case PuzzleType.Vertical:
                        width = 1 * 1.8f / (float)size;
                        height = 1 / (float)size;
                        break;
                    case PuzzleType.Horizontal:
                        height = 1 * 1.8f / (float)size;
                        width = 1 / (float)size;
                        break;
                }
                switch (type)
                {
                    case PuzzleType.Vertical:
                        rowSize = size;
                        colSize = size / 2;
                        break;
                    case PuzzleType.Horizontal:
                        rowSize = size / 2;
                        colSize = size;
                        break;
                }
                // UV coord order: (0, 1), (1, 1), (0, 0), (1, 0)
                uv[0] = new Vector2((width * col) + gap, 1 - ((height * (row + 1)) - gap));
                uv[1] = new Vector2((width * (col + 1)) - gap, 1 - ((height * (row + 1)) - gap));
                uv[2] = new Vector2((width * col) + gap, 1 - ((height * row) + gap));
                uv[3] = new Vector2((width * (col + 1)) - gap, 1 - ((height * row) + gap));
                // Assign our new UVs to the mesh.
                mesh.uv = uv;
            }
        }
    }
    private void CreateGamePieces2(float gapThickness, PuzzleType type)
    {
        ClearPuzzle();

        float width = 1 * 1.8f / (float)size;
        float height = 1 / (float)size;
        float customWidth = 1 / (float)size * 100;
        switch (type)
        {
            case PuzzleType.Vertical:
                width = 1 * 1.8f / (float)size;
                height = 1 / (float)size;
                break;
            case PuzzleType.Horizontal:
                height = 1 * 1.8f / (float)size;
                width = 1 / (float)size;
                break;
        }
        float rowSize = 0, colSize = 0;

        switch (type)
        {
            case PuzzleType.Vertical:
                rowSize = size;
                colSize = size / 1.8f;
                break;
            case PuzzleType.Horizontal:
                rowSize = size / 1.8f;
                colSize = size;
                break;
        }

        for (int row = 0; row < rowSize; row++)
        {
            for (int col = 0; col < colSize; col++)
            {
                //Debug.LogError("Cell " + ((row * size) + col));
                Transform piece = null;
                switch (type)
                {
                    case PuzzleType.Horizontal:
                        //piece = Instantiate(piecePrefab, gameHorizontalTransform);
                        piece = puzzleParts[(row * size) + col].transform;
                        piece.gameObject.transform.SetParent(gameHorizontalTransform);
                        piece.gameObject.SetActive(true);
                        break;
                    case PuzzleType.Vertical:
                        //piece = Instantiate(piecePrefab, gameVerticalTransform);
                        piece = puzzleParts[(row * size) + col].transform;
                        piece.gameObject.transform.SetParent(gameVerticalTransform);

                        piece.gameObject.SetActive(true);
                        break;
                }
                puzzleParts.Add(piece.gameObject);
                piece.GetComponent<RollObject>().controller = this;
                piece.GetComponent<RollObject>().num = (row * size) + col;



                // Pieces will be in a game board going from -1 to +1.
                piece.localPosition = new Vector3(-1 + (1.8f * customWidth * col) + customWidth * 1.8f,
                                                  +1 - (1.8f * customWidth * row) - customWidth, 1.8f);

                piece.localScale = ((1.8f * customWidth) - gapThickness) * Vector3.one;
                piece.name = $"{(row * size) + col}";
                // We want an empty space in the bottom right.

                // We want to map the UV coordinates appropriately, they are 0->1.
                float gap = gapThickness / 2;
                Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                Vector2[] uv = new Vector2[4];


                switch (type)
                {
                    case PuzzleType.Vertical:
                        width = 1 * 1.8f / (float)size;
                        height = 1 / (float)size;
                        break;
                    case PuzzleType.Horizontal:
                        height = 1 * 1.8f / (float)size;
                        width = 1 / (float)size;
                        break;
                }
                switch (type)
                {
                    case PuzzleType.Vertical:
                        rowSize = size;
                        colSize = size / 2;
                        break;
                    case PuzzleType.Horizontal:
                        rowSize = size / 2;
                        colSize = size;
                        break;
                }

                // UV coord order: (0, 1), (1, 1), (0, 0), (1, 0)
                uv[0] = new Vector2((width * col) + gap, 1 - ((height * (row + 1)) - gap));
                uv[1] = new Vector2((width * (col + 1)) - gap, 1 - ((height * (row + 1)) - gap));
                uv[2] = new Vector2((width * col) + gap, 1 - ((height * row) + gap));
                uv[3] = new Vector2((width * (col + 1)) - gap, 1 - ((height * row) + gap));
                // Assign our new UVs to the mesh.
                mesh.uv = uv;


            }
        }
    

    }
}

public enum PuzzleType
{
    Vertical,
    Horizontal
}