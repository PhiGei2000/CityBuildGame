#version 450
out vec4 frag_color;

uniform vec4 backgroundColor;

void main() {
    frag_color = backgroundColor;
}
