// Loading a Song using the content pipeline
Song song = Content.Load<Song>("song");

// Set whether the song should repeat when finished
MediaPlayer.IsRepeating = true;

// Adjust the volume (0.0f to 1.0f)
MediaPlayer.Volume = 0.5f;

// Check if the media player is already playing, if so, stop it
if(MediaPlayer.State == MediaState.Playing)
{
    MediaPlayer.Stop();
}

// Start playing the background music
MediaPlayer.Play(song);