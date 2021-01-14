#version 400
in struct {
    vec3 normal;
    vec2 texCoord;
} FragmentShaderIn;

out vec4 frag_color;

void main() {
    frag_color = vec4(0.06f, 0.7f, 0.0f, 1.0f);
}