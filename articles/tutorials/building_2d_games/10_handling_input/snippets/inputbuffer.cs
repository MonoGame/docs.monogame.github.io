// Use a queue directly for input buffering
private Queue<Vector2> _inputBuffer;
private const int MAX_BUFFER_SIZE = 2;

// In initialization code:
_inputBuffer = new Queue<Vector2>(MAX_BUFFER_SIZE);

// In the input handling code:
KeyboardState keyboard = Keyboard.GetState();
Vector2 newDirection = Vector2.Zero;

if(keyboard.IsKeyDown(Keys.Up))
{
    newDirection = -Vector2.UnitY;
}
else if(keyboard.IsKeyDown(Keys.Down))
{
    newDirection = Vector2.UnitY;
}
else if(keyboard.IsKeyDown(Keys.Left))
{
    newDirection = -Vector2.UnitX;
}
else if(keyboard.IsKeyDown(Keys.Right))
{
    newDirection = Vector2.UnitX;
}

// Only add if a valid direction and does not exceed the buffer size.
if(newDirection != Vector2.Zero && _inputBuffer.Count < MAX_BUFFER_SIZE)
{
    _inputBuffer.Enqueue(newDirection);
}

// In movement update code.
if(_inputBuffer.COunt > 0)
{
    Vector2 nextDirection = _inputBuffer.Dequeue();
    _position += nextDirection * _speed;
}