using UnityEngine;

public class PlantMiniGame : MonoBehaviour
{
    public GameObject prefab;
    private int numOfPlants;
    [SerializeField] private AudioClip[] _cutSounds = new AudioClip[3];
    [SerializeField] private float _cutSoundVolume = 1f;

    void Awake()
    {
        numOfPlants = Random.Range(4, 6);
        for (int i = 0; i < numOfPlants; i++)
        {
            int x_rand = Random.Range(-500, 500);
            int y_rand = Random.Range(-300, -100);
            GameObject plant = Instantiate(prefab, gameObject.transform);
            RectTransform rect = plant.GetComponent<RectTransform>();
            rect.localPosition = Vector3.zero;
            rect.localScale = Vector3.one;
            rect.anchoredPosition = new Vector2(x_rand, y_rand);
            plant.GetComponent<Plant>().Init(this);
        }
    }

    public void CutPlant(GameObject Plant)
    {
        PlayRandomSound();
        numOfPlants--;
        Destroy(Plant);
        if (numOfPlants <= 0)
        {
            MinigameSpawner.Instance.EndMinigame();
            Destroy(gameObject);
        }
    }

    private void PlayRandomSound()
    {
        if (_cutSounds.Length == 0) return;
        AudioClip clip = _cutSounds[Random.Range(0, _cutSounds.Length)];
        if (clip != null)
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, _cutSoundVolume);
    }
}