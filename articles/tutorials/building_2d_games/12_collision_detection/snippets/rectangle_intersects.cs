// Rectangle 1
//                Top: 0
//          ----------------
//         |                |
//         |                |
// Left: 0 |                |  Right: 32
//         |                |
//         |                |
//          ----------------
//              Bottom: 32
Rectangle rect1 = new Rectangle(0, 0, 32, 32);

// Rectangle 2
//                Top: 16
//           ----------------
//          |                |
//          |                |
// Left: 16 |                |  Right: 48
//          |                |
//          |                |
//           ----------------
//              Bottom: 48
Rectangle rect2 = new Rectangle (16, 16, 32, 32);

// rect1.Left (0)  < rect2.Right (48) = true
// rect1.Right (32) > rect3.Left (16) = true
// rect1.Top (0) < rect2.Bottom (48) = true
// rect1.Bottom (32) > rect2.Top (16) = true
bool isColliding = rect1.Intersects(rect2); // returns true