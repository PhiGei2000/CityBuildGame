#version 450
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoord;

out vec3 normal;
out vec2 texCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main() {
    normal = aNormal;
    texCoord = aTexCoord;

    gl_Position = projection * view * model * vec4(aPosition, 1.0f);
}