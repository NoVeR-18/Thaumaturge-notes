using UnityEngine;

public class DayCycle : MonoBehaviour
{
    [Range(0, 1)]
    public float DayTime;
    public float DayDuration = 30f;

    public AnimationCurve SunCurve;
    public AnimationCurve MoonCurve;
    public AnimationCurve SkyboxCurve;

    public Material DaySkybox;
    public Material NightSkybox;

    public Light Sun;
    public Light Moon;

    public ParticleSystem Stars;

    private float SunIntensity;
    private float MoonIntensity;

    // Start is called before the first frame update
    void Start()
    {
        SunIntensity = Sun.intensity;
        MoonIntensity = Moon.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        DayTime += Time.deltaTime / DayDuration;
        if (DayTime >= 1) DayTime -= 1;

        RenderSettings.skybox.Lerp(NightSkybox, DaySkybox, SkyboxCurve.Evaluate(DayDuration));
        RenderSettings.sun = SkyboxCurve.Evaluate(DayTime) > 0.101f ? Sun : Moon;
        DynamicGI.UpdateEnvironment();

        var mainModule = Stars.main;
        mainModule.startColor = new Color(1, 1, 1, 1 - SkyboxCurve.Evaluate(DayTime));

        Sun.transform.localRotation = Quaternion.Euler(DayTime * 360f, 180, 0);
        Sun.intensity = SunIntensity * SunCurve.Evaluate(DayTime);

        Moon.transform.localRotation = Quaternion.Euler(DayTime * 360f + 180f, 180, 0);
        Moon.intensity = MoonIntensity * MoonCurve.Evaluate(DayTime);
    }
}
