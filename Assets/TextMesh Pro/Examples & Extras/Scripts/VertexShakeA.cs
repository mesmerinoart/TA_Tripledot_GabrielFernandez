using UnityEngine;
using TMPro;

public class JumpingText : MonoBehaviour
{
    public float jumpHeight = 10f;
    public float speed = 5f;

    private TMP_Text textMesh;
    private TMP_TextInfo textInfo;
    private float[] charOffsets;

    void Start()
    {
        textMesh = GetComponent<TMP_Text>();
        textMesh.ForceMeshUpdate();
        textInfo = textMesh.textInfo;
        charOffsets = new float[textInfo.characterCount];
    }

    void Update()
    {
        textMesh.ForceMeshUpdate();
        textInfo = textMesh.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible)
                continue;

            var verts = textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].vertices;
            int index = textInfo.characterInfo[i].vertexIndex;

            float offset = Mathf.Sin(Time.time * speed + i * 0.2f) * jumpHeight;

            for (int j = 0; j < 4; j++)
                verts[index + j].y += offset;
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
        }
    }
}
