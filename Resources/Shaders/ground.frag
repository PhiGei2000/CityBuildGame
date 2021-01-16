#version 450
in vec3 normal;
in vec2 texCoord;

out vec4 frag_color;

void main() {
    frag_color = vec4(0.06f, 0.7f, 0.0f, 1.0f);
}