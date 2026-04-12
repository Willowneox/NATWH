using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BatteryVFX : MonoBehaviour
{
    private Volume volume;
    private ChromaticAberration chromaticAberration;
    private Player p;

    [SerializeField] private float maxIntensity = 1f;

    void Start()
    {
        p = Player.Instance;
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out chromaticAberration);
    }

    void Update()
    {
        if (chromaticAberration == null || p == null) return;

        float batteryPercent = p.batteryLeft / p.batteryCapacity;

        if (batteryPercent > 0.5f)
        {
            chromaticAberration.intensity.value = 0f;
        }
        else
        {
            // remap 0.5 -> 0 to 0 -> maxIntensity
            float t = 1f - (batteryPercent / 0.5f);
            // square it so it stays low for a while then spikes hard at the end
            chromaticAberration.intensity.value = Mathf.Pow(t, 3f) * maxIntensity;
        }
    }
}