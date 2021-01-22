#version 450
in vec2 texCoords;

out vec4 frag_color;

uniform sampler2D quadTexture;

void main() {
    frag_color = texture2D(quadTexture, texCoords);
}