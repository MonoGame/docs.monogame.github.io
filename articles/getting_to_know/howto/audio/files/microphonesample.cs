using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameMicrophoneExample;

public class Game1 : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    SpriteFont font;
    // The most recent microphone samples.
    byte[] micSamples;
    // DynamicSoundEffectInstance is used to playback the captured audio after processing it for echo.
    DynamicSoundEffectInstance dynamicSound;
    // Microphone used for recording.
    Microphone activeMicrophone;
    // Used to communicate the microphone status to the user.
    string microphoneStatus = string.Empty;
    const string instructions = @"Press 'Space' to start and 'P' to stop recording";

#region MonoGame Boilerplate
    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        // Make sure to create a SpriteFont in the Content project and set its build action to "Content".
        font = Content.Load<SpriteFont>("File");
    }


    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Constantly check for a good microphone to start recording.
        InitializeMicrophone();

        // Check and update microphone status.
        UpdateMicrophoneStatus();

        if (IsKeyPressed(Keys.Space))
        {
            StartRecording();
        }
        if (IsKeyPressed(Keys.P))
        {
            StopRecording();
        }

        base.Update(gameTime);
    }


    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        spriteBatch.DrawString(font, instructions, new Vector2(10f, 20f), Color.White);
        spriteBatch.DrawString(font, microphoneStatus, new Vector2(10f, 50f), Color.White);
        spriteBatch.End();
        base.Draw(gameTime);
    }    
#endregion MonoGame Boilerplate

#region Input Handling
    private bool IsKeyPressed(Keys key)
    {
        return Keyboard.GetState().IsKeyDown(key);
    }

    private void StartRecording()
    {
        // Can't start a microphone that doesn't exist.
        if (activeMicrophone == null) { return; }

        try
        {
            activeMicrophone.Start();
        }
        catch (NoMicrophoneConnectedException)
        {
            UpdateMicrophoneStatus();
        }
    }

    private void StopRecording()
    {
        // Can't stop a microphone that doesn't exist.
        if (activeMicrophone == null) { return; }

        try
        {
            // Stop the microphone
            activeMicrophone.Stop();
        }
        catch (NoMicrophoneConnectedException)
        {
            UpdateMicrophoneStatus();
        }
    }
#endregion Input Handling

#region Microphone Handling
    /// <summary>
    /// Keep track of the microphone status to communicate to the user.
    /// </summary>
    private void UpdateMicrophoneStatus()
    {
        // We don't have any microphones connected to the system.
        if (activeMicrophone == null)
        {
            microphoneStatus = "Waiting for microphone connection...";
        }
        else
        {
            try
            {
                // Update the status - if the microphone gets disconnected this will throw
                microphoneStatus = string.Format("{0} is {1}", activeMicrophone.Name, activeMicrophone.State);
            }
            catch (NoMicrophoneConnectedException)
            {
                // Microphone got disconnected - Let's ask the user to reconnect it.
                microphoneStatus = string.Format("Please reconnect {0}", activeMicrophone.Name);
            }
        }
    }

    /// <summary>
    /// This is called each time a microphone buffer has been filled.
    /// </summary>
    void BufferReady(object sender, EventArgs e)
    {
        try
        {
            // Copy the captured audio data into the pre-allocated array.
            int sampleDataSize = activeMicrophone.GetData(micSamples);

            // Process the captured audio for playback
            dynamicSound.SubmitBuffer(micSamples, 0, sampleDataSize);
        }
        catch (NoMicrophoneConnectedException)
        {
            // Microphone was disconnected - let the user know.
            UpdateMicrophoneStatus();
        }
    }

    /// <summary>
    /// Look for a good microphone to start recording.
    /// </summary>
    /// <returns></returns>
    private Microphone PickFirstConnectedMicrophone()
    {
        // Let's pick the default microphone if it's ready.
        if (Microphone.Default != null && Microphone.Default.IsConnected())
        {
            return Microphone.Default;
        }

        // Default microphone seems to be disconnected so look for another microphone that we can use.
        // And if the default was null then the list will be empty and we'll skip the search.
        foreach (Microphone microphone in Microphone.All)
        {
            if (microphone.IsConnected())
            {
                return microphone;
            }
        }

        // There are no microphones hooked up to the system!
        return null;
    }

    /// <summary>
    /// Finds a good microphone to use and sets up everything to start recording and playback.
    /// Once a microphone is selected the game uses it throughout its lifetime.
    /// If it gets disconnected it will tell the user to reconnect it.
    /// </summary>
    private void InitializeMicrophone()
    {
        // We already have a microphone, skip out early.
        if (activeMicrophone != null) { return; }

        try
        {
            // Find the first microphone that's ready to rock.
            activeMicrophone = PickFirstConnectedMicrophone();
            if (activeMicrophone != null)
            {
                // Set the capture buffer size for kow latency.
                // Microphone will call the game back when it has captured at least that much audio data.
                activeMicrophone.BufferDuration = TimeSpan.FromMilliseconds(100);
                // Subscribe to the event that's raised when the capture buffer is filled.
                activeMicrophone.BufferReady += BufferReady;
                // We will put the mic samples in this buffer.  We only want to allocate it once.
                micSamples = new byte[activeMicrophone.GetSampleSizeInBytes(activeMicrophone.BufferDuration)];

                 // Create a DynamicSoundEffectInstance in the right format to playback the captured audio.
                dynamicSound = new DynamicSoundEffectInstance(activeMicrophone.SampleRate, AudioChannels.Mono);
                dynamicSound.Play();
            }
        }
        catch (NoMicrophoneConnectedException)
        {
            // Uh oh, the microphone was disconnected in the middle of initialization.
            // Let's clean up everything so we can look for another microphone again on the next update.
            activeMicrophone.BufferReady -= BufferReady;
            activeMicrophone = null;
        }
    }
#endregion Microphone Handling
}

#region Helper Microphone Extension Method
public static class MicrophoneExtensions
{
    // Provides a simple method to check if a microphone is connected.
    // There is no guarantee that the microphone will not get disconnected at any time.
    // This method helps in simplifying the microphone enumeration code.
    public static bool IsConnected(this Microphone microphone)
    {
        try
        {
            MicrophoneState state = microphone.State;
            return true;
        }
        catch (NoMicrophoneConnectedException)
        {
            return false;
        }
    }
}
#endregion Helper Microphone Extension Method