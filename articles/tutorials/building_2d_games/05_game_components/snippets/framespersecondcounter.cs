using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary;

/// <summary>
/// Tracks and calculates the number of frames rendered per second.
/// </summary>
public class FramesPerSecondCounter : DrawableGameComponent
{
    /// A static TimeSpan representing one second, used for FPS calculation intervals.
    private static readonly TimeSpan s_oneSecond = TimeSpan.FromSeconds(1);

    /// Tracks the number of frames rendered in the current second.
    private int _frameCounter;

    /// Tracks the elapsed time since the last FPS calculation.
    private TimeSpan _elapsedTime;

    /// <summary>
    /// Gets the current frames per second calculation.
    /// </summary>
    public float FramesPerSecond { get; private set; }

    /// <summary>
    /// Creates a new FramesPerSecondCounter.
    /// </summary>
    public FramesPerSecondCounter(Game game) : base(game)
    {
    }

    /// <summary>
    /// Updates the FPS calculation based on elapsed game time.
    /// </summary>
    /// <param name="gameTime">A snapshot of the game's timing values.</param>
    public override Update(GameTime gameTime)
    {
        _elapsedTime += gameTime.ElapsedGameTime;

        if (_elapsedTime > s_oneSecond)
        {
            FramesPerSecond = _frameCounter;
            _frameCounter = 0;
            _elapsedTime -= s_oneSecond;
        }
    }

    /// <summary>
    /// Increments the frame counter. Should be called once per frame during the game's Draw method.
    /// </summary>
    public override Draw(GameTime gameTime)
    {
        // Increment the frame counter only during draw.
        _frameCounter++;

        // Update the window title to show the frames per second.
        Game.Window.Title = $"FPS: {FramesPerSecond}";
    }
}