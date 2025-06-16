Vector2 circle1Position = new Vector2(8, 10);
Vector2 circle2Position = new Vector2(5, 6);

float circle1Radius = 5;
float circle2Radius = 5;

// c^2 = a^2 + b^2
// c^2 = (8 - 5)^2 + (10 - 6)^2
// c^2 = 3^2 + 4^2
// c^2 = 9 + 16
// c^2 = 25
float distanceSquared = Vector2.DistanceSquared(circle1Position, circle2Position);

// r^2 = (5 + 5)^2
// r^2 = (10)^2
// r^2 = 100
int radiiSquared = (circle1Radius + circle2Radius) * (circle1Radius + circle2Radius);

// The circles overlap because 100 is greater than 25.
if(radiiSquared > distanceSquared)
{
    
}