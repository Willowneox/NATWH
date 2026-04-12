using UnityEngine;
using UnityEngine.UI;

public class TrashClickSpawner : MonoBehaviour
{
    public GameObject trashPrefab;
    public int numOfTrash = 4;
    [SerializeField] private AudioClip[] _clickSounds = new AudioClip[3];
    [SerializeField] private float _clickSoundVolume = 1f;

    private void Start()
    {
        for (int i = 0; i < numOfTrash; i++)
        {
            int x_rand = Random.Range(-800, 800);
            int y_rand = Random.Range(-450, 450);
            GameObject btn = Instantiate(trashPrefab, gameObject.transform);
            RectTransform rect = btn.GetComponent<RectTransform>();
            rect.localPosition = Vector3.zero;
            rect.localScale = Vector3.one;
            rect.anchoredPosition = new Vector2(x_rand, y_rand);
            btn.GetComponent<Button>().onClick.AddListener(() => PickupTrash(btn));
        }
    }

    public void PickupTrash(GameObject trash)
    {
        PlayRandomSound();
        numOfTrash--;
        Destroy(trash);
        if (numOfTrash <= 0)
        {
            MinigameSpawner.Instance.EndMinigame();
            Destroy(gameObject);
        }
    }

    private void PlayRandomSound()
    {
        if (_clickSounds.Length == 0) return;
        AudioClip clip = _clickSounds[Random.Range(0, _clickSounds.Length)];
        if (clip != null)
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, _clickSoundVolume);
    }
}