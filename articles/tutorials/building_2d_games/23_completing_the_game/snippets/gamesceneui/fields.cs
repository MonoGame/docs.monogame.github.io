// The string format to use when updating the text for the score display.
private static readonly string s_scoreFormat = "SCORE: {0:D6}";

// The sound effect to play for auditory feedback of the user interface.
private SoundEffect _uiSoundEffect;

// The pause panel
private Panel _pausePanel;

// The resume button on the pause panel. Field is used to track reference so
// focus can be set when the pause panel is shown.
private AnimatedButton _resumeButton;

// The game over panel.
private Panel _gameOverPanel;

// The retry button on the game over panel. Field is used to track reference
// so focus can be set when the game over panel is shown.
private AnimatedButton _retryButton;

// The text runtime used to display the players score on the game screen.
private TextRuntime _scoreText;
