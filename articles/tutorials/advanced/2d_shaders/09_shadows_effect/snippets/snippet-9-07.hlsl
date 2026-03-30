int id = input.TexCoord.x + input.TexCoord.y * 2;
if (id == 0) {        // S --> A
	pos = A;
} else if (id == 1) { // D --> a
	pos = a;
} else if (id == 3) { // F --> b
	pos = b;
} else if (id == 2) { // G --> B
	pos = B;
}
