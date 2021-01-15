#version 400
in vec3 normal;
in vec2 texCoord;

out vec4 frag_color;

uniform sampler2D diffuse;

void main() {
    frag_color = texture(diffuse, texCoord);
}