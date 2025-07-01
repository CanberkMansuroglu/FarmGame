using Assets.Scripts;
using UnityEngine;

public class PlowController : MonoBehaviour
{

    public Transform rayPivot;

    void Update()
    {
        // Raycast için baþlangýç noktasý
        Vector3 origin = rayPivot.position;
        // Raycast yönü
        Vector3 direction = Vector3.down;
        // Raycast mesafesi
        float distance = 3f;

        // Raycast iþlemini gerçekleþtir
        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            // Temas edilen objenin tagini kontrol et
            if (hit.collider.CompareTag("PartDirt"))
            {
                FieldController field = hit.collider.GetComponent<FieldController>();

                if (!field.isProcessed)
                {
                    field.isProcessed = true;
                }


            }
        }
    }

    // Debug için rayi sahnede göstermek istersen
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 3f);
    }
}
