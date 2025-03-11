// Render the left and right paddles first.
// This reduces the number of texture swaps needed from two to one.
_spriteBatch.Draw(paddleTexture, _leftPaddlePosition, Color.White);
_spriteBatch.Draw(paddleTexture, _rightPaddlePosition, Color.White);
_spriteBatch.Draw(ballTexture, _ballPosition, Color.White);