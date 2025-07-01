using UnityEngine;
using DG.Tweening;

public class RotateForever : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1440f; // 1 saniyede ka� derece d�nece�i
    public Ease easeType = Ease.Linear; // Animasyon tipi

    void Start()
    {
        // Sonsuz d�nd�rme i�lemini ba�lat
        transform.DORotate(new Vector3(0, 360, 0), rotationSpeed / 360f, RotateMode.LocalAxisAdd)
                 .SetEase(easeType) // Hangi easing t�r� kullan�lacak
                 .SetLoops(-1, LoopType.Incremental); // Sonsuz d�ng�
    }
}
