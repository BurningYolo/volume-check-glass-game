using UnityEngine;

public class GlassShatter : MonoBehaviour
{
    public float destroyThreshold = 0.1f; // Adjust this value as needed
    public int micSampleRate = 44100; // Adjust this value to match your microphone's sample rate
    public string micDeviceName = null; // Set this to the desired microphone device name or leave it as null to use the default microphone
    public GameObject shatteredObjectPrefab; // Prefab of the shattered glass object
    public SliderController sliderController;
    public float vibrationIntensity = 0.1f; // Adjust this value to control the intensity of the vibration
    public AudioClip shatterSound; // Sound effect for shattering
    public GameController gameController;

    private AudioClip micClip;
    private bool shattered = false; // Flag to track if object has been shattered

    private Vector3 originalPosition; // The original position of the object

    private void Start()
    {
        // Initialize the microphone
        if (Microphone.devices.Length > 0)
        {
            if (micDeviceName == null)
            {
                micDeviceName = Microphone.devices[0];
            }

            micClip = Microphone.Start(micDeviceName, true, 1, micSampleRate);
        }
        else
        {
            Debug.LogError("No microphone devices found.");
        }

        // Record the original position of the object
        originalPosition = transform.position;
    }

    private void Update()
    {
        
        // Check if the microphone is recording
        if (Microphone.IsRecording(micDeviceName) && !shattered && !gameController.isGamePaused )
        {
            // Calculate the input level from the microphone
            float inputLevel = GetMicInputLevel();
            sliderController.sliderValue = inputLevel;

            if (inputLevel >= destroyThreshold * 0.5f)
            {
                // Vibrate the object
                Vibrate();
            }

            // Check if the input level exceeds the destroy threshold
            if (inputLevel >= destroyThreshold)
            {
                // Shatter the glass object
                Shatter();
            }
        }
        
    }

    private float GetMicInputLevel()
    {
        // Read the microphone data into an array
        float[] samples = new float[micClip.samples];
        micClip.GetData(samples, 0);

        // Calculate the RMS (Root Mean Square) of the samples
        float rms = 0f;
        foreach (float sample in samples)
        {
            rms += sample * sample;
        }
        rms /= samples.Length;
        rms = Mathf.Sqrt(rms);

        return rms;
    }

   private void Shatter()
{
    shattered = true;

    // Instantiate multiple copies of the shattered glass object prefab
    for (int i = 0; i < 100; i++)
    {
        // Instantiate the shattered glass object prefab at the same position and rotation as the original object
        GameObject shatteredObject = Instantiate(shatteredObjectPrefab, transform.position, transform.rotation);

        // Get the Rigidbody component of the shattered object
        Rigidbody rigidbody = shatteredObject.GetComponent<Rigidbody>();

        // Apply a random velocity to the shattered object
        Vector3 randomVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        float randomSpeed = Random.Range(1f, 5f);
        rigidbody.velocity = randomVelocity * randomSpeed;

        // Apply any necessary shattering effect to the shattered object (e.g., play particle effects, break into smaller pieces)

        // Play the shatter sound effect
        AudioSource.PlayClipAtPoint(shatterSound, transform.position, 0.1f); //o.4 volume
    }

    // Destroy the original glass object
    Destroy(gameObject);
}

    private void Vibrate()
    {
        // Calculate the random displacement for the object's position
        Vector3 displacement = new Vector3(Random.Range(-vibrationIntensity, vibrationIntensity), Random.Range(-vibrationIntensity, vibrationIntensity), Random.Range(-vibrationIntensity, vibrationIntensity));

        // Apply the displacement to the object's position
        transform.position = originalPosition + displacement * vibrationIntensity;
    }

    private void OnDisable()
    {
        // Stop the microphone when the script is disabled or the object is destroyed
        if (Microphone.IsRecording(micDeviceName))
        {
            Microphone.End(micDeviceName);
        }
    }
}
