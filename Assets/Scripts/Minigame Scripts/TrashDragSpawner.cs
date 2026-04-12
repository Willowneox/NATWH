using UnityEngine;

public class TrashDragSpawner : MonoBehaviour
{
    [SerializeField] private DropZone dropZone;
    [SerializeField] private GameObject trashPrefab;
    [SerializeField] private AudioClip[] _paperSounds = new AudioClip[3];
    [SerializeField] private float _paperSoundVolume = 1f;

    public int numOfTrash = 4;

    private void Start()
    {
        for (int i = 0; i < numOfTrash; i++)
        {
            int x_rand = Random.Range(-800, 800);
            int y_rand = Random.Range(-450, 450);
            GameObject trash = Instantiate(trashPrefab, gameObject.transform);
            RectTransform rect = trash.GetComponent<RectTransform>();
            rect.localPosition = Vector3.zero;
            rect.localScale = Vector3.one;
            rect.anchoredPosition = new Vector2(x_rand, y_rand);
            trash.GetComponent<DraggableTrash>().Init(this, dropZone);
        }
    }

    public void PickupTrash()
    {
        PlayRandomPaperSound();

        numOfTrash--;
        if (numOfTrash <= 0)
        {
            MinigameSpawner.Instance.EndMinigame();
            Destroy(gameObject);
        }
    }

    private void PlayRandomPaperSound()
    {
        if (_paperSounds.Length == 0) return;

        AudioClip clip = _paperSounds[Random.Range(0, _paperSounds.Length)];
        if (clip != null)
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, _paperSoundVolume);
    }
}