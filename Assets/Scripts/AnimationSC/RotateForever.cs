using UnityEngine;
using DG.Tweening;

public class RotateForever : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1440f; // 1 saniyede kaç derece döneceði
    public Ease easeType = Ease.Linear; // Animasyon tipi

    void Start()
    {
        // Sonsuz döndürme iþlemini baþlat
        transform.DORotate(new Vector3(0, 360, 0), rotationSpeed / 360f, RotateMode.LocalAxisAdd)
                 .SetEase(easeType) // Hangi easing türü kullanýlacak
                 .SetLoops(-1, LoopType.Incremental); // Sonsuz döngü
    }
}
