using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;


public class CurrencyEmissionEvent 
{
    public Vector2 position;
    public CurrencyType type;
    public int amount;

    public CurrencyEmissionEvent(Vector2 _position, CurrencyType _type, int _amount)
    {
        position = _position;
        type = _type;
        amount = _amount;
    }
}

public class CurrencyParticleManager : MonoBehaviour
{
    public GameObject currencyParticleEmitter;

    public Material runCurrencySprite;
    public Material permanentCurrencySprite;

    public float delayBeforeReturn = 5.0f;
    public float emissionTime = 0.1f;
    public float particleReturnSpeed = 15.0f;
    static bool x = true;


    // The target could be the player or the Score UI, for example
    private Transform targetTransform;

    void Start()
    {
        EventBus.Subscribe<CurrencyEmissionEvent>(OnCurrencyEmissionEvent);
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnCurrencyEmissionEvent(CurrencyEmissionEvent e)
    {
        if (SettingsLoader.Settings.particlesEnabled)
        {
            GameObject emitter = Instantiate(currencyParticleEmitter);

            emitter.transform.position = e.position;

            CurrencyParticleEmitterController controller = emitter.GetComponent<CurrencyParticleEmitterController>();

            if (e.type == CurrencyType.Run)
                controller.EmitCurrency(targetTransform, e.type, e.amount, emissionTime, runCurrencySprite, delayBeforeReturn, particleReturnSpeed);
            else if (e.type == CurrencyType.Permanent)
                controller.EmitCurrency(targetTransform, e.type, e.amount, emissionTime, permanentCurrencySprite, delayBeforeReturn, particleReturnSpeed);
        }
        else
        {
            if (e.type == CurrencyType.Permanent)
                RunManager.Instance.AddCurrency(e.amount, 0);
            else
                RunManager.Instance.AddCurrency(0, e.amount);
        }
    }
}
