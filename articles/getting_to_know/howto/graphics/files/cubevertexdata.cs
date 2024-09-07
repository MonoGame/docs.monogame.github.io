        Vector3 LeftTopFront = new Vector3(-1.0f, 1.0f, 1.0f);
        Vector3 LeftBottomFront = new Vector3(-1.0f, -1.0f, 1.0f);
        Vector3 LeftTopBack = new Vector3(-1.0f, 1.0f, -1.0f);
        Vector3 LeftBottomBack = new Vector3(-1.0f, -1.0f, -1.0f);

        Vector3 RightTopFront = new Vector3(1.0f, 1.0f, 1.0f);
        Vector3 RightBottomFront = new Vector3(1.0f, -1.0f, 1.0f);
        Vector3 RightTopBack = new Vector3(1.0f, 1.0f, -1.0f);
        Vector3 RightBottomBack = new Vector3(1.0f, -1.0f, -1.0f);

        Vector2 textureLeftTop = new Vector2(0.0f, 0.0f);
        Vector2 textureLeftBottom = new Vector2(0.0f, 1.0f);
        Vector2 textureRightTop = new Vector2(1.0f, 0.0f);
        Vector2 textureRightBottom = new Vector2(1.0f, 1.0f);

        // Front face.
        cubeVertices = new CustomVertex1[36];
        cubeVertices[0] = new CustomVertex1(LeftTopFront, textureLeftTop);
        cubeVertices[1] = new CustomVertex1(LeftBottomFront, textureLeftBottom);
        cubeVertices[2] = new CustomVertex1(RightTopFront, textureRightTop);
        cubeVertices[3] = new CustomVertex1(LeftBottomFront, textureLeftBottom);
        cubeVertices[4] = new CustomVertex1(RightBottomFront, textureRightBottom);
        cubeVertices[5] = new CustomVertex1(RightTopFront, textureRightTop);

        // Add the vertices for the BACK face.
        cubeVertices[6] = new CustomVertex1(LeftTopBack, textureRightTop);
        cubeVertices[7] = new CustomVertex1(RightTopBack, textureLeftTop);
        cubeVertices[8] = new CustomVertex1(LeftBottomBack, textureRightBottom);
        cubeVertices[9] = new CustomVertex1(LeftBottomBack, textureRightBottom);
        cubeVertices[10] = new CustomVertex1(RightTopBack, textureLeftTop);
        cubeVertices[11] = new CustomVertex1(RightBottomBack, textureLeftBottom);

        // Add the vertices for the TOP face.
        cubeVertices[12] = new CustomVertex1(LeftTopFront, textureLeftBottom);
        cubeVertices[13] = new CustomVertex1(RightTopBack, textureRightTop);
        cubeVertices[14] = new CustomVertex1(LeftTopBack, textureLeftTop);
        cubeVertices[15] = new CustomVertex1(LeftTopFront, textureLeftBottom);
        cubeVertices[16] = new CustomVertex1(RightTopFront, textureRightBottom);
        cubeVertices[17] = new CustomVertex1(RightTopBack, textureRightTop);

        // Add the vertices for the BOTTOM face. 
        cubeVertices[18] = new CustomVertex1(LeftBottomFront, textureLeftTop);
        cubeVertices[19] = new CustomVertex1(LeftBottomBack, textureLeftBottom);
        cubeVertices[20] = new CustomVertex1(RightBottomBack, textureRightBottom);
        cubeVertices[21] = new CustomVertex1(LeftBottomFront, textureLeftTop);
        cubeVertices[22] = new CustomVertex1(RightBottomBack, textureRightBottom);
        cubeVertices[23] = new CustomVertex1(RightBottomFront, textureRightTop);

        // Add the vertices for the LEFT face.
        cubeVertices[24] = new CustomVertex1(LeftTopFront, textureRightTop);
        cubeVertices[25] = new CustomVertex1(LeftBottomBack, textureLeftBottom);
        cubeVertices[26] = new CustomVertex1(LeftBottomFront, textureRightBottom);
        cubeVertices[27] = new CustomVertex1(LeftTopBack, textureLeftTop);
        cubeVertices[28] = new CustomVertex1(LeftBottomBack, textureLeftBottom);
        cubeVertices[29] = new CustomVertex1(LeftTopFront, textureRightTop);

        // Add the vertices for the RIGHT face. 
        cubeVertices[30] = new CustomVertex1(RightTopFront, textureLeftTop);
        cubeVertices[31] = new CustomVertex1(RightBottomFront, textureLeftBottom);
        cubeVertices[32] = new CustomVertex1(RightBottomBack, textureRightBottom);
        cubeVertices[33] = new CustomVertex1(RightTopBack, textureRightTop);
        cubeVertices[34] = new CustomVertex1(RightTopFront, textureLeftTop);
        cubeVertices[35] = new CustomVertex1(RightBottomBack, textureRightBottom);