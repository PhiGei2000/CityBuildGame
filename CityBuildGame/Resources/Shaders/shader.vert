#version 400
layout (location = 0) in vec3 a_position;
layout (location = 1) in vec3 a_normal;
layout (location = 2) in vec2 texureCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec2 textureCoords;

void main() {
    gl_Position = projection * view * model * vec4(a_position, 1.0);
}