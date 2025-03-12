// Loading a SoundEffect using the content pipeline
SoundEffect soundEffect = Content.Load<SoundEffect>("soundEffect");

// Create an instance we can control
SoundEffectInstance soundEffectInstance = soundEffect.CreateInstance();

// Adjust the properties of the instance as needed
soundEffectInstance.IsLooped = true;    // Make it loop
soundEffectInstance.Volume = 0.5f;      // Set half volume.

// Play the sound effect using the instance.
soundEffectInstance.Play();